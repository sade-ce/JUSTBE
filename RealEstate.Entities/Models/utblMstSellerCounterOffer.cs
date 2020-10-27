using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Entities.Models
{
    public class utblMstSellerCounterOffer
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public string CounterOfferID { get; set; }
        public string TransactionID { get; set; }
        public string SellerID { get; set; }
        public int BuyerID { get; set; }
        public string Status { get; set; }

        [Display(Name ="Description :")]
        [Required(ErrorMessage ="brief about this transaction")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Quote Price")]
        [Display(Name ="Offer Price ($) :")]

        public double Price { get; set; }

        public int ClientTypeID { get; set; }
        public DateTime CounteredOn { get; set; }
        public bool IsLatest { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
