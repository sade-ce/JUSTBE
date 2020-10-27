using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressiveAnnotations.Attributes;
namespace RealEstate.Entities.Models
{
   public class utblMstClientHomeListing
    {
        [Key]
        public int MLSListingID { get; set; }

        [RequiredIf("ListingTypeID == 1", ErrorMessage ="Specify MLS ID")]
        [Display(Name ="MLS ID :")]
        public string MLSID { get; set; }
        [Display(Name ="Listing Type :")]
        [Required(ErrorMessage = "Select Listing Type")]
        
        public int ListingTypeID { get; set; }
        [Display(Name = "Home Address :")]
        [RequiredIf("ListingTypeID == 2", ErrorMessage = "Address is required")]
        public string Address { get; set; }
        public string Email { get; set; }
        [Display(Name = "Tour Link :")]
        [RequiredIf("ListingTypeID == 2", ErrorMessage = "Home Tour Link is required")]
        public string URL { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
