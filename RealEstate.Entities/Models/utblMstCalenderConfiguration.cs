using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstCalenderConfiguration
    {
        [Key]
        public int CalenderConfigID { get; set; }

        [Required(ErrorMessage ="Please Select Status")]
        [Display(Name ="Status")]
        public int StatusID { get; set; }
        public DateTime ConfigDate { get; set; }
          
    }
}
