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


    public class BlogController : Controller
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
        // GET: /Admin/Blog/
        BlogViewModel objViewModel = new BlogViewModel();
        dalBlog objDal = new dalBlog();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            objViewModel.BlogList = objDal.BlogGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/blog/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvBlogList", objViewModel);
            }
            return View(objViewModel);
        }
        public ActionResult CreateBlog()
        {
            ViewBag.ActiveURL = "/admin/blog/list";

            return View("Blog", objViewModel.utblBlogs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateBlog(HttpPostedFileBase files, BlogViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "/admin/blog/list";
            if (ModelState.IsValid)
            {
                if (files != null && files.ContentLength > 0)
                {
                    Model.utblBlogs.BlogFile = Path.GetExtension(files.FileName);
                    TempData["ErrMsg"] = objDal.Save(Model.utblBlogs, CreatedBy);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                        //var BlogID = Model.utblBlogs.BlogID;
                        string ext = Path.GetExtension(files.FileName);
                        var fileName = Model.utblBlogs.BlogID;
                        var path = string.Concat(Server.MapPath("~/UploadedFiles/Blogs/" + fileName + ext));
                        files.SaveAs(path);
                        return RedirectToAction("list");
                    }
                }
                TempData["ErrMsg"] = objDal.Save(Model.utblBlogs, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }

            }
            return View("Blog", Model);
        }
        public ActionResult EditBlog(string BlogID)
        {
            ViewBag.ActiveURL = "/admin/blog/list";

            objViewModel.utblBlogs = objDal.GetBlogByID(BlogID);
            objViewModel.utblBlogs.BlogContent = System.Web.HttpUtility.HtmlDecode(objViewModel.utblBlogs.BlogContent);
            return View(objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditBlog(HttpPostedFileBase files, BlogViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "/admin/blog/list";
            if (ModelState.IsValid)
            {
                if (files != null && files.ContentLength > 0)
                {
                    Model.utblBlogs.BlogFile = Path.GetExtension(files.FileName);
                    TempData["ErrMsg"] = objDal.Edit(Model.utblBlogs, CreatedBy);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                        //var BlogID = Model.utblBlogs.BlogID;
                        string ext = Path.GetExtension(files.FileName);
                        var fileName = Model.utblBlogs.BlogID;
                        var path = string.Concat(Server.MapPath("~/UploadedFiles/Blogs/" + fileName + ext));
                        files.SaveAs(path);
                        return RedirectToAction("list");
                    }
                }

                TempData["ErrMsg"] = objDal.Edit(Model.utblBlogs, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }
            }
            return View("EditBlog", Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string BlogID, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/UserAdmin/List";
            TempData["ErrMsg"] = objDal.Delete(BlogID);
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