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
    [Authorize(Roles = "Admin")]
    public class SellerDocumentController : Controller
    {
        MstSellerDocumentManageModel ObjModel = new MstSellerDocumentManageModel();
        dalMstSellerDocuments ObjDal = new dalMstSellerDocuments();
        public ActionResult SellerDocumentList()
        {
            ViewBag.ActiveURL = "SellerDocumentList";
            ObjModel.MstSellerDocList = ObjDal.MstSellerDocList.OrderByDescending(x=>x.UpdatedOn);
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvSellerDocList", ObjModel);
            }

            return View(ObjModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MstSellerDocumentManageModel ItemData)
        {

          
                TempData["ErrMsg"] = ObjDal.Save(ItemData.utblMstSellerDocument);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                   
                }
            return RedirectToAction("SellerDocumentList");


        }

        //public ActionResult Edit(int DocID)
        //{
        //    utblMstBuyerDocument ObjViewModel = new utblMstBuyerDocument();

        //    ObjViewModel = ObjDal.GetStatusByID(DocID);
        //    ViewBag.ActiveURL = "BuyerDocumentList";
        //    return View(ObjViewModel);
        //}

        public JsonResult Edit(int DocID)
        {

            utblMstSellerDocument ObjViewModel = new utblMstSellerDocument();

            ObjViewModel = ObjDal.GetStatusByID(DocID);
            return Json(ObjViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MstSellerDocumentManageModel ItemData)
        {
          
           
                TempData["ErrMsg"] = ObjDal.Edit(ItemData.utblMstSellerDocument);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
            }
            return RedirectToAction("SellerDocumentList");

        }

        #region Delete BuyerDoc
        [HttpPost]
        public ActionResult Delete(int DocID)
        {
            TempData["ErrMsg"] = ObjDal.Delete(DocID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("SellerDocumentList");
        }
        #endregion
    }
}