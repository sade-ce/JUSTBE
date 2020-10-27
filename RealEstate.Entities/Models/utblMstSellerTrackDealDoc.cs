using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RealEstate.Entities.Models
{
    public class utblMstSellerTrackDealDoc
    {
        [Key]
        public string SellerDealDocID { get; set; }
        public string SellerTrackingID { get; set; }
        public string TransactionID { get; set; }

        [Required(ErrorMessage = "Select Document")]
        public int SellerDocID { get; set; }
        [Required(ErrorMessage = "Select Status")]
        public int SellerStatusID { get; set; }

        [DisplayName("Upload Tracking Document")]
        public string TrackDocFile { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
