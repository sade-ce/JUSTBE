using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Client.Controllers
{
    [Authorize]
    public class JustBEController : Controller
    {
        // GET: Client/JustBE
        public ActionResult Welcome()
        {
            ViewBag.ActiveURL = "Welcome";

            return View();
        }
    }
}