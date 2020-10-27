using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using RealEstate.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers
{
    public class QuizController : Controller
    {
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
        dalMstContact ObjDal = new dalMstContact();
        TeamMemberViewModel objTeamModel = new TeamMemberViewModel();
        dalQuiz obj = new dalQuiz();
        dalUser objUser = new dalUser();
        // GET: Quiz
        public ActionResult Buyer()
        {


            ViewBag.Email = "";
            ViewBag.FirstName = "";
            ViewBag.LastName = "";
            if (User.IsInRole("Client") || User.IsInRole("Agent") || User.IsInRole("Admin"))
            {
                var user = UserManager.FindById(User.Identity.GetUserId());

                ViewBag.Email = user.UserName;
                ViewBag.FirstName = user.Name;
                ViewBag.LastName = user.LastName;

            }


            return View();
        }

        [HttpPost]
        public ActionResult Save(List<QuizView> listObject)
        {
            int i = 0;
            //operation return
            foreach (var c in listObject)
            {
               
                Quiz quiz = new Quiz();
                quiz.Question = c.Question == null ? "" : c.Question.ToString();
                quiz.Answer = c.Answer == null ? "" : c.Answer.ToString();
                quiz.Email = c.Email == null ? "" : c.Email.ToString();
                quiz.FirstName = c.FirstName == null ? "" : c.FirstName.ToString();
                quiz.LastName = c.LastName == null ? "" : c.LastName.ToString();
                quiz.Address = c.Address == null ? "" : c.Address.ToString();
                quiz.Unit = c.Unit == null ? "" : c.Unit.ToString();
                quiz.QuizType = "Buyer";
                quiz.QuizOrder = 1;
                quiz.AlterQuestion = c.AlterQuestion;
                obj.Save(quiz);
                listObject[i].QuizType = "Buyer";
                listObject[i].AlternateQuestion = obj.AlternateQuestion(c.AlterQuestion);
                i++;
            }
            List<QuizView> Quizz = listObject.Where(x => x.AlterQuestion != 0).ToList();
            string ClientEmail = Quizz.FirstOrDefault().Email;
            string ClientName = Quizz.FirstOrDefault().FirstName + " " + Quizz.FirstOrDefault().LastName;
            string QuizType = Quizz.FirstOrDefault().QuizType;
            int MailStatus = SendEmailConfirmationToken(ClientEmail, ClientName, QuizType, Quizz);
            return Json(new { listObject }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveSeller(List<QuizView> listObject)
        {
            //operation return
            int i = 0;
            foreach (var c in listObject)
            {
             
                Quiz quiz = new Quiz();
                quiz.Question = c.Question == null ? "" : c.Question.ToString();
                quiz.Answer = c.Answer == null ? "" : c.Answer.ToString();
                quiz.Email = c.Email == null ? "" : c.Email.ToString();
                quiz.FirstName = c.FirstName == null ? "" : c.FirstName.ToString();
                quiz.LastName = c.LastName == null ? "" : c.LastName.ToString();
                quiz.Address = c.Address == null ? "" : c.Address.ToString();
                quiz.Unit = c.Unit == null ? "" : c.Unit.ToString();
                quiz.QuizType = "Seller";
                quiz.QuizOrder = 1;
                quiz.AlterQuestion = c.AlterQuestion;
                obj.Save(quiz);
                listObject[i].QuizType = "Seller";
                listObject[i].AlternateQuestion = obj.AlternateQuestion(c.AlterQuestion);
                i++;
            }
            List<QuizView> Quizz = listObject.Where(x => x.AlterQuestion!=0).ToList();
            string ClientEmail = Quizz.FirstOrDefault().Email;
            string ClientName = Quizz.FirstOrDefault().FirstName + " " + Quizz.FirstOrDefault().LastName;
            string QuizType = Quizz.FirstOrDefault().QuizType;
            int MailStatus = SendEmailConfirmationToken(ClientEmail, ClientName, QuizType, Quizz);

            return Json(new { listObject }, JsonRequestBehavior.AllowGet);
        }

        private int SendEmailConfirmationToken(string ClientEmail, string ClientName, string QuizType, IEnumerable<QuizView> Quizz)
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

            mm.To.Add("jay@justbere.com");
            mm.CC.Add("neha@justbere.com");
            mm.Priority = MailPriority.High;
            mm.Subject = "Quiz";


            mm.Body = this.PopulateBody(ClientEmail, ClientName, QuizType, Quizz);
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

        private string PopulateBody(string ClientEmail, string ClientName,string QuizType, IEnumerable<QuizView> Quizz)
        {
            string body = string.Empty;
         
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/Quiz.html")))
            {
                body = reader.ReadToEnd();
            }
      
            string content = "";

            foreach (var item in Quizz)
            {
                content = content + item.AlternateQuestion + " : "+ item.Answer + "<br/>";


            }
            body = body.Replace("{Status}", content);
            body = body.Replace("{ClientEmail}", ClientEmail);
            body = body.Replace("{ClientName}", ClientName);
            body = body.Replace("{QuizType}", QuizType);
            

            return body;
        }

        public ActionResult Seller()
        {


            ViewBag.Email = "";
            ViewBag.FirstName = "";
            ViewBag.LastName = "";
            if (User.IsInRole("Client")|| User.IsInRole("Agent")||User.IsInRole("Admin"))
            {

                var user = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.Email = user.UserName;
                ViewBag.FirstName = user.Name;
                ViewBag.LastName = user.LastName;

            }


            return View();
        }

        public ActionResult Finish(string FirstName, string LastName, string Email)
        {
            var user = UserManager.FindByEmail(Email);
            if (user == null)
            {
                RegisterViewModel Model = new RegisterViewModel();
                Model.Email = Email;
                Model.Name = FirstName;
                Model.LastName = LastName;
                return RedirectToAction("Register", "Account", Model);
            }
            else
            {
                return RedirectToAction("Brand", "Home");
            }

        }
    }
}
