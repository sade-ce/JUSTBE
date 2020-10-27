using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{

    public class NeighborhoodsController : Controller
    {
        MstNeighborhoodViewModel ObjModel = new MstNeighborhoodViewModel();
        dalNeighborhood ObjDal = new dalNeighborhood();


        #region GetCityDDL
        public JsonResult GetCity(int id)
        {
            return Json(new SelectList(ObjDal.CityDropDownList(id), "CityID", "CityName"));
        }
        #endregion
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            ObjModel.MstNeighborhoodList = ObjDal.GetNeighborhoodPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/Neighborhoods/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvNeighborhoodList", ObjModel);
            }
            return View(ObjModel);
        }

        public ActionResult Create()
        {
            ViewBag.ActiveURL = "/admin/Neighborhoods/list";
            ObjModel.MstStateDDl = ObjDal.MstStateList;
            return View(ObjModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(MstNeighborhoodViewModel Itemdata)
        {
            ViewBag.ActiveURL = "/admin/Neighborhoods/list";

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Save(Itemdata.utblNeighborhoods);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }
            }
            Itemdata.MstStateDDl = ObjDal.MstStateList;
            //Itemdata.utblNeighborhoods.PostContent = System.Web.HttpUtility.HtmlDecode(Itemdata.utblNeighborhoods.PostContent);

            return View(Itemdata);
        }

        public ActionResult Edit(int NeighborhoodID)
        {
            ViewBag.ActiveURL = "/admin/Neighborhoods/list";

            ObjModel.utblNeighborhoods = ObjDal.GetDetailsByNeighborhoodID(NeighborhoodID);
            ObjModel.MstStateDDl = ObjDal.MstStateList;
            ObjModel.utblNeighborhoods.PostContent = System.Web.HttpUtility.HtmlDecode(ObjModel.utblNeighborhoods.PostContent);
            return View(ObjModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(MstNeighborhoodViewModel Model)
        {
            ViewBag.ActiveURL = "/admin/Neighborhoods/list";
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Save(Model.utblNeighborhoods);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }
            }
            Model.utblNeighborhoods = ObjDal.GetDetailsByNeighborhoodID(Model.utblNeighborhoods.NeighborhoodID);
            Model.utblNeighborhoods.PostContent = System.Web.HttpUtility.HtmlDecode(Model.utblNeighborhoods.PostContent);
            Model.MstStateDDl = ObjDal.MstStateList;
            return View("Edit", Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int NeighborhoodID, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/admin/Neighborhood/list";
            TempData["ErrMsg"] = ObjDal.Delete(NeighborhoodID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            ListCount--;
            if (ListCount == 0)
                PgNo = 1;
            return RedirectToAction("List", new { PageNo = PgNo, PageSize = PgSize });

        }


    }
}