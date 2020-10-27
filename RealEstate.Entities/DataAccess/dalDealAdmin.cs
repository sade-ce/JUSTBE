using RealEstate.Entities.Models;
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
    public class dalDealAdmin
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();
        public IEnumerable<MstDealClientList> DealGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<MstDealClientList> objList = new List<MstDealClientList>();
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
            objList = objDB.Database.SqlQuery<MstDealClientList>("udspDealAdminClientGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        //Old Version
        public string AssignAgent(utblMstTrackDealMaster Item)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.TransactionID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDealMasters");
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            //parameter for Deal Master TrackingID
            var TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            //parameter for Track Deal Master SellerTrackingID
            var SellerTrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDeals");
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", SellerTrackingID);

            var parAddress = new SqlParameter("@Address", "");

            if (Item.ClientTypeID == 1)
                parAddress = new SqlParameter("@Address", "N/A");
            else
                parAddress = new SqlParameter("@Address", Item.Address);


            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parAgentID = new SqlParameter("@AgentID", Item.AgentID);
            var parClientTypeID = new SqlParameter("@ClientTypeID", Item.ClientTypeID);
            var parStatus = new SqlParameter("@Status", Item.Status);
            var parYear = new SqlParameter("@Year", CurrentYear);
            var parDescription = new SqlParameter("@Description", Item.Description);
           
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstDealAssignAgentInsert @TransactionID, @ClientID,@AgentID,@ClientTypeID,@Status,@Year,@Description,@Address,@UpdatedOn,@TrackingID,@SellerTrackingID", parTransactionID, parClientID, parAgentID, parClientTypeID, parStatus, parYear, parDescription, parAddress,parUpdatedOn, parTrackingID,parSellerTrackingID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        //New Version
        public string AssignAgentVersion2(utblMstTrackDealMaster Item)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.TransactionID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDealMasters");
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            //parameter for Deal Master TrackingID
            var TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            //parameter for Track Deal Master SellerTrackingID
            var SellerTrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDeals");
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", SellerTrackingID);

            var parAddress = new SqlParameter("@Address", "");

            if (Item.ClientTypeID == 1)
                parAddress = new SqlParameter("@Address", "N/A");
            else
                parAddress = new SqlParameter("@Address", Item.Address);


            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parAgentID = new SqlParameter("@AgentID", Item.AgentID);
            var parClientTypeID = new SqlParameter("@ClientTypeID", Item.ClientTypeID);
            var parStatus = new SqlParameter("@Status", Item.Status);
            var parYear = new SqlParameter("@Year", CurrentYear);
            var parDescription = new SqlParameter("@Description", Item.Description);

            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstDealAssignAgentInsertVersion22 @TransactionID, @ClientID,@AgentID,@ClientTypeID,@Status,@Year,@Description,@Address,@UpdatedOn,@TrackingID,@SellerTrackingID", parTransactionID, parClientID, parAgentID, parClientTypeID, parStatus, parYear, parDescription, parAddress, parUpdatedOn, parTrackingID, parSellerTrackingID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        //New Version
        public string DeleteDealVersion2(string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstDealAssignAgentDeletVersION2 @TransactionID", parTransactionID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        //Add Client Old Version
        public string CreateClientAndAssignAgent(UserAdminRegisterModel Item)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.TransactionID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDealMasters");
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            //parameter for Deal Master TrackingID
            var TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            //parameter for Track Deal Master SellerTrackingID
            var SellerTrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDeals");
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", SellerTrackingID);

            var parAddress = new SqlParameter("@Address", "");

            if (Item.ClientTypeID == 1)
                parAddress = new SqlParameter("@Address", "N/A");
            else
                parAddress = new SqlParameter("@Address", Item.Address);


            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parAgentID = new SqlParameter("@AgentID", Item.AgentID);
            var parClientTypeID = new SqlParameter("@ClientTypeID", Item.ClientTypeID);
            var parStatus = new SqlParameter("@Status", Item.Status);
            var parYear = new SqlParameter("@Year", CurrentYear);
            var parDescription = new SqlParameter("@Description", "");

            if(string.IsNullOrEmpty(Item.Description))
                parDescription = new SqlParameter("@Description", "N/A");
            else
                parDescription = new SqlParameter("@Description", Item.Description);


            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstDealAssignAgentInsert @TransactionID, @ClientID,@AgentID,@ClientTypeID,@Status,@Year,@Description,@Address,@UpdatedOn,@TrackingID,@SellerTrackingID", parTransactionID, parClientID, parAgentID, parClientTypeID, parStatus, parYear, parDescription, parAddress, parUpdatedOn, parTrackingID, parSellerTrackingID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        //Add Client New Version
        public string CreateClientAndAssignAgentVersion2(UserAdminRegisterModel Item)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.TransactionID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDealMasters");
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            //parameter for Deal Master TrackingID
            var TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            //parameter for Track Deal Master SellerTrackingID
            var SellerTrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDeals");
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", SellerTrackingID);

            var parAddress = new SqlParameter("@Address", "");

            if (Item.ClientTypeID == 1)
                parAddress = new SqlParameter("@Address", "N/A");
            else
                parAddress = new SqlParameter("@Address", Item.ClientAddress);


            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parAgentID = new SqlParameter("@AgentID", Item.AgentID);
            var parClientTypeID = new SqlParameter("@ClientTypeID", Item.ClientTypeID);
            var parStatus = new SqlParameter("@Status", Item.Status);
            var parYear = new SqlParameter("@Year", CurrentYear);
            var parDescription = new SqlParameter("@Description", "");

            if (string.IsNullOrEmpty(Item.Description))
                parDescription = new SqlParameter("@Description", "N/A");
            else
                parDescription = new SqlParameter("@Description", Item.Description);


            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstDealAssignAgentInsertVersion2 @TransactionID, @ClientID,@AgentID,@ClientTypeID,@Status,@Year,@Description,@Address,@UpdatedOn,@TrackingID,@SellerTrackingID", parTransactionID, parClientID, parAgentID, parClientTypeID, parStatus, parYear, parDescription, parAddress, parUpdatedOn, parTrackingID, parSellerTrackingID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string Edit(utblMstTrackDealMaster Item)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");

            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            var parAddress = new SqlParameter("@Address", "");
            if (Item.ClientTypeID == 1)
                parAddress = new SqlParameter("@Address", "N/A");
            else
                parAddress = new SqlParameter("@Address", Item.Address);
            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parAgentID = new SqlParameter("@AgentID", Item.AgentID);
            var parClientTypeID = new SqlParameter("@ClientTypeID", Item.ClientTypeID);
            var parStatus = new SqlParameter("@Status", Item.Status);
            var parYear = new SqlParameter("@Year", CurrentYear);
            var parDescription = new SqlParameter("@Description", Item.Description);

            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstDealAssignAgentUpdate @TransactionID, @ClientID,@AgentID,@ClientTypeID,@Status,@Year,@Description,@Address,@UpdatedOn", parTransactionID, parClientID, parAgentID, parClientTypeID, parStatus, parYear, parDescription, parAddress, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string Delete(string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstDealAssignAgentDelete @TransactionID", parTransactionID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string GetClientHomeAnniversary(string ClientId)
        {
         
            var parClientId = new SqlParameter("@ClientId", ClientId);
            string anniversary = objDB.Database.SqlQuery<string>("udspHomeAnniversary @ClientId", parClientId).FirstOrDefault();
            return anniversary;
        }

        public IEnumerable<AgentDDl> GetAgentDDL()
        {
            List<AgentDDl> objList = new List<AgentDDl>();
            var parUserRole = new SqlParameter("@UserRole", "Agent");
            objList = objDB.Database.SqlQuery<AgentDDl>("SELECT Id, Name +' '+LastName +'('+Email+')' as AgentName FROM AspNetUsers where UserRole=@UserRole order by Name ,LastName,Email", parUserRole).ToList(); //added order by Name ,LastName,Email by sonika -27-04-2019
            return objList;
        }

        public IEnumerable<ClientDDl> GetClientDDL()
        {
            List<ClientDDl> objList = new List<ClientDDl>();
            var parUserRole = new SqlParameter("@UserRole", "Client");
            objList = objDB.Database.SqlQuery<ClientDDl>("SELECT Id, Name +' '+LastName +'('+Email+')' as ClientName FROM AspNetUsers where UserRole=@UserRole order by Name ,LastName,Email", parUserRole).ToList();  //added order by Name ,LastName,Email by sonika -27-04-2019
            return objList;
        }

        public IEnumerable<MstClientTypes> GetClientTypeDDL()
        {
            List<MstClientTypes> objList = new List<MstClientTypes>();
            objList = objDB.Database.SqlQuery<MstClientTypes>("SELECT ClientTypeID,ClientType FROM utblMstClientypes").ToList();
            return objList;
        }
        public List<NeighborhoodDropDown> GetNeighborhood()
        {
            List<NeighborhoodDropDown> model = new List<NeighborhoodDropDown>();
            model = objDB.Database.SqlQuery<NeighborhoodDropDown>("select StateID,CityID[Neighborhood_Id],CityName[Name] from utblMstNeighborhoodCities order by name").ToList();
            return model;
        }
        public utblMstTrackDealMaster GetDealbyID(string TransactionID)
        {
            utblMstTrackDealMaster objDetails = new utblMstTrackDealMaster();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objDetails = objDB.Database.SqlQuery<utblMstTrackDealMaster>("udspTrackDealMasterByID @TransactionID", parTransactionID).FirstOrDefault();
            return objDetails;
        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public IEnumerable<SearchAutoCompleteViewModel> dalDealadminAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspDealAdminClientGetPagedAutoComplete @SearchTerm ", searchParam).ToList();
            return obj;
        }
        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        ///   

        public IEnumerable<SearchAutoCompleteViewModel> dalDealadminAutoCompleteAgent(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspUserSelectAutoCompleteAgent @SearchTerm", searchParam).ToList();
            return obj;
        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        ///   
        public IEnumerable<MstDealClientList> DealGetPagedAgent(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<MstDealClientList> objList = new List<MstDealClientList>();
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
            objList = objDB.Database.SqlQuery<MstDealClientList>("udspDealAdminClientGetPagedByAgent @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }


    }
}
