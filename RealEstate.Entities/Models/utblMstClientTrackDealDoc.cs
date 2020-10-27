using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstClientTrackDealDoc
    {
        [Key]
        public string ClientTrackDocID { get; set; }
        public string TrackingID { get; set; }
        public int DocID { get; set; }
        public string TrackDocFile { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
