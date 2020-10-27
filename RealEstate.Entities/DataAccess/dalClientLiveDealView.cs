using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalClientLiveDealView
    {
        public EFDBContext objDB = new EFDBContext();
        public IEnumerable<utblMstKeyInfoLink> MstKeyInfoLinkList
        {
            get
            {
                return objDB.utblMstKeyInfoLinks;
            }
        }

        public IEnumerable<MstClientLiveDealView> ClientDealGetPaged(int PageNo, int PageSize, string Email, out int TotalRows, string SearchTerm = "")
        {
            List<MstClientLiveDealView> objList = new List<MstClientLiveDealView>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }

            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parEmail = new SqlParameter("@Email", Email);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<MstClientLiveDealView>("udspMstClientLiveDealGetPaged @Email,@Start,@PageSize,@SearchTerm,@TotalCount out", parEmail, parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public string GetSharedEmail(string TransactionID,string Email, out string OutEmail)
        {
            string objResult;
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            var spOutput = new SqlParameter
            {
                ParameterName = "@OutEmail",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Size=50,
                Direction = System.Data.ParameterDirection.Output
            };
            objResult = objDB.Database.SqlQuery<string>("udspShareDealCheckShared @Email,@TransactionID,@OutEmail out", parEmail, parTransactionID,spOutput).FirstOrDefault();
            OutEmail = spOutput.Value.ToString();
            return objResult;
        }
        public IEnumerable<DealTimeline> ClientDealTimeline(string Email, string TransactionID)
        {
            List<DealTimeline> objList = new List<DealTimeline>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<DealTimeline>("udspMstClientDealTimelineSelect @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }
      
        //New Version
        public IEnumerable<DealTimeline> ClientDealTimelineVersion2(string Email, string TransactionID)
        {
            List<DealTimeline> objList = new List<DealTimeline>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<DealTimeline>("udspMstClientDealTimelineSelectVersion2 @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        //New Version
        public IEnumerable<DealTimeline> ClientDealTimelineCompleted(string Email, string TransactionID)
        {
            List<DealTimeline> objList = new List<DealTimeline>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<DealTimeline>("udspMstClientDealTimelineSelectCompleted @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        //New Version
        public IEnumerable<DealTimeline> ClientSellerDealTimelineVersion2(string Email, string TransactionID)
        {
            List<DealTimeline> objList = new List<DealTimeline>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<DealTimeline>("udspMstClientSellerDealTimelineSelectVersion2 @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        //New Version
        public IEnumerable<DealTimeline> ClientSellerDealTimelineCompleted(string Email, string TransactionID)
        {
            List<DealTimeline> objList = new List<DealTimeline>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<DealTimeline>("udspMstClientSellerDealTimelineSelectCompleted @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }


        public IEnumerable<DealTimeline> ClientDealTimelineNotPresent(string Email, string TransactionID)
        {
            List<DealTimeline> objList = new List<DealTimeline>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<DealTimeline>("udspMstClientDealTimelineNotPresentSelect @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }
        public IEnumerable<ChartData> ChartData(string Email, string TransactionID)
        {
            List<ChartData> objList = new List<ChartData>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<ChartData>("udspMstClientDealChartData @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }
        public IEnumerable<ClientDoc> ClientDoc(string Email, string TransactionID)
        {
            List<ClientDoc> objList = new List<ClientDoc>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<ClientDoc>("udspMstClientDocumentSelect @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        public ClosingDateView ClosingDate(string Email)
        {
            ClosingDateView objList = new ClosingDateView();
            var parEmail = new SqlParameter("@Email", Email);
            objList = objDB.Database.SqlQuery<ClosingDateView>("udspMstClientClosingDateSelect @Email", parEmail).FirstOrDefault();
            return objList;
        }
        public StatusPercentage Percentage(string Email,string TransactionID)
        {
            StatusPercentage objList = new StatusPercentage();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<StatusPercentage>("udspMstClientDealPercentageSelect @Email,@TransactionID", parEmail, parTransactionID).FirstOrDefault();
            return objList;
        }


        public MstUserAgentView AgentView(string TransactionID)
        {
            MstUserAgentView objList = new MstUserAgentView();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<MstUserAgentView>("udspUserAgentSelect @TransactionID", parTransactionID).FirstOrDefault();
            return objList;
        }

        public MstUserClientView ClientView(string TransactionID)
        {
            MstUserClientView objList = new MstUserClientView();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<MstUserClientView>("udspClientIDSelect @TransactionID", parTransactionID).FirstOrDefault();
            return objList;
        }

        public IEnumerable<MstClientCalenderView> GetCalenderList(string Email)
        {
            List<MstClientCalenderView> objList = new List<MstClientCalenderView>();
            var parEmail = new SqlParameter("@Email", Email);
            objList = objDB.Database.SqlQuery<MstClientCalenderView>("udspAppointmentClientCalenderSelect @Email", parEmail).ToList();
            return objList;
        }

        public IEnumerable<MstClientCalenderView> GetClientCalenderList(string AgentID,string ClientID)
        {
            List<MstClientCalenderView> objList = new List<MstClientCalenderView>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            var parClientID = new SqlParameter("@ClientID", ClientID);
            objList = objDB.Database.SqlQuery<MstClientCalenderView>("udspAppointmentAgentClientCalenderSelect @AgentID,@ClientID", parAgentID, parClientID).ToList();
            return objList;
        }

        public IEnumerable<MLSListing> MLSListing(string Email, string TransactionID)
        {
            List<MLSListing> objList = new List<MLSListing>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<MLSListing>("SELECT utblMstClosingDates.ClosingDate, utblMstClosingDates.MLSID, utblMstClosingDates.Address, utblMstClosingDates.UpdatedOn, utblMstClosingDates.URL, utblMstClosingDates.ListingTypeID FROM utblMstClosingDates INNER JOIN  utblMstListingTypes ON utblMstClosingDates.ListingTypeID = utblMstListingTypes.ListingTypeID WHERE Email= @Email and TransactionID=@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

 


        public IEnumerable<MstPhotoGalleryView> GetPhotoGalleryList(string Email, string TransactionID)
        {
            List<MstPhotoGalleryView> objList = new List<MstPhotoGalleryView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<MstPhotoGalleryView>("udspMstAgentPhotoGallery @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }
        public IEnumerable<DealVendorView> GetDealVendorList(string Email, string TransactionID)
        {
            List<DealVendorView> objList = new List<DealVendorView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@Transaction_Id", TransactionID);

            objList = objDB.Database.SqlQuery<DealVendorView>("udspGetDealVendors @Email,@Transaction_Id", parEmail, parTransactionID).ToList();
            return objList;
        }
        /// <summary>
        /// /Added by sonika 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="TransactionID"></param>
        /// <returns></returns>
        public IEnumerable<DealVendorView> GetDealVendorListNew(string Email, string TransactionID)
        {
            List<DealVendorView> objList = new List<DealVendorView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@Transaction_Id", TransactionID);

            objList = objDB.Database.SqlQuery<DealVendorView>("udspGetDealVendorsNew @Email,@Transaction_Id", parEmail, parTransactionID).ToList();
            return objList;
        }

        public ClientHomeGallery ClientHomeGallery(string ClientID, string TransactionID)
        {
            var parClientID = new SqlParameter("@ClientID", ClientID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            return objDB.Database.SqlQuery<ClientHomeGallery>("udspClientHomeGallery @ClientID,@TransactionID", parClientID, parTransactionID).FirstOrDefault();
        }
        public IEnumerable<ClientView> ClientMultipleDeal(string ClientID)
        {
            List<ClientView> objList = new List<ClientView>();
            var parClientID = new SqlParameter("@ClientID", ClientID);
            objList = objDB.Database.SqlQuery<ClientView>("udspdealClientMultipleList @ClientID", parClientID).ToList();
            return objList;
        }
        public IEnumerable<SharedClientView> SharedDeal(string ClientID)
        {
            List<SharedClientView> objList = new List<SharedClientView>();
            var parClientID = new SqlParameter("@ClientID", ClientID);
            objList = objDB.Database.SqlQuery<SharedClientView>("udspShareDealGetList @ClientID", parClientID).ToList();
            return objList;
        }

        public IEnumerable<ClientView> AgentMultipleDeal(string AgentID,string ClientID)
        {
            List<ClientView> objList = new List<ClientView>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            var parClientID = new SqlParameter("@ClientID", ClientID);
            objList = objDB.Database.SqlQuery<ClientView>("udspdealAgentMultipleList @AgentID,@ClientID", parAgentID, parClientID).ToList();
            return objList;
        }

        public IEnumerable<TransactionIDDL> TransactionDDL(string ClientID)
        {
            List<TransactionIDDL> objList = new List<TransactionIDDL>();
            var parClientID = new SqlParameter("@ClientID", ClientID);
            objList = objDB.Database.SqlQuery<TransactionIDDL>("udspMstClientTransactionDropDown @ClientID", parClientID).ToList();
            return objList;
        }

        public IEnumerable<CheckDeal> CheckDeal(string Email)
        {
            List<CheckDeal> objList = new List<CheckDeal>();
            var parEmail = new SqlParameter("@Email", Email);
            objList = objDB.Database.SqlQuery<CheckDeal>("select TransactionID from utblMstTrackDeals where email=@Email union select TransactionID from utblMstSellerTrackDeals where email=@Email", parEmail).ToList();
            return objList;
        }
        public IEnumerable<CheckCalendar> CheckCalendar(string Email)
        {
            List<CheckCalendar> objList = new List<CheckCalendar>();
            var parEmail = new SqlParameter("@Email", Email);
            objList = objDB.Database.SqlQuery<CheckCalendar>("select Id from utblMstAppointments where email=@Email", parEmail).ToList();
            return objList;
        }

        public string ShareTransaction(string TransactionID,string SharedWithEmail)
        {
           
            SPErrorViewModel objStatus = new SPErrorViewModel();
           
            
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);


            var parSharedWith = new SqlParameter("@SharedWith", SharedWithEmail);
            //objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspShareDealInsert @TransactionID,@SharedWith", parTransactionID, parSharedWith).FirstOrDefault();
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspShareDealInsertNew @TransactionID,@SharedWith", parTransactionID, parSharedWith).FirstOrDefault();

            return objStatus.ErrorCode + objStatus.ErrorMessage;


        }
        public string RemoveSharing(string TransactionID, string ClientID,out int ShareCount)
        {

            SPErrorViewModel objStatus = new SPErrorViewModel();


            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);


            var parClientID = new SqlParameter("@ClientID", ClientID);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspShareDealRemove @TransactionID,@ClientID,@TotalCount out", parTransactionID, parClientID,spOutput).FirstOrDefault();
            ShareCount = int.Parse(spOutput.Value.ToString());
            return objStatus.ErrorCode + objStatus.ErrorMessage;


        }
        public IEnumerable<SharedWith> GetSharedAccounts(string TransactionID)
        {
            List<SharedWith> objList = new List<SharedWith>();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<SharedWith>("udspMstClientSharedAccounts @TransactionID", parTransactionID).ToList();
            return objList;
        }
        public IEnumerable<string> GetAutoCompleteClientList(string searchTerm)
        {
            List<string> objList = new List<string>();
            var parSearchTerm = new SqlParameter("@SearchTerm", searchTerm);
            objList = objDB.Database.SqlQuery<string>("udspShareDealAutoCompleteClientList @SearchTerm", parSearchTerm).ToList();
            return objList;
        }


        public bool IsClient(string Email)
        {
            bool objCheckIsClient = new bool();
            var parClientEmail = new SqlParameter("@ClientEmail", Email);
            objCheckIsClient =objDB.Database.SqlQuery<bool>("udspMstCheckIsClient @ClientEmail", parClientEmail).FirstOrDefault();
            return objCheckIsClient;
        }

        public IEnumerable<Agenda> GetClientAppointments(string Email,string TransactionID)
        {
            List<Agenda> objList = new List<Agenda>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<Agenda>("udspAppointmentClientCalenderAgenda @Email,@TransactionID", parEmail,parTransactionID).ToList();
            return objList;
        }

        public string TransactionType(string TransactionID)
        {
            string Type = "";
            var parEmail = new SqlParameter("@TransactionID", TransactionID);
            Type = objDB.Database.SqlQuery<string>("select (select ClientType from utblMstClientypes where utblMstClientypes.ClientTypeID= utblMstTrackDealMasters.ClientTypeID)[Type] from utblMstTrackDealMasters where TransactionID=@TransactionID", parEmail).FirstOrDefault();
            return Type;
        }
    }
}
