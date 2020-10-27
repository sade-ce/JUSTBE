using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstClosingConfigutation
    {
        [Key]
        public int ConfigCloseID { get; set; }

        [Required(ErrorMessage = "Select Status")]
        [Display(Name ="Update Status :")]
        public int StatusID { get; set; }
       
        public DateTime ConfigDate { get; set; }
    }
}
