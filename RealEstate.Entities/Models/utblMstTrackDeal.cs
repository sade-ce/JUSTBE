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
    public class utblMstTrackDeal
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public string TrackingID { get; set; }
        public string TransactionID { get; set; }
        public string ClientID { get; set; }
        [DisplayName("Status :")]
        [Required(ErrorMessage = "Select Status..")]

        public int StatusID { get; set; }

        [DisplayName("Client :")]
        public string Email { get; set; }
        public bool IsApplicable { get; set; }
        public DateTime UpdatedOn { get; set; }


    }
}
