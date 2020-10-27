using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Models
{
    public class UserListViewModel
    {
        public IEnumerable<ApplicationUser> UserList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public UserSortingInfo UserSortingInfo { get; set; }
        public UserListFiterInfo UserListFiterInfo { get; set; }
        public UserAdminSortingInfo UserAdminSortingInfo { get; set; }
        public UserAdminListFiterInfo UserAdminListFiterInfo { get; set; }
        public MstAgentsClientView AgentClientView { get; set; }
        public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }
        public IEnumerable<SearchAutoCompleteNeighborhod> NeighborhodSearchAutoComplete { get; set; }
        public IEnumerable<SearchAutoCompleteClient> ClientSearchAutoComplete { get; set; }

    }


    //New Version Start
    public class UserSortingInfo
    {
        public string CurrentSort { get; set; }
        public string NameSort { get; set; }
        public string PhoneSort { get; set; }
        public string TierSort { get; set; }
        public string TypeSort { get; set; }
        public string StageSort { get; set; }
        public string YearSort { get; set; }
    }

    public class UserListFiterInfo
    {
        public string SearchFilter { get; set; }
        public string YearFilter { get; set; }
        public string TypeFilter { get; set; }
        public string StageFilter { get; set; }
        public string TierFilter { get; set; }
    }

    public class UserAdminSortingInfo
    {
        public string CurrentSort { get; set; }
        public string NameSort { get; set; }
        public string AgentNameSort { get; set; }
        public string RoleSort { get; set; }
        public string PhoneSort { get; set; }
        public string TierSort { get; set; }
        public string TypeSort { get; set; }
        public string StageSort { get; set; }
        public string YearSort { get; set; }

    }

    public class UserAdminListFiterInfo
    {
        public string SearchFilter { get; set; }
        public string YearFilter { get; set; }
        public string TypeFilter { get; set; }
        public string StageFilter { get; set; }
        public string TierFilter { get; set; }
        public string UserSearchFilter { get; set; }
        public string AgentSearchFilter { get; set; }
    }
    //New Version End
}