using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.Calculator;
using RealEstate.Entities.DataAccess;

namespace RealEstate.Web.Areas.Client.Controllers
{
    [Authorize]
    public class ToolsController : Controller
    {
        MstMortgageCalculator ObjModel = new MstMortgageCalculator();
        dalMortgagecalculator ObjDal = new dalMortgagecalculator();



        public ActionResult NetProceeds()
        {
            ViewBag.ActiveURL = "NetProceeds";
            return View();
        }

        public ActionResult QuickQuote()
        {
            ViewBag.ActiveURL = "QuickQuote";
            return View();
        }
        public ActionResult ClosingCost()
        {
            ViewBag.ActiveURL = "Calculator";
            return View();
        }

        //public ActionResult Mortgage()
        //{
        //    ObjModel.Region = ObjDal.StateDropDown();
        //    ObjModel.TimePeriod = ObjDal.YearDropDown();
        //    return View(ObjModel);
        //}

        //[HttpPost]
        public ActionResult Mortgage(MstMortgageCalculator ItemData)
        {
            ObjModel.Region = ObjDal.StateDropDown();
            ObjModel.TimePeriod = ObjDal.YearDropDown();
            ObjModel.DirectCost = new MstCalcDirectCost();
            ObjModel.Inputs = new MstCalcInputs();
            ObjModel.Inputs.StateID = 1;
            ObjModel.Inputs.HousePrice = 700000;
            ObjModel.Inputs.DownPercent = 20;
            ObjModel.Inputs.TimeFrame = 30;
            ObjModel.Inputs.MortgageRate = 4.00;
           
            if (Request.IsAjaxRequest())
            {
                ItemData = ObjDal.Calculate(ItemData);
                return PartialView("pvCalc", ItemData);
            }
            ObjModel = ObjDal.Calculate(ObjModel);

            return View(ObjModel);
        }

        public ActionResult Affordibility(MstMortgageCalculator ItemData)
        {
            ObjModel.Region = ObjDal.StateDropDown();
            ObjModel.TimePeriod = ObjDal.YearDropDown();
            ObjModel.DirectCost = new MstCalcDirectCost();
            ObjModel.Inputs = new MstCalcInputs();
            ObjModel.Inputs.StateID = 1;
            ObjModel.Inputs.HousePrice = 455000;
            ObjModel.Inputs.DownPercent = 20;
            ObjModel.Inputs.TimeFrame = 30;
            ObjModel.Inputs.MortgageRate = 4.00;
            ObjModel.Inputs.PropertyTaxPercent = 0.57;
            ObjModel.Inputs.PropertyTax = Math.Round(((ObjModel.Inputs.HousePrice * 0.57) / 100) );
           
            if (Request.IsAjaxRequest())
            {
                ItemData = ObjDal.Calculate(ItemData);
                return PartialView("pvDirectCost", ItemData);
            }
            ObjModel = ObjDal.Calculate(ObjModel);

            return View(ObjModel);
        }

    }
}