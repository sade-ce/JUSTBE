using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Client.Controllers
{
    [Authorize(Roles = "Client,Agent,Admin")]
    public class MyCalendarController : Controller
    {
        // GET: Client/MyCalender

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
        MstCalenderManageModel ObjModel = new MstCalenderManageModel();
        EFDBContext db = new EFDBContext();
        dalClientLiveDealView Objdal = new dalClientLiveDealView();
        public ActionResult List(string ClientID = "",string AgentID="")
        {
            Session["ClientID"] = ClientID;
            TempData["AgentID"] = AgentID;

            // ViewBag.ActiveURL = "/client/mycalendar/list";
            ViewBag.ActiveURL = "CalendarList";
            return View();
        }
        public JsonResult GetCalendarInfo()
        {
            ViewBag.Current = "Index";
            var Events = Objdal.GetCalenderList(User.Identity.Name).ToList();
            if (User.IsInRole("Agent"))
            {
                var Agent = UserManager.FindById(User.Identity.GetUserId());
                Events = Objdal.GetClientCalenderList(Agent.Id, Session["ClientID"].ToString()).ToList();
            }

            if(User.IsInRole("Admin"))
            {
                Events = Objdal.GetClientCalenderList(TempData["AgentID"].ToString(), Session["ClientID"].ToString()).ToList();
            }
            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}