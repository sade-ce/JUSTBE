using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers
{
    public class BuyPropertyController : Controller
    {
        // GET: BuyProperty
        public ActionResult Listing()
        {
            return View();
        }
    }
}