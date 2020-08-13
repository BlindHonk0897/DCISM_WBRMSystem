using DCISM_WBRMSystem.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCISM_WBRMSystem.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reserve
        public ActionResult UserReserveForm()
        {
            return View();
        }
        public ActionResult Index()
        {
            RouterModel model = new RouterModel();
            if (Session["UserTitleRole"] != null) { 
                if (Session["UserTitleRole"].ToString() == "Administrator")
                {
                    model._Partial = "_AdminPage";
                }
                else if (Session["UserTitleRole"].ToString() == "Teacher" || Session["UserTitleRole"].ToString() == "Student")
                {
                    model._Partial = "_UserPage";
                }else if(Session["UserTitleRole"].ToString() == "Chairperson")
                {
                    model._Partial = "_ChairpersonPage";
                }
               return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}