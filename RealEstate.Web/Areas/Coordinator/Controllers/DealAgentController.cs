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

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]

    public class DealAgentController : Controller
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

        //Old Version List
        public ActionResult List(string SearchTerm = "", string Data="", int PageNo = 1, int PageSize = 30)
        {
            int TotalRow;
            var Agent = UserManager.FindById(User.Identity.GetUserId());
            
            ObjModel.ClientList = ObjDal.ClientList(PageNo, PageSize, out TotalRow, Agent.Id, SearchTerm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            if (Request.IsAjaxRequest())
            {
                if(Data.ToString()== "ViewShared")
                {
                    ObjModel.SharedClientList = ObjShare.SharedClientList(Agent.Id);

                    return PartialView("pvSharedView", ObjModel);
                }
                else if(Data.ToString()== "HasShared")
                {
                    ObjModel.SharedDealList = ObjShare.SharedDealList(Agent.Id);

                    return PartialView("pvHasShared", ObjModel);

                }
                else
                {
                    return PartialView("pvList", ObjModel);

                }
            }
            return View(ObjModel);
        }
   
        [HttpPost]
        public ActionResult RemoveShared(string TransactionID)
        {
           
            TempData["ErrMsg"] = ObjShare.Delete(TransactionID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;

            }
            var Agent = UserManager.FindById(User.Identity.GetUserId());
            ObjModel.SharedDealList = ObjShare.SharedDealList(Agent.Id);
            return PartialView("pvHasShared", ObjModel);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ClientDetails(string ClientID,string AgentID="")
        {
            ObjModel.GetSharedDeal = new List<ClientDetails>();


            if (User.IsInRole("Admin"))
            {
                ObjModel.ClientDetails = ObjDal.ClientDetails(ClientID, AgentID);

            }
            else
            {
                var Agent = UserManager.FindById(User.Identity.GetUserId());
                ObjModel.ClientDetails = ObjDal.ClientDetails(ClientID, Agent.Id);

                ObjModel.GetSharedDeal = ObjShare.GetSharedDeal(Agent.Id);
                AgentID = Agent.Id;//Added by Neha New
            }
            ObjModel.UserProfile = ObjDal.GetUserDetails(ClientID);
            ViewBag.AgentID = AgentID;
            return View(ObjModel);
        }


        public ActionResult SellerDeal()
        {
            return View();
        }


        #region CreateTransaction
        public ActionResult CreateTransaction()
        {
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            ViewBag.AgentID = Agent.Id;
            ObjModel.ClientDropDown=ObjAdminDal.GetClientDDL();
            ObjModel.AgentDropDown = ObjAdminDal.GetAgentDDL();
            ObjModel.ClientTypeDropDown = ObjAdminDal.GetClientTypeDDL();
            return PartialView("pvCreateTransaction", ObjModel);
        }

        [HttpPost]
        public ActionResult CreateTransaction(MstDealAgentManageModel ItemData)
        {
            if (ModelState.IsValid)
            {
                ItemData.utblMstTrackDealMasters.Status = "Active";
                TempData["ErrMsg"] = ObjAdminDal.AssignAgent(ItemData.utblMstTrackDealMasters);
                if (TempData["ErrMsg"].ToString() == "0")
                {
                    var Agent = UserManager.FindById(ItemData.utblMstTrackDealMasters.AgentID);
                    var Client = UserManager.FindById(ItemData.utblMstTrackDealMasters.ClientID);
                    int MailStatus = SignUpEmailConfirmation(Agent.Name, Agent.Email, "Thanks for registering with Just BE | Real Estate", Client.Name, Client.Email);
                    string url = Url.Action("ClientDetails", "DealAgent", new { ClientID = ItemData.utblMstTrackDealMasters.ClientID, AgentID = ItemData.utblMstTrackDealMasters.AgentID });
                    return Json(new { success = true, url = url, type = "gettrans" });
                }
            }
            ViewBag.ClientID = ItemData.utblMstTrackDealMasters.ClientID;
            ItemData.AgentDropDown = ObjAdminDal.GetAgentDDL();
            ItemData.ClientDropDown = ObjAdminDal.GetClientDDL();
            ItemData.ClientTypeDropDown = ObjAdminDal.GetClientTypeDDL();
            ViewBag.AgentID = ItemData.utblMstTrackDealMasters.AgentID;
            return PartialView("pvCreateTransaction", ItemData);
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
            var Agent = UserManager.FindById(User.Identity.GetUserId());
            ObjModel.ListSearchAutoComplete = ObjDal.ClientAutoComplete(term, Agent.Id);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in ObjModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// Added by sonika 18-05-2019
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <returns></returns>
        #region Delete Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string TransactionID, string clientId, string agentID = "")
        {

            TempData["ErrMsg"] = ObjAdminDal.Delete(TransactionID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("ClientTransDetails", new { ClientId = clientId, AgentID = agentID });
        }
        #endregion
        public ActionResult ClientTransDetails(string ClientID, string AgentID = "")
        {
            ObjModel.GetSharedDeal = new List<ClientDetails>();


            if (User.IsInRole("Admin"))
            {
                ObjModel.ClientDetails = ObjDal.ClientDetails(ClientID, AgentID);

            }
            else
            {
                var Agent = UserManager.FindById(User.Identity.GetUserId());
                ObjModel.ClientDetails = ObjDal.ClientDetails(ClientID, Agent.Id);

                ObjModel.GetSharedDeal = ObjShare.GetSharedDeal(Agent.Id);
            }
            ObjModel.UserProfile = ObjDal.GetUserDetails(ClientID);
            ViewBag.AgentID = AgentID;
            return PartialView("pvTransactionList", ObjModel);
        }
    }
}