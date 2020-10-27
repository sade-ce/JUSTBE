using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class UserGallery
    {
        [Key]
        public string UserGalleryId { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Upload")]
        public string Photo { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
