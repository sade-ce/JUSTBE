using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Entities.Models
{
    public class utblMstEnquire
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public long EnquireID { get; set; }
        [Required(ErrorMessage = "* Tell Us Your Name")]
        [Display(Name = "Your Name")]

        public string Name { get; set; }
        [Required(ErrorMessage = "* Your Email Address")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "* Invalid Email Address...")]
        public string Email { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "* Your Message to us")]
        [Display(Name = "Message")]
        public string Message { get; set; }
        public DateTime EnquireDate { get; set; }
    }
}
