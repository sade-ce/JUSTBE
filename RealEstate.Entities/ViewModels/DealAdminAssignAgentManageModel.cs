using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;

namespace RealEstate.Entities.ViewModels
{
    public class DealAdminAssignAgentManageModel
    {
        public IEnumerable<MstDealClientList> ClientList { get; set; }
        public utblMstTrackDealMaster utblMstTrackDealMasters { get; set; }
        public IEnumerable<AgentDDl> AgentDropDown { get; set; }
        public IEnumerable<ClientDDl> ClientDropDown { get; set; }
        public IEnumerable<MstClientTypes> ClientTypeDropDown { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }

    }

    public class MstDealClientList
    {
        public string TransactionID { get; set; }
        public string ClientName { get; set; }
        public string ClientType { get; set; }
        public string Year { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string AgentName { get; set; }
        public string AgentID { get; set; }
        public string Photos { get; set; }
        public string ClientID { get; set; }
    }

    public class MstClientTypes
    {
        public int ClientTypeID { get; set; }
        public string ClientType { get; set; }
    }

    public class AgentDDl
    {
        public string Id { get; set; }
        public string AgentName { get; set; }
    }

    public class ClientDDl
    {
        public string Id { get; set; }
        public string ClientName { get; set; }
    }
}
