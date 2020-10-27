using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstClientGallerie
    {
        [Key]
        public string GallaryID { get; set; }

        public string TransactionID { get; set; }

        [Display(Name = "Upload")]
        public string PhotoNormal { get; set; }
        public string PhotoThumb { get; set; }

        //[Required(ErrorMessage = "Write some description")] //Commented By Neha
        //[Display(Name = "Description")]
        public string Description { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
