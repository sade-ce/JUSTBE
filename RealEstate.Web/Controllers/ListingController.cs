using HtmlAgilityPack;
using Newtonsoft.Json;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers
{
    public class ListingController : Controller
    {
        // GET: Test

        public ActionResult Details(string mls_id)
        {
            string MLSURL = "https://www.compass.com/listing/" + mls_id + "/view";
            var request = WebRequest.Create(MLSURL);
            var response = request.GetResponse();
            string stringResponse = "";
            var content = "";
            HtmlDocument document = new HtmlDocument();

            try
            {

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);
                    stringResponse = reader.ReadToEnd();
                }
                document.LoadHtml(stringResponse);
                var nodes = document.DocumentNode.SelectNodes("//*[contains(@class,'uc-corpNav-menuItem')]");

                foreach (var node in nodes)
                {
                    node.Remove();
                }

                var Lead = document.DocumentNode.SelectNodes("//*[contains(@class,'consumerFooter-wrapper')]");
                foreach (var node in Lead)
                {
                    node.Remove();
                }


                var res = document.DocumentNode.SelectSingleNode("//html");

                content = res.InnerHtml;

            }
            catch (Exception ex)
            {
                ViewBag.HTML = ex.Message;
            }

            List<string> Tag = new List<string>();
            Tag.Add("footer");
            Tag.Add("uc-global-header");
            Tag.Add("consumer-menu");
            Tag.Add("logo");
            Tag.Add("svg");
            Tag.Add("uc-listing-transit");

            //HtmlNode node = document.DocumentNode.SelectSingleNode("//uc-global-header[@class='uc-globalHeader u-print--displayNone uc-globalHeader--jumbo']");
            //var div = document.DocumentNode.SelectSingleNode("//li[@class='uc-globalHeader-navListItem']");

            //foreach (HtmlNode node in div.SelectNodes("*"))
            //{
            //    node.Remove();
            //}
            //var innerText = div.InnerText.Trim();
            ViewBag.HTML = Helper.RemoveUnwantedHtmlTags(content, Tag);



            return View();
        }
       

        public ActionResult BackUp(string mls_id)
        {
            string MLSURL = "http://www.mandyanddavid.com/idx/search.html?";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            uriBuilder.Port = -1;

            query["refine"] = "true";
            query["search_location"] = mls_id;

            uriBuilder.Query = query.ToString();
            MLSURL = uriBuilder.ToString();

            var request = WebRequest.Create(MLSURL);

            var response = request.GetResponse();
            string stringResponse = "";
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                stringResponse = reader.ReadToEnd();
            }


            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(stringResponse);

            var Address = document.DocumentNode.SelectSingleNode("//div[@class='idx-default']/h4").InnerText;

            var div = document.DocumentNode.SelectSingleNode("//div[contains(@class,'idx-listing-image')]");
            var IMGURL = Regex.Match(div.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value;

            ViewBag.Address = Address;
            ViewBag.Image = IMGURL;
            //var res = document.DocumentNode.SelectSingleNode("//html");
            //var content = res.InnerHtml;

            var content = "";
            try
            {
                var value = document.DocumentNode.SelectSingleNode("//div[@class='idx-listings-area']/a").Attributes["href"].Value;



                var request1 = WebRequest.Create(value);

                var response1 = request1.GetResponse();
                string stringResponse1 = "";
                using (Stream responseStream = response1.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);
                    stringResponse1 = reader.ReadToEnd();
                }


                HtmlDocument document1 = new HtmlDocument();
                document1.LoadHtml(stringResponse1);
                HtmlNode node = document1.DocumentNode.SelectSingleNode("//div[@class='fw fw-header uk-position-relative uk-position-z-index']");
                node.ParentNode.RemoveChild(node);

                var res = document1.DocumentNode.SelectSingleNode("//html");
                content = res.InnerHtml;

            }
            catch (Exception ex)
            {
                ViewBag.HTML = ex.Message;
            }

            List<string> Tag = new List<string>();
            Tag.Add("header");

            ViewBag.HTML = Helper.RemoveUnwantedHtmlTags(content, Tag);
            return View();
        }

        public async Task<ActionResult> autocomplete(string term)
        {
            MstCompassAutocompleteView Obj = new MstCompassAutocompleteView();
            Obj = await GetMLSInfo(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in Obj.addresses_info)
            {
                result.Add(new KeyValuePair<string, string>(item.address + " " + "-" + " " + "|" + " " + item.bedrooms + " " + "Bed" + " " + "|" + " " + item.bathrooms + " " + "Bath" + " " + "|" + " " + "$" + " " + item.price, item.listingIdSHA));

            }



            return Json(result, JsonRequestBehavior.AllowGet);


        }
        public ActionResult Compass(string mls_id)
        {
            MstCompassAutocompleteView Obj = new MstCompassAutocompleteView();

            string MLSURL = "https://www.compass.com/listing/400-massachusetts-avenue-northwest-unit-709-washington-dc-20001/f872f63347f9225c6fb803d2029f9eb051fe1df8/";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            uriBuilder.Port = -1;

            query["refine"] = "true";
            query["search_location"] = mls_id;

            uriBuilder.Query = query.ToString();
            //MLSURL = uriBuilder.ToString();

            var request = WebRequest.Create(MLSURL);

            var response = request.GetResponse();
            string stringResponse = "";
            var img = "";
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                stringResponse = reader.ReadToEnd();
            }


            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(stringResponse);
            var metaTags = document.DocumentNode.SelectNodes("//meta");

            if (metaTags != null)
            {
                foreach (var tag in metaTags)
                {

                    var tagContent = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];
                    if (tagProperty != null && tagContent != null)
                    {
                        if (tagProperty.Value.ToString() == "og:image")
                        {
                            img = tagContent.Value;

                        }
                    }
                }
            }
            var content = "";
            try
            {
                var res = document.DocumentNode.SelectSingleNode("//html");
                content = res.InnerHtml;


            }
            catch (Exception ex)
            {
                ViewBag.HTML = ex.Message;
            }


            ViewBag.HTML = content;
            return View();
        }
        public static async Task<MstCompassAutocompleteView> GetMLSInfo(string MLS)
        {
            MstCompassAutocompleteView ObjviewModel = new MstCompassAutocompleteView();
            WebClient wclient = new WebClient();
            //string MLSURL = "https://www.compass.com/api/search_suggest/homepage?";
            string MLSURL = "https://www.compass.com/api/v3/search/suggest/homepage?";
            var uriBuilder = new UriBuilder(MLSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["q"] = MLS;
            query["type"] = "listings";
            query["num"] = "25";
            query["geography"] = "dc";

            uriBuilder.Query = query.ToString();
            MLSURL = uriBuilder.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(MLSURL);
                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<MstCompassAutocompleteView>(result);
                ObjviewModel = list;
            }
            return ObjviewModel;
        }
    }

    public static class Helper
    {
        public static string RemoveUnwantedHtmlTags(this string html, List<string> unwantedTags)
        {
            if (String.IsNullOrEmpty(html))
            {
                return html;
            }

            var document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNodeCollection tryGetNodes = document.DocumentNode.SelectNodes("./*|./text()");

            if (tryGetNodes == null || !tryGetNodes.Any())
            {
                return html;
            }

            var nodes = new Queue<HtmlNode>(tryGetNodes);

            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode;

                var childNodes = node.SelectNodes("./*|./text()");

                if (childNodes != null)
                {
                    foreach (var child in childNodes)
                    {
                        nodes.Enqueue(child);
                    }
                }

                if (unwantedTags.Any(tag => tag == node.Name))
                {
                    if (childNodes != null)
                    {
                        foreach (var child in childNodes)
                        {
                            parentNode.InsertBefore(child, node);
                        }
                    }

                    parentNode.RemoveChild(node);

                }
            }

            return document.DocumentNode.InnerHtml;
        }

    }



}