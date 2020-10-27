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
    public class QuizesController : Controller
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
        QuizViewModel objViewModel = new QuizViewModel();
        dalQuiz objDal = new dalQuiz();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            objViewModel.BuyerQuizList = objDal.QuizGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/quizes/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvQuizUsersList", objViewModel);
            }
            return View(objViewModel);
        }

        public ActionResult autocomplete(string term)
        {

            objViewModel.ListSearchAutoComplete = objDal.QuizAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objViewModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ViewQuiz(string Email)
        {
            ViewBag.ActiveURL = "/admin/quizes/list";
            objViewModel.BuyerQuizList = objDal.GetQuizByEmail(Email);
            return View(objViewModel);
        }

    }
}