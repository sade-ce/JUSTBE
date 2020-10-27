using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstTaskSchedular
    {
        public IEnumerable<MstSelectTask>GetTask { get; set; }
    }

   public class MstSelectTask
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsContingency { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ClientName { get; set; }
        public string AgentName { get; set; }
        public string  AgentEmail { get; set; }
        public string AgentPhone { get; set; }
        public string AgentPhoto { get; set; }
        public bool IsClient { get; set; }


    }
}
