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
    public class BuyerDocumentController : Controller
    {
        MstBuyerDocumentManageModel ObjModel = new MstBuyerDocumentManageModel();
        dalMstBuyerDocuments ObjDal = new dalMstBuyerDocuments();
        public ActionResult BuyerDocumentList()
        {
            ViewBag.ActiveURL = "BuyerDocumentList";
            ObjModel.MstBuyerDocList = ObjDal.MstBuyerDocList.OrderByDescending(x=>x.UpdatedOn);
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvBuyerDocList", ObjModel);
            }

            return View(ObjModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MstBuyerDocumentManageModel ItemData)
        {

          
                TempData["ErrMsg"] = ObjDal.Save(ItemData.utblMstBuyerDocument);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                   
                }
            return RedirectToAction("BuyerDocumentList");


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

            utblMstBuyerDocument ObjViewModel = new utblMstBuyerDocument();

            ObjViewModel = ObjDal.GetStatusByID(DocID);
            return Json(ObjViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MstBuyerDocumentManageModel ItemData)
        {
          
           
                TempData["ErrMsg"] = ObjDal.Edit(ItemData.utblMstBuyerDocument);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
            }
            return RedirectToAction("BuyerDocumentList");

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
            return RedirectToAction("BuyerDocumentList");
        }
        #endregion
    }
}