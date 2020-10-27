using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JobController : Controller
    {
        AdminCareerManageModel ObjModel = new AdminCareerManageModel();
        dalAdminCareer ObjDal = new dalAdminCareer();
        dalJobDepartment ObjDeptDal = new dalJobDepartment();
        MstJobDepartmentModel ObjDeptModel = new MstJobDepartmentModel();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            ObjModel.CareerList = ObjDal.JobGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/admin/Job/list";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvJobList", ObjModel);
            }
            return View(ObjModel);
        }

        public ActionResult Create()
        {
            ViewBag.ActiveURL = "/admin/Job/list";

            ObjModel.DepartmentDDL = ObjDal.DepartmentDDL();
            return View(ObjModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(AdminCareerManageModel Model)
        {
            ViewBag.ActiveURL = "/admin/Job/list";
            if (ModelState.IsValid)
            {
                Model.utblMstJobPositions.EndDate = DateTime.Now;
                //Model.utblMstJobPositions.JobContent = System.Net.WebUtility.HtmlDecode(Model.utblMstJobPositions.JobContent);
                TempData["ErrMsg"] = ObjDal.Save(Model.utblMstJobPositions);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }
            }
            //Model.utblMstJobPositions.JobContent = System.Net.WebUtility.HtmlDecode(Model.utblMstJobPositions.JobContent);

            Model.DepartmentDDL = ObjDal.DepartmentDDL();
            return View(Model);
        }

        public ActionResult Edit(int JobID)
        {
            ViewBag.ActiveURL = "/admin/job/list";

            ObjModel.utblMstJobPositions = ObjDal.GetJobID(JobID);
            ObjModel.DepartmentDDL = ObjDal.DepartmentDDL();
            ObjModel.utblMstJobPositions.JobContent = System.Web.HttpUtility.HtmlDecode(ObjModel.utblMstJobPositions.JobContent);
            return View(ObjModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(AdminCareerManageModel Model)
        {

            ViewBag.ActiveURL = "/admin/job/list";
            if (ModelState.IsValid)
            {
                Model.utblMstJobPositions.EndDate = DateTime.Now;
                TempData["ErrMsg"] = ObjDal.Save(Model.utblMstJobPositions);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");
                }
            }
            Model.utblMstJobPositions = ObjDal.GetJobID(Model.utblMstJobPositions.JobID);
            Model.utblMstJobPositions.JobContent = System.Web.HttpUtility.HtmlDecode(Model.utblMstJobPositions.JobContent);
            Model.DepartmentDDL = ObjDal.DepartmentDDL();

            return View("Edit",Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int JobID, int PgNo, int PgSize, int ListCount)
        {
            ViewBag.ActiveURL = "/admin/job/list";
            TempData["ErrMsg"] = ObjDal.Delete(JobID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            ListCount--;
            if (ListCount == 0)
                PgNo = 1;
            return RedirectToAction("List", new { PageNo = PgNo, PageSize = PgSize });

        }

        #region JobDepartment
        public ActionResult DepartmentList()
        {
            ObjDeptModel.MstDepartmentList = ObjDeptDal.MstDepartmentList;
            if(Request.IsAjaxRequest())
            {
                return PartialView("pvDepartmentList", ObjDeptModel);
            }
            return View(ObjDeptModel);
        }

        public ActionResult DepartmentCreate()
        {
            ViewBag.ActiveURL = "/admin/Job/DepartmentList";

            return View(ObjDeptModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepartmentCreate(MstJobDepartmentModel ItemData)
        {
            ViewBag.ActiveURL = "/admin/Job/DepartmentList";
            if (ModelState.IsValid)
            {
                ItemData.utblMstCareerDepartments.TotalPosition = 1;
                TempData["ErrMsg"] = ObjDeptDal.Save(ItemData.utblMstCareerDepartments);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("DepartmentList");
                }
            }
            return View(ItemData);
        }

        public ActionResult DepartmentEdit(int DepartmentID)
        {
            ViewBag.ActiveURL = "/admin/Job/DepartmentList";

            ObjDeptModel.utblMstCareerDepartments = ObjDeptDal.GetByID(DepartmentID);
            return View(ObjDeptModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepartmentEdit(MstJobDepartmentModel Model)
        {
            ViewBag.ActiveURL = "/admin/job/DepartmentList";
            if (ModelState.IsValid)
            {
                Model.utblMstCareerDepartments.TotalPosition = 1;
                TempData["ErrMsg"] = ObjDeptDal.Save(Model.utblMstCareerDepartments);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("DepartmentList");
                }
            }
            Model.utblMstCareerDepartments = ObjDeptDal.GetByID(Model.utblMstCareerDepartments.DepartmentID);
            return View(Model);
        }
        #endregion

        #region Delete Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepartmentDelete(int DepartmentID)
        {
            ViewBag.ActiveURL = "/admin/job/DepartmentList";
            TempData["ErrMsg"] = ObjDeptDal.Delete(DepartmentID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("DepartmentList");
        }
        #endregion
    }
}