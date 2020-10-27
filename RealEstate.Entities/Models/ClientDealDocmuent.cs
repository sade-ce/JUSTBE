using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class ClientDealDocmuent
    {
        [Key]
        public string ClientDocId { get; set; }
        public string TransactionID { get; set; }
        public int DocId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string DocFile { get; set; }
   
        public string ClientId { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
