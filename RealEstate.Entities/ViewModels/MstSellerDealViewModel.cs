using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstSellerDealViewModel
    {
        public IEnumerable<MstSellerStatusDDL> MstStatusDDL { get; set; }
        public IEnumerable<MstClientDDL> MstClientDDL { get; set; }
        public IEnumerable<utblMstSellerTrackDeal> MstTrackDealList { get; set; }
        public utblMstSellerTrackDeal utblMstSellerTrackDeal { get; set; }
        public IEnumerable<utblMstSellerTrackDealDoc> MstLiveDealDocList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public utblMstSellerTrackDealDoc utblMstSellerTrackDealDoc { get; set; }
        public IEnumerable<MstSellerLiveDealView> MstDealView { get; set; }

    }

    public class MstSellerLiveDealView
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string SellerTrackingID { get; set; }
        public string Status { get; set; }

    }
    public class MstSellerStatusDDL
    {
        public int SellerStatusID { get; set; }
        public string Status { get; set; }
    }

    
}
