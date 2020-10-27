using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.Text.RegularExpressions;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class TestimonialV2Controller : Controller
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
        // GET: /Admin/Testimonial/
        TestimonialViewModel objViewModel = new TestimonialViewModel();
        dalTestimonial objDal = new dalTestimonial();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10, string sortOrder = "")
        {
            int TotalRow;
            objViewModel.TestimonialSortingInfo = new TestimonialSortingInfo
            {
                CurrentSort = sortOrder,
                TitleSort = String.IsNullOrEmpty(sortOrder) ? "Title_asc" : "",
               TypeSort = sortOrder == "type_asc" ? "type_desc" : "type_asc",
                CreatedOnSort = sortOrder == "createdon_asc" ? "createdon_desc" : "createdon_asc",
                RatingSort =sortOrder == "rating_asc" ? "rating_desc" : "rating_asc",
            };
            if (SearchTerm != null)
            {
                SearchTerm = SearchTerm.Trim();
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            objViewModel.TestimonialFilterInfo = new TestimonialFilterInfo
            {
                SearchFilter = SearchTerm
            };
            objViewModel.TestimonialList = objDal.TestimonialGetPagedVersion2(PageNo, PageSize, out TotalRow, SearchTerm,sortOrder);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "Testmonial";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvTestimonialList", objViewModel);
            }
            return View(objViewModel);
        }
        public ActionResult Create()
        {
            ViewBag.ActiveURL = "AddTestmonial";

            return View(objViewModel.Testimonial);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(TestimonialViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "AddTestmonial";
            if (ModelState.IsValid)
            {
                //if (files != null && files.ContentLength > 0)
                //{
                //    Model.utblBlogs.BlogFile = Path.GetExtension(files.FileName);
                //    TempData["ErrMsg"] = objDal.Save(Model.utblBlogs, CreatedBy);
                //    if ((TempData["ErrMsg"].ToString()) == "0")
                //    {
                //        TempData["ErrMsg"] = null;
                //        //var BlogID = Model.utblBlogs.BlogID;
                //        string ext = Path.GetExtension(files.FileName);
                //        var fileName = Model.utblBlogs.BlogID;
                //        var path = string.Concat(Server.MapPath("~/UploadedFiles/Blogs/" + fileName + ext));
                //        files.SaveAs(path);
                //        return RedirectToAction("list");
                //    }
                //}
                TempData["ErrMsg"] = objDal.Save(Model.Testimonial, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list", "TestimonialV2", new { area = "Admin" });
                }

            }
            return View("Create","TestimonialV2", Model);
        }
        public ActionResult Edit(int Id)
        {
            ViewBag.ActiveURL = "AddTestmonial";

            objViewModel.Testimonial = objDal.GetTestimonialByID(Id);
            objViewModel.Testimonial.Description = System.Web.HttpUtility.HtmlDecode(objViewModel.Testimonial.Description);

            return View(objViewModel);
        }
        [HttpPost]
       // [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(TestimonialViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "AddTestmonial";
            if (ModelState.IsValid)
            {
                //if (files != null && files.ContentLength > 0)
                //{
                //    Model.utblBlogs.BlogFile = Path.GetExtension(files.FileName);
                //    TempData["ErrMsg"] = objDal.Edit(Model.utblBlogs, CreatedBy);
                //    if ((TempData["ErrMsg"].ToString()) == "0")
                //    {
                //        TempData["ErrMsg"] = null;
                //        //var BlogID = Model.utblBlogs.BlogID;
                //        string ext = Path.GetExtension(files.FileName);
                //        var fileName = Model.utblBlogs.BlogID;
                //        var path = string.Concat(Server.MapPath("~/UploadedFiles/Blogs/" + fileName + ext));
                //        files.SaveAs(path);
                //        return RedirectToAction("list");
                //    }
                //}

                TempData["ErrMsg"] = objDal.Edit(Model.Testimonial, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list","TestimonialV2",new { area = "Admin" });
                }
            }
            return View("Edit", "TestimonialV2", Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "Testmonial";
            TempData["ErrMsg"] = objDal.Delete(Id);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            ListCount--;
            if (ListCount == 0)
                PgNo = 1;
            return RedirectToAction("List", "TestimonialV2", new { PageNo = PgNo, PageSize = PgSize, area = "Admin" });

        }
    }
}