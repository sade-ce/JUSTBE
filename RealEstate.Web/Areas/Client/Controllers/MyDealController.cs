using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.DataAccess;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using RealEstate.Web.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using RealEstate.Entities.Models;
using Yelp.Api.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Net.Configuration;
using System.Web.Configuration;
using System.Net.Mail;
using System.Text;
using HtmlAgilityPack;




using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Calendar.v3.Data;
using static RealEstate.Web.Controllers.AboutController;

namespace RealEstate.Web.Areas.Client.Controllers
{
    [Authorize(Roles = "Client,Agent,Admin")]
    public class MyDealController : Controller
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
        MstClientLiveDealViewModel ObjModel = new MstClientLiveDealViewModel();
        dalClientLiveDealView ObjDal = new dalClientLiveDealView();
        dalMstEnquire ObjEnquire = new dalMstEnquire();
        dalMstClientList objTransactionDal = new dalMstClientList();
        VendorCategoryViewModel objCategoryModel = new VendorCategoryViewModel();
        dalVendorCategory objCategoryDal = new dalVendorCategory();
        VendorView objVendorDetails = new VendorView();
        VendorViewModel objVendorModel = new VendorViewModel();
        dalVendor objVendorDal = new dalVendor();
        ClientDealDocumentsViewModel objClientDocModel = new ClientDealDocumentsViewModel();
        dalClientDealDocuments objClientDocDal = new dalClientDealDocuments();
        dalClientToDoList objToDoList = new dalClientToDoList();


        dalMstAppointment objAppointment = new dalMstAppointment();
        MstCalenderManageModel objCalenderManageModel = new MstCalenderManageModel();
    
        dalBuildings objdalBuilding = new dalBuildings();

        VendorsReviewViewModel objReviewsModel = new VendorsReviewViewModel();
        dalVendorReviews objReviewsDal = new dalVendorReviews();


        EFDBContext db = new EFDBContext();

        public async Task<ActionResult> List(string ClientID = "", string Message = "", string AgentID = "")
        {
            //ViewBag.ActiveURL = "/client/mydeal/list";  
            ViewBag.ActiveURL = "DealList";
            ViewBag.AgentID = AgentID;
            var Client = UserManager.FindById(User.Identity.GetUserId());

            MultipleDealClientManageModel Objmodel = new MultipleDealClientManageModel();
            Objmodel.HomeGalleryView = new List<ClientHomeGallery>();
            Objmodel.DisplayListing = new MLSListingDetails();
         

            if (User.IsInRole("Agent"))
            {
                Objmodel.MultipleDealList = ObjDal.AgentMultipleDeal(Client.Id, ClientID);
                //If deal is shared with client --Added by Neha 19-03-2020
                Objmodel.SharedDealList = ObjDal.SharedDeal(ClientID);
                ViewBag.IsClient = "Agent";
            }
            else if (User.IsInRole("Admin"))
            {
                Objmodel.MultipleDealList = ObjDal.AgentMultipleDeal(AgentID, ClientID);
                //If deal is shared with client --Added by Neha 19-03-2020
                Objmodel.SharedDealList = ObjDal.SharedDeal(ClientID);
                ViewBag.IsClient = "Admin";
            }

            else
            {
                Objmodel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
                //If deal is shared with client --Added by Neha 19-03-2020
                Objmodel.SharedDealList = ObjDal.SharedDeal(Client.Id);
                ViewBag.IsClient = "Client";
            }

            foreach (var item in Objmodel.MultipleDealList)
            {
                if (item.MLS != "N/A")
                {

                    Objmodel.DisplayListing = ListingDetails(item.MLS);
                    item.Image = Objmodel.DisplayListing.ImageURL;
                }
                else
                {
                    var data = new ClientHomeGallery();
                    if (User.IsInRole("Agent") || User.IsInRole("Admin"))
                    {
                        data = ObjDal.ClientHomeGallery(ClientID, item.TransactionID);
                    }
                    else
                    {
                        data = ObjDal.ClientHomeGallery(Client.Id, item.TransactionID);

                    }

                    if (data != null)
                    {
                        item.Image = "../../Uploadedfiles/SellerHome/" + data.HomePhotoID + data.FileExt;

                    }
                    else
                    {
                        item.Image = "N/A";
                    }

                }



            }
            //If deal is shared with client --Added by Neha 19-03-2020
            foreach (var item in Objmodel.SharedDealList)
            {
                if (item.MLS != "N/A")
                {
                    Objmodel.DisplayListing = ListingDetails(item.MLS);
                    item.Image = Objmodel.DisplayListing.ImageURL;
                }
                else
                {
                    var data = new ClientHomeGallery();


                    data = ObjDal.ClientHomeGallery(Client.Id, item.TransactionID);



                    if (data != null)
                    {
                        item.Image = "../../Uploadedfiles/SellerHome/" + data.HomePhotoID + data.FileExt;

                    }
                    else
                    {
                        item.Image = "N/A";
                    }

                }



            }

            Objmodel.TranDDL = ObjDal.TransactionDDL(Client.Id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvList", Objmodel);

            }
            Objmodel.CheckDeal = ObjDal.CheckDeal(User.Identity.Name);
            Objmodel.KeyInfoLinkList = ObjDal.MstKeyInfoLinkList;
            Objmodel.MstUserAgentView = new MstUserAgentView();
            foreach (var item in Objmodel.TranDDL)
            {
                Objmodel.MstUserAgentView = ObjDal.AgentView(item.TransactionID);
                break;
            }

            ViewBag.message = Message;
            if (Objmodel.MultipleDealList.Count() == 0 && Objmodel.SharedDealList.Count()==0)
            {
                return RedirectToAction("Welcome", "JustBE", new { area = "Client" });

            }
            return View(Objmodel);
        }
        public async Task<ActionResult> SharedTransactions(string ClientID = "", string Message = "", string AgentID = "")
        {
            ViewBag.AgentID = AgentID;
            var Client = UserManager.FindById(User.Identity.GetUserId());

            MultipleDealClientManageModel Objmodel = new MultipleDealClientManageModel();
            Objmodel.HomeGalleryView = new List<ClientHomeGallery>();
            Objmodel.DisplayListing = new MLSListingDetails();
            Objmodel.SharedDealList = ObjDal.SharedDeal(Client.Id);
            foreach (var item in Objmodel.SharedDealList)
            {
                if (item.MLS != "N/A")
                {
                    Objmodel.DisplayListing = ListingDetails(item.MLS);
                    item.Image = Objmodel.DisplayListing.ImageURL;
                }
                else
                {
                    var data = new ClientHomeGallery();


                    data = ObjDal.ClientHomeGallery(Client.Id, item.TransactionID);



                    if (data != null)
                    {
                        item.Image = "../../Uploadedfiles/SellerHome/" + data.HomePhotoID + data.FileExt;

                    }
                    else
                    {
                        item.Image = "N/A";
                    }

                }



            }


            Objmodel.TranDDL = ObjDal.TransactionDDL(Client.Id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvSharedTransactions", Objmodel);

            }
            Objmodel.CheckDeal = ObjDal.CheckDeal(User.Identity.Name);
            Objmodel.KeyInfoLinkList = ObjDal.MstKeyInfoLinkList;
            Objmodel.MstUserAgentView = new MstUserAgentView();
            foreach (var item in Objmodel.TranDDL)
            {
                Objmodel.MstUserAgentView = ObjDal.AgentView(item.TransactionID);
                break;
            }

            ViewBag.message = Message;
            return View(Objmodel);
        }
        public ActionResult ShareDeal(string TransactionID)
        {
            MstClientShareDealManageModel objModel = new MstClientShareDealManageModel();
            objModel.TransactionDetails = objTransactionDal.GetTransactionDetails(TransactionID);
            objModel.SharedClientList = ObjDal.GetSharedAccounts(TransactionID);
            return PartialView("pvShareDeal", objModel);
        }
        [HttpPost]
        public async Task<ActionResult> ShareDeal(MstClientShareDealManageModel Model, string SharedAccounts)
        {
            string url = Url.Action("ShareDeal", "MyDeal", new { TransactionID = Model.TransactionDetails.TransactionID });
           
            if (SharedAccounts != "")
            {
                var user = await UserManager.FindByNameAsync(SharedAccounts);
                
                TempData["ErrMsg"] = ObjDal.ShareTransaction(Model.TransactionDetails.TransactionID, SharedAccounts);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {

                    TempData["ErrMsg"] = null;
                }
                var SharedByUsername = UserManager.FindById(User.Identity.GetUserId());
                if (ObjDal.IsClient(SharedAccounts))
                {
                 
                    int MailStatus = SendSharedEmailToRegisteredUser(user.Name, user.Id, "Shared Transaction", user.Email, Model.TransactionDetails.TransactionID,User.Identity.Name,SharedByUsername.Name+" "+ SharedByUsername.LastName);
                    if (MailStatus == 0)
                    {
                        //TempData["MailMsg"] = 1;
                    }
                    else
                    {
                       // TempData["MailMsg"] = 0;
                    }
                }
                else
                {
                    int MailStatus1 = SendSharedEmailToNotRegisteredUser("Shared Transaction", SharedAccounts, Model.TransactionDetails.TransactionID, User.Identity.Name, SharedByUsername.Name + " " + SharedByUsername.LastName);
                    if (MailStatus1 == 0)
                    {
                        //TempData["MailMsg"] = 1;
                    }
                    else
                    {
                        // TempData["MailMsg"] = 0;
                    }
                }
                return Json(new { success = true, url = url, message = "found" });
            
            }

            return Json(new { url = url, message = "Client Not Selected" });
        }

