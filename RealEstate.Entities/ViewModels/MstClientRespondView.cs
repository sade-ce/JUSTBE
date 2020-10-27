using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientRespondView
    {
        public string TrackingID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime ClientRespondDate { get; set; }

    }
}
