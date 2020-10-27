using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.ViewModels;
using System.Net.Configuration;
using System.Web.Configuration;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]

    public class DealAgentV2Controller : Controller
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
        dalDealAgent ObjDal = new dalDealAgent();
        MstDealAgentManageModel ObjModel = new MstDealAgentManageModel();
        dalDealAdmin ObjAdminDal = new dalDealAdmin();
        dalAgentDealSharing ObjShare = new dalAgentDealSharing();

      

        public ActionResult List(int PageNo = 1, int PageSize = 10, string sortOrder = "", string searchterm = "", string year = "", string type = "", string stage = "",string tier="")
        {
            ViewBag.ActiveURL = "DealList";
            int TotalRow;
            ObjModel.ClientSortingInfo = new ClientSortingInfo
            {
                CurrentSort = sortOrder,
                TransactionIdSort = String.IsNullOrEmpty(sortOrder) ? "Id_asc" : "",
                //PhoneSort = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc",
                NameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc",
                AddressSort = sortOrder == "address_asc" ? "address_desc" : "address_asc",
                TypeSort = sortOrder == "type_asc" ? "type_desc" : "type_asc",
                YearSort = sortOrder == "year_asc" ? "year_desc" : "year_asc",
                StageSort = sortOrder == "stage_asc" ? "stage_desc" : "stage_asc",
                TierSort = sortOrder == "tier_asc" ? "tier_desc" : "tier_asc",
            };
            if (searchterm != null)
            {
                searchterm = searchterm.Trim();
                searchterm = Regex.Replace(searchterm, @"\s+", " ");
            }
            ObjModel.ClientFilterInfo = new ClientFilterInfo
            {
                SearchFilter = searchterm,
                YearFilter = year,
                TypeFilter = type,
                StageFilter = stage,
            };
            var Agent = UserManager.FindById(User.Identity.GetUserId());

            ObjModel.ClientList = ObjDal.ClientListVersion2(PageNo, PageSize, out TotalRow, Agent.Id, searchterm, year, type, sortOrder, stage,tier);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvList", ObjModel);

            }
            return View(ObjModel);
        }

        /// <summary>
        /// added by neha
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult autocomplete(string term)
        {
            var Agent = UserManager.FindById(User.Identity.GetUserId());
            ObjModel.ListSearchAutoComplete = ObjDal.ClientAutoCompleteVesrsion2(term, Agent.Id);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in ObjModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }


        public ActionResult ActiveDeals(int PageNo = 1, int PageSize = 10, string searchterm = "Lead")
        {
            ViewBag.ActiveURL = "ActiveDealList";
            if (searchterm != null)
            {
                searchterm = searchterm.Trim();
                searchterm = Regex.Replace(searchterm, @"\s+", " ");
            }
           
            ObjModel.ClientFilterInfo = new ClientFilterInfo
            {
                SearchFilter = searchterm
            };
            int TotalRow;

            var Agent = UserManager.FindById(User.Identity.GetUserId());

            ObjModel.ClientList = ObjDal.ActiveDeals(PageNo, PageSize, out TotalRow, Agent.Id,searchterm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvActiveDeals", ObjModel);

            }
            return View(ObjModel);

        }


    }
}