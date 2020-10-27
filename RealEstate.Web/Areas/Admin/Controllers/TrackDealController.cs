using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.Models;
using RealEstate.Entities.DataAccess;
using System.IO;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrackDealController : Controller
    {
        MstDealViewModel ObjModel = new MstDealViewModel();
        dalTrackDeal ObjDal = new dalTrackDeal();

        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            ObjModel.MstDealView = ObjDal.DealGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvDealList", ObjModel);
            }
            return View(ObjModel);
        }
       
        public ActionResult Create()
        {
            ObjModel.MstClientDDL = ObjDal.GetClientDDL();
            ObjModel.MstStatusDDL = ObjDal.GetStatusDDL();
            return View(ObjModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MstDealViewModel ItemData)
        {
            if (ModelState.IsValid)
            {
                //TempData["ErrMsg"] = ObjDal.Save(ItemData.utblMstTrackDeal);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("UploadDealDocument", new { ID = ItemData.utblMstTrackDeal.TrackingID });
                }

            }
            //ViewBag.ActiveURL = "/Admin/NewsAndEvent/List";
            ItemData.MstClientDDL = ObjDal.GetClientDDL();
            ItemData.MstStatusDDL = ObjDal.GetStatusDDL();
            return View(ItemData);
        }


        #region  UploadDealDocument
        public ActionResult UploadDealDocument(string ID ,string TransactionID)
        {
            ObjModel.utblMstTrackDeal = new utblMstTrackDeal();
            ObjModel.utblMstTrackDeal = ObjDal.GetLiveDealDetailsByID(ID, TransactionID);
            ObjModel.MstLiveDealDocList = ObjDal.GetLiveDealDocList(ID);
            ViewBag.ActiveURL = "/Admin/NewsAndEvent/List";

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvUploadDealDocument", ObjModel);
            }
            return View(ObjModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDealDocument(HttpPostedFileBase files, MstDealViewModel ItemData)
        {

            if (files != null && files.ContentLength > 0)
            {
                ItemData.utblMstTrackDealDoc = new utblMstTrackDealDoc();
                ItemData.utblMstTrackDealDoc.TrackingID = ItemData.utblMstTrackDeal.TrackingID;
                ItemData.utblMstTrackDealDoc.TrackDocFile = Path.GetExtension(files.FileName);

                TempData["ErrMsg"] = ObjDal.SaveTrackDealDoc(ItemData.utblMstTrackDealDoc);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    var TrackingID = ItemData.utblMstTrackDeal.TrackingID;
                    string ext = Path.GetExtension(files.FileName);
                    var fileName = ItemData.utblMstTrackDealDoc.DealTrackDocID;
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + fileName + ext));
                    files.SaveAs(path);
                    ItemData.MstLiveDealDocList = ObjDal.GetLiveDealDocList(TrackingID);
                    return RedirectToAction("UploadDealDocument", new { ID = TrackingID });
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
        public ActionResult DeleteDocument(string DealTrackDocID, string TrackingID)
        {
            utblMstTrackDealDoc objDoc = new utblMstTrackDealDoc();
            objDoc = ObjDal.GetDocDetailsByID(DealTrackDocID);
            TempData["ErrMsg"] = ObjDal.DeleteLiveDealDocument(DealTrackDocID, TrackingID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/TrackDeal/" + DealTrackDocID + objDoc.TrackDocFile));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("UploadDealDocument", new { TrackingID = TrackingID });
        }
        #endregion



    }
}