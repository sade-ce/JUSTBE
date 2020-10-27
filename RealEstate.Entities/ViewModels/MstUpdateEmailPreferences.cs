using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
  public class MstUpdateEmailPreferences
    {
        public bool EventCalendar { get; set; }
        public bool StatusTimeline { get; set; }
        public bool SettlementDate { get; set; }
    }
}
