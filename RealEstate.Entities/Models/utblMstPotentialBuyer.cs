using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstPotentialBuyer
    {
        [Key]
        public int BuyerID { get; set; }

        [Required(ErrorMessage = "Buyer Name is required")]
        [Display(Name = "Buyer Name :")]
        public string BuyerName { get; set; }
        [Required(ErrorMessage = "Buyer Email is required")]
        [Display(Name = "Email :")]
        public string BuyerEmail { get; set; }
        [Required(ErrorMessage = "Buyer Phone is required")]
        [Display(Name = "Phone :")]
        public string BuyerPhone { get; set; }
        [Required(ErrorMessage = "Buyer Address is required")]
        [Display(Name = "Address :")]
        public string BuyerAddress { get; set; }
        public string UpdatedOn { get; set; }

    }
}
