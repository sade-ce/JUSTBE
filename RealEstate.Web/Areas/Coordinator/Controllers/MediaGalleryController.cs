using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.DataAccess;
using System.IO;
using RealEstate.Entities.Models;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]

    public class MediaGalleryController : Controller
    {
        MstAgentPhotoGalleryManageModel ObjModel = new MstAgentPhotoGalleryManageModel();
        dalMstAgentPhotoGallery ObjDal = new dalMstAgentPhotoGallery();
        public ActionResult Create(string Email,string ClientID, string TransactionID,string AgentID="")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            ObjModel.MstGalleryList = ObjDal.GetPhotoGalleryList(Email, TransactionID);
            ObjModel.utblMstClientGalleries = new utblMstClientGallerie();
            ObjModel.utblMstClientGalleries.Email = Email;
            ObjModel.utblMstClientGalleries.TransactionID = TransactionID;
            ObjModel.UserProfile = ObjDal.GetUserDetails(ClientID);
            ViewBag.AgentID = AgentID;

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvGalleryList", ObjModel);
            }
            return View(ObjModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, MstAgentPhotoGalleryManageModel ItemData,string AgentID="")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            ObjModel.MstGalleryList = ObjDal.GetPhotoGalleryList(ItemData.utblMstClientGalleries.Email, ItemData.utblMstClientGalleries.TransactionID);
            ObjModel.utblMstClientGalleries = new utblMstClientGallerie();
            ObjModel.utblMstClientGalleries.Email = ItemData.utblMstClientGalleries.Email;
            ObjModel.utblMstClientGalleries.TransactionID = ItemData.utblMstClientGalleries.TransactionID;
            ViewBag.AgentID = AgentID;
            if (!ModelState.IsValid)
            {
                return View(ObjModel);
            }

            if (file != null && file.ContentLength > 0)
            {
                ItemData.utblMstClientGalleries.PhotoNormal = Path.GetExtension(file.FileName);
                TempData["ErrMsg"] = ObjDal.Save(ItemData.utblMstClientGalleries);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    string ext = Path.GetExtension(file.FileName);
                    var fileName = ItemData.utblMstClientGalleries.GallaryID;
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/PhotoGallery/" + fileName + ext));
                    file.SaveAs(path);
                    return RedirectToAction("Create", new { Email = ItemData.utblMstClientGalleries.Email,ClientID= ItemData.UserProfile.ClientID, TransactionID= ItemData.utblMstClientGalleries.TransactionID, AgentID= AgentID });
                }
            }
            else
            {
                TempData["ErrMsg"] = null;
                return RedirectToAction("Create", new { Email = ItemData.utblMstClientGalleries.Email });
            }
           
            return View(ItemData);
        }


        #region Delete Gallery
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGallery(string GallaryID, string Email,string ClientID,string TransactionID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            utblMstClientGallerie objGallary = new utblMstClientGallerie();
            objGallary = ObjDal.GetPhotoGalleryByID(GallaryID);
            TempData["ErrMsg"] = ObjDal.DeleteGallery(GallaryID);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/PhotoGallery/" + GallaryID + objGallary.PhotoNormal));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("Create", new { Email = Email,ClientID= ClientID,TransactionID=TransactionID });
        }
        #endregion

    }
}