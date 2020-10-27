using RealEstate.Entities.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.ViewModels;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnquiryController : Controller
    {
        // GET: Admin/Enquiry
        dalMstEnquire objEnquire = new dalMstEnquire();
        public ActionResult List(int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            MstEnquireViewModel model = new MstEnquireViewModel();
            model.MstEnquireList = objEnquire.GetEnquiryPaged(PageNo, PageSize, out TotalRow);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/Enquiry/List";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvEnquiryList", model);
            }
            return View(model);
        }

        public ActionResult Contact(int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            MstContactViewModel model = new MstContactViewModel();
            model.MstContactList = objEnquire.GetContactPaged(PageNo, PageSize, out TotalRow);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/Enquiry/Contact";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvContactList", model);
            }
            return View(model);
        }
    }
}