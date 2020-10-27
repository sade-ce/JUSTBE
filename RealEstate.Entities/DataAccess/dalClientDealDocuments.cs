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
    public class dalClientDealDocuments
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();

        //public IEnumerable<ClientDealDocuments> List(string TransactionID)
        //{
        //    List<ClientDealDocuments> objList = new List<ClientDealDocuments>();
        //    var parTransactionID = new SqlParameter("@TransactionID", TransactionID); 
        //     objList = objDB.Database.SqlQuery<ClientDealDocuments>("udspMstClientDealDocumentSelect @TransactionID",  parTransactionID).ToList();
        //    return objList;
        //}

        public IEnumerable<ClientDealDocumentsView> List(string TransactionID)
        {
            List<ClientDealDocumentsView> objList = new List<ClientDealDocumentsView>();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<ClientDealDocumentsView>("udspMstClientDealDocumentSelectNew @TransactionID", parTransactionID).ToList();
            return objList;
        }

        //public string Save(ClientDealDocuments model)
        //{
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parTransID = new SqlParameter("@TransactionID", model.TransactionID);
        //    var parDocFile = new SqlParameter("@DocFile", model.DocFile);
        //    var parClientId = new SqlParameter("@ClientId", model.ClientId);
        //    var parTitle = new SqlParameter("@Title", model.Title);
        //    var parDescription = new SqlParameter("@Description", model.Description==null?"":model.Description);
        //    var parDocumentType = new SqlParameter("@DocumentType", model.DocumentType);

        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientDealDocumentInsert @TransactionID,@DocFile,@ClientId,@Title,@Description,@DocumentType", parTransID,parDocFile, parClientId, parTitle, parDescription, parDocumentType).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}

        public string Save(ClientDealDocumentsView model)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();



            string ClientDocId = CurrentYear + objUtil.generateUniqueCode("ClientDealDocument");
            var parClientDocId = new SqlParameter("@ClientDocId", ClientDocId);
            var parTransID = new SqlParameter("@TransactionID", model.TransactionID);
            var parDocFile = new SqlParameter("@DocFile", model.DocFile);
            var parClientId = new SqlParameter("@ClientId", model.ClientId);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parDescription = new SqlParameter("@Description", model.Description == null ? "" : model.Description);
            var parDocumentType = new SqlParameter("@DocumentType", model.DocumentType);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientDocumentInsert @ClientDocId,@TransactionID,@DocFile,@ClientId,@Title,@Description,@DocumentType", parClientDocId, parTransID, parDocFile, parClientId, parTitle, parDescription, parDocumentType).FirstOrDefault();
            return objStatus.ErrorMessage;
        }

        //public string Edit(ClientDealDocuments model)
        //{
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parId = new SqlParameter("@Id", model.Id);
        //    var parDocFile = new SqlParameter("@DocFile", model.DocFile);
        //    var parTitle = new SqlParameter("@Title", model.Title);
        //    var parDescription = new SqlParameter("@Description", model.Description == null ? "" : model.Description);
        //    var parDocumentType = new SqlParameter("@DocumentType", model.DocumentType);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientDealDocumentUpdate @Id,@DocFile,@Title,@Description,@DocumentType", parId, parDocFile, parTitle, parDescription, parDocumentType).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}

        public string Edit(ClientDealDocumentsView model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@DocId", model.DocId);
            var parClientDocId = new SqlParameter("@ClientDocId", model.ClientDocId);
            var parDocFile = new SqlParameter("@DocFile", model.DocFile);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parDescription = new SqlParameter("@Description", model.Description == null ? "" : model.Description);
            var parDocumentType = new SqlParameter("@DocumentType", model.DocumentType);
            var parTransactionID = new SqlParameter("@TransactionID", model.TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientDealDocumentUpdateNew @DocId,@ClientDocId,@DocFile,@Title,@Description,@DocumentType,@TransactionID", parId,parClientDocId, parDocFile, parTitle, parDescription, parDocumentType, parTransactionID).FirstOrDefault();
            return objStatus.ErrorMessage;
        }

        //public ClientDealDocuments GetClientDealDocumentByID(int Id)
        //{
        //    ClientDealDocuments objDetails = new ClientDealDocuments();
        //    var parId = new SqlParameter("@Id", Id);
        //    objDetails = objDB.Database.SqlQuery<ClientDealDocuments>("GetClientDealDocumentByID @Id", parId).FirstOrDefault();
        //    return objDetails;
        //}

        public ClientDealDocumentsView GetClientDealDocumentByID(int DocId, string ClientDocId)
        {
            ClientDealDocumentsView objDetails = new ClientDealDocumentsView();
            var parDocId = new SqlParameter("@DocId", DocId);
            var parClientDocId = new SqlParameter("@ClientDocId", ClientDocId);
            objDetails = objDB.Database.SqlQuery<ClientDealDocumentsView>("GetClientDocumentByID @DocId,@ClientDocId", parDocId, parClientDocId).FirstOrDefault();
            return objDetails;
        }

        public string GetClientFileName(string ClientDocId)
        {

            var parClientDocId = new SqlParameter("@ClientDocId", ClientDocId);
            string Filename = objDB.Database.SqlQuery<string>("GetClientDocFileName @ClientDocId", parClientDocId).FirstOrDefault();
            return Filename;
        }
        //public string Delete(int Id)
        //{
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parId = new SqlParameter("@Id", Id);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientDealDocumentDelete @Id", parId).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}
        public string Delete(string @ClientDocId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@ClientDocId", @ClientDocId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientDealDocumentDeleteNew @ClientDocId", parId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

    }
}
