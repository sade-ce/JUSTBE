using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstTrackDealDoc
    {
        [Key]
        public string DealTrackDocID { get; set; }
        public string TrackingID { get; set; }
        public string TransactionID { get; set; }

        [Required(ErrorMessage ="Select Document")]
        public int DocID { get; set; }
        [Required(ErrorMessage = "Select Status")]
        public int StatusID { get; set; }

        [DisplayName("Upload Tracking Document")]
        public string TrackDocFile { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
