using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.Models;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class DealAssignAgentController : Controller
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
        // GET: Admin/DealAssignAgent
        DealAdminAssignAgentManageModel ObjModel = new DealAdminAssignAgentManageModel();
        dalDealAdmin ObjDal = new dalDealAdmin();
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            ObjModel.ClientList = ObjDal.DealGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvList", ObjModel);
            }
            return View(ObjModel);
        }

        public ActionResult Create()
        {
            ObjModel.ClientDropDown = ObjDal.GetClientDDL();
            ObjModel.AgentDropDown = ObjDal.GetAgentDDL();
            ObjModel.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            return View(ObjModel);
        }

        [HttpPost]
        public ActionResult Create(DealAdminAssignAgentManageModel ItemData)
        {
            if (ModelState.IsValid)
            {
                ItemData.utblMstTrackDealMasters.Status = "Active";
                TempData["ErrMsg"] = ObjDal.AssignAgentVersion2(ItemData.utblMstTrackDealMasters);
                if (TempData["ErrMsg"].ToString() == "0")
                {
                    var Agent = UserManager.FindById(ItemData.utblMstTrackDealMasters.AgentID);
                    var Client = UserManager.FindById(ItemData.utblMstTrackDealMasters.ClientID);
                    int MailStatus = SignUpEmailConfirmation(Agent.Name, Agent.Email, "Thanks for registering with Just Be | Real Estate", Client.Name, Client.Email);
                    return RedirectToAction("List");
                }
            }
            ItemData.ClientDropDown = ObjDal.GetClientDDL();
            ItemData.AgentDropDown = ObjDal.GetAgentDDL();
            ItemData.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            return View(ItemData);
        }




        public ActionResult Edit(string TransactionID)
        {
            ObjModel.utblMstTrackDealMasters = ObjDal.GetDealbyID(TransactionID);
            ObjModel.ClientDropDown = ObjDal.GetClientDDL();
            ObjModel.AgentDropDown = ObjDal.GetAgentDDL();
            ObjModel.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            return View(ObjModel);
        }

        [HttpPost]
        public ActionResult Edit(DealAdminAssignAgentManageModel ItemData)
        {
            if (ModelState.IsValid)
            {
                ItemData.utblMstTrackDealMasters.Status = "Active";
                TempData["ErrMsg"] = ObjDal.Edit(ItemData.utblMstTrackDealMasters);
                if (TempData["ErrMsg"].ToString() == "0")
                {
                    return RedirectToAction("List");
                }
            }
            ObjModel.utblMstTrackDealMasters = ObjDal.GetDealbyID(ItemData.utblMstTrackDealMasters.TransactionID);

            ItemData.ClientDropDown = ObjDal.GetClientDDL();
            ItemData.AgentDropDown = ObjDal.GetAgentDDL();
            ItemData.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            return View(ItemData);
        }

        #region Delete Department
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(string TransactionID)
        {
            ViewBag.ActiveURL = "/Admin/DealAssignAgent/List";
            TempData["ErrMsg"] = ObjDal.Delete(TransactionID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("List");
        }
        #endregion

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

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult autocomplete(string term)
        {

            ObjModel.ListSearchAutoComplete = ObjDal.dalDealadminAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in ObjModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="SearchTerm"></param>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public ActionResult ListAgent(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            ObjModel.ClientList = ObjDal.DealGetPagedAgent(PageNo, PageSize, out TotalRow, SearchTerm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            return PartialView("pvList", ObjModel);
        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult AutocompleteAgent(string term)
        {

            ObjModel.ListSearchAutoComplete = ObjDal.dalDealadminAutoCompleteAgent(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in ObjModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

    }

}