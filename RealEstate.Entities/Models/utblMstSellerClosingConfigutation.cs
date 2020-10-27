using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RealEstate.Entities.Models
{
    public class utblMstSellerClosingConfigutation
    {
        [Key]
        public int SellerClosingConfigID { get; set; }

        [Required(ErrorMessage = "Select Status")]
        [Display(Name = "Update Status :")]
        public int SellerStatusID { get; set; }

        public DateTime ConfigDate { get; set; }
    }
}
