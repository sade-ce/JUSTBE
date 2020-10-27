using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstCareerDepartment
    {
        [Key]
        public int DepartmentID { get; set; }
        [Required]
        [Display(Name ="Position Title :")]
        public string Position { get; set; }
        [Required]
        [Display(Name = "Location :")]
        public string Description { get; set; }

      //  [Required]
       // [Range(1, Int32.MaxValue, ErrorMessage = "must be greater than or equal to 1")]
        [Display(Name = "Total Position :")]
        public int TotalPosition { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
