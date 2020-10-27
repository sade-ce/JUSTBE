using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.Models;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace RealEstate.Entities.DataAccess
{
    public class dalMstBuyerDocuments
    {
        private EFDBContext objDB = new EFDBContext();

        public IEnumerable<utblMstBuyerDocument> MstBuyerDocList
        {
            get
            {
                return objDB.utblMstBuyerDocuments;
            }
        }

        public string Save(utblMstBuyerDocument Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Title = Regex.Replace(Item.Title.Trim(), @"\s+", " ");
            var parTitle = new SqlParameter("@Title", Item.Title);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstBuyerDocumentInsert @Title,@Description,@UpdatedOn", parTitle, parDescription, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public utblMstBuyerDocument GetStatusByID(int ID)
        {
            utblMstBuyerDocument objItem = objDB.utblMstBuyerDocuments.FirstOrDefault(p => p.DocID == ID);
            return objItem;
        }

        public string Edit(utblMstBuyerDocument Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Title = Regex.Replace(Item.Title.Trim(), @"\s+", " ");
            var parDocID = new SqlParameter("@DocID", Item.DocID);
            Item.Title = Regex.Replace(Item.Title.Trim(), @"\s+", " ");
            var parTitle = new SqlParameter("@Title", Item.Title);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstBuyerDocumentUpdate @DocID,@Title,@Description,@UpdatedOn", parDocID, parTitle, parDescription, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string Delete(int ID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parDocID = new SqlParameter("@DocID", ID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstBuyerDocumentDelete @DocID", parDocID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string SellerDocMaster(utblMstSellerDocument Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Title = Regex.Replace(Item.Title.Trim(), @"\s+", " ");
            var parTitle = new SqlParameter("@Title", Item.Title);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerDocumentInsert @Title,@Description,@UpdatedOn", parTitle, parDescription, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    }
}
