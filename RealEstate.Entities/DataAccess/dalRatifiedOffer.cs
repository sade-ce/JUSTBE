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
    public class dalRatifiedOffer
    {
        public EFDBContext objDB = new EFDBContext();
        SPErrorViewModel objStatus = new SPErrorViewModel();


        public MstRatifiedEditViewModel GetRatifiedOfferDetails(string TrackingID, string TransactionID)
        {
            MstRatifiedEditViewModel Obj = new MstRatifiedEditViewModel();
            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            Obj = objDB.Database.SqlQuery<MstRatifiedEditViewModel>("udspMstRatiedEditselect @TrackingID,@TransactionID", parTrackingID, parTransactionID).FirstOrDefault();
            return Obj;

        }

        public string Save(MstRatifiedEditViewModel Item)
        {
            var parMLSID = new SqlParameter("@MLSID", "");
            var parURL = new SqlParameter("@URL", "");
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
            var parTrackingID = new SqlParameter("@TrackingID", Item.TrackingID);
            var parClosingDateID = new SqlParameter("@ClosingDateID", Item.ClosingDateID);
            var parSettlementDate = new SqlParameter("@SettlementDate", Item.SettlementDate);
            var parListingTypeID = new SqlParameter("@ListingTypeID", Item.ListingTypeID);
            var parAddress = new SqlParameter("@Address", Item.Address);
            if (Item.URL != null)
                parURL = new SqlParameter("@URL", Item.URL);

            if (Item.MLSID != null)
                parMLSID = new SqlParameter("@MLSID", Item.MLSID);

            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstRatifiedOfferUpdate @TransactionID,@TrackingID,@ClosingDateID,@SettlementDate,@ListingTypeID,@Address,@URL,@MLSID,@UpdatedOn", parTransactionID, parTrackingID, parClosingDateID, parSettlementDate, parListingTypeID, parAddress, parURL, parMLSID, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

    }
}
