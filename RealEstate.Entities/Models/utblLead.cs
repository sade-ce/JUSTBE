using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblPartialLead
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string UnitNo { get; set; }
        public string CityStateZip { get; set; }
        public DateTime VisitedOn { get; set; }
    }
    public class utblFullLead
    {
        public long VisitorID { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public string UnitNo { get; set; }
        public string CityStateZip { get; set; }
        [Required(ErrorMessage = "* Your Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "* Your Email ")]
        [EmailAddress(ErrorMessage = "* Please correct Email")]
        public string EmailID { get; set; }
        //[Required(ErrorMessage = "Enter Your Contact No")]

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }
        public DateTime VisitedOn { get; set; }
    }
}
