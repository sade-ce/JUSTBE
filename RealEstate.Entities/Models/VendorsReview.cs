using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class VendorsReview
    {
        [Key]
        public int ReviewId { get; set; }
        public string ClientId { get; set; }
        public string VendorId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
