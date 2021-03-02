using DCISM_WBRMSystem.ChartsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DCISM_WBRMSystem.Models;
using DCISM_WBRMSystem.Models.CustomModels;
using MoreLinq;
using System.Data.Entity;

namespace DCISM_WBRMSystem.Controllers.Api
{
    [RoutePrefix("api/Common")]
    public class CommonController : ApiController
    {
        [HttpGet, Route("ChartPoints")]
        public IEnumerable<DataPoint> GetCharts()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint("LB 400", 2500));
            dataPoints.Add(new DataPoint("LB 442", 2790));
            dataPoints.Add(new DataPoint("LB 443", 3380));
            dataPoints.Add(new DataPoint("LB 445", 4940));
            dataPoints.Add(new DataPoint("LB 446", 4020));
            dataPoints.Add(new DataPoint("LB 447", 3390));
            dataPoints.Add(new DataPoint("LB 448", 4200));
            dataPoints.Add(new DataPoint("LC 466", 3150));
            dataPoints.Add(new DataPoint("LB 467", 3230));
            dataPoints.Add(new DataPoint("LB 468", 4200));
            dataPoints.Add(new DataPoint("Control Room", 5210));
            dataPoints.Add(new DataPoint("Functional Unit", 4940));
            //ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            /*
              <option value="400">LB 400</option>
                    <option value="442">LB 442</option>
                    <option value="443">LB 443</option>
                    <option value="445">LB 445</option>
                    <option value="446">LB 446</option>
                    <option value="447">LB 447</option>
                    <option value="448">LB 448</option>
                    <option value="466">LC 466</option>
                    <option value="467">LB 467</option>
                    <option value="468">LB 468</option>

