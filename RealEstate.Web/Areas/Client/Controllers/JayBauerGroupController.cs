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

namespace RealEstate.Web.Areas.Client.Controllers
{
    [Authorize(Roles = "Client,Agent,Admin")]
    public class JayBauerGroupController : Controller
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
        // GET: Client/JayBauerGroup
        dalUser objUser = new dalUser();
        UserAuthenticateView ObjUser = new UserAuthenticateView();

        dalCheckUserDeal Obj = new dalCheckUserDeal();
        MstCheckLoggedClientDeal ObjModel = new MstCheckLoggedClientDeal();


        public ActionResult Dashboard()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult UserDetails()
        {
            var model = new UserProfileViewModel();
            model = objUser.GetUserDetails(User.Identity.Name);
            return PartialView("pvUserDetails", model);
        }

        public ActionResult ScheduleMeeting()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult CheckDeal()
        {
            //ViewBag.ActiveURL = "DealList";
            ObjModel.CheckDeal = Obj.CheckDeal(User.Identity.Name);
            ObjModel.CheckSharedDeal = Obj.CheckSharedDeal(User.Identity.Name);
            return PartialView("pvCheckDeal", ObjModel);
        }

        [AllowAnonymous]
        public ActionResult CheckCalendar()
        {
            ObjModel.CheckCalendar = Obj.CheckCalendar(User.Identity.Name);
            // ViewBag.ActiveURL = "/client/mycalendar/list";
            //ViewBag.ActiveURL = "CalendarList";
            return PartialView("pvCheckCalendar", ObjModel);
        }


        [AllowAnonymous]
        public ActionResult LoggedInUser()
        {
            ObjModel.CheckCalendar = Obj.CheckCalendar(User.Identity.Name);
            ObjModel.CheckDeal = Obj.CheckDeal(User.Identity.Name);

           // ViewBag.ActiveURL = "/client/mycalendar/list";
           // ViewBag.ActiveURL = "CalendarList";
            return PartialView("pvCheckCalendar", ObjModel);
        }
        [AllowAnonymous]
        public ActionResult SharedTran()
        {
            ViewBag.ActiveURL = "Account";
            MultipleDealClientManageModel Objmodel = new MultipleDealClientManageModel();
            dalClientLiveDealView ObjDal = new dalClientLiveDealView();
            Objmodel.SharedDealList = new List<SharedClientView>();
            var Client = UserManager.FindById(User.Identity.GetUserId());
            Objmodel.SharedDealList = ObjDal.SharedDeal(Client.Id);
            return PartialView("pvSharedHistory", Objmodel);

        }

        public ActionResult AgentDDL()
        {
            //ViewBag.ActiveURL = "/client/mydeal/myagent";
            //ViewBag.ActiveURL = "DealAgentList";
            
            var Client = UserManager.FindById(User.Identity.GetUserId());
            ObjModel.CheckAgent = Obj.CheckAgent(Client.Id);
            return PartialView("pvAgentDDL", ObjModel);
        }


    }


}