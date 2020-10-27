using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class TeamMemberController : Controller
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
        // GET: Admin/TeamMember
        TeamMemberViewModel objViewModel = new TeamMemberViewModel();
        dalTeamMember objDal = new dalTeamMember();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            objViewModel.TeamMemberList = objDal.TeamMemberGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            objViewModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/teammember/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvTeamMemberList", objViewModel);
            }
            return View(objViewModel);
        }
        public ActionResult Create()
        {
            ViewBag.ActiveURL = "/admin/teammember/list";
            return View("Create", objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(HttpPostedFileBase files, TeamMemberViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "/admin/teammember/list";
    
            if (ModelState.IsValid)
            {
                if (files != null && files.ContentLength > 0)
                {
                    var guid = Guid.NewGuid().ToString().Substring(0, 4);
                    Model.TeamMembers.MemberImage = guid + Path.GetExtension(files.FileName);
                    TempData["ErrMsg"] = objDal.Save(Model.TeamMembers, CreatedBy);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                        string ext = guid + Path.GetExtension(files.FileName);
                        var fileName = Model.TeamMembers.Id;
                        var path = string.Concat(Server.MapPath("~/img/teamMembers/" + fileName + ext));
                        files.SaveAs(path);
                        return RedirectToAction("list");
                    }
                }
                TempData["ErrMsg"] = objDal.Save(Model.TeamMembers, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }



            }
            return View("Create", Model);
        }


        public ActionResult Edit(string Id)
        {
            ViewBag.ActiveURL = "/admin/teammember/list";
            objViewModel.TeamMembers = objDal.GetTeamMemberByID(Id);
            return View(objViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase files, TeamMemberViewModel Model)
        {
            string CreatedBy = User.Identity.Name;
            ViewBag.ActiveURL = "/admin/teammembers/list";
            if (ModelState.IsValid)
            {
                if (files != null && files.ContentLength > 0)
                {

                    string ExistingFilePath = Server.MapPath("~/img/teamMembers/" + Model.TeamMembers.MemberImage);
                    FileInfo file = new FileInfo(ExistingFilePath);

                    var guid = Guid.NewGuid().ToString().Substring(0, 4);

                    Model.TeamMembers.MemberImage = Model.TeamMembers.Id + guid + Path.GetExtension(files.FileName);
                    TempData["ErrMsg"] = objDal.Edit(Model.TeamMembers, CreatedBy);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        if (file.Exists)
                            file.Delete();

                        TempData["ErrMsg"] = null;
                        string ext = guid + Path.GetExtension(files.FileName);
                        var fileName = Model.TeamMembers.Id;
                        var path = string.Concat(Server.MapPath("/img/teamMembers/" + fileName + ext));
                        files.SaveAs(path);
                        return RedirectToAction("list");
                    }

                }

                TempData["ErrMsg"] = objDal.Edit(Model.TeamMembers, CreatedBy);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }


            }
            return View("Edit", Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string Id, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/TeamMember/List";
            string ExistingFilePath = Server.MapPath("/img/teamMembers/" + objDal.GetTeamMemberByID(Id).MemberImage);
            FileInfo file = new FileInfo(ExistingFilePath);
            TempData["ErrMsg"] = objDal.Delete(Id);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (file.Exists)
                    file.Delete();
                TempData["ErrMsg"] = null;
            }
            ListCount--;
            if (ListCount == 0)
                PgNo = 1;
            return RedirectToAction("List", new { PageNo = PgNo, PageSize = PgSize });
        }


    }
}