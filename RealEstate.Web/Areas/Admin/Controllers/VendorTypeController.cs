using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.IO;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class VendorTypeController : Controller
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
        VendorCategoryViewModel objViewModel = new VendorCategoryViewModel();
        dalVendorCategory objDal = new dalVendorCategory();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            objViewModel.VendorCategoryList = objDal.VendorCategoryGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/vendortype/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvVendorTypeList", objViewModel);
            }
            return View(objViewModel);
        }
        public ActionResult Create()
        {
            ViewBag.ActiveURL = "/admin/vendortype/list";

            return View("Create", objViewModel.VendorCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase files, VendorCategoryViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "/admin/vendortype/list";
            if (ModelState.IsValid)
            {
                if (files != null && files.ContentLength > 0)
                {
                    var guid = Guid.NewGuid().ToString().Substring(0, 4);
                    Model.VendorCategory.CategoryImage = guid + Path.GetExtension(files.FileName);
                    TempData["ErrMsg"] = objDal.Save(Model.VendorCategory, CreatedBy);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                        string ext = guid + Path.GetExtension(files.FileName);
                        var fileName = Model.VendorCategory.CategoryId;
                        var path = string.Concat(Server.MapPath("~/img/vendorCategory/" + fileName + ext));
                        files.SaveAs(path);
                        return RedirectToAction("list");
                    }
                    else
                    {
                        TempData["ErrMsg"] = null;
                        ModelState.AddModelError("", "Unable to add vendor type, record already exists...");
                    }
                }
                else
                {
                    TempData["ErrMsg"] = objDal.Save(Model.VendorCategory, CreatedBy);
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

            }
            return View("Create", Model);
        }


        public ActionResult Edit(string CategoryId)
        {
            ViewBag.ActiveURL = "/admin/vendortype/list";

            objViewModel.VendorCategory = objDal.GetVendorCategoryByID(CategoryId);
            return View(objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase files, VendorCategoryViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "/admin/vendortype/list";
            if (ModelState.IsValid)
            {
                if (files != null && files.ContentLength > 0)
                {
                    
                    string ExistingFilePath = Server.MapPath("~/img/vendorCategory/" + Model.VendorCategory.CategoryImage);
                    FileInfo file = new FileInfo(ExistingFilePath);

                    var guid = Guid.NewGuid().ToString().Substring(0, 4);

                    Model.VendorCategory.CategoryImage = Model.VendorCategory.CategoryId + guid + Path.GetExtension(files.FileName);
                    TempData["ErrMsg"] = objDal.Edit(Model.VendorCategory, CreatedBy);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        if (file.Exists)
                            file.Delete();
                       
                        TempData["ErrMsg"] = null;
                        string ext = guid + Path.GetExtension(files.FileName);
                        var fileName = Model.VendorCategory.CategoryId;
                        var path = string.Concat(Server.MapPath("~/img/vendorCategory/" + fileName + ext));
                        files.SaveAs(path);
                        return RedirectToAction("list");
                    }
                    else
                    {
                        TempData["ErrMsg"] = null;
                        ModelState.AddModelError("", "Unable to add vendor type, record already exists...");
                    }
                }
                else
                {
                    TempData["ErrMsg"] = objDal.Edit(Model.VendorCategory, CreatedBy);
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
            }
            return View("Edit", Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string CategoryId, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/VendorType/List";
            string ExistingFilePath = Server.MapPath("~/img/vendorCategory/" + objDal.GetVendorCategoryByID(CategoryId).CategoryImage);
            FileInfo file = new FileInfo(ExistingFilePath);
            TempData["ErrMsg"] = objDal.Delete(CategoryId);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (file.Exists)
                    file.Delete();
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

            objViewModel.ListSearchAutoComplete = objDal.VendorTypeAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objViewModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
    }
}