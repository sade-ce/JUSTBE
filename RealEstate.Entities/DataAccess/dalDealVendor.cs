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
    public class dalDealVendor
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
        public IEnumerable<DealVendorView> GetDealVendorList(string Email, string TransactionID)
        {
            List<DealVendorView> objList = new List<DealVendorView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@Transaction_Id", TransactionID);

            objList = objDB.Database.SqlQuery<DealVendorView>("udspGetDealVendors @Email,@Transaction_Id", parEmail, parTransactionID).ToList();
            return objList;
        }

        public string Save(DealVendor Item,int VendorContactId)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.DealVendorId = CurrentYear + objUtil.generateUniqueCode("DealVendor");
            var parDealVendorId = new SqlParameter("@DealVendorId", Item.DealVendorId);
            var parTransaction_ID = new SqlParameter("@Transaction_Id", Item.Transaction_Id);

            var parVendor_Id = new SqlParameter("@Vendor_Id", Item.Vendor_Id);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            var parCreatedBy = new SqlParameter("@CreatedBy", Item.CreatedBy);
            var parVendorContactId = new SqlParameter("@VendorContactId", VendorContactId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspDealVendorInsert @DealVendorId,@Transaction_Id,@Vendor_Id,@Email,@UpdatedOn,@CreatedBy,@VendorContactId", parDealVendorId,parTransaction_ID, parVendor_Id, parEmail, parUpdatedOn,parCreatedBy,parVendorContactId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public DealVendor GetDealVendorByID(string DealVendorId)
        {
            DealVendor objItem = objDB.DealVendor.FirstOrDefault(p => p.DealVendorId == DealVendorId);
            return objItem;
        }

        public string DeleteDealVendor(string DealVendorId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parDealVendorId = new SqlParameter("@DealVendorId", DealVendorId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspDealVendorDelete @DealVendorId", parDealVendorId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public List<DealVendorCategoryDDl> GetVendorCategoryDDl()
        {
            List<DealVendorCategoryDDl> model = new List<DealVendorCategoryDDl>();
            model = objDB.Database.SqlQuery<DealVendorCategoryDDl>("select CategoryId[Category_Id],Name from VendorCategory where CategoryId in(select categoryId from Vendor) order by Name").ToList();
            return model;
        }
        public List<DealVendorDDl> GetVendorDDl(string VendorType)
        {
            List<DealVendorDDl> objList = new List<DealVendorDDl>();
            var parCategoryId = new SqlParameter("@Category", VendorType);
            objList = objDB.Database.SqlQuery<DealVendorDDl>("udspGetVendorByVendorTypeId @Category", parCategoryId).ToList();
            return objList;
        }
        public List<DealVendorContacts> GetVendorContact(string VendorId)
        {
            List<DealVendorContacts> objList = new List<DealVendorContacts>();
            var parVendor = new SqlParameter("@VendorId", VendorId);
            objList = objDB.Database.SqlQuery<DealVendorContacts>("GetVendorContactByVendor @VendorId", parVendor).ToList();
            return objList;
        }
        public DealVendorView GetVendorDetails(string VendorId)
        {
            DealVendorView obj= new DealVendorView();
            var parVendorId = new SqlParameter("@VendorId", VendorId);
            obj = objDB.Database.SqlQuery<DealVendorView>("udspGetVendorDetailsByID @VendorId", parVendorId).FirstOrDefault();
            return obj;
        }
        public DealVendorContacts GetContactDetails(string ContactId)
        {
            int Id = 0;
            if (!string.IsNullOrEmpty(ContactId))
                Id = Convert.ToInt32(ContactId);

            DealVendorContacts objList = new DealVendorContacts();
            var parVendor = new SqlParameter("@VendorContactId", Id);
            objList = objDB.Database.SqlQuery<DealVendorContacts>("udspGetContactByID @VendorContactId", parVendor).FirstOrDefault();
            return objList;
        }
        

    }
}

