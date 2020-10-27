using RealEstate.Entities.Utility;
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
    public class dalDealAgent
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();

        //Old Version ClientList
        public IEnumerable<ClientList> ClientList(int PageNo, int PageSize, out int TotalRows, string Id,string SearchTerm = "")
        {
            List<ClientList> objList = new List<ClientList>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }

            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parId = new SqlParameter("@Id", Id);

            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<ClientList>("udspDealAgentsClientListNew @Start,@PageSize,@SearchTerm,@Id,@TotalCount out", parStart, parPageSize, parSearchTerm, parId, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }


        //New Version ClientList
        public IEnumerable<ClientList> ClientListVersion2(int PageNo, int PageSize, out int TotalRows, string Id, string SearchTerm = "", string Year = "", string Type = "", string SortOrder = "",string Stage="",string Tier="")
        {
            List<ClientList> objList = new List<ClientList>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }

            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parId = new SqlParameter("@Id", Id);

            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            var parSortOrder = new SqlParameter("@SortOrder", SortOrder);
            var parYearFilter = new SqlParameter("@Year", Year);
            var parTypeFilter = new SqlParameter("@Type", Type);
            var parStageFilter = new SqlParameter("@Stage", Stage);
            var parTierFilter = new SqlParameter("@Tier", Tier);
            objList = objDB.Database.SqlQuery<ClientList>("udspDealAgentsClientListNewVersion2 @Start,@PageSize,@SearchTerm,@Id,@SortOrder,@Year,@Type,@Stage,@Tier,@TotalCount out", parStart, parPageSize, parSearchTerm, parId,parSortOrder,parYearFilter,parTypeFilter,parStageFilter,parTierFilter, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }


        //New Version ClientList
        public IEnumerable<ClientList> ClientListAdmin(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "",string AgentSearchTerm="", string Year = "", string Type = "", string SortOrder = "", string Stage = "", string Tier = "")
        {
            List<ClientList> objList = new List<ClientList>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }

            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parAgentSearchTerm = new SqlParameter("@AgentSearchTerm", AgentSearchTerm);

            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            var parSortOrder = new SqlParameter("@SortOrder", SortOrder);
            var parYearFilter = new SqlParameter("@Year", Year);
            var parTypeFilter = new SqlParameter("@Type", Type);
            var parStageFilter = new SqlParameter("@Stage", Stage);
            var parTierFilter = new SqlParameter("@Tier", Tier);
            objList = objDB.Database.SqlQuery<ClientList>("udspDealAdminClientListNewVersion2 @Start,@PageSize,@SearchTerm,@AgentSearchTerm,@SortOrder,@Year,@Type,@Stage,@Tier,@TotalCount out", parStart, parPageSize, parSearchTerm,parAgentSearchTerm, parSortOrder, parYearFilter, parTypeFilter, parStageFilter, parTierFilter, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }


        public string DeleteDeal(string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstDealAssignAgentDelete @TransactionID", parTransactionID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public IEnumerable<ClientDetails> ClientDetails(string Id,string AgentID)
        {
            List<ClientDetails> obj = new List<ClientDetails>();
            var parId = new SqlParameter("@Id", Id);
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            obj = objDB.Database.SqlQuery<ClientDetails>("udspDealAgentsClientDetails @Id,@AgentID", parId, parAgentID).ToList();
            return obj;
        }

        public UserProfileView GetUserDetails(string ClientID)
        {
            var model = new UserProfileView();
            var parClientID = new SqlParameter("@ClientID", ClientID);
            model = objDB.Database.SqlQuery<UserProfileView>("udspDealUserProfileSelect @ClientID", parClientID).FirstOrDefault();
            return model;
        }



        /// <summary>
        /// Added by sonika Old Version
        /// </summary>
        /// <param name="SearchTerm"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public IEnumerable<SearchAutoCompleteViewModel> ClientAutoComplete(string searchTerm, string agentID)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var Id = new SqlParameter("@Id", agentID);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspDealAgentsClientListAutocomplete @SearchTerm,@Id", searchParam, Id).ToList();
            return obj;
        }


        

        //New Version ActiveList
        public IEnumerable<ClientList> ActiveDeals(int PageNo, int PageSize, out int TotalRows, string Id, string SearchTerm = "")
        {
            List<ClientList> objList = new List<ClientList>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }

            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parId = new SqlParameter("@Id", Id);

            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<ClientList>("udspActiveDealsVersion2 @Start,@PageSize,@SearchTerm,@Id,@TotalCount out", parStart, parPageSize, parSearchTerm, parId, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        //New Version ActiveList
        public IEnumerable<ClientList> ActiveDealsAdmin(int PageNo, int PageSize, out int TotalRows,  string SearchTerm = "")
        {
            List<ClientList> objList = new List<ClientList>();
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
            objList = objDB.Database.SqlQuery<ClientList>("udspActiveDealsAdmin @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        /// <summary>
        /// Added by neha New Version
        /// </summary>
        /// <param name="SearchTerm"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public IEnumerable<SearchAutoCompleteViewModel> ClientAutoCompleteVesrsion2(string searchTerm, string agentID)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var Id = new SqlParameter("@Id", agentID);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspDealAgentsClientListAutocompleteVersion2 @SearchTerm,@Id", searchParam, Id).ToList();
            return obj;
        }

        public IEnumerable<SearchAutoCompleteViewModel> AdminAutoCompleteVesrsion2(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspAdminClientListAutocompleteVersion2 @SearchTerm", searchParam).ToList();
            return obj;
        }
        

        public IEnumerable<SearchAutoCompleteAgent> AgentAutoCompleteVesrsion2(string searchTerm)
        {
            List<SearchAutoCompleteAgent> obj = new List<SearchAutoCompleteAgent>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteAgent>("udspUserSelectAutoCompleteAgentVersion2 @SearchTerm", searchParam).ToList();
            return obj;
        }


        public IEnumerable<MstClientTypes> GetClientTypeDDL()
        {
            List<MstClientTypes> objList = new List<MstClientTypes>();
            objList = objDB.Database.SqlQuery<MstClientTypes>("SELECT ClientTypeID,ClientType FROM utblMstClientypes").ToList();
            return objList;
        }
    }
}
