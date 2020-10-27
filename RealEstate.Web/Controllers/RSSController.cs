using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace RealEstate.Web.Controllers
{
    public class RSSController : Controller
    {
        // GET: RSS
        public ActionResult Index()
        {
            ParseRssFile();
            return View();
        }
        private string ParseRssFile()
        {
            XmlDocument rssXmlDoc = new XmlDocument();

            // Load the RSS file from the RSS URL
            rssXmlDoc.Load("https://news.google.com/news/section?output=rss&q=adams%20morgan%20real%20estate%20news");

            // Parse the Items in the RSS file
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            StringBuilder rssContent = new StringBuilder();

            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("link");
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("description");
                string description = rssSubNode != null ? rssSubNode.InnerText : "";
                rssContent.Append("<a href='" + link + "'>" + title + "</a><br>" + description);
            }

            // Return the string that contain the RSS items
            return rssContent.ToString();
        }


        string RemoveHtmlTags(string html)
        {
            return Regex.Replace(html, "<.+?>", string.Empty);
        }
        public ActionResult Feed()
        {
            WebClient wclient = new WebClient();
            string RSSURL = "https://news.google.com/news/section?output=rss&q=adamsmorgan%20real%20estate%20news";
            string RSSData = wclient.DownloadString(RSSURL);

            XDocument xml = XDocument.Parse(RSSData);
            var RSSFeedData = (from x in xml.Descendants("item")
                               select new RssFeed
                               {
                                   Title = ((string)x.Element("title")),
                                   Link = ((string)x.Element("link")),

                                   Description = RemoveHtmlTags(((string)x.Element("description"))),
                                   PubDate = ((string)x.Element("pubDate"))
                               });
            ViewBag.RSSFeed = RSSFeedData;
            ViewBag.URL = RSSURL;
            return View();
        }




        public ActionResult Testing()
        {
         int val=   BE(95);
            return Content(val.ToString());
        }

        int BE(int n)
        {
            if (n > 100)
                return n - 10;
            else
                return (BE(BE(n + 11)));
        }

    }
}
