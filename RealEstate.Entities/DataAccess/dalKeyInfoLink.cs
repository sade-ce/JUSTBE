using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalKeyInfoLink
    {
        private EFDBContext objDB = new EFDBContext();
        public IEnumerable<utblMstKeyInfoLink> MstKeyInfoLinkList
        {
            get
            {
                return objDB.utblMstKeyInfoLinks;
            }
        }

        public string Save(utblMstKeyInfoLink Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Title = Regex.Replace(Item.Title.Trim(), @"\s+", " ");
            var parTitle = new SqlParameter("@Title", Item.Title);
            var parTourURL = new SqlParameter("@TourURL", Item.TourURL);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstKeyInfoLinkInsert @Title,@TourURL,@UpdatedOn", parTitle, parTourURL, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public utblMstKeyInfoLink GetKeyInfoLinkByID(int ID)
        {
            utblMstKeyInfoLink objItem = objDB.utblMstKeyInfoLinks.FirstOrDefault(p => p.KeyInfoID == ID);
            return objItem;
        }

        public string Edit(utblMstKeyInfoLink Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Title = Regex.Replace(Item.Title.Trim(), @"\s+", " ");
            var parKeyInfoID = new SqlParameter("@KeyInfoID", Item.KeyInfoID);
            var parTitle = new SqlParameter("@Title", Item.Title);
            var parTourURL = new SqlParameter("@TourURL", Item.TourURL);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstKeyInfoLinkUpdate @KeyInfoID,@Title,@TourURL,@UpdatedOn", parKeyInfoID, parTitle, parTourURL, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string Delete(int ID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parKeyInfoID = new SqlParameter("@KeyInfoID", ID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstKeyInfoLinkDelete @KeyInfoID", parKeyInfoID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

    }
}