        //Email to registered user
        private int SendSharedEmailToRegisteredUser(string Name, string userID, string subject, string email, string TransactionId,string SharedBy,string SharedByUser)
        {
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

            mm.To.Add(email);
            mm.Priority = MailPriority.High;

            mm.Subject = "Shared Transaction- Just BE.";
            mm.Body = this.RegisteredUserEmail(Name, email, TransactionId,SharedBy,SharedByUser);
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

        private string RegisteredUserEmail(string Name, string userName, string TransactionId,string SharedBy,string SharedByUser)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/RegisteredUsers.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FirstName}", Name);
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{TransactionId}", TransactionId);
            body = body.Replace("{SharedBy}", SharedBy);
            body = body.Replace("{SharedByUser}", SharedByUser);
            body = body.Replace("{Url}", "http://justbere.com/Account/Login");
            return body;
        }


        //Email to registered user
        private int SendSharedEmailToNotRegisteredUser(string subject, string email, string TransactionId,string SharedBy, string SharedByUser)
        {
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

            mm.To.Add(email);
            mm.Priority = MailPriority.High;

            mm.Subject = "Shared Transaction- Just BE.";
            mm.Body = this.NotRegisteredUserEmail(email, TransactionId,SharedBy, SharedByUser);
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

        private string NotRegisteredUserEmail(string Email, string TransactionId,string SharedBy, string SharedByUser)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/NotRegisteredUsers.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Email}", Email);
            //body = body.Replace("{UserName}", userName);
            body = body.Replace("{TransactionId}", TransactionId);
            body = body.Replace("{SharedBy}", SharedBy);
            body = body.Replace("{SharedByUser}", SharedByUser);
            body = body.Replace("{Url}", "https://www.justbere.com/Account/Register");
            return body;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveSharing(string ClientID, string TransactionID)
        {
            string url = Url.Action("ShareDeal", "MyDeal", new { TransactionID = TransactionID });
            int ShareCount = 0;
            TempData["ErrMsg"] = ObjDal.RemoveSharing(TransactionID, ClientID, out ShareCount);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
                return Json(new { success = true, url = url, sharedcount = ShareCount, message = "done" });
            }
            return Json(new { success = false, url = url, sharedcount = 1, message = "error" });
            //string url = Url.Action("ManageDeal", "Agent", new { area = "Coordinator", ClientID = ClientID, TransactionID = TransactionID,AgentID=AgentID });
            //return Json(new { success = true, url = url });
            //return RedirectToAction("CreateDeal", new { Email = Email });

        }
        [HttpPost]
        public ActionResult List(MultipleDealClientManageModel Itemdata)
        {
            if (ModelState.IsValid)
            {
                return Json("Thanks for your submission, we will be in touch shortly ", JsonRequestBehavior.AllowGet);

            }
            var Client = UserManager.FindById(User.Identity.GetUserId());
            MultipleDealClientManageModel Objmodel = new MultipleDealClientManageModel();
            Objmodel.TranDDL = ObjDal.TransactionDDL(Client.Id);

            return Json("* Please fill all fields", JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Status(string TransactionID, string ClientID = "", string AgentID = "")
        {
            //ViewBag.ActiveURL = "/client/mydeal/list";
            ViewBag.ActiveURL = "DealList";
            ViewBag.TransactionID = TransactionID;
            ViewBag.ClientID = ClientID;
            ViewBag.TransactionType = ObjDal.TransactionType(TransactionID);
            ViewBag.VendorType = objVendorDal.GetVendorCategoryDDl();
            ViewBag.loginuser = User.Identity.Name;
            string ClientType = ObjDal.TransactionType(TransactionID);
            string Email = string.Empty;
            string OutEmail;
            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                ObjModel.MstUserClientView = ObjDal.ClientView(TransactionID);
                Email = ObjModel.MstUserClientView.Email;
                ViewBag.ClientID = ObjModel.MstUserClientView.Id;
                var Agent = UserManager.FindById(User.Identity.GetUserId());

                //ObjModel.MultipleDealList = ObjDal.AgentMultipleDeal(Agent.Id, ClientID);
            }


            else
            {
                Email = User.Identity.Name;
                var Client = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.ClientID = Client.Id;
                //ObjModel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
                ObjDal.GetSharedEmail(TransactionID, Email, out OutEmail);
                Email = OutEmail;
            }

            ObjModel.ClosingDate = new ClosingDateView();
            //ObjModel.ClosingDate = ObjDal.ClosingDate(Email);
            if (ClientType == "Buyer")
            {
                ObjModel.DealTimeline = ObjDal.ClientDealTimelineVersion2(Email, TransactionID);
                ObjModel.CompleletedDealTimeline= ObjDal.ClientDealTimelineCompleted(Email, TransactionID);
      
            }
            else
            {
                // ObjModel.DealTimeline = ObjDal.ClientDealTimeline(Email, TransactionID);
                ObjModel.DealTimeline = ObjDal.ClientSellerDealTimelineVersion2(Email, TransactionID);
                ObjModel.CompleletedDealTimeline = ObjDal.ClientSellerDealTimelineCompleted(Email, TransactionID);
            }
            ObjModel.TimelineMissingData = ObjDal.ClientDealTimelineNotPresent(Email, TransactionID);
            //ObjModel.Percentage = ObjDal.Percentage(Email, TransactionID);
            //ObjModel.ChartData = ObjDal.ChartData(Email, TransactionID);
            ObjModel.ClientDoc = ObjDal.ClientDoc(Email, TransactionID);
            ObjModel.MLSListing = ObjDal.MLSListing(Email, TransactionID);
            ObjModel.MstUserAgentView = ObjDal.AgentView(TransactionID);
            ObjModel.MstGalleryList = ObjDal.GetPhotoGalleryList(Email, TransactionID);
            ObjModel.DealVendorList = ObjDal.GetDealVendorListNew(Email, TransactionID); // updated by sonika

            ObjModel.ClientDealDocumentList = objClientDocDal.List(TransactionID);
            ObjModel.Agenda = ObjDal.GetClientAppointments(Email, TransactionID);//Added by Neha New
            ObjModel.KeyInfoLinkList = ObjDal.MstKeyInfoLinkList;
            ObjModel.AppointmentList = objAppointment.GetCalenderListByTransactionIdNew(TransactionID);
            ObjModel.BuildingView = objdalBuilding.GetBuildingByTransaction(TransactionID);
            if (ObjModel.MLSListing.Count() > 0)
            {
                foreach (var item in ObjModel.MLSListing)
                {
                    ObjModel.ClosingDate.ClosingDate = item.ClosingDate;
                    ObjModel.ClosingDate.ListingTypeID = item.ListingTypeID;
                    ObjModel.ClosingDate.URL = item.URL;
                    ObjModel.ClosingDate.Address = item.Address;
                    ObjModel.DisplayListing = ListingDetails(item.MLSID);
                    ObjModel.DisplayListing.ListingSource = "Compass";
                    ObjModel.DisplayListing.MLSID = item.MLSID;
                    ObjModel.DisplayListing.Address = item.Address;


                }
            }

            return View(ObjModel);
        }

        public async Task<ActionResult> SharedHome(string TransactionID, string ClientID = "", string AgentID = "")
        {
            // ViewBag.ActiveURL = "/client/mydeal/list";
            ViewBag.ActiveURL = "DealList";
            ViewBag.TransactionID = TransactionID;
            string Email = string.Empty;
            string OutEmail;
            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                ObjModel.MstUserClientView = ObjDal.ClientView(TransactionID);
                Email = ObjModel.MstUserClientView.Email;
                var Agent = UserManager.FindById(User.Identity.GetUserId());

                //ObjModel.MultipleDealList = ObjDal.AgentMultipleDeal(Agent.Id, ClientID);
            }


            else
            {
                Email = User.Identity.Name;
                var Client = UserManager.FindById(User.Identity.GetUserId());
                //ObjModel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
                ObjDal.GetSharedEmail(TransactionID, Email, out OutEmail);
                Email = OutEmail;
            }

            ObjModel.ClosingDate = new ClosingDateView();
            ObjModel.DealTimeline = ObjDal.ClientDealTimeline(Email, TransactionID);
            ObjModel.TimelineMissingData = ObjDal.ClientDealTimelineNotPresent(Email, TransactionID);
            ObjModel.ClientDoc = ObjDal.ClientDoc(Email, TransactionID);
            ObjModel.MLSListing = ObjDal.MLSListing(Email, TransactionID);
            ObjModel.MstUserAgentView = ObjDal.AgentView(TransactionID);
            ObjModel.MstGalleryList = ObjDal.GetPhotoGalleryList(Email, TransactionID);
            ObjModel.KeyInfoLinkList = ObjDal.MstKeyInfoLinkList;
            if (ObjModel.MLSListing.Count() > 0)
            {
                foreach (var item in ObjModel.MLSListing)
                {
                    ObjModel.ClosingDate.ClosingDate = item.ClosingDate;
                    ObjModel.ClosingDate.ListingTypeID = item.ListingTypeID;
                    ObjModel.ClosingDate.URL = item.URL;
                    ObjModel.ClosingDate.Address = item.Address;
                    ObjModel.DisplayListing = ListingDetails(item.MLSID);
                    ObjModel.DisplayListing.ListingSource = "Compass";
                    ObjModel.DisplayListing.MLSID = item.MLSID;
                    ObjModel.DisplayListing.Address = item.Address;

                }
            }

            return View(ObjModel);
        }
        public async Task<ActionResult> ClosingDate(string TransactionID, string ClientID = "", string AgentID = "")
        {
            ViewBag.Current = "Index";
            string Email = string.Empty;
            string OutEmail;
            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                ObjModel.MstUserClientView = ObjDal.ClientView(TransactionID);
                Email = ObjModel.MstUserClientView.Email;
                var Agent = UserManager.FindById(User.Identity.GetUserId());

                //ObjModel.MultipleDealList = ObjDal.AgentMultipleDeal(Agent.Id, ClientID);
            }


            else
            {
                Email = User.Identity.Name;
                var Client = UserManager.FindById(User.Identity.GetUserId());
                ObjDal.GetSharedEmail(TransactionID, Email, out OutEmail);
                Email = OutEmail;
                //ObjModel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
            }

            ObjModel.ClosingDate = new ClosingDateView();
            //ObjModel.ClosingDate = ObjDal.ClosingDate(Email);
            ObjModel.DealTimeline = ObjDal.ClientDealTimeline(Email, TransactionID);
            ObjModel.TimelineMissingData = ObjDal.ClientDealTimelineNotPresent(Email, TransactionID);
            //ObjModel.Percentage = ObjDal.Percentage(Email, TransactionID);
            //ObjModel.ChartData = ObjDal.ChartData(Email, TransactionID);
            ObjModel.ClientDoc = ObjDal.ClientDoc(Email, TransactionID);
            ObjModel.MLSListing = ObjDal.MLSListing(Email, TransactionID);
            ObjModel.MstUserAgentView = ObjDal.AgentView(TransactionID);
            ObjModel.MstGalleryList = ObjDal.GetPhotoGalleryList(Email, TransactionID);
            ObjModel.KeyInfoLinkList = ObjDal.MstKeyInfoLinkList;
            if (ObjModel.MLSListing.Count() > 0)
            {
                foreach (var item in ObjModel.MLSListing)
                {
                    ObjModel.ClosingDate.ClosingDate = item.ClosingDate;
                    ObjModel.ClosingDate.ListingTypeID = item.ListingTypeID;
                    ObjModel.ClosingDate.URL = item.URL;
                    ObjModel.ClosingDate.Address = item.Address;
                    ObjModel.DisplayListing = ListingDetails(item.MLSID);
                    ObjModel.DisplayListing.ListingSource = "Compass";
                    ObjModel.DisplayListing.MLSID = item.MLSID;
                    ObjModel.DisplayListing.Address = item.Address;
                }
            }

            return View(ObjModel);
        }

        [HttpGet]
        public JsonResult Chart(string TransactionID)
        {
            string Email = string.Empty;
            string OutEmail;
            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                ObjModel.MstUserClientView = ObjDal.ClientView(TransactionID);
                ObjModel.ChartData = ObjDal.ChartData(ObjModel.MstUserClientView.Email, TransactionID);

            }
            else
            {
                Email = User.Identity.Name;
                ObjDal.GetSharedEmail(TransactionID, Email, out OutEmail);
                Email = OutEmail;
                ObjModel.ChartData = ObjDal.ChartData(Email, TransactionID);

            }

            //ObjModel.ChartData = ObjDal.ChartData(User.Identity.Name, TransactionID);
            return Json(ObjModel.ChartData, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Gallery(string MLSID)
        {
            ObjModel.SearchResults = new Root();
            ObjModel.SearchResults = await GetMLSInfo(MLSID);
            return PartialView("pvGallery", ObjModel);
        }

        public ActionResult MyDocuments(string TransactionID, string ClientID = "", string AgentID = "")
        {
            ViewBag.Current = "Index";
            string Email = string.Empty;
            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                ObjModel.MstUserClientView = ObjDal.ClientView(TransactionID);
                Email = ObjModel.MstUserClientView.Email;
                var Agent = UserManager.FindById(User.Identity.GetUserId());
            }
            else
            {
                Email = User.Identity.Name;
                var Client = UserManager.FindById(User.Identity.GetUserId());
            }
            ObjModel.ClientDoc = ObjDal.ClientDoc(Email, TransactionID);
            return View(ObjModel);
        }

        public static async Task<Root> GetMLSInfo(string MLSID)
        {
            Root ObjviewModel = new Root();
            WebClient wclient = new WebClient();
            string MLSURL = "https://queryservicec.placester.net/search?";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["mls_id"] = MLSID;
            query["region_id"] = "va_dc_md";

            uriBuilder.Query = query.ToString();
            MLSURL = uriBuilder.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(MLSURL);
                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<Root>(result);
                ObjviewModel = list;
            }
            return ObjviewModel;
        }

        public ActionResult HelpRequest()
        {
            return PartialView("pvEnquiry");
        }

        public ActionResult Enquiry()
        {
            return PartialView("pvEnquiry");
        }

        [HttpPost]
        public ActionResult Enquiry(utblMstArticleEnquire ItemData)
        {
            if (string.IsNullOrEmpty(ItemData.Name) ||
                string.IsNullOrEmpty(ItemData.Email) ||
                string.IsNullOrEmpty(ItemData.Message)
                )
            {
                return Json("* Please fill all fields", JsonRequestBehavior.AllowGet);

            }
            TempData["ErrMsg"] = ObjEnquire.ArticleEnquire(ItemData);
            return Json("Thanks for your submission, we will be in touch shortly ", JsonRequestBehavior.AllowGet);

        }


        public async Task<ActionResult> VendorList(string SearchTerm = "")
        {

            SearchResponse SR = new SearchResponse();
            var client = new Yelp.Api.Client("JcybcL65orLyIIV-gv6dJg", "z47dDQNd8ljNsLgChVzeDWmqQe0T7KxFqc3IKLLYqzUQllewbi3TFRDO1Os3B1oV");
            if (!string.IsNullOrEmpty(SearchTerm))
                SR = await client.SearchBusinessesAllAsync(SearchTerm, 38.885158, -76.996507);
            else
                SR = await client.SearchBusinessesAllAsync("Lenders", 38.885158, -76.996507);


            if (Request.IsAjaxRequest())
            {
                return PartialView("pvVendorList", SR);
            }

            return View(SR);
        }



        [AllowAnonymous]
        public ActionResult Vendors()
        {
            ViewBag.ActiveURL = "Vendors";
            objCategoryModel.VendorCategoryList = objCategoryDal.GetVendorTypes();
            return View(objCategoryModel);
        }
        [AllowAnonymous]
        public ActionResult VendorsList(string Vendor)
        {
            ViewBag.Title = objCategoryDal.GetVendorCategoryByID(Vendor).Name;
            ViewBag.ActiveURL = "Vendors";
            ViewBag.DisplayType = objCategoryDal.GetVendorCategoryByID(Vendor).DisplayType;
            objVendorModel.VendorList = objVendorDal.GetVendorByVendorType(Vendor);
            return View(objVendorModel);
        }
        [AllowAnonymous]
        public ActionResult Details(string Vendor)
        {
         
            ViewBag.ActiveURL = "Vendors";
            ViewBag.VendorId = Vendor;
            objVendorDetails = objVendorDal.GetVendorByVendorID(Vendor);
            ViewBag.VendorTitle = objVendorDetails.Title;
            return View(objVendorDetails);
        }

     
        public ActionResult VendorReview(string Vendor,string Title)
        {
            VendorsReview Model = new VendorsReview();
            var Client = UserManager.FindById(User.Identity.GetUserId());
            Model.ClientId = Client.Id;
            Model.VendorId = Vendor;
            Model.UpdatedOn = System.DateTime.Now;
            ViewBag.Title = Title;
            return PartialView("pvReview", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult VendorReview(VendorsReview ItemData)
        {
            dalVendorReviews ObjDal = new dalVendorReviews();
            var Client = UserManager.FindById(User.Identity.GetUserId());
            if (ItemData.Rating==0 ) { return Json("Please rate the vendor.", JsonRequestBehavior.AllowGet);}
            TempData["ErrMsg"] = ObjDal.Save(ItemData);
            if (TempData["ErrMsg"].ToString() == "0")
            {
                return Json("Thank you!", JsonRequestBehavior.AllowGet);
                

            }


            return Json("Thank you!", JsonRequestBehavior.AllowGet);

        }

        [AllowAnonymous]
        public ActionResult Staging()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Moving()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Painting()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Settlement()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Appraiser()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Design()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Concierge()
        {
            ViewBag.ActiveURL = "Concierge";
            MultipleDealClientManageModel Objmodel = new MultipleDealClientManageModel();
            Objmodel.KeyInfoLinkList = ObjDal.MstKeyInfoLinkList;
            return View(Objmodel);
        }



        #region ProfilePic
        [AllowAnonymous]
        public ActionResult UserDetails()
        {
            dalUser objUser = new dalUser();
            var model = new UserProfileViewModel();
            model = objUser.GetUserDetails(User.Identity.Name);
            return PartialView("pvUserDetails", model);
        }
        #endregion



        public ActionResult FAQ()
        {
            ViewBag.ActiveURL = "FAQ";
            return View();
        }
        public JsonResult getData(string term)
        {
            List<string> getValues = ObjDal.GetAutoCompleteClientList(term).ToList();

            // Return the result set as JSON
            return Json(getValues, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult Jay()
        {
            // ViewBag.ActiveURL = "/client/mydeal/jay";
            MstAgentView ObjModel = new MstAgentView();
            dalAgentProfile objDal = new dalAgentProfile();
            dalMstAgentPhotoGallery objMediaDal = new dalMstAgentPhotoGallery();
            ObjModel.UserGalleryList = objMediaDal.GetUserGalleryList("b778ead6-da99-41dc-b004-d65a788f3a11");
            ViewBag.ActiveURL = "DealAgentListJay";
            return View(ObjModel);
        }


        public ActionResult JayContact()
        {
            utblMstContact Model = new utblMstContact();
            var Client = UserManager.FindById(User.Identity.GetUserId());
            Model.ContactPerson = Client.Name + ' ' + Client.LastName;
            Model.Email = Client.Email;
            Model.ContactDate = System.DateTime.Now;
            return PartialView("pvJayContact", Model);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult JayContact(utblMstContact ItemData)
        {
            dalMstContact ObjDal = new dalMstContact();
            var Client = UserManager.FindById(User.Identity.GetUserId());

            //if (string.IsNullOrEmpty(ItemData.ContactPerson) ||
            //    string.IsNullOrEmpty(ItemData.Email) ||
            //    string.IsNullOrEmpty(ItemData.Message)
            //    )
            //{
            //    return Json("* Please type message to send", JsonRequestBehavior.AllowGet);

            //}
            //TempData["ErrMsg"] = ObjDal.ContactInfo(ItemData);
            //if (TempData["ErrMsg"].ToString() == "0")
            //{
            //    int Status = SendEmail(ItemData.ContactPerson, ItemData.Email, Client.Phone, ItemData.Message, "Jay", "jay@justbere.com");
            //    if (Status == 0)
            //    {
            //        return Json("Thanks for your submission, agent will be in touch shortly.", JsonRequestBehavior.AllowGet);
            //    }

            //}


            //return Json("Thanks for your submission, agent will be in touch shortly.", JsonRequestBehavior.AllowGet);

            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);

            if (response.Success)
            {
                if (string.IsNullOrEmpty(ItemData.ContactPerson) ||
               string.IsNullOrEmpty(ItemData.Email) ||
               string.IsNullOrEmpty(ItemData.Message)
               )
                {
                    return Json("* Please fill all fields", JsonRequestBehavior.AllowGet);
                }

                else
                {
                    TempData["ErrMsg"] = ObjDal.ContactInfo(ItemData);
                    if (TempData["ErrMsg"].ToString() == "0")
                    {
                        int Status = SendEmail(ItemData.ContactPerson, ItemData.Email, Client.Phone, ItemData.Message, "Jay", "jay@justbere.com");
                        if (Status == 0)
                        {
                            return Json("Thanks for your submission, agent will be in touch shortly.", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json("Please complete captcha challenge", JsonRequestBehavior.AllowGet);

        }


        [AllowAnonymous]
        public ActionResult MyAgent(string AgentID, string AgentName)
        {
            MstAgentView ObjModel = new MstAgentView();
            dalAgentProfile objDal = new dalAgentProfile();
            dalMstAgentPhotoGallery objMediaDal = new dalMstAgentPhotoGallery();
            ObjModel.MstAgentViewModel = objDal.AgentView(AgentID);
            ObjModel.UserGalleryList = objMediaDal.GetUserGalleryList(AgentID);
            ViewBag.AgentName = AgentName;
            //ViewBag.ActiveURL = "/client/mydeal/myagent";
            ViewBag.ActiveURL = "DealAgentList"+ AgentName;
            ViewBag.AgentID = AgentID;
            return View(ObjModel);
        }
        public ActionResult AgentContactPage(string AgentID)
        {
            utblMstContact Model = new utblMstContact();
            var Client = UserManager.FindById(User.Identity.GetUserId());
            Model.ContactPerson = Client.Name + ' ' + Client.LastName;
            Model.Email = Client.Email;
          
            Model.ContactDate = System.DateTime.Now;
            ViewBag.AgentID = AgentID;
            var Agent= UserManager.FindById(AgentID);
            ViewBag.AgentName = Agent.Name;
            return PartialView("pvAgentContact", Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AgentContactPage(utblMstContact ItemData, string AgentID)
        {
            dalMstContact ObjDal = new dalMstContact();
            var Agent = UserManager.FindById(AgentID);
            var Client = UserManager.FindById(User.Identity.GetUserId());

            //if (string.IsNullOrEmpty(ItemData.ContactPerson) ||
            //    string.IsNullOrEmpty(ItemData.Email) ||
            //    string.IsNullOrEmpty(ItemData.Message)
            //    )
            //{
            //    return Json("* Please type message to send", JsonRequestBehavior.AllowGet);

            //}
            //TempData["ErrMsg"] = ObjDal.ContactInfo(ItemData);
            //if (TempData["ErrMsg"].ToString() == "0")
            //{
            //    int Status = SendEmail(ItemData.ContactPerson, ItemData.Email, Client.Phone, ItemData.Message, Agent.Name, Agent.Email);
            //    if (Status == 0)
            //    {
            //        return Json("Thanks for your submission, we will be in touch shortly. ", JsonRequestBehavior.AllowGet);
            //    }

            //}
            //return Json("Thanks for your submission, we will be in touch shortly", JsonRequestBehavior.AllowGet);




            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);

            if (response.Success)
            {
                if (string.IsNullOrEmpty(ItemData.ContactPerson) ||
               string.IsNullOrEmpty(ItemData.Email) ||
               string.IsNullOrEmpty(ItemData.Message)
               )
                {
                    return Json("* Please fill all fields", JsonRequestBehavior.AllowGet);
                }

                else
                {
                    TempData["ErrMsg"] = ObjDal.ContactInfo(ItemData);
                    if (TempData["ErrMsg"].ToString() == "0")
                    {
                        int Status = SendEmail(ItemData.ContactPerson, ItemData.Email, Client.Phone, ItemData.Message, Agent.Name, Agent.Email);
                        if (Status == 0)
                        {
                            return Json("Thanks for your submission, agent will be in touch shortly.", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json("Please complete captcha challenge", JsonRequestBehavior.AllowGet);
        }


        //----------------------Client Deal Document -------------------------------------------
        public JsonResult ClientDealDocumentList(string TransactionId)
        {

            return Json(objClientDocDal.List(TransactionId), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult AddClientDealDocument(ClientDealDocuments document, HttpPostedFileBase DocFile)
        //{
        //    string errorMessage = "";
        //    if (DocFile != null && DocFile.ContentLength > 0)
        //    {
        //        var guid = Guid.NewGuid().ToString().Substring(0, 4);
        //        string Filename= guid + DocFile.FileName;
        //        document.DocFile = Filename;
        //         errorMessage = objClientDocDal.Save(document);
        //        if (errorMessage == "0")
        //        {
        //            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + Filename));
        //            DocFile.SaveAs(path);
        //        }
        //    }
        //    return Json(errorMessage, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult AddClientDealDocument(ClientDealDocumentsView document, HttpPostedFileBase DocFile)
        {
            string errorMessage = "";
            if (DocFile != null && DocFile.ContentLength > 0)
            {
                var guid = Guid.NewGuid().ToString().Substring(0, 4);
                string Filename = guid + DocFile.FileName;
                document.DocFile = Filename;
                errorMessage = objClientDocDal.Save(document);
                if (errorMessage == "0")
                {
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + Filename));
                    DocFile.SaveAs(path);
                }
            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetClientDealDocumentByID(int ID)
        //{
        //    var Employee = objClientDocDal.GetClientDealDocumentByID(ID);
        //    return Json(Employee, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetClientDealDocumentByID(int ID, string ClientDocId)
        {
            var Employee = objClientDocDal.GetClientDealDocumentByID(ID, ClientDocId);
            return Json(Employee, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult UpdateClientDealDocument(ClientDealDocuments document, HttpPostedFileBase DocFile,string ExistingFile)
        //{
        //    string errorMessage = "";
        //    if (DocFile != null && DocFile.ContentLength > 0)
        //    {
        //        var guid = Guid.NewGuid().ToString().Substring(0, 4);
        //        string Filename = guid + DocFile.FileName;
        //        document.DocFile = Filename;
        //        errorMessage = objClientDocDal.Edit(document);
        //        if (errorMessage == "0")
        //        {
        //            string ExistingFilePath = Server.MapPath("~/UploadedFiles/TrackDeal/" + ExistingFile);
        //            FileInfo file = new FileInfo(ExistingFilePath);
        //            if (file.Exists)
        //                file.Delete();

        //            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + Filename));
        //            DocFile.SaveAs(path);
        //        }
        //    }
        //    else
        //    {
        //        document.DocFile = ExistingFile;
        //        errorMessage = objClientDocDal.Edit(document);
        //    }
        //    return Json(errorMessage, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult UpdateClientDealDocument(ClientDealDocumentsView document, HttpPostedFileBase DocFile, string ExistingFile)
        {
            string errorMessage = "";
            if (DocFile != null && DocFile.ContentLength > 0)
            {
                var guid = Guid.NewGuid().ToString().Substring(0, 4);
                string Filename = guid + DocFile.FileName;
                document.DocFile = Filename;
                errorMessage = objClientDocDal.Edit(document);
                if (errorMessage == "0")
                {
                    string ExistingFilePath = Server.MapPath("~/UploadedFiles/TrackDeal/" + ExistingFile);
                    FileInfo file = new FileInfo(ExistingFilePath);
                    if (file.Exists)
                        file.Delete();

                    var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + Filename));
                    DocFile.SaveAs(path);
                }
            }
            else
            {
                document.DocFile = ExistingFile;
                errorMessage = objClientDocDal.Edit(document);
            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteClientDealDocument(string ID)
        {
            string ExistingFilePath = Server.MapPath("~/UploadedFiles/TrackDeal/" + objClientDocDal.GetClientFileName(ID));
            FileInfo file = new FileInfo(ExistingFilePath);
            string errorMessage = objClientDocDal.Delete(ID);
            if (errorMessage == "0")
            {
                if (file.Exists)
                    file.Delete();
            }

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ClientDocumentautocomplete(string term, string ClientId, string TransactionId)
        {

            objCategoryModel.ClientDocumentAutoComplete = objCategoryDal.ClientDocumentAutoComplete(term, ClientId, TransactionId);
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in objCategoryModel.ClientDocumentAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.Description));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
        //-------------------------Client Deal Document----------------------------------------


        //----------------------Client Vendor added by sonika -------------------------------------------
        public JsonResult ClientVendorList(string TransactionId)
        {

            ViewBag.loginuser = User.Identity.Name;
            string Email = string.Empty;
            string OutEmail;
            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                ObjModel.MstUserClientView = ObjDal.ClientView(TransactionId);
                Email = ObjModel.MstUserClientView.Email;
                var Agent = UserManager.FindById(User.Identity.GetUserId());

                //ObjModel.MultipleDealList = ObjDal.AgentMultipleDeal(Agent.Id, ClientID);
            }


            else
            {
                Email = User.Identity.Name;
                var Client = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.ClientID = Client.Id;
                //ObjModel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
                ObjDal.GetSharedEmail(TransactionId, Email, out OutEmail);
                Email = OutEmail;
            }
            ObjModel.DealVendorList = ObjDal.GetDealVendorListNew(Email, TransactionId);
            return Json(ObjModel.DealVendorList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddClientDealVendor(FormCollection form, HttpPostedFileBase VenFile)
        {
            string errorMessage = "";

            string Email = string.Empty;
            string OutEmail;
            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                ObjModel.MstUserClientView = ObjDal.ClientView(form["TransactionID"].ToString());
                Email = ObjModel.MstUserClientView.Email;
                var Agent = UserManager.FindById(User.Identity.GetUserId());

                //ObjModel.MultipleDealList = ObjDal.AgentMultipleDeal(Agent.Id, ClientID);
            }


            else
            {
                Email = User.Identity.Name;
                var Client = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.ClientID = Client.Id;
                //ObjModel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
                ObjDal.GetSharedEmail(form["TransactionID"].ToString(), Email, out OutEmail);
                Email = OutEmail;
            }

            Vendor vendor = new Vendor();
            vendor.Category_Id = form["VendorType"].ToString();
            vendor.Title = form["txtTitleV"].ToString();
            vendor.SubTitle = form["SubTitle"].ToString();
            vendor.Email = form["Email"].ToString();
            vendor.About = form["About"].ToString();
            vendor.WebsiteLink = form["Websitelink"].ToString();
            vendor.Phone = form["Phone"].ToString();
            vendor.Location = form["Location"].ToString();

            if (VenFile != null && VenFile.ContentLength > 0)
            {
                var guid = Guid.NewGuid().ToString().Substring(0, 4);
                string Filename = guid + VenFile.FileName;
                vendor.VendorImage = Filename;
                errorMessage = objVendorDal.SaveClientVendorClient(vendor, Email, form["TransactionID"].ToString());
                if (errorMessage == "0")
                {
                    var path = string.Concat(Server.MapPath("~/img/vendors/" + Filename));
                    VenFile.SaveAs(path);
                    return Json(errorMessage, JsonRequestBehavior.AllowGet);
                }
            }
            errorMessage = objVendorDal.SaveClientVendorClient(vendor, Email, form["TransactionID"].ToString());
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClientDealVendorByID(string ID)
        {
            var vendor = objVendorDal.GetVendorByIDClient(ID);
            return Json(vendor, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateClientDealVendor(FormCollection form, HttpPostedFileBase VenFile)
        {
            string Email = string.Empty;
            string OutEmail;
            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                ObjModel.MstUserClientView = ObjDal.ClientView(form["TransactionID"].ToString());
                Email = ObjModel.MstUserClientView.Email;
                var Agent = UserManager.FindById(User.Identity.GetUserId());

                //ObjModel.MultipleDealList = ObjDal.AgentMultipleDeal(Agent.Id, ClientID);
            }


            else
            {
                Email = User.Identity.Name;
                var Client = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.ClientID = Client.Id;
                //ObjModel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
                ObjDal.GetSharedEmail(form["TransactionID"].ToString(), Email, out OutEmail);
                Email = OutEmail;
            }

            string errorMessage = "";
            Vendor vendor = new Vendor();
            vendor.Category_Id = form["VendorType"].ToString();
            vendor.Title = form["txtTitleV"].ToString();
            vendor.SubTitle = form["SubTitle"].ToString();
            vendor.Email = form["Email"].ToString();
            vendor.About = form["About"].ToString();
            vendor.WebsiteLink = form["Websitelink"].ToString();
            vendor.Phone = form["Phone"].ToString();
            vendor.Location = form["Location"].ToString();
            vendor.VendorId = form["Id"].ToString();
            string CreatedBy = Email;

            if (VenFile != null && VenFile.ContentLength > 0)
            {

                string ExistingFilePath = Server.MapPath("~/img/vendors/" + vendor.VendorImage);
                FileInfo file = new FileInfo(ExistingFilePath);

                var guid = Guid.NewGuid().ToString().Substring(0, 4);

                vendor.VendorImage = vendor.VendorId + guid + Path.GetExtension(VenFile.FileName);
                errorMessage = objVendorDal.EditVendorClient(vendor, CreatedBy, form["TransactionID"].ToString(), form["DealVendorId"].ToString());
                if (errorMessage == "0")
                {
                    if (file.Exists)
                        file.Delete();

                    string ext = guid + Path.GetExtension(VenFile.FileName);
                    var fileName = vendor.VendorId;
                    var path = string.Concat(Server.MapPath("~/img/vendors/" + fileName + ext));
                    VenFile.SaveAs(path);
                    return Json(errorMessage, JsonRequestBehavior.AllowGet);
                }

            }
            vendor.VendorImage = form["txtVenImage"].ToString();
            errorMessage = objVendorDal.EditVendorClient(vendor, CreatedBy, form["TransactionID"].ToString(), form["DealVendorId"].ToString());

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteClientDealVendor(string ID)
        {
            string errorMessage = "";
            //string ExistingFilePath = Server.MapPath("~/img/vendors/" + objVendorDal.GetVendorByID(ID).VendorImage);
            //FileInfo file = new FileInfo(ExistingFilePath);
            errorMessage = objVendorDal.DeleteNew(ID);
            //if (errorMessage == "0")
            //{
            //    if (file.Exists)
            //        file.Delete();

            //}

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult autocomplete(string term)
        {

            objCategoryModel.ListSearchAutoComplete = objCategoryDal.VendorTypeAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objCategoryModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ClientVendorautocomplete(string term, string ClientId, string VendorType)
        {

            objCategoryModel.ClientVendorsListSearchAutoComplete = objCategoryDal.ClientVendorAutoComplete(term, ClientId, VendorType);
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in objCategoryModel.ClientVendorsListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.Phone + "}" + item.Email + "}" + item.CatgoryName + "}" + item.Company + "}" + item.WebsiteLink + "}" + item.Location + "}" + item.VendorImage + "}" + item.Description));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        //-------------------------Client Vendor----------------------------------------



        //----------------------To Do list -------------------------------------------
        public JsonResult BindToDoList(string TransactionID)
        {
            string SearchTerm = ""; int PageNo = 1; int PageSize = 20;
            ClientToDoListViewModel model = new ClientToDoListViewModel();
            int TotalRow;
            string ClientId = objToDoList.GetClientIdByTransactionId(TransactionID);
            model.ClientToDoListAll = objToDoList.ClientToDoListViewGetPaged(TransactionID,ClientId, PageNo, PageSize, out TotalRow, SearchTerm);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            return Json(model.ClientToDoListAll, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddToDoList(ClientToDoListView model,string TransactionID)
        {
            string errorMessage = "";
            string ClientId ="";
            if (!User.IsInRole("Client"))
                ClientId = objToDoList.GetClientIdByTransactionId(TransactionID);
            else
                ClientId = User.Identity.GetUserId().ToString();
            model.TransactionID = TransactionID;
                errorMessage = objToDoList.Save(model, User.Identity.GetUserId().ToString(), ClientId);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditToDoList(string Task, int Id)
        {
            string errorMessage = "";
            errorMessage = objToDoList.Edit(Task, Id);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTask(int ID)
        {

            string errorMessage = objToDoList.Delete(ID);

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MarkAsDone(int ID, bool IsDone)
        {

            string errorMessage = objToDoList.MarkAsDone(ID, IsDone);

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        //-------------------------To Do list----------------------------------------

        private string PopulateBody(string CPN, string Email, string CPPhone, string Message, string AgentName)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/AgentMessage.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{AgentName}", AgentName);
            body = body.Replace("{CPN}", CPN);
            body = body.Replace("{Email}", Email);
            body = body.Replace("{Phone}", CPPhone);
            body = body.Replace("{Message}", Message);
            return body;
        }

        private int SendEmail(string CPN, string Email, string CPPhone, string Message, string AgentName, string AgentEmail)
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
            client.Port = 25;
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = false;
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(settings.Smtp.Network.UserName, "Just BE");
            StringBuilder mailbody = new StringBuilder();
            mm.To.Add(AgentEmail);
            mm.CC.Add("jay@jaybauergroup.com");
            mm.Priority = MailPriority.High;
            mm.Subject = "Message from client";
            mm.Body = this.PopulateBody(CPN, Email, CPPhone, Message, AgentName);
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




        #region MLSIDLOOKUP

        public MLSListingDetails ListingDetails(string mls_id)
        {
            MLSListingDetails Obj = new MLSListingDetails();

            string MLSURL = "https://www.compass.com/listing/" + mls_id + "/view";
            try
            {
                var request = WebRequest.Create(MLSURL);
                var response = request.GetResponse();

                var content = "";
                string stringResponse = "";
                string ImgUrl = "";
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);
                    stringResponse = reader.ReadToEnd();
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(stringResponse);

                var res = document.DocumentNode.SelectSingleNode("//html");
                content = res.InnerHtml;

                //var script = document.DocumentNode.Descendants("script").ToArray();
                //dynamic json = JsonConvert.DeserializeObject(script[3].InnerHtml);
                //var result = json.SelectToken("mainEntity.image").ToString();
                //dynamic json1 = JsonConvert.DeserializeObject(result);
                //var result1 = json1[0]["url"].Value;

                var metaTags = document.DocumentNode.SelectNodes("//meta");

                if (metaTags != null)
                {
                    foreach (var tag in metaTags)
                    {

                        var tagContent = tag.Attributes["content"];
                        var tagProperty = tag.Attributes["property"];
                        if (tagProperty != null && tagContent != null)
                        {
                            if (tagProperty.Value.ToString() == "og:image")
                            {
                                ImgUrl = tagContent.Value;
                                break;
                            }
                        }
                    }
                }
                // ImgUrl = result1;
                Obj.Address = "";
                Obj.ImageURL = ImgUrl;
            }
            catch (Exception)
            {
                Obj.Address = "";
                Obj.ImageURL = "";

            }
            return Obj;
        }

        #endregion


        ///--------------------------Event -----------------------------------//
        ///
        public JsonResult ClientEventList(string TransactionId)
        {

            ViewBag.loginuser = User.Identity.Name;
            string Email = string.Empty;
            string OutEmail;
            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                ObjModel.MstUserClientView = ObjDal.ClientView(TransactionId);
                Email = ObjModel.MstUserClientView.Email;
                var Agent = UserManager.FindById(User.Identity.GetUserId());

                //ObjModel.MultipleDealList = ObjDal.AgentMultipleDeal(Agent.Id, ClientID);
            }


            else
            {
                Email = User.Identity.Name;
                var Client = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.ClientID = Client.Id;
                //ObjModel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
                ObjDal.GetSharedEmail(TransactionId, Email, out OutEmail);
                Email = OutEmail;
            }
            ObjModel.AppointmentList = objAppointment.GetCalenderListByTransactionIdNew(TransactionId);
            return Json(ObjModel.AppointmentList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult autocompleteEvent(string term)
        {

            objCategoryModel.EventListSearchAutoComplete = objAppointment.EventListSearch(term);
            var result = new List<KeyValuePair<int, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objCategoryModel.EventListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<int, string>(item.Id, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }


        public ActionResult AddClientDealEvent(FormCollection form)
        {
            string errorMessage = "0";
            try
            {
                string Email = string.Empty;
                string AgentEmail = string.Empty;
                string Id = string.Empty;
                string OutEmail;
                bool RepeatEvent = false;
                int RepeatInterval = 0;
                string RepeatFrequency = form["recurringtype"].ToString();
                if (!string.IsNullOrEmpty(RepeatFrequency))
                    RepeatEvent = true;
                RepeatInterval = Convert.ToInt32(form["quantity"]);
                ObjModel.MstUserAgentView = ObjDal.AgentView(form["TransactionID"].ToString());
                ObjModel.MstUserClientView = ObjDal.ClientView(form["TransactionID"].ToString());

                if (User.IsInRole("Agent") || User.IsInRole("Admin"))
                {

                    Email = ObjModel.MstUserClientView.Email;
                    var Agent = UserManager.FindById(User.Identity.GetUserId());
                    Id = ObjModel.MstUserClientView.Id;
                    AgentEmail = Agent.Email;
                }
                else
                {
                    AgentEmail = ObjModel.MstUserAgentView.Email;
                    Email = User.Identity.Name;
                    var Client = UserManager.FindById(User.Identity.GetUserId());
                    ViewBag.ClientID = Client.Id;
                    Id = Client.Id;
                    //ObjModel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
                    ObjDal.GetSharedEmail(form["TransactionID"].ToString(), Email, out OutEmail);
                    Email = OutEmail;
                }


                utblMstAppointment appointment = new utblMstAppointment();

                appointment.IsContingency = false;
                appointment.IsEmailSent = false;
                appointment.StartDate = Convert.ToDateTime(form["txtStartDate"].ToString());
                appointment.EndDate = Convert.ToDateTime(form["txtEndDate"].ToString());
                appointment.Description = form["EventTitle"].ToString();
                appointment.AgentID = Id;
                appointment.Email = Email;
                appointment.color = "#294780";
                appointment.TransactionID = form["TransactionID"].ToString();
                appointment.RepeatEvent = RepeatEvent;
                appointment.RepeatFrequency = RepeatFrequency;
                appointment.RepeatInterval = RepeatInterval;

                appointment.createdby = User.Identity.GetUserId();
                errorMessage = objAppointment.AddClientEvent(appointment);

                //google calendar code start
                if (objAppointment.emailPreferences(appointment.Email))
                {
                    objCalenderManageModel.MstAgentClientNameSelect = objAppointment.GetNameEmail(Id);
                    string SDate = appointment.StartDate.ToString("dd MMM yyyy hh:mm tt");
                    string EDate = appointment.EndDate.ToString("dd MMM yyyy hh:mm tt");
                    string Date = string.Join(" to ", SDate, EDate);

                   //add back
                    // errorMessage = SendEmailConfirmationToken(Email, ObjModel.MstUserClientView.Name, appointment.Description, ObjModel.MstUserAgentView.Name, AgentEmail, Date, ObjModel.MstUserAgentView.PhoneNumber, AgentEmail, RepeatEvent, appointment.RepeatFrequency, appointment.RepeatInterval);
                    

                }
                //google calendar code end


                return Json(errorMessage, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }




        private string SendEmailConfirmationToken(string email, string Name, string EventName, string InviteeName, string InviteeEmail, string EventDate, string AgentPhone, string AgentEmail, bool RepeatEvent, string RepeatFrequency, int RepeatInterval)
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

            mm.To.Add(email);
            // mm.CC.Add(AgentEmail);
            //if (User.IsInRole("Admin"))
            //{

            //    mm.CC.Add(AgentEmail);
            //    mm.CC.Add(User.Identity.Name);

            //}

            mm.Priority = MailPriority.High;

            mm.Subject = "New Event Scheduled";
            mm.Body = this.PopulateBody(Name, EventName, InviteeName, InviteeEmail, EventDate, AgentPhone, RepeatEvent, RepeatFrequency, RepeatInterval);
            mm.IsBodyHtml = true;

            try
            {
                client.Send(mm);
                return "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }





  

        private string PopulateBody(string Name, string EventName, string InviteeName, string InviteeEmail, string EventDate, string AgentPhone, bool RepeatEvent, string RepeatFrequency, int RepeatInterval)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/CalendarClient.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Name}", Name);
            body = body.Replace("{EventName}", EventName);
           // body = body.Replace("{AgentName}", InviteeName);
            //body = body.Replace("{InviteeEmail}", InviteeEmail);
            body = body.Replace("{EventDate}", EventDate);
            if (RepeatEvent)
            {
                string Freq = "";
                switch (RepeatFrequency)
                {
                    case "DAILY":
                        Freq = "Day";
                        break;
                    case "WEEKLY":
                        Freq = "Week";
                        break;
                    case "MONTHLY":
                        Freq = "Month";
                        break;
                    case "YEARLY":
                        Freq = "Year";
                        break;
                    default:
                        Freq = "Day";
                        break;
                }
                body = body.Replace("{Recurrence}", "Recurrence: Repeat every " + RepeatInterval+" " + Freq);
            }
                //  body = body.Replace("{AgentNumber}", AgentPhone);

            return body;
        }

        //private GoogleAuthenticator GetAuthenticator(string Email)
        //{
        //    var authenticator = (GoogleAuthenticator)Session["authenticator"];

        //    if (authenticator == null || !authenticator.IsValid)
        //    {
        //        // Get a new Authenticator using the Refresh Token
        //        var refreshToken = new EFDBContext().utblMstGmailTokens.FirstOrDefault(c => c.UserEmail == Email).RefreshToken;
        //        authenticator = GoogleAuthorizationHelper.RefreshAuthenticator(refreshToken);
        //        Session["authenticator"] = authenticator;
        //    }

        //    return authenticator;
        //}

        public JsonResult GetClientDealEventByID(int ID)
        {
            var eventapp = objAppointment.GetAppointmentByIDNew(ID);
            return Json(eventapp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateClientDealEvent(FormCollection form, HttpPostedFileBase VenFile)
        {
            string errorMessage = "0";


            string Email = string.Empty;
            string AgentEmail = string.Empty;
            string Id = string.Empty;
            string OutEmail;
            bool RepeatEvent = false;
            int RepeatInterval = 0;
            string RepeatFrequency = form["recurringtype"].ToString();
            if (!string.IsNullOrEmpty(RepeatFrequency))
                RepeatEvent = true;
            RepeatInterval = Convert.ToInt32(form["quantity"]);
            ObjModel.MstUserAgentView = ObjDal.AgentView(form["TransactionID"].ToString());
            ObjModel.MstUserClientView = ObjDal.ClientView(form["TransactionID"].ToString());

            if (User.IsInRole("Agent") || User.IsInRole("Admin"))
            {

                Email = ObjModel.MstUserClientView.Email;
                var Agent = UserManager.FindById(User.Identity.GetUserId());
                Id = ObjModel.MstUserClientView.Id;
                AgentEmail = Agent.Email;
            }
            else
            {
                AgentEmail = ObjModel.MstUserAgentView.Email;
                Email = User.Identity.Name;
                var Client = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.ClientID = Client.Id;
                Id = Client.Id;
                //ObjModel.MultipleDealList = ObjDal.ClientMultipleDeal(Client.Id);
                ObjDal.GetSharedEmail(form["TransactionID"].ToString(), Email, out OutEmail);
                Email = OutEmail;
            }


            utblMstAppointment appointment = new utblMstAppointment();
            appointment.Id = Convert.ToInt16(form["Id"].ToString());
            appointment.IsContingency = false;
            appointment.IsEmailSent = false;
            appointment.StartDate = Convert.ToDateTime(form["txtStartDate"].ToString());
            appointment.EndDate = Convert.ToDateTime(form["txtEndDate"].ToString());
            appointment.Description = form["EventTitle"].ToString();
            appointment.AgentID = Id;
            appointment.Email = Email;
            appointment.color = "#294780";
            appointment.TransactionID = form["TransactionID"].ToString();
            appointment.RepeatEvent = RepeatEvent;
            appointment.RepeatFrequency = RepeatFrequency;
            appointment.RepeatInterval = RepeatInterval;
            errorMessage = objAppointment.EditEventClient(appointment);

            ////google calendar code start
            if (objAppointment.emailPreferences(appointment.Email))
            {
                objCalenderManageModel.MstAgentClientNameSelect = objAppointment.GetNameEmail(Id);
                string SDate = appointment.StartDate.ToString("dd MMM yyyy hh:mm tt");
                string EDate = appointment.EndDate.ToString("dd MMM yyyy hh:mm tt");
                string Date = string.Join(" to ", SDate, EDate);

                //    string calendarId = string.Empty;
                //    using (var context = new EFDBContext())
                //    {
                //        //  calendarId = context.utblMstGmailTokens.FirstOrDefault(m => m.UserEmail == Agent.Email).GmailAccount; Commented by Neha
                //        calendarId = context.utblMstGmailTokens.FirstOrDefault(m => m.UserEmail == AgentEmail).GmailAccount;//Added by Neha
                //    }

                //    var model = new CalendarEvent()
                //    {
                //        CalendarId = calendarId,
                //        Title = appointment.Description,
                //        Location = "660 Pennsylvania Ave. SE Suite 300, Washington, DC 20003 ",
                //        StartDate = appointment.StartDate,
                //        EndDate = appointment.EndDate,
                //        Description = appointment.Description,
                //        Attendees = new string[] { Email }
                //    };

                //    //var authenticator = GetAuthenticator(User.Identity.Name);Commented by Neha
                //    var authenticator = GetAuthenticator(AgentEmail);
                //    var service = new GoogleCalendarServiceProxy(authenticator);//Added by Neha
                //    service.UpdateEvent(model);
                 errorMessage = SendEmailConfirmationToken(Email, ObjModel.MstUserClientView.Name, appointment.Description, ObjModel.MstUserAgentView.Name, AgentEmail, Date, ObjModel.MstUserAgentView.PhoneNumber, AgentEmail, RepeatEvent, appointment.RepeatFrequency, appointment.RepeatInterval);

            }
            //google calendar code end
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteClientDealEvent(int ID)
        {
            string errorMessage = "";
            errorMessage = objAppointment.RemoveDealEventNew(ID); //added by sonika



            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProfileImage()
        {
            dalUser objUser = new dalUser();
            var model = new UserProfileViewModel();
            model = objUser.GetUserDetails(User.Identity.Name);




            return Json(model.UserPhotoThumb==null?"": model.UserPhotoThumb, JsonRequestBehavior.AllowGet);
        }


    }
}