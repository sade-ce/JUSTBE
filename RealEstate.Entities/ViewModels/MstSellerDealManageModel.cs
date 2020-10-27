using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstSellerDealManageModel
    {
        public MstInfoView UserInfo { get; set; }
        public UserProfileView UserProfile { get; set; }

        public IEnumerable<SellerStatusDropdown> StatusList { get; set; }
        public SellerClosingConfig ClosingConfig { get; set; }
        public TrackClosingDate TrackClosingDate { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public utblMstTrackDealMaster utblMstTrackDealMasters { get; set; }
        public utblMstSellerTrackDeal utblMstSellerTrackDeal { get; set; }
        public IEnumerable<utblMstSellerTrackDeal> TrackDealList { get; set; }
        public utblMstSellerTrackDealDoc utblMstSellerTrackDealDoc { get; set; }
        public utblMstClosingDate utblMstClosingDate { get; set; }
        public IEnumerable<utblMstSellerTrackDealDoc> MstLiveDealDocList { get; set; }


        public IEnumerable<SellerTrackDealStatusList> TrackDealStatusList { get; set; }

        public IEnumerable<utblMstCalenderConfiguration> MstCalenderConfigList { get; set; }

        public IEnumerable<MstSellerDocList> MstSellerDocList { get; set; }
        public utblMstBuyerDocument utblMstBuyerDocument { get; set; }

        public IEnumerable<MstSellerDocumentListView> MstSellerDocumentList { get; set; }

        public MstClosingConfigViewModel MstClosingConfig { get; set; }
        public MstAgentClientNameSelect MstAgentClientNameSelect { get; set; }

        public UserStatusSelect MstUserStatusSelect { get; set; }
        public utblMstSellerDocument utblMstSellerDocuments { get; set; }

        public TrackDealMasterView TrackDealMasterView { get; set; }

        public ClientDetails TransactionDetails { get; set; }

        public IsDealRatified IsDealRatified { get; set; }
    }

    public class SellerStatusDropdown
    {
        public int SellerStatusID { get; set; }
        public string Status { get; set; }
        public bool IsContingencies { get; set; }
        public long RowID { get; set; }
    }

    public class SellerClosingConfig
    {
        public int SellerStatusID { get; set; }
        public string Status { get; set; }
    }

    public class SellerTrackDealStatusList
    {
        public string SellerTrackingID { get; set; }
        public string TransactionID { get; set; }
        public string ClientID { get; set; }
        public string AgentID { get; set; }
        public int SellerStatusID { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public bool IsApplicable { get; set; }

        public bool IsContingencies { get; set; }
    }

    public class MstSellerDocumentListView
    {
        public string SellerDealDocID { get; set; }
        public string TransactionID { get; set; }
        public string SellerTrackingID { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public int SellerDocID { get; set; }
        public string TrackDocFile { get; set; }
        public string UpdatedOn { get; set; }
    }

    public class MstSellerDocList
    {
        public int SellerDocID { get; set; }
        public string Title { get; set; }
    }

    public class IsDealRatified
    {
        public DateTime RatifiedDate { get; set; }
    }

}
