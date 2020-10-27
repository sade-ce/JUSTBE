using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class ClientDealDocuments
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string DocFile { get; set; }
        public string TransactionID { get; set; }
        public string ClientId { get; set; }
        [Display(Name = "Document Title")]
        [Required(ErrorMessage = "This field is required")]

        public string Title { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "This field is required")]
        public string Description { get; set; }
        public string DocumentType { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
