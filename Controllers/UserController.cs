using DCISM_WBRMSystem.Infrastructure;
using DCISM_WBRMSystem.Models;
using DCISM_WBRMSystem.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCISM_WBRMSystem.Controllers
{
    [CustomAuthenticationFilter]
    public class UserController : Controller
    {
        [CustomAuthorize("Administrator", "Chairperson")]
        public ActionResult Index()
        {
            RouterModel model = new RouterModel();
            if (Session["UserTitleRole"] != null)
            {
                if (Session["UserTitleRole"].ToString() == "Administrator")
                {
                    model._Partial = "_AdminPage";
                }
                else if (Session["UserTitleRole"].ToString() == "Chairperson")
                {
                    model._Partial = "_ChairpersonPage";
                }

            }
            else
            {
                RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        public JsonResult BlockUser(string userid)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var ID = Convert.ToUInt32(userid);
                var user = database.Users.FirstOrDefault(u => u.Id == ID);
                if (user != null)
                {
                    user.Status = "Blocked";
                    database.Entry(user).State = EntityState.Modified;
                    database.SaveChanges();
                    ajm.Message = "success";
                }
                else
                {
                    ajm.Message = "User is not found!.";
                }

                return Json(ajm);
            }
        }

        public JsonResult UnBlockUser(string userid)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var ID = Convert.ToUInt32(userid);
                var user = database.Users.FirstOrDefault(u => u.Id == ID);
                if (user != null)
                {
                    user.Status = "Active";
                    database.Entry(user).State = EntityState.Modified;
                    database.SaveChanges();
                    ajm.Message = "success";
                }
                else
                {
                    ajm.Message = "User is not found!.";
                }

                return Json(ajm);
            }
        }

        public JsonResult RemoveUser(string userid)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var ID = Convert.ToUInt32(userid);
                var user = database.Users.FirstOrDefault(u => u.Id == ID);
                if (user != null)
                {
                    database.Users.Remove(user);
                    database.SaveChanges();
                    switch (user.ID_Role)
                    {
                        case 1:
                        case 2:
                        case 3:
                            var faculty = database.Faculties.FirstOrDefault(f => f.ID_Number == user.ID_Number);
                            if (faculty != null)
                            {
                                database.Faculties.Remove(faculty);
                                database.SaveChanges();
                            }
                            break;
                        case 4:
                            var student = database.Students.FirstOrDefault(f => f.ID_Number == user.ID_Number);
                            if (student != null)
                            {
                                database.Students.Remove(student);
                                database.SaveChanges();
                            }
                            break;
                        default: break;
                    }
                    ajm.Message = "success";
                }
                else
                {
                    ajm.Message = "User is not found!.";
                }

                return Json(ajm);
            }
        }

        public ActionResult UserProfile(string IDNumber)
        {
            User_Details_vw model = new User_Details_vw();

            PasswordSecurity ps = new PasswordSecurity();
            if (IDNumber != "")
            {
                try
                {
                    IDNumber = ps.Decryptdata(IDNumber);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("UnAuthorized", "Home");
                }

                using (WBRMSystemEntities database = new WBRMSystemEntities())
                {
                    var uProfile = database.User_Details_vw.FirstOrDefault(u => u.ID_Number == IDNumber);
                    if (uProfile == null)
                    {

                        return RedirectToAction("UnAuthorized", "Home");
                    }
                    else
                    {
                        model = uProfile;
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult changePassword(string idnum, string oldP, string newP, string conP)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            PasswordSecurity ps = new PasswordSecurity();

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var user = database.Users.FirstOrDefault(u => u.ID_Number == idnum);
                if (user != null)
                {
                    if (user.Password == ps.Encryptdata(oldP))
                    {
                        user.Password = ps.Encryptdata(newP);
                        database.Entry(user).State = EntityState.Modified;
                        database.SaveChanges();
                        ajm.Message = "success";
                    }
                    else
                    {
                        ajm.Message = "Old password is not correct.";
                    }
                }
                else
                {
                    ajm.Message = "User is not found!.";
                }

                return Json(ajm);
            }
        }

        [HttpPost]
        public JsonResult changeEmail(string idnum, string myP, string newE)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            PasswordSecurity ps = new PasswordSecurity();

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var user = database.Users.FirstOrDefault(u => u.ID_Number == idnum);
                if (user != null)
                {
                    if (user.Password == ps.Encryptdata(myP))
                    {

                        switch (user.ID_Role)
                        {
                            case 1:
                            case 2:
                            case 3:
                                var user_dynamic = database.Faculties.FirstOrDefault(f => f.ID_Number == user.ID_Number);
                                if (user_dynamic != null)
                                {
                                    if (!checkEmailExist(newE))
                                    {
                                        user_dynamic.Email_Add = newE;
                                        database.Entry(user).State = EntityState.Modified;
                                        database.SaveChanges();
                                        ajm.Message = "success";
                                    }
                                    else
                                    {
                                        ajm.Message = "Email is already used.";
                                    }
                                }
                                break;
                            case 4:
                                var user_dynamic1 = database.Students.FirstOrDefault(f => f.ID_Number == user.ID_Number);
                                if (user_dynamic1 != null)
                                {
                                    if (!checkEmailExist(newE))
                                    {
                                        user_dynamic1.Email_Add = newE;
                                        database.Entry(user).State = EntityState.Modified;
                                        database.SaveChanges();
                                        ajm.Message = "success";
                                    }
                                    else
                                    {
                                        ajm.Message = "Email is already used.";
                                    }
                                }
                                break;
                            default: break;
                        }


                    }
                    else
                    {
                        ajm.Message = "Your password is not correct.";
                    }
                }
                else
                {
                    ajm.Message = "User is not found!.";
                }

                return Json(ajm);
            }
        }


        [HttpPost]
        public JsonResult changeNumber(string idnum, string myP, string newNum)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            PasswordSecurity ps = new PasswordSecurity();

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var user = database.Users.FirstOrDefault(u => u.ID_Number == idnum);
                if (user != null)
                {
                    if (user.Password == ps.Encryptdata(myP))
                    {
                        switch (user.ID_Role)
                        {
                            case 1:
                            case 2:
                            case 3:
                                var user_dynamic = database.Faculties.FirstOrDefault(f => f.ID_Number == user.ID_Number);
                                if (user_dynamic != null)
                                {
                                    if (!checkNumberExist(newNum))
                                    {
                                        user_dynamic.Contact_No = newNum;
                                        database.Entry(user_dynamic).State = EntityState.Modified;
                                        database.SaveChanges();
                                        ajm.Message = "success";
                                    }
                                    else
                                    {
                                        ajm.Message = "Mobile number is already used.";
                                    }
                                }
                                break;
                            case 4:
                                var user_dynamic1 = database.Students.FirstOrDefault(f => f.ID_Number == user.ID_Number);
                                if (user_dynamic1 != null)
                                {
                                    if (!checkNumberExist(newNum))
                                    {
                                        user_dynamic1.Contact_No = newNum;
                                        database.Entry(user_dynamic1).State = EntityState.Modified;
                                        database.SaveChanges();
                                        ajm.Message = "success";
                                    }
                                    else
                                    {
                                        ajm.Message = "Email is already used.";
                                    }
                                }
                                break;
                            default: break;
                        }
                    }
                    else
                    {
                        ajm.Message = "Your password is not correct.";
                    }
                }
                else
                {
                    ajm.Message = "User is not found!.";
                }

                return Json(ajm);
            }
        }

        [HttpPost]
        public JsonResult changeUsername(string idnum, string myP, string newUsername)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "failed"
            };
            PasswordSecurity ps = new PasswordSecurity();

            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var user = database.Users.FirstOrDefault(u => u.ID_Number == idnum);
                if (user != null)
                {
                    if (user.Password == ps.Encryptdata(myP))
                    {
                        if (!checkUsernameExist(newUsername))
                        {
                            user.Username = newUsername;
                            database.Entry(user).State = EntityState.Modified;
                            database.SaveChanges();
                            Session["Username"] = newUsername;
                            ajm.Message = "success";
                        }
                        else
                        {
                            ajm.Message = "Username already used.Please choose unique one.";
                        }
                    }
                    else
                    {
                        ajm.Message = "Your password is not correct.";
                    }
                }
                else
                {
                    ajm.Message = "User is not found!.";
                }

                return Json(ajm);
            }
        }

        public Boolean checkEmailExist(string email)
        {
            Boolean isExist = false;
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var user = database.User_Details_vw.FirstOrDefault(u => u.Email_Add == email);
                if (user != null)
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        public Boolean checkNumberExist(string number)
        {
            Boolean isExist = false;
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var user = database.User_Details_vw.FirstOrDefault(u => u.Contact_No == number);
                if (user != null)
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        public Boolean checkUsernameExist(string username)
        {
            Boolean isExist = false;
            using (WBRMSystemEntities database = new WBRMSystemEntities())
            {
                var user = database.User_Details_vw.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    isExist = true;
                }
            }
            return isExist;
        }
    }
}