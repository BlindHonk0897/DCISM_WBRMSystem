using DCISM_WBRMSystem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCISM_WBRMSystem.Controllers
{
    [CustomAuthenticationFilter]
    public class FacilityController : Controller
    {
        // GET: Room
        [CustomAuthorize("Administrator")]
        public ActionResult Rooms()
        {
            return View();
        }

        [CustomAuthorize("Administrator")]
        public ActionResult Details(int? id)
        {
            return View();
        }

    }
}