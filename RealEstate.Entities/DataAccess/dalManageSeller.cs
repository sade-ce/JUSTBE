using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
using RealEstate.Entities.Utility;

namespace RealEstate.Entities.DataAccess
{
    public class dalManageSeller
    {
        private EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public UserProfileView GetUserDetails(string ClientID)
        {
            var model = new UserProfileView();
            var parClientID = new SqlParameter("@ClientID", ClientID);
            model = objDB.Database.SqlQuery<UserProfileView>("udspDealUserProfileSelect @ClientID", parClientID).FirstOrDefault();
            return model;
        }
        public MstInfoView MstInfoView(string Email)
        {
            var parEmail = new SqlParameter("@Email", Email);
            return objDB.Database.SqlQuery<MstInfoView>("SELECT  Name +' '+LastName as Name,Phone,Email FROM AspNetUsers where Email= @Email", parEmail).FirstOrDefault();
        }

        public IEnumerable<SellerStatusDropdown> StatusList(string Email, string TransactionID)
        {
            List<SellerStatusDropdown> objList = new List<SellerStatusDropdown>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<SellerStatusDropdown>("udspClientSellerStatusList @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        public SellerClosingConfig GetClosingConfig()
        {
            return objDB.Database.SqlQuery<SellerClosingConfig>("SELECT SellerStatusID FROM utblMstSellerClosingConfigutations").FirstOrDefault();
        }
        public IEnumerable<SellerTrackDealStatusList> TrackDealStatusList(string Email, string TransactionID)
        {
            List<SellerTrackDealStatusList> objList = new List<SellerTrackDealStatusList>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<SellerTrackDealStatusList>("udspTrackSellerDealStatusList @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        public string Save(utblMstSellerTrackDeal Item)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.SellerTrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDeals");
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", Item.SellerTrackingID);
            var parSellerStatusID = new SqlParameter("@SellerStatusID", Item.SellerStatusID);
            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            var parIsApplicable = new SqlParameter("@IsApplicable", Item.IsApplicable);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", Item.UpdatedOn);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerInsert @SellerTrackingID, @SellerStatusID,@ClientID,@TransactionID,@Email,@IsApplicable,@UpdatedOn", parSellerTrackingID, parSellerStatusID, parClientID, parTransactionID, parEmail, parIsApplicable, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string SaveNew(utblMstSellerTrackDeal Item, string AgentEmail, int NoOfDays, bool IsContigency, DateTime RatifiedDate)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.SellerTrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDeals");
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", Item.SellerTrackingID);
            var parSellerStatusID = new SqlParameter("@SellerStatusID", Item.SellerStatusID);
            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            var parIsApplicable = new SqlParameter("@IsApplicable", Item.IsApplicable);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", "");


            if (IsContigency == true)
            {
                if (Item.IsApplicable == true)
                {
                    parUpdatedOn = new SqlParameter("@UpdatedOn", RatifiedDate.AddDays(NoOfDays).AddHours(18));

                }
                else
                {
                    parUpdatedOn = new SqlParameter("@UpdatedOn", Item.UpdatedOn);

                }
            }

            else
            {
                parUpdatedOn = new SqlParameter("@UpdatedOn", Item.UpdatedOn);

            }




            //Add steps to appointments
            if (IsContigency == true && Item.IsApplicable == false)
            {
            }

            else
            {

                if (Item.SellerStatusID != 15)//Welcome Status
                {
                    var parAppointmentTrackingID = new SqlParameter("@TrackingID", Item.SellerTrackingID);
                    var parAppointmentTTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);//Added by Neha
                    var parClientEmail = new SqlParameter("@Email", Item.Email);
                    var parAgentEmail = new SqlParameter("@AgentEmail", AgentEmail);
                    var parIsContingency = new SqlParameter("@IsContingency", Item.IsApplicable);
                    var parDescription = new SqlParameter("@StatusID", Item.SellerStatusID);
                    var parcolor = new SqlParameter("@Color", "#962e06");
                    if (Item.IsApplicable == true)
                    {

                        var parStartDate = new SqlParameter("@StartDate", RatifiedDate.AddDays(NoOfDays).AddHours(17));
                        var parEndDate = new SqlParameter("@EndDate", RatifiedDate.AddDays(NoOfDays).AddHours(18));
                        objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCalenderContingencyInsertNewSeller @Email,@AgentEmail, @IsContingency,@StatusID,@StartDate,@EndDate,@Color,@TrackingID,@TransactionID", parClientEmail, parAgentEmail, parIsContingency, parDescription, parStartDate, parEndDate, parcolor, parAppointmentTrackingID, parAppointmentTTransactionID).FirstOrDefault();

                    }
                    else
                    {
                        var parStartDate = new SqlParameter("@StartDate", Item.UpdatedOn.AddHours(17));
                        var parEndDate = new SqlParameter("@EndDate", Item.UpdatedOn.AddHours(18));
                        objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCalenderContingencyInsertNewSeller @Email,@AgentEmail, @IsContingency,@StatusID,@StartDate,@EndDate,@Color,@TrackingID,@TransactionID", parClientEmail, parAgentEmail, parIsContingency, parDescription, parStartDate, parEndDate, parcolor, parAppointmentTrackingID, parAppointmentTTransactionID).FirstOrDefault();

                    }
                }

            }


            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerInsert @SellerTrackingID, @SellerStatusID,@ClientID,@TransactionID,@Email,@IsApplicable,@UpdatedOn", parSellerTrackingID, parSellerStatusID, parClientID, parTransactionID, parEmail, parIsApplicable, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IsDealRatified IsDealRatified(string Email, string TransactionID) //Added By Neha
        {
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            return objDB.Database.SqlQuery<IsDealRatified>("SELECT UpdatedOn[RatifiedDate] FROM utblMstSellerTrackDeals where Email = @Email and TransactionID=@TransactionID and SellerStatusID=8", parEmail, parTransactionID).FirstOrDefault();
        }

        public bool emailPreferences(string Email)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var result = objDB.Database.SqlQuery<bool>("select StatusTimeline from AspNetUsers where UserName=@Email", parEmail).FirstOrDefault();
            return result;
        }
        public bool GetContigency(int SellerStatusId)
        {
            var parSellerStatusId = new SqlParameter("@SellerStatusId", SellerStatusId);
            var result = objDB.Database.SqlQuery<bool>("select IsContingencies from utblMstSellerStatus where SellerStatusId=@SellerStatusId", parSellerStatusId).FirstOrDefault();
            return result;
        }
        public DateTime GetRatifiedDate(string TransactionId)
        {
            var parSellerStatusId = new SqlParameter("@TransactionId", TransactionId);
            var result = objDB.Database.SqlQuery<DateTime>("select UpdatedOn from utblMstSellerTrackDeals where TransactionId=@TransactionId and SellerStatusId=8", parSellerStatusId).FirstOrDefault();
            return result;
        }
        public UserStatusSelect GetClientStatusfromEmail(string Email, int SellerStatusID)
        {
            UserStatusSelect obj = new UserStatusSelect();
            var parEmail = new SqlParameter("@Email", Email);
            var parSellerStatusID = new SqlParameter("@SellerStatusID", SellerStatusID);

            obj = objDB.Database.SqlQuery<UserStatusSelect>("udspUserClientSellerNameSelect @Email,@SellerStatusID", parEmail, parSellerStatusID).FirstOrDefault();
            return obj;
        }
        public MstAgentClientNameSelect GetNameEmail(string Email)
        {
            MstAgentClientNameSelect obj = new MstAgentClientNameSelect();
            var parEmail = new SqlParameter("@Email", Email);
            obj = objDB.Database.SqlQuery<MstAgentClientNameSelect>("udspUserAgentNameSelect @Email", parEmail).FirstOrDefault();
            return obj;
        }
        public TrackClosingDate TrackClosingDate(string Email, string TransactionID)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", Email);

            return objDB.Database.SqlQuery<TrackClosingDate>("SELECT ClosingDate FROM utblMstClosingDates where Email = @Email and TransactionID=@TransactionID", parEmail, parTransactionID).FirstOrDefault();
        }
        public string RemoveDeal(string SellerTrackingID, string Email, int SellerStatusID, string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", SellerTrackingID);
            var parEmail = new SqlParameter("@Email", Email);
            var parSellerStatusID = new SqlParameter("@SellerStatusID", SellerStatusID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstLiveSellerDealDelete @SellerTrackingID,@Email,@SellerStatusID,@TransactionID", parSellerTrackingID, parEmail, parSellerStatusID, parTransactionID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public IEnumerable<MstListingType> ListingTypeDDL()
        {
            List<MstListingType> objList = new List<MstListingType>();
            objList = objDB.Database.SqlQuery<MstListingType>("select ListingTypeID,ListingType from utblMstListingTypes").ToList();
            return objList;
        }
        public string SaveClosingConfiguration(utblMstClosingDate Item, int SellerStatusID, string TransactionID)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parMLSID = new SqlParameter("@MLSID", "");
            var parURL = new SqlParameter("@URL", "");
            var SellerTrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDeals");
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", SellerTrackingID);
            var parSellerStatusID = new SqlParameter("@SellerStatusID", SellerStatusID);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parClosingDate = new SqlParameter("@ClosingDate", Item.ClosingDate);
            if (Item.MLSID != null)
                parMLSID = new SqlParameter("@MLSID", Item.MLSID);
            var parListingTypeID = new SqlParameter("@ListingTypeID", Item.ListingTypeID);
            var parAddress = new SqlParameter("@Address", Item.Address);
            if (Item.URL != null)
                parURL = new SqlParameter("@URL", Item.URL);
            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerClosingDateConfigurationInsert @SellerTrackingID, @SellerStatusID,@Email,@ClosingDate,@MLSID,@ListingTypeID,@Address,@URL,@ClientID,@TransactionID,@UpdatedOn", parSellerTrackingID, parSellerStatusID, parEmail, parClosingDate, parMLSID, parListingTypeID, parAddress, parURL, parClientID, parTransactionID, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public string GetTrackingID(string Email, string TransactionID)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            return objDB.Database.SqlQuery<string>("select MAX(SellerTrackingID) from utblMstSellerTrackDeals where Email = @Email and TransactionID=@TransactionID", parEmail, parTransactionID).FirstOrDefault();
        }
        public utblMstSellerTrackDeal GetLiveDealDetailsByID(string ID, string TransactionID)
        {
            utblMstSellerTrackDeal obj = new utblMstSellerTrackDeal();
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", ID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            obj = objDB.Database.SqlQuery<utblMstSellerTrackDeal>("udspMstSellerDocumentFirstRowSelect @SellerTrackingID,@TransactionID", parSellerTrackingID, parTransactionID).FirstOrDefault();
            return obj;

        }

        public IEnumerable<MstSellerDocumentListView> SellerDocumentListView(string Email, string TransactionID)
        {
            List<MstSellerDocumentListView> objList = new List<MstSellerDocumentListView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<MstSellerDocumentListView>("udspMstSellerDocumentSelect @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        public IEnumerable<MstSellerDocList> SellerDocumentList(string Email, string TransactionID)
        {
            List<MstSellerDocList> objList = new List<MstSellerDocList>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<MstSellerDocList>("udspSellerDocumentList @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        public string SaveSellerDoc(utblMstSellerTrackDealDoc ItemData)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            ItemData.SellerDealDocID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDealDocs");
            var parSellerDealDocID = new SqlParameter("@SellerDealDocID", ItemData.SellerDealDocID);
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", ItemData.SellerTrackingID);
            var parTransactionID = new SqlParameter("@TransactionID", ItemData.TransactionID);
            var parSellerDocID = new SqlParameter("@SellerDocID", ItemData.SellerDocID);
            var parSellerStatusID = new SqlParameter("@SellerStatusID", ItemData.SellerStatusID);
            var parTrackDocFile = new SqlParameter("@TrackDocFile", ItemData.TrackDocFile ?? "");
            var parEmail = new SqlParameter("@Email", ItemData.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerDealDocInsert @SellerDealDocID, @SellerTrackingID,@TransactionID,@SellerDocID,@SellerStatusID, @TrackDocFile,@Email,@UpdatedOn", parSellerDealDocID, parSellerTrackingID, parTransactionID, parSellerDocID, parSellerStatusID, parTrackDocFile, parEmail, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }



        public IEnumerable<utblMstSellerTrackDealDoc> GetSellerDealDocList(string ID)
        {
            List<utblMstSellerTrackDealDoc> objList = new List<utblMstSellerTrackDealDoc>();
            objList = objDB.utblMstSellerTrackDealDocs.Where(x => x.SellerTrackingID == ID).ToList();
            return objList;
        }

        //submitting  seller offer

        public string SubmittingOffer(MstSellerOffer ItemData)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            ItemData.utblMstSellerCounterOffer.CounterOfferID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerCounterOffers");
            var parCounterOfferID = new SqlParameter("@CounterOfferID", ItemData.utblMstSellerCounterOffer.CounterOfferID);
            var parTransactionID = new SqlParameter("@TransactionID", ItemData.utblMstSellerCounterOffer.TransactionID);
            var parSellerID = new SqlParameter("@SellerID", ItemData.utblMstSellerCounterOffer.SellerID);
            var parBuyerName = new SqlParameter("@BuyerName", ItemData.utblMstPotentialBuyer.BuyerName);
            var parBuyerEmail = new SqlParameter("@BuyerEmail", ItemData.utblMstPotentialBuyer.BuyerEmail);
            var parBuyerPhone = new SqlParameter("@BuyerPhone", ItemData.utblMstPotentialBuyer.BuyerPhone);
            var parBuyerAddress = new SqlParameter("@BuyerAddress", ItemData.utblMstPotentialBuyer.BuyerAddress);
            var parStatus = new SqlParameter("@Status", ItemData.utblMstSellerCounterOffer.Status);
            var parDescription = new SqlParameter("@Description", ItemData.utblMstSellerCounterOffer.Description);
            var parPrice = new SqlParameter("@Price", ItemData.utblMstSellerCounterOffer.Price);
            var parClientTypeID = new SqlParameter("@ClientTypeID", 1);
            var parCounteredOn = new SqlParameter("@CounteredOn", System.DateTime.Now);
            var parIsLatest = new SqlParameter("@IsLatest", true);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerOfferInsert @CounterOfferID,@TransactionID,@SellerID,@BuyerName,@BuyerEmail,@BuyerPhone,@BuyerAddress,@Status,@Description,@Price,@ClientTypeID,@CounteredOn,@IsLatest,@UpdatedOn", parCounterOfferID, parTransactionID, parSellerID, parBuyerName, parBuyerEmail, parBuyerPhone, parBuyerAddress, parStatus, parDescription, parPrice, parClientTypeID, parCounteredOn, parIsLatest, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public IEnumerable<ReceivedOffer> OfferList(string TransactionID)
        {
            List<ReceivedOffer> objList = new List<ReceivedOffer>();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<ReceivedOffer>("udspMstbuyerofferselect @TransactionID", parTransactionID).ToList();
            return objList;
        }
        //Home gallery
        public IEnumerable<HomeGalleryView> GetHomeGalleryList(string Email, string TransactionID)
        {
            List<HomeGalleryView> objList = new List<HomeGalleryView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<HomeGalleryView>("udspMstClientHomeGallery @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }
        //Save Home Gallery

        public string SaveHomeGallery(utblMstClientHomeGallerie Item)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.HomePhotoID = CurrentYear + objUtil.generateUniqueCode("utblMstClientHomeGalleries");
            var parHomePhotoID = new SqlParameter("@HomePhotoID", Item.HomePhotoID);
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            var parFileExt = new SqlParameter("@FileExt", Item.FileExt);
            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parDescription = new SqlParameter("@Description", Item.Description ?? "");
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstClientHomeGalleryInsert @HomePhotoID,@TransactionID,@FileExt,@ClientID,@Description,@Email,@UpdatedOn", parHomePhotoID, parTransactionID, parFileExt, parClientID, parDescription, parEmail, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        //Delete Home Gallery
        public utblMstClientHomeGallerie GetHomeGalleryByID(string ID)
        {
            utblMstClientHomeGallerie objItem = objDB.utblMstClientHomeGalleries.FirstOrDefault(p => p.HomePhotoID == ID);
            return objItem;
        }

        public string DeleteGallery(string HomePhotoID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parHomePhotoID = new SqlParameter("@HomePhotoID", HomePhotoID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstClientHomeGalleryDelete @HomePhotoID", parHomePhotoID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        //remove seller document

        public utblMstSellerTrackDealDoc GetDocDetailsByID(string SellerDealDocID)
        {
            utblMstSellerTrackDealDoc objItem = objDB.utblMstSellerTrackDealDocs.FirstOrDefault(p => p.SellerDealDocID == SellerDealDocID);
            return objItem;
        }

        public string DeleteLiveDealDocument(string SellerDealDocID, string SellerTrackingID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parSellerDealDocID = new SqlParameter("@SellerDealDocID", SellerDealDocID);
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", SellerTrackingID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerDocumentDelete @SellerDealDocID,@SellerTrackingID", parSellerDealDocID, parSellerTrackingID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    }
}
