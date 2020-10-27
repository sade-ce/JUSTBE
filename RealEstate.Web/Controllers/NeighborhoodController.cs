using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using RealEstate.Entities.DataAccess;

namespace RealEstate.Web.Controllers
{

    public static class StringHelpers
    {
        public static string StripHTML(this string HTMLText)
        {
            var reg = new Regex("<[^>]+>|&nbsp;|â€œ|â€|â€¦|&raquo;|&quot;|â€”;|â€”|â€”", RegexOptions.IgnoreCase);
            return reg.Replace(HTMLText, "");
            //return Regex.Replace(HTMLText, @"<(.|\n)*?>|&nbsp;|&raquo;|&quot;|â€”;|â€”|â€”", String.Empty);

        }
    }

    
    public class NeighborhoodController : Controller
    {

        MstNeighborhoodDetails ObjModel = new MstNeighborhoodDetails();
        dalClientNeighborhood ObjDal = new dalClientNeighborhood();

        string RemoveHtmlTags(string html)
        {
            return Regex.Replace(html, "<.+?>", string.Empty);
        }
        public ActionResult News( string City)
        {
            WebClient wclient = new WebClient();
            string RSSURL = "https://news.google.com/news/rss/search/section/q/";

            string validurl = RSSURL + City;
            var uriBuilder = new UriBuilder(RSSURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["q"] = City;          
                  
            uriBuilder.Query = query.ToString();
            RSSURL = validurl.ToString();
            string RSSData = wclient.DownloadString(RSSURL);

            XDocument xml = XDocument.Parse(RSSData);
            var RSSFeedData = (from x in xml.Descendants("item")
                               select new RssFeed
                               {
                                   Title = ((string)x.Element("title")),
                                   Link = ((string)x.Element("link")),
                                   //Description = ((string)x.Element("description")).StripHTML(),

                                   Description = WebUtility.HtmlDecode(((string)x.Element("description")).StripHTML()),
                                   PubDate = ((string)x.Element("pubDate"))
                               });
            ViewBag.RSSFeed = RSSFeedData;
            ViewBag.City = City;
            ViewBag.URL = RSSURL;
            ObjModel.MstNeighborhood = ObjDal.GetNeighborhood(City);
            return View(ObjModel);
        }


        public ActionResult Community()
        {
            ViewBag.MenuActive = "Neighborhood";
            return View();
        }

        public ActionResult Data()
        {
            return View();
        }
    }
}