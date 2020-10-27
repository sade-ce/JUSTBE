using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]

    public class DocumentsController : Controller
    {
        MstBuyerDocumentManageModel ObjModel = new MstBuyerDocumentManageModel();
        dalMstBuyerDocuments ObjDal = new dalMstBuyerDocuments();

        public ActionResult BuyerDocument(utblMstBuyerDocument ItemData)
        {

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjDal.Save(ItemData);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                    return RedirectToAction("BuyerDocumentList");
                }
            }
            ViewBag.ActiveURL = "/Admin/DocumentMaster/BuyerDocumentList";
            return View(ItemData);
        }

       
    }
}