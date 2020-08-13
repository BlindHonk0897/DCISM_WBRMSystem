using DCISM_WBRMSystem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCISM_WBRMSystem.Controllers
{
    [CustomAuthenticationFilter]
    public class HistoryController : Controller
    {
        // GET: History
        [CustomAuthorize("Teacher","Student")]
        public ActionResult Index()
        {
            return View();
        }
    }
}