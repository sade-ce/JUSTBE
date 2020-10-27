using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstClosingDate
    {
        [Key]
        public int ClosingDateID { get; set; }
        public string Email { get; set; }
        public string ClientID { get; set; }

        [Required(ErrorMessage ="Settlement Date Is Required")]
        [Display(Name ="Settlement Date :")]
        public DateTime? ClosingDate { get; set; }
        [Display(Name = "MLS Address :")]
        [RequiredIf("ListingTypeID == 1",ErrorMessage ="Please select address from the dropdown")]
        public string MLSID { get; set; }

        [Required(ErrorMessage = "Select Listing Type")]
        [Display(Name = "Listing Type :")]
        public int ListingTypeID { get; set; }
        [Display(Name = "Address :")]
        [RequiredIf("ListingTypeID == 2", ErrorMessage = "Please Enter Address")]

        public string Address { get; set; }
        [Display(Name = "Tour URL :")]
        //[RequiredIf("ListingTypeID == 2", ErrorMessage = "Please Enter Valid URL")]
        [Url]
        public string URL { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int? HomeType { get; set; }
        public int? StateId { get; set; }
        public int? ContingencyType { get; set; }
        public int? lots { get; set; }

    }
}
