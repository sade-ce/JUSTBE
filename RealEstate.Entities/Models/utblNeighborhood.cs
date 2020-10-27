using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblNeighborhood
    {
        [Key]
        public int NeighborhoodID { get; set; }

        [Required(ErrorMessage ="Please Select Neighborhood")]
        public int StateID { get; set; }
        [Required(ErrorMessage="Please Select City")]
        public int CityID { get; set; }

        [Required(ErrorMessage ="Neighborhood Content is required")]

        public string PostContent { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
