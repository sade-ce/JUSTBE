using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
    public class MstClosingConfigurationModel
    {
        public utblMstClosingConfigutation utblMstClosingConfigutation { get; set; }
        public IEnumerable<utblMstStatus> MstStatusList { get; set; }

        public IEnumerable<GetClosingConfig> GetClosingConfig { get; set; }

    }

    public class GetClosingConfig
    {
        public int StatusID { get; set; }
        public string Status { get; set; }
        public DateTime ConfigDate { get; set; }


    }
}
