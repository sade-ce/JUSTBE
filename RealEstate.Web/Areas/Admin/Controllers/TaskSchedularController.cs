using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{

    public class TaskSchedularController : Controller
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
        dalTaskSchedular ObjDal = new dalTaskSchedular();
        MstTaskSchedular ObjMoodel = new MstTaskSchedular();
        public async Task<ActionResult> SendEmail()
        {
            ObjMoodel.GetTask = new List<MstSelectTask>();
            ObjMoodel.GetTask = ObjDal.GetClientEmail();
            if (ObjMoodel.GetTask.Count() > 0)
            {

                foreach (var item in ObjMoodel.GetTask)
                {

                    try
                    {
                        var Message = ClientEmailBody(item.StartDate, item.EndDate, item.Description, item.IsContingency,item.ClientName, item.Email, item.AgentName, item.AgentEmail, item.AgentPhone, item.AgentPhoto,item.IsClient);

                        await SendEmalToClientAndAgentAsync(item.Email, item.AgentEmail,Message);
                        ObjDal.Update(item.Id);
                        //if (item.IsClient)//added by sonika
                        //{
                        //    ObjDal.UpdateNewTable(item.Id, item.StartDate, item.EndDate);
                        //}
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                    }

                }

            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }



        #region ClientEmailBody

        public string ClientEmailBody(DateTime StartDate, DateTime EndDate, string Description, bool IsContingency, string ClientName,string ClientEmail, string AgentName, string AgentEmail, string AgentPhone, string AgentPhoto,bool IsClient)
        {

            try
            {
                string body = "";
                if (IsClient)
                     body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/HtmlEmail/EventReminderClient.html"));
                else
                     body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/HtmlEmail/EventReminder.html"));

                string Date ="";
                if(IsContingency == true)
                {
                    Date = EndDate.DayOfWeek.ToString() + ',' + ' ' + EndDate.ToString("MMMM dd, yyyy 06:00") + ' ' + "PM";
                    body = body.Replace("{StartTime}", Date);
                }
                else
                {
                    Date = StartDate.DayOfWeek.ToString() + ',' + ' ' + StartDate.ToString("MMMM dd, yyyy hh:mm tt") + ' ' + "To"+' ' + EndDate.DayOfWeek.ToString() + ',' + ' ' + EndDate.ToString("MMMM dd, yyyy hh:mm tt");
                    body = body.Replace("{StartTime}", Date);
                }
                if (IsClient)
                {
                    body = body.Replace("{EndTime}", EndDate.ToString("hh:mm tt"));
                    body = body.Replace("{EventDescription}", Description);
                    body = body.Replace("{ClientName}", ClientName);
                    body = body.Replace("{Month}", EndDate.ToString("MMM"));
                    body = body.Replace("{Dy}", EndDate.ToString("dd"));
                    body = body.Replace("{Year}", EndDate.ToString("yyyy"));
                }
                  
                else
                {
                    body = body.Replace("{EndTime}", EndDate.ToString("hh:mm tt"));
                    body = body.Replace("{EventDescription}", Description);
                    body = body.Replace("{ClientName}", ClientName);
                    body = body.Replace("{AgentName}", AgentName);
                    body = body.Replace("{AgentEmail}", AgentEmail);
                    body = body.Replace("{AgentPhone}", AgentPhone);
                    body = body.Replace("{AgentPhoto}", AgentPhoto);
                    body = body.Replace("{Month}", EndDate.ToString("MMM"));
                    body = body.Replace("{Dy}", EndDate.ToString("dd"));
                    body = body.Replace("{Year}", EndDate.ToString("yyyy"));

                }

                return body.ToString();


            }

            catch (Exception ex)
            {
                throw ex;
            }



        }



        #endregion


        #region AgentEmailBody

        //private string AgentEmailBody(DateTime EndDate, string Description, string Email, string AgentName, string AgentEmail, string AgentPhone, string AgentPhoto)
        //{


        //    string body = string.Empty;
        //    using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/Email.html")))
        //    {
        //        body = reader.ReadToEnd();
        //    }
        //    body = body.Replace("{FirstName}", Name);
        //    body = body.Replace("{UserName}", userName);
        //    body = body.Replace("{Password}", password);
        //    return body;
        //}

        #endregion

        #region SendEmailCode
        public async static Task SendEmalToClientAndAgentAsync(string ClientEmail,string AgentEmail, string Message)
        {
            try
            {
                var _email = "no-reply@justbere.com";
                var _epass = "JustBEre";
                var _dispname = "Just BE";
                MailMessage MyMessage = new MailMessage();
                MyMessage.To.Add(ClientEmail);
                MyMessage.CC.Add(AgentEmail);
                MyMessage.From=new MailAddress(_email, _dispname);
                MyMessage.Subject = "Notification @ Just BE | Real Estate";
                MyMessage.Body = Message;
                MyMessage.IsBodyHtml = true;

                using (SmtpClient Client = new SmtpClient())
                {




                    Client.EnableSsl = false;

                    Client.Host = "relay-hosting.secureserver.net";
                    Client.Port = 25;
                    Client.UseDefaultCredentials = false;
                    Client.Credentials = new NetworkCredential(_email, _epass);
                    Client.Timeout = 300000;
                    Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    Client.SendCompleted += (s, e) => { Client.Dispose(); };
                    await Client.SendMailAsync(MyMessage);
                }
            }

            catch(Exception ex)
            {
                throw ex;

            }
           
            
        }

        #endregion
    }
}