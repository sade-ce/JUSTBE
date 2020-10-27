using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstBuyerDocument
    {
        [Key]
        public int DocID { get; set; }
        [Display(Name = "Document Title")]
        [Required(ErrorMessage = "This field is required")]

        public string Title { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "This field is required")]
        public string Description { get; set; }
        public DateTime UpdatedOn { get; set; }


    }
}
