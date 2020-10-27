using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using GoogleApiUtils.GoogleCalendarApi;
using GoogleApiUtils;

namespace RealEstate.Web.Controllers
{
    public class MeetingController : Controller
    {
        dalScheduleMeeting objDal = new dalScheduleMeeting();
        MstScheduleMeeting ObjMeeting = new MstScheduleMeeting();
        public ActionResult Schedule()
        {
            ObjMeeting.Agent = objDal.AgentList();
            return View(ObjMeeting);
        }
        [HttpPost]
        public ActionResult Schedule(MstScheduleMeeting ItemData, double? Duration,string Office,string Email)
        {
            

            if (ModelState.IsValid)
            {
                string calendarId = string.Empty;
                using (var context = new EFDBContext())
                {
                    calendarId = context.utblMstGmailTokens.FirstOrDefault(m => m.UserEmail == ItemData.AgentEmail).GmailAccount;
                }

                double Minutes = Convert.ToDouble( Duration != null ? Duration : 15);

                TimeSpan timeSpan = ItemData.StartDate.TimeOfDay;
                string s = string.Format("{0} {1}", ItemData.When.Date.ToShortDateString(), timeSpan);
                DateTime StartDate = DateTime.Parse(s);
                DateTime EndDate = StartDate.AddMinutes(Minutes);
                var model = new CalendarEvent()
                {
                    CalendarId = calendarId,
                    Title = ItemData.Agenda,
                    Location = Office.ToString(),
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Description = ItemData.Description.ToString(),
                    Attendees = new string[] { Email }
            };

                var authenticator = GetAuthenticator(ItemData.AgentEmail);
                var service = new GoogleCalendarServiceProxy(authenticator);
                service.CreateEvent(model);
             
                
            }

            ObjMeeting.Agent = objDal.AgentList();
            return View(ObjMeeting);
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