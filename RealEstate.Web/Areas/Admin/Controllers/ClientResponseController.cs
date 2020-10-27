using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class ClientResponseController : Controller
    {
        // GET: Admin/ClientResponse
        MstClientRespondAdminManageModel ObjModel = new MstClientRespondAdminManageModel();
        dalAdminViewClientRespond ObjDal = new dalAdminViewClientRespond();
        public ActionResult List(string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            ObjModel.MstRespondList = ObjDal.ClientRespondDealGetPaged(PageNo, PageSize, out TotalRow, SearchTerm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClientRespond", ObjModel);
            }
            return View(ObjModel);
        }


        public ActionResult ViewDocuments(string TrackingID)
        {
            //ObjClient.MstClientRespondView = ObjClientDal.GetClientDealRespondDetails(TrackingID);
            ObjModel.DocList = ObjDal.GetClientLiveDealDocList(TrackingID);
            ViewBag.ActiveURL = "/Client/LiveDeal/Respond";

            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClientDocumentView", ObjModel);
            }
            return View(ObjModel);
        }

    }
}