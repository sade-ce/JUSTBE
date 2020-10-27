using RealEstate.Entities.Models;
using RealEstate.Entities.Utility;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalAgentDealSharing
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();

        public IEnumerable<ClientDetails> GetSharedDeal(string AgentID)
        {
            List<ClientDetails> obj = new List<ClientDetails>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            obj = objDB.Database.SqlQuery<ClientDetails>("udspDealAgentShareDeal @AgentID", parAgentID).ToList();
            return obj;
        }
        public IEnumerable<AgentDDl> GetAgentDDL(string AgentID)
        {
            List<AgentDDl> objList = new List<AgentDDl>();
            var parUserRole = new SqlParameter("@UserRole", "Agent");
            var parAgentID = new SqlParameter("@AgentID", AgentID);

            objList = objDB.Database.SqlQuery<AgentDDl>("SELECT Id, Name +' '+LastName +'('+Email+')' as AgentName FROM AspNetUsers where UserRole=@UserRole and Id <> @AgentID", parUserRole, parAgentID).ToList();
            return objList;
        }

        public string ShareAgentTrans(utblMstAgentDealSharing Item)
        {
          
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parSharerAgentID = new SqlParameter("@SharerAgentID", Item.SharerAgentID);
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            var parSharedToAgentID = new SqlParameter("@SharedToAgentID", Item.SharedToAgentID);
            var parNotes = new SqlParameter("@Notes", "");

            if (string.IsNullOrEmpty(Item.Notes))
                parNotes = new SqlParameter("@Notes", "N/A");
            else
                parNotes = new SqlParameter("@Notes", Item.Notes);


            var parSharedOn = new SqlParameter("@SharedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udsputblMstAgentDealSharingsInsert @ClientID, @SharerAgentID,@TransactionID,@SharedToAgentID,@Notes,@SharedOn", parClientID, parSharerAgentID, parTransactionID, parSharedToAgentID, parNotes, parSharedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<ClientList> SharedClientList(string Id)
        {
            List<ClientList> objList = new List<ClientList>();     
            var parId = new SqlParameter("@Id", Id);
            objList = objDB.Database.SqlQuery<ClientList>("udspSharedDealAgentsClientList @Id",parId).ToList();
            return objList;
        }

        public IEnumerable<MstAgentDealShareViewModel> SharedDealList(string AgentID)
        {
            List<MstAgentDealShareViewModel> objList = new List<MstAgentDealShareViewModel>();
            var parAgentID  = new SqlParameter("@AgentID", AgentID);
            objList = objDB.Database.SqlQuery<MstAgentDealShareViewModel>("udspGetAgentSharedDeal @AgentID", parAgentID).ToList();
            return objList;
        }
        public string Delete(string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udsputblMstAgentDealSharingsDelete @TransactionID", parTransactionID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

    }
}
