using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class VendorCategory
    {
        [Key]
        public string CategoryId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Name")]
        public string Name { get; set; }
        public string CategoryImage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        [Required(ErrorMessage = "Select Display Type")]
        public string DisplayType { get; set; }
    }
}
