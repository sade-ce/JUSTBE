using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Entities.ViewModels
{

    public class MstContingenciesViewVersion2
    {
        public int StatusID { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsContingencies { get; set; }

        public int NoOfDays { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsShowClient { get; set; }
        public bool IsCompleted { get; set; }
        public string TrackingID { get; set; }
        public string AdditionalInfo { get; set; }
      
        public string Time { get; set; }
        [AllowHtml]
        public string Notes { get; set; }
    }
}
