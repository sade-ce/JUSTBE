using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
    public class MstCalenderConfiguration
    {
        public utblMstCalenderConfiguration utblMstCalenderConfiguration { get; set; }
        public IEnumerable<utblMstStatus> MstStatusList { get; set; }
        public IEnumerable<utblMstCalenderConfiguration> MstCalenderConfigList { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public IEnumerable<CalenderConfigList> CalenderConfigList { get; set; }
    }

    public class CalenderConfigList
    {
        public long RowID { get; set; }
        public int CalenderConfigID { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public DateTime ConfigDate { get; set; }

    }
}
