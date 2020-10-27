using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using Microsoft.AspNet.Identity;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class BuildingAttachmentsController : Controller
    {
        #region Membership Initialization
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion
        // GET: Admin/VendorType
        BuildingAttachmentsViewModel objViewModel = new BuildingAttachmentsViewModel();
        dalBuildingAttachments objDal = new dalBuildingAttachments();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            objViewModel.BuildingAttachmentsList = objDal.BuildingAttachmentsGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/BuildingAttachments/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvBuildingAttachmentsList", objViewModel);
            }
            return View(objViewModel);
        }
        public ActionResult Create()
        {
            ViewBag.ActiveURL = "/admin/BuildingAttachments/list";

            return View("Create", objViewModel.BuildingAttachments);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(BuildingAttachmentsViewModel Model)
        {
            string CreatedBy = User.Identity.GetUserId();
            ViewBag.ActiveURL = "/admin/BuildingAttachments/list";
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = objDal.Save(Model.BuildingAttachments, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }
                else
                {
                    TempData["ErrMsg"] = null;
                    ModelState.AddModelError("", "Unable to add vendor type, record already exists...");
                }

            }
            return View("Create", Model);
        }


        public ActionResult Edit(int BuildingAttachmentId)
        {
            ViewBag.ActiveURL = "/admin/BuildingAttachments/list";

            objViewModel.BuildingAttachmentsModel = objDal.GetBuildingAttachmentsByID(BuildingAttachmentId);
            return View(objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(BuildingAttachmentsViewModel Model)
        {
            string CreatedBy = User.Identity.GetUserId();
            ViewBag.ActiveURL = "/admin/BuildingAttachments/list";
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = objDal.Edit(Model.BuildingAttachmentsModel, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }
                else
                {
                    TempData["ErrMsg"] = null;
                    ModelState.AddModelError("", "Unable to add vendor type, record already exists...");
                }
            }
            return View("Edit", Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int BuildingAttachmentId, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/BuildingAttachments/List";
         
            TempData["ErrMsg"] = objDal.Delete(BuildingAttachmentId);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
              
                TempData["ErrMsg"] = null;
            }
            ListCount--;
            if (ListCount == 0)
                PgNo = 1;
            return RedirectToAction("List", new { PageNo = PgNo, PageSize = PgSize });

        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult autocomplete(string term)
        {

            objViewModel.ListSearchAutoComplete = objDal.BuildingAttachmentsAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objViewModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, Convert.ToString( item.Id)));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
    }
}