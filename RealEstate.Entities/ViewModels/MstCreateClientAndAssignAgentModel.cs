using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstCreateClientAndAssignAgentModel
    {
        public UserAdminRegisterModel Register { get; set; }
        public DealAdminAssignAgentManageModel AssignAgent { get; set; }


        //New Version
        public IEnumerable<MstClientTypes> ClientTypeDropDown { get; set; }
        public IEnumerable<ClientDDl> ClientDropDown { get; set; }
        public IEnumerable<AgentDDl> AgentDropDown { get; set; }
        public IEnumerable<MstViewEmailActivityLog> ClientActivity { get; set; }
        public IEnumerable<ClientList> ClientList { get; set; }
        public IEnumerable<QuizView> BuyerQuiz { get; set; }
        public IEnumerable<QuizView> SellerQuiz { get; set; }
        public IEnumerable<NeighborhoodDropDown> NeighborhoodDDL { get; set; }
        public utblMstTrackDealMaster utblMstTrackDealMasters { get; set; }
        //public ClientProfile ClientProfile { get; set; }
    }


    public class UserAdminRegisterModel
    {
        public string TransactionID { get; set; }
        [Required]

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "* Please correct Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First Name is required...")]
        [DisplayName("First Name :")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Last Name is required...")]
        [DisplayName("Last Name :")]
        public string LastName { get; set; }
        [DisplayName("Password :")]
        public string Password { get; set; }

        [StringLength(12)]
        [MaxLength(12)]
        [Display(Name = "Phone No :")]
        [Required(ErrorMessage = "Phone Number is required...")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Select Role...")]
        public string UserRole { get; set; }

        public string AgentEmail { get; set; }

        public string UserPhotoThumb { get; set; }
        public string UserPhotoNormal { get; set; }

        [Display(Name = "Select Client :")]
        public string ClientID { get; set; }

        [Display(Name = "Select Agent :")]
        public string AgentID { get; set; }

        [Display(Name = "Transaction type :")]
        public int? ClientTypeID { get; set; }
        public string Status { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime UpdatedOn { get; set; }

        //New Version
        public string ClientType { get; set; }
        public string HaveReferred { get; set; }
        public string ReferalSource { get; set; }
        public string ClientTier { get; set; }
        public int? Neighborhood { get; set; }
        public string ClientNeighborhood { get; set; }
        public string Partner { get; set; }
        public int? Children { get; set; }
        public string DOB { get; set; }
        public string ClientDOB { get; set; }
        public string ClientAddress { get; set; }
        public string HomeAnniversary { get; set; }
    }

    public class ClientProfile
    {
        public string TransactionID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string UserRole { get; set; }
        public string AgentEmail { get; set; }
        public string UserPhotoThumb { get; set; }
        public string UserPhotoNormal { get; set; }
        public string ClientID { get; set; }
        public string AgentID { get; set; }
        public int? ClientTypeID { get; set; }
        public string Status { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ClientType { get; set; }
    }
  
}
