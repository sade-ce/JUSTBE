using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RealEstate.Entities.ViewModels;
using RealEstate.Services;
using RealEstate.Services.Schema;
using RealEstate.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RealEstate.Web.Controllers
{
    public class HomeController : Controller
    {
        dalNeighborhood objNeighborhood = new dalNeighborhood();
        utblNeighborhood objmodel = new utblNeighborhood();
        dalLead objdb = new dalLead();
        UserAuthenticateView ObjUser = new UserAuthenticateView();
        dalMstContact ObjDal = new dalMstContact();
        dalMstEnquire ObjEnquire = new dalMstEnquire();
        #region Membership Initialization
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion
        public class CaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success
            {
                get;
                set;
            }
            [JsonProperty("error-codes")]
            public List<string> ErrorMessage
            {
                get;
                set;
            }
        }
        public ActionResult Index(string returnUrl, string URL = "")
        {
            ViewBag.Current = "Index";

            var model = new utblPartialLead();
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
               // return RedirectToAction("Index", "ControlPanel", new { area = "Admin" });
            }
            if (User.IsInRole("Agent"))
            {
                return RedirectToAction("Index", "DashboardV2", new { area = "Coordinator" });
                //return RedirectToAction("List", "DealAgent", new { area = "Coordinator" });

            }

            if (!string.IsNullOrEmpty(URL))
            {
                return View();
            }

            if (User.IsInRole("Client"))
            {
                return RedirectToAction("List", "MyDeal", new { area = "Client" });
            }



            else
                return View();

        }
        [HttpPost]
        public ActionResult Index(utblPartialLead model, string Street, string City)
        {
            model.Address = Street;
            model.City = City;

            Session["City"] = City.ToString();

            if (string.IsNullOrEmpty(model.UnitNo))
            {
                model.UnitNo = "NA";

            }
            if (ModelState.IsValid)
            {
                long id = objdb.SavePartialLead(model);
                return Json(new { success = true, id = id, address = model.Address, zip = model.CityStateZip, unit = model.UnitNo, city = City.ToString() });
            }
            return View(model);
        }
        public ActionResult LoadLeadForm(long id, string address, string unit = "", string zip = "", string city = "")
        {
            utblFullLead fullmodel = new utblFullLead();
            fullmodel.VisitorID = id;
            // utblPartialLead model = objdb.GetLeadByID(id);
            fullmodel.Address = address;
            fullmodel.CityStateZip = zip;
            fullmodel.UnitNo = unit;
            fullmodel.City = city;
            return PartialView("pvFullLead", fullmodel);
        }


        public ActionResult SaveFullLead(utblFullLead model)
        {
            //System.Threading.Thread.Sleep(2000);
            if (ModelState.IsValid)
            {
                objdb.SaveFullLead(model);
                string url = Url.Action("HomeEstimate", "Home", new { txtAddress = model.Address, txtCity = model.CityStateZip, City = model.City, Unit = model.UnitNo, name = model.Name, email = model.EmailID, phone = model.Phone });
                return Json(new { success = true, url = url });
            }
            return PartialView("pvFullLead", model);
        }
        public async Task<ActionResult> HomeEstimate(string txtAddress, string txtCity, string City, string Unit, string name, string email, string phone)
        {
            ViewBag.Current = "Index";
            utblMstEnquire objenquire = new utblMstEnquire();
            MstHomeEstimationViewModel Objview = new MstHomeEstimationViewModel();
            ZillowModel objViewModel = new ZillowModel();
            string clientid = System.Configuration.ConfigurationManager.AppSettings["ZillowAPIDeveloperKey"];
            ZillowClient client = new ZillowClient(clientid);
            int flag = 0;

            int MailStatus = SendEmailConfirmationToken(txtAddress, txtCity, City, Unit, name, email, phone);

            if (Unit != "NA")
            {
                txtAddress = txtAddress + "" + Unit + "," + City.ToString();
            }
            else
            {
                txtAddress = txtAddress + "," + City.ToString();

            }
            for (int i = 1; i <= 3; i++)
            {
                try
                {
                    flag++;

                    objViewModel.results = await client.GetDeepSearchResultsAsync(txtAddress.Trim().ToString(), txtCity.Trim().ToString());

                    //string Message = SendSMS(txtAddress, name, email, phone);

                    foreach (SimpleProperty prop in objViewModel.results.response.results)
                    {
                        //objViewModel.zest = await client.GetZestimateAsync(prop.zpid.ToString());
                        objViewModel.chart = await client.GetChartAsync(prop.zpid.ToString(), "dollar", "600", "300");

                        //objViewModel.regionchart = client.GetRegionChart("", "", "", prop.address.zipcode, "dollar", "600", "300", SimpleChartDuration.Item1year, ChartVariant.detailed);
                        //objViewModel.comp = client.GetComps(prop.zpid.ToString(), "10");
                        if (objViewModel.results.message.code == "0")
                        {
                            TempData["ErrMsg"] = null;
                            Objview.ZillowModel = objViewModel;
                            objenquire.Name = name;
                            objenquire.Email = email;
                            objenquire.Phone = phone;
                            Objview.utblMstEnquire = objenquire;
                            return View(Objview);

                        }

                    }

                }
                catch (Exception ex)
                {
                    //utblFullLead model = new utblFullLead();
                    TempData["ErrMsg"] = ex.Message;
                    if (flag == 3)
                    {
                        return RedirectToAction("Success", "Home");

                    }

                }
            }

            TempData["ErrMsg"] = "Successfully processed";
            Objview.ZillowModel = objViewModel;
            objenquire.Name = name;
            objenquire.Email = email;
            objenquire.Phone = phone;
            Objview.utblMstEnquire = objenquire;
            return View(Objview);
        }

        [HttpPost]
        public ActionResult HomeEstimate(MstHomeEstimationViewModel ItemData, string txtmessage)
        {
            ViewBag.Current = "Index";

            ItemData.utblMstEnquire.Message = txtmessage;
            if (string.IsNullOrEmpty(ItemData.utblMstEnquire.Name) ||
                 string.IsNullOrEmpty(ItemData.utblMstEnquire.Email) ||
                 string.IsNullOrEmpty(ItemData.utblMstEnquire.Phone) ||
                  string.IsNullOrEmpty(ItemData.utblMstEnquire.Message)
                 )
            {
                return Json("* Please fill all fields", JsonRequestBehavior.AllowGet);

            }
            TempData["ErrMsg"] = ObjEnquire.EnquireInfo(ItemData.utblMstEnquire);
            return Json("Thanks for your submission, we will be in touch shortly ", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCharts(uint Zpid)
        {
            string clientid = System.Configuration.ConfigurationManager.AppSettings["ZillowAPIDeveloperKey"];
            ZillowClient client = new ZillowClient(clientid);
            chart c = client.GetChart(Zpid.ToString(), "dollar", "600", "300");
            ViewBag.Message = "Your application description page.";

            return PartialView("pvGetCharts", c);
        }
        public ActionResult About()
        {
            //objmodel.NeighborhoodView = objNeighborhood.MstLocationList;
            ViewBag.Current = "About";
            return View(objmodel);
        }

        public ActionResult Neighborhood()
        {
            ViewBag.Current = "Neighborhood";
            //objmodel.NeighborhoodView = objNeighborhood.MstLocationList;
            ViewBag.Message = "Your application description page.";

            return View(objmodel);
        }

        public ActionResult Contact()
        {
            ViewBag.MenuActive = "Contact";
            ViewBag.Message = "Your contact page.";
            ViewBag.Current = "Contact";

            return View();
        }

        public ActionResult ContactPage()
        {
            return PartialView("pvContactNew");
        }

        [HttpPost]
        public ActionResult ContactPage(utblMstContact ItemData)
        {
            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);

            if (response.Success)
            {
                if (string.IsNullOrEmpty(ItemData.ContactPerson) ||
               string.IsNullOrEmpty(ItemData.Email) 
             //  ||string.IsNullOrEmpty(ItemData.Message)
               )
                {
                    return Json("* Please fill all fields", JsonRequestBehavior.AllowGet);
                }

                else
                {
                    TempData["ErrMsg"] = ObjDal.ContactInfo(ItemData);
                    if (TempData["ErrMsg"].ToString() == "0")
                    {
                        int Status = SendEmail(ItemData.ContactPerson, ItemData.Email, ItemData.Message);
                        if (Status == 0)
                        {
                            return Json("Thanks for your submission, we will be in touch shortly.", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json("Please complete captcha challenge", JsonRequestBehavior.AllowGet);

        }
        private string ContactHTMLEmail(string CPN, string Email, string Message)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/AgentMessage.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{AgentName}", "Jay");
            body = body.Replace("{CPN}", CPN);
            body = body.Replace("{Email}", Email);
            body = body.Replace("{Phone}", "Not Provided");
            body = body.Replace("{Message}", Message);
            return body;
        }

        private int SendEmail(string CPN, string Email, string Message)
        {


            //string code = UserManager.GenerateEmailConfirmationToken(userID);
            //string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code, area = "" }, protocol: Request.Url.Scheme);


            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            //Create the SMTP Client
            SmtpClient client = new SmtpClient();
            client.Host = settings.Smtp.Network.Host;
            client.Port = settings.Smtp.Network.Port;
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = false;
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(settings.Smtp.Network.UserName, "Just BE");
            StringBuilder mailbody = new StringBuilder();
            mm.To.Add("jay@jaybauergroup.com");
            mm.Priority = MailPriority.High;
            mm.Subject = "Message from Contact Page";


            mm.Body = this.ContactHTMLEmail(CPN, Email, Message);
            mm.IsBodyHtml = true;





            try
            {
                client.Send(mm);
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }

        public ActionResult Enquiry()
        {
            utblMstEnquire Model = new utblMstEnquire();


            return PartialView("pvEnquiry", Model);
        }

        [HttpPost]
        public ActionResult Enquiry(utblMstEnquire Itemdata)
        {

            if (ModelState.IsValid)
            {
                TempData["ErrMsg"] = ObjEnquire.EnquireInfo(Itemdata);

                if (TempData["ErrMsg"].ToString() == "0")
                {
                    return Json(new { success = true });
                }
            }
            return PartialView("pvEnquiry", Itemdata);

        }


        public ActionResult LoggedInUser()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            ObjUser.ISClient = user.ISClient;
            return PartialView("pvLoggedInUser", ObjUser);
        }

        public ActionResult Welcome()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            ObjUser.Name = user.Name;
            return PartialView("pvWelcomeUser", ObjUser);



        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Congratulation()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult JayBauerListing()
        {
            ViewBag.Current = "JayBauerListing";
            return View();
        }


        public ActionResult CompassListing()
        {
            return View();
        }


        public string SendSMS(string Address, string Name, string Email, string Phone)
        {
            string Err = string.Empty;
            try
            {
                string[] Message = { "Lead Captured", Address, Name, Email, Phone, "Good Luck!" };
                string accountSid = "ACb6bfb9ce149ab64bdcb8888a9764b864";
                string authToken = "de9c8ed4080cf9e1c48ef4c6b6bd219c";
                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    to: new PhoneNumber("+13017851100"),
                    from: new PhoneNumber("+13019005931"),
                    body: string.Join("-", Message));
                Err = "success";

            }
            catch (Exception ex)
            {
                Err = ex.Message;
            }


            return Err;
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("ChangePassword", "Account");
        }


        public ActionResult ClosingCost()
        {
            return View();
        }

        public ActionResult Buyer()
        {
            return View();
        }
        public ActionResult Seller()
        {
            return View();
        }
        public ActionResult Jaybauergroupteam()
        {
            return View();
        }
        public ActionResult Greetings(string Message = "")
        {
            MstCheckLoggedClientDeal ObjModel = new MstCheckLoggedClientDeal();
            dalCheckUserDeal Obj = new dalCheckUserDeal();
            ObjModel.CheckDeal = Obj.CheckDeal(User.Identity.Name);

            ViewBag.message = Message;
            return View(ObjModel);
        }


        public ActionResult Maintainance()
        {
            return View();
        }

        public ActionResult DanaCloudListings()
        {
            return View();
        }
        public ActionResult ZackLondonListings()
        {
            return View();
        }
        public ActionResult ChristopherPlogListings()
        {
            return View();
        }



        #region LeadEmail
        private string PopulateBody(string txtAddress, string txtCity, string City, string Unit, string name, string email, string phone)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/Leademail.html")))
            {
                body = reader.ReadToEnd();
            }
            WebClient wclient = new WebClient();
            string URL = "http://justbere.com/Home/HomeEstimate?";
            var uriBuilder = new UriBuilder(URL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["txtAddress"] = txtAddress;
            query["txtCity"] = txtCity;
            query["City"] = City;
            query["Unit"] = Unit;
            query["name"] = name;
            query["email"] = email;
            //query["text_search"] = MLS;
            query["phone"] = phone;

            uriBuilder.Query = query.ToString();
            URL = uriBuilder.ToString();
            body = body.Replace("{FirstName}", name);
            body = body.Replace("{Url}", URL);
            return body;
        }

        private int SendEmailConfirmationToken(string txtAddress, string txtCity, string City, string Unit, string name, string email, string phone)
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            //Create the SMTP Client
            SmtpClient client = new SmtpClient();
            client.Host = settings.Smtp.Network.Host;
            client.Port = settings.Smtp.Network.Port;
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = false;
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(settings.Smtp.Network.UserName, "Just BE.");

            StringBuilder mailbody = new StringBuilder();

            mm.To.Add(email);
            mm.CC.Add("jay@jaybauergroup.com");
            mm.Priority = MailPriority.High;

            mm.Subject = "We received your consultation request";
            mm.Body = this.PopulateBody(txtAddress, txtCity, City, Unit, name, email, phone);
            mm.IsBodyHtml = true;

            try
            {
                client.Send(mm);
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }
        #endregion



        public ActionResult Brand()
        {
            return View();
        }
        public static CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["recaptchaPrivateKey"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }

        //public ActionResult Api() {

        //    List<string> list = new List<string>();
        //    HttpClient client = new HttpClient();
        //    var result = client.GetAsync("https://api.simplyrets.com/properties?limit=500&lastId=0").Result;
        //    if (result.IsSuccessStatusCode)
        //    {
        //        list = result.Content.ReadAsAsync<List<Employee>>().Result;
        //    }
        //    return View(list);

        //}


        //public ActionResult Api()
        //{
        //    var credentials = new NetworkCredential("simplyrets", "simplyrets");
        //    var client = new HttpClient(new HttpClientHandler() { Credentials = credentials })
        //    {
        //        BaseAddress = new Uri("https://api.simplyrets.com/")
        //    };
        //    //HttpClient client = new HttpClient();
        //    //client.BaseAddress = new Uri("https://api.simplyrets.com/properties?limit=500&lastId=0");
        //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    HttpResponseMessage response = client.GetAsync("properties?limit=500&lastId=0").Result;
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    var products = response.Content.ReadAsAsync<IEnumerable<BlogView>>().Result;
        //    //    foreach (var p in products)
        //    //    {
        //    //        Console.WriteLine("{0}\t{1};\t{2}", p.Name, p.Price, p.Category);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase); //Get 401 error here.
        //    //}

        //    return View("Index");
        //}



    }
 

}