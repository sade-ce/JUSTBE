using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class TeamMemberViewModel
    {

        public TeamMembers TeamMembers { get; set; }
        public TeamMemberView TeamMemberView { get; set; }
        public IEnumerable<TeamMemberView> TeamMemberList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public TeamMemberPaging TeamMemberPaging { get; set; }
    }
}
