using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class Vendor
    {
        [Key]
        public string VendorId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Sub Title")]
        public string SubTitle { get; set; }
        [Required(ErrorMessage = "Select Vendor Type")]
        public string Category_Id { get; set; }

        [StringLength(12)]
        [MaxLength(12)]
        [Display(Name = "Phone No.")]
        //[Required(ErrorMessage = "Phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }

        //[Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter correct Email")]
        public string Email { get; set; }

        [Display(Name = "Website link")]
        //[Url(ErrorMessage = "Please enter correct link")]

        [RegularExpression(@"^(?:https?://|s?ftps?://)?(?!www | www\.)[A-Za-z0-9_-]+\.+[A-Za-z0-9.\/%&=\?_:;-]+$", ErrorMessage = "Please enter correct link")]
        public string WebsiteLink { get; set; }

        public string About { get; set; }
        public string VendorImage { get; set; }
        public string Location { get; set; }
        public bool IsRecommended { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        //public string VendorName { get; set; }
    }
}
