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
    public class dalMstSellerDocuments
    {
        private EFDBContext objDB = new EFDBContext();

        public IEnumerable<utblMstSellerDocument> MstSellerDocList
        {
            get
            {
                return objDB.utblMstSellerDocuments;
            }
        }

        public string Save(utblMstSellerDocument Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Title = Regex.Replace(Item.Title.Trim(), @"\s+", " ");
            var parTitle = new SqlParameter("@Title", Item.Title);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerDocumentInsert @Title,@Description,@UpdatedOn", parTitle, parDescription, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public utblMstSellerDocument GetStatusByID(int ID)
        {
            utblMstSellerDocument objItem = objDB.utblMstSellerDocuments.FirstOrDefault(p => p.SellerDocID == ID);
            return objItem;
        }

        public string Edit(utblMstSellerDocument Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Title = Regex.Replace(Item.Title.Trim(), @"\s+", " ");
            var parDocID = new SqlParameter("@DocID", Item.SellerDocID);
            Item.Title = Regex.Replace(Item.Title.Trim(), @"\s+", " ");
            var parTitle = new SqlParameter("@Title", Item.Title);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerDocumentUpdate @DocID,@Title,@Description,@UpdatedOn", parDocID, parTitle, parDescription, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string Delete(int ID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parDocID = new SqlParameter("@DocID", ID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerADocumentDelete @DocID", parDocID).FirstOrDefault();
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
