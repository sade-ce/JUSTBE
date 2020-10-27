using Google.Apis.Authentication;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleApiUtils.GoogleCalendarApi
{
    public class GoogleCalendarServiceProxy
    {
        private IAuthenticator _authenticator;

        public GoogleCalendarServiceProxy(GoogleAuthenticator googleAuthenticator)
        {
            _authenticator = googleAuthenticator.Authenticator;
        }

        // see https://developers.google.com/google-apps/calendar/v3/reference/calendarList/list
        public IEnumerable<Calendar> GetCalendars()
        {
            var calendarService = new CalendarService(_authenticator);
            var calendars = calendarService.CalendarList.List().Fetch().Items.Select(c => new Calendar()
            {
                Id = c.Id,
                Title = c.Summary,
                Location = c.Location,
                Description = c.Description
            });

            return calendars;
        }

        // see https://developers.google.com/google-apps/calendar/v3/reference/calendars/get
        public Calendar GetCalendar(string calendarId)
        {
            var calendarService = new CalendarService(_authenticator);
            var calendar = calendarService.CalendarList.List().Fetch().Items.FirstOrDefault(c => c.Summary.Contains(calendarId));
            if (calendar == null) throw new GoogleCalendarServiceProxyException("There's no calendar with that id");

            return new Calendar()
            {
                Id = calendar.Id,
                Title = calendar.Summary,
                Location = calendar.Location,
                Description = calendar.Description
            };
        }

        // see https://developers.google.com/google-apps/calendar/v3/reference/events/list
        public IEnumerable<CalendarEvent> GetEvents(string calendarId, DateTime startDate, DateTime endDate)
        {
            List<CalendarEvent> calendarEvents = null;
            var calendarService = new CalendarService(_authenticator);
            var calendar = GetCalendar(calendarId);

            var request = calendarService.Events.List(calendar.Id);
            request.TimeMin = startDate.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");
            request.TimeMax = endDate.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");
            var result = request.Fetch().Items;

            if (result != null)
            {
                calendarEvents = result.Select(c => new CalendarEvent()
                {
                    Id = c.Id,
                    CalendarId = calendarId,
                    Title = c.Summary,
                    Location = c.Location,
                    StartDate = DateTime.Parse(c.Start.DateTime),
                    EndDate = DateTime.Parse(c.End.DateTime),
                    Description = c.Description,
                    ColorId = Int32.Parse(c.ColorId),
                    Attendees = c.Attendees != null ? c.Attendees.Select(attendee => attendee.Email) : null
                }).ToList();
            }

            return calendarEvents;
        }

        // see https://developers.google.com/google-apps/calendar/v3/reference/events/get
        public CalendarEvent GetEvent(string calendarId, string eventId)
        {
            var calendarService = new CalendarService(_authenticator);
            var calendarEvent = calendarService.Events.Get(calendarId, eventId).Fetch();
            if (calendarEvent == null) throw new GoogleCalendarServiceProxyException("There is no event stored in the calendar with that id");

            return new CalendarEvent()
            {
                Id = calendarEvent.Id,
                CalendarId = calendarId,
                Title = calendarEvent.Summary,
                Location = calendarEvent.Location,
                StartDate = DateTime.Parse(calendarEvent.Start.DateTime),
                EndDate = DateTime.Parse(calendarEvent.End.DateTime),
                Description = calendarEvent.Description,
                ColorId = Int32.Parse(calendarEvent.ColorId),
                Attendees = calendarEvent.Attendees != null ? calendarEvent.Attendees.Select(c => c.Email) : null
            };
        }

        // see https://developers.google.com/google-apps/calendar/v3/reference/events/insert
        public bool CreateEvent(CalendarEvent calendarEvent)
        {

            var calendarService = new CalendarService(_authenticator);
            var calendar = GetCalendar(calendarEvent.CalendarId);

            //List<EventReminder> eventReminder = new List<EventReminder>();
            //eventReminder.Add(new EventReminder { Minutes = 4, Method = "email" });

            //Event.RemindersData de = new Event.RemindersData();
            //de.UseDefault=false;

            //var curTimeZone = TimeZone.CurrentTimeZone;
            //var dateOffsetStart = new DateTimeOffset(calendarEvent.StartDate, curTimeZone.GetUtcOffset(calendarEvent.StartDate));
            //var dateOffsetEnd = new DateTimeOffset(calendarEvent.EndDate, curTimeZone.GetUtcOffset(calendarEvent.EndDate));
            //var startTimeString = dateOffsetStart.ToString("o");
            //var endTimeString = dateOffsetEnd.ToString("o");

            var r1 = new EventReminder { Method = "email", Minutes = 1440 };
            var r2 = new EventReminder { Method = "popup", Minutes =1440 };
            var erd = new Event.RemindersData { UseDefault = false, Overrides = new[] { r1, r2 } };


            Event newEvent = new Event()
            {
                Reminders = erd,
                Summary = calendarEvent.Title,
                //Location = calendarEvent.Location,
                Description = calendarEvent.Description,
                Start = new EventDateTime()
                {
                    DateTime =
                    //String.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}Z", calendarEvent.StartDate)
                   String.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}Z", calendarEvent.StartDate).Replace("Z", "")
                    ,
                    TimeZone= "America/New_York"


                },
                End = new EventDateTime()
                {
                    DateTime = String.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}Z", calendarEvent.EndDate).Replace("Z", "")
                     ,
                    
                    TimeZone = "America/New_York"
                },
                Attendees = (calendarEvent.Attendees != null) ? calendarEvent.Attendees.Select(email => new EventAttendee { Email = email }).ToList<EventAttendee>() : null,
                ColorId = ((int)calendarEvent.ColorId).ToString()
            };

            var result = calendarService.Events.Insert(newEvent, calendar.Id).Fetch();
           
            return result != null;
        }

        // see https://developers.google.com/google-apps/calendar/v3/reference/events/update
        public bool UpdateEvent(CalendarEvent calendarEvent)
        {
            var calendarService = new CalendarService(_authenticator);
            var calendar = GetCalendar(calendarEvent.CalendarId);
            var toUpdate = calendarService.Events.Get(calendarEvent.CalendarId, calendarEvent.Id).Fetch();
            if (toUpdate == null) throw new GoogleCalendarServiceProxyException("There is no event stored in the calendar with that id");

            toUpdate.Summary = calendarEvent.Title;
            //toUpdate.Location = calendarEvent.Location;
            toUpdate.Description = calendarEvent.Description;
            toUpdate.Start = new EventDateTime() {

                DateTime = calendarEvent.StartDate.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK"),
                TimeZone= "America/Washington"

            };
            toUpdate.End = new EventDateTime() {
            DateTime = calendarEvent.EndDate.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffK"),
            TimeZone = "America/Washington"

            };
            toUpdate.ColorId = ((int)calendarEvent.ColorId).ToString();

            if (calendarEvent.Attendees != null && calendarEvent.Attendees.Count() > 0 && !string.IsNullOrEmpty(calendarEvent.Attendees.First()))
            {
                toUpdate.Attendees = calendarEvent.Attendees.Select(email => new EventAttendee { Email = email }).ToList<EventAttendee>();
            }

            var result = calendarService.Events.Update(toUpdate, calendar.Id, calendarEvent.Id).Fetch();

            return result != null;
        }

        // see https://developers.google.com/google-apps/calendar/v3/reference/events/delete
        public void DeleteEvent(string calendarId, string eventId)
        {
            var calendarService = new CalendarService(_authenticator);
            calendarService.Events.Delete(calendarId, eventId).Fetch();
        }
    }

    // see https://developers.google.com/google-apps/calendar/v3/reference/colors/get
    public enum GoogleEventColors
    {
        LightBlue = 1, // #a4bdfc
        LightGreen, // #7ae7bf
        LightViolet, // #dbadff
        LightRed, // #ff887c
        Yellow, // #fbd75b
        Orange, // #ffb878
        Turquoise, // #46d6db
        Gray, // #e1e1e1
        Blue, // #5484ed
        Green, // #51b749
        Red // #dc2127
    }

    public class GoogleCalendarServiceProxyException : Exception
    {
        public GoogleCalendarServiceProxyException(string errorMessage) : base(errorMessage) { }
    }
}
