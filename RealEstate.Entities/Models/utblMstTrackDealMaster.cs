using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstTrackDealMaster
    {
        [Key]
        public string TransactionID { get; set; }

        [Display(Name ="Select Client :")]
        [Required(ErrorMessage ="* Choose Client")]
        public string ClientID { get; set; }
        [Display(Name = "Select Agent :")]
        [Required(ErrorMessage = "* Choose Agent")]
        public string AgentID { get; set; }

        [Display(Name = "Transaction type :")]
        [Required(ErrorMessage = "* Transaction Type :")]
        public int ClientTypeID { get; set; }

        public string Status { get; set; }
        public string Year { get; set; }

        [Required]
        public string Description { get; set; }

        [RequiredIf("ClientTypeID == 2", ErrorMessage = "Please enter selling address")]

        public string Address { get; set; }
        public DateTime UpdatedOn { get; set; }



    }
}
