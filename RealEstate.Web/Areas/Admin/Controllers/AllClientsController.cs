using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using RealEstate.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using RealEstate.Entities.Models;
using Newtonsoft.Json.Linq;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AllClientsController : Controller
    {
        #region Membership Initialization
        public AllClientsController()
        {
        }
        public AllClientsController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

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
        dalDealAdmin ObjDal = new dalDealAdmin();
        dalUser objUser = new dalUser();
        // GET: Admin/AllClients
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List(int PageNo = 1, int PageSize = 10, string sortOrder = "", string searchterm = "", string agentsearchterm = "", string year = "", string type = "", string stage = "", string tier = "")
        {
            ViewBag.ActiveURL = "ClientList";

            int TotalRow;
            UserListViewModel objUsers = new UserListViewModel();
            objUsers.AgentClientView = new MstAgentsClientView();
            objUsers.UserAdminSortingInfo = new UserAdminSortingInfo
            {
                CurrentSort = sortOrder,
                NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "",
                AgentNameSort = sortOrder == "agentname_asc" ? "agentname_desc" : "agentname_asc",
                RoleSort = sortOrder == "role_asc" ? "role_desc" : "role_asc",
                PhoneSort = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc",
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
            if (agentsearchterm != null)
            {
                agentsearchterm = agentsearchterm.Trim();
                agentsearchterm = Regex.Replace(agentsearchterm, @"\s+", " ");
            }
            objUsers.UserAdminListFiterInfo = new UserAdminListFiterInfo
            {
                SearchFilter = searchterm,
                AgentSearchFilter = agentsearchterm,
                YearFilter = year,
                TypeFilter = type,
                StageFilter = stage,
            };
            //if (searchterm != null)
            //{
            //    searchterm = searchterm.Trim();
            //    searchterm = Regex.Replace(searchterm, @"\s+", " ");
            //}
            //else
            //{
            //    searchterm = currentFilter;
            //}
            //ViewBag.CurrentFilter = searchterm;
            string AgentEmail = User.Identity.Name;
            var AgentID = UserManager.FindByEmail(User.Identity.Name);
            objUsers.AgentClientView.ClientList = objUser.GetAdminClientListNew(PageNo, PageSize, out TotalRow, AgentID.Id, searchterm, agentsearchterm, year, type, sortOrder, stage, tier);
            //  TotalRow = objUsers.AgentClientView.ClientList.Count();

            objUsers.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvUserList", objUsers);
            }
            return View(objUsers);
        }


        //public ActionResult Archive(string Id, int PageNo, int PageSize, int ListCount, string sortOrder = "", string searchterm = "", string year = "", string type = "", string stage = "")
        //{
        //    ViewBag.ActiveURL = "ClientList";



        //    TempData["ErrMsg"] = objUser.ArchiveUser(Id);
        //    if ((TempData["ErrMsg"].ToString()) == "0")
        //    {
        //        TempData["ErrMsg"] = null;
        //    }
        //    ListCount--;
        //    if (ListCount == 0)
        //        ListCount = 1;

        //    return RedirectToAction("List", new { PageNo = PageNo, PageSize = PageSize, sortOrder = sortOrder, searchterm = searchterm, year = year, type = type, stage = stage });
        //}
        public ActionResult autocomplete(string term)
        {
            UserListViewModel objUsers = new UserListViewModel();
            objUsers.ListSearchAutoComplete = objUser.AdminClientAutoCompletNew(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objUsers.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Delete(string Id, int PageNo, int PageSize, int ListCount, string sortOrder = "", string searchterm = "",string agentsearchterm="", string year = "", string type = "", string stage = "")
        {
            ViewBag.ActiveURL = "ClientList";

            if (Id == User.Identity.GetUserId())
            {
                TempData["ErrMsg"] = 4;
            }
            else if (ModelState.IsValid)
            {
                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                try
                {

                    TempData["ErrMsg"] = objUser.DeleteUser(user.Id, user.Email);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                    }
                    ListCount--;
                    if (ListCount == 0)
                        PageNo = 1;
                }

                catch (Exception)
                {
                    TempData["ErrMsg"] = "....Unable to delete user,Probably there is ongoing deal for this user,delete all the transaction and try again...";
                    //ModelState.AddModelError("", result.Errors.First());

                }

            }
            searchterm = "";
            agentsearchterm = "";
            return RedirectToAction("List", new { PageNo = PageNo, PageSize = PageSize, sortOrder = sortOrder, searchterm = searchterm, year = year, type = type, stage = stage });
        }

    }
}