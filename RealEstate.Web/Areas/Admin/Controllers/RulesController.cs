using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using Microsoft.AspNet.Identity;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class RulesController : Controller
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
        // GET: Admin/VendorType
        RulesViewModel objViewModel = new RulesViewModel();
        dalRules objDal = new dalRules();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            objViewModel.RulesList = objDal.RulesGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/rules/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvrulesList", objViewModel);
            }
            return View(objViewModel);
        }
        public ActionResult Create()
        {
            ViewBag.ActiveURL = "/admin/Rules/list";

            return View("Create", objViewModel.Rules);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(RulesViewModel Model)
        {
            string CreatedBy = User.Identity.GetUserId();
            ViewBag.ActiveURL = "/admin/Rules/list";
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = objDal.Save(Model.Rules, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }
                else
                {
                    TempData["ErrMsg"] = null;
                    ModelState.AddModelError("", "Unable to add vendor type, record already exists...");
                }

            }
            return View("Create", Model);
        }


        public ActionResult Edit(int RuleId)
        {
            ViewBag.ActiveURL = "/admin/Rules/list";

            objViewModel.RulesModel = objDal.GetRulesByID(RuleId);
            return View(objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(RulesViewModel Model)
        {
            string CreatedBy = User.Identity.GetUserId();
            ViewBag.ActiveURL = "/admin/Rules/list";
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = objDal.Edit(Model.RulesModel, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }
                else
                {
                    TempData["ErrMsg"] = null;
                    ModelState.AddModelError("", "Unable to add vendor type, record already exists...");
                }
            }
            return View("Edit", Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int RuleId, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/Rules/List";
         
            TempData["ErrMsg"] = objDal.Delete(RuleId);
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

            objViewModel.ListSearchAutoComplete = objDal.RuleSAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objViewModel.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
    }
}