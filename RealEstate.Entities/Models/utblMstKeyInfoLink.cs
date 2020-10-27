using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstKeyInfoLink
    {
        [Key]
        public int KeyInfoID { get; set; }

        [Required(ErrorMessage ="Please Specify valid link")]
        [Display(Name ="Tour Url")]
        [Url]
        public string TourURL { get; set; }

        [Required(ErrorMessage ="Please Specify Title")]
        [Display(Name ="Title")]
        public string Title { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string KeyInfoImage { get; set; }
        
    }
}
