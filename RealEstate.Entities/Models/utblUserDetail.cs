using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblUserDetail
    {
        [Key]
        public int UserDetailsID { get; set; }
        public string UserName { get; set; }
        public string UserPhotoThumb { get; set; }
        public string UserPhotoNormal { get; set; }
    }
}
