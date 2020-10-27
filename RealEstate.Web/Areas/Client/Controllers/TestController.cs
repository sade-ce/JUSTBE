using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Yelp.Api.Models;

namespace RealEstate.Web.Areas.Client.Controllers
{
    [Authorize(Roles = "Client,Agent,Admin")]

    public class TestController : Controller
    {
        // GET: Client/Test
        public async Task<ActionResult> Index()
        {
            SearchResponse SR = new SearchResponse();
            var client = new Yelp.Api.Client("JcybcL65orLyIIV-gv6dJg", "z47dDQNd8ljNsLgChVzeDWmqQe0T7KxFqc3IKLLYqzUQllewbi3TFRDO1Os3B1oV");
            SR = await client.SearchBusinessesAllAsync("Plumber", 37.786882, -122.399972);
            return View(SR);
        }

        public ActionResult UI()
        {
            return View();
        }
    }
}