using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class VendorController : Controller
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
        // GET: Admin/vendorsubtype
        VendorViewModel objViewModel = new VendorViewModel();
        VendorCategoryViewModel objCategoryModel = new VendorCategoryViewModel();
        dalVendor objDal = new dalVendor();
        dalVendorContacts objVendorContact = new dalVendorContacts();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            objViewModel.VendorList = objDal.VendorGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/vendor/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvvendorList", objViewModel);
            }
            return View(objViewModel);
        }
        public ActionResult Create()
        {
            ViewBag.ActiveURL = "/admin/vendor/list";
            objViewModel.CategoryDropDown = objDal.GetVendorCategoryDDl();
            return View("Create", objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase files, VendorViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "/admin/vendor/list";
            Model.CategoryDropDown = objDal.GetVendorCategoryDDl();
            if (ModelState.IsValid)
            {
                if (files != null && files.ContentLength > 0)
                {
                    var guid = Guid.NewGuid().ToString().Substring(0, 4);
                    Model.Vendor.VendorImage = guid + Path.GetExtension(files.FileName);
                    TempData["ErrMsg"] = objDal.Save(Model.Vendor, Model.VendorContacts, CreatedBy);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                        string ext = guid + Path.GetExtension(files.FileName);
                        var fileName = Model.Vendor.VendorId;
                        var path = string.Concat(Server.MapPath("~/img/vendors/" + fileName + ext));
                        files.SaveAs(path);
                        return RedirectToAction("list");
                    }
                }
                TempData["ErrMsg"] = objDal.Save(Model.Vendor,Model.VendorContacts, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }



            }
            return View("Create", Model);
        }


        public ActionResult Edit(string VendorId,string VendorContactId)
        {
            if(string.IsNullOrEmpty(VendorId))
            ViewBag.ActiveURL = "/admin/vendor/list";
            objViewModel.CategoryDropDown = objDal.GetVendorCategoryDDl();
            if(!string.IsNullOrEmpty(VendorId))
            objViewModel.Vendor = objDal.GetVendorByID(VendorId);
            if(!string.IsNullOrEmpty(VendorContactId))
            objViewModel.VendorContacts = objDal.GetVendorContactByID(Convert.ToInt32(VendorContactId));
            return View(objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase files, VendorViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "/admin/vendorsubtype/list";
            Model.CategoryDropDown = objDal.GetVendorCategoryDDl();
            ModelState.Remove("VendorContacts.VendorContactId");
            if (ModelState.IsValid)
            {
                if ((Model.VendorContacts.ContactName != null)||(Model.VendorContacts.ContactPhone)!=null||(Model.VendorContacts.ContactTitle!=null))
                {
                    Model.VendorContacts.Vendor_Id = Model.Vendor.VendorId;
                    if (Model.VendorContacts.VendorContactId == 0) {
                   
                        objVendorContact.SaveVendorContact(Model.VendorContacts);
                    }
                    else
                    {
                        objVendorContact.EditVendorContact(Model.VendorContacts);
                    }
                }
                if (files != null && files.ContentLength > 0)
                {

                    string ExistingFilePath = Server.MapPath("~/img/vendors/" + Model.Vendor.VendorImage);
                    FileInfo file = new FileInfo(ExistingFilePath);

                    var guid = Guid.NewGuid().ToString().Substring(0, 4);

                    Model.Vendor.VendorImage = Model.Vendor.VendorId + guid + Path.GetExtension(files.FileName);
                    TempData["ErrMsg"] = objDal.Edit(Model.Vendor, CreatedBy);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        if (file.Exists)
                            file.Delete();

                        TempData["ErrMsg"] = null;
                        string ext = guid + Path.GetExtension(files.FileName);
                        var fileName = Model.Vendor.VendorId;
                        var path = string.Concat(Server.MapPath("~/img/vendors/" + fileName + ext));
                        files.SaveAs(path);
                        return RedirectToAction("list");
                    }

                }

                TempData["ErrMsg"] = objDal.Edit(Model.Vendor, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }


            }
            return View("Edit", Model);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteContact(string VendorContactId, int PgNo, int PgSize, int ListCount)
        //{
        //    ViewBag.ActiveURL = "/Vendor/List";

        //    string Error = objDal.DeleteVendorContacts(VendorContactId);
        //    string errorcode = Error.Split('_')[0];
        //    TempData["ErrMsg"] = Error.Split('_')[1];
        //    if ((TempData["ErrMsg"].ToString()) == "0")
        //    {

        //        TempData["ErrMsg"] = null;
        //    }

        //    ListCount--;
        //    if (ListCount == 0)
        //        PgNo = 1;
        //    return RedirectToAction("List", new { PageNo = PgNo, PageSize = PgSize });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string VendorId,string VendorContactId, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/Vendor/List";

            string ExistingFilePath = Server.MapPath("~/img/vendors/" + objDal.GetVendorByID(VendorId).VendorImage);
           string Error = objDal.DeleteVendorContacts(VendorId,VendorContactId);
            string errorcode = Error.Split('_')[0];
            TempData["ErrMsg"] = Error.Split('_')[1];
            if ((TempData["ErrMsg"].ToString()) == "")
            {
                if (errorcode == "1")
                {

                    FileInfo file = new FileInfo(ExistingFilePath);
                    if (file.Exists)
                        file.Delete();
                 
                }
                TempData["ErrMsg"] = null;
            }
            
                ListCount--;
            if (ListCount == 0)
                PgNo = 1;
            return RedirectToAction("List", new { PageNo = PgNo, PageSize = PgSize });
        }

        public ActionResult ChangeStatus(string VendorId, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/Vendor/List";
            TempData["ErrMsg"] = objDal.ChangeStatus(VendorId);
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

            objViewModel.ListSearchAutoComplete = objDal.VendorAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objViewModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }


        public ActionResult Vendorautocomplete(string term, string VendorType)
        {

            objCategoryModel.ClientVendorsListSearchAutoComplete = objDal.AdminVendorAutoComplete(term, VendorType);
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in objCategoryModel.ClientVendorsListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.Phone + "}" + item.Email + "}" + item.CatgoryName + "}" + item.Company + "}" + item.WebsiteLink + "}" + item.Location + "}" + item.VendorImage + "}" + item.Description + "}" + item.VendorId + "}" + item.Title));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public ActionResult VendorNameautocomplete(string term, string Vendor_Id)
        {

            objCategoryModel.VendorContactsAutocomplete = objVendorContact.VendorContactsAutoComplete(term, Vendor_Id);
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in objCategoryModel.VendorContactsAutocomplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.ContactName + "}" + item.ContactTitle + "}" + item.ContactPhone + "}" ));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        //public ActionResult VendorAddressautocomplete(string term, string Vendor_Id)
        //{

        //    objCategoryModel.VendorAddressAutocomplete = objDal.VendorAddressAutocomplete(term, Vendor_Id);
        //    var result = new List<KeyValuePair<string, string>>();
        //    foreach (var item in objCategoryModel.VendorContactsAutocomplete)
        //    {
        //        result.Add(new KeyValuePair<string, string>(item.searchResult, item.Vendor_Id));

        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);


        //}

    }
}