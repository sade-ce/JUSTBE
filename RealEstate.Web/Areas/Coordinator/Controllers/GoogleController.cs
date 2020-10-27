using GoogleApiUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]

    public class GoogleController : Controller
    {
        // GET: Coordinator/Google
        public ActionResult Authenticate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(string Gmail)
        {
            if (!string.IsNullOrEmpty(Gmail))
            {
                Session["Gmail"] = Gmail;
                string url = GoogleAuthorizationHelper.GetAuthorizationUrl(Gmail);
                Response.Redirect(url,false);
            }
            return View();
        }
    }
}