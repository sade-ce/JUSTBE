using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class DocumentMasterController : Controller
    {
        MstBuyerDocumentManageModel ObjModel = new MstBuyerDocumentManageModel();
        dalMstBuyerDocuments ObjDal = new dalMstBuyerDocuments();
        public ActionResult BuyerDocumentList()
        {
            ObjModel.MstBuyerDocList = ObjDal.MstBuyerDocList;
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvBuyerDocList", ObjModel);
            }

            return View(ObjModel);
        }
        
        public ActionResult Create(utblMstBuyerDocument ItemData)
        {

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Save(ItemData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                    return RedirectToAction("BuyerDocumentList");
                }
            }
            ViewBag.ActiveURL = "/Admin/DocumentMaster/BuyerDocumentList";
            return View(ItemData);
        }

        public ActionResult Edit(int DocID)
        {
            utblMstBuyerDocument ObjViewModel = new utblMstBuyerDocument();

            ObjViewModel = ObjDal.GetStatusByID(DocID);
            ViewBag.ActiveURL = "/Admin/DocumentMaster/BuyerDocumentList";
            return View(ObjViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(utblMstBuyerDocument ItemData)
        {
            ViewBag.ActiveURL = "/Admin/DocumentMaster/BuyerDocumentList";
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Edit(ItemData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("BuyerDocumentList");
                }
            }
            return View(ItemData);
        }

        #region Delete BuyerDoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int DocID)
        {
            TempData["ErrMsg"] = ObjDal.Delete(DocID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("BuyerDocumentList");
        }
        #endregion
    }
}