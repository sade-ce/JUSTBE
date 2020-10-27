using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientDealCreateManageModel
    {
        public UserProfileView UserProfile { get; set; }
        public MstInfoView UserInfo { get; set; }
        public MstInfoViewV2 UserInfoV2 { get; set; }
        public IEnumerable<StatusDropdown> StatusList { get; set; }
        public ClosingConfig ClosingConfig { get; set; }
        public SellerClosingConfig SellerClosingConfig { get; set; }
        public TrackClosingDate TrackClosingDate { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public utblMstTrackDealMaster utblMstTrackDealMasters { get; set; }
        public utblMstTrackDeal utblMstTrackDeal { get; set; }
        public IEnumerable<utblMstTrackDeal> TrackDealList { get; set; }
        public utblMstTrackDealDoc utblMstTrackDealDoc { get; set; }
        public utblMstClosingDate utblMstClosingDate { get; set; }
        public IEnumerable<utblMstTrackDealDoc> MstLiveDealDocList { get; set; }
        public IEnumerable<TrackDealStatusList> TrackDealStatusList { get; set; }
        public IEnumerable<utblMstCalenderConfiguration> MstCalenderConfigList { get; set; }
        public IEnumerable<MstBuyerDocList> MstBuyerDocList { get; set; }
        public utblMstBuyerDocument utblMstBuyerDocument { get; set; }
        public IEnumerable<MstBuyerDocumentListView> MstBuyerDocumentListView { get; set; }
        public IEnumerable<MstSellerDocumentListView> MstSellerDocumentListView { get; set; }
        public MstClosingConfigViewModel MstClosingConfig { get; set; }
        public MstAgentClientNameSelect MstAgentClientNameSelect { get; set; }
        public UserStatusSelect MstUserStatusSelect { get; set; }

        public List<UserStatusSelect> BulkEmailStatus { get; set; }
        public TrackDealMasterView TrackDealMasterView { get; set; }
        public List<MstContingenciesView> ContingenciesData { get; set; }
        public List<MstContingenciesViewVersion2> ContingenciesDataVersion2 { get; set; }
        public List<MstSellerContingenciesViewVersion2> SellerContingenciesDataVersion2 { get; set; }
        public MstContingenciesViewVersion2 ContingenciesVersion2 { get; set; }
        public MstContingenciesView SettlementDate { get; set; }

        public utblMstSellerDocument utblMstSellerDocuments { get; set; }
        public TrackClosingDateID TrackClosingDateID { get; set; }
        public TrackRatifiedDate TrackRatifiedDate { get; set; }
        public MstRatifiedEditViewModel RatifiedOfferView { get; set; }

        public ClientDetails TransactionDetails { get; set; }
        public IEnumerable<ClientDetails> ClientDetails { get; set; }
        public IEnumerable<ClientDealDocumentsView> ClientDealDocumentList { get; set; }//Client Document
        //New Version
        public DealTracker DealTracker { get; set; }
        public IEnumerable<DealTracker> DealTrackerList { get; set; }
        public IEnumerable<CondoStepType> CondoStepDDL { get; set; }
        public IEnumerable<SearchDocumentAutoCompleteViewModel> DocumentAutoComplete { get; set; }
        public IEnumerable<CheckListItems> CheckListItems { get; set; }
    }

    public class SearchDocumentAutoCompleteViewModel
    {
        public string searchResult { get; set; }
        public string DocID { get; set; }
    }

    public class CheckListItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Stage { get; set; }
    }

    public class CondoStepType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ClientHomeGallery
    {
        public string HomePhotoID { get; set; }
        public string TransactionID { get; set; }
        public string FileExt { get; set; }
        public string Description { get; set; }
        public string ClientID { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
   
    public class MstInfoView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
      
    }
    public class MstInfoViewV2
    {

  

     
        public string Email { get; set; }
   
        public string Name { get; set; }

        public string LastName { get; set; }
 


      
        public string PhoneNumber { get; set; }

 



        public string UserPhotoThumb { get; set; }
        public string UserPhotoNormal { get; set; }

 
        public string Id { get; set; }

        //New Version
        public string ClientType { get; set; }

    }
    public class StatusDropdown
    {
        public int StatusID { get; set; }
        public string Status { get; set; }
        public bool IsContingencies { get; set; }
        public long RowID { get; set; }
    }
    public class ClosingConfig
    {
        public int StatusID { get; set; }
        public string Status { get; set; }
    }
    public class TrackClosingDate
    {
        public DateTime ClosingDate { get; set; }
    }


    public class TrackRatifiedDate
    {
        public DateTime UpdatedOn { get; set; }
    }


    public class TrackClosingDateID
    {
        public int ClosingDateID { get; set; }
    }

    public class TrackDealStatusList
    {
        public string TrackingID { get; set; }
        public string TransactionID { get; set; }
        public string ClientID { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public bool IsContingencies { get; set; }
        public bool IsApplicable { get; set; }
    }

    //New Version
    public class DealTracker
    {
        public string TrackingID { get; set; }
        public string SellerTrackingID { get; set; }
        public string TransactionID { get; set; }
        public string ClientID { get; set; }
        public int StatusID { get; set; }
        public int SellerStatusID { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public bool IsContingencies { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsShowClient { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdatedOn { get; set; }
        public string dateupdated { get; set; }
        public int? NoOfDays { get; set; }
        public string AdditionalInfo { get; set; }
        public string Time { get; set; }
        public bool IsCompleted { get; set; }
        public string CheckListItems { get; set; }
        [AllowHtml]
        public string Notes { get; set; }
        public decimal? DealPrice { get; set; }
    }

    public class MstBuyerDocList
    {
        public int DocID { get; set; }
        public string Title { get; set; }
    }

    public class MstBuyerDocumentListView
    {
        public string DealTrackDocID { get; set; }
        public string TransactionID { get; set; }
        public string TrackingID { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public int DocID { get; set; }
        public string TrackDocFile { get; set; }
        public string UpdatedOn { get; set; }
    }

    public class UserStatusSelect
    {
        public string ClientName { get; set; }
        public string Status { get; set; }
    }
    public class TrackDealMasterView
    {
        public string AgentID { get; set; }

        public string TransactionID { get; set; }
        public string ClientID { get; set; }

    }
}
