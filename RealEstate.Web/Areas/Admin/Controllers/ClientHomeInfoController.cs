using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.Models;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System.Threading.Tasks;
using RealEstate.Web.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    public class ClientHomeInfoController : Controller
    {
        dalMstClientMLSHomeListing Objdal = new dalMstClientMLSHomeListing();
        MstClientMLSHomeListing ObjModel = new MstClientMLSHomeListing();
        RootObject ObjviewModel = new RootObject();

        public ActionResult Create(string Email)
        {
            ObjModel.ListingDDL = Objdal.ListingTypeDDL();
            ObjModel.MstInfoView = Objdal.MstClientInfoView(Email);
            ObjModel.MstClientHomeList = Objdal.GetClientHomeList(Email);
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvClientHomeList", ObjModel);
            }
            return View(ObjModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MstClientMLSHomeListing ItemData, string Email)
        {
            string Address, Locality, region, postal;
            ItemData.utblMstClientHomeListing.Email = Email;
            if (ModelState.IsValid)
            {
                if (ItemData.utblMstClientHomeListing.ListingTypeID == 1)
                {
                    string MLSID = ItemData.utblMstClientHomeListing.MLSID;
                    ObjviewModel = await GetMLSInfo(MLSID);
                    if(ObjviewModel.organic_results.search_results.Count()>0)
                    {
                    Address = ObjviewModel.organic_results.search_results[0].location.address;
                    Locality = ObjviewModel.organic_results.search_results[0].location.locality;
                    region = ObjviewModel.organic_results.search_results[0].location.region;
                    postal = ObjviewModel.organic_results.search_results[0].location.postal;
                    string HomeAddress = string.Join(" ", Address, Locality, region, postal);
                    ItemData.utblMstClientHomeListing.Address = HomeAddress;
                    }
                    else
                    { 
                    TempData["ErrMsg"] = "....Please Enter Valid MLS ID...";
                    return RedirectToAction("Create", new { Email = ItemData.utblMstClientHomeListing.Email });
                    }
                }

                TempData["ErrMsg"] = Objdal.Save(ItemData.utblMstClientHomeListing);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("Create", new { Email = ItemData.utblMstClientHomeListing.Email });
                }
            }
            ObjModel.ListingDDL = Objdal.ListingTypeDDL();
            ObjModel.MstInfoView = Objdal.MstClientInfoView(Email);
            ObjModel.MstClientHomeList = Objdal.GetClientHomeList(Email);
            return View(ObjModel);
        }

        public static async Task<RootObject> GetMLSInfo(string MLS)
        {
            RootObject ObjviewModel = new RootObject();
            WebClient wclient = new WebClient();
            string MLSURL = "https://queryservicec.placester.net/search?";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["text_search"] = MLS;
            query["region_id"] = "va_dc_md";

            uriBuilder.Query = query.ToString();
            MLSURL = uriBuilder.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(MLSURL);
                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<RootObject>(result);
                ObjviewModel = list;
            }
            return ObjviewModel;
        }
        #region Delete Home
        [HttpPost]
        public ActionResult Delete(int MLSListingID)
        {
            TempData["ErrMsg"] = Objdal.Delete(MLSListingID);
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("List");
        }
        #endregion

    }
}