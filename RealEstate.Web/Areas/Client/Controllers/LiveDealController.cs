using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.Models;
using System.IO;

namespace RealEstate.Web.Areas.Client.Controllers
{
    [Authorize(Roles = "Client,Agent,Admin")]
    public class LiveDealController : Controller
    {
        MstClientLiveDealManageModel ObjModel = new MstClientLiveDealManageModel();
        dalClientLiveDealView ObjDal = new dalClientLiveDealView();
        dalClientRespond ObjClientDal = new dalClientRespond();

        MstClientDealRespondManageModel ObjClient = new MstClientDealRespondManageModel();
        // GET: Client/LiveDeal
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

        public ActionResult Status(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            string Email = User.Identity.Name;
            ObjModel.MstClientLiveDealList = ObjDal.ClientDealGetPaged(PageNo, PageSize, Email, out TotalRow, SearchTerm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvStatus", ObjModel);
            }
            return View(ObjModel);
        }

        public ActionResult ViewDocuments(string TrackingID)
        {
            ObjClient.MstClientRespondView = ObjClientDal.GetClientDealRespondDetails(TrackingID);
            ObjClient.MstClientTrackDocList = ObjClientDal.GetClientLiveDealDocList(TrackingID);
            ViewBag.ActiveURL = "/Client/LiveDeal/Respond";

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvUploadDealDocument", ObjClient);
            }
            return View(ObjClient);
        }

        public ActionResult Respond(string TrackingID)
        {
            ObjClient.MstClientRespondView = ObjClientDal.GetClientDealRespondDetails(TrackingID);
            ObjClient.DocList = ObjClientDal.GetClientRespondDocList(TrackingID);
            ViewBag.ActiveURL = "/Client/LiveDeal/Respond";

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvUploadDealDocument", ObjClient);
            }
            return View(ObjClient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Respond(HttpPostedFileBase files, MstClientDealRespondManageModel ItemData)
        {

            if (files != null && files.ContentLength > 0)
            {
                ItemData.utblMstClientTrackDealDoc = new utblMstClientTrackDealDoc();
                ItemData.utblMstClientTrackDealDoc.TrackingID = ItemData.MstClientRespondView.TrackingID;
                ItemData.utblMstClientTrackDealDoc.TrackDocFile = Path.GetExtension(files.FileName);

                TempData["ErrMsg"] = ObjClientDal.SaveClientTrackDealDoc(ItemData.utblMstClientTrackDealDoc);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    var TrackingID = ItemData.MstClientRespondView.TrackingID;
                    string ext = Path.GetExtension(files.FileName);
                    var fileName = ItemData.utblMstClientTrackDealDoc.ClientTrackDocID;
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/ClientTrackDeal/" + fileName + ext));
                    files.SaveAs(path);
                    ItemData.DocList = ObjClientDal.GetClientRespondDocList(TrackingID);
                    return RedirectToAction("Respond", new { TrackingID = TrackingID });
                }

            }
            else
            {
                TempData["ErrMsg"] = null;
                return RedirectToAction("List");
            }

            return View(ItemData);
        }

        #region Delete ClientDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteClientDocument(string ClientTrackDocID, string TrackingID)
        {
            utblMstClientTrackDealDoc objDoc = new utblMstClientTrackDealDoc();
            objDoc = ObjClientDal.GetClientDocDetailsByID(ClientTrackDocID);
            TempData["ErrMsg"] = ObjClientDal.DeleteClientDocument(ClientTrackDocID, TrackingID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/ClientTrackDeal/" + ClientTrackDocID + objDoc.TrackDocFile));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("Respond", new { TrackingID = TrackingID });
        }
        #endregion
    }
}