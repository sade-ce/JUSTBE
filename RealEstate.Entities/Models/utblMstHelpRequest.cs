using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstHelpRequest
    {
        [Key]
        public int HelpID { get; set; }
        public string TransactionID { get; set; }
        public string ClientID { get; set; }
        [Required]
        public string Message { get; set; }
        public string UpdatedOn { get; set; }
    }
}
