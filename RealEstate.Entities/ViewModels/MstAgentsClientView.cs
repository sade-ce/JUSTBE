using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
   public class MstAgentsClientView
    {
       public IEnumerable<AgentsClient> ClientList { get; set; }
    }

    public class AgentsClient
    {
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ClientType { get; set; }
        //New Admin
        public string Image { get; set; }
        public string AgentName { get; set; }
        public string Year { get; set; }
        public string Stage { get; set; }
        public string ClientTier { get; set; }
        public string UserRole { get; set; }
    }
}
