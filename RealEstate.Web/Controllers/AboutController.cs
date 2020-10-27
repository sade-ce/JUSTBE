using Newtonsoft.Json;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        dalMstContact ObjDal = new dalMstContact();
        TeamMemberViewModel objTeamModel = new TeamMemberViewModel();
        dalTeamMember objTeamDal = new dalTeamMember();
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
        public ActionResult BE()
        {
            ViewBag.MenuActive = "About";
            objTeamModel.TeamMemberList = objTeamDal.GetTeamMembers();
            return View(objTeamModel);
        }
        public ActionResult BE1()
        {
            ViewBag.MenuActive = "About";
            objTeamModel.TeamMemberList = objTeamDal.GetTeamMembers();
            return View(objTeamModel);
        }
        public ActionResult BE2()
        {
            ViewBag.MenuActive = "About";
            objTeamModel.TeamMemberList = objTeamDal.GetTeamMembers();
            return View(objTeamModel);
        }
        public ActionResult TeamMember(string Name)
        {
            ViewBag.MenuActive = "About";
            objTeamModel.TeamMemberView = objTeamDal.GetTeamMemberByName(Name);
            return View(objTeamModel);
        }
        public ActionResult DanaCloud()
        {
            return View();
        }
        public ActionResult JayBauer()
        {
            return View();
        }
        public ActionResult ZackLondon()
        {
            return View();
        }
        public ActionResult ChristopherPlog()
        {
            return View();
        }
        public ActionResult CiaraLee()
        {
            return View();
        }
        public ActionResult TanniRednor()
        {
            return View();
        }
        public ActionResult CalebEtoile()
        {
            return View();
        }

        public ActionResult UserAgreement()
        {
            return View();
        }

        public ActionResult MeetTheTeam()
        {
            return View();
        }
        public ActionResult Julia()
        {

            return View();
        }
        public ActionResult Meredith()
        {

            return View();
        }


        public ActionResult AgentContact(string Agent)
        {
            ViewBag.AgentName = Agent;
            return PartialView("pvAgentContact");
        }

        [HttpPost]
        public ActionResult AgentContact(utblMstAgentContacts ItemData, string Email,string Name)
        {
            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            if (response.Success)
            {
                string AgentName = Name;
                string AgentEmail = Email;

                if (string.IsNullOrEmpty(ItemData.ContactPerson) ||
                    string.IsNullOrEmpty(ItemData.Email) ||
                    string.IsNullOrEmpty(ItemData.Message)
                    )
                {
                    return Json("* Please fill all fields", JsonRequestBehavior.AllowGet);

                }
                int Status = SendEmail(AgentName, AgentEmail, ItemData.ContactPerson, ItemData.Email, ItemData.Phone, ItemData.Subject, ItemData.Message);
                //TempData["ErrMsg"] = ObjDal.AgentContactInfo(ItemData);
                return Json("Thanks for your submission, we will be in touch shortly ", JsonRequestBehavior.AllowGet);
            }
            return Json("Please complete captcha challenge", JsonRequestBehavior.AllowGet);
        }

        private string PopulateBody(string AgentName,string AgentEmail, string CPN, string Email,string Phone,string Message)
        {


            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/AgentContact.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{AgentName}", AgentName);
            body = body.Replace("{CPN}", CPN);
            body = body.Replace("{Email}", Email);
            body = body.Replace("{Phone}", Phone);
            body = body.Replace("{Message}", Message);
            return body;
        }

        private int SendEmail(string AgentName,string AgentEmail, string CPN, string Email, string Phone, string Subject, string Message)
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
            mm.From = new MailAddress(settings.Smtp.Network.UserName, "Just BE.");

            StringBuilder mailbody = new StringBuilder();

            mm.To.Add(AgentEmail);
            mm.CC.Add("support@justbere.com");
            mm.Priority = MailPriority.High;

            mm.Subject = Subject;
            mm.Body = this.PopulateBody(AgentName, AgentEmail, CPN, Email, Phone,Message);
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
        public static CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["recaptchaPrivateKey"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }
    }
}