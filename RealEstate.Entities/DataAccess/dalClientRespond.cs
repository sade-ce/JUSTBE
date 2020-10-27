using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
using RealEstate.Entities.Utility;

namespace RealEstate.Entities.DataAccess
{
    public class dalClientRespond
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public MstClientDealRespondViewModel GetClientDealRespondDetails(string TrackingID)
        {
            var model = new MstClientDealRespondViewModel();
            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            model = objDB.Database.SqlQuery<MstClientDealRespondViewModel>("udspClientDealRespondSelect @TrackingID", parTrackingID).FirstOrDefault();
            return model;
        }

        public IEnumerable<utblMstClientTrackDealDoc> GetClientRespondDocList(string ID)
        {
            List<utblMstClientTrackDealDoc> objList = new List<utblMstClientTrackDealDoc>();
            objList = objDB.utblMstClientTrackDealDocs.Where(x => x.TrackingID == ID).ToList();
            return objList;
        }


        public utblMstClientTrackDealDoc GetClientDocDetailsByID(string ID)
        {
            utblMstClientTrackDealDoc objItem = objDB.utblMstClientTrackDealDocs.FirstOrDefault(p => p.ClientTrackDocID == ID);
            return objItem;
        }


        public string DeleteClientDocument(string ClientTrackDocID,string TrackingID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parClientTrackDocID = new SqlParameter("@ClientTrackDocID", ClientTrackDocID);

            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstClientDocumentDelete @ClientTrackDocID,@TrackingID", parClientTrackDocID, parTrackingID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public string SaveClientTrackDealDoc(utblMstClientTrackDealDoc ItemData)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            ItemData.ClientTrackDocID = CurrentYear + objUtil.generateUniqueCode("utblMstClientTrackDealDocs");
            var parClientTrackDocID = new SqlParameter("@ClientTrackDocID", ItemData.ClientTrackDocID);
            var parTrackingID = new SqlParameter("@TrackingID", ItemData.TrackingID);
            var parTrackDocFile = new SqlParameter("@TrackDocFile", ItemData.TrackDocFile ?? "");
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstClientRespondDealDocInsert @ClientTrackDocID, @TrackingID, @TrackDocFile,@UpdatedOn", parClientTrackDocID, parTrackingID, parTrackDocFile, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<utblMstTrackDealDoc> GetClientLiveDealDocList(string ID)
        {
            List<utblMstTrackDealDoc> objList = new List<utblMstTrackDealDoc>();
            objList = objDB.utblMstTrackDealDocs.Where(x => x.TrackingID == ID).ToList();
            return objList;
        }
    }
}
