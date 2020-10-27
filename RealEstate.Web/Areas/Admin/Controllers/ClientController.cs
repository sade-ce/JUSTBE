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

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientController : Controller
    {
        // GET: Admin/Client
        dalMstClientList objDal = new dalMstClientList();
        MstClientListManageModel model = new MstClientListManageModel();
        MstClientDealCreateManageModel ObjManageModel = new MstClientDealCreateManageModel();
        dalTrackDeal ObjStatus = new dalTrackDeal();
        dalMstClientMLSHomeListing Objdal = new dalMstClientMLSHomeListing();
        RootObject ObjviewModel = new RootObject();

        #region List
        public ActionResult List(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            int TotalRow;
            model.MstClientList = objDal.ClientPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/Admin/Client/List";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClientList", model);
            }
            return View(model);
        }
        #endregion
        #region CreateDeal
        public ActionResult CreateDeal(string Email,string TransactionID)
        {
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.StatusList = objDal.StatusList(Email, TransactionID);
            ObjManageModel.TrackClosingDate = objDal.TrackClosingDate(Email, TransactionID);
            ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            ObjManageModel.TrackDealStatusList = objDal.TrackDealStatusList(Email, TransactionID);
            ViewBag.ActiveURL = "/Admin/Client/List";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClientStatusList", ObjManageModel);
            }
            return View(ObjManageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDeal(MstClientDealCreateManageModel ItemData)
        {
            ViewBag.ActiveURL = "/Admin/Client/List";
            ItemData.MstUserStatusSelect = new UserStatusSelect();
            ItemData.utblMstTrackDeal.Email = ItemData.UserInfo.Email;
            string Email = ItemData.UserInfo.Email;
            if (ModelState.IsValid)
            {

                if (ItemData.ClosingConfig.StatusID == ItemData.utblMstTrackDeal.StatusID)
                {
                    return RedirectToAction("ClosingConfig", new { Email = ItemData.UserInfo.Email, StatusID = ItemData.utblMstTrackDeal.StatusID, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID });
                }

                //TempData["ErrMsg"] = 0; ObjStatus.Save(ItemData.utblMstTrackDeal);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                    if (ObjStatus.emailPreferences(ItemData.UserInfo.Email))
                    {
                        string Date = DateTime.Now.ToString("dd MMM yyyy HH:mm");
                        string EmailID = ItemData.utblMstTrackDeal.Email;
                        ItemData.MstUserStatusSelect = ObjStatus.GetClientStatusfromEmail(Email, ItemData.utblMstTrackDeal.StatusID);
                        ItemData.MstAgentClientNameSelect = ObjStatus.GetNameEmail(ItemData.utblMstTrackDeal.TransactionID);

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

                    return RedirectToAction("CreateDeal", new { ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID });
                }
            }
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.StatusList = objDal.StatusList(Email, ItemData.utblMstTrackDeal.TransactionID);
            ObjManageModel.TrackClosingDate = objDal.TrackClosingDate(Email, ItemData.utblMstTrackDeal.TransactionID);
            ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            ObjManageModel.TrackDealStatusList = objDal.TrackDealStatusList(Email, ItemData.utblMstTrackDeal.TransactionID);
            return View(ObjManageModel);
        }
        #endregion

        #region ClosingConfig
        public ActionResult ClosingConfig(string Email, int StatusID, string ClientID, string TransactionID)
        {
            ObjManageModel.UserInfo = new MstInfoView();
            ObjManageModel.utblMstTrackDeal = new utblMstTrackDeal();
            ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ObjManageModel.UserInfo.Email = Email;
            ObjManageModel.utblMstTrackDeal = new utblMstTrackDeal()
            {
                StatusID = StatusID,
                ClientID = ClientID,
                TransactionID = TransactionID
            }; return View(ObjManageModel);
        }
        [HttpPost]
        public async Task<ActionResult> ClosingConfig(MstClientDealCreateManageModel ItemData)
        {
            string Address, Locality, region, postal;

            ViewBag.ActiveURL = "/Admin/Client/List";

            ItemData.MstClosingConfig.utblMstClosingDate.Email = ItemData.UserInfo.Email;
            if (ModelState.IsValid)
            {
                if (ItemData.MstClosingConfig.utblMstClosingDate.ListingTypeID == 1)
                {
                    string MLSID = ItemData.MstClosingConfig.utblMstClosingDate.MLSID;
                    ObjviewModel = await GetMLSInfo(MLSID);
                    if (ObjviewModel.organic_results.search_results.Count() > 0)
                    {
                        Address = ObjviewModel.organic_results.search_results[0].location.address;
                        Locality = ObjviewModel.organic_results.search_results[0].location.locality;
                        region = ObjviewModel.organic_results.search_results[0].location.region;
                        postal = ObjviewModel.organic_results.search_results[0].location.postal;
                        string HomeAddress = string.Join(" ", Address, Locality, region, postal);
                        ItemData.MstClosingConfig.utblMstClosingDate.Address = HomeAddress;
                    }
                    else
                    {
                        TempData["ErrMsg"] = "....Please Enter Valid MLS ID...";
                        ItemData.MstClosingConfig = new MstClosingConfigViewModel();
                        ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
                        return View(ItemData);
                    }
                }

                //TempData["ErrMsg"] = ObjStatus.SaveClosingConfiguration(ItemData.MstClosingConfig.utblMstClosingDate, ItemData.utblMstTrackDeal.StatusID, ItemData.utblMstTrackDeal.TransactionID);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("CreateDeal", new { Email = ItemData.UserInfo.Email, TransactionID= ItemData.utblMstTrackDeal.TransactionID });
                }
            }
            ItemData.MstClosingConfig = new MstClosingConfigViewModel();
            ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            return View(ItemData);

        }
        public ActionResult ClosingConfigView(string Email, int StatusID)
        {
            string url = Url.Action("ClosingConfig", "Client", new { area = "Admin", Email = Email, StatusID= StatusID });
            return Json(new { success = true, url = url },JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Delete TrackDeal Record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveDeal(string TrackingID, string Email, int StatusID, string ClientID, string TransactionID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            TempData["ErrMsg"] = ObjStatus.RemoveDeal(TrackingID, Email, StatusID, TransactionID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }

            string url = Url.Action("CreateDeal", "Agent", new { area = "Coordinator", ClientID = ClientID, TransactionID = TransactionID });
            return Json(new { success = true, url = url });
            //return RedirectToAction("CreateDeal", new { Email = Email });

        }

        #endregion
        #region  UploadDealDocument
        public ActionResult UploadDealDocument(string Email,string TransactionID)
        {
            string ID = objDal.GetTrackingID(Email,TransactionID);
            ObjManageModel.utblMstTrackDeal = new utblMstTrackDeal();
            ObjManageModel.MstCalenderConfigList = objDal.MstCalenderConfigList;
            ObjManageModel.utblMstTrackDeal = ObjStatus.GetLiveDealDetailsByID(ID, TransactionID);
            ObjManageModel.MstBuyerDocumentListView = ObjStatus.BuyerDocumentListView(Email,TransactionID);
            //ObjManageModel.MstLiveDealDocList = ObjStatus.GetLiveDealDocList(ID);
            ObjManageModel.MstBuyerDocList = ObjStatus.BuyerDocumentList(Email, TransactionID);
            ViewBag.ActiveURL = "/Admin/Client/List";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvUploadDealDocument", ObjManageModel);
            }
            return View(ObjManageModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDealDocument(HttpPostedFileBase files, MstDealViewModel ItemData, MstClientDealCreateManageModel Item)
        {
            ViewBag.ActiveURL = "/Admin/Client/List";

            if (files != null && files.ContentLength > 0)
            {
                int DocID = ItemData.utblMstTrackDealDoc.DocID;
                ItemData.utblMstTrackDealDoc = new utblMstTrackDealDoc();
                ItemData.utblMstTrackDealDoc.TrackingID = ItemData.utblMstTrackDeal.TrackingID;
                ItemData.utblMstTrackDealDoc.TrackDocFile = Path.GetExtension(files.FileName);
                ItemData.utblMstTrackDealDoc.Email = ItemData.utblMstTrackDeal.Email;
                ItemData.utblMstTrackDealDoc.StatusID = ItemData.utblMstTrackDeal.StatusID;
                ItemData.utblMstTrackDealDoc.DocID = DocID;
                TempData["ErrMsg"] = ObjStatus.SaveTrackDealDoc(ItemData.utblMstTrackDealDoc);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    var TrackingID = ItemData.utblMstTrackDeal.TrackingID;
                    string ext = Path.GetExtension(files.FileName);
                    var fileName = ItemData.utblMstTrackDealDoc.DealTrackDocID;
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + fileName + ext));
                    files.SaveAs(path);
                    ItemData.MstLiveDealDocList = ObjStatus.GetLiveDealDocList(TrackingID);
                    return RedirectToAction("UploadDealDocument", new { Email = Item.utblMstTrackDeal.Email });
                }

            }
            else
            {
                TempData["ErrMsg"] = null;
                return RedirectToAction("List");
            }

            return View(ItemData);
        }
        #endregion
        #region Delete Document
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDocument(string DealTrackDocID, string TrackingID, string Email)
        {
            utblMstTrackDealDoc objDoc = new utblMstTrackDealDoc();
            objDoc = ObjStatus.GetDocDetailsByID(DealTrackDocID);
            TempData["ErrMsg"] = ObjStatus.DeleteLiveDealDocument(DealTrackDocID, TrackingID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + DealTrackDocID + objDoc.TrackDocFile));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("UploadDealDocument", new { Email = Email });
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
            query["text_search"] = MLS;
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


            //string code = UserManager.GenerateEmailConfirmationToken(userID);
            //string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code, area = "" }, protocol: Request.Url.Scheme);

            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            //Create the SMTP Client
            SmtpClient client = new SmtpClient();
            client.Host = settings.Smtp.Network.Host;
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
    }
}