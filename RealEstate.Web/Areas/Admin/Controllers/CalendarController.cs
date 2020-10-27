using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.DataAccess;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
using RealEstate.Entities.Models;
using System.Data.Entity;
using RealEstate.Entities.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.IO;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CalendarController : Controller
    {
        EFDBContext db = new EFDBContext();
        dalMstAppointment Objdal = new dalMstAppointment();
        MstCalenderManageModel ObjModel = new MstCalenderManageModel();

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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public string color()
        {
            var random = new Random();
            string color = String.Format("#{0:X6}", random.Next(0x1000000));
            return color;
        }
        
        public ActionResult CreateEvent(string Email)
        {
            ViewBag.ActiveURL = "/Admin/Client/List";
            ObjModel.utblMstAppointment = new utblMstAppointment();
            ObjModel.MstAppointmentList = Objdal.GetAdminCalenderList(Email).ToList();
            ObjModel.utblMstAppointment.Email = Email;
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvEventList", ObjModel);
            }
            return View(ObjModel);

        }
        [HttpPost]
        public ActionResult CreateEvent(MstCalenderManageModel Itemdata)
        {
            ViewBag.ActiveURL = "/Admin/Client/List";
            ObjModel.MstAdminClientNameSelect = new MstAdminClientNameSelect();
            ObjModel.utblMstAppointment = new utblMstAppointment();
            ObjModel.MstAppointmentList = Objdal.GetAdminCalenderList(Itemdata.utblMstAppointment.Email).ToList();
            ObjModel.utblMstAppointment.Email = Itemdata.utblMstAppointment.Email;
            if (ModelState.IsValid)
            {
                using (var db = new EFDBContext())
                {
                    try
                    {
                        db.utblMstAppointments.Add(Itemdata.utblMstAppointment);
                        db.SaveChanges();
                        if (Objdal.emailPreferences(Itemdata.utblMstAppointment.Email))
                        {
                            ObjModel.MstAdminClientNameSelect = Objdal.GetAdminClientName(User.Identity.Name, Itemdata.utblMstAppointment.Email);
                            string SDate = Itemdata.utblMstAppointment.StartDate.ToString("dd MMM yyyy HH:mm");
                            string EDate = Itemdata.utblMstAppointment.EndDate.ToString("dd MMM yyyy HH:mm");
                            string Date = string.Join(" to ", SDate, EDate);
                            int MailStatus = SendEmailConfirmationToken(Itemdata.utblMstAppointment.Email, ObjModel.MstAdminClientNameSelect.ClientName, Itemdata.utblMstAppointment.Description, ObjModel.MstAdminClientNameSelect.AgentName, User.Identity.Name, Date, ObjModel.MstAdminClientNameSelect.AgentPhone);
                            if (MailStatus == 0)
                            {
                                TempData["MailMsg"] = 1;
                            }
                            else
                            {
                                TempData["ErrMsg"] = null;
                            }

                        }

                        return RedirectToAction("CreateEvent", new { Email = Itemdata.utblMstAppointment.Email });
                    }
                    catch
                    {
                        return View(ObjModel);

                    }
                  
                }
            }

            return View(ObjModel);
        }
        
        #region Delete Event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, string Email)
        {
            ViewBag.ActiveURL = "/Admin/Client/List";

            var obj = new utblMstAppointment()
            {
                Id = Id

            };
            db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("CreateEvent", new { Email = Email });

        }
        #endregion


        private string PopulateBody(string Name, string EventName, string InviteeName, string InviteeEmail, string EventDate,string AgentPhone)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/Calendar.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Name}", Name);
            body = body.Replace("{EventName}", EventName);
            body = body.Replace("{AgentName}", InviteeName);
            //body = body.Replace("{InviteeEmail}", InviteeEmail);
            body = body.Replace("{EventDate}", EventDate);
            body = body.Replace("{AgentNumber}", AgentPhone);

            return body;
        }

        private int SendEmailConfirmationToken(string email, string Name, string EventName, string InviteeName, string InviteeEmail, string EventDate,string AgentPhone)
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
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = false;
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(settings.Smtp.Network.UserName, "Just BE.");

            StringBuilder mailbody = new StringBuilder();

            mm.To.Add(email);
            mm.Priority = MailPriority.High;

            mm.Subject = "New Event Scheduled";
            mm.Body = this.PopulateBody(Name, EventName, InviteeName, InviteeEmail, EventDate,AgentPhone);
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
    }
}

