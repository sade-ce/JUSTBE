using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Models
{
    public class UpdateEmailPreferences
    {
        public bool EventCalendar { get; set; }
        public bool StatusTimeline { get; set; }

        public bool SettlementDate { get; set; }

    }
}