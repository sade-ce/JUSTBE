using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class BuildingController : Controller
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

        BuildingViewModel objViewModel = new BuildingViewModel();
        dalBuildings objDal = new dalBuildings();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            objViewModel.BuildingList = objDal.BuildingGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/building/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvbuildingList", objViewModel);
            }
            return View(objViewModel);
        }
        public ActionResult Create()
        {
            ViewBag.ActiveURL = "/admin/building/list";
            objViewModel.BuildingAttachmentsDropDown = objDal.GetBuildingsAttachments();
            objViewModel.NeighborhoodDDL = objDal.GetNeighborhood();
            objViewModel.RulesDropDown = objDal.GetRules();
            return View("Create", objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase files, BuildingViewModel Model)
        {
            var CreatedBy = UserManager.FindByEmail(User.Identity.Name).Id;
            ViewBag.ActiveURL = "/admin/building/list";
            Model.Building.Neighborhood_Id = Convert.ToInt32(Model.Building.Neighborhood_Id);
            if (Model.Building.NumberOfUnits == 0)
                Model.Building.NumberOfUnits = 0;
            else
                Model.Building.NumberOfUnits = Convert.ToInt32(Model.Building.NumberOfUnits);
       
            
            if (ModelState.IsValid)
            {

                TempData["ErrMsg"] = objDal.Save(Model.Building, Model.RulesDropDown, CreatedBy);
                String[] ErrorMessage = TempData["ErrMsg"].ToString().Split('_');
                string ErrorCode = ErrorMessage[0];
                string BuildingId= ErrorMessage[1];
                if (ErrorCode == "0")
                {
                  
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("Edit",new { BuildingId = BuildingId });
                }
                
            }
            else
            {
                Model.RulesDropDown = objDal.GetRules();
                Model.NeighborhoodDDL = objDal.GetNeighborhood();
            }
            return View("Create", Model);
        }


        public ActionResult Edit(string BuildingId)
        {
            if (string.IsNullOrEmpty(BuildingId))
                ViewBag.ActiveURL = "/admin/building/list";
            //   objViewModel.BuildingAttachmentsDropDown = objDal.GetBuildingsAttachments();
            int Id = Convert.ToInt32(BuildingId);
            if (!string.IsNullOrEmpty(BuildingId))
            {
                objViewModel.RulesDropDown = objDal.GetRulesByBuilding(Id);
                objViewModel.NeighborhoodDDL = objDal.GetNeighborhood();
                objViewModel.Building = objDal.GetBuildingByID(Id);
                objViewModel.BuildingAttachmentsDropDown = objDal.GetBuildingsAttachments();
            }

            return View(objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(BuildingViewModel Model)
        {
            var CreatedBy = UserManager.FindByEmail(User.Identity.Name).Id;
            ViewBag.ActiveURL = "/admin/building/list";
            Model.Building.Neighborhood_Id = Convert.ToInt32(Model.Building.Neighborhood_Id);
            Model.Building.NumberOfUnits = Convert.ToInt32(Model.Building.NumberOfUnits);
            Model.Building.BuildingId = Convert.ToInt32(Model.Building.BuildingId);

            if (ModelState.IsValid)
            {
                

                TempData["ErrMsg"] = objDal.Edit(Model.Building, Model.RulesDropDown, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {

                    TempData["ErrMsg"] = null;

                }
                return RedirectToAction("Edit", new { BuildingId = Model.Building.BuildingId });

            }
            else
            {
                Model.RulesDropDown = objDal.GetRulesByBuilding(Model.Building.BuildingId);
                Model.NeighborhoodDDL = objDal.GetNeighborhood();
                //Model.Building = objDal.GetBuildingByID(Model.Building.BuildingId);
            }

            return View("Edit", Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int BuildingId, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/building/List";
            //string ExistingFilePath = Server.MapPath("~/img/vendors/" + objDal.GetVendorByID(VendorId).VendorImage);
            //FileInfo file = new FileInfo(ExistingFilePath);
            TempData["ErrMsg"] = objDal.Delete(BuildingId);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                //if (file.Exists)
                //    file.Delete();
                TempData["ErrMsg"] = null;
            }
            ListCount--;
            if (ListCount == 0)
                PgNo = 1;
            return RedirectToAction("List", new { PageNo = PgNo, PageSize = PgSize });
        }

        public ActionResult ChangeStatus(string BuildingId, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/building/List";
            TempData["ErrMsg"] = objDal.ChangeStatus(BuildingId);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            ListCount--;
            if (ListCount == 0)
                PgNo = 1;
            return RedirectToAction("List", new { PageNo = PgNo, PageSize = PgSize });
        }

        public ActionResult autocomplete(string term)
        {

            objViewModel.ListSearchAutoComplete = objDal.BuildingAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objViewModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetBuildingById(int BuildingId)
        {
            return Json(objDal.GetBuildingByID(BuildingId), JsonRequestBehavior.AllowGet);
        }

        //---------------- Building Documents------------------------//

        public JsonResult GetBuildingDocuments(int BuildingId)
        {
            return Json(objDal.GetBuildingDocuments(BuildingId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddBuildingDocument(BuildingDocuments document, HttpPostedFileBase DocFile)
        {
            document.CreatedBy = UserManager.FindByEmail(User.Identity.Name).Id;
            string errorMessage = "";
            if (DocFile != null && DocFile.ContentLength > 0)
            {
                var guid = Guid.NewGuid().ToString().Substring(0, 4);
                string Filename = guid + DocFile.FileName;
                document.AttachmentFile = Filename;
                errorMessage = objDal.SaveBuildingDocument(document);
                if (errorMessage == "0")
                {
                    var path = string.Concat(Server.MapPath("~/UploadedFiles/BuildingDocuments/" + Filename));
                    DocFile.SaveAs(path);
                }
            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteBuildingDocument(int ID)
        {
            string ExistingFilePath = Server.MapPath("~/UploadedFiles/BuildingDocuments/" + objDal.GetBuildingFileName(ID));
            FileInfo file = new FileInfo(ExistingFilePath);
            string errorMessage = objDal.DeleteBuildingDoc(ID);
            if (errorMessage == "0")
            {
                if (file.Exists)
                    file.Delete();
            }

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }




        //---------------- Building Documents------------------------//


        //----------------------Building Vendor -------------------------------------------
        public JsonResult ClientVendorList(string BuildingId)
        {
            return Json(objDal.GetBuildingVendorList(Convert.ToInt32(BuildingId)), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddBuildingVendor(FormCollection form)
        {
            var CreatedBy = UserManager.FindByEmail(User.Identity.Name).Id;
            var CreatedByEmail = UserManager.FindByEmail(User.Identity.Name).Email;
            string errorMessage = "";
            Vendor vendor = new Vendor();
            vendor.Category_Id = form["VendorType"].ToString();
            vendor.Title = form["txtTitleV"].ToString();
            vendor.Email = form["Email"].ToString();
            vendor.Phone = form["Phone"].ToString();
            errorMessage = objDal.SaveBuildingVendor(vendor, CreatedBy, Convert.ToInt32(form["Building_Id"].ToString()), CreatedByEmail);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuiildingVendorById(string ID)
        {
            var vendor = objDal.GetVendorByIDBuilding(ID);
            return Json(vendor, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateBuildingVendor(FormCollection form)
        {

            var CreatedBy = UserManager.FindByEmail(User.Identity.Name).Id;
            string errorMessage = "";
            Vendor vendor = new Vendor();
            vendor.Category_Id = form["VendorType"].ToString();
            vendor.Title = form["txtTitleV"].ToString();
            vendor.Email = form["Email"].ToString();
            vendor.Phone = form["Phone"].ToString();
            vendor.VendorId = form["Id"].ToString();
            int BuildingId = Convert.ToInt32(form["Building_Id"].ToString());
            int BuildingVendorId = Convert.ToInt32(form["BuildingVendorId"].ToString());
            errorMessage = objDal.EditBuildingVendor(vendor, CreatedBy, BuildingId, BuildingVendorId);

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteBuildingVendor(string ID)
        {
            string errorMessage = "";
            //string ExistingFilePath = Server.MapPath("~/img/vendors/" + objVendorDal.GetVendorByID(ID).VendorImage);
            //FileInfo file = new FileInfo(ExistingFilePath);
            errorMessage = objDal.DeleteBuildingVendor(ID);
            //if (errorMessage == "0")
            //{
            //    if (file.Exists)
            //        file.Delete();

            //}

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        //----------------------Building Vendor -------------------------------------------

        //----------------------Building Gallery -------------------------------------------
        public JsonResult GalleryList(string BuildingId)
        {
            return Json(objDal.GetGalleryList(Convert.ToInt32(BuildingId)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddGallery(FormCollection form)
        {
            var CreatedBy = UserManager.FindByEmail(User.Identity.Name).Id;
            string errorMessage = "";
            var image = form["upGalleryfiles"];
            if (image.StartsWith(",")) { image = image.Substring(1); }
            if (!string.IsNullOrEmpty(image))
            {

                dynamic data = JObject.Parse(image);
                string imagename = data.output.name;
                string imagedata = data.output.image;
                string imageType = imagedata.Substring(0, imagedata.IndexOf(',', imagedata.IndexOf(',')) + 1);
                string convert = imagedata.Replace(imageType, String.Empty);

                var guid = Guid.NewGuid().ToString().Substring(0, 6);
                string fileName = guid + imagename;

                GalleryView gallery = new GalleryView();
                gallery.Building_Id = Convert.ToInt32(form["Building_Id"].ToString());
                gallery.Description = "";
                gallery.Image = fileName;
                gallery.CreatedBy = CreatedBy;

                errorMessage = objDal.SaveGallery(gallery);
                if (errorMessage == "0")
                {
                    byte[] image64 = Convert.FromBase64String(convert);
                    using (var ms = new MemoryStream(image64))
                    {
                        var images = System.Drawing.Image.FromStream(ms);
                        System.Drawing.Image img = images.GetThumbnailImage(Convert.ToInt32(data.output.width), Convert.ToInt32(data.output.height), null, IntPtr.Zero);
                        img.Save(Server.MapPath("~/UploadedFiles/BuildingImages/" + fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }



            }
            return Json(errorMessage, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteBuildingGallery(int ID)
        {
            string errorMessage = "";
            string ExistingFilePath = Server.MapPath("~/UploadedFiles/BuildingImages/" + objDal.GetImageByBuildingGalleryId(ID).Image);
            FileInfo file = new FileInfo(ExistingFilePath);
            errorMessage = objDal.DeleteGallery(ID);
            if (errorMessage == "0")
            {
                if (file.Exists)
                    file.Delete();

            }

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        //----------------------Building Gallery -------------------------------------------

    }



}