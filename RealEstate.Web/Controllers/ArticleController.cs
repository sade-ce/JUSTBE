using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.DataAccess;

namespace RealEstate.Web.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        dalMstEnquire ObjDal = new dalMstEnquire();
        public ActionResult BuyerGuides()
        {
            ViewBag.Current = "BuyerGuides";
            return View();
        }

        public ActionResult SellerGuides()
        {
            ViewBag.Current = "BuyerGuides";
            return View();
        }

        public ActionResult BuyerTips()
        {
            ViewBag.Current = "BuyerGuides";
            return View();
        }


        public ActionResult Enquiry()
        {
            return PartialView("pvEnquiry");
        }

        [HttpPost]
        public ActionResult Enquiry(utblMstArticleEnquire ItemData)
        {
            if (string.IsNullOrEmpty(ItemData.Name) ||
                string.IsNullOrEmpty(ItemData.Email) ||              
                string.IsNullOrEmpty(ItemData.Message)
                )
            {
                return Json("* Please fill all fields", JsonRequestBehavior.AllowGet);

            }
            TempData["ErrMsg"] = ObjDal.ArticleEnquire(ItemData);
            return Json("Thanks for your submission, we will be in touch shortly ", JsonRequestBehavior.AllowGet);

        }

        public ActionResult BuyingaHomeFall2017()
        {
            return View();
        }

        public ActionResult SellingYourHouseFall2017()
        {
            return View();
        }
    }
}