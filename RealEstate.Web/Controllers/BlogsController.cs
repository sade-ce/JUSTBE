using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.Models;

namespace RealEstate.Web.Controllers
{
    public class BlogsController : Controller
    {
        //
        // GET: /Blogs/
        BlogViewModel objViewModel = new BlogViewModel();
        dalBlog objDal = new dalBlog();
        dalMstEnquire ObjEnqDal = new dalMstEnquire();

        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 3)
        {
            ViewBag.Current = "Blogs";
              ViewBag.MenuActive = "Blogs";
            ViewBag.SearchTerm = SearchTerm;
            int TotalRow;
            objViewModel.BlogList = objDal.BlogGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            //int items = (PageSize % TotalRow);

            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvBlogList", objViewModel);
            }
            return View(objViewModel);

        }

        public ActionResult Recent(string BlogID)
        {
            ViewBag.Current = "Blogs";
            ViewBag.MenuActive = "Blogs";
            objViewModel.utblBlogs = objDal.GetBlogByID(BlogID);
            objViewModel.BlogPaging = objDal.GetBlogPrevNext(BlogID);
            return View(objViewModel);

        }

        [HttpPost]
        public ActionResult Enquiry(utblMstArticleEnquire ItemData)
        {
            if (string.IsNullOrEmpty(ItemData.Name) ||
                string.IsNullOrEmpty(ItemData.Email) ||
                string.IsNullOrEmpty(ItemData.Message)
                )
            {
                return Json("* Please fill all fields", JsonRequestBehavior.AllowGet);

            }
            TempData["ErrMsg"] = ObjEnqDal.ArticleEnquire(ItemData);
            return Json("Thanks for your submission, we will be in touch shortly ", JsonRequestBehavior.AllowGet);

        }

    }
}