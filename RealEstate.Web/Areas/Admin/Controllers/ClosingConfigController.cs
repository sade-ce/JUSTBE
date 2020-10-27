using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.Models;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClosingConfigController : Controller
    {
        dalMstClosingConfig ObjDal = new dalMstClosingConfig();
        MstClosingConfigurationModel ObjModel = new MstClosingConfigurationModel();

        public ActionResult List()
        {
            ObjModel.GetClosingConfig = ObjDal.GetClosingConfig();
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClosingConfig", ObjModel);
            }
            ViewBag.ActiveURL = "/Admin/ClosingConfig/list";

            return View(ObjModel);
        }
        public ActionResult Create()
        {
            ObjModel.MstStatusList = ObjDal.MstStatusList;
            ViewBag.ActiveURL = "/Admin/ClosingConfig/list";

            return View(ObjModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MstClosingConfigurationModel Item)
        {
            ObjModel.MstStatusList = ObjDal.MstStatusList;
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Save(Item.utblMstClosingConfigutation);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("List");
                }
            }
            ViewBag.ActiveURL = "/Admin/ClosingConfig/list";

            return View(ObjModel);
        }
    }
}