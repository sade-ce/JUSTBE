using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.IO;

namespace RealEstate.Web.Controllers
{
    public class BEpetworthController : Controller
    {
        // GET: BEpetworth
        bepetworthViewModel objViewModel = new bepetworthViewModel();
        dalbepetworth objDal = new dalbepetworth();
        public ActionResult Index()
        {
            if (TempData["Msg"] != null)
            {
                ViewBag.Message = "Thank you";
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Index(bepetworthViewModel Model)
        {
       
        
            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = objDal.Save(Model.bepetworth);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["Msg"] = "Success";
                    return RedirectToAction("Index");
                }

            }
            return View("Index", Model);
        }
    }
}