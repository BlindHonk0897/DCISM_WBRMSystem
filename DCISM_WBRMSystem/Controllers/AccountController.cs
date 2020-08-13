using DCISM_WBRMSystem.Models;
using DCISM_WBRMSystem.Models.CustomModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DCISM_WBRMSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
      
        [HttpPost]
        public ActionResult Login(Account model, string returnUrl)
        {
            //var userName = Request.Form["user_input_username"];
            //var userPass = Request.Form["user_input_pass"];
            PasswordSecurity passwordSecurity = new PasswordSecurity();
            if(model.Username != null && model.Password != null)
            {
                using(WBRMSystemEntities database =  new WBRMSystemEntities())
                {
                    var EncryptedPass = passwordSecurity.Encryptdata(model.Password);
                    Debug.WriteLine(EncryptedPass);
                    var userDetails = database.User_Details_vw.FirstOrDefault(u => (u.Username == model.Username || u.ID_Number == model.Username) && u.Password == EncryptedPass);
                    if (userDetails != null)
                    {
                        if (userDetails.Status == "Active" || userDetails.Status == "") {
                            Session["UserRole"] = userDetails.ID_Role;
                            Session["UserTitleRole"] = userDetails.Role_Name;
                            Session["Username"] = userDetails.Username;
                            Session["IDNumber"] = userDetails.ID_Number;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Failed. You are currently "+userDetails.Status+".Try to approach the administrator.");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Credential!");
                        return View();
                    }

                    //Session["UserRole"] = 1;
                    //Session["UserTitleRole"] = "Administrator";
                    //Session["Username"] = "Dan";
                    //return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Credential!");
                return RedirectToAction("Login","Account");
            }
            
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login","Account");
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpCredential()
        {
            return RedirectToAction("CodeConfirmation", "Account");
        }

        public ActionResult CodeConfirmation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendOTP(string code,string number)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            var status = "";
            try
            {

                var receipent = "63"+ number.Remove(0, 1);
                string message = "Your One-Time Password (OTP) is:" + code + " ( from: DCSIM_WBRMSystem )\n\nNote: Don't share this to anyone.Reply is not allowed.";
                string encodedMessage = HttpUtility.UrlEncode(message);

                // Using GoogleSheet & MIT App -> android
                var APIBaseURL = System.Configuration.ConfigurationManager.AppSettings["googleSheetScript"];
                using (var webclient = new WebClient())
                {
                    byte[] response = webclient.UploadValues(APIBaseURL, new NameValueCollection()
                    {
                        {"Phone",receipent },
                        {"Status","" },
                        {"Message",message },
                        {"action","add" }
                    });

                    string result = System.Text.Encoding.UTF8.GetString(response);
                    var jsonObject = JObject.Parse(result);
                    status = jsonObject["result"].ToString();
                    ajm.Message = status;
                }

                // Clickatell API -> uncomment lines below to use Clickatell API Gateway
                /*
                    var APIKey = System.Configuration.ConfigurationManager.AppSettings["clickatellKey"];
                    var APIBaseURL = System.Configuration.ConfigurationManager.AppSettings["clickatellURL"];

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
                            ajm.Message = "success";
                        }
                    }
                */
                // END Clickatell API

                // textlocal API -> uncomment lines below to use Textlocal API Gateway
                /* 
                    var APIKey = System.Configuration.ConfigurationManager.AppSettings["textlocalKey"];
                    var APIBaseURL = System.Configuration.ConfigurationManager.AppSettings["textlocalURL"];

                    using (var webclient = new WebClient())
                    {
                        byte[] response = webclient.UploadValues(APIBaseURL, new NameValueCollection()
                        {
                            {"apikey",APIKey },
                            {"numbers",receipent },
                            {"message",encodedMessage },
                            {"sender","DCSIM WBRMS" }
                        });

                        string result = System.Text.Encoding.UTF8.GetString(response);
                        var jsonObject = JObject.Parse(result);
                        status = jsonObject["status"].ToString();
                        ajm.Message = status;
                    }
                */
                // END Textlocal API

                return Json(ajm);
            }
            catch (Exception ex)
            {
                ajm.Message = ex.Message;
            }
            return Json(ajm);
        }


        [HttpPost]
        public JsonResult AjaxSignUpsave(SignUp signupmodel)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "Failed"
            };
            CodeGenerator codeGenerator = new CodeGenerator();
            PasswordSecurity passwordSecurity = new PasswordSecurity();
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var tempSign = database.Temp_Signup.FirstOrDefault(t => t.ID_Number == signupmodel.IdNumber);

                if (tempSign != null)
                {
                    database.Temp_Signup.Remove(tempSign);
                    database.SaveChanges();
                }
                Temp_Signup temp = new Temp_Signup
                {
                    First_Name = signupmodel.FirstName,
                    ID_Number = signupmodel.IdNumber,
                    Last_Name = signupmodel.LastName,
                    Middle_Name = signupmodel.MiddleName,
                    Email_Add = signupmodel.Email,
                    CourseAndYear = signupmodel.CourseYr,
                    Password = passwordSecurity.Encryptdata(signupmodel.Password),
                    ConfirmPassword = passwordSecurity.Encryptdata(signupmodel.ConfirmPassword),
                    Contact_No = signupmodel.Mobile,
                    UserType = signupmodel.Usertype,
                    Code_Generated = codeGenerator.Generate_Four_Digit_Random().ToString(),
                    Signup_Date = DateTime.Now.ToShortDateString()
                };
                Session["current_code"] = temp.Code_Generated;
                database.Temp_Signup.Add(temp);
                database.SaveChanges();
                ajm.Message = "Success";

            }
            return Json(ajm);
        }

        [HttpPost]
        public JsonResult CheckIsNumberExist(string num)
        {
            num = num.PadLeft(11, '0');
            AjaxMessage ajm = new AjaxMessage();
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                ajm.Message = database.Temp_Signup.FirstOrDefault(t => t.Contact_No == num) != null ? "Yes" : "No";
                if (ajm.Message == "No")
                {
                    ajm.Message = database.User_Details_vw.FirstOrDefault(u => u.Contact_No == num) != null ? "Yes" : "No";
                }
            }
            return Json(ajm);
        }

        [HttpPost]
        public JsonResult CheckIsIdNumberExist(string idnumber)
        {         
            AjaxMessage ajm = new AjaxMessage();
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                ajm.Message = database.Temp_Signup.FirstOrDefault(t => t.ID_Number == idnumber) != null ? "Yes" : "No";
                if (ajm.Message == "No")
                {
                    ajm.Message = database.User_Details_vw.FirstOrDefault(u => u.ID_Number == idnumber) != null ? "Yes" : "No";
                }
            }
            return Json(ajm);
        }

        [HttpPost]
        public JsonResult IsEmailExist(string email)
        {
            AjaxMessage ajm = new AjaxMessage();
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                ajm.Message = database.Temp_Signup.FirstOrDefault(t => t.Email_Add == email) != null ? "Yes" : "No";
                if (ajm.Message == "No")
                {
                    ajm.Message = database.User_Details_vw.FirstOrDefault(u => u.Email_Add == email) != null ? "Yes" : "No";
                }
            }
            return Json(ajm);
        }

        public ActionResult ChangePass()
        {
            return View();
        }

        public ActionResult ProfileInfo()
        {
            return View();
        }

    }


}