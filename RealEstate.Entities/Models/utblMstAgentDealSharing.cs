using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstAgentDealSharing
    {
        [Key]
        public string ShareID { get; set; }
        public string ClientID { get; set; }
        public string SharerAgentID { get; set; }
        public string TransactionID { get; set; }
        [DisplayName("Agent")]
        [Required(ErrorMessage = "Select Agent")]
        public string SharedToAgentID { get; set; }
        public string Notes { get; set; }
        public DateTime SharedOn { get; set; }


    }
}
