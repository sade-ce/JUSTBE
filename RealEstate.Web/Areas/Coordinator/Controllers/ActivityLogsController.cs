using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    public class ActivityLogsController : Controller
    {
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
        // GET: Coordinator/ActivityLogs
        dalTrackActivity ObjDal = new dalTrackActivity();

        public ActionResult Track(string ClientID,string AgentID,string ActivityTitle,string Remarks,string TrackingSource)
        {
            utblMstClientActivityLog ObjModel = new utblMstClientActivityLog();
            ObjModel.ActivityTitle = ActivityTitle;
            ObjModel.AgentID = AgentID;
            ObjModel.ClientID = ClientID;
            ObjModel.Remarks = Remarks;
            ObjModel.TrackingSource = TrackingSource;
            ObjDal.LogActivity(ObjModel);
            return Json("",JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ListActivityLog()
        {
            MstActivityLogViewModel ObjModel = new MstActivityLogViewModel();
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            if (Agent == null)
            {
                return PartialView("pvClientEmailActivityLog", ObjModel);
            }
            else
            {
                ObjModel.ActivityLogView = ObjDal.GetActivityHistory(Agent.Id.ToString());
                return PartialView("pvClientEmailActivityLog", ObjModel);
            }
  
        }

    }
}