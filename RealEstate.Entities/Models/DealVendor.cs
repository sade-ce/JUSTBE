using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class DealVendor
    {
        [Key]
        public string DealVendorId { get; set; }
        public string Transaction_Id { get; set; }

        [Display(Name = "Vendor")]
        [Required(ErrorMessage ="Select Vendor")]
        public string Vendor_Id { get; set; }


        public string Email { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int VendorContact_Id { get; set; }
    }
}
