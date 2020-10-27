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
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;
using System.Text;
using Microsoft.AspNet.Identity;
using GoogleApiUtils;
using GoogleApiUtils.GoogleCalendarApi;
using Google.Apis.Calendar.v3.Data;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]

    public class CalendlyController : Controller
    {
        EFDBContext db = new EFDBContext();
        dalMstAppointment Objdal = new dalMstAppointment();
        MstCalenderManageModel ObjModel = new MstCalenderManageModel();
        dalTrackDeal ObjStatus = new dalTrackDeal();//added by sonika
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



        //public ActionResult ListEvent()
        //{
        //    ViewBag.ActiveURL = "/Coordinator/Agent/Index";
        //    ObjModel.MstAppointmentList = Objdal.GetCalenderList(User.Identity.Name).ToList();
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("pvEventList", ObjModel);
        //    }
        //    return View(ObjModel);
        //}
        public ActionResult AllSchedule(int PageNo = 1, int PageSize = 10, string SearchTerm = "")
        {

            var Agent = UserManager.FindByEmail(User.Identity.Name);

            int TotalRow;
            ObjModel.AppointmentViewModelList = Objdal.GetAgentCalenderList(PageNo, PageSize, out TotalRow, Agent.Id, SearchTerm);
            ObjModel.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvRawEventList", ObjModel);
            }
            return View(ObjModel);
        }

        public ActionResult CreateEvent(string Email, string ClientID, string TransactionID, string AgentID = "", string ErrMsg = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ViewBag.AgentID = AgentID;
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            if (!string.IsNullOrEmpty(ErrMsg))
            {
                if (ErrMsg == "Unauthorized")
                {

                    TempData["ErrMsg"] = "....cannot register event on google calendar,because there is no calendar avaiable for this user....";
                }
                else if (ErrMsg == "NotConfigured")
                {
                    TempData["ErrMsg"] = "....looks like you havent configured your google account yet, or you've  changed your password recently, to configure your google account please click Sync google Calendar from sidebar menu.....";


                }
                else
                {
                    TempData["ErrMsg"] = null;
                }
            }

            if (User.IsInRole("Admin"))
            {
                Agent = UserManager.FindById(AgentID);
            }

            ObjModel.utblMstAppointment = new utblMstAppointment();
            ObjModel.CalEvents = Objdal.EventList();
            //   ObjModel.MstAppointmentList = Objdal.GetCalenderList(Agent.Id, TransactionID).ToList(); Commented By Neha
            ObjModel.AppointmentViewModelList = Objdal.GetCalenderListByTransactionIdNew1(TransactionID).ToList(); //Added by Neha
            ObjModel.utblMstAppointment.Email = Email;
            ObjModel.UserProfile = Objdal.GetUserDetails(ClientID);
            ObjModel.TransactionView = new TID()
            {
                TransactionID = TransactionID
            };
            ObjModel.AgentView = new AID()
            {
                AgentID = AgentID
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvEventList", ObjModel);
            }
            return View(ObjModel);

        }



        [HttpPost]
        public ActionResult CreateEvent(MstCalenderManageModel Itemdata, string AgentID = "")
        {

            ViewBag.Time = String.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}Z", Itemdata.utblMstAppointment.StartDate);
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            if (User.IsInRole("Admin"))
            {
                Agent = UserManager.FindById(AgentID);
            }//Added by Neha
            ObjModel.CalEvents = Objdal.EventList();
            ObjModel.MstAgentClientNameSelect = new MstAgentClientNameSelect();
            ObjModel.utblMstAppointment = new utblMstAppointment();
            // ObjModel.MstAppointmentList = Objdal.GetCalenderList(Agent.Id, Itemdata.TransactionView.TransactionID).ToList(); Removed by Neha
            ObjModel.AppointmentViewModelList = Objdal.GetCalenderListByTransactionIdNew1(Itemdata.TransactionView.TransactionID).ToList();//Added by Neha
            ObjModel.utblMstAppointment.Email = Itemdata.utblMstAppointment.Email;
            ObjModel.UserProfile = Objdal.GetUserDetails(Itemdata.UserProfile.ClientID);
            // Itemdata.utblMstAppointment.AgentID = Agent.Id; //Commented by Neha
            Itemdata.utblMstAppointment.AgentID = AgentID; //Added by Neha
            Itemdata.utblMstAppointment.IsEmailSent = false;
            Itemdata.utblMstAppointment.TransactionID = Itemdata.TransactionView.TransactionID;//added by neha
            Itemdata.utblMstAppointment.createdby = User.Identity.GetUserId();
            ObjModel.TransactionView = new TID()
            {
                TransactionID = Itemdata.TransactionView.TransactionID
            };
            ObjModel.AgentView = new AID()
            {
                AgentID = Itemdata.AgentView.AgentID
            };
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
                            ObjModel.MstAgentClientNameSelect = Objdal.GetNameEmail(Itemdata.UserProfile.ClientID);
                            string SDate = Itemdata.utblMstAppointment.StartDate.ToString("dd MMM yyyy hh:mm tt");
                            string EDate = Itemdata.utblMstAppointment.EndDate.ToString("dd MMM yyyy hh:mm tt");
                            string Date = string.Join(" to ", SDate, EDate);

                            string calendarId = string.Empty;
                            using (var context = new EFDBContext())
                            {
                                //  calendarId = context.utblMstGmailTokens.FirstOrDefault(m => m.UserEmail == Agent.Email).GmailAccount; Commented by Neha
                                calendarId = context.utblMstGmailTokens.FirstOrDefault(m => m.UserEmail == Agent.Email).GmailAccount;//Added by Neha
                            }

                            var model = new CalendarEvent()
                            {
                                CalendarId = calendarId,
                                Title = Itemdata.utblMstAppointment.Description,
                                Location = "660 Pennsylvania Ave. SE Suite 300, Washington, DC 20003 ",
                                StartDate = Itemdata.utblMstAppointment.StartDate,
                                EndDate = Itemdata.utblMstAppointment.EndDate,
                                Description = Itemdata.utblMstAppointment.Description,
                                Attendees = new string[] { ObjModel.MstAgentClientNameSelect.ClientEmail }
                            };

                            //var authenticator = GetAuthenticator(User.Identity.Name);Commented by Neha
                            var authenticator = GetAuthenticator(Agent.Email);
                            var service = new GoogleCalendarServiceProxy(authenticator);//Added by Neha
                            service.CreateEvent(model);
                            int MailStatus = SendEmailConfirmationToken(ObjModel.MstAgentClientNameSelect.ClientEmail, ObjModel.MstAgentClientNameSelect.ClientName, Itemdata.utblMstAppointment.Description, Agent.Name, Agent.Email, Date, Agent.Phone, Agent.Email);
                            if (MailStatus == 0)
                            {
                                TempData["MailMsg"] = 1;
                            }
                            else
                            {
                                TempData["ErrMsg"] = null;
                            }
                        }
                        return RedirectToAction("CreateEvent", new { Email = Itemdata.utblMstAppointment.Email, ClientID = Itemdata.UserProfile.ClientID, TransactionID = Itemdata.TransactionView.TransactionID, AgentID = ObjModel.AgentView.AgentID });
                    }
                    catch (Exception e)
                    {
                        string GAPI = string.Empty;


                        if (e.Message == "Error occurred while sending a direct message or getting the response.")
                        {
                            GAPI = "Unauthorized";
                        }
                        else
                        {
                            GAPI = "NotConfigured";
                        }
                        return RedirectToAction("CreateEvent", new { Email = Itemdata.utblMstAppointment.Email, ClientID = Itemdata.UserProfile.ClientID, TransactionID = Itemdata.TransactionView.TransactionID, AgentID = ObjModel.AgentView.AgentID, ErrMsg = GAPI });

                        //ModelState.AddModelError("", GAPI);
                        //return View(ObjModel);

                    }
                }
            }
            //ObjModel.MstAppointmentList = Objdal.GetCalenderList(User.Identity.Name, Itemdata.utblMstAppointment.Email).ToList(); Removed by Neha
            ObjModel.AppointmentViewModelList = Objdal.GetCalenderListByTransactionIdNew1(Itemdata.TransactionView.TransactionID).ToList();// Added by Neha
            ObjModel.utblMstAppointment.Email = Itemdata.utblMstAppointment.Email;
            return View(ObjModel);
        }


        #region Delete Event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, string Email, string ClientID, string TransactionID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            //var obj = new utblMstAppointment()
            //{
            //    Id = Id

            //};
            //db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
            //db.SaveChanges();
            var res = ObjStatus.RemoveDealEvent(Id, Email, TransactionID); //added by sonika
            return RedirectToAction("CreateEvent", "Calendly", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID });

        }
        #endregion



        public ActionResult EditEvent(int Id, string Email, string ClientID, string TransactionID, string AgentID = "")//AgentID Added by Neha
        {
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            if (User.IsInRole("Admin"))
            {
                Agent = UserManager.FindById(AgentID);
            }
            ObjModel.utblMstAppointment = new utblMstAppointment();
            ObjModel.CalEvents = Objdal.EventList();
            // ObjModel.MstAppointmentList = Objdal.GetCalenderList(Agent.Id, TransactionID).ToList();// Removed by Neha
            ObjModel.MstAppointmentList = Objdal.GetCalenderListByTransactionId(TransactionID).ToList();//Added by Neha
            ObjModel.utblMstAppointment.Email = Email;
            ObjModel.UserProfile = Objdal.GetUserDetails(ClientID);
            ObjModel.TransactionView = new TID()
            {
                TransactionID = TransactionID
            };
            ObjModel.AgentView = new AID()
            {
                AgentID = AgentID
            };
            ObjModel.utblMstAppointment = Objdal.GetAppointmentByID(Id);
            return View(ObjModel);

        }

        [HttpPost]
        public ActionResult EditEvent(MstCalenderManageModel Itemdata)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            if (User.IsInRole("Admin"))
            {
                Agent = UserManager.FindById(Itemdata.AgentView.AgentID);
            }//Added by Neha
            ObjModel.CalEvents = Objdal.EventList();
            ObjModel.MstAgentClientNameSelect = new MstAgentClientNameSelect();
            ObjModel.utblMstAppointment = new utblMstAppointment();
            //ObjModel.MstAppointmentList = Objdal.GetCalenderList(Agent.Id, Itemdata.TransactionView.TransactionID).ToList(); Removed by Neha
            ObjModel.MstAppointmentList = Objdal.GetCalenderListByTransactionId(Itemdata.TransactionView.TransactionID).ToList();// Added by Neha
            ObjModel.utblMstAppointment.Email = Itemdata.utblMstAppointment.Email;
            ObjModel.UserProfile = Objdal.GetUserDetails(Itemdata.UserProfile.ClientID);
            // Itemdata.utblMstAppointment.AgentID = Agent.Id; Commented by Neha
            Itemdata.utblMstAppointment.AgentID = Itemdata.AgentView.AgentID; //Added by Neha
            Itemdata.utblMstAppointment.IsEmailSent = false;
            if (ModelState.IsValid)
            {
                using (var db = new EFDBContext())
                {

                    try
                    {
                        TempData["ErrMsg"] = Objdal.EditEvent(Itemdata.utblMstAppointment);

                        if (Objdal.emailPreferences(Itemdata.utblMstAppointment.Email))
                        {
                            ObjModel.MstAgentClientNameSelect = Objdal.GetNameEmail(Itemdata.UserProfile.ClientID);
                            string SDate = Itemdata.utblMstAppointment.StartDate.ToString("dd MMM yyyy hh:mm tt");
                            string EDate = Itemdata.utblMstAppointment.EndDate.ToString("dd MMM yyyy hh:mm tt");
                            string Date = string.Join(" to ", SDate, EDate);

                            string calendarId = string.Empty;
                            using (var context = new EFDBContext())
                            {
                                // calendarId = context.utblMstGmailTokens.FirstOrDefault(m => m.UserEmail == User.Identity.Name).GmailAccount;  Commented by Neha 
                                calendarId = context.utblMstGmailTokens.FirstOrDefault(m => m.UserEmail == Agent.Email).GmailAccount;//Added by Neha
                            }

                            //var model = new CalendarEvent()
                            //{
                            //    CalendarId = calendarId,
                            //    Title = Itemdata.utblMstAppointment.Description,
                            //    Location = "660 Pennsylvania Ave. SE Suite 300, Washington, DC 20003 ",
                            //    StartDate = Itemdata.utblMstAppointment.StartDate,
                            //    EndDate = Itemdata.utblMstAppointment.EndDate,
                            //    Description = Itemdata.utblMstAppointment.Description,
                            //    Attendees = new string[] { ObjModel.MstAgentClientNameSelect.ClientEmail }
                            //};

                            //var authenticator = GetAuthenticator(User.Identity.Name);
                            //var service = new GoogleCalendarServiceProxy(authenticator);
                            //service.UpdateEvent(model);
                            int MailStatus = SendEmailConfirmationToken(ObjModel.MstAgentClientNameSelect.ClientEmail, ObjModel.MstAgentClientNameSelect.ClientName, Itemdata.utblMstAppointment.Description, Agent.Name, Agent.Email, Date, Agent.Phone, Agent.Email);
                            if (MailStatus == 0)
                            {
                                TempData["MailMsg"] = 1;
                            }
                            else
                            {
                                TempData["ErrMsg"] = null;
                            }
                        }
                        return RedirectToAction("CreateEvent", new { Email = Itemdata.utblMstAppointment.Email, ClientID = Itemdata.UserProfile.ClientID, TransactionID = Itemdata.TransactionView.TransactionID, AgentID = Itemdata.AgentView.AgentID });
                    }
                    catch (Exception e)
                    {
                        string a = e.Message;

                        ModelState.AddModelError("", a);
                        //return View(ObjModel);

                    }
                }
            }
            //ObjModel.MstAppointmentList = Objdal.GetCalenderList(User.Identity.Name, Itemdata.utblMstAppointment.Email).ToList(); Removed by Neha
            ObjModel.MstAppointmentList = Objdal.GetCalenderListByTransactionId(Itemdata.TransactionView.TransactionID).ToList();//Added by Neha

            ObjModel.utblMstAppointment.Email = Itemdata.utblMstAppointment.Email;
            return View(ObjModel);
        }

        //Added By Neha
        public ActionResult ViewAllEvents(string Email, string ClientID, string TransactionID, string AgentID = "", string ErrMsg = "")
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            ViewBag.AgentID = AgentID;
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            if (!string.IsNullOrEmpty(ErrMsg))
            {
                if (ErrMsg == "Unauthorized")
                {

                    TempData["ErrMsg"] = "....cannot register event on google calendar,because there is no calendar avaiable for this user....";
                }
                else if (ErrMsg == "NotConfigured")
                {
                    TempData["ErrMsg"] = "....looks like you havent configured your google account yet, or you've  changed your password recently, to configure your google account please click Sync google Calendar from sidebar menu.....";


                }
                else
                {
                    TempData["ErrMsg"] = null;
                }
            }

            if (User.IsInRole("Admin"))
            {
                Agent = UserManager.FindById(AgentID);
            }

            ObjModel.utblMstAppointment = new utblMstAppointment();
            ObjModel.CalEvents = Objdal.EventList();
            ObjModel.AppointmentViewModelList = Objdal.GetCalenderListWithUserRole(Agent.Id, TransactionID).ToList();
            // ObjModel.MstAppointmentList = Objdal.GetCalenderListNew(Agent.Id, Email).ToList();
            ObjModel.utblMstAppointment.Email = Email;
            ObjModel.UserProfile = Objdal.GetUserDetails(ClientID);
            ObjModel.TransactionView = new TID()
            {
                TransactionID = TransactionID
            };
            ObjModel.AgentView = new AID()
            {
                AgentID = AgentID
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvAllEventList", ObjModel);
            }
            return View(ObjModel);

        }


        //Added By Neha
        #region Delete Event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEvent(int Id, string Email, string ClientID, string TransactionID, string AgentID)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            //var obj = new utblMstAppointment()
            //{
            //    Id = Id

            //};
            //db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
            //db.SaveChanges();
            var res = ObjStatus.RemoveDealEvent(Id, Email, TransactionID); //added by sonika
            return RedirectToAction("ViewAllEvents", "Calendly", new { Email = Email, ClientID = ClientID, TransactionID = TransactionID, AgentID = AgentID });

        }
        #endregion


        private string PopulateBody(string Name, string EventName, string InviteeName, string InviteeEmail, string EventDate, string AgentPhone)
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

        private int SendEmailConfirmationToken(string email, string Name, string EventName, string InviteeName, string InviteeEmail, string EventDate, string AgentPhone, string AgentEmail)
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

            mm.To.Add(email);
            mm.CC.Add(AgentEmail);
            if (User.IsInRole("Admin"))
            {

                mm.CC.Add(AgentEmail);
                mm.CC.Add(User.Identity.Name);

            }

            mm.Priority = MailPriority.High;

            mm.Subject = "New Event Scheduled";
            mm.Body = this.PopulateBody(Name, EventName, InviteeName, InviteeEmail, EventDate, AgentPhone);
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


        private GoogleAuthenticator GetAuthenticator(string Email)
        {
            var authenticator = (GoogleAuthenticator)Session["authenticator"];

            if (authenticator == null || !authenticator.IsValid)
            {
                // Get a new Authenticator using the Refresh Token
                var refreshToken = new EFDBContext().utblMstGmailTokens.FirstOrDefault(c => c.UserEmail == Email).RefreshToken;
                authenticator = GoogleAuthorizationHelper.RefreshAuthenticator(refreshToken);
                Session["authenticator"] = authenticator;
            }

            return authenticator;
        }

    }
}