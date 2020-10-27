using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstEventCalMaster
    {
        [Key]
        public int CalEventID { get; set; }
        [Display(Name ="Event Title")]
        [Required(ErrorMessage ="Select Event Title")]
        public string EventText { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
