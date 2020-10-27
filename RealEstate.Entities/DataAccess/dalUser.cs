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
    public class dalUser
    {
        private EFDBContext objDB = new EFDBContext();
        public UserProfileViewModel GetUserDetails(string UserName)
        {
            var model = new UserProfileViewModel();
            var parUser = new SqlParameter("@UserName", UserName);
            model = objDB.Database.SqlQuery<UserProfileViewModel>("udspUserProfileSelect @UserName", parUser).FirstOrDefault();
            return model;
        }

//Old AdmiClientListig
        public IEnumerable<AgentsClient> GetAgentClientList(int PageNo, int PageSize, out int TotalRows, string AgentID, string SearchTerm="")
        {
            List<AgentsClient> objList = new List<AgentsClient>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            objList = objDB.Database.SqlQuery<AgentsClient>("udspGetAgentClientPaged @Start,@PageSize,@SearchTerm,@AgentID,@TotalCount out", parStart, parPageSize, parSearchTerm, parAgentID, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        //New AgentClientListig
        public IEnumerable<AgentsClient> GetAgentClientListNew(int PageNo, int PageSize, out int TotalRows, string AgentID, string SearchTerm = "", string Year="",string Type="", string SortOrder="",string Stage="",string Tier="")
        {
            List<AgentsClient> objList = new List<AgentsClient>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            var parSortOrder = new SqlParameter("@SortOrder", SortOrder);
            var parYearFilter= new SqlParameter("@Year", Year);
            var parTypeFilter = new SqlParameter("@Type", Type);
            var parStageFilter = new SqlParameter("@Stage", Stage);
            var parTierFilter = new SqlParameter("@Tier", Tier);
            objList = objDB.Database.SqlQuery<AgentsClient>("udspGetAgentClientPagedNew @Start,@PageSize,@SearchTerm,@AgentID,@SortOrder,@Year,@Type,@Stage,@Tier,@TotalCount out", parStart, parPageSize, parSearchTerm, parAgentID, parSortOrder, parYearFilter, parTypeFilter,parStageFilter,parTierFilter, spOutput).ToList();

            //objList = objDB.Database.SqlQuery<AgentsClient>("udspGetAgentClientPagedNew @Start,@PageSize,@SearchTerm,@AgentID,@TotalCount out", parStart, parPageSize, parSearchTerm, parAgentID, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        //New AdminClientListig
        public IEnumerable<AgentsClient> GetAdminClientListNew(int PageNo, int PageSize, out int TotalRows, string AgentID, string SearchTerm = "", string AgentSearchTerm = "", string Year = "", string Type = "", string SortOrder = "", string Stage = "", string Tier = "")
        {
            List<AgentsClient> objList = new List<AgentsClient>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            var parSortOrder = new SqlParameter("@SortOrder", SortOrder);
            var parYearFilter = new SqlParameter("@Year", Year);
            var parTypeFilter = new SqlParameter("@Type", Type);
            var parStageFilter = new SqlParameter("@Stage", Stage);
            var parTierFilter = new SqlParameter("@Tier", Tier);
            var parAgentSearchTerm = new SqlParameter("@AgentSearchTerm", AgentSearchTerm);
            objList = objDB.Database.SqlQuery<AgentsClient>("udspGetAdminClientVersion2 @Start,@PageSize,@SearchTerm,@AgentSearchTerm,@AgentID,@SortOrder,@Year,@Type,@Stage,@Tier,@TotalCount out", parStart, parPageSize, parSearchTerm,parAgentSearchTerm, parAgentID, parSortOrder, parYearFilter, parTypeFilter, parStageFilter, parTierFilter, spOutput).ToList();

            //objList = objDB.Database.SqlQuery<AgentsClient>("udspGetAgentClientPagedNew @Start,@PageSize,@SearchTerm,@AgentID,@TotalCount out", parStart, parPageSize, parSearchTerm, parAgentID, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }



        public UserProfileEdit GetDetailsEdit(string UserName)
        {
            var model = new UserProfileEdit();
            var parUser = new SqlParameter("@UserName", UserName);
            model = objDB.Database.SqlQuery<UserProfileEdit>("udspUserProfileSelect @UserName", parUser).FirstOrDefault();
            return model;
        }
        public UserEdit GetAdminEdit(string UserName)
        {
            var model = new UserEdit();
            var parUser = new SqlParameter("@UserName", UserName);
            model = objDB.Database.SqlQuery<UserEdit>("udspAdminProfileEdit @UserName", parUser).FirstOrDefault();
            return model;
        }


        public MstUpdateEmailPreferences GetEmailPreferences(string Email)
        {
            var parUser = new SqlParameter("@UserName", Email);
            return objDB.Database.SqlQuery<MstUpdateEmailPreferences>("udspUserEmailPreferenceSelect @UserName", parUser).FirstOrDefault();
        }


        public List<AgentEmailDDl> GetAgentDDl()
        {
            List<AgentEmailDDl> model = new List<AgentEmailDDl>();
            model = objDB.Database.SqlQuery<AgentEmailDDl>("select Id,Email from AspNetUsers where UserRole='Agent'").ToList();
            return model;
        }

        public string UpdateProfile(UserProfileEdit model)
        {
            SPErrorViewModel objError = new SPErrorViewModel();
            var parUserName = new SqlParameter("@UserName", model.UserName);
            var parName = new SqlParameter("@Name", model.Name);
            var parLastName = new SqlParameter("@LastName", model.LastName);
            var parEmail = new SqlParameter("@Email", model.Email);
            var parPhone = new SqlParameter("@Phone", model.Phone);
            var parPhotoThumb = new SqlParameter("@PhotoThumb", model.UserPhotoThumb ?? "");
            var parPhotoNormal = new SqlParameter("@PhotoNormal", model.UserPhotoNormal ?? "");
            objError = objDB.Database.SqlQuery<SPErrorViewModel>("udspUserProfileEdit @UserName, @Name,@LastName, @Email, @Phone, @PhotoThumb, @PhotoNormal",
               parUserName, parName, parLastName, parEmail, parPhone, parPhotoThumb, parPhotoNormal).FirstOrDefault();
            return objError.ErrorCode + objError.ErrorMessage;
        }
        public string UpdateProfileNew(UserProfileEdit model)
        {
            SPErrorViewModel objError = new SPErrorViewModel();
            var parUserName = new SqlParameter("@UserName", model.UserName);
            var parName = new SqlParameter("@Name", model.Name);
            var parLastName = new SqlParameter("@LastName", model.LastName);
            var parEmail = new SqlParameter("@Email", model.Email);
            var parPhone = new SqlParameter("@Phone", model.Phone);
            objError = objDB.Database.SqlQuery<SPErrorViewModel>("udspUserProfileEditNew @UserName, @Name,@LastName, @Email, @Phone",
               parUserName, parName, parLastName, parEmail, parPhone).FirstOrDefault();
            return objError.ErrorCode + objError.ErrorMessage;
        }
        public string UpdateProfileImage(UserProfileEdit model)
        {
            SPErrorViewModel objError = new SPErrorViewModel();
            var parUserName = new SqlParameter("@UserName", model.UserName);
            var parPhotoThumb = new SqlParameter("@PhotoThumb", model.UserPhotoThumb ?? "");
            var parPhotoNormal = new SqlParameter("@PhotoNormal", model.UserPhotoNormal ?? "");
            objError = objDB.Database.SqlQuery<SPErrorViewModel>("udspUserProfileImageEdit @UserName, @PhotoThumb, @PhotoNormal",
               parUserName, parPhotoThumb, parPhotoNormal).FirstOrDefault();
            return objError.ErrorCode + objError.ErrorMessage;
        }

        //Old Version Edit ClientProfile
        public string AdminEditProfile(UserEdit model)
        {
            SPErrorViewModel objError = new SPErrorViewModel();
            var parUserName = new SqlParameter("@UserName", model.Email);
            var parName = new SqlParameter("@Name", model.Name);
            var parLastName = new SqlParameter("@LastName", model.LastName);
            var parEmail = new SqlParameter("@Email", model.Email);
            var parPhone = new SqlParameter("@Phone", model.Phone);
            var parPhotoThumb = new SqlParameter("@PhotoThumb", model.UserPhotoThumb ?? "");
            var parPhotoNormal = new SqlParameter("@PhotoNormal", model.UserPhotoNormal ?? "");
            objError = objDB.Database.SqlQuery<SPErrorViewModel>("udspUserProfileEdit @UserName, @Name,@LastName, @Email, @Phone, @PhotoThumb, @PhotoNormal",
               parUserName, parName, parLastName, parEmail, parPhone, parPhotoThumb, parPhotoNormal).FirstOrDefault();
            return objError.ErrorCode + objError.ErrorMessage;
        }


        //New Version Edit ClientProfile
        public string AdminEditProfileVersion2(Entities.ViewModels.UserAdminRegisterModel model)
        {
            SPErrorViewModel objError = new SPErrorViewModel();
            var parUserName = new SqlParameter("@UserName", model.Email);
            var parName = new SqlParameter("@Name", model.Name);
            var parLastName = new SqlParameter("@LastName", model.LastName);
            var parEmail = new SqlParameter("@Email", model.Email);
            var parPhone = new SqlParameter("@Phone", model.PhoneNumber);
            var parPhotoThumb = new SqlParameter("@PhotoThumb", model.UserPhotoThumb ?? "");
            var parPhotoNormal = new SqlParameter("@PhotoNormal", model.UserPhotoNormal ?? "");
            var parHaveReferred = new SqlParameter("@HaveReferred", model.HaveReferred ?? "");
            var parReferalSource = new SqlParameter("@ReferalSource", model.ReferalSource ?? "");
            var parClientTier = new SqlParameter("@ClientTier", model.ClientTier ?? "");
            var parNeighborhood = new SqlParameter("@Neighborhood", model.Neighborhood??0);
            var parPartner = new SqlParameter("@Partner", model.Partner ?? "");
            var parChildren = new SqlParameter("@Children", model.Children??0);
            //var parDOB = new SqlParameter("@DOB", model.DOB ?? (object)DBNull.Value);
            var parDOB = new SqlParameter("@DOB", model.DOB ?? "");
            var parClientAddress = new SqlParameter("@ClientAddress", model.ClientAddress ?? "");

            objError = objDB.Database.SqlQuery<SPErrorViewModel>("udspUserProfileEditVersion2 @UserName, @Name,@LastName, @Email, @Phone, @PhotoThumb, @PhotoNormal,@HaveReferred,@ReferalSource,@ClientTier,@Neighborhood,@Partner ,@Children,@DOB,@ClientAddress",
               parUserName, parName, parLastName, parEmail, parPhone, parPhotoThumb, parPhotoNormal,parHaveReferred,parReferalSource,parClientTier,parNeighborhood,parPartner,parChildren,parDOB,parClientAddress).FirstOrDefault();
            return objError.ErrorCode + objError.ErrorMessage;
        }


        //New Version Create ClientProfile
        public string AdminCreateProfileVersion2(Entities.ViewModels.UserAdminRegisterModel model)
        {
            SPErrorViewModel objError = new SPErrorViewModel();
            var parUserName = new SqlParameter("@UserName", model.Email);
            var parPhotoThumb = new SqlParameter("@PhotoThumb", model.UserPhotoThumb ?? "");
            var parPhotoNormal = new SqlParameter("@PhotoNormal", model.UserPhotoNormal ?? "");
            var parHaveReferred = new SqlParameter("@HaveReferred", model.HaveReferred ?? "");
            var parReferalSource = new SqlParameter("@ReferalSource", model.ReferalSource ?? "");
            var parClientTier = new SqlParameter("@ClientTier", model.ClientTier ?? "");
            var parNeighborhood = new SqlParameter("@Neighborhood", model.Neighborhood ?? 0);
            var parPartner = new SqlParameter("@Partner", model.Partner ?? "");
            var parChildren = new SqlParameter("@Children", model.Children ?? 0);
            //var parDOB = new SqlParameter("@DOB", model.DOB ?? (object)DBNull.Value);
            var parDOB = new SqlParameter("@DOB", model.DOB ?? "");
            var parClientAddress = new SqlParameter("@ClientAddress", model.ClientAddress ?? "");
            objError = objDB.Database.SqlQuery<SPErrorViewModel>("udspUserProfileInsertVersion2 @UserName, @PhotoThumb, @PhotoNormal,@HaveReferred,@ReferalSource,@ClientTier,@Neighborhood,@Partner ,@Children,@DOB,@ClientAddress",
               parUserName, parPhotoThumb, parPhotoNormal, parHaveReferred, parReferalSource, parClientTier, parNeighborhood, parPartner, parChildren, parDOB, parClientAddress).FirstOrDefault();
            return objError.ErrorCode + objError.ErrorMessage;
        }

        public IEnumerable<UserProfileViewModel> UserGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<UserProfileViewModel> objList = new List<UserProfileViewModel>();
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
            objList = objDB.Database.SqlQuery<UserProfileViewModel>("udspUserSelect @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public string SaveClient(string Email,string AgentEmail,string ColorCode)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            
            var parEmail = new SqlParameter("@Email", Email);
            var parIsClient = new SqlParameter("@IsClient", true);
            var parAgentEmail = new SqlParameter("@AgentEmail", AgentEmail);
            var parColorCode = new SqlParameter("@ColorCode", ColorCode);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspClientUpdate @Email,@IsClient,@AgentEmail,@ColorCode",  parEmail,parIsClient,parAgentEmail, parColorCode).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;

        }

        public UserProfileViewModel GetUserByEmail(string Email)
        {
            var parEmail = new SqlParameter("@Email", Email);
            return objDB.Database.SqlQuery<UserProfileViewModel>("select Name,PhoneNumber,Email from AspNetUsers where Email=@Email", parEmail).FirstOrDefault();
        }

        public string DeleteUser(string ClientID,string Email)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parClientID = new SqlParameter("@ClientID", ClientID);
            var parEmail = new SqlParameter("@Email", Email);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspUserDelete @ClientID,@Email", parClientID, parEmail).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<SearchAutoCompleteClient> ClientAutocompleteVersion2(string searchTerm)
        {
            List<SearchAutoCompleteClient> obj = new List<SearchAutoCompleteClient>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);     
            obj = objDB.Database.SqlQuery<SearchAutoCompleteClient>("ClientAutocomplete @SearchTerm", searchParam).ToList();
            return obj;
        }



        //Old Admin
        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public IEnumerable<SearchAutoCompleteViewModel> UserAutoComplete(string searchTerm, string agentId)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var Id = new SqlParameter("@AgentID", agentId);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspGetAgentClientPagedAutoComplete @SearchTerm ,@AgentID ", searchParam, Id).ToList();
            return obj;
        }



        //New Admin
        /// <summary>
        /// added by neha
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public IEnumerable<SearchAutoCompleteViewModel> AdminClientAutoCompletNew(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspGetAdminClientAutoCompleteV2 @SearchTerm  ", searchParam).ToList();
            return obj;
        }

        //New Admin
        /// <summary>
        /// added by neha
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public IEnumerable<SearchAutoCompleteViewModel> UserAutoCompletNew(string searchTerm, string agentId)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var Id = new SqlParameter("@AgentID", agentId);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspGetAgentClientPagedAutoCompleteNew @SearchTerm ,@AgentID ", searchParam, Id).ToList();
            return obj;
        }

        //New Admin
        /// <summary>
        /// added by neha
        /// </summary>
        /// <param name="ClientID"></param>
       
        /// <returns></returns>
        /// 
        public string ArchiveUser(string ClientID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parClientID = new SqlParameter("@ClientID", ClientID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspUserArchive @ClientID", parClientID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public IEnumerable<SearchAutoCompleteViewModel> ControlPanelAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspUserSelectAutoComplete @SearchTerm", searchParam).ToList();
            return obj;
        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public IEnumerable<SearchAutoCompleteViewModel> userAdminAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspuseradminAutoComplete @SearchTerm", searchParam).ToList();
            return obj;
        }

        //Client Profile New Version
        public UserAdminRegisterModel GetClientProfileVersion2(string UserName)
        {
            var model = new UserAdminRegisterModel();
            var parUser = new SqlParameter("@UserName", UserName);
            model = objDB.Database.SqlQuery<UserAdminRegisterModel>("udspClientProfileVersion2 @UserName", parUser).FirstOrDefault();
            return model;
        }
        //New Version Client Activity
        public IEnumerable<MstViewEmailActivityLog> GetClientActivityVersion2(string AgentID, string ClientId)
        {
            var ParAgentID = new SqlParameter("@AgentID", AgentID);
            var ParClientID = new SqlParameter("@ClientId", ClientId);
            return objDB.Database.SqlQuery<MstViewEmailActivityLog>("udspTrackClientActivityEmailLogsVersion2 @AgentID,@ClientId", ParAgentID, ParClientID).ToList();
        }

        //New Version Client Activity
        public IEnumerable<MstViewEmailActivityLog> GetAdminClientActivityVersion2( string ClientId)
        {
         
            var ParClientID = new SqlParameter("@ClientId", ClientId);
            return objDB.Database.SqlQuery<MstViewEmailActivityLog>("udspTrackClientActivityEmailLogsAdminVersion2 @ClientId", ParClientID).ToList();
        }

        //New Version DealList
        public IEnumerable<ClientList> ClientDealListVersion2(int PageNo, int PageSize, out int TotalRows, string Id,string ClientId)
        {
            List<ClientList> objList = new List<ClientList>();
         
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);

            var parId = new SqlParameter("@Id", Id);
            var parClientId = new SqlParameter("@ClientId", ClientId);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
      
            objList = objDB.Database.SqlQuery<ClientList>("udspClientDealsVersion2 @Start,@PageSize,@Id,@ClientId,@TotalCount out", parStart, parPageSize, parId, parClientId, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        //New Version DealList
        public IEnumerable<ClientList> ClientAdminDealListVersion2(int PageNo, int PageSize, out int TotalRows, string ClientId)
        {
            List<ClientList> objList = new List<ClientList>();

            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parClientId = new SqlParameter("@ClientId", ClientId);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            objList = objDB.Database.SqlQuery<ClientList>("udspClientDealsAdminVersion2 @Start,@PageSize,@ClientId,@TotalCount out", parStart, parPageSize, parClientId, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        //New Version Client Buyer Quiz
        public IEnumerable<QuizView> GetClientBuyerQuizVersion2(string ClientId)
        {
            var ParClientID = new SqlParameter("@ClientId", ClientId);
            return objDB.Database.SqlQuery<QuizView>("udspGetClientBuyerQuizVersion2 @ClientId", ParClientID).ToList();
        }
        //New Version Client Seller Quiz
        public IEnumerable<QuizView> GetClientSellerQuizVersion2(string ClientId)
        {
            var ParClientID = new SqlParameter("@ClientId", ClientId);
            return objDB.Database.SqlQuery<QuizView>("udspGetClientSellerQuizVersion2 @ClientId", ParClientID).ToList();
        }
        //New Version ReferalSource Autocomplete 
        public IEnumerable<SearchAutoCompleteViewModel> ReferalSourceAutoCompletNew(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("ReferalSourceVersion2 @SearchTerm  ", searchParam).ToList();
            return obj;
        }
        //New Version ReferalSource Autocomplete 
        public IEnumerable<SearchAutoCompleteClient> PartnerAutoCompletNew(string searchTerm)
        {
            List<SearchAutoCompleteClient> obj = new List<SearchAutoCompleteClient>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteClient>("Partner @SearchTerm  ", searchParam).ToList();
            return obj;
        }

        //New Version Neighborhood Autocomplete 
        public IEnumerable<SearchAutoCompleteNeighborhod> NeighborhoodAutoCompletNew(string searchTerm)
        {
            List<SearchAutoCompleteNeighborhod> obj = new List<SearchAutoCompleteNeighborhod>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteNeighborhod>("NeighborhoodVersion2 @SearchTerm  ", searchParam).ToList();
            return obj;
        }
    }
}
