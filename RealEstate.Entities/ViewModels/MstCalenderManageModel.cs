using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;

namespace RealEstate.Entities.ViewModels
{
    public class MstCalenderManageModel
    {
        public IEnumerable<Events> CalEvents { get; set; }
        public UserProfileView UserProfile { get; set; }
        public TID TransactionView { get; set; }
        public AID AgentView { get; set; }
        public IEnumerable<utblMstAppointment> MstAppointmentList { get; set; }
        public IEnumerable<AppointmentViewModel> AppointmentViewModelList { get; set; }
        public utblMstAppointment utblMstAppointment { get; set; }
        public IEnumerable<MstClientCalenderView> data { get; set; }

        public MstAgentClientNameSelect MstAgentClientNameSelect { get; set; }

        public MstAdminClientNameSelect MstAdminClientNameSelect { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<AgentResourceView> AgentResourceS { get; set; }
        public IEnumerable<MstViewEmailActivityLog> ClientActivity { get; set; }
        public IEnumerable<SearchAutoCompleteAgent> AgentSearchAutoComplete { get; set; }
    }

    public class MstAgentClientNameSelect
    {
        public string AgentName { get; set; }
        public string AgentEmail { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string AgentPhone { get; set; }
    }

    public class MstAdminClientNameSelect
    {
        public string AgentName { get; set; }
        public string ClientName { get; set; }
        public string AgentPhone { get; set; }

    }

    public class TID
    {
        public string TransactionID { get; set; }
    }

    public class Events
    {
        public int CalEventID { get; set; }
        public string EventText { get; set; }
    }

    public class AID
    {
        public string AgentID { get; set; }
    }
    //New Version Agent Graph
    public partial class GraphPoints
    {
        public DateTime x { get; set; }
        public int y { get; set; }
        public int TotalCount { get; set; }
    }
}
