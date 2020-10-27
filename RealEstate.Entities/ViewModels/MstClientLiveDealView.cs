using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientLiveDealView
    {
        public string TrackingID { get; set; }
        public string DealTrackDocID { get; set; }
        public string TrackDocFile { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string Percentage { get; set; }
        public string IsResReq { get; set; }

    }
}