             */
            return dataPoints;
        }


        // ----------------------- ------------ Items API ----------- ----------------------------//
        [HttpGet, Route("Items")]
        public IEnumerable<item_vw> GetAllItems()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.item_vw.OrderByDescending( i=> i.Id_Item).ToList();
            }

        }

        [HttpGet, Route("Items/category/{category}/assign/{assign}/condition{condition}/status{status}")]
        public IEnumerable<item_vw> GetFilteredItems(string category,string assign ,string condition , string status)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var items = database.item_vw.OrderByDescending(i => i.Id_Item).ToList();

                if (category != "X")
                {
                    items = items.Where(i => i.Category == category.ToUpper()).ToList();
                }
                if(assign != "X" && assign != "Not yet assigned")
                {
                    items = items.Where(i => i.Room_No.Contains(assign)).ToList();
                }else if(assign == "Not yet assigned")
                {
                    items = items.Where(i => i.Is_Assign == false || i.Is_Assign == null).ToList();
                }
                if(condition != "X")
                {
                    items = items.Where(i => i.Condition.Contains(condition)).ToList();
                }
                if(status != "X")
                {
                    items = items.Where(i => i.Status.Contains(status)).ToList();
                }
                return items;
            }

        }

        [HttpGet, Route("AddedToday")]
        public IEnumerable<added_today_vw> GetAddedToday()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var today = DateTime.Now.ToShortDateString();
                return database.added_today_vw.OrderByDescending(i => i.Id_Item).ToList();
            }

        }

        [HttpGet, Route("Items/Recent")]
        public IEnumerable<Item> GetRecents()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Items.OrderByDescending(i => i.Id_Item).ToList().Take(10);
            }

        }

        [HttpGet, Route("Conditions")]
        public IEnumerable<Condition> Conditions()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Conditions.ToList();
            }

        }

        [HttpGet, Route("Facilities")]
        public IEnumerable<Room> GetFacilites()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Rooms.ToList();
            }
        }

        [HttpGet, Route("GetItemNames")]
        public IEnumerable<Item_Name_vw> GetItemNames()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Item_Name_vw.DistinctBy( i => i.Item_Name).ToList();
            }
        }

        [HttpGet, Route("GetRequest/idnumber/{idnumber}/username/{username}")]
        public IEnumerable<Request_vw> GetRequest(string idnumber,string username)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Request_vw.Where(r => r.Id_Number == idnumber && r.Requestor_Name == username).ToList();
            }
        }

        [HttpGet, Route("GetOpenRequest/idnumber/{idnumber}/username/{username}")]
        public IEnumerable<Request_vw> GetOpenRequest(string idnumber, string username)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Request_vw.Where(r => r.Id_Number == idnumber && r.Requestor_Name == username && r.Status =="Open").ToList();
            }
        }

        [HttpGet, Route("GetAllOpenRequest")]
        public IEnumerable<Request_vw> GetAllOpenRequest()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Request_vw.Where(r =>  r.Status == "Open").ToList();
            }
        }

        [HttpGet, Route("GetForReleaseRequest")]
        public IEnumerable<Request_vw> GetForReleaseRequest(string key)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var userD = database.Users.FirstOrDefault(u => u.RF == key || u.ID_Number == key);
                if (userD != null)
                {
                    return database.Request_vw.Where(r => r.Id_Number == userD.ID_Number && r.Status.Contains("Approved")).ToList();
                }
                else
                {
                    try
                    {
                        var IdRequest = Convert.ToInt32(key);
                        return database.Request_vw.Where(r => r.Id_Request == IdRequest && r.Status.Contains("Approved")).ToList();
                    }
                    catch(Exception ex) {
                        return null;
                    }
                    
                }
            }
        }


        [HttpGet, Route("GetForReturnRequest")]
        public IEnumerable<Request_vw> GetForReturnRequest(string key)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var userD = database.Users.FirstOrDefault(u => u.RF == key || u.ID_Number == key);
                if (userD != null)
                {
                    return database.Request_vw.Where(r => r.Id_Number == userD.ID_Number && r.Status.Contains("Released")).ToList();
                }
                else
                {
                    try
                    {
                        var IdRequest = Convert.ToInt32(key);
                        return database.Request_vw.Where(r => r.Id_Request == IdRequest && r.Status.Contains("Released")).ToList();
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }

                }
            }
        }


        [HttpGet, Route("GetAllActiveRequest/idnumber/{idnumber}/username/{username}")]
        public IEnumerable<Request_vw> GetAllActiveRequest(string idnumber, string username)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Request_vw.Where(r => r.Status != "Cancelled" && r.Status !="Closed" && r.Id_Number == idnumber).ToList();
            }
        }

        [HttpGet, Route("GetAllRequest")]
        public IEnumerable<Request_vw> GetAllRequest()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Request_vw.ToList();
            }
        }

        [HttpGet, Route("GetUserCode/id/{id}/num/{num}/pass/{pass}")]
        public Temp_Signup GetUserCode(string id, string num, string pass)
        {
            PasswordSecurity passwordSecurity = new PasswordSecurity();
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var EncryptedPass = passwordSecurity.Encryptdata(pass);
                return database.Temp_Signup.FirstOrDefault(p => p.ID_Number == id && p.Contact_No == num && p.Password == EncryptedPass);
            }
        }

        [HttpGet, Route("Rooms")]
        public IEnumerable<string> GetRooms()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                // need to get it directly to room table
                return database.Items.ToList().Select(i =>i.Room_No).Distinct();
            }
        }

        [HttpGet, Route("Categories")]
        public IEnumerable<ItemCategory> GetCategories()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {

                return database.ItemCategories.ToList().Distinct();
            }
        }


        [HttpGet, Route("GetItem/room/{room}/category/{category}")]
        public IEnumerable<Item> GetUserCode( string room, string category)
        {
           
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {

                return database.Items.Where(i => i.Room_No == room && i.Category == category).ToList();
            }
        }

        [HttpGet, Route("GetItem/id/{id}")]
        public item_vw GetOneItem(int id)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.item_vw.FirstOrDefault(i => i.Id_Item == id);
            }
        }

        [HttpGet, Route("GetItem/strcode/{strcode}")]
        public item_vw GetOneItem_ByStrCode(string strcode)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.item_vw.FirstOrDefault(i => i.Str_Code == strcode);
            }
        }

        [HttpGet, Route("GetUnverifiedItems")]
        public IEnumerable<item_vw> GetUnverifiedItems()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.item_vw.Where(i => i.Is_Verify == false || i.Is_Verify == null).ToList();
            }
        }

        [HttpGet, Route("GetUnAssignedItems")]
        public IEnumerable<item_vw> GetUnAssignedItems()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.item_vw.Where(i => i.Is_Assign == false || i.Is_Assign == null).ToList();
            }
        }

        [HttpGet, Route("GetAssignedItems")]
        public IEnumerable<item_vw> GetAssignedItems()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.item_vw.Where(i => i.Is_Assign == true).ToList();
            }
        }

        [HttpGet, Route("GetBorrowedItems")]
        public IEnumerable<borrowed_items_vw> GetBorrowedItems()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.borrowed_items_vw.ToList();
            }
        }

        [HttpGet, Route("GetDamagedItems")]
        public IEnumerable<damaged_items_vw> GetDamagedItems()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.damaged_items_vw.ToList();
            }
        }

        [HttpGet, Route("GetAvailableItems")]
        public IEnumerable<item_vw> GetAvailableItems()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.item_vw.Where(i => i.Status=="Available").ToList();
            }
        }

        [HttpGet, Route("GetCartItems/cartid{cartid}")]
        public IEnumerable<Cart_Items_vw> GetCartItems(int cartid)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Cart_Items_vw.Where(c => c.Id_Cart == cartid).ToList();
            }
        }

        [HttpGet, Route("GetRequestNotify")]
        public IEnumerable<Request_vw> GetRequestNotify()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.Request_vw.Where(c => c.Status == "Open").ToList();
            }
        }

        [HttpGet, Route("GetTempSingUpUser/code{code}")]
        public AjaxMessage GetTempSingUpUser(string code)
        {
            PasswordSecurity passwordSecurity = new PasswordSecurity();
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                AjaxMessage ajm = new AjaxMessage()
                {
                    Message = "failed"
                };
                var last = code.Remove(0, 4);
                var id = Convert.ToInt32(last);
                var code_generated = code.Substring(0, 4);
                var temp_signup = database.Temp_Signup.FirstOrDefault(c => c.Id == id && c.Code_Generated == code_generated);
                //database
                if (temp_signup != null)
                {
                    temp_signup.Is_Contact_No_Confirm = true;
                    database.Entry(temp_signup).State = EntityState.Modified;
                    var userRole = 3;

                    if(temp_signup.UserType == "Student")
                    {
                        // add to student table
                        Student student = new Student() 
                        {
                            ID_Number = temp_signup.ID_Number,
                            First_Name = temp_signup.First_Name,
                            Last_Name = temp_signup.Last_Name,
                            Middle_Name = temp_signup.Middle_Name,
                            CourseAndYear = temp_signup.CourseAndYear,
                            Email_Add = temp_signup.Email_Add,
                            Contact_No = temp_signup.Contact_No,
                            Address = temp_signup.Address
                        };
                        userRole = 4;
                        database.Students.Add(student);
                        database.SaveChanges();
                    }
                    else
                    {
                        //add to faculty table
                        Faculty faculty = new Faculty()
                        {
                            ID_Number = temp_signup.ID_Number,
                            First_Name = temp_signup.First_Name,
                            Last_Name = temp_signup.Last_Name,
                            Middle_Name = temp_signup.Middle_Name,
                            Email_Add = temp_signup.Email_Add,
                            Contact_No = temp_signup.Contact_No,
                            Address = temp_signup.Address
                        };
                        userRole = 3;
                        database.Faculties.Add(faculty);
                        database.SaveChanges();
                    }

                    // add to user table
                    User user = new User()
                    {
                        ID_Number = temp_signup.ID_Number,
                        Username = temp_signup.ID_Number,
                        Password = temp_signup.Password,
                        Is_Active = true,
                        ID_Role = userRole,
                        Status ="Active"
                    };
                    database.Users.Add(user);
                    database.SaveChanges();

                    // remove in temp_signup table
                    database.Temp_Signup.Remove(temp_signup);
                    database.SaveChanges();
                    ajm.Message = "success";
                }
               
                return ajm;
            }
        }

        

        [HttpGet, Route("GetUsers")]
        public IEnumerable<User_Details_vw> GetUsers()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.User_Details_vw.ToList();
            }
        }

        [HttpGet, Route("GetUsers/key{key}")]
        public User_Details_vw GetUsersBykey(string key)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                return database.User_Details_vw.FirstOrDefault(u => u.ID_Number == key || u.RF == key);
            }
        }

        [HttpGet, Route("Users/status/{status}/type/{type}")]
        public IEnumerable<User_Details_vw> GetFilteredUsers(string status, string type)
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var users = database.User_Details_vw.OrderByDescending(i => i.Id).ToList();

                if (status != "X")
                {
                    users = users.Where(i => i.Status == status).ToList();
                }
                if (type != "X" )
                {
                    users = users.Where(i => i.Role_Name==type).ToList();
                }
                
                return users;
            }

        }

        [HttpGet, Route("GetStocksByFacility")]
        public IEnumerable<Temp_StocksbyFacility> GetStocksByFacility()
        {
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                database.Stocks_Per_Facility_Update_sp();
                return database.Temp_StocksbyFacility.ToList();
            }
        }

    }
}
