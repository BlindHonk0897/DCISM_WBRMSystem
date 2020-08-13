using DCISM_WBRMSystem.Hubs;
using DCISM_WBRMSystem.Models.CustomModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCISM_WBRMSystem.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Test2()
        {
            return View();
        }
        [HttpPost]
        public JsonResult NontificationSender(string from , string type)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "success"
            };
            var notifContext = GlobalHost.ConnectionManager.GetHubContext<AdminNotificationHub>();
            notifContext.Clients.All.showNotification("New "+type+" Request. From user: "+ from);
            return Json(ajm);
        }

        [HttpPost]
        public JsonResult NontificationReceiver(string to, string message)
        {
            AjaxMessage ajm = new AjaxMessage()
            {
                Message = "success"
            };
            var notifContext = GlobalHost.ConnectionManager.GetHubContext<UserNotificationHub>();
            notifContext.Clients.All.showNotification(to, message);
            return Json(ajm);
        }

        public ActionResult UnAuthorized()
        {
            //string currentURL = Request.Url.AbsoluteUri;
            //CurrentUrl model = new CurrentUrl()
            //{
            //    _Url = currentURL
            //};
            return View();
        }
    }
}