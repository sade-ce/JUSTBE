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
    public class dalVendorContacts
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();
        public string SaveVendorContact(VendorContacts model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendor_Id = new SqlParameter("@Vendor_Id", model.Vendor_Id);
            var parContactName = new SqlParameter("@ContactName", model.ContactName==null?"":model.ContactName);
            var parContactTitle = new SqlParameter("@ContactTitle", model.ContactTitle==null?"":model.ContactTitle);
            var parContactPhone = new SqlParameter("@ContactPhone", model.ContactPhone == null ? "" : model.ContactPhone);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorContactsInsert @Vendor_Id,@ContactName,@ContactTitle,@ContactPhone", parVendor_Id, parContactName, parContactTitle, parContactPhone).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string SaveVendorContactV2(VendorContactView model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendor_Id = new SqlParameter("@Vendor_Id", model.Vendor_Id);
            var parContactName = new SqlParameter("@ContactName", model.ContactName == null ? "" : model.ContactName);
            var parContactTitle = new SqlParameter("@ContactTitle", model.ContactTitle == null ? "" : model.ContactTitle);
            var parContactPhone = new SqlParameter("@ContactPhone", model.ContactPhone == null ? "" : model.ContactPhone);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorContactsInsert @Vendor_Id,@ContactName,@ContactTitle,@ContactPhone", parVendor_Id, parContactName, parContactTitle, parContactPhone).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public string EditVendorContact(VendorContacts model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendor_Id = new SqlParameter("@VendorContactId", model.VendorContactId);
            var parVendorContactId = new SqlParameter("@Vendor_Id", model.Vendor_Id);
            var parContactName = new SqlParameter("@ContactName", model.ContactName == null ? "" : model.ContactName);
            var parContactTitle = new SqlParameter("@ContactTitle", model.ContactTitle == null ? "" : model.ContactTitle);
            var parContactPhone = new SqlParameter("@ContactPhone", model.ContactPhone == null ? "" : model.ContactPhone);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorContactUpdate @VendorContactId,@Vendor_Id,@ContactName,@ContactTitle,@ContactPhone", parVendorContactId, parVendor_Id, parContactName, parContactTitle, parContactPhone).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<VendorContactsAutocompleteViewModel> VendorContactsAutoComplete(string searchTerm, string Vendor_Id)
        {
            List<VendorContactsAutocompleteViewModel> obj = new List<VendorContactsAutocompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var parVendorType = new SqlParameter("@Vendor_Id", Vendor_Id);
            obj = objDB.Database.SqlQuery<VendorContactsAutocompleteViewModel>("udspGetVendorContactAutocomplete @SearchTerm,@Vendor_Id", searchParam, parVendorType).ToList();
            return obj;
        }

    }
}
