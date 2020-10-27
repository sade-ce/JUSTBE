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
    public class SuggestionController : Controller
    {
        // GET: Admin/Suggestion
        dalMstEnquire objEnquire = new dalMstEnquire();
        public ActionResult List(int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            MstArticleSuggestionManageModel model = new MstArticleSuggestionManageModel();
            model.MstSuggestionList = objEnquire.GetSugesstionPaged(PageNo, PageSize, out TotalRow);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/Suggestion/List";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvSuggestionList", model);
            }
            return View(model);
        }

    }
}