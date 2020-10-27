using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class VendorV2Controller : Controller
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
        // GET: Admin/VendorV2
        VendorViewModel objViewModel = new VendorViewModel();
        VendorCategoryViewModel objCategoryModel = new VendorCategoryViewModel();
        dalVendor objDal = new dalVendor();
        dalVendorContacts objVendorContact = new dalVendorContacts();

        public ActionResult List(string searchterm = "", int PageNo = 1, int PageSize = 10, string sortOrder = "")
        {
            int TotalRow;
            objViewModel.VendorSortingInfo = new VendorSortingInfo
            {
                CurrentSort = sortOrder,
                TitleSort = String.IsNullOrEmpty(sortOrder) ? "Title_asc" : "",
                VendorTypeSort = sortOrder == "type_asc" ? "type_desc" : "type_asc",
                CreatedOnSort = sortOrder == "createdon_asc" ? "createdon_desc" : "createdon_asc",
               CreatedBySort = sortOrder == "createdby_asc" ? "createdby_desc" : "createdby_asc",
                UserTypeSort = sortOrder == "usertype_asc" ? "usertype_desc" : "usertype_asc",
                IsRecommendedSort = sortOrder == "recommended_asc" ? "recommended_desc" : "recommended_asc",
            };
            if (searchterm != null)
            {
                searchterm = searchterm.Trim();
                searchterm = Regex.Replace(searchterm, @"\s+", " ");
            }
            objViewModel.VendorFilterInfo = new VendorFilterInfo
            {
                SearchFilter = searchterm
            };
            objViewModel.VendorList = objDal.VendorGetPagedV2(PageNo, PageSize, out TotalRow, searchterm,sortOrder);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "VendorList";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvvendorList", objViewModel);
            }
            return View(objViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string VendorId, string VendorContactId, int PgNo, int PgSize, int ListCount)
        {
  

            string ExistingFilePath = Server.MapPath("/img/vendors/" + objDal.GetVendorByID(VendorId).VendorImage);
            string Error = objDal.DeleteVendorContacts(VendorId, VendorContactId);
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

        public ActionResult ChangeStatus(string VendorId, int PageNo, int PageSize, int ListCount, string sortOrder = "", string searchterm = "")
        {
            ViewBag.ActiveURL = "VendorList";
            TempData["ErrMsg"] = objDal.ChangeStatus(VendorId);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            ListCount--;
            if (ListCount == 0)
                ListCount = 1;
            return RedirectToAction("List", new { PageNo = PageNo, PageSize = PageSize, sortOrder = sortOrder, searchterm = searchterm});

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


        public ActionResult Create()
        {
            ViewBag.ActiveURL = "Vendor";
            objViewModel.CategoryDropDown = objDal.GetVendorCategoryDDl();
            return View("Create", objViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(VendorViewModel Model, FormCollection form)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "Vendor";
            Model.CategoryDropDown = objDal.GetVendorCategoryDDl();
            if (ModelState.IsValid)
            {

                var image = form["upfiles"];
                string fileName = "";
                if (!string.IsNullOrEmpty(image))
                {
                    dynamic data = JObject.Parse(image);
                    string imagename = data.output.name;
                    string imagedata = data.output.image;
                    string imageType = imagedata.Substring(0, imagedata.IndexOf(',', imagedata.IndexOf(',')) + 1);
                    string convert = imagedata.Replace(imageType, String.Empty);


                    var guid = Guid.NewGuid().ToString().Substring(0, 6);
                   // fileName = "/img/vendors/" + guid + imagename;
                    Model.Vendor.VendorImage = guid + imagename;
                    byte[] image64 = Convert.FromBase64String(convert);
                    TempData["ErrMsg"] = objDal.SaveVersion2(Model.Vendor, CreatedBy);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                        using (var ms = new MemoryStream(image64))
                        {
                            var images = System.Drawing.Image.FromStream(ms);
                            System.Drawing.Image img = images.GetThumbnailImage(Convert.ToInt32(data.output.width), Convert.ToInt32(data.output.height), null, IntPtr.Zero);
                            img.Save(Server.MapPath("/img/vendors/" + guid + imagename), System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        return RedirectToAction("Edit", new { VendorId = Model.Vendor.VendorId });
                    }
                  
                }
                TempData["ErrMsg"] = objDal.SaveVersion2(Model.Vendor, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("Edit", new { VendorId = Model.Vendor.VendorId });
                }

            }
            return View("Create", Model);
        }


        public ActionResult Edit(string VendorId)
        {
            if (string.IsNullOrEmpty(VendorId))
                ViewBag.ActiveURL = "Vendor";
            objViewModel.CategoryDropDown = objDal.GetVendorCategoryDDl();
            if (!string.IsNullOrEmpty(VendorId))
            {
                objViewModel.VendorViews = objDal.GetVendorByVendorID(VendorId);
                objViewModel.Vendor = objDal.GetVendorByID(VendorId);
            }
            //if (!string.IsNullOrEmpty(VendorContactId))
            //    objViewModel.VendorContacts = objDal.GetVendorContactByID(Convert.ToInt32(VendorContactId));
            return View(objViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(VendorViewModel Model, FormCollection form)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "Vendor";
            Model.CategoryDropDown = objDal.GetVendorCategoryDDl();
            ModelState.Remove("VendorContacts.VendorContactId");
            if (ModelState.IsValid)
            {
                var image = form["upfiles"];
                if (!string.IsNullOrEmpty(image))
                {
                    if (Model.Vendor.VendorImage != null)
                    {
                        if (Model.Vendor.VendorImage.IndexOf("data:image") == -1)
                        {
                            string ExistingFilePath = Server.MapPath("~/img/vendors/" + Model.Vendor.VendorImage);
                            FileInfo file = new FileInfo(ExistingFilePath);
                            if (file.Exists)
                                file.Delete();
                        }
                    }
                    if (image == "PhotoDeleted")
                    {

                        Model.Vendor.VendorImage = "";
                    }
                    else
                    {


                        dynamic data = JObject.Parse(image);
                        string imagename = data.output.name;
                        string imagedata = data.output.image;
                        string imageType = imagedata.Substring(0, imagedata.IndexOf(',', imagedata.IndexOf(',')) + 1);
                        string convert = imagedata.Replace(imageType, String.Empty);


                        var guid = Guid.NewGuid().ToString().Substring(0, 6);
                        // fileName = "/img/vendors/" + guid + imagename;
                        Model.Vendor.VendorImage = guid + imagename;
                        byte[] image64 = Convert.FromBase64String(convert);

                        TempData["ErrMsg"] = objDal.EditVersion2(Model.Vendor, CreatedBy);
                        if ((TempData["ErrMsg"].ToString()) == "0")
                        {


                            TempData["ErrMsg"] = null;
                            using (var ms = new MemoryStream(image64))
                            {
                                var images = System.Drawing.Image.FromStream(ms);
                                System.Drawing.Image img = images.GetThumbnailImage(Convert.ToInt32(data.output.width), Convert.ToInt32(data.output.height), null, IntPtr.Zero);
                                img.Save(Server.MapPath("/img/vendors/" + guid + imagename), System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                            return RedirectToAction("Edit", new { VendorId = Model.Vendor.VendorId });
                        }
                    }
                }
                
                TempData["ErrMsg"] = objDal.EditVersion2(Model.Vendor, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("Edit", new { VendorId = Model.Vendor.VendorId });
                }

            }
            return View("Edit", Model);
        }

        public JsonResult VendorContacts(string VendorId)
        {

            return Json(objDal.GetVendorContacts(VendorId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddContact(VendorContactView data)
        {
            
            string errorMessage = "";
            errorMessage = objVendorContact.SaveVendorContactV2(data);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getContactByID(string VendorContactId)
        {

            return Json(objDal.GetVendorContactByID( Convert.ToInt32(VendorContactId)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateContact(VendorContacts data)
        {
            string errorMessage = "";
            errorMessage = objVendorContact.EditVendorContact(data);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteContact(string Id)
        {
            string errorMessage = objDal.DeleteVendorContactsV2(Id);

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
    }
}