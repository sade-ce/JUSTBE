using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientRespondAdminManageModel
    {
        public IEnumerable<MstClientRespondView> MstRespondList { get; set; }
        public utblMstClientTrackDealDoc utblMstClientTrackDealDoc { get; set; }
        public IEnumerable<utblMstClientTrackDealDoc> DocList { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
