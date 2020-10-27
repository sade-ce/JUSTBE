using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class Testimonial
    {
        [Key]
        public int Id { get; set; }
        //[Required(ErrorMessage = "Title is required")]
      
        public string UserType { get; set; }
        public string UserName { get; set; }

        public string Description { get; set; }
        //[Required(ErrorMessage = "Content is required")]

        public int Rating { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
