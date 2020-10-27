using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstClientActivityLog
    {
        [Key]
        public int ActivityID { get; set; }
        public string ActivityTitle { get; set; }

        public string ClientID { get; set; }

        public string AgentID { get; set; }

        public string Remarks { get; set; }
        public string TrackingSource { get; set; }

        public DateTime TrackingDate { get; set; }



    }
}
