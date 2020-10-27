using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    
   
    public class MstRatifiedEditViewModel
    {
        public int ClosingDateID { get; set; }
        public string TrackingID { get; set; }
        public string ClientID { get; set; }
        public string TransactionID { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage ="* Please enter settlement date")]
        [Display(Name ="Settlement Date:")]
        
        public DateTime SettlementDate { get; set; }

        [Display(Name = "MLS Address :")]
        [RequiredIf("ListingTypeID == 1", ErrorMessage = "Please select valid address from dropdown")]

        public string MLSID { get; set; }

        [Required(ErrorMessage = "* Please select listing type")]
        [Display(Name = "Listing Type:")]
        public int ListingTypeID { get; set; }
        [Display(Name = "Address :")]
        [RequiredIf("ListingTypeID == 2", ErrorMessage = "Please Enter Address")]
        public string Address { get; set; }
        public string URL { get; set; }

        [Required(ErrorMessage = "* Please enter ratified date")]
        [Display(Name = "Ratified Date:")]
        public DateTime RatifiedDate { get; set; }


    }
}
