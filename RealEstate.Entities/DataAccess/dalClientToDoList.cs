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
    public class dalClientToDoList
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public IEnumerable<ClientToDoListView> ClientToDoListViewGetPaged(string TransactionID,string ClientId,int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<ClientToDoListView> objList = new List<ClientToDoListView>();
            if (SearchTerm != "")
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            var parClientId = new SqlParameter("@ClientId", ClientId); 
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parTransactionId = new SqlParameter("@TransactionID", TransactionID);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<ClientToDoListView>("ClientToDoListGetPaged @ClientId,@Start,@PageSize,@SearchTerm,@TransactionID,@TotalCount out", parClientId,parStart, parPageSize, parSearchTerm, parTransactionId, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public string Save(ClientToDoListView model, string CreatedBy,string ClientId)
        {
           
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parTask = new SqlParameter("@Task", model.Task);
            var parClientId = new SqlParameter("@Client_Id", ClientId);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parTransactionId = new SqlParameter("@TransactionID", model.TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientToDoListInsert @Task,@Client_Id,@CreatedBy,@TransactionID", parTask, parClientId, parCreatedBy,parTransactionId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        //public string Edit(ClientToDoListView model, string CreatedBy)
        //{
          
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parId = new SqlParameter("@Id", model.Id);
        //    var parTask = new SqlParameter("@Task", model.Task);
        //    var parClientId = new SqlParameter("@Client_Id", CreatedBy);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientToDoListUpdate @Id,@Task,@Client_Id", parId, parTask, parClientId).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}
        public string Edit(string Task,int Id)
        {

            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Id);
            var parTask = new SqlParameter("@Task", Task);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientToDoListUpdate @Id,@Task", parId, parTask).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public ClientToDoList GetClientToDoListByID(string Id)
        {
            ClientToDoList objDetails = new ClientToDoList();
            var parId = new SqlParameter("@Id", Id);
            objDetails = objDB.Database.SqlQuery<ClientToDoList>("GetClientToDoListByID @Id", Id).FirstOrDefault();
            return objDetails;
        }
        //for client paging blog single
        public ClientToDoListPaging GetBlogPrevNext(string Id)
        {
            ClientToDoListPaging objDetails = new ClientToDoListPaging();
            var parId = new SqlParameter("@Id", Id);
            objDetails = objDB.Database.SqlQuery<ClientToDoListPaging>("ToDoListGetPaging @Id", parId).FirstOrDefault();
            return objDetails;
        }
        public string Delete(int Id)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Id);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientToDoListDelete @Id", parId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string MarkAsDone(int Id,bool IsDone)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Id);
            var parIsDone = new SqlParameter("@IsDone", IsDone);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("MarkIdDone @Id,@IsDone", parId, parIsDone).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public string GetClientIdByTransactionId(string TransactionID)
        {
           
            var parId = new SqlParameter("@TransactionID", TransactionID);
            string ClientId  = objDB.Database.SqlQuery<string>("GetClientIdByTransactionId @TransactionID", parId).FirstOrDefault();
            return ClientId;
        }
    }
}
