using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Client.Controllers
{
    [Authorize(Roles = "Client,Agent,Admin")]
    public class BuildingsController : Controller
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
        BuildingViewModel objViewModel = new BuildingViewModel();
        dalBuildings objDal = new dalBuildings();

        // GET: Client/Building
        public ActionResult Index()
        {
            return View();
        }
        // GET: Client/Building
        public ActionResult Details(int BuildingId)
        {
            ViewBag.Title = objDal.GetBuildingByID(BuildingId).Name;
            ViewBag.ActiveURL = "Details";
            objViewModel.BuildingViews = objDal.GetBuildingClientSide(BuildingId);
            objViewModel.Gallery = objDal.GetGalleryList(BuildingId);
            objViewModel.BuildingVendorList = objDal.GetBuildingVendorList(BuildingId);
            objViewModel.BuildingDocuments = objDal.GetBuildingDocuments(BuildingId);
            objViewModel.RulesDropDown = objDal.GetRulesByBuilding(BuildingId);
            return View(objViewModel);
        }
    }
}