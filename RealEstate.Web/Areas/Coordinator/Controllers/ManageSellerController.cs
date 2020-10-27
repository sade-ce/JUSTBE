using HtmlAgilityPack;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using RealEstate.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]

    public class ManageSellerController : Controller
    {
        MstSellerDealManageModel ObjManageModel = new MstSellerDealManageModel();
        dalManageSeller objDal = new dalManageSeller();
        dalMstClientList objClientDetails = new dalMstClientList();
        MstSellerOffer ObjSeller = new MstSellerOffer();
        RootObject ObjviewModel = new RootObject();


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
        #region CreateDeal
        public async Task<ActionResult> CreateDeal(string ClientID, string TransactionID, string AgentID = "")
        {
            string Email = await UserManager.GetEmailAsync(ClientID);
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.StatusList = objDal.StatusList(Email, TransactionID);
            ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            ObjManageModel.TrackDealStatusList = objDal.TrackDealStatusList(Email, TransactionID);
            //ObjManageModel.UserProfile = objDal.GetUserDetails(ClientID);

            ObjManageModel.TrackDealMasterView = new TrackDealMasterView()
            {
                ClientID = ClientID,
                TransactionID = TransactionID,
                AgentID = AgentID
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClientStatusList", ObjManageModel);
            }
            return View(ObjManageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDeal(MstSellerDealManageModel ItemData)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ItemData.MstUserStatusSelect = new UserStatusSelect();
            ItemData.utblMstSellerTrackDeal.Email = ItemData.UserInfo.Email;
            string Email = ItemData.UserInfo.Email;
            if (ModelState.IsValid)
            {

                if (ItemData.ClosingConfig.SellerStatusID == ItemData.utblMstSellerTrackDeal.SellerStatusID)
                {
                    return RedirectToAction("ClosingConfig", new { Email = ItemData.UserInfo.Email, SellerStatusID = ItemData.utblMstSellerTrackDeal.SellerStatusID, ClientID = ItemData.utblMstSellerTrackDeal.ClientID, TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
                }

                TempData["ErrMsg"] = objDal.Save(ItemData.utblMstSellerTrackDeal);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                    if (objDal.emailPreferences(ItemData.UserInfo.Email))
                    {
                        string Date = DateTime.Now.ToString("dd MMM yyyy HH:mm");
                        string EmailID = ItemData.utblMstSellerTrackDeal.Email;
                        ItemData.MstUserStatusSelect = objDal.GetClientStatusfromEmail(Email, ItemData.utblMstSellerTrackDeal.SellerStatusID);
                        ItemData.MstAgentClientNameSelect = objDal.GetNameEmail(ItemData.utblMstSellerTrackDeal.TransactionID);

                        int MailStatus = SendEmailConfirmationToken(EmailID, ItemData.MstUserStatusSelect.ClientName, Date, ItemData.MstUserStatusSelect.Status, ItemData.MstAgentClientNameSelect.AgentName, ItemData.MstAgentClientNameSelect.AgentPhone);
                        if (MailStatus == 0)
                        {

                            TempData["MailMsg"] = 1;
                        }
                        else
                        {
                            TempData["ErrMsg"] = null;
                        }
                    }

                    return RedirectToAction("CreateDeal", new { ClientID = ItemData.utblMstSellerTrackDeal.ClientID, TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
                }
            }
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.StatusList = objDal.StatusList(Email, ItemData.utblMstSellerTrackDeal.TransactionID);
            ObjManageModel.TrackClosingDate = objDal.TrackClosingDate(Email, ItemData.utblMstSellerTrackDeal.TransactionID);
            ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            ObjManageModel.TrackDealStatusList = objDal.TrackDealStatusList(Email, ItemData.utblMstSellerTrackDeal.TransactionID);
            return View(ObjManageModel);
        }
        #endregion
        #region  UploadDealDocument
        public ActionResult UploadDealDocument(string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            string ID = objDal.GetTrackingID(Email, TransactionID);
            ObjManageModel.utblMstSellerTrackDeal = new utblMstSellerTrackDeal();
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView();
            ObjManageModel.utblMstSellerTrackDeal = objDal.GetLiveDealDetailsByID(ID, TransactionID);
            ObjManageModel.MstSellerDocumentList = objDal.SellerDocumentListView(Email, TransactionID);
            ObjManageModel.MstSellerDocList = objDal.SellerDocumentList(Email, TransactionID);
            ObjManageModel.UserProfile = objDal.GetUserDetails(ClientID);
            ObjManageModel.TrackDealMasterView.AgentID = AgentID;

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvUploadDealDocument", ObjManageModel);
            }
            return View(ObjManageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDealDocument(HttpPostedFileBase files, MstSellerDealViewModel ItemData, MstSellerDealManageModel Item)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            if (files != null && files.ContentLength > 0)
            {
                int DocID = ItemData.utblMstSellerTrackDealDoc.SellerDocID;
                ItemData.utblMstSellerTrackDealDoc = new utblMstSellerTrackDealDoc();
                ItemData.utblMstSellerTrackDealDoc.SellerTrackingID = ItemData.utblMstSellerTrackDeal.SellerTrackingID;
                ItemData.utblMstSellerTrackDealDoc.TrackDocFile = Path.GetExtension(files.FileName);
                ItemData.utblMstSellerTrackDealDoc.Email = ItemData.utblMstSellerTrackDeal.Email;
                ItemData.utblMstSellerTrackDealDoc.SellerStatusID = ItemData.utblMstSellerTrackDeal.SellerStatusID;
                ItemData.utblMstSellerTrackDealDoc.SellerDocID = DocID;
                ItemData.utblMstSellerTrackDealDoc.TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID;
                TempData["ErrMsg"] = objDal.SaveSellerDoc(ItemData.utblMstSellerTrackDealDoc);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    var TrackingID = ItemData.utblMstSellerTrackDeal.SellerTrackingID;
                    string ext = Path.GetExtension(files.FileName);
                    var fileName = ItemData.utblMstSellerTrackDealDoc.SellerDealDocID;
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + fileName + ext));
                    files.SaveAs(path);
                    ItemData.MstLiveDealDocList = objDal.GetSellerDealDocList(TrackingID);
                    return RedirectToAction("UploadDealDocument", new { Email = Item.utblMstSellerTrackDeal.Email, ClientID = ItemData.utblMstSellerTrackDeal.ClientID, TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID, AgentID = Item.TrackDealMasterView.AgentID });
                }

            }
            else
            {
                TempData["ErrMsg"] = null;
                return RedirectToAction("List", "DealAgent");
            }

            return View(ItemData);
        }
        #endregion
        #region Delete Document
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDocument(string SellerDealDocID, string ClientID, string SellerTrackingID, string Email, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            utblMstSellerTrackDealDoc objDoc = new utblMstSellerTrackDealDoc();
            objDoc = objDal.GetDocDetailsByID(SellerDealDocID);
            TempData["ErrMsg"] = objDal.DeleteLiveDealDocument(SellerDealDocID, SellerTrackingID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + SellerDealDocID + objDoc.TrackDocFile));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("UploadDealDocument", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
        }
        #endregion
        #region HomePhotograph

        public ActionResult SellerHome(string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            MstClientHomeGalleryManageModel ObjModel = new MstClientHomeGalleryManageModel();

            ObjModel.HomeGalleryView = objDal.GetHomeGalleryList(Email, TransactionID);
            ObjModel.utblMstClientHomeGalleries = new utblMstClientHomeGallerie();
            ObjModel.utblMstClientHomeGalleries.Email = Email;
            ObjModel.utblMstClientHomeGalleries.TransactionID = TransactionID;
            ObjModel.utblMstClientHomeGalleries.ClientID = ClientID;
            ObjModel.UserProfile = objDal.GetUserDetails(ClientID);
            ViewBag.AgentID = AgentID;

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvHomeGalleryList", ObjModel);
            }
            return View(ObjModel);
        }

        [HttpPost]
        public ActionResult SellerHomes(MstClientHomeGalleryManageModel ItemData, string AgentID = "")
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            ViewBag.AgentID = AgentID;
            MstClientHomeGalleryManageModel ObjModel = new MstClientHomeGalleryManageModel();
            ObjModel.HomeGalleryView = objDal.GetHomeGalleryList(ItemData.utblMstClientHomeGalleries.Email, ItemData.utblMstClientHomeGalleries.TransactionID);
            ObjModel.utblMstClientHomeGalleries = new utblMstClientHomeGallerie();
            ObjModel.utblMstClientHomeGalleries.Email = ItemData.utblMstClientHomeGalleries.Email;
            ObjModel.utblMstClientHomeGalleries.TransactionID = ItemData.utblMstClientHomeGalleries.TransactionID;

            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;


                    if (file != null && file.ContentLength > 0)
                    {
                        ItemData.utblMstClientHomeGalleries.FileExt = Path.GetExtension(file.FileName);
                        TempData["ErrMsg"] = objDal.SaveHomeGallery(ItemData.utblMstClientHomeGalleries);
                        if ((TempData["ErrMsg"].ToString()) == "0")
                        {
                            TempData["ErrMsg"] = null;
                            string ext = Path.GetExtension(file.FileName);
                            var filName = ItemData.utblMstClientHomeGalleries.HomePhotoID;
                            var path = string.Concat(Server.MapPath("~/UploadedFiles/SellerHome/" + filName + ext));
                            file.SaveAs(path);
                        }


                    }


                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                string url = Url.Action("SellerHome", "ManageSeller", new { area = "Coordinator", Email = ItemData.utblMstClientHomeGalleries.Email, ClientID = ItemData.utblMstClientHomeGalleries.ClientID, TransactionID = ItemData.utblMstClientHomeGalleries.TransactionID, AgentID = AgentID });
                return Json(new { success = true, url = url });
                //return Json(new { Message = fName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "Error in saving file" }, JsonRequestBehavior.AllowGet);
            }



        }

        #endregion
        #region Home Gallery
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGallery(string HomePhotoID, string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            utblMstClientHomeGallerie objGallary = new utblMstClientHomeGallerie();
            objGallary = objDal.GetHomeGalleryByID(HomePhotoID);
            TempData["ErrMsg"] = objDal.DeleteGallery(HomePhotoID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/SellerHome/" + HomePhotoID + objGallary.FileExt));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("SellerHome", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
        }
        #endregion
        #region ClosingConfig
        public ActionResult ClosingConfig(string Email, int SellerStatusID, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            ObjManageModel.UserInfo = new MstInfoView();
            ObjManageModel.utblMstSellerTrackDeal = new utblMstSellerTrackDeal();
            ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ObjManageModel.UserInfo.Email = Email;
            ObjManageModel.utblMstSellerTrackDeal = new utblMstSellerTrackDeal()
            {
                SellerStatusID = SellerStatusID,
                ClientID = ClientID,
                TransactionID = TransactionID
            };
            return View(ObjManageModel);
        }
        [HttpPost]
        public ActionResult ClosingConfig(MstSellerDealManageModel ItemData, string ListingSHA = "")
        {

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            ItemData.MstClosingConfig.utblMstClosingDate.Email = ItemData.UserInfo.Email;
            ItemData.MstClosingConfig.utblMstClosingDate.ClientID = ItemData.utblMstSellerTrackDeal.ClientID;

            if (ModelState.IsValid)
            {
                if (ItemData.MstClosingConfig.utblMstClosingDate.ListingTypeID == 1)
                {
                    //string MLSID = ItemData.MstClosingConfig.utblMstClosingDate.MLSID;

                    //string HomeAddress = ListingDetails(MLSID);
                    if (!string.IsNullOrEmpty(ListingSHA))
                    {
                        ItemData.MstClosingConfig.utblMstClosingDate.Address = ItemData.MstClosingConfig.utblMstClosingDate.MLSID;
                        ItemData.MstClosingConfig.utblMstClosingDate.MLSID = ListingSHA;

                    }

                    else
                    {
                        TempData["ErrMsg"] = "....Please select valid address from dropdown...";
                        //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
                        ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
                        return View(ItemData);
                    }
                }

                TempData["ErrMsg"] = objDal.SaveClosingConfiguration(ItemData.MstClosingConfig.utblMstClosingDate, ItemData.utblMstSellerTrackDeal.SellerStatusID, ItemData.utblMstSellerTrackDeal.TransactionID);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;


                    return RedirectToAction("CreateDeal", new { ClientID = ItemData.MstClosingConfig.utblMstClosingDate.ClientID, TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID });
                }
            }
            //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
            ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();

            return View(ItemData);

        }
        public ActionResult ClosingConfigView(string Email, int SellerStatusID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            string url = Url.Action("ClosingConfig", "ManageSeller", new { area = "Coordinator", Email = Email, SellerStatusID = SellerStatusID });
            return Json(new { success = true, url = url }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Delete TrackDeal Record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveDeal(string TrackingID, string Email, int StatusID, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            TempData["ErrMsg"] = objDal.RemoveDeal(TrackingID, Email, StatusID, TransactionID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }

            string url = Url.Action("CreateDeal", "ManageSeller", new { area = "Coordinator", ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
            return Json(new { success = true, url = url });
            //return RedirectToAction("CreateDeal", new { Email = Email });

        }

        #endregion
        #region Submitting Offer

        public ActionResult SubmitOffer(string ClientID, string TransactionID)
        {
            ObjSeller.utblMstSellerCounterOffer = new utblMstSellerCounterOffer()
            {
                TransactionID = TransactionID,
                SellerID = ClientID,
                ClientTypeID = 1,
                Status = "Recieved Offer",
            };

            ObjSeller.ReceivedOfferList = objDal.OfferList(TransactionID);

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvSubmitOffer", ObjSeller);
            }

            return View(ObjSeller);
        }


        [HttpPost]
        public ActionResult SubmitOffer(MstSellerOffer Itemdata)
        {

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = objDal.SubmittingOffer(Itemdata);

                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    return RedirectToAction("SubmitOffer", new { ClientID = Itemdata.utblMstSellerCounterOffer.SellerID, TransactionID = Itemdata.utblMstSellerCounterOffer.TransactionID });
                }

            }
            ObjSeller.utblMstSellerCounterOffer = new utblMstSellerCounterOffer()
            {
                TransactionID = Itemdata.utblMstSellerCounterOffer.TransactionID,
                SellerID = Itemdata.utblMstSellerCounterOffer.SellerID,
                ClientTypeID = Itemdata.utblMstSellerCounterOffer.ClientTypeID,
                Status = "Recieved Offer",
            };
            ObjSeller.ReceivedOfferList = objDal.OfferList(Itemdata.utblMstSellerCounterOffer.TransactionID);

            return View(ObjSeller);
        }

        #endregion
        #region MLSHomeLookUp
        public static async Task<RootObject> GetMLSInfo(string MLS)
        {
            RootObject ObjviewModel = new RootObject();
            WebClient wclient = new WebClient();
            string MLSURL = "https://queryservicec.placester.net/search?";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["mls_id"] = MLS;
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
                var list = JsonConvert.DeserializeObject<RootObject>(result);
                ObjviewModel = list;
            }
            return ObjviewModel;
        }
        #endregion
        #region SendEmail
        private string PopulateBody(string Name, string Date, string Status, string AgentName, string AgentNumber)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/Status.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Name}", Name);
            body = body.Replace("{Date}", Date);
            body = body.Replace("{Status}", Status);
            body = body.Replace("{AgentName}", AgentName);
            body = body.Replace("{AgentNumber}", AgentNumber);


            return body;
        }
        private int SendEmailConfirmationToken(string email, string Name, string Date, string Status, string AgentName, string AgentNumber)
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

            mm.Subject = "Your Deal Tracker Notification";
            mm.Body = this.PopulateBody(Name, Date, Status, AgentName, AgentNumber);
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
        #region JsonResult
        public ActionResult SellerDoc(string Email, string ClientID, string TransactionID)
        {
            MstSellerDealManageModel model = new MstSellerDealManageModel();
            model.utblMstSellerTrackDeal = new utblMstSellerTrackDeal()
            {
                Email = Email,
                ClientID = ClientID,
                TransactionID = TransactionID
            };

            return PartialView("pvSellerDoc", model);
        }
        [HttpPost]

        public ActionResult SellerDoc(MstSellerDealManageModel Itemdata)
        {
            dalMstBuyerDocuments ObjDal = new dalMstBuyerDocuments();

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.SellerDocMaster(Itemdata.utblMstSellerDocuments);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    ModelState.AddModelError("", "Document Added Succesfully");
                    string url = Url.Action("UploadDealDocument", new { Email = Itemdata.utblMstSellerTrackDeal.Email, ClientID = Itemdata.utblMstSellerTrackDeal.ClientID, TransactionID = Itemdata.utblMstSellerTrackDeal.TransactionID });
                    return Json(new { success = true, url = url });
                    //return RedirectToAction("UploadDealDocument", new { Email = Itemdata.utblMstTrackDeal.Email, ClientID = Itemdata.utblMstTrackDeal.ClientID, TransactionID = Itemdata.utblMstTrackDeal.TransactionID });
                }

            }

            return PartialView("pvSellerDoc", Itemdata);
        }
        #endregion


        #region Modified Codes

        #region Manage Deal
        public async Task<ActionResult> ManageDeal(string ClientID, string TransactionID, string AgentID = "", string Error = "")
        {
            string Email = await UserManager.GetEmailAsync(ClientID);
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.StatusList = objDal.StatusList(Email, TransactionID);
            ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            ObjManageModel.TrackDealStatusList = objDal.TrackDealStatusList(Email, TransactionID);
            
            ObjManageModel.IsDealRatified=objDal.IsDealRatified(Email, TransactionID);//Added By Neha
            //ObjManageModel.UserProfile = objDal.GetUserDetails(ClientID);
            ObjManageModel.TransactionDetails = objClientDetails.GetTransactionDetails(TransactionID);
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView()
            {
                ClientID = ClientID,
                TransactionID = TransactionID,
                AgentID = AgentID
            };
            ViewBag.Error = Error;
            return PartialView("pvClientStatusList_ver1", ObjManageModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageDeal(MstSellerDealManageModel ItemData, FormCollection data)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ItemData.MstUserStatusSelect = new UserStatusSelect();
            ItemData.utblMstSellerTrackDeal.Email = ItemData.UserInfo.Email;
         
            string Email = ItemData.UserInfo.Email;
            int NoOfDays = 0;
            bool IsContigency = objDal.GetContigency(ItemData.utblMstSellerTrackDeal.SellerStatusID);
            if (!string.IsNullOrEmpty(data["[seller-steps].NoOfDays"]))
             NoOfDays = Convert.ToInt32(data["[seller-steps].NoOfDays"]);//Added By Neha
            string AgentEmail=UserManager.GetEmail(ItemData.TrackDealMasterView.AgentID);//Added By Neha
            DateTime RatifiedDate = objDal.GetRatifiedDate(ItemData.utblMstSellerTrackDeal.TransactionID);
            if (data.AllKeys.Contains("datenotrequired"))
                ItemData.utblMstSellerTrackDeal.UpdatedOn = System.DateTime.Now;
            if (ModelState.IsValid)
            {

                if (ItemData.ClosingConfig.SellerStatusID == ItemData.utblMstSellerTrackDeal.SellerStatusID)
                {
                    string url = Url.Action("closingconfiguration", "manageseller", new { Email = ItemData.UserInfo.Email, SellerStatusID = ItemData.utblMstSellerTrackDeal.SellerStatusID, ClientID = ItemData.utblMstSellerTrackDeal.ClientID, TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
                    return Json(new { success = true, val = 1, url = url });
                    //return RedirectToAction("ClosingConfig", new { Email = ItemData.UserInfo.Email, SellerStatusID = ItemData.utblMstSellerTrackDeal.SellerStatusID, ClientID = ItemData.utblMstSellerTrackDeal.ClientID, TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
                }

                //  TempData["ErrMsg"] = objDal.Save(ItemData.utblMstSellerTrackDeal);//Removed by Neha
                TempData["ErrMsg"] = objDal.SaveNew(ItemData.utblMstSellerTrackDeal, AgentEmail,NoOfDays, IsContigency, RatifiedDate);//Added By Neha
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                    if (objDal.emailPreferences(ItemData.UserInfo.Email))
                    {
                        string Date = DateTime.Now.ToString("dd MMM yyyy HH:mm");
                        string EmailID = ItemData.utblMstSellerTrackDeal.Email;
                        ItemData.MstUserStatusSelect = objDal.GetClientStatusfromEmail(Email, ItemData.utblMstSellerTrackDeal.SellerStatusID);
                        ItemData.MstAgentClientNameSelect = objDal.GetNameEmail(ItemData.utblMstSellerTrackDeal.TransactionID);

                        int MailStatus = SendEmailConfirmationToken(EmailID, ItemData.MstUserStatusSelect.ClientName, Date, ItemData.MstUserStatusSelect.Status, ItemData.MstAgentClientNameSelect.AgentName, ItemData.MstAgentClientNameSelect.AgentPhone);
                        if (MailStatus == 0)
                        {

                            TempData["MailMsg"] = 1;
                        }
                        else
                        {
                            TempData["ErrMsg"] = null;
                        }
                    }
                }
            }
            return RedirectToAction("ManageDeal", "manageseller", new { ClientID = ItemData.utblMstSellerTrackDeal.ClientID, TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID, Error = "* date field is required" });
            //ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            //ObjManageModel.StatusList = objDal.StatusList(Email, ItemData.utblMstSellerTrackDeal.TransactionID);
            //ObjManageModel.TrackClosingDate = objDal.TrackClosingDate(Email, ItemData.utblMstSellerTrackDeal.TransactionID);
            //ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            //ObjManageModel.TrackDealStatusList = objDal.TrackDealStatusList(Email, ItemData.utblMstSellerTrackDeal.TransactionID);
            //return View(ObjManageModel);
        }
        #endregion
        #region ClosingConfig Modified
        public ActionResult ClosingConfiguration(string Email, int SellerStatusID, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            ObjManageModel.UserInfo = new MstInfoView();
            ObjManageModel.utblMstSellerTrackDeal = new utblMstSellerTrackDeal();
            ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ObjManageModel.UserInfo.Email = Email;
            ObjManageModel.utblMstSellerTrackDeal = new utblMstSellerTrackDeal()
            {
                SellerStatusID = SellerStatusID,
                ClientID = ClientID,
                TransactionID = TransactionID
            };
            return PartialView("pvCreateRatifiedOffer", ObjManageModel);
        }
        [HttpPost]
        public ActionResult ClosingConfiguration(MstSellerDealManageModel ItemData, string ListingSHA = "")
        {

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            ItemData.MstClosingConfig.utblMstClosingDate.Email = ItemData.UserInfo.Email;
            ItemData.MstClosingConfig.utblMstClosingDate.ClientID = ItemData.utblMstSellerTrackDeal.ClientID;

            if (ModelState.IsValid)
            {
                if (ItemData.MstClosingConfig.utblMstClosingDate.ListingTypeID == 1)
                {
                    if (!string.IsNullOrEmpty(ListingSHA))
                    {
                        ItemData.MstClosingConfig.utblMstClosingDate.Address = ItemData.MstClosingConfig.utblMstClosingDate.MLSID;
                        ItemData.MstClosingConfig.utblMstClosingDate.MLSID = ListingSHA;
                    }
                    else
                    {
                        TempData["ErrMsg"] = "....Please re-select valid address from the dropdown...";
                        ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
                        return PartialView("pvCreateRatifiedOffer", ItemData);
                    }
                }
                TempData["ErrMsg"] = objDal.SaveClosingConfiguration(ItemData.MstClosingConfig.utblMstClosingDate, ItemData.utblMstSellerTrackDeal.SellerStatusID, ItemData.utblMstSellerTrackDeal.TransactionID);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                }
                string url = Url.Action("ManageDeal", new { ClientID = ItemData.MstClosingConfig.utblMstClosingDate.ClientID, TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID });
                return Json(new { success = true, url = url, type = "editRO" });
            }
            ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            return PartialView("pvCreateRatifiedOffer", ItemData);
        }
        #endregion
        #region  UploadDealDocument Modified
        public ActionResult UploadDealDocumentPV(string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            string ID = objDal.GetTrackingID(Email, TransactionID);
            ObjManageModel.utblMstSellerTrackDeal = new utblMstSellerTrackDeal();
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView();
            ObjManageModel.utblMstSellerTrackDeal = objDal.GetLiveDealDetailsByID(ID, TransactionID);
            ObjManageModel.MstSellerDocumentList = objDal.SellerDocumentListView(Email, TransactionID);
            ObjManageModel.MstSellerDocList = objDal.SellerDocumentList(Email, TransactionID);
            ObjManageModel.UserProfile = objDal.GetUserDetails(ClientID);
            ObjManageModel.TrackDealMasterView.AgentID = AgentID;

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            return PartialView("pvUploadDealDocument_ver1", ObjManageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDealDocumentPV(HttpPostedFileBase files, MstSellerDealViewModel ItemData, MstSellerDealManageModel Item)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            if (files != null && files.ContentLength > 0)
            {
                int DocID = ItemData.utblMstSellerTrackDealDoc.SellerDocID;
                ItemData.utblMstSellerTrackDealDoc = new utblMstSellerTrackDealDoc();
                ItemData.utblMstSellerTrackDealDoc.SellerTrackingID = ItemData.utblMstSellerTrackDeal.SellerTrackingID;
                ItemData.utblMstSellerTrackDealDoc.TrackDocFile = Path.GetExtension(files.FileName);
                ItemData.utblMstSellerTrackDealDoc.Email = ItemData.utblMstSellerTrackDeal.Email;
                ItemData.utblMstSellerTrackDealDoc.SellerStatusID = ItemData.utblMstSellerTrackDeal.SellerStatusID;
                ItemData.utblMstSellerTrackDealDoc.SellerDocID = DocID;
                ItemData.utblMstSellerTrackDealDoc.TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID;
                TempData["ErrMsg"] = objDal.SaveSellerDoc(ItemData.utblMstSellerTrackDealDoc);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    var TrackingID = ItemData.utblMstSellerTrackDeal.SellerTrackingID;
                    string ext = Path.GetExtension(files.FileName);
                    var fileName = ItemData.utblMstSellerTrackDealDoc.SellerDealDocID;
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + fileName + ext));
                    files.SaveAs(path);
                    ItemData.MstLiveDealDocList = objDal.GetSellerDealDocList(TrackingID);

                }

            }
            return RedirectToAction("UploadDealDocumentPV", new { Email = Item.utblMstSellerTrackDeal.Email, ClientID = ItemData.utblMstSellerTrackDeal.ClientID, TransactionID = ItemData.utblMstSellerTrackDeal.TransactionID, AgentID = Item.TrackDealMasterView.AgentID });
        }
        #endregion
        #region Delete Document Modified
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDocumentPV(string SellerDealDocID, string ClientID, string SellerTrackingID, string Email, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            utblMstSellerTrackDealDoc objDoc = new utblMstSellerTrackDealDoc();
            objDoc = objDal.GetDocDetailsByID(SellerDealDocID);
            TempData["ErrMsg"] = objDal.DeleteLiveDealDocument(SellerDealDocID, SellerTrackingID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + SellerDealDocID + objDoc.TrackDocFile));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("UploadDealDocumentPV", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
        }
        #endregion
        #region Home Gallery
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGalleryPV(string HomePhotoID, string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            utblMstClientHomeGallerie objGallary = new utblMstClientHomeGallerie();
            objGallary = objDal.GetHomeGalleryByID(HomePhotoID);
            TempData["ErrMsg"] = objDal.DeleteGallery(HomePhotoID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/SellerHome/" + HomePhotoID + objGallary.FileExt));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("SellerHomePV", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
        }
        #endregion
        #region Delete TrackDeal Record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveDealPV(string TrackingID, string Email, int StatusID, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            TempData["ErrMsg"] = objDal.RemoveDeal(TrackingID, Email, StatusID, TransactionID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }

            return RedirectToAction("ManageDeal", "manageseller", new { area = "Coordinator", ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });

            //return RedirectToAction("CreateDeal", new { Email = Email });

        }

        #endregion
        #region JsonResult Modified
        public ActionResult SellerDocPV(string Email, string ClientID, string TransactionID)
        {
            MstSellerDealManageModel model = new MstSellerDealManageModel();
            model.utblMstSellerTrackDeal = new utblMstSellerTrackDeal()
            {
                Email = Email,
                ClientID = ClientID,
                TransactionID = TransactionID
            };

            return PartialView("pvSellerDocPV", model);
        }
        [HttpPost]

        public ActionResult SellerDocPV(MstSellerDealManageModel Itemdata)
        {
            dalMstBuyerDocuments ObjDal = new dalMstBuyerDocuments();

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.SellerDocMaster(Itemdata.utblMstSellerDocuments);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    ModelState.AddModelError("", "Document Added Succesfully");
                    string url = Url.Action("UploadDealDocumentPV", new { Email = Itemdata.utblMstSellerTrackDeal.Email, ClientID = Itemdata.utblMstSellerTrackDeal.ClientID, TransactionID = Itemdata.utblMstSellerTrackDeal.TransactionID });
                    return Json(new { success = true, url = url, type = "addDocType" });
                    //return RedirectToAction("UploadDealDocument", new { Email = Itemdata.utblMstTrackDeal.Email, ClientID = Itemdata.utblMstTrackDeal.ClientID, TransactionID = Itemdata.utblMstTrackDeal.TransactionID });
                }

            }

            return PartialView("pvSellerDocPV", Itemdata);
        }
        #endregion
        #region HomePhotograph

        public ActionResult SellerHomePV(string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            MstClientHomeGalleryManageModel ObjModel = new MstClientHomeGalleryManageModel();

            ObjModel.HomeGalleryView = objDal.GetHomeGalleryList(Email, TransactionID);
            ObjModel.utblMstClientHomeGalleries = new utblMstClientHomeGallerie();
            ObjModel.utblMstClientHomeGalleries.Email = Email;
            ObjModel.utblMstClientHomeGalleries.TransactionID = TransactionID;
            ObjModel.utblMstClientHomeGalleries.ClientID = ClientID;
            ObjModel.UserProfile = objDal.GetUserDetails(ClientID);
            ViewBag.AgentID = AgentID;
            return PartialView("pvHomeGalleryList_ver1", ObjModel);

        }

        [HttpPost]
        public ActionResult SellerHomePV(MstClientHomeGalleryManageModel ItemData, string AgentID = "")
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            ViewBag.AgentID = AgentID;
            MstClientHomeGalleryManageModel ObjModel = new MstClientHomeGalleryManageModel();
            ObjModel.HomeGalleryView = objDal.GetHomeGalleryList(ItemData.utblMstClientHomeGalleries.Email, ItemData.utblMstClientHomeGalleries.TransactionID);
            ObjModel.utblMstClientHomeGalleries = new utblMstClientHomeGallerie();
            ObjModel.utblMstClientHomeGalleries.Email = ItemData.utblMstClientHomeGalleries.Email;
            ObjModel.utblMstClientHomeGalleries.TransactionID = ItemData.utblMstClientHomeGalleries.TransactionID;

            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;


                    if (file != null && file.ContentLength > 0)
                    {
                        ItemData.utblMstClientHomeGalleries.FileExt = Path.GetExtension(file.FileName);
                        TempData["ErrMsg"] = objDal.SaveHomeGallery(ItemData.utblMstClientHomeGalleries);
                        if ((TempData["ErrMsg"].ToString()) == "0")
                        {
                            TempData["ErrMsg"] = null;
                            string ext = Path.GetExtension(file.FileName);
                            var filName = ItemData.utblMstClientHomeGalleries.HomePhotoID;
                            var path = string.Concat(Server.MapPath("~/UploadedFiles/SellerHome/" + filName + ext));
                            file.SaveAs(path);
                        }


                    }


                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                string url = Url.Action("SellerHomePV", "ManageSeller", new { area = "Coordinator", Email = ItemData.utblMstClientHomeGalleries.Email, ClientID = ItemData.utblMstClientHomeGalleries.ClientID, TransactionID = ItemData.utblMstClientHomeGalleries.TransactionID, AgentID = AgentID });
                return Json(new { success = true, url = url });
                //return Json(new { Message = fName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "Error in saving file" }, JsonRequestBehavior.AllowGet);
            }



        }

        #endregion
        #endregion


        #region MLSIDLOOKUP

        public string ListingDetails(string mls_id)
        {
            string MLSURL = "http://www.mandyanddavid.com/idx/search.html?";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            uriBuilder.Port = -1;

            query["refine"] = "true";
            query["search_location"] = mls_id;

            uriBuilder.Query = query.ToString();
            MLSURL = uriBuilder.ToString();
            var request = WebRequest.Create(MLSURL);
            var response = request.GetResponse();
            string stringResponse = "";
            string Address = "";

            try
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);
                    stringResponse = reader.ReadToEnd();
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(stringResponse);
                Address = document.DocumentNode.SelectSingleNode("//div[@class='idx-default']/h4").InnerText;
                //var div = document.DocumentNode.SelectSingleNode("//div[contains(@class,'idx-listing-image')]");
                //IMGURL = Regex.Match(div.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value;
                //ViewBag.Address = Address;
                //ViewBag.Image = IMGURL;
            }
            catch (Exception)
            {
                Address = "";
            }

            return Address;
        }

        #endregion
    }
}