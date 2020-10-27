using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
   public class MstCheckLoggedClientDeal
    {
        public IEnumerable<CheckDeal> CheckDeal { get; set; }
        public IEnumerable<CheckDeal> CheckSharedDeal { get; set; }

        public IEnumerable<CheckCalendar> CheckCalendar { get; set; }
        public IEnumerable<CheckAgent> CheckAgent { get; set; }


    }

    public class CheckDeal
    {
        public string TransactionID { get; set; }
    }
    public  class CheckCalendar
    {
        public int Id { get; set; }
    }
    public class CheckAgent
    {
        public string Id { get; set; }
        public string AgentName { get; set; }
    }
}
