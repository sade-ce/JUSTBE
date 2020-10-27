using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.DataAccess;
namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CalenderConfigController : Controller
    {
        MstCalenderConfiguration ObjModel = new MstCalenderConfiguration();
        dalMstCalenderConfig ObjDal = new dalMstCalenderConfig();
        public ActionResult List(int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            ObjModel.CalenderConfigList = ObjDal.GetCalenderConfigList(PageNo, PageSize, out TotalRow);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/Admin/CalenderConfig/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvCalenderConfig", ObjModel);
            }
            return View(ObjModel);
        }
        public ActionResult Create(int PageNo = 1, int PageSize = 10)
        {
            ViewBag.ActiveURL = "/Admin/CalenderConfig/list";


            ObjModel.MstStatusList = ObjDal.MstStatusList;
            return View(ObjModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MstCalenderConfiguration Item)
        {
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Save(Item.utblMstCalenderConfiguration);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("List");
                }
            }
            Item.MstStatusList = ObjDal.MstStatusList;
            ViewBag.ActiveURL = "/Admin/CalenderConfig/list";
            return View(Item);
        }

        #region Calender Config Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int CalenderConfigID)
        {
            TempData["ErrMsg"] = ObjDal.Delete(CalenderConfigID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("List");
        }
        #endregion
    }
}