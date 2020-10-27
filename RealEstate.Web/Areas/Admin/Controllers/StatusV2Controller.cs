using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatusV2Controller : Controller
    {
        // GET: Admin/Status
        MstStatusViewModel ObjModel = new MstStatusViewModel();
        dalStatus ObjDal = new dalStatus();

        #region List
        public ActionResult List()
        {
            ViewBag.ActiveURL = "BuyerStatusList";
            ObjModel.MstStatusList = ObjDal.MstStatusList;
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvStatusList", ObjModel);
            }
            return View(ObjModel);
        }
        #endregion
        #region Create
        public ActionResult Create()
        {         
            ViewBag.ActiveURL = "BuyerStatus";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(utblMstStatus ItemData)
        {

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Save(ItemData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                    return RedirectToAction("List");
                }
            }
            ViewBag.ActiveURL = "BuyerStatus";
            return View(ItemData);
        }

        #endregion
        #region Edit
        public ActionResult Edit(int StatusID)
        {
            utblMstStatus ObjViewModel = new utblMstStatus();

            ObjViewModel = ObjDal.GetStatusByID(StatusID);
            ViewBag.ActiveURL = "BuyerStatus";
            return View(ObjViewModel);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(utblMstStatus ItemData)
        {
            ViewBag.ActiveURL = "BuyerStatus";
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Edit(ItemData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("List");
                }
            }
            ViewBag.ActiveURL = "/Admin/Status/list";
            return View(ItemData);
        }
        #endregion
        #region Delete Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int StatusID)
        {
            TempData["ErrMsg"] = ObjDal.Delete(StatusID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("List");
        }
        #endregion



        #region SellerStatusList
        public ActionResult SellerStatusList()
        {
            ViewBag.ActiveURL = "SellerStatusList";
            ObjModel.MstSellerStatusList = ObjDal.MstSellerStatusList;
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvSellerStatusList", ObjModel);
            }
            return View(ObjModel);
        }
        #endregion
        #region SellerStatusCreate
        public ActionResult SellerStatusCreate()
        {

            //if (ModelState.IsValid)
            //{
            //    TempData["ErrMsg"] = ObjDal.SellerStatusSave(ItemData);
            //    if ((TempData["ErrMsg"].ToString()) == "0")
            //    {
            //        TempData["ErrMsg"] = null;

            //        return RedirectToAction("SellerStatusList");
            //    }
            //}
            ViewBag.ActiveURL = "SellerStatus";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellerStatusCreate(utblMstSellerStatus ItemData)
        {

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.SellerStatusSave(ItemData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                    return RedirectToAction("SellerStatusList");
                }
            }
            ViewBag.ActiveURL = "SellerStatus";
            return View(ItemData);
        }

        #endregion
        #region SellerStatusEdit
        public ActionResult SellerStatusEdit(int SellerStatusID)
        {
            utblMstSellerStatus ObjViewModel = new utblMstSellerStatus();

            ObjViewModel = ObjDal.GetSellerStatusByID(SellerStatusID);
            ViewBag.ActiveURL = "SellerStatus";
            return View(ObjViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellerStatusEdit(utblMstSellerStatus ItemData)
        {
            ViewBag.ActiveURL = "SellerStatus";
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Edit(ItemData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("SellerStatusList");
                }
            }
            ViewBag.ActiveURL = "/Admin/Status/list";
            return View(ItemData);
        }
        #endregion
        #region  SellerStatusDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellerStatusDelete(int SellerStatusID)
        {
            TempData["ErrMsg"] = ObjDal.SellerStatusDelete(SellerStatusID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("SellerStatusList");
        }
        #endregion
    }
}