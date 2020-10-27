using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstJobPosition
    {
        [Key]
        public int JobID { get; set; }
        [Required(ErrorMessage = "Please Select Department")]
        [Display(Name = "Select Department")]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Please Post Job Content")]
        public string JobContent { get; set; }

        //[Required(ErrorMessage = "Specify End Date")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
