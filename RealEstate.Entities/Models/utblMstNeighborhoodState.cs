using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstNeighborhoodState
    {
        [Key]
        public int StateID { get; set; }
        [Display(Name = "Neighborhood")]
        [Required(ErrorMessage = "Select Neighborhood")]
        public string StateName { get; set; }
    }
}
