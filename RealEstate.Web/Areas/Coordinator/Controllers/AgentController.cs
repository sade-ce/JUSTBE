using HtmlAgilityPack;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]
    public class AgentController : Controller
    {
        dalMstClientList objDal = new dalMstClientList();
        MstClientListManageModel model = new MstClientListManageModel();
        MstClientDealCreateManageModel ObjManageModel = new MstClientDealCreateManageModel();
        dalTrackDeal ObjStatus = new dalTrackDeal();
        dalMstClientMLSHomeListing Objdal = new dalMstClientMLSHomeListing();
        RootObject ObjviewModel = new RootObject();
        MstAgentPhotoGalleryManageModel objMediaModel = new MstAgentPhotoGalleryManageModel();
        dalMstAgentPhotoGallery objMediaDal = new dalMstAgentPhotoGallery();
        DealAdminAssignAgentManageModel ObjModel = new DealAdminAssignAgentManageModel();
        MstAgentDealShareManageModel ObjAgentShare = new MstAgentDealShareManageModel();
        dalDealAdmin ObjDal = new dalDealAdmin();
        dalAgentDealSharing ObjAgentDealShare = new dalAgentDealSharing();
        DealVendorViewModel objDealVendorModel = new DealVendorViewModel();
        dalDealVendor objMeDealVendorDal = new dalDealVendor();
        BuildingViewModel objBuildingModel = new BuildingViewModel();
        dalBuildings objBuildingDal = new dalBuildings();

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
        public ActionResult Index(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {
            string AgentEmail = User.Identity.Name;
            int TotalRow;
            model.MstClientList = objDal.AgentClientPaged(PageNo, PageSize, out TotalRow, AgentEmail, SearchTerm);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClientList", model);
            }
            return View(model);
        }

        #region CreateDeal
        public async Task<ActionResult> CreateDeal(string ClientID, string TransactionID, string AgentID = "")
        {
            string Email = await UserManager.GetEmailAsync(ClientID);
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.StatusList = objDal.StatusList(Email, TransactionID);
            ObjManageModel.TrackClosingDateID = objDal.TrackClosingDateID(Email, TransactionID);
            ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            ObjManageModel.TrackDealStatusList = objDal.TrackDealStatusList(Email, TransactionID);
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView()
            {
                AgentID = AgentID,
                ClientID = ClientID,
                TransactionID = TransactionID

            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClientStatusList", ObjManageModel);
            }
            return View(ObjManageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDeal(MstClientDealCreateManageModel ItemData, List<MstContingenciesView> Listdata)
        {
            string Agent = "";
            Agent = User.Identity.Name;
            if (User.IsInRole("Admin"))
            {
                Agent = await UserManager.GetEmailAsync(ItemData.TrackDealMasterView.AgentID);
            }

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ItemData.MstUserStatusSelect = new UserStatusSelect();
            ItemData.utblMstTrackDeal.Email = ItemData.UserInfo.Email;
            string Email = ItemData.UserInfo.Email;
            ItemData.BulkEmailStatus = new List<UserStatusSelect>();

            //if (ModelState.IsValid)
            //{
            ItemData.ContingenciesData = Listdata;

            //if(ItemData.ClosingConfig.StatusID==ItemData.utblMstTrackDeal.StatusID)
            //{
            //    return RedirectToAction("ClosingConfig", new { Email = ItemData.UserInfo.Email, StatusID = ItemData.utblMstTrackDeal.StatusID, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID });
            //}
            //two scenerio ratified offer done/ not done

            foreach (var item in Listdata)
            {

                if (item.IsApplicable)
                {
                    ObjManageModel.TrackRatifiedDate = objDal.TrackRatifiedDate(Email, ItemData.utblMstTrackDeal.TransactionID);
                    //select ratified offer date and add no of days
                    ItemData.utblMstTrackDeal.UpdatedOn = ObjManageModel.TrackRatifiedDate.UpdatedOn;
                }

                if (ItemData.ClosingConfig.StatusID == item.StatusID)
                {
                    //send listdata to ratified offer
                    return RedirectToAction("ClosingConfig", new { Email = ItemData.UserInfo.Email, StatusID = item.StatusID, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, UpdatedOn = ItemData.utblMstTrackDeal.UpdatedOn, Listdata = JsonConvert.SerializeObject(Listdata), AgentID = ItemData.TrackDealMasterView.AgentID });

                }
                var Data = ObjStatus.SelectBulkStatus(Email, item.StatusID);
                ItemData.BulkEmailStatus.Add(Data);
            }
            TempData["ErrMsg"] = ObjStatus.Save(ItemData.utblMstTrackDeal, Listdata, Agent);

            //TempData["ErrMsg"] = ObjStatus.Save(ItemData.utblMstTrackDeal);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;

                if (ObjStatus.emailPreferences(ItemData.UserInfo.Email))
                {
                    string Date = DateTime.Now.ToString("dd MMM yyyy HH:mm");
                    string EmailID = ItemData.utblMstTrackDeal.Email;
                    //ItemData.MstUserStatusSelect = ObjStatus.GetClientStatusfromEmail(Email, ItemData.utblMstTrackDeal.StatusID);
                    ItemData.MstAgentClientNameSelect = ObjStatus.GetNameEmail(ItemData.utblMstTrackDeal.TransactionID);
                    //bulk mail
                    int MailStatus = SendEmailConfirmationToken(EmailID, ItemData.MstUserStatusSelect.ClientName, Date, ItemData.BulkEmailStatus, ItemData.MstAgentClientNameSelect.AgentName, ItemData.MstAgentClientNameSelect.AgentPhone, ItemData.MstAgentClientNameSelect.AgentEmail);
                    if (MailStatus == 0)
                    {

                        TempData["MailMsg"] = 1;
                    }
                    else
                    {
                        TempData["ErrMsg"] = null;
                    }
                }

                return RedirectToAction("CreateDeal", new { ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
            }
            //}
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.StatusList = objDal.StatusList(Email, ItemData.utblMstTrackDeal.TransactionID);
            ObjManageModel.TrackClosingDate = objDal.TrackClosingDate(Email, ItemData.utblMstTrackDeal.TransactionID);
            ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            ObjManageModel.TrackDealStatusList = objDal.TrackDealStatusList(Email, ItemData.utblMstTrackDeal.TransactionID);
            return View(ObjManageModel);
        }
        #endregion
        #region ClosingConfig
        public ActionResult ClosingConfig(string Email, int StatusID, string ClientID, string TransactionID, DateTime UpdatedOn, string Listdata, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ObjManageModel.UserInfo = new MstInfoView();
            ObjManageModel.utblMstTrackDeal = new utblMstTrackDeal();

            ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView();
            ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ObjManageModel.UserInfo.Email = Email;
            List<MstContingenciesView> Contengencies = JsonConvert.DeserializeObject<List<MstContingenciesView>>(Listdata);
            ObjManageModel.ContingenciesData = Contengencies;
            ObjManageModel.TrackDealMasterView.AgentID = AgentID;
            ObjManageModel.utblMstTrackDeal = new utblMstTrackDeal()
            {
                StatusID = StatusID,
                ClientID = ClientID,
                TransactionID = TransactionID,
                UpdatedOn = UpdatedOn
            };
            return View(ObjManageModel);
        }
        [HttpPost]
        public ActionResult ClosingConfig(MstClientDealCreateManageModel ItemData)
        {
            string Agent = User.Identity.Name;

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            ItemData.MstClosingConfig.utblMstClosingDate.Email = ItemData.UserInfo.Email;
            ItemData.MstClosingConfig.utblMstClosingDate.ClientID = ItemData.utblMstTrackDeal.ClientID;
            ItemData.MstClosingConfig.utblMstClosingDate.UpdatedOn = ItemData.utblMstTrackDeal.UpdatedOn;
            if (ModelState.IsValid)
            {
                if (ItemData.MstClosingConfig.utblMstClosingDate.ListingTypeID == 1)
                {
                    string MLSID = ItemData.MstClosingConfig.utblMstClosingDate.MLSID;
                    string HomeAddress = ListingDetails(MLSID);
                    if (!string.IsNullOrEmpty(HomeAddress))
                    {
                        ItemData.MstClosingConfig.utblMstClosingDate.Address = HomeAddress;
                    }
                    else
                    {
                        TempData["ErrMsg"] = "....Please Enter Valid MLS ID...";
                        //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
                        ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
                        return View(ItemData);
                    }
                }

                TempData["ErrMsg"] = ObjStatus.SaveClosingConfiguration(ItemData.MstClosingConfig.utblMstClosingDate, ItemData.utblMstTrackDeal.StatusID, ItemData.utblMstTrackDeal.TransactionID, Agent, ItemData.ContingenciesData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {

                    TempData["ErrMsg"] = ObjStatus.SaveBulk(ItemData.MstClosingConfig.utblMstClosingDate, ItemData.utblMstTrackDeal.StatusID, ItemData.utblMstTrackDeal.TransactionID, Agent, ItemData.ContingenciesData);


                    TempData["ErrMsg"] = null;


                    return RedirectToAction("CreateDeal", new { ClientID = ItemData.MstClosingConfig.utblMstClosingDate.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
                }
            }
            //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
            ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();


            return View(ItemData);

        }

        public ActionResult EditClosingConfig(string TrackingID, string Email, string TransactionID)
        {
            dalRatifiedOffer Objratified = new dalRatifiedOffer();
            ObjManageModel.UserInfo = new MstInfoView();
            ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            ObjManageModel.RatifiedOfferView = new MstRatifiedEditViewModel();
            ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ObjManageModel.UserInfo.Email = Email;
            ObjManageModel.RatifiedOfferView = Objratified.GetRatifiedOfferDetails(TrackingID, TransactionID);
            return PartialView("pvEditRatifiedOffer", ObjManageModel);

        }
        [HttpPost]
        public ActionResult EditClosingConfig(MstClientDealCreateManageModel ItemData, string ListingSHA = "")
        {
            dalRatifiedOffer Objratified = new dalRatifiedOffer();
            //ItemData.UserInfo = new MstInfoView();
            //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
            //ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            //ItemData.UserInfo.Email = ItemData.RatifiedOfferView.Email;
            //ItemData.RatifiedOfferView = Objratified.GetRatifiedOfferDetails(ItemData.RatifiedOfferView.TrackingID);
            if (ModelState.IsValid)
            {
                if (ItemData.RatifiedOfferView.ListingTypeID == 1)
                {
                    //string MLSID = ItemData.RatifiedOfferView.MLSID;
                    //string HomeAddress = ListingDetails(MLSID);
                    if (!string.IsNullOrEmpty(ListingSHA))
                    {
                        ItemData.RatifiedOfferView.Address = ItemData.RatifiedOfferView.MLSID;
                        ItemData.RatifiedOfferView.MLSID = ListingSHA;

                    }
                    else
                    {
                        ModelState.AddModelError("", "Please select valid address from dropdown");
                        ItemData.UserInfo = new MstInfoView();
                        ItemData.MstClosingConfig = new MstClosingConfigViewModel();
                        ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
                        ItemData.UserInfo.Email = ItemData.RatifiedOfferView.Email;
                        ItemData.RatifiedOfferView = Objratified.GetRatifiedOfferDetails(ItemData.RatifiedOfferView.TrackingID, ItemData.RatifiedOfferView.TransactionID);
                        return PartialView("pvEditRatifiedOffer", ItemData);

                    }
                }

                TempData["ErrMsg"] = Objratified.Save(ItemData.RatifiedOfferView);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    //string url = Url.Action("UploadDealDocument", new { Email = Itemdata.utblMstTrackDeal.Email, ClientID = Itemdata.utblMstTrackDeal.ClientID, TransactionID = Itemdata.utblMstTrackDeal.TransactionID });
                    return Json(new { success = true });
                }
            }
            ItemData.UserInfo = new MstInfoView();
            ItemData.MstClosingConfig = new MstClosingConfigViewModel();
            ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ItemData.UserInfo.Email = ItemData.RatifiedOfferView.Email;
            ItemData.RatifiedOfferView = Objratified.GetRatifiedOfferDetails(ItemData.RatifiedOfferView.TrackingID, ItemData.RatifiedOfferView.TransactionID);
            return PartialView("pvEditRatifiedOffer", ItemData);

        }
        public ActionResult ClosingConfigView(string Email, int StatusID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            string url = Url.Action("ClosingConfig", "Agent", new { area = "Coordinator", Email = Email, StatusID = StatusID });
            return Json(new { success = true, url = url }, JsonRequestBehavior.AllowGet);
        }


        //sulochan

        public ActionResult ClosingConfiguration(string Email, int StatusID, string ClientID, string TransactionID, DateTime UpdatedOn, string Listdata, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ObjManageModel.UserInfo = new MstInfoView();
            ObjManageModel.utblMstTrackDeal = new utblMstTrackDeal();
            ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView();
            ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ObjManageModel.UserInfo.Email = Email;
            List<MstContingenciesView> Contengencies = JsonConvert.DeserializeObject<List<MstContingenciesView>>(Listdata);
            ObjManageModel.ContingenciesData = Contengencies;
            ObjManageModel.TrackDealMasterView.AgentID = AgentID;
            ObjManageModel.utblMstTrackDeal = new utblMstTrackDeal()
            {
                StatusID = StatusID,
                ClientID = ClientID,
                TransactionID = TransactionID,
                UpdatedOn = UpdatedOn
            };
            return PartialView("pvCreateRatifiedOffer", ObjManageModel);

        }

        [HttpPost]
        public ActionResult ClosingConfiguration(MstClientDealCreateManageModel ItemData, string ListingSHA = "")
        {
            string Agent = User.Identity.Name;

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            ItemData.MstClosingConfig.utblMstClosingDate.Email = ItemData.UserInfo.Email;
            ItemData.MstClosingConfig.utblMstClosingDate.ClientID = ItemData.utblMstTrackDeal.ClientID;
            ItemData.MstClosingConfig.utblMstClosingDate.UpdatedOn = ItemData.utblMstTrackDeal.UpdatedOn;
            ItemData.BulkEmailStatus = new List<UserStatusSelect>();
            if (ModelState.IsValid)
            {
                if (ItemData.MstClosingConfig.utblMstClosingDate.ListingTypeID == 1)
                {
                    //string MLSID = ListingSHA;
                    //string HomeAddress = ListingDetails(MLSID);
                    if (!string.IsNullOrEmpty(ListingSHA))
                    {
                        ItemData.MstClosingConfig.utblMstClosingDate.Address = ItemData.MstClosingConfig.utblMstClosingDate.MLSID;
                        ItemData.MstClosingConfig.utblMstClosingDate.MLSID = ListingSHA;

                    }

                    else
                    {
                        ModelState.AddModelError("", "Please re-select valid address from the dropdown");
                        //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
                        ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
                        return PartialView("pvCreateRatifiedOffer", ItemData);
                    }
                }
                if (ItemData.MstClosingConfig.utblMstClosingDate.UpdatedOn == DateTime.MinValue)
                {
                    ModelState.AddModelError("", "please enter date you missed for ratified offer");

                }
                else
                {
                    TempData["ErrMsg"] = ObjStatus.SaveClosingConfiguration(ItemData.MstClosingConfig.utblMstClosingDate, ItemData.utblMstTrackDeal.StatusID, ItemData.utblMstTrackDeal.TransactionID, Agent, ItemData.ContingenciesData);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = ObjStatus.SaveBulk(ItemData.MstClosingConfig.utblMstClosingDate, ItemData.utblMstTrackDeal.StatusID, ItemData.utblMstTrackDeal.TransactionID, Agent, ItemData.ContingenciesData);
                        foreach (var item in ItemData.ContingenciesData)
                        {
                            var Data = ObjStatus.SelectBulkStatus(ItemData.UserInfo.Email, item.StatusID);
                            ItemData.BulkEmailStatus.Add(Data);
                        }

                        if (ObjStatus.emailPreferences(ItemData.UserInfo.Email))
                        {
                            string Date = DateTime.Now.ToString("dd MMM yyyy HH:mm");
                            ItemData.MstAgentClientNameSelect = ObjStatus.GetNameEmail(ItemData.utblMstTrackDeal.TransactionID);
                            //bulk mail
                            var Client = UserManager.FindByEmail(ItemData.UserInfo.Email);
                            int MailStatus = SendEmailConfirmationToken(ItemData.UserInfo.Email, Client.Name, Date, ItemData.BulkEmailStatus, ItemData.MstAgentClientNameSelect.AgentName, ItemData.MstAgentClientNameSelect.AgentPhone, ItemData.MstAgentClientNameSelect.AgentEmail);
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
                    string url = Url.Action("ManageDeal", new { ClientID = ItemData.MstClosingConfig.utblMstClosingDate.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
                    return Json(new { success = true, url = url, type = "editRO" });
                }
            }
            //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
            ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            return PartialView("pvCreateRatifiedOffer", ItemData);


        }
        public ActionResult EditClosingConfigPV(string TrackingID, string Email, string TransactionID)
        {
            dalRatifiedOffer Objratified = new dalRatifiedOffer();
            ObjManageModel.UserInfo = new MstInfoView();
            ObjManageModel.MstClosingConfig = new MstClosingConfigViewModel();
            ObjManageModel.RatifiedOfferView = new MstRatifiedEditViewModel();
            ObjManageModel.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ObjManageModel.UserInfo.Email = Email;
            ObjManageModel.RatifiedOfferView = Objratified.GetRatifiedOfferDetails(TrackingID, TransactionID);
            ViewBag.ListingSHA = ObjManageModel.RatifiedOfferView.MLSID;
            ObjManageModel.RatifiedOfferView.MLSID = ObjManageModel.RatifiedOfferView.Address;


            return PartialView("pvEditRatifiedOffer_ver1", ObjManageModel);

        }
        [HttpPost]
        public ActionResult EditClosingConfigPV(MstClientDealCreateManageModel ItemData, string ListingSHA = "")
        {
            dalRatifiedOffer Objratified = new dalRatifiedOffer();
            //ItemData.UserInfo = new MstInfoView();
            //ItemData.MstClosingConfig = new MstClosingConfigViewModel();
            //ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            //ItemData.UserInfo.Email = ItemData.RatifiedOfferView.Email;
            //ItemData.RatifiedOfferView = Objratified.GetRatifiedOfferDetails(ItemData.RatifiedOfferView.TrackingID);
            if (ModelState.IsValid)
            {
                if (ItemData.RatifiedOfferView.ListingTypeID == 1)
                {
                    //string MLSID = ItemData.RatifiedOfferView.MLSID;
                    //string HomeAddress = ListingDetails(MLSID);
                    if (!string.IsNullOrEmpty(ListingSHA))
                    {
                        ItemData.RatifiedOfferView.Address = ItemData.RatifiedOfferView.MLSID;
                        ItemData.RatifiedOfferView.MLSID = ListingSHA;

                    }

                    else
                    {
                        ModelState.AddModelError("", "Please select valid address from dropdown");
                        //TempData["ErrMsg"] = "....Please Enter Valid MLS ID...";
                        //ItemData.MstClosingConfig = new MstClosingConfigViewModel();

                        ItemData.UserInfo = new MstInfoView();
                        ItemData.MstClosingConfig = new MstClosingConfigViewModel();
                        ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
                        ItemData.UserInfo.Email = ItemData.RatifiedOfferView.Email;
                        ItemData.RatifiedOfferView = Objratified.GetRatifiedOfferDetails(ItemData.RatifiedOfferView.TrackingID, ItemData.RatifiedOfferView.TransactionID);
                        return PartialView("pvEditRatifiedOffer_ver1", ItemData);

                    }
                }

                TempData["ErrMsg"] = Objratified.Save(ItemData.RatifiedOfferView);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    //string url = Url.Action("UploadDealDocument", new { Email = Itemdata.utblMstTrackDeal.Email, ClientID = Itemdata.utblMstTrackDeal.ClientID, TransactionID = Itemdata.utblMstTrackDeal.TransactionID });
                    //string url = Url.Action("ManageDeal", new { ClientID = ItemData.MstClosingConfig.utblMstClosingDate.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
                    return Json(new { success = true });

                    //return Json(new { success = true, url = url, type = "editRO" });
                }
            }
            ItemData.UserInfo = new MstInfoView();
            ItemData.MstClosingConfig = new MstClosingConfigViewModel();
            ItemData.MstClosingConfig.ListingTypeDDL = objDal.ListingTypeDDL();
            ItemData.UserInfo.Email = ItemData.RatifiedOfferView.Email;
            ItemData.RatifiedOfferView = Objratified.GetRatifiedOfferDetails(ItemData.RatifiedOfferView.TrackingID, ItemData.RatifiedOfferView.TransactionID);
            return PartialView("pvEditRatifiedOffer", ItemData);

        }



        #endregion

        #region Delete TrackDeal Record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveDeal(string TrackingID, string Email, int StatusID, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            TempData["ErrMsg"] = ObjStatus.RemoveDeal(TrackingID, Email, StatusID, TransactionID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }

            string url = Url.Action("CreateDeal", "Agent", new { area = "Coordinator", ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
            return Json(new { success = true, url = url });
            //return RedirectToAction("CreateDeal", new { Email = Email });

        }

        #endregion

        #region  UploadDealDocument
        public ActionResult UploadDealDocument(string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            string ID = objDal.GetTrackingID(Email, TransactionID);
            ObjManageModel.utblMstTrackDeal = new utblMstTrackDeal();
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView();
            ObjManageModel.MstCalenderConfigList = objDal.MstCalenderConfigList;
            ObjManageModel.utblMstTrackDeal = ObjStatus.GetLiveDealDetailsByID(ID, TransactionID);
            ObjManageModel.MstBuyerDocumentListView = ObjStatus.BuyerDocumentListView(Email, TransactionID);
            ObjManageModel.MstBuyerDocList = ObjStatus.BuyerDocumentList(Email, TransactionID);
            ObjManageModel.UserProfile = ObjStatus.GetUserDetails(ClientID);
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
        public ActionResult UploadDealDocument(HttpPostedFileBase files, MstDealViewModel ItemData, MstClientDealCreateManageModel Item)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            if (files != null && files.ContentLength > 0)
            {
                int DocID = ItemData.utblMstTrackDealDoc.DocID;
                ItemData.utblMstTrackDealDoc = new utblMstTrackDealDoc();
                ItemData.utblMstTrackDealDoc.TrackingID = ItemData.utblMstTrackDeal.TrackingID;
                ItemData.utblMstTrackDealDoc.TrackDocFile = Path.GetExtension(files.FileName);
                ItemData.utblMstTrackDealDoc.Email = ItemData.utblMstTrackDeal.Email;
                ItemData.utblMstTrackDealDoc.StatusID = ItemData.utblMstTrackDeal.StatusID;
                ItemData.utblMstTrackDealDoc.DocID = DocID;
                ItemData.utblMstTrackDealDoc.TransactionID = ItemData.utblMstTrackDeal.TransactionID;
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
                    return RedirectToAction("UploadDealDocument", new { Email = Item.utblMstTrackDeal.Email, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, AgentID = Item.TrackDealMasterView.AgentID });
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
        public ActionResult DeleteDocument(string DealTrackDocID, string ClientID, string TrackingID, string Email, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

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
            return RedirectToAction("UploadDealDocument", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
        }
        #endregion

        #region DealVendor
        public ActionResult DealVendor(string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            //  string ID = objDal.GetTrackingID(Email, TransactionID);
            objDealVendorModel.DealVendorList = objMeDealVendorDal.GetDealVendorList(Email, TransactionID);
            objDealVendorModel.CategoryDropDown = objMeDealVendorDal.GetVendorCategoryDDl();
            objDealVendorModel.DealVendor = new DealVendor();
            objDealVendorModel.DealVendor.Email = Email;
            //  objDealVendorModel.DealVendor.TrackingId = ID;
            objDealVendorModel.DealVendor.Transaction_Id = TransactionID;
            objDealVendorModel.UserProfile = objMeDealVendorDal.GetUserDetails(ClientID);
            ViewBag.AgentID = AgentID;

            return PartialView("pvDealVendor", objDealVendorModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetVendorByCategory(string VendorType)
        {

            return Json(objMeDealVendorDal.GetVendorDDl(VendorType), JsonRequestBehavior.AllowGet);

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetVendorByVendorId(string VendorId)
        {
           
            DealVendorView Vendor = new DealVendorView();
            Vendor = objMeDealVendorDal.GetVendorDetails(VendorId);
            Vendor.VendorContacts = objMeDealVendorDal.GetVendorContact(VendorId);
           
            return Json(Vendor, JsonRequestBehavior.AllowGet);

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDetailByContactId(string ContactId)
        {
            return Json(objMeDealVendorDal.GetContactDetails(ContactId), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DealVendor(HttpPostedFileBase file, [Bind(Exclude = "VendorContact_Id")] DealVendorViewModel ItemData, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            objDealVendorModel.DealVendorList = objMeDealVendorDal.GetDealVendorList(ItemData.DealVendor.Email, ItemData.DealVendor.Transaction_Id);
            objDealVendorModel.DealVendor = new DealVendor();
            objDealVendorModel.DealVendor.Email = ItemData.DealVendor.Email;
            objDealVendorModel.DealVendor.Vendor_Id = ItemData.DealVendor.Vendor_Id;
            objDealVendorModel.DealVendor.Transaction_Id = ItemData.DealVendor.Transaction_Id;
            objDealVendorModel.DealVendor.CreatedBy = User.Identity.GetUserName();
            int VendorContactId = ItemData.VendorContact.VendorContactId;
            objDealVendorModel.DealVendor.VendorContact_Id= VendorContactId;
      
            ViewBag.AgentID = AgentID;
            //if (!ModelState.IsValid)
            //{
               // return PartialView("pvDealVendor", objDealVendorModel);
            //}

            TempData["ErrMsg"] = objMeDealVendorDal.Save(objDealVendorModel.DealVendor,VendorContactId);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;

            }

            return RedirectToAction("DealVendor", new { Email = ItemData.DealVendor.Email, ClientID = ItemData.UserProfile.ClientID, TransactionID = ItemData.DealVendor.Transaction_Id, AgentID = AgentID });
        }
        #endregion


        #region DealBuilding
        public ActionResult DealBuilding(string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            objBuildingModel.BuildingDropdown = objBuildingDal.GetBuildings();
            objBuildingModel.BuildingViews = objBuildingDal.GetBuildingByTransaction(TransactionID);
            
            DealBuildingView DealBuilding = new DealBuildingView();
            DealBuilding.Transaction_ID = TransactionID;
            if (objBuildingModel.BuildingViews != null){
                int? TransactionBuildingId = objBuildingModel.BuildingViews.TransactionBuildingId;
                DealBuilding.Building_Id = objBuildingModel.BuildingViews.BuildingId;
                DealBuilding.TransactionBuildingId = Convert.ToInt32(TransactionBuildingId);
            }
              
            objBuildingModel.DealBuilding = DealBuilding;
            objBuildingModel.DealBuilding.Email = Email;
            objBuildingModel.UserProfile = objMeDealVendorDal.GetUserDetails(ClientID);
            ViewBag.AgentID = AgentID;
            return PartialView("pvDealBuilding", objBuildingModel);
        }


        public JsonResult GetBuildingById(int BuildingId)
        {
            return Json(objBuildingDal.GetBuildingClientSide(BuildingId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DealBuilding(BuildingViewModel ItemData,string subbtn, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            DealBuildingView model = new DealBuildingView();
            model.CreatedBy = UserManager.FindByEmail(User.Identity.Name).Id;
            model.Transaction_ID = ItemData.DealBuilding.Transaction_ID;
            model.Building_Id = ItemData.DealBuilding.Building_Id;
            ViewBag.AgentID = AgentID;
            if (model.Building_Id==0)
                TempData["ErrMsg"] = objBuildingDal.DeleteTransactionBuilding(ItemData.DealBuilding.TransactionBuildingId);
            
            else
                TempData["ErrMsg"] = objBuildingDal.SaveTransactionBuilding(model);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;

            }
         
            return RedirectToAction("DealBuilding", new { Email = ItemData.DealBuilding.Email, ClientID = ItemData.UserProfile.ClientID, TransactionID = ItemData.DealBuilding.Transaction_ID, AgentID = AgentID });
        }
        #endregion
        #region Delete Deal Vendor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDealVendor(string DealVendorId, string Email, string ClientID, string TransactionID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            DealVendor objVendor = new DealVendor();
            objVendor = objMeDealVendorDal.GetDealVendorByID(DealVendorId);
            TempData["ErrMsg"] = objMeDealVendorDal.DeleteDealVendor(DealVendorId);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("DealVendor", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID });
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


        #region JsonResult
        public ActionResult BuyerDoc(string Email, string ClientID, string TransactionID)
        {
            MstClientDealCreateManageModel model = new MstClientDealCreateManageModel();
            model.utblMstTrackDeal = new utblMstTrackDeal()
            {
                Email = Email,
                ClientID = ClientID,
                TransactionID = TransactionID
            };

            return PartialView("pvBuyerDoc", model);
        }
        [HttpPost]
        public ActionResult BuyerDoc(MstClientDealCreateManageModel Itemdata)
        {
            dalMstBuyerDocuments ObjDal = new dalMstBuyerDocuments();

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Save(Itemdata.utblMstBuyerDocument);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    ModelState.AddModelError("", "Document Added Succesfully");
                    string url = Url.Action("UploadDealDocument", new { Email = Itemdata.utblMstTrackDeal.Email, ClientID = Itemdata.utblMstTrackDeal.ClientID, TransactionID = Itemdata.utblMstTrackDeal.TransactionID });
                    return Json(new { success = true, url = url });
                    //return RedirectToAction("UploadDealDocument", new { Email = Itemdata.utblMstTrackDeal.Email, ClientID = Itemdata.utblMstTrackDeal.ClientID, TransactionID = Itemdata.utblMstTrackDeal.TransactionID });
                }

            }

            return PartialView("pvBuyerDoc", Itemdata);
        }
        #endregion

        #region Delete Gallery
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGallery(string GallaryID, string Email, string ClientID, string TransactionID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            utblMstClientGallerie objGallary = new utblMstClientGallerie();
            objGallary = objMediaDal.GetPhotoGalleryByID(GallaryID);
            TempData["ErrMsg"] = objMediaDal.DeleteGallery(GallaryID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/PhotoGallery/" + GallaryID + objGallary.PhotoNormal));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("mediagallery", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID });
        }
        #endregion

        #region Modified Codes
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

        public ActionResult CreateTransaction(string ClientID)
        {
            ViewBag.ClientID = ClientID;
            ObjModel.AgentDropDown = ObjDal.GetAgentDDL();
            ObjModel.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            return PartialView("pvCreateTransaction", ObjModel);
        }

        [HttpPost]
        public ActionResult CreateTransaction(DealAdminAssignAgentManageModel ItemData)
        {
            if (ModelState.IsValid)
            {
                ItemData.utblMstTrackDealMasters.Status = "Active";
                TempData["ErrMsg"] = ObjDal.AssignAgent(ItemData.utblMstTrackDealMasters);
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
            ItemData.AgentDropDown = ObjDal.GetAgentDDL();
            ItemData.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            return PartialView("pvCreateTransaction", ItemData);
        }
        //public ActionResult GetTransactionDetails(string TransactionID)
        //{
        //    ObjManageModel.TransactionDetails = objDal.GetTransactionDetails(TransactionID);
        //    return PartialView("pvTransactionItem", ObjManageModel);
        //}

        public ActionResult ShareAgentTransaction(string ClientID, string TransactionID)
        {
            ObjAgentShare.utblMstAgentDealSharings = new utblMstAgentDealSharing();
            var Agent = UserManager.FindById(User.Identity.GetUserId());

            ViewBag.ClientID = ClientID;
            ObjAgentShare.utblMstAgentDealSharings.SharerAgentID = Agent.Id;
            ObjAgentShare.utblMstAgentDealSharings.TransactionID = TransactionID;
            ObjAgentShare.AgentDropDown = ObjAgentDealShare.GetAgentDDL(Agent.Id);
            return PartialView("pvShareAgentTransaction", ObjAgentShare);
        }


        [HttpPost]
        public ActionResult ShareAgentTransaction(MstAgentDealShareManageModel ItemData)
        {
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjAgentDealShare.ShareAgentTrans(ItemData.utblMstAgentDealSharings);
                if (TempData["ErrMsg"].ToString() == "0")
                {
                    var Agent = UserManager.FindById(ItemData.utblMstAgentDealSharings.SharerAgentID);
                    //int MailStatus = SignUpEmailConfirmation(Agent.Name, Agent.Email, "Thanks for registering with Just BE | Real Estate", Client.Name, Client.Email);
                    string url = Url.Action("ClientDetails", "DealAgent", new { ClientID = ItemData.utblMstAgentDealSharings.ClientID, AgentID = ItemData.utblMstAgentDealSharings.SharerAgentID });
                    return Json(new { success = true, url = url, type = "gettrans" });
                }
            }
            ViewBag.ClientID = ItemData.utblMstAgentDealSharings.ClientID;
            ItemData.AgentDropDown = ObjAgentDealShare.GetAgentDDL(ItemData.utblMstAgentDealSharings.SharerAgentID);
            return PartialView("pvShareAgentTransaction", ItemData);
        }

        #region BuyerDoc Modified
        public ActionResult BuyerDocPV(string Email, string ClientID, string TransactionID)
        {
            MstClientDealCreateManageModel model = new MstClientDealCreateManageModel();
            model.utblMstTrackDeal = new utblMstTrackDeal()
            {
                Email = Email,
                ClientID = ClientID,
                TransactionID = TransactionID
            };

            return PartialView("pvBuyerDoc_ver1", model);
        }
        [HttpPost]
        public ActionResult BuyerDocPV(MstClientDealCreateManageModel Itemdata)
        {
            dalMstBuyerDocuments ObjDal = new dalMstBuyerDocuments();

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Save(Itemdata.utblMstBuyerDocument);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    ModelState.AddModelError("", "Document Added Succesfully");
                    string url = Url.Action("UploadDealDocumentPV", new { Email = Itemdata.utblMstTrackDeal.Email, ClientID = Itemdata.utblMstTrackDeal.ClientID, TransactionID = Itemdata.utblMstTrackDeal.TransactionID });
                    return Json(new { success = true, url = url, type = "addDocType" });
                    //return RedirectToAction("UploadDealDocument", new { Email = Itemdata.utblMstTrackDeal.Email, ClientID = Itemdata.utblMstTrackDeal.ClientID, TransactionID = Itemdata.utblMstTrackDeal.TransactionID });
                }

            }

            return PartialView("pvBuyerDoc_ver1", Itemdata);
        }
        #endregion
        #region Delete Gallery Modified
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGalleryPV(string GallaryID, string Email, string ClientID, string TransactionID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            utblMstClientGallerie objGallary = new utblMstClientGallerie();
            objGallary = objMediaDal.GetPhotoGalleryByID(GallaryID);
            TempData["ErrMsg"] = objMediaDal.DeleteGallery(GallaryID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/PhotoGallery/" + GallaryID + objGallary.PhotoNormal));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("mediagallery", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID });
        }
        #endregion
        #region ManageDeal Buyer
        public async Task<ActionResult> ManageDeal(string ClientID, string TransactionID, string AgentID = "")
        {
            string Email = await UserManager.GetEmailAsync(ClientID);
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ObjManageModel.UserInfo = objDal.MstInfoView(Email);
            ObjManageModel.StatusList = objDal.StatusList(Email, TransactionID);
            ObjManageModel.TransactionDetails = objDal.GetTransactionDetails(TransactionID);
            ObjManageModel.TrackClosingDateID = objDal.TrackClosingDateID(Email, TransactionID);
            ObjManageModel.ClosingConfig = objDal.GetClosingConfig();
            ObjManageModel.TrackDealStatusList = objDal.TrackDealStatusList(Email, TransactionID);
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView()
            {
                AgentID = AgentID,
                ClientID = ClientID,
                TransactionID = TransactionID
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClientStatusList_ver1", ObjManageModel);
            }
            return View(ObjManageModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageDeal(MstClientDealCreateManageModel ItemData, List<MstContingenciesView> Listdata, string ListingSHA = "")
        {
            string Agent = "";
            Agent = User.Identity.Name;
            if (User.IsInRole("Admin"))
            {
                Agent = await UserManager.GetEmailAsync(ItemData.TrackDealMasterView.AgentID);
            }

            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ItemData.MstUserStatusSelect = new UserStatusSelect();
            ItemData.utblMstTrackDeal.Email = ItemData.UserInfo.Email;
            string Email = ItemData.UserInfo.Email;
            ItemData.BulkEmailStatus = new List<UserStatusSelect>();

            //if (ModelState.IsValid)
            //{
            ItemData.ContingenciesData = Listdata;

            //if(ItemData.ClosingConfig.StatusID==ItemData.utblMstTrackDeal.StatusID)
            //{
            //    return RedirectToAction("ClosingConfig", new { Email = ItemData.UserInfo.Email, StatusID = ItemData.utblMstTrackDeal.StatusID, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID });
            //}
            //two scenerio ratified offer done/ not done

            foreach (var item in Listdata)
            {

                if (item.IsApplicable)
                {
                    ObjManageModel.TrackRatifiedDate = objDal.TrackRatifiedDate(Email, ItemData.utblMstTrackDeal.TransactionID);
                    //select ratified offer date and add no of days
                    ItemData.utblMstTrackDeal.UpdatedOn = ObjManageModel.TrackRatifiedDate.UpdatedOn;
                }

                if (ItemData.ClosingConfig.StatusID == item.StatusID)
                {
                    //send listdata to ratified offer
                    string url = Url.Action("closingconfiguration", new { Email = ItemData.UserInfo.Email, StatusID = item.StatusID, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, UpdatedOn = ItemData.utblMstTrackDeal.UpdatedOn, Listdata = JsonConvert.SerializeObject(Listdata), AgentID = ItemData.TrackDealMasterView.AgentID });
                    return Json(new { success = true, val = 1, url = url });

                }
                var Data = ObjStatus.SelectBulkStatus(Email, item.StatusID);
                ItemData.BulkEmailStatus.Add(Data);
            }
            TempData["ErrMsg"] = ObjStatus.Save(ItemData.utblMstTrackDeal, Listdata, Agent);

            //TempData["ErrMsg"] = ObjStatus.Save(ItemData.utblMstTrackDeal);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;

                if (ObjStatus.emailPreferences(ItemData.UserInfo.Email))
                {
                    string Date = DateTime.Now.ToString("dd MMM yyyy HH:mm");
                    string EmailID = ItemData.utblMstTrackDeal.Email;
                    //ItemData.MstUserStatusSelect = ObjStatus.GetClientStatusfromEmail(Email, ItemData.utblMstTrackDeal.StatusID);
                    ItemData.MstAgentClientNameSelect = ObjStatus.GetNameEmail(ItemData.utblMstTrackDeal.TransactionID);
                    //bulk mail
                    int MailStatus = SendEmailConfirmationToken(EmailID, ItemData.MstUserStatusSelect.ClientName, Date, ItemData.BulkEmailStatus, ItemData.MstAgentClientNameSelect.AgentName, ItemData.MstAgentClientNameSelect.AgentPhone, ItemData.MstAgentClientNameSelect.AgentEmail);
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
            return RedirectToAction("ManageDeal", new { ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, AgentID = ItemData.TrackDealMasterView.AgentID });
            //}
            //return Json(new { success = true, val = 0 });
        }
        #endregion
        #region media gallery
        public ActionResult MediaGallery(string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            objMediaModel.MstGalleryList = objMediaDal.GetPhotoGalleryList(Email, TransactionID);
            objMediaModel.utblMstClientGalleries = new utblMstClientGallerie();
            objMediaModel.utblMstClientGalleries.Email = Email;
            objMediaModel.utblMstClientGalleries.TransactionID = TransactionID;
            objMediaModel.UserProfile = objMediaDal.GetUserDetails(ClientID);
            ViewBag.AgentID = AgentID;
            return PartialView("pvGalleryList", objMediaModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MediaGallery(FormCollection form, MstAgentPhotoGalleryManageModel ItemData, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
         
            objMediaModel.MstGalleryList = objMediaDal.GetPhotoGalleryList(ItemData.utblMstClientGalleries.Email, ItemData.utblMstClientGalleries.TransactionID);
            objMediaModel.utblMstClientGalleries = new utblMstClientGallerie();
            objMediaModel.utblMstClientGalleries.Email = ItemData.utblMstClientGalleries.Email;
            objMediaModel.utblMstClientGalleries.TransactionID = ItemData.utblMstClientGalleries.TransactionID;
            ViewBag.AgentID = AgentID;
            if (!ModelState.IsValid)
            {
                return PartialView("pvGalleryList", objMediaModel);
            }
            //if (Request.Form["Cancel"] == "Cancel")
            //{
            //    return PartialView("pvGalleryList", objMediaModel);
            //}
            var image = form["upfiles"];
            //var EditStatus = form["IsEdited"];
            if (!string.IsNullOrEmpty(image))
            {
                if (ItemData.utblMstClientGalleries.PhotoNormal != null)
                {
                    if (ItemData.utblMstClientGalleries.PhotoNormal.IndexOf("data:image") == -1)
                    {
                        string ExistingFilePath = Server.MapPath("/UploadedFiles/PhotoGallery/" + ItemData.utblMstClientGalleries.GallaryID + ItemData.utblMstClientGalleries.PhotoNormal);
                        FileInfo ExistingFileInfo = new FileInfo(ExistingFilePath);
                        if (ExistingFileInfo.Exists)
                            ExistingFileInfo.Delete();
                    }
                }
                if (image == "PhotoDeleted")
                {

                    ItemData.utblMstClientGalleries.PhotoNormal = "";
                    ItemData.utblMstClientGalleries.PhotoThumb = "";
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


                    ItemData.utblMstClientGalleries.PhotoNormal = guid + imageext;
                    ItemData.utblMstClientGalleries.PhotoThumb = imagedata;
                    TempData["ErrMsg"] = objMediaDal.SaveNew(ItemData.utblMstClientGalleries);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                        byte[] image64 = Convert.FromBase64String(convert);
                        using (var ms = new MemoryStream(image64))
                        {

                            var images = System.Drawing.Image.FromStream(ms);
                            //if (EditStatus == "Yes")
                            //{
                                System.Drawing.Image img = images.GetThumbnailImage(Convert.ToInt32(data.output.width), Convert.ToInt32(data.output.height), null, IntPtr.Zero);
                                img.Save(Server.MapPath("/UploadedFiles/PhotoGallery/" + ItemData.utblMstClientGalleries.GallaryID + guid + imageext), System.Drawing.Imaging.ImageFormat.Jpeg);

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

                TempData["ErrMsg"] = objMediaDal.SaveNew(ItemData.utblMstClientGalleries);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                }
            }
            //if (file != null && file.ContentLength > 0)
            //{
            //    ItemData.utblMstClientGalleries.PhotoNormal = Path.GetExtension(file.FileName);
            //    TempData["ErrMsg"] = objMediaDal.Save(ItemData.utblMstClientGalleries);
            //    if ((TempData["ErrMsg"].ToString()) == "0")
            //    {
            //        TempData["ErrMsg"] = null;
            //        string ext = Path.GetExtension(file.FileName);
            //        var fileName = ItemData.utblMstClientGalleries.GallaryID;
            //        var path = string.Concat(Server.MapPath("~/UploadedFiles/PhotoGallery/" + fileName + ext));
            //        file.SaveAs(path);

            //    }
            //}
            //else
            //{
            //    TempData["ErrMsg"] = "";
            //}
            return RedirectToAction("mediagallery", new { Email = ItemData.utblMstClientGalleries.Email, ClientID = ItemData.UserProfile.ClientID, TransactionID = ItemData.utblMstClientGalleries.TransactionID, AgentID = AgentID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(MstAgentPhotoGalleryManageModel ItemData, string AgentID = "")
        {
            ViewBag.AgentID = AgentID;
            return RedirectToAction("mediagallery", new { Email = ItemData.utblMstClientGalleries.Email, ClientID = ItemData.UserProfile.ClientID, TransactionID = ItemData.utblMstClientGalleries.TransactionID, AgentID = AgentID });
        }
        #endregion
            #region Edit MediaGallery

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetPhotoGalleryByID(string GallaryID, string Email, string ClientID, string TransactionID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            objMediaModel.MstGalleryList = objMediaDal.GetPhotoGalleryList(Email, TransactionID);
            objMediaModel.utblMstClientGalleries = new utblMstClientGallerie();
            objMediaModel.utblMstClientGalleries = objMediaDal.GetPhotoGalleryByID(GallaryID);
            objMediaModel.utblMstClientGalleries.GallaryID = GallaryID;
            objMediaModel.utblMstClientGalleries.Email = Email;
            objMediaModel.utblMstClientGalleries.TransactionID = TransactionID;
            objMediaModel.UserProfile = objMediaDal.GetUserDetails(ClientID);

            // ViewBag.AgentID = AgentID;
            return PartialView("pvGalleryList", objMediaModel);
        }
        #endregion
        #region UploadDealDocumentModified
        public ActionResult UploadDealDocumentPV(string Email, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            string ID = objDal.GetTrackingID(Email, TransactionID);
            ObjManageModel.utblMstTrackDeal = new utblMstTrackDeal();
            ObjManageModel.TrackDealMasterView = new TrackDealMasterView();
            ObjManageModel.MstCalenderConfigList = objDal.MstCalenderConfigList;
            ObjManageModel.utblMstTrackDeal = ObjStatus.GetLiveDealDetailsByID(ID, TransactionID);
            ObjManageModel.MstBuyerDocumentListView = ObjStatus.BuyerDocumentListView(Email, TransactionID);
            ObjManageModel.ClientDealDocumentList = ObjStatus.GetClientDealDocumentById(TransactionID);//Client Documents
            ObjManageModel.MstBuyerDocList = ObjStatus.BuyerDocumentList(Email, TransactionID);
            ObjManageModel.UserProfile = ObjStatus.GetUserDetails(ClientID);
            ObjManageModel.TrackDealMasterView.AgentID = AgentID;
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            return PartialView("pvUploadDealDocument_ver1", ObjManageModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDealDocumentPV(HttpPostedFileBase files, MstDealViewModel ItemData, MstClientDealCreateManageModel Item)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            if (files != null && files.ContentLength > 0)
            {
                int DocID = ItemData.utblMstTrackDealDoc.DocID;
                ItemData.utblMstTrackDealDoc = new utblMstTrackDealDoc();
                ItemData.utblMstTrackDealDoc.TrackingID = ItemData.utblMstTrackDeal.TrackingID;
                ItemData.utblMstTrackDealDoc.TrackDocFile = Path.GetExtension(files.FileName);
                ItemData.utblMstTrackDealDoc.Email = ItemData.utblMstTrackDeal.Email;
                ItemData.utblMstTrackDealDoc.StatusID = ItemData.utblMstTrackDeal.StatusID;
                ItemData.utblMstTrackDealDoc.DocID = DocID;
                ItemData.utblMstTrackDealDoc.TransactionID = ItemData.utblMstTrackDeal.TransactionID;
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
                }
            }
            return RedirectToAction("UploadDealDocumentPV", new { Email = Item.utblMstTrackDeal.Email, ClientID = ItemData.utblMstTrackDeal.ClientID, TransactionID = ItemData.utblMstTrackDeal.TransactionID, AgentID = Item.TrackDealMasterView.AgentID });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDocumentPV(string DealTrackDocID, string ClientID, string TrackingID, string Email, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

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
            return RedirectToAction("UploadDealDocumentPV", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
        }

        //Client Documents
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteClientDocument(string DocId, string ClientID, string Email, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            
            TempData["ErrMsg"] = ObjStatus.DeleteClientDealDocument(DocId);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + ObjStatus.GetClientFileName(DocId)));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            Email = ObjStatus.GetUserDetails(ClientID).Email;
            return RedirectToAction("UploadDealDocumentPV", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
        }
        #endregion  
        #region Delete TrackDeal Record Modified
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveDealPV(string TrackingID, string Email, int StatusID, string ClientID, string TransactionID, string AgentID = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            TempData["ErrMsg"] = ObjStatus.RemoveDeal(TrackingID, Email, StatusID, TransactionID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("ManageDeal", new { ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });
            //string url = Url.Action("ManageDeal", "Agent", new { area = "Coordinator", ClientID = ClientID, TransactionID = TransactionID,AgentID=AgentID });
            //return Json(new { success = true, url = url });
            //return RedirectToAction("CreateDeal", new { Email = Email });

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


    #region Mls_id Helper_Remove_HtmlTags
    public static class Helper
    {
        public static string RemoveUnwantedHtmlTags(this string html, List<string> unwantedTags)
        {
            if (String.IsNullOrEmpty(html))
            {
                return html;
            }

            var document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNodeCollection tryGetNodes = document.DocumentNode.SelectNodes("./*|./text()");

            if (tryGetNodes == null || !tryGetNodes.Any())
            {
                return html;
            }

            var nodes = new Queue<HtmlNode>(tryGetNodes);

            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode;

                var childNodes = node.SelectNodes("./*|./text()");

                if (childNodes != null)
                {
                    foreach (var child in childNodes)
                    {
                        nodes.Enqueue(child);
                    }
                }

                if (unwantedTags.Any(tag => tag == node.Name))
                {
                    if (childNodes != null)
                    {
                        foreach (var child in childNodes)
                        {
                            parentNode.InsertBefore(child, node);
                        }
                    }

                    parentNode.RemoveChild(node);

                }
            }

            return document.DocumentNode.InnerHtml;
        }

    }

    #endregion
}