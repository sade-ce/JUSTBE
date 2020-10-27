using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstActivityLogViewModel
    {
        public IEnumerable<utblMstClientActivityLog> ActivityLog { get; set; }
        public IEnumerable<MstViewEmailActivityLog> ActivityLogView { get; set; }

    }

    public class MstViewEmailActivityLog
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ActivityTitle { get; set; }
        public string Remarks { get; set; }
        public string TrackingSource { get; set; }
        public DateTime TrackingDate { get; set; }
        //New Version
        //public string TrackingDateNew { get; set; }
    }

}
