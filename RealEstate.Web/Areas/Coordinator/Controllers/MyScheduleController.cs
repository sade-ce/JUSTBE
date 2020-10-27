using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]

    public class MyScheduleController : Controller
    {
        // GET: Coordinator/MySchedule
        dalMstAppointment Objdal = new dalMstAppointment();
        MstCalenderManageModel ObjModel = new MstCalenderManageModel();
        #region Membership Initialization
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion
        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetCalendarInfo()
        {
            var Agent = UserManager.FindByEmail(User.Identity.Name);

            var Events = Objdal.GetAgentCalenderList(Agent.Id).ToList();
            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}