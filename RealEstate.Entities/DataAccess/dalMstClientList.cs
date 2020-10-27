using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace RealEstate.Entities.DataAccess
{
    public class dalMstClientList
    {
        private EFDBContext objDB = new EFDBContext();

        public IEnumerable<MstClientListView> ClientPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<MstClientListView> objList = new List<MstClientListView>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }

            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<MstClientListView>("udspMstClientList @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        public IEnumerable<MstClientListView> AgentClientPaged(int PageNo, int PageSize, out int TotalRows,string AgentEmail, string SearchTerm = "")
        {
            List<MstClientListView> objList = new List<MstClientListView>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }

            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parAgentEmail = new SqlParameter("@AgentEmail", AgentEmail);

            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<MstClientListView>("udspMstAgentClientList @Start,@PageSize,@SearchTerm,@AgentEmail,@TotalCount out", parStart, parPageSize, parSearchTerm, parAgentEmail, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }



        public MstInfoView MstInfoView(string Email)
        {
            var parEmail = new SqlParameter("@Email", Email);
            return objDB.Database.SqlQuery<MstInfoView>("SELECT  Id,Name +' '+LastName as Name,Phone,Email FROM AspNetUsers where Email= @Email", parEmail).FirstOrDefault();
        }
    
        public MstInfoViewV2 MstInfoViewVersion2(string Email,string Id)
        {
            var model = new MstInfoViewV2();
            var parUser = new SqlParameter("@UserName", Email);
            var parId = new SqlParameter("@Id", Id);
            model = objDB.Database.SqlQuery<MstInfoViewV2>("udspClientProfiledForDeal @UserName,@Id", parUser, parId).FirstOrDefault();
            return model;
        }
        public TrackClosingDate TrackClosingDate(string Email, string TransactionID)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            return objDB.Database.SqlQuery<TrackClosingDate>("SELECT ClosingDate FROM utblMstClosingDates where Email = @Email and TransactionID=@TransactionID", parEmail, parTransactionID).FirstOrDefault();
        }

        public TrackRatifiedDate TrackRatifiedDate(string Email, string TransactionID)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            return objDB.Database.SqlQuery<TrackRatifiedDate>("udspTrackRatifiedDate @Email,@TransactionID", parEmail, parTransactionID).FirstOrDefault();


        }
        public ClientDetails GetTransactionDetails(string TransactionID)
        {
            ClientDetails obj = new ClientDetails();
            var parId = new SqlParameter("@TransactionID", TransactionID);
            obj = objDB.Database.SqlQuery<ClientDetails>("udspMstClientTransactionDetails @TransactionID", parId).FirstOrDefault();
            return obj;
        }




        public TrackClosingDateID TrackClosingDateID(string Email, string TransactionID)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            return objDB.Database.SqlQuery<TrackClosingDateID>("SELECT ClosingDateID FROM utblMstClosingDates where Email = @Email and TransactionID=@TransactionID", parEmail, parTransactionID).FirstOrDefault();
        }

        public ClosingConfig GetClosingConfig()
        {
            return objDB.Database.SqlQuery<ClosingConfig>("SELECT StatusID FROM utblMstClosingConfigutations").FirstOrDefault();
        }

        public SellerClosingConfig GetSellerClosingConfig()
        {
            return objDB.Database.SqlQuery<SellerClosingConfig>("SELECT SellerStatusID FROM utblMstSellerClosingConfigutations").FirstOrDefault();
        }

        public string GetTrackingID(string Email, string TransactionID)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            return objDB.Database.SqlQuery<string>("select MAX(TrackingID) from utblMstTrackDeals where Email = @Email and TransactionID=@TransactionID", parEmail, parTransactionID).FirstOrDefault();
        }

        public string GetSellerTrackingID(string Email, string TransactionID)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            return objDB.Database.SqlQuery<string>("select MAX(SellerTrackingID) from utblMstSellerTrackDeals where Email = @Email and TransactionID=@TransactionID", parEmail, parTransactionID).FirstOrDefault();
        }

        public IEnumerable<StatusDropdown> StatusList(string Email,string TransactionID)
        {
            List<StatusDropdown> objList = new List<StatusDropdown>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<StatusDropdown>("udspClientStatusList @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        //TrackDealStatusList Old Version
        public IEnumerable<TrackDealStatusList> TrackDealStatusList(string Email,string TransactionID)
        {
            List<TrackDealStatusList> objList = new List<TrackDealStatusList>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<TrackDealStatusList>("udspTrackDealStatusList @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        //TrackDealStatusList New Version
        public IEnumerable<DealTracker> TrackDealStatusListVersion2(string Email, string TransactionID)
        {
            List<DealTracker> objList = new List<DealTracker>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<DealTracker>("DealTrackerVersion2 @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        //TrackDealStatusList New Version
        public IEnumerable<DealTracker> SellerTrackDealStatusListVersion2(string Email, string TransactionID)
        {
            List<DealTracker> objList = new List<DealTracker>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<DealTracker>("SellerDealTrackerVersion2 @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

    

        public IEnumerable<utblMstCalenderConfiguration> MstCalenderConfigList
        {
            get
            {
                return objDB.utblMstCalenderConfigurations;
            }
        }


        public IEnumerable<MstListingType> ListingTypeDDL()
        {
            List<MstListingType> objList = new List<MstListingType>();
            objList = objDB.Database.SqlQuery<MstListingType>("select ListingTypeID,ListingType from utblMstListingTypes").ToList();
            return objList;
        }

        //New Version
        public utblMstClosingDate GetRatifiedDetails(string TransactionID)
        {
            utblMstClosingDate obj = new utblMstClosingDate();
            var parId = new SqlParameter("@TransactionID", TransactionID);
            obj = objDB.Database.SqlQuery<utblMstClosingDate>("GetRatifiedDetails @TransactionID", parId).FirstOrDefault();
            return obj;
        }

        //Deal Price New Version
        public decimal GetDealPrice(string TransactionID)
        {
            decimal Price;
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            Price = objDB.Database.SqlQuery<decimal>("GetDealPrice @TransactionID", parTransactionID).FirstOrDefault();
            return Price;
        }

        //Deal Price New Version
        public string SetDealPrice(string TransactionID, decimal DealPrice)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parDealPrice = new SqlParameter("@DealPrice", DealPrice);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("SetDealPrice @DealPrice,@TransactionID", parDealPrice, parTransactionID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public IEnumerable<HomeType> HomeTypeDDL()
        {
            List<HomeType> objList = new List<HomeType>();
            objList = objDB.Database.SqlQuery<HomeType>("select HomeTypeId,Name from HomeType").ToList();
            return objList;
        }
        public IEnumerable<CondoStepType> CondoStepDDL()
        {
            List<CondoStepType> objList = new List<CondoStepType>();
            objList = objDB.Database.SqlQuery<CondoStepType>("select Id,Name from CondoContingencyType").ToList();
            return objList;
        }
        public IEnumerable<State> StateDDL()
        {
            List<State> objList = new List<State>();
            objList = objDB.Database.SqlQuery<State>("select StateID,StateName from utblMstNeighborhoodStates").ToList();
            return objList;
        }
        public IEnumerable<CheckListItems> GetChecklistItems()
        {
            List <CheckListItems> obj = new List<CheckListItems>();
            obj = objDB.Database.SqlQuery<CheckListItems>("select Id,Name,Stage from CheckList where TransactionType='Buyer'").ToList();
            return obj;
        }

        public IEnumerable<CheckListItems> GetSellerChecklistItems()
        {
            List<CheckListItems> obj = new List<CheckListItems>();
            obj = objDB.Database.SqlQuery<CheckListItems>("select Id,Name,Stage from CheckList where TransactionType='Seller'").ToList();
            return obj;
        }
    }
}
