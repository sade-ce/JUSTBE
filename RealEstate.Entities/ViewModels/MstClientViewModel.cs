using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientViewModel
    {
        public IEnumerable<UserProfileViewModel> MstClientView { get; set; }
        public UserProfileViewModel ClientView { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int FullLead { get; set; }
        public int PartialLead { get; set; }
        public IEnumerable<AgentEmailDDl> CoOrdinator { get; set; }

        public DataGrid DataGrid { get; set; }
        public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }
    }

    public class AgentEmailDDl
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Select Co-Ordinator")]
        [Display(Name ="Co-Ordinator")]
        public string Email { get; set; }
    }

    public class DataGrid
    {
        public string Email { get; set; }

        [Required(ErrorMessage ="Select Agent")]
        public string AgentEmail { get; set; }

    }
}
