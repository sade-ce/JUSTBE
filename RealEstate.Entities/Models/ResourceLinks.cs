using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class ResourceLinks
    {
        [Key]
        public int LinkId { get; set; }
        public int Type_Id { get; set; }
        public int Title_Id { get; set; }
        public string Link { get; set; }
        public string CreatedBy { get; set; }
        public int CreatedOn { get; set; }
    }
}
