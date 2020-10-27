using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientDealRespondManageModel
    {
        public MstClientDealRespondViewModel MstClientRespondView { get; set; }

        public IEnumerable<utblMstTrackDealDoc> MstClientTrackDocList { get; set; }
        public utblMstClientTrackDealDoc utblMstClientTrackDealDoc { get; set; }
        public IEnumerable<utblMstClientTrackDealDoc> DocList { get; set; }
    }
}
