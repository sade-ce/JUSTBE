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
    public class dalMstAgentPhotoGallery
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();

        public UserProfileView GetUserDetails(string ClientID)
        {
            var model = new UserProfileView();
            var parClientID = new SqlParameter("@ClientID", ClientID);
            model = objDB.Database.SqlQuery<UserProfileView>("udspDealUserProfileSelect @ClientID", parClientID).FirstOrDefault();
            return model;
        }
        public IEnumerable<MstPhotoGalleryView> GetPhotoGalleryList(string Email, string TransactionID)
        {
            List<MstPhotoGalleryView> objList = new List<MstPhotoGalleryView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<MstPhotoGalleryView>("udspMstAgentPhotoGallery @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        public string Save(utblMstClientGallerie Item)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.GallaryID = CurrentYear + objUtil.generateUniqueCode("utblMstClientGalleries");
            var parGallaryID = new SqlParameter("@GallaryID", Item.GallaryID);
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);

            var parPhotoNormal = new SqlParameter("@PhotoNormal", Item.PhotoNormal);
            var parPhotoThumb = new SqlParameter("@PhotoThumb", Item.PhotoThumb);
            var parDescription = new SqlParameter("@Description", Item.Description ?? "");
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstAgentPhotoGalleryInsert @GallaryID,@TransactionID,@PhotoNormal,@PhotoThumb,@Description,@Email,@UpdatedOn", parGallaryID, parTransactionID, parPhotoNormal, parPhotoThumb, parDescription, parEmail,parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string SaveNew(utblMstClientGallerie Item)
        {
            if (Item.GallaryID == null)
            {
                DateTime dt = DateTime.Now;
                var CurrentYear = dt.ToString("yyyy");

                Item.GallaryID = CurrentYear + objUtil.generateUniqueCode("utblMstClientGalleries");
            }
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parGallaryID = new SqlParameter("@GallaryID", Item.GallaryID);
            var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);

            var parPhotoNormal = new SqlParameter("@PhotoNormal", Item.PhotoNormal);
            var parPhotoThumb = new SqlParameter("@PhotoThumb", Item.PhotoThumb);
            var parDescription = new SqlParameter("@Description", Item.Description ?? "");
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstAgentPhotoGalleryInsertNew @GallaryID,@TransactionID,@PhotoNormal,@PhotoThumb,@Description,@Email,@UpdatedOn", parGallaryID, parTransactionID, parPhotoNormal, parPhotoThumb, parDescription, parEmail, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public utblMstClientGallerie GetPhotoGalleryByID(string ID)
        {
            utblMstClientGallerie objItem = objDB.utblMstClientGalleries.FirstOrDefault(p => p.GallaryID == ID);
            return objItem;
        }

        public string DeleteGallery(string GallaryID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parGallaryID = new SqlParameter("@GallaryID", GallaryID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstPhotoGalleryDelete @GallaryID", parGallaryID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }



        public string SaveUserGallery(UserGallery Item)
        {
            if (Item.UserGalleryId == null)
            {
                var guid = Guid.NewGuid().ToString();
                Item.UserGalleryId = guid;
                  
            }
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parUserGalleryId = new SqlParameter("@UserGalleryId", Item.UserGalleryId);
            var parUserId = new SqlParameter("@UserId", Item.UserId);

            var parPhotoNormal = new SqlParameter("@Photo", Item.Photo);
           
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("UserGalleryInsert @UserGalleryId,@UserId,@Photo,@UpdatedOn", parUserGalleryId, parUserId, parPhotoNormal,parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public UserGalleryView GetUserGalleryByID(Guid ID)
        {
           UserGalleryView objList = new UserGalleryView();
            var parUserId = new SqlParameter("@UserGalleryId", ID);

            objList = objDB.Database.SqlQuery<UserGalleryView>("GetUserGalleryById @UserGalleryId", parUserId).FirstOrDefault();
            return objList;
        }
        public IEnumerable<UserGalleryView> GetUserGalleryList(string UserId)
        {
            List<UserGalleryView> objList = new List<UserGalleryView>();
            var parUserId = new SqlParameter("@UserId", UserId);

            objList = objDB.Database.SqlQuery<UserGalleryView>("GetUserGallery @UserId", parUserId).ToList();
            return objList;
        }
        public string DeleteUserGallery(Guid UserGalleryId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parUserGalleryId = new SqlParameter("@UserGalleryId", UserGalleryId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("UserPhotoGalleryDelete @UserGalleryId", parUserGalleryId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    }
}
