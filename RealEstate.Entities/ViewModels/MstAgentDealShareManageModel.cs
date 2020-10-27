using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;

namespace RealEstate.Entities.ViewModels
{
    public class MstAgentDealShareManageModel
    {
        public utblMstAgentDealSharing utblMstAgentDealSharings { get; set; }
        public IEnumerable<AgentDDl> AgentDropDown { get; set; }
    }
}
