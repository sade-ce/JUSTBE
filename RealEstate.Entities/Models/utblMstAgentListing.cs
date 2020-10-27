using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstAgentListing
    {
        [Key]
        public int AgentListingID { get; set; }

        [Required(ErrorMessage = "Please Input Agent Name")]
        [Display(Name = "Agent Name")]
        public string AgentName { get; set; }

        [Required(ErrorMessage = "Please Select Agent Email")]
        [Display(Name = "Agent Email")]
        public string AgentEmail { get; set; }

        [Required(ErrorMessage = "Please input Agent Phone")]
        [Display(Name = "Agent Phone")]
        public string AgentPhone { get; set; }
        [Required(ErrorMessage = "Please input MLS Listing ID")]
        [Display(Name = "Agent Phone")]
        public string AgentMLS { get; set; }
        [Url]
        [Required(ErrorMessage = "Please input Listing URL")]
        [Display(Name = "Listing URL")]
        public string ListingURL { get; set; }

        public int UpdatedOn { get; set; }

    }
}
