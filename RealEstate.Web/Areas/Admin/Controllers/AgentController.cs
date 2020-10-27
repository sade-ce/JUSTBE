using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{
        [Authorize(Roles = "Admin")]
    public class AgentController : Controller
    {
        // GET: Admin/Agent
        public ActionResult Index()
        {
            return View();
        }
    }
}