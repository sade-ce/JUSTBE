using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalMstClientMLSHomeListing
    {
        public EFDBContext objDB = new EFDBContext();

        public IEnumerable<utblMstClientHomeListing> GetClientHomeList(string Email)
        {
            List<utblMstClientHomeListing> objList = new List<utblMstClientHomeListing>();
            objList = objDB.utblMstClientHomeListings.Where(x => x.Email == Email).ToList();
            return objList;
        }

        public MstInfoView MstClientInfoView(string Email)
        {
            var parEmail = new SqlParameter("@Email", Email);
            return objDB.Database.SqlQuery<MstInfoView>("SELECT  Name,Phone,Email FROM AspNetUsers where Email= @Email", parEmail).FirstOrDefault();
        }
        public IEnumerable<ListingTypes> ListingTypeDDL()
        {
            List<ListingTypes> objList = new List<ListingTypes>();
            objList = objDB.Database.SqlQuery<ListingTypes>("select ListingTypeID,ListingType from utblMstListingTypes").ToList();
            return objList;
        }
        public string Save(utblMstClientHomeListing Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parMLSID = new SqlParameter("@MLSID", "");
            var parListingTypeID = new SqlParameter("@ListingTypeID", Item.ListingTypeID);
            var parAddress = new SqlParameter("@Address", Item.Address);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parURL = new SqlParameter("@URL", "");
            if (Item.URL != null)
            {
                parURL = new SqlParameter("@URL", Item.URL);
            }
            else
            {
                parURL = new SqlParameter("@URL", DBNull.Value);

            }

            if (Item.MLSID != null)
            {
                parMLSID = new SqlParameter("@MLSID", Item.MLSID);

            }
            else
            {
                parMLSID = new SqlParameter("@MLSID", DBNull.Value);
            }
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstClientHomeListingInfoInsert @MLSID,@ListingTypeID,@Address,@Email,@URL,@UpdatedOn", parMLSID, parListingTypeID,parAddress ,parEmail,parURL, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string Delete(int MLSListingID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parMLSListingID = new SqlParameter("@MLSListingID", MLSListingID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstClientHomeListingInfoDelete @MLSListingID", parMLSListingID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

    }
}
