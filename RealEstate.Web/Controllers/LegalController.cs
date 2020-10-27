using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers
{
    public class LegalController : Controller
    {
        // GET: Legal
        public ActionResult privacypolicy()
        {
            return View();
        }
        public ActionResult Terms()
        {
            return View();
        }

    }
}