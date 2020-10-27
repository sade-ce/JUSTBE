using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Entities.Models
{
    public class utblMstStatus
    {
        [Key]
        public int StatusID { get; set; }
        [DisplayName("Status")]
        [Required(ErrorMessage = "Enter Status...")]
        public string Status { get; set; }
        [DisplayName("Percentage")]
        [Required(ErrorMessage = "Enter Percentage...")]
        [Range(5, double.MaxValue, ErrorMessage = "must be greater than or equal to 5")]
        public double Percentage { get; set; }



        [DisplayName("Is Contingencies?")]
        [Required(ErrorMessage = "This field is required...")]

        public bool IsContingencies { get; set; } = false;
        [DisplayName("Description")]
        [Required(ErrorMessage = "This field is required...")]
        public string Description { get; set; }
        [DisplayName("Video URL")]
        [Required(ErrorMessage = "Please input video URL")]
        [Url]
        public string VideoURL { get; set; }
        [DisplayName("Hierarchy")]
        [Required(ErrorMessage = "Please input Hierarchy")]
        public double Hierarchy { get; set; }
        public DateTime UpdatedOn { get; set; }
      
    }
}
