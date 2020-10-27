using Newtonsoft.Json;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers
{
    public class MLSController : Controller
    {
        // GET: MLS
        dalMstEnquire ObjDal = new dalMstEnquire();

        public async Task<ActionResult> Index(string searchTerm="")
        {
            RootObject ObjviewModel = new RootObject();

            //ObjviewModel = await GetMLSInfo(searchTerm);

            if(Request.IsAjaxRequest())
            {
                return PartialView("pvMLS", ObjviewModel);
            }
            //return View(ObjviewModel);
            return View();

        }

        public async Task<ActionResult> Listing(string MLS)
        {
            RootObject ObjviewModel = new RootObject();
            WebClient wclient = new WebClient();
            string MLSURL = "https://queryservicec.placester.net/search?";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["mls_id"] = MLS;
            //query["text_search"] = MLS;
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
                ObjviewModel= list;
            }
            return View(ObjviewModel);
        }

        public static async Task<RootObject> GetMLSInfo(string URL)
        {
            RootObject ObjviewModel = new RootObject();
            WebClient wclient = new WebClient();
            string MLSURL = "https://queryservicec.placester.net/search?";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["mls_id"] = URL;
            //query["text_search"] = MLS;
            query["region_id"] = "va_dc_md";

            uriBuilder.Query = query.ToString();
            MLSURL = uriBuilder.ToString();

            using (var client = new HttpClient())
            {
                var url = string.Format("https://queryservicec.placester.net/search?sort_field=price&sort_direction=desc&search_num_results=9&search_start_offset=0&region_id=va_dc_md&origin_ids=4f7a2922d23a5474f800007a");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<RootObject>(result);
                /*  var js = new DataContractJsonSerializer(typeof(Company));
                  var ms = new MemoryStream(Encoding.ASCII.GetBytes(result));
                  var list = (Company)js.ReadObject(ms);*/
                return list;
            }
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
    }
}