using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class BuildingAttachments
    {
        [Key]
        public int BuildingAttachmentId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Name")]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Updatedby { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
