using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers
{
    public class DCController : Controller
    {
        // GET: DC
        public ActionResult Neighborhood()
        {
            return View();
        }
    }
}