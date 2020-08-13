using DCISM_WBRMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using DCISM_WBRMSystem.Models.CustomModels;
using Microsoft.AspNet.SignalR;
using DCISM_WBRMSystem.Hubs;
using System.Data.Entity;
using MoreLinq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace DCISM_WBRMSystem.Controllers
{
    public class BorrowingController : Controller
    {
        // GET: Borrow
        public ActionResult AdminPage()
        {
            List<BorrowPendingRequest> list = new List<BorrowPendingRequest>();

            for(int i =0; i < 100; i++)
            {
                list.Add(new BorrowPendingRequest() { id = i, Requestor = "Dan", DateRequested = "12/02/2019", Purpose = "Study Purposes" });
            }

            return View();
        }

        public ActionResult UserPage()
        {
            return View();
        }

        public ActionResult Index()
        {
            RouterModel model = new RouterModel();
            if (Session["UserTitleRole"] != null)
            {
                if (Session["UserTitleRole"].ToString() == "Administrator")
                {
                    model._Partial = "_AdminPage";
                }
                else if (Session["UserTitleRole"].ToString() == "Teacher" || Session["UserTitleRole"].ToString() == "Student")
                {
                    model._Partial = "_UserPage";
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult AjaxSendRequest(BorrowRequest model)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            Randomizer randomizer = new Randomizer();
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                // first add generate secret code transaction => idnumber+username+datenow+ramdomnumber

                var username = Session["Username"] != null ? Session["Username"].ToString() : "";
                var Idnumber = Session["IDNumber"] != null ? Session["IDNumber"].ToString() : "";
                var DateNow = DateTime.Now;
                var RandomNum = (randomizer.Generate_Four_Digit_Random() + randomizer.Generate_Four_Digit_Random()).ToString();
                var Secret_Code = Idnumber + username + DateNow.ToString() + RandomNum;

                Transaction trans = new Transaction()
                {
                    ID_Number = Idnumber,
                    Transaction_Date = DateNow,
                    Transaction_Type = "Borrowing",
                    Secret_Code = Secret_Code
                };
                database.Transactions.Add(trans);
                database.SaveChanges();

                // then add Cart using secret code 
                Cart cart = new Cart()
                {
                    Id_Number = Idnumber,
                    Requestor_Name = username,
                    Secret_Code = Secret_Code
                };

                database.Carts.Add(cart);
                database.SaveChanges();
                // then add Request => query cart id using secret code 
                var tq = database.Transactions.FirstOrDefault(c => c.Secret_Code == Secret_Code);
                var crt = database.Carts.FirstOrDefault(c => c.Secret_Code == Secret_Code);
                if (crt != null && tq != null)
                {
                    Request req = new Request()
                    {
                        Id_Transaction = tq.Id_Transaction,
                        Id_Cart = crt.Id_Cart,
                        Date_Want_To_Receive = model.Date_Want_To_Receive,
                        Date_Expected_Return = model.Date_Expected_Return,
                        Purpose = model.Purpose,
                        Status = "Open"
                    };
                    database.Requests.Add(req);
                    database.SaveChanges();

                    // then add cart item using cart id
                    foreach(var el in model.Items_Request)
                    {
                        for (int i = 0; i < el.Qty; i++) {
                            Cart_Item citem = new Cart_Item()
                            {
                                Id_Cart = crt.Id_Cart,
                                Item_Name = el.Item_Name,
                                Is_Returned = false,
                                Status = "Pending"

                            };
                            database.Cart_Item.Add(citem);
                            database.SaveChanges();
                        }
                    }
                    ajm.Message = "success";
                }
                
            }
            return Json(ajm);
        }


        public JsonResult AjaxCancelRequest(string idr,string idc)
        {
            //var idRequest = ids.Split('*')[0];
            //var idCart = ids.Split('*')[1];
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var IdR = Convert.ToInt32(idr);
                var IdC = Convert.ToInt32(idc);
                var cartItems = database.Cart_Item.Where(c => c.Id_Cart == IdC).ToList();
                var AllPending = true;
                if (cartItems!=null)
                {
                    foreach(var el in cartItems)
                    {
                        if (el.Status != "Pending")
                        {
                            AllPending = false;
                            break;
                        }
                    }

                }
                if (AllPending)
                {
                    database.Cancel_Request_sp(IdR,IdC);
                    ajm.Message = "success";
                }
                else
                {
                    ajm.Message = "This request is already approved.You cannot cancelled this";
                }
            }
            return Json(ajm);
        }

        [HttpPost]
        public JsonResult AjaxApproveItem(string Id ,string cartid,string requestid)
        {
            //var idRequest = ids.Split('*')[0];
            //var idCart = ids.Split('*')[1];
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var ID = Convert.ToInt32(Id);
                var IdCart = Convert.ToInt32(cartid);
                var IdRequest = Convert.ToInt32(requestid);
                var cartItem = database.Cart_Item.FirstOrDefault(c => c.Id == ID);
           
                if (cartItem != null)
                {
                    cartItem.Status = "Approved";
                    cartItem.Date_Approved = DateTime.Now;
                    database.Entry(cartItem).State = EntityState.Modified;
                    database.SaveChanges();
                    ajm.Message = "success";
                }

                database.Update_Request_sp(IdRequest);

            }
            return Json(ajm);
        }

        [HttpPost]
        public JsonResult AjaxDeclineItem(string Id, string cartid, string requestid,string remarks)
        {
            //var idRequest = ids.Split('*')[0];
            //var idCart = ids.Split('*')[1];
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var ID = Convert.ToInt32(Id);
                var IdCart = Convert.ToInt32(cartid);
                var IdRequest = Convert.ToInt32(requestid);
                var cartItem = database.Cart_Item.FirstOrDefault(c => c.Id == ID);

                if (cartItem != null)
                {
                    cartItem.Status = "Declined";
                    cartItem.Date_Declined = DateTime.Now;
                    cartItem.Remarks_for_Declined = remarks;
                    database.Entry(cartItem).State = EntityState.Modified;
                    database.SaveChanges();
                    ajm.Message = "success";
                }

                database.Update_Request_sp(IdRequest);
            }
            return Json(ajm);
        }

        public JsonResult AjaxReleaseItem(string idcart,string strcode, string conditionb4release, string remarks4release,string releasedto, string expdateofreturn , string requestid)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                
                var DateNow = DateTime.Now;

                // get item details
                var It = database.Items.FirstOrDefault(i => i.Str_Code == strcode);
                if (It != null) {

                    // add history first
                    Item_History his = new Item_History();
                    his.Id_Item = It.Id_Item;
                    his.Str_Code = It.Str_Code;
                    his.Action = "Release";
                    his.Action_Date = DateNow;
                    his.Action_History_Desc = "Item is released";
                    his.Done_By = Session["Username"] != null ? Session["Username"].ToString() : "";
                    his.Released_Date = DateNow;
                    his.Released_To = releasedto;
                    his.Remarks_Before_Released = remarks4release;
                    his.Condition_Before_Released = conditionb4release;
                    his.Expected_Date_Of_Return = Convert.ToDateTime(expdateofreturn);
                    database.Item_History.Add(his);
                    database.SaveChanges();

                    // update cart item - idItem and StrCode field
                    var Id = Convert.ToInt32(idcart);
                    var carItem = database.Cart_Item.FirstOrDefault(ci => ci.Id == Id);
                    if (carItem != null)
                    {
                        carItem.Id_Item = It.Id_Item;
                        carItem.Str_Code = It.Str_Code;
                        carItem.Codition_Before_Released = conditionb4release;
                        carItem.Remarks_for_Released = remarks4release;
                        carItem.Date_Released = DateNow;
                        carItem.Status = "Released";
                        database.Entry(carItem).State = EntityState.Modified;
                        database.SaveChanges();

                        var IdRequest = Convert.ToInt32(requestid);

                        database.Update_Request_sp(IdRequest);
                    }

                    // Update Item - Status -> borrowed
                    It.Status = "Borrowed";
                    database.Entry(It).State = EntityState.Modified;
                    database.SaveChanges();

                    ajm.Message = "success";
                    
                }
               
            }
            return Json(ajm);
        }


        public JsonResult AjaxDiscardItem(string id, string requestid)
        {
            //var idRequest = ids.Split('*')[0];
            //var idCart = ids.Split('*')[1];
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };


            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                
                var IdC = Convert.ToInt32(id);
                var cartItem = database.Cart_Item.FirstOrDefault(c => c.Id == IdC);
                
                if (cartItem!= null)
                {
                    cartItem.Status = "Discard";
                    database.Entry(cartItem).State = EntityState.Modified;
                    database.SaveChanges();
                    ajm.Message = "success";

                    var IdRequest = Convert.ToInt32(requestid);

                    database.Update_Request_sp(IdRequest);
                }
              
            }
            return Json(ajm);
        }


        public JsonResult AjaxReturnItem(string id, string itemreturnC, string itemreturnR , string requestid)
        {
            //var idRequest = ids.Split('*')[0];
            //var idCart = ids.Split('*')[1];
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };


            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {

                var IdC = Convert.ToInt32(id);
                var datenow = DateTime.Now;
                var cartItem = database.Cart_Item.FirstOrDefault(c => c.Id == IdC);

                if (cartItem != null)
                {

                    // add history first
                    Item_History his = new Item_History();
                    his.Id_Item = cartItem.Id_Item;
                    his.Str_Code = cartItem.Str_Code;
                    his.Action = "Return";
                    his.Action_Date = datenow;
                    his.Action_History_Desc = "Item is returned";
                    his.Returned_Date = datenow.ToShortDateString();
                    his.Remarks_After_Returned = itemreturnR;
                    his.Condition_After_Returned = itemreturnC;
                    database.Item_History.Add(his);
                    database.SaveChanges();

                    cartItem.Status = "Returned";
                    cartItem.Is_Returned = true;
                    cartItem.Date_Returned = datenow;
                    cartItem.Remarks_for_Retrun = itemreturnR;
                    cartItem.Codition_After_Returned = itemreturnC;
                    database.Entry(cartItem).State = EntityState.Modified;
                    database.SaveChanges();
                    

                    var IdRequest = Convert.ToInt32(requestid);

                    database.Update_Request_sp(IdRequest);

                    var ite = database.Items.FirstOrDefault(i => i.Str_Code == cartItem.Str_Code);

                    if(ite != null)
                    {
                        ite.Status = "Available";
                        ite.Condition = itemreturnC;
                        ite.Remarks = itemreturnR;
                        database.Entry(ite).State = EntityState.Modified;
                        database.SaveChanges();
                       
                    }
                    ajm.Message = "success";
                }

            }
            return Json(ajm);
        }

        [HttpPost]
        public JsonResult SendSMSNotification(string idr)
        {
            //var idRequest = ids.Split('*')[0];
            //var idCart = ids.Split('*')[1];
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };


            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var IdR = Convert.ToInt32(idr);
                var request_Details = database.Request_vw.FirstOrDefault(r => r.Id_Request == IdR);
                if (request_Details != null)
                {
                    var userDetails = database.User_Details_vw.FirstOrDefault(u => u.ID_Number == request_Details.Id_Number);
                    if (userDetails != null)
                    {
                        string message = "Hi. Good Day. This is notification for your borrowing request:";
                        if (request_Details.Status.Contains("Approved"))
                        {
                            message += "\n Request ID: " + request_Details.Id_Request + ", Status: " + request_Details.Status;
                            message += "\n You may now claim your requested item/s to the admin or in the control room ";
                            message += "\n from: DCSIM_WBRMSystem";
                        }
                        else
                        {
                            message += "\n Request ID: " + request_Details.Id_Request + ", Status: " + request_Details.Status;
                            message += "\n from: DCSIM_WBRMSystem";
                        }
                        ajm.Message = SendSMSToUser(message, userDetails.Contact_No);
                    }
                }
            }
            return Json(ajm);
        }


        public string SendSMSToUser(string message, string number)
        {
          
            var status = "failed";
            try
            {

                var receipent = "63" + number.Remove(0, 1);
                // clickatell API
                var APIKey = System.Configuration.ConfigurationManager.AppSettings["clickatellKey"];
                var APIBaseURL = System.Configuration.ConfigurationManager.AppSettings["clickatellURL"];

                // textlocal API
                // var APIKey = System.Configuration.ConfigurationManager.AppSettings["textlocalKey"];
                // var APIBaseURL = System.Configuration.ConfigurationManager.AppSettings["textlocalURL"];
                
                string encodedMessage = HttpUtility.UrlEncode(message);

                // for clickatell API Only ------------------
                using (var webclient = new WebClient())
                {
                    webclient.QueryString.Add("apiKey", APIKey);
                    webclient.QueryString.Add("to", receipent);
                    webclient.QueryString.Add("content", encodedMessage);
                    byte[] response = webclient.DownloadData(APIBaseURL);

                    string result = System.Text.Encoding.UTF8.GetString(response);
                    var jsonObject = JObject.Parse(result);
                    var accepted = jsonObject["messages"][0]["accepted"].ToString();
                    if (accepted == "True")
                    {
                        status = "success";
                    }

                }

                // for textlocal API Only ------------------

                //string message = "Your OTP password is:" + code + " ( sent by: DCSIM_WBRMSystem )";
                //string encodedMessage = HttpUtility.UrlEncode(message);
                //using ( var webclient = new WebClient())
                //{
                //    byte[] response = webclient.UploadValues(APIBaseURL, new NameValueCollection()
                //    {
                //        {"apikey",APIKey },
                //        {"numbers",receipent },
                //        {"message",encodedMessage },
                //        {"sender","DCSIM WBRMS" }
                //    });

                //    string result = System.Text.Encoding.UTF8.GetString(response);
                //    var jsonObject = JObject.Parse(result);
                //    status = jsonObject["status"].ToString();
                //    ajm.Message = status;
                //}
                return status;
            }
            catch (Exception ex)
            {
                return status;
            }
        }
    }
}