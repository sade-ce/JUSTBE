using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class TeamMembers
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [DisplayName("Designation")]
        public string Designation { get; set; }
        public string MemberImage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }


        [Url(ErrorMessage = "Please enter correct url")]
        public string Facebook { get; set; }
     
        [Url(ErrorMessage = "Please enter correct url")]
        public string Twitter { get; set; }

        [Url(ErrorMessage = "Please enter correct url")]
        public string Instagram { get; set; }

        [Url(ErrorMessage = "Please enter correct url")]
        public string Pinterest { get; set; }

        [Required(ErrorMessage = "Profile link is required")]
        public string ProfileLink { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [DisplayName("Short Description")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Order number is required")]
        [DisplayName("Order Number")]
        [RegularExpression("^[1-9]*$", ErrorMessage = "Order must be numeric and > 0")]
        public int OrderNumber { get; set; }
        public string About { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter correct email")]
        public string ContactEmail { get; set; }

        [StringLength(12)]
        [MaxLength(12)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }
    }
}
