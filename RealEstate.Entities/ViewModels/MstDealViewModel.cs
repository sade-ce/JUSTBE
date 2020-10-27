using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
    public class MstDealViewModel
    {
        public IEnumerable<MstStatusDDL> MstStatusDDL { get; set; }
        public IEnumerable<MstClientDDL> MstClientDDL { get; set; }
        public IEnumerable<utblMstTrackDeal> MstTrackDealList { get; set; }
        public utblMstTrackDeal utblMstTrackDeal { get; set; }
        public IEnumerable<utblMstTrackDealDoc> MstLiveDealDocList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public utblMstTrackDealDoc utblMstTrackDealDoc { get; set; }
        public IEnumerable<MstLiveDealView> MstDealView { get; set; }

    }
    public class MstLiveDealView
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TrackingID { get; set; }
        public string Status { get; set; }

    }
    public class MstStatusDDL
    {
        public int StatusID { get; set; }
        public string Status { get; set; }
    }

    public class MstClientDDL
    {
        public string Email { get; set; }
        public string ClientName { get; set; }
    }
}
