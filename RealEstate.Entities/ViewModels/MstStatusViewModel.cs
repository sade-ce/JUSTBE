using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{

    public class MstStatusViewModel
    {
        public utblMstStatus utblMstStatus { get; set; }
        public utblMstSellerStatus utblMstSellerStatus { get; set; }
        public IEnumerable<utblMstStatus> MstStatusList{ get; set; }
        public IEnumerable<utblMstSellerStatus> MstSellerStatusList { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
