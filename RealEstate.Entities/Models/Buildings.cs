using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
 
    public class Buildings
    {
        [Key]
        public int BuildingId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Neighborhood is required")]
        public int Neighborhood_Id { get; set; }

      
        public string Address { get; set; }
     
    

        [StringLength(12)]
        [MaxLength(12)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string FrontDeskPhone { get; set; }

        [Display(Name = "Vendors")]
        public string Vendors { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int? NumberOfUnits { get; set; }
    }
}
