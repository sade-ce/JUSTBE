using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Entities.Models
{
   public class utblMstAgentContacts
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public long ContactID { get; set; }
        [Required(ErrorMessage = "* Tell Us Your Name")]
        [Display(Name = "Your Name")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "* Your Email Address")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "* Invalid Email Address...")]

        public string Email { get; set; }


        [Required(ErrorMessage = "*Phone")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "* Please specify a subject")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "* Your Message")]
        [Display(Name = "Message")]
        public string Message { get; set; }
        public DateTime ContactDate { get; set; }
    }
}
