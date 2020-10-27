using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstDealAgentManageModel

    {
        public utblMstTrackDealMaster utblMstTrackDealMasters { get; set; }

        public IEnumerable<ClientList> ClientList { get; set; }
        public IEnumerable<ClientList> SharedClientList { get; set; }
        public IEnumerable<MstAgentDealShareViewModel> SharedDealList { get; set; }
        public IEnumerable<ClientDetails> ClientDetails { get; set; }
        public IEnumerable<ClientDetails> GetSharedDeal { get; set; }
        public UserProfileView UserProfile { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<AgentDDl> AgentDropDown { get; set; }
        public IEnumerable<ClientDDl> ClientDropDown { get; set; }
        public IEnumerable<MstClientTypes> ClientTypeDropDown { get; set; }
        public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }
        public IEnumerable<SearchAutoCompleteAgent> AgentSearchAutoComplete { get; set; }

        //New Version Start
        public ClientSortingInfo ClientSortingInfo { get; set; }
        public ClientFilterInfo ClientFilterInfo { get; set; }
        public AdminDealsSortingInfo AdminDealsSortingInfo { get; set; }
        public AdminDealsFilterInfo AdminDealsFilterInfo { get; set; }
      
        //New Version End


    }

    public class ClientList
    {
        public string ClientID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Photos { get; set; }
        //New Version Start
        public string ClientType { get; set; }
        public string Year { get; set; }
        public string ClientTier { get; set; }
        public string Address { get; set; }
        public string Price { get; set; }
        public string Stage { get; set; }
        public string TransactionID { get; set; }
        public string AgentName { get; set; }
        public string AgentID { get; set; }
        //New Version End
    }

    public class ClientDetails
    {
        public string TransactionID { get; set; }
        public string AgentID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ClientType { get; set; }
        public string ClientID { get; set; }
        public string Status { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }

    public class UserProfileView
    {
        public string ClientID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserPhotoThumb { get; set; }
    }


    //New Version Start
    public class ClientSortingInfo
    {
        public string CurrentSort { get; set; }
        public string NameSort { get; set; }
        public string PhoneSort { get; set; }
        public string TierSort { get; set; }
        public string TypeSort { get; set; }
        public string StageSort { get; set; }
        public string YearSort { get; set; }
        public string TransactionIdSort { get; set; }
        public string AddressSort { get; set; }
    }

    public class ClientFilterInfo
    {
        public string SearchFilter { get; set; }
        public string YearFilter { get; set; }
        public string TypeFilter { get; set; }
        public string StageFilter { get; set; }
        public string TierFilter { get; set; }
    }

    public class AdminDealsSortingInfo
    {
        public string CurrentSort { get; set; }
        public string NameSort { get; set; }
        public string AgentNameSort { get; set; }
        public string PhoneSort { get; set; }
        public string TierSort { get; set; }
        public string TypeSort { get; set; }
        public string StageSort { get; set; }
        public string YearSort { get; set; }
        public string TransactionIdSort { get; set; }
        public string AddressSort { get; set; }
    }

    public class AdminDealsFilterInfo
    {
        public string SearchFilter { get; set; }
        public string AgentSearchFilter { get; set; }
        public string YearFilter { get; set; }
        public string TypeFilter { get; set; }
        public string StageFilter { get; set; }
        public string TierFilter { get; set; }
    }
    //New Version End
}
