using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using RealEstate.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DealsController : Controller
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
        DealAdminAssignAgentManageModel ObjDealModel = new DealAdminAssignAgentManageModel();
        dalDealAdmin ObjAdminDal = new dalDealAdmin();
        dalAgentDealSharing ObjShare = new dalAgentDealSharing();
        dalUser objUser = new dalUser();

        public ActionResult List(int PageNo = 1, int PageSize = 10, string sortOrder = "", string searchterm = "", string agentsearchterm = "", string year = "", string type = "", string stage = "", string tier = "")
        {
            ViewBag.ActiveURL = "DealList";
            int TotalRow;
            ObjModel.AdminDealsSortingInfo = new AdminDealsSortingInfo
            {
                CurrentSort = sortOrder,
                TransactionIdSort = String.IsNullOrEmpty(sortOrder) ? "Id_asc" : "",
                //PhoneSort = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc",
                NameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc",
                AgentNameSort = sortOrder == "agentname_asc" ? "agentname_desc" : "agentname_asc",
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
            if (agentsearchterm != null)
            {
                agentsearchterm = agentsearchterm.Trim();
                agentsearchterm = Regex.Replace(agentsearchterm, @"\s+", " ");
            }
            ObjModel.AdminDealsFilterInfo = new AdminDealsFilterInfo
            {
                SearchFilter = searchterm,
                AgentSearchFilter= agentsearchterm,
                YearFilter = year,
                TypeFilter = type,
                StageFilter = stage,
            };
         
            ObjModel.ClientList = ObjDal.ClientListAdmin(PageNo, PageSize, out TotalRow,searchterm, agentsearchterm, year, type, sortOrder, stage, tier);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvList", ObjModel);

            }
            return View(ObjModel);
        }


    
       public ActionResult Delete(string TransactionID, int PageNo, int PageSize, int ListCount, string sortOrder = "", string searchterm = "", string agentsearchterm = "", string year = "", string type = "", string stage = "")

        {
            ViewBag.ActiveURL = "DealList";
            TempData["ErrMsg"] = ObjDal.DeleteDeal(TransactionID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("List", new { PageNo = PageNo, PageSize = PageSize, sortOrder = sortOrder, searchterm = searchterm, agentsearchterm = agentsearchterm, year = year, type = type, stage = stage });
        }



        /// <summary>
        /// added by neha
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult autocomplete(string term)
        {
            var Agent = UserManager.FindById(User.Identity.GetUserId());
            ObjModel.ListSearchAutoComplete = ObjDal.AdminAutoCompleteVesrsion2(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in ObjModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

           public ActionResult ClientAutocomplete(string term)
        {
            var AgentID = UserManager.FindByEmail(User.Identity.Name);
            UserListViewModel objUsers = new UserListViewModel();
            objUsers.ClientSearchAutoComplete = objUser.ClientAutocompleteVersion2(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objUsers.ClientSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.Id));

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutocompleteAgent(string term)
        {

            ObjModel.AgentSearchAutoComplete = ObjDal.AgentAutoCompleteVesrsion2(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in ObjModel.AgentSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.Id));

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

            ObjModel.ClientList = ObjDal.ActiveDealsAdmin(PageNo, PageSize, out TotalRow, searchterm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvActiveDeals", ObjModel);

            }
            return View(ObjModel);

        }


        public ActionResult Create()
        {
            ViewBag.ActiveURL = "AddDeal";
            ObjDealModel.ClientTypeDropDown = ObjAdminDal.GetClientTypeDDL();
            return View(ObjDealModel);
        }

        [HttpPost]
        public ActionResult Create(DealAdminAssignAgentManageModel ItemData)
        {
            //if (ModelState.IsValid)
            // {
            ItemData.utblMstTrackDealMasters.Status = "Active";
            TempData["ErrMsg"] = ObjAdminDal.AssignAgentVersion2(ItemData.utblMstTrackDealMasters);
                if (TempData["ErrMsg"].ToString() == "0")
                {
                TempData["ErrMsg"] = null;
                var Agent = UserManager.FindById(ItemData.utblMstTrackDealMasters.AgentID);
                    var Client = UserManager.FindById(ItemData.utblMstTrackDealMasters.ClientID);
                    int MailStatus = SignUpEmailConfirmation(Agent.Name, Agent.Email, "Thanks for registering with Just Be | Real Estate", Client.Name, Client.Email);
                   return RedirectToAction("List");
                }
            //}
            //   return Json(new { success = true });
            ItemData.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            return View(ItemData);
        }

        #region SignUpEmailConfirmation
        private int SignUpEmailConfirmation(string AgentName, string AgentEmail, string subject, string ClientName, string ClientEmail)
        {


            //string code = UserManager.GenerateEmailConfirmationToken(userID);
            //string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code, area = "" }, protocol: Request.Url.Scheme);

            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            //Create the SMTP Client
            SmtpClient client = new SmtpClient();
            client.Host = settings.Smtp.Network.Host;
            client.Port = settings.Smtp.Network.Port;
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = false;
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(settings.Smtp.Network.UserName, "Just BE.");

            StringBuilder mailbody = new StringBuilder();

            mm.To.Add(AgentEmail);
            mm.Priority = MailPriority.High;

            mm.Subject = "You have a new client assignment";
            mm.Body = this.PopulateBody(AgentName, ClientName, ClientEmail);
            mm.IsBodyHtml = true;

            try
            {
                client.Send(mm);
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        #endregion
        #region HtmlEmail
        private string PopulateBody(string AgentName, string ClientName, string ClientEmail)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/AgentEmail.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{AgentName}", AgentName);
            body = body.Replace("{ClientName}", ClientName);
            body = body.Replace("{ClientEmail}", ClientEmail);
            body = body.Replace("{Url}", "http://justbere.com/Account/Login");
            return body;
        }
        #endregion
    }
}