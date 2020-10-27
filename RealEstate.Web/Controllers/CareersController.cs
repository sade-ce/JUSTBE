using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers
{
    public class CareersController : Controller
    {
        MstCareerManageModel ObjModel = new MstCareerManageModel();
        dalCareer ObjDal = new dalCareer();
        public ActionResult Departments()
        {
            ViewBag.Current = "Index";

            ObjModel.MstPositionList = ObjDal.GetDepartmentList();

            return View(ObjModel);
        }

        public ActionResult Position(int DepartmentID)
        {
            ViewBag.Current = "Index";

            ObjModel.Career = ObjDal.GetJobByDepartmentID(DepartmentID);
            return View(ObjModel);
        }
    }
}