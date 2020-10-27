using GoogleApiUtils;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ControlPanelController : Controller
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
        dalUser objUser = new dalUser();
        dalLead objLead = new dalLead();
        MstClientViewModel Viewmodel = new MstClientViewModel();

        [AllowAnonymous]
        public ActionResult UserDetails()
        {
            var model = new UserProfileViewModel();
            model = objUser.GetUserDetails(User.Identity.Name);
            return PartialView("pvUserDetails", model);
        }

        public ActionResult Index(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            int TotalRow;
            //var currUser = UserManager.FindById(User.Identity.GetUserId());

            Viewmodel.MstClientView = objUser.UserGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            Viewmodel.FullLead = objLead.GetTotalFullLeads();
            Viewmodel.PartialLead = objLead.GetTotalPartialLeads();
            Viewmodel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/ControlPanel/Index";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvUserList", Viewmodel);
            }
            return View(Viewmodel);
        }

        public ActionResult MarkClient(string Email)
        {
            Viewmodel.CoOrdinator = objUser.GetAgentDDl();
            Viewmodel.DataGrid = new DataGrid();
            Viewmodel.DataGrid.Email = Email;

            return PartialView("pvAgentList", Viewmodel);
        }


        public ActionResult MarkClients(MstClientViewModel Itemdata)
        {
            if (ModelState.IsValid)
            {
                string Email = Itemdata.DataGrid.Email;
                string AgentEmail = Itemdata.DataGrid.AgentEmail;
                Itemdata.DataGrid = new DataGrid();
                Itemdata.DataGrid.Email = Email;
                string Color = color();
                Viewmodel.ClientView = objUser.GetUserByEmail(Itemdata.DataGrid.Email);
                TempData["ErrMsg"] = objUser.SaveClient(Itemdata.DataGrid.Email, AgentEmail, Color);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    string url = Url.Action("Index", "ControlPanel", new { area = "Admin", PageNo = 1, PageSize = 10 });
                    return Json(new { success = true, url = url });
                }
            }
            Viewmodel.CoOrdinator = objUser.GetAgentDDl();
            return PartialView("pvAgentList", Viewmodel);

        }

        public ActionResult PartialLead(int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            MstPartialLeadViewModel model = new MstPartialLeadViewModel();
            model.PartialLeads = objLead.GetPartialLeadPaged(PageNo, PageSize, out TotalRow);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/ControlPanel/PartialLead";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvPartialLead", model);
            }
            return View(model);
        }
        public ActionResult FullLead(int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            MstFullLeadViewModel model = new MstFullLeadViewModel();
            model.FullLeads = objLead.GetFullLeadPaged(PageNo, PageSize, out TotalRow);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/ControlPanel/FullLead";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvFullLead", model);
            }
            return View(model);
        }

        public string color()
        {
            var random = new Random();
            string color = String.Format("#{0:X6}", random.Next(0x1000000));
            return color;
        }

        public ActionResult GmailAuth()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GmailAuth(string Gmail)
        {
            if(!string.IsNullOrEmpty(Gmail))
            {
                
                Session["Gmail"] = Gmail;
                string url = GoogleAuthorizationHelper.GetAuthorizationUrl(Gmail);
                Response.Redirect(url);
            }
            return View();
        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult autocomplete(string term)
        {

            Viewmodel.ListSearchAutoComplete = objUser.ControlPanelAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in Viewmodel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }


    }
}