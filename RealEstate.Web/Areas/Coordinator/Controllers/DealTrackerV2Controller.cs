using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
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
    public class DealTrackerV2Controller : Controller
    {
        dalMstClientList objDal = new dalMstClientList();
        MstClientDealCreateManageModel ObjManageModel = new MstClientDealCreateManageModel();
        MstAgentPhotoGalleryManageModel objMediaModel = new MstAgentPhotoGalleryManageModel();
        dalMstAgentPhotoGallery objMediaDal = new dalMstAgentPhotoGallery();
        VendorCategoryViewModel objCategoryModel = new VendorCategoryViewModel();
        DealVendorViewModel objDealVendorModel = new DealVendorViewModel();
        dalTrackDeal ObjStatus = new dalTrackDeal();
        dalUser objUser = new dalUser();
        MstClientLiveDealViewModel ObjModel = new MstClientLiveDealViewModel();
        dalClientLiveDealView ObjDal = new dalClientLiveDealView();
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
        // GET: Coordinator/DealTrackerV2
        public async Task<ActionResult> Buyer(string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "DealTracker";
            string Email = await UserManager.GetEmailAsync(ClientID);
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.DealTracker = new DealTracker();
            ObjManageModel.DealTracker.Email = Email;
            ObjManageModel.UserInfoV2 = objDal.MstInfoViewVersion2(Email, TransactionID);
            // ObjManageModel.StatusList = objDal.StatusList(Email, TransactionID);
            ObjManageModel.TransactionDetails = objDal.GetTransactionDetails(TransactionID);
            ObjManageModel.TrackClosingDateID = objDal.TrackClosingDateID(Email, TransactionID);
            ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            ObjManageModel.DealTrackerList = objDal.TrackDealStatusListVersion2(Email, TransactionID);
            if (ObjManageModel.DealTrackerList.Count() > 0)
                ObjManageModel.DealTracker.CheckListItems = ObjManageModel.DealTrackerList.FirstOrDefault().CheckListItems;
            ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ObjManageModel.MstClosingConfig.StateDDL = objDal.StateDDL();
            ObjManageModel.MstClosingConfig.HomeTypeDDL = objDal.HomeTypeDDL();
            ObjManageModel.CondoStepDDL = objDal.CondoStepDDL();
            ObjManageModel.MstClosingConfig.utblMstClosingDate = objDal.GetRatifiedDetails(TransactionID);
            ObjManageModel.DealTracker.DealPrice = objDal.GetDealPrice(TransactionID);
            ObjManageModel.CheckListItems = objDal.GetChecklistItems();
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView()
            {
                AgentID = AgentID,
                ClientID = ClientID,
                TransactionID = TransactionID

            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvBuyerDealTracker", ObjManageModel);
            }
            return View(ObjManageModel);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemData"></param>
        /// <param name="Listdata"></param>
        /// <param name="ListingSHA"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Buyer(MstClientDealCreateManageModel ItemData, List<MstContingenciesViewVersion2> Listdata, string ListingSHA = "")
        {
            string Agent = "";
            Agent = User.Identity.Name;
            if (User.IsInRole("Admin"))
            {
                Agent = await UserManager.GetEmailAsync(ItemData.TrackDealMasterView.AgentID);
            }

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ItemData.MstUserStatusSelect = new UserStatusSelect();
            ItemData.DealTracker.Email = ItemData.UserInfo.Email;
            string Email = ItemData.UserInfo.Email;
            ItemData.BulkEmailStatus = new List<UserStatusSelect>();

            //if (ModelState.IsValid)
            //{
            ItemData.ContingenciesDataVersion2 = Listdata;

            //if(ItemData.ClosingConfig.StatusID==ItemData.utblMstTrackDeal.StatusID)
            //{
            //    return RedirectToAction("ClosingConfig", new { Email = ItemData.UserInfo.Email, StatusID = ItemData.utblMstTrackDeal.StatusID, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID });
            //}
            //two scenerio ratified offer done/ not done

            foreach (var item in Listdata)
            {

                //if (item.IsApplicable)
                // {
                // ObjManageModel.TrackRatifiedDate = objDal.TrackRatifiedDate(Email, ItemData.DealTracker.TransactionID);
                //select ratified offer date and add no of days
                // ItemData.DealTracker.UpdatedOn = ObjManageModel.TrackRatifiedDate.UpdatedOn;
                // }

                if (ItemData.ClosingConfig.StatusID == item.StatusID && item.IsCompleted == true)//Ratified offer
                {
                    //send listdata to ratified offer
                    //string url = Url.Action("closingconfiguration", new { Email = ItemData.UserInfo.Email, StatusID = item.StatusID, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, UpdatedOn = ItemData.utblMstTrackDeal.UpdatedOn, Listdata = JsonConvert.SerializeObject(Listdata), AgentID = ItemData.TrackDealMasterView.AgentID });
                    //return Json(new { success = true, val = 1, url = url });
                    ItemData.MstClosingConfig.utblMstClosingDate.Email = ItemData.UserInfo.Email;
                    ItemData.MstClosingConfig.utblMstClosingDate.ClientID = ItemData.DealTracker.ClientID;
                    ItemData.MstClosingConfig.utblMstClosingDate.UpdatedOn = ItemData.DealTracker.UpdatedOn;

                    if (ItemData.MstClosingConfig.utblMstClosingDate.ListingTypeID == 1)
                    {
                        if (!string.IsNullOrEmpty(ListingSHA))
                        {
                            ItemData.MstClosingConfig.utblMstClosingDate.Address = ItemData.MstClosingConfig.utblMstClosingDate.MLSID;
                            ItemData.MstClosingConfig.utblMstClosingDate.MLSID = ListingSHA;

                        }

                        //else
                        //{
                        //    ModelState.AddModelError("", "Please re-select valid address from the dropdown");
                        //    //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
                        //    ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
                        //    return PartialView("pvCreateRatifiedOffer", ItemData);
                        //}
                    }
                    TempData["ErrMsg"] = ObjStatus.SaveClosingConfigurationVersion2(ItemData.MstClosingConfig.utblMstClosingDate, item.StatusID, ItemData.DealTracker.TransactionID, Agent, ItemData.ContingenciesDataVersion2);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                    }
                    else
                    {
                        // TempData["ErrMsg"] = "Please add address to complete Ratified Offer step.";
                        // return Json(new { success = false, val = 0});
                        //TempData["ErrMsg"] = 5;
                        //return Json(ObjManageModel, JsonRequestBehavior.AllowGet);
                         return Json("Please add address to complete Ratified Offer step.", JsonRequestBehavior.AllowGet);
                    }
                }

                //var Data = ObjStatus.SelectBulkStatus(Email, item.StatusID);

                //ItemData.BulkEmailStatus.Add(Data);
            }
            TempData["ErrMsg"] = ObjStatus.SaveVersion2(ItemData.DealTracker, Listdata, Agent);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
                ItemData.BulkEmailStatus = ObjStatus.SelectBulkStatusVersion2(Email, ItemData.DealTracker.TransactionID);

            if (ItemData.BulkEmailStatus.Count > 0)
            {

               

                if (ObjStatus.emailPreferences(ItemData.UserInfo.Email))
                {
                    string Date = DateTime.Now.ToString("dd MMM yyyy HH:mm");
                    string EmailID = ItemData.DealTracker.Email;
                    ItemData.MstAgentClientNameSelect = ObjStatus.GetNameEmail(ItemData.DealTracker.TransactionID);
                    //bulk mail
                    int MailStatus = SendEmailConfirmationToken(EmailID, ItemData.MstUserStatusSelect.ClientName, Date, ItemData.BulkEmailStatus, ItemData.MstAgentClientNameSelect.AgentName, ItemData.MstAgentClientNameSelect.AgentPhone, ItemData.MstAgentClientNameSelect.AgentEmail);
                        //if (MailStatus == 0)
                        //{

                        //    TempData["ErrMsg"] = 0;
                        //}
                        //else
                        //{
                        //    TempData["ErrMsg"] = null;
                        //}
                    }

                   ObjStatus.DeleteSendEmail(ItemData.DealTracker.TransactionID);
                    



                }
                TempData["ErrMsg"] = 0;
            }
            //return RedirectToAction("Buyer", new { ClientID = ItemData.DealTracker.ClientID, TransactionID = ItemData.DealTracker.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
            //}
            //return Json(new { success = true, val = 0 });

            // ViewBag.ActiveURL = "DealTracker";
            //   string Email = await UserManager.GetEmailAsync(ItemData.DealTracker.ClientID);



            //added fgfhdfh
            //ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            //ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            //ObjManageModel.MstClosingConfig.HomeTypeDDL = objDal.HomeTypeDDL();
            //ObjManageModel.MstClosingConfig.StateDDL = objDal.StateDDL();
            //ObjManageModel.CondoStepDDL = objDal.CondoStepDDL();
            //ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            //ObjManageModel.DealTracker = new DealTracker();
            //ObjManageModel.DealTracker.Email = Email;
            //// ObjManageModel.StatusList = objDal.StatusList(Email, TransactionID);
            //ObjManageModel.TransactionDetails = objDal.GetTransactionDetails(ItemData.DealTracker.TransactionID);
            //ObjManageModel.TrackClosingDateID = objDal.TrackClosingDateID(Email, ItemData.DealTracker.TransactionID);
            //ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            //ObjManageModel.DealTrackerList = objDal.TrackDealStatusListVersion2(Email, ItemData.DealTracker.TransactionID);
            //ObjManageModel.MstClosingConfig.utblMstClosingDate = objDal.GetRatifiedDetails(ItemData.DealTracker.TransactionID);
            //ObjManageModel.CheckListItems = objDal.GetChecklistItems();
            //ObjManageModel.TrackDealMasterView = new TrackDealMasterView()
            //{
            //    AgentID = ItemData.TrackDealMasterView.AgentID,
            //    ClientID = ItemData.DealTracker.ClientID,
            //    TransactionID = ItemData.DealTracker.TransactionID
            //};


            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("pvBuyerDealTracker", ObjManageModel);
            //}
            //return View(ObjManageModel);
            //return RedirectToAction("Buyer", new { ClientID = ItemData.DealTracker.ClientID, TransactionID = ItemData.DealTracker.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
            return Json(ObjManageModel, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Seller(string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "DealTracker";
            string Email = await UserManager.GetEmailAsync(ClientID);
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.DealTracker = new DealTracker();
            ObjManageModel.DealTracker.Email = Email;
            ObjManageModel.UserInfoV2 = objDal.MstInfoViewVersion2(Email, TransactionID);

           // ObjManageModel.TransactionDetails = objDal.GetTransactionDetails(TransactionID);
           // ObjManageModel.TrackClosingDateID = objDal.TrackClosingDateID(Email, TransactionID);
            ObjManageModel.SellerClosingConfig = objDal.GetSellerClosingConfig();
            ObjManageModel.DealTrackerList = objDal.SellerTrackDealStatusListVersion2(Email, TransactionID);
            if (ObjManageModel.DealTrackerList.Count() > 0)
                ObjManageModel.DealTracker.CheckListItems = ObjManageModel.DealTrackerList.FirstOrDefault().CheckListItems;
            ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ObjManageModel.MstClosingConfig.StateDDL = objDal.StateDDL();
            ObjManageModel.MstClosingConfig.HomeTypeDDL = objDal.HomeTypeDDL();
            ObjManageModel.CondoStepDDL = objDal.CondoStepDDL();
            ObjManageModel.MstClosingConfig.utblMstClosingDate = objDal.GetRatifiedDetails(TransactionID);
            ObjManageModel.DealTracker.DealPrice = objDal.GetDealPrice(TransactionID);
            ObjManageModel.CheckListItems = objDal.GetSellerChecklistItems();
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView()
            {
                AgentID = AgentID,
                ClientID = ClientID,
                TransactionID = TransactionID

            };
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("pvBuyerDealTracker", ObjManageModel);
            //}
            return View(ObjManageModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Seller(MstClientDealCreateManageModel ItemData, List<MstSellerContingenciesViewVersion2> Listdata, string ListingSHA = "")
        {
            string Agent = "";
            Agent = User.Identity.Name;
            if (User.IsInRole("Admin"))
            {
                Agent = await UserManager.GetEmailAsync(ItemData.TrackDealMasterView.AgentID);
            }

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ItemData.MstUserStatusSelect = new UserStatusSelect();
            ItemData.DealTracker.Email = ItemData.UserInfo.Email;
            string Email = ItemData.UserInfo.Email;
            ItemData.BulkEmailStatus = new List<UserStatusSelect>();

            //if (ModelState.IsValid)
            //{
            ItemData.SellerContingenciesDataVersion2 = Listdata;
            ;
            //two scenerio ratified offer done/ not done

            foreach (var item in Listdata)
            {

                //if (item.IsApplicable)
                // {
                // ObjManageModel.TrackRatifiedDate = objDal.TrackRatifiedDate(Email, ItemData.DealTracker.TransactionID);
                //select ratified offer date and add no of days
                // ItemData.DealTracker.UpdatedOn = ObjManageModel.TrackRatifiedDate.UpdatedOn;
                // }

                if (ItemData.SellerClosingConfig.SellerStatusID == item.SellerStatusID && ItemData.MstClosingConfig.utblMstClosingDate.ListingTypeID>0)//House goes live
                {
                    //send listdata to ratified offer
                    //string url = Url.Action("closingconfiguration", new { Email = ItemData.UserInfo.Email, StatusID = item.StatusID, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, UpdatedOn = ItemData.utblMstTrackDeal.UpdatedOn, Listdata = JsonConvert.SerializeObject(Listdata), AgentID = ItemData.TrackDealMasterView.AgentID });
                    //return Json(new { success = true, val = 1, url = url });
                    ItemData.MstClosingConfig.utblMstClosingDate.Email = ItemData.UserInfo.Email;
                    ItemData.MstClosingConfig.utblMstClosingDate.ClientID = ItemData.DealTracker.ClientID;
                    ItemData.MstClosingConfig.utblMstClosingDate.UpdatedOn = ItemData.DealTracker.UpdatedOn;

                    if (ItemData.MstClosingConfig.utblMstClosingDate.ListingTypeID == 1)
                    {
                        if (!string.IsNullOrEmpty(ListingSHA))
                        {
                            ItemData.MstClosingConfig.utblMstClosingDate.Address = ItemData.MstClosingConfig.utblMstClosingDate.MLSID;
                            ItemData.MstClosingConfig.utblMstClosingDate.MLSID = ListingSHA;

                        }

                        //else
                        //{
                        //    ModelState.AddModelError("", "Please re-select valid address from the dropdown");
                        //    //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
                        //    ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
                        //    return PartialView("pvCreateRatifiedOffer", ItemData);
                        //}
                    }
                    TempData["ErrMsg"] = ObjStatus.SaveSellerClosingConfigurationVersion2(ItemData.MstClosingConfig.utblMstClosingDate, item.SellerStatusID, ItemData.DealTracker.TransactionID, Agent, ItemData.SellerContingenciesDataVersion2);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                    }
                    else
                    {
                        // TempData["ErrMsg"] = "Please add address to complete Ratified Offer step.";
                        // return Json(new { success = false, val = 0});
                        //TempData["ErrMsg"] = 5;
                        //return Json(ObjManageModel, JsonRequestBehavior.AllowGet);
                       // return Json("Please add address to complete Ratified Offer step.", JsonRequestBehavior.AllowGet);
                    }
                }

                //var Data = ObjStatus.SelectBulkStatus(Email, item.StatusID);

                //ItemData.BulkEmailStatus.Add(Data);
            }
            TempData["ErrMsg"] = ObjStatus.SaveSellerVersion2(ItemData.DealTracker, Listdata, Agent);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
                ItemData.BulkEmailStatus = ObjStatus.SelectSellerBulkStatusVersion2(Email, ItemData.DealTracker.TransactionID);

                if (ItemData.BulkEmailStatus.Count > 0)
                {



                    if (ObjStatus.emailPreferences(ItemData.UserInfo.Email))
                    {
                        string Date = DateTime.Now.ToString("dd MMM yyyy HH:mm");
                        string EmailID = ItemData.DealTracker.Email;
                        ItemData.MstAgentClientNameSelect = ObjStatus.GetNameEmail(ItemData.DealTracker.TransactionID);
                        //bulk mail
                        int MailStatus = SendEmailConfirmationToken(EmailID, ItemData.MstUserStatusSelect.ClientName, Date, ItemData.BulkEmailStatus, ItemData.MstAgentClientNameSelect.AgentName, ItemData.MstAgentClientNameSelect.AgentPhone, ItemData.MstAgentClientNameSelect.AgentEmail);
                        //if (MailStatus == 0)
                        //{

                        //    TempData["ErrMsg"] = 0;
                        //}
                        //else
                        //{
                        //    TempData["ErrMsg"] = null;
                        //}
                    }

                    ObjStatus.DeleteSendEmail(ItemData.DealTracker.TransactionID);




                }
                TempData["ErrMsg"] = 0;
            }
            //return RedirectToAction("Buyer", new { ClientID = ItemData.DealTracker.ClientID, TransactionID = ItemData.DealTracker.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
            //}
            //return Json(new { success = true, val = 0 });

            // ViewBag.ActiveURL = "DealTracker";
            //   string Email = await UserManager.GetEmailAsync(ItemData.DealTracker.ClientID);



            //added fgfhdfh
            //ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            //ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            //ObjManageModel.MstClosingConfig.HomeTypeDDL = objDal.HomeTypeDDL();
            //ObjManageModel.MstClosingConfig.StateDDL = objDal.StateDDL();
            //ObjManageModel.CondoStepDDL = objDal.CondoStepDDL();
            //ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            //ObjManageModel.DealTracker = new DealTracker();
            //ObjManageModel.DealTracker.Email = Email;
            //// ObjManageModel.StatusList = objDal.StatusList(Email, TransactionID);
            //ObjManageModel.TransactionDetails = objDal.GetTransactionDetails(ItemData.DealTracker.TransactionID);
            //ObjManageModel.TrackClosingDateID = objDal.TrackClosingDateID(Email, ItemData.DealTracker.TransactionID);
            //ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            //ObjManageModel.DealTrackerList = objDal.TrackDealStatusListVersion2(Email, ItemData.DealTracker.TransactionID);
            //ObjManageModel.MstClosingConfig.utblMstClosingDate = objDal.GetRatifiedDetails(ItemData.DealTracker.TransactionID);
            //ObjManageModel.CheckListItems = objDal.GetChecklistItems();
            //ObjManageModel.TrackDealMasterView = new TrackDealMasterView()
            //{
            //    AgentID = ItemData.TrackDealMasterView.AgentID,
            //    ClientID = ItemData.DealTracker.ClientID,
            //    TransactionID = ItemData.DealTracker.TransactionID
            //};


            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("pvBuyerDealTracker", ObjManageModel);
            //}
            //return View(ObjManageModel);
            //return RedirectToAction("Buyer", new { ClientID = ItemData.DealTracker.ClientID, TransactionID = ItemData.DealTracker.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
            return Json(ObjManageModel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Documentautocomplete(string term, string Email, string TransactionID)
        {
            ObjManageModel.DocumentAutoComplete = ObjStatus.BuyerDocumentListVersion2(term, Email, TransactionID);
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in ObjManageModel.DocumentAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.DocID));

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DealDocumentList(string Email, string TransactionID)
        {
            ObjManageModel.MstBuyerDocumentListView = ObjStatus.BuyerDocumentListViewVersion2(Email, TransactionID);
            ObjManageModel.ClientDealDocumentList = ObjStatus.GetClientDealDocumentByIdVersion2(TransactionID);//Client Documents
            var result = ObjManageModel;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddDealDocument(utblMstTrackDealDoc TrackDealDoc, utblMstBuyerDocument BuyerDoc, HttpPostedFileBase DocFile)
        {
            string ID = objDal.GetTrackingID(TrackDealDoc.Email, TrackDealDoc.TransactionID);
            DealTracker ItemData = new DealTracker();
            ItemData = ObjStatus.GetLiveDealDetailsByIDVersion2(ID, TrackDealDoc.TransactionID);
            string errorMessage = "";
            if (DocFile != null && DocFile.ContentLength > 0)
            {
                int DocID = TrackDealDoc.DocID;
                TrackDealDoc.TrackingID = ItemData.TrackingID;
                TrackDealDoc.TrackDocFile = Path.GetExtension(DocFile.FileName);
                TrackDealDoc.Email = ItemData.Email;
                TrackDealDoc.StatusID = ItemData.StatusID;
                TrackDealDoc.DocID = DocID;
                TrackDealDoc.TransactionID = ItemData.TransactionID;

                errorMessage = ObjStatus.SaveTrackDealDocVersion2(TrackDealDoc, BuyerDoc);
                if (errorMessage == "0")
                {
                    TempData["ErrMsg"] = null;
                    var TrackingID = ItemData.TrackingID;
                    string ext = Path.GetExtension(DocFile.FileName);
                    var fileName = TrackDealDoc.DealTrackDocID;
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + fileName + ext));
                    DocFile.SaveAs(path);
                    //ItemData.MstLiveDealDocList = ObjStatus.GetLiveDealDocList(TrackingID);
                }
            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }




        public JsonResult DeleteDealDocument(string DealTrackDocID, string TrackingID)
        {
            string errorMessage = "";

            utblMstTrackDealDoc objDoc = new utblMstTrackDealDoc();
            objDoc = ObjStatus.GetDocDetailsByID(DealTrackDocID);
            errorMessage = ObjStatus.DeleteLiveDealDocument(DealTrackDocID, TrackingID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + DealTrackDocID + objDoc.TrackDocFile));
            if (errorMessage == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteClientDealDocument(string DealTrackDocID)
        {
            string errorMessage = "";

            errorMessage = ObjStatus.DeleteClientDealDocument(DealTrackDocID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + ObjStatus.GetClientFileName(DealTrackDocID)));
            if (errorMessage == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }

            return Json(errorMessage, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SellerDocumentautocomplete(string term, string Email, string TransactionID)
        {
            ObjManageModel.DocumentAutoComplete = ObjStatus.SellerDocumentListViewVersion2(term, Email, TransactionID);
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in ObjManageModel.DocumentAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.DocID));

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SellerDealDocumentList(string Email, string TransactionID)
        {
            ObjManageModel.MstSellerDocumentListView = ObjStatus.SellerDocumentListViewVersion2(Email, TransactionID);
            ObjManageModel.ClientDealDocumentList = ObjStatus.GetClientDealDocumentByIdVersion2(TransactionID);//Client Documents
            var result = ObjManageModel;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddSellerDealDocument(utblMstSellerTrackDealDoc TrackDealDoc, utblMstSellerDocument SellerDoc, HttpPostedFileBase DocFile)
        {
            string ID = objDal.GetSellerTrackingID(TrackDealDoc.Email, TrackDealDoc.TransactionID);
            DealTracker ItemData = new DealTracker();
            ItemData = ObjStatus.GetLiveSellerDealDetailsByID(ID, TrackDealDoc.TransactionID);
            string errorMessage = "";
            if (DocFile != null && DocFile.ContentLength > 0)
            {
                int DocID = TrackDealDoc.SellerDocID;
                TrackDealDoc.SellerTrackingID = ItemData.SellerTrackingID;
                TrackDealDoc.TrackDocFile = Path.GetExtension(DocFile.FileName);
                TrackDealDoc.Email = ItemData.Email;
                TrackDealDoc.SellerStatusID = ItemData.SellerStatusID;
                TrackDealDoc.SellerDocID = DocID;
                TrackDealDoc.TransactionID = ItemData.TransactionID;

                errorMessage = ObjStatus.SaveSellerDocVersion2(TrackDealDoc, SellerDoc);
                if (errorMessage == "0")
                {
                    TempData["ErrMsg"] = null;
                    var TrackingID = ItemData.SellerTrackingID;
                    string ext = Path.GetExtension(DocFile.FileName);
                    var fileName = TrackDealDoc.SellerDealDocID;
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + fileName + ext));
                    DocFile.SaveAs(path);
                    //ItemData.MstLiveDealDocList = ObjStatus.GetLiveDealDocList(TrackingID);
                }
            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSellerDealDocument(string DealTrackDocID, string TrackingID)
        {
            string errorMessage = "";

            utblMstSellerTrackDealDoc objDoc = new utblMstSellerTrackDealDoc();
            objDoc = ObjStatus.GetSellerDocDetailsByID(DealTrackDocID);
            errorMessage = ObjStatus.DeleteSellerLiveDealDocument(DealTrackDocID, TrackingID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + DealTrackDocID + objDoc.TrackDocFile));
            if (errorMessage == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GalleryList(string Email, string TransactionID)
        {
            objMediaModel.MstGalleryList = objMediaDal.GetPhotoGalleryList(Email, TransactionID);
            var result = objMediaModel.MstGalleryList;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddDealGallery(FormCollection form, utblMstClientGallerie ItemData, HttpPostedFileBase DocFile, string AgentID = "")
        {
            string errorMessage = "";

            ViewBag.AgentID = AgentID;
            var image = form["upfiles"];
            //var EditStatus = form["IsEdited"];
            if (!string.IsNullOrEmpty(image))
            {
                if (ItemData.PhotoNormal != null)
                {
                    if (ItemData.PhotoNormal.IndexOf("data:image") == -1)
                    {
                        string ExistingFilePath = Server.MapPath("/UploadedFiles/PhotoGallery/" + ItemData.GallaryID + ItemData.PhotoNormal);
                        FileInfo ExistingFileInfo = new FileInfo(ExistingFilePath);
                        if (ExistingFileInfo.Exists)
                            ExistingFileInfo.Delete();
                    }
                }
                if (image == "PhotoDeleted")
                {

                    ItemData.PhotoNormal = "";
                    ItemData.PhotoThumb = "";
                }
                else
                {
                    dynamic data = JObject.Parse(image);
                    string imagename = data.output.name;
                    string imageext = imagename.Substring(imagename.LastIndexOf('.'));
                    string imagedata = data.output.image;
                    string imageType = imagedata.Substring(0, imagedata.IndexOf(',', imagedata.IndexOf(',')) + 1);
                    string convert = imagedata.Replace(imageType, String.Empty);


                    var guid = Guid.NewGuid().ToString().Substring(0, 6);


                    ItemData.PhotoNormal = guid + imageext;
                    ItemData.PhotoThumb = imagedata;
                    errorMessage = objMediaDal.SaveNew(ItemData);
                    if (errorMessage == "0")
                    {
                        byte[] image64 = Convert.FromBase64String(convert);
                        using (var ms = new MemoryStream(image64))
                        {

                            var images = System.Drawing.Image.FromStream(ms);
                            //if (EditStatus == "Yes")
                            //{
                            System.Drawing.Image img = images.GetThumbnailImage(Convert.ToInt32(data.output.width), Convert.ToInt32(data.output.height), null, IntPtr.Zero);
                            img.Save(Server.MapPath("/UploadedFiles/PhotoGallery/" + ItemData.GallaryID + guid + imageext), System.Drawing.Imaging.ImageFormat.Jpeg);

                            //}
                            //else
                            //{
                            //    images.Save(Server.MapPath("/UploadedFiles/PhotoGallery/" + ItemData.utblMstClientGalleries.GallaryID + guid + imageext), System.Drawing.Imaging.ImageFormat.Jpeg);

                            //}
                        }

                    }

                }


            }
            else
            {

                errorMessage = objMediaDal.SaveNew(ItemData);

            }

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteGalleryDocument(string GallaryID)
        {
            string errorMessage = "";
            utblMstClientGallerie objGallary = new utblMstClientGallerie();
            objGallary = objMediaDal.GetPhotoGalleryByID(GallaryID);
            errorMessage = objMediaDal.DeleteGallery(GallaryID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/PhotoGallery/" + GallaryID + objGallary.PhotoNormal));
            if (errorMessage == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Vendorautocomplete(string term)
        {

            objCategoryModel.ListSearchAutoComplete = ObjStatus.VendorTypeAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objCategoryModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ClientVendorautocomplete(string term, string VendorType)
        {

            objCategoryModel.ClientVendorsListSearchAutoComplete = ObjStatus.ClientVendorAutoComplete(term, VendorType);
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in objCategoryModel.ClientVendorsListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.Phone + "}" + item.Email + "}" + item.CatgoryName + "}" + item.VendorId));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
 
        public JsonResult GetVendorContactByVendorId(string term, string VendorId)
        {
            

            objCategoryModel.VendorContactsAutocomplete = ObjStatus.GetVendorContact(term, VendorId);
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in objCategoryModel.VendorContactsAutocomplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.ContactPhone + "}" + item.ContactTitle + "}" + item.VendorContactId));

            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DealVendorList(string Email, string TransactionID)
        {
            return Json(ObjStatus.GetDealVendorList(Email, TransactionID), JsonRequestBehavior.AllowGet);
        }




        public ActionResult AddDealVendor(FormCollection form)
        {
            string errorMessage = "";

            string Email = string.Empty;
            //string OutEmail;
           
                ObjModel.MstUserClientView = ObjDal.ClientView(form["TransactionID"].ToString());
                Email = ObjModel.MstUserClientView.Email;
                var Agent = UserManager.FindById(User.Identity.GetUserId());

                //ObjModel.MultipleDealList = ObjDal.AgentMultipleDeal(Agent.Id, ClientID);
    


        

            Vendor vendor = new Vendor();
            vendor.Category_Id = form["VendorType"].ToString();
            vendor.Title = form["txtTitleV"].ToString();
            //vendor.SubTitle = form["SubTitle"].ToString();
            vendor.Email = form["VendorEmail"].ToString();
            //vendor.About = form["About"].ToString();
            //vendor.WebsiteLink = form["Websitelink"].ToString();
            vendor.Phone = form["VendorPhone"].ToString();
            //vendor.Location = form["Location"].ToString();
            vendor.CreatedBy = User.Identity.GetUserName();

            //if (VenFile != null && VenFile.ContentLength > 0)
            //{
            //    var guid = Guid.NewGuid().ToString().Substring(0, 4);
            //    string Filename = guid + VenFile.FileName;lename;
            //    errorMessage = objVendorDal.SaveClientVendorClient(vendor, Email, form["TransactionID"].ToString());
            //    if (errorMessage == 
            //    vendor.VendorImage = Fi"0")
            //    {
            //        var path = string.Concat(Server.MapPath("~/img/vendors/" + Filename));
            //        VenFile.SaveAs(path);
            //        return Json(errorMessage, JsonRequestBehavior.AllowGet);
            //    }
            //}

            VendorContacts contact = new VendorContacts();
            contact.VendorContactId= Convert.ToInt32(string.IsNullOrEmpty(form["VendorContactId"].ToString())?"0": form["VendorContactId"].ToString());
            contact.ContactName = form["ContactName"].ToString();
            contact.ContactTitle= form["ContactTitle"].ToString();
            contact.ContactPhone = form["ContactPhone"].ToString();
         

            errorMessage = ObjStatus.SaveDealVendorVersion2(vendor,contact, User.Identity.GetUserName(), Email, form["TransactionID"].ToString());
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDealVendor(string DealVendorId)
        {
            string errorMessage = "";

            DealVendor objVendor = new DealVendor();
            //objVendor = ObjStatus.GetDealVendorByID(DealVendorId);
            errorMessage = ObjStatus.DeleteDealVendor(DealVendorId);

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SetDealPrice(FormCollection form)
        {
            string errorMessage = "";
            decimal DealPrice = 0;
            if (String.IsNullOrEmpty(form["DealPrice"].ToString())) { }
            else
             DealPrice = Convert.ToDecimal(form["DealPrice"].ToString());
           
         
            string TransactionID = (form["TransactionID"].ToString());
            errorMessage = objDal.SetDealPrice(TransactionID,DealPrice);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        private int SendEmailConfirmationToken(string email, string Name, string Date, IEnumerable<UserStatusSelect> Status, string AgentName, string AgentNumber, string AgentEmail)
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
            mm.CC.Add(AgentEmail);
            mm.Priority = MailPriority.High;
            mm.Subject = "Your Deal Tracker Notification";


            mm.Body = this.PopulateBody(Name, Date, Status, AgentName, AgentNumber, email, AgentEmail);
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

        private string PopulateBody(string Name, string Date, IEnumerable<UserStatusSelect> Status, string AgentName, string AgentNumber, string ClientEmail, string AgentEmail)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/Status.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Date}", Date);
            string content = "";
            string EmailData = "";
            var Client = UserManager.FindByEmail(ClientEmail);
            var Agent = UserManager.FindByEmail(AgentEmail);


            string Url = "http://justbere.com/Coordinator/ActivityLogs/Track?";
            var uriBuilder = new UriBuilder(Url);
            uriBuilder.Port = -1;
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["ClientID"] = Client.Id;
            query["AgentID"] = Agent.Id;
            query["Remarks"] = "Opened";
            query["TrackingSource"] = "Email";


            foreach (var item in Status)
            {
                if (item.Status != null)
                {
                    body = body.Replace("{Name}", item.ClientName);

                    content = content + item.Status + "<br/>";
                    EmailData = EmailData + item.Status + ",";
                }


            }
            body = body.Replace("{Status}", content);
            body = body.Replace("{AgentName}", AgentName);
            body = body.Replace("{AgentNumber}", AgentNumber);




            query["ActivityTitle"] = EmailData;

            uriBuilder.Query = query.ToString();
            string ActualUrl = uriBuilder.ToString();

            body = body.Replace("{TrackURL}", ActualUrl);

            return body;
        }
        #region AutoComplete

        public async Task<ActionResult> autocomplete(string term)
        {
            MstCompassAutocompleteView Obj = new MstCompassAutocompleteView();
            Obj = await GetAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in Obj.addresses_info)
            {
                result.Add(new KeyValuePair<string, string>(item.address + " " + "-" + " " + "|" + " " + item.bedrooms + " " + "Bed" + " " + "|" + " " + item.bathrooms + " " + "Bath" + " " + "|" + " " + "$" + " " + item.price.ToString("#,##0"), item.listingIdSHA));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public static async Task<MstCompassAutocompleteView> GetAutoComplete(string term)
        {
            MstCompassAutocompleteView ObjviewModel = new MstCompassAutocompleteView();
            WebClient wclient = new WebClient();
            // string MLSURL = "https://www.compass.com/api/search_suggest/homepage?";
            string MLSURL = "https://www.compass.com/api/v3/search/suggest/homepage?";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["q"] = term;
            query["type"] = "listings";
            query["num"] = "25";
            query["geography"] = "dc";

            uriBuilder.Query = query.ToString();
            MLSURL = uriBuilder.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(MLSURL);
                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<MstCompassAutocompleteView>(result);
                ObjviewModel = list;
            }
            return ObjviewModel;
        }

        #endregion
    }
}