using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers
{
    public class TestimonialsController : Controller
    {
        TestimonialViewModel objViewModel = new TestimonialViewModel();
        dalTestimonial objDal = new dalTestimonial();
        // GET: Testimonials
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            ViewBag.Current = "Testimonials";
            ViewBag.MenuActive = "Testimonials";
            ViewBag.SearchTerm = SearchTerm;
            int TotalRow;
            objViewModel.TestimonialList = objDal.TestimonialGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            //int items = (PageSize % TotalRow);

            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvTestimonialList", objViewModel);
            }
            return View(objViewModel);

        }

        public ActionResult Recent(int Id)
        {
            ViewBag.MenuActive = "Testimonials";

            objViewModel.Testimonial = objDal.GetTestimonialByID(Id);
            objViewModel.TestimonialPaging = objDal.GetTestimonialPrevNext(Id);
            return View(objViewModel);

        }
        public ActionResult page1()
        {
            ViewBag.MenuActive = "Testimonials";
            return View();
        }
        // GET: Testimonials
        public ActionResult page2()
        {
            ViewBag.MenuActive = "Testimonials";
            return View();
        }
    }
}