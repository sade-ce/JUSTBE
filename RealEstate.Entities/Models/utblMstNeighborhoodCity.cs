using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
   public class utblMstNeighborhoodCity
    {
        [Key]
        public int CityID { get; set; }
        public int StateID { get; set; }

        [Display(Name ="City")]
        [Required(ErrorMessage ="Please Select City")]
        public string CityName { get; set; }
    }
}
