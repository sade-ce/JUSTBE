using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.Models;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class KeyInfoController : Controller
    {
        MstKeyInfoLinkManageModel ObjModel = new MstKeyInfoLinkManageModel();
        dalKeyInfoLink Objdal = new dalKeyInfoLink();
        public ActionResult List()
        {
            ObjModel.List = Objdal.MstKeyInfoLinkList;
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvList", ObjModel);
            }
            return View(ObjModel);
        }

        public ActionResult Create(utblMstKeyInfoLink ItemData)
        {

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = Objdal.Save(ItemData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                    return RedirectToAction("List");
                }
            }
            ViewBag.ActiveURL = "/Admin/KeyInfo/list";
            return View(ItemData);
        }

        public ActionResult Edit(int KeyInfoID)
        {
            utblMstKeyInfoLink ObjModel = new utblMstKeyInfoLink();

            ObjModel = Objdal.GetKeyInfoLinkByID(KeyInfoID);
            ViewBag.ActiveURL = "/Admin/KeyInfo/list";
            return View(ObjModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(utblMstKeyInfoLink ItemData)
        {
            ViewBag.ActiveURL = "/Admin/KeyInfo/list";
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = Objdal.Edit(ItemData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("List");
                }
            }
            ViewBag.ActiveURL = "/Admin/KeyInfo/list";
            return View(ItemData);
        }

        #region Delete KeyInfoLink
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int KeyInfoID)
        {
            TempData["ErrMsg"] = Objdal.Delete(KeyInfoID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("List");
        }
        #endregion
    }
}