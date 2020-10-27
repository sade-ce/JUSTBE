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
    public class dalVendor
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();
        dalVendorContacts objVendorContact = new dalVendorContacts();
        //Old Version
        public IEnumerable<VendorView> VendorGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<VendorView> objList = new List<VendorView>();
            if (SearchTerm != "")
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<VendorView>("udspVendorGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        //New Version
        public IEnumerable<VendorView> VendorGetPagedV2(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "", string SortOrder = "")
        {
            List<VendorView> objList = new List<VendorView>();
            if (SearchTerm != "")
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            var parSortOrder = new SqlParameter("@SortOrder", SortOrder);
            objList = objDB.Database.SqlQuery<VendorView>("udspVendorGetPagedV2 @Start,@PageSize,@SearchTerm,@SortOrder,@TotalCount out", parStart, parPageSize, parSearchTerm,parSortOrder, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public string Save(Vendor model, VendorContacts VendorContact, string CreatedBy)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
      
            if (string.IsNullOrEmpty(model.VendorId)){
                DateTime dt = DateTime.Now;
                var CurrentYear = dt.ToString("yyyy");
                model.VendorId = CurrentYear + objUtil.generateUniqueCode("Vendor");
            }
          else
            {
                bool isVendoeexists = GetVendorByLocation(model.VendorId, model.Location);
                if (isVendoeexists == false)
                {
                    DateTime dt = DateTime.Now;
                    var CurrentYear = dt.ToString("yyyy");
                    model.VendorId = CurrentYear + objUtil.generateUniqueCode("Vendor");
                }
            }
          
            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
            var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parVendorImage = new SqlParameter("@VendorImage", DBNull.Value);
            var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
            if (model.VendorImage != null)
                parVendorImage = new SqlParameter("@VendorImage", model.VendorId.ToString() + model.VendorImage.ToString());
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategory_Id = new SqlParameter("@Category_Id", model.Category_Id);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorInsert @VendorId,@Title,@SubTitle,@Category_Id,@Phone,@Email,@About,@VendorImage,@Location,@CreatedBy,@WebsiteLink", parVendorId, parTitle, parSubTitle, parCategory_Id, parPhone,parEmail,parAbout,parVendorImage,parLocation, parCreatedBy,parWeb).FirstOrDefault();
            if(objStatus.ErrorMessage=="")
            {

                if ((VendorContact.ContactName != null)|| (VendorContact.ContactPhone != null)|| (VendorContact.ContactTitle != null))
                
                {
                    VendorContact.Vendor_Id = model.VendorId;
                    objVendorContact.SaveVendorContact(VendorContact);
                }
            }
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        
        }


        public string SaveVersion2(Vendor model,string CreatedBy)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();

           
                DateTime dt = DateTime.Now;
                var CurrentYear = dt.ToString("yyyy");
                model.VendorId = CurrentYear + objUtil.generateUniqueCode("Vendor");
         

            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
            var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parVendorImage = new SqlParameter("@VendorImage", DBNull.Value);
            var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
            if (model.VendorImage != null)
                parVendorImage = new SqlParameter("@VendorImage",  model.VendorImage.ToString());
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategory_Id = new SqlParameter("@Category_Id", model.Category_Id);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorInsertVersion2 @VendorId,@Title,@SubTitle,@Category_Id,@Phone,@Email,@About,@VendorImage,@Location,@CreatedBy,@WebsiteLink", parVendorId, parTitle, parSubTitle, parCategory_Id, parPhone, parEmail, parAbout, parVendorImage, parLocation, parCreatedBy, parWeb).FirstOrDefault();
          
            return objStatus.ErrorCode + objStatus.ErrorMessage;

        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <param name="TransactionId"></param>
        /// <returns></returns>
        public string SaveClientVendorClient(Vendor model, string CreatedBy, string TransactionId)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            model.VendorId = CurrentYear + objUtil.generateUniqueCode("Vendor");
            var DealVendorId = CurrentYear + objUtil.generateUniqueCode("DealVendor");
            var vendorCategoryId = CurrentYear + objUtil.generateUniqueCode("VendorCategory");
            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
            var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parVendorImage = new SqlParameter("@VendorImage", DBNull.Value);
            var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
            if (model.VendorImage != null)
                parVendorImage = new SqlParameter("@VendorImage",model.VendorImage.ToString());
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategory_Id = new SqlParameter("@Category_Name", model.Category_Id);
            var parTransactionId = new SqlParameter("@TransactionId", TransactionId);
            var parDealVendorId = new SqlParameter("@DealVendorId", DealVendorId);
            var parvendorCategory = new SqlParameter("@VendorCategoryId", vendorCategoryId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorInsertClientVendorClient @VendorId,@Title,@SubTitle,@Category_Name,@Phone,@Email,@About,@VendorImage,@Location,@CreatedBy,@WebsiteLink,@TransactionId,@DealVendorId,@VendorCategoryId", parVendorId, parTitle, parSubTitle, parCategory_Id, parPhone, parEmail, parAbout, parVendorImage, parLocation, parCreatedBy, parWeb, parTransactionId, parDealVendorId, parvendorCategory).FirstOrDefault();
            return objStatus.ErrorMessage;
        }

        public string Edit(Vendor model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
            var parPhone = new SqlParameter("@Phone", model.Phone==null?"":model.Phone);
            var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
            var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategory_Id = new SqlParameter("@Category_Id", model.Category_Id);
            var parVendorImage = new SqlParameter("@VendorImage", DBNull.Value);
            var parLocation = new SqlParameter("@Location", model.Location==null?"": model.Location);
            if (model.VendorImage != null)
                parVendorImage = new SqlParameter("@VendorImage", model.VendorImage.ToString());
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorUpdate @VendorId,@Title,@SubTitle,@Category_Id,@Phone,@Email,@About,@VendorImage,@Location,@CreatedBy,@WebsiteLink", parVendorId, parTitle, parSubTitle, parCategory_Id, parPhone, parEmail, parAbout, parVendorImage, parLocation, parCreatedBy,parWeb).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string EditVersion2(Vendor model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
            var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategory_Id = new SqlParameter("@Category_Id", model.Category_Id);
            var parVendorImage = new SqlParameter("@VendorImage", DBNull.Value);
            var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
            if (model.VendorImage != null)
                parVendorImage = new SqlParameter("@VendorImage", model.VendorImage.ToString());
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorUpdateVersion2 @VendorId,@Title,@SubTitle,@Category_Id,@Phone,@Email,@About,@VendorImage,@Location,@CreatedBy,@WebsiteLink", parVendorId, parTitle, parSubTitle, parCategory_Id, parPhone, parEmail, parAbout, parVendorImage, parLocation, parCreatedBy, parWeb).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        /// <summary>
        /// added by sonika
        /// </summary>
        /// <returns></returns>
        /// 
        public string EditVendorClient(Vendor model, string CreatedBy,string TransactionId,string DealVendorId)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var vendorCategoryId = CurrentYear + objUtil.generateUniqueCode("VendorCategory");
            var parTitle = new SqlParameter("@Title", model.Title);
            var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
            var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategory_Id = new SqlParameter("@Category_Name", model.Category_Id);
            var parVendorImage = new SqlParameter("@VendorImage", DBNull.Value);
            var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
            var parDealVendorId = new SqlParameter("@DealVendorId", DealVendorId);
            var parTransactionId = new SqlParameter("@TransactionId", TransactionId);
            if (model.VendorImage != null)
                parVendorImage = new SqlParameter("@VendorImage", model.VendorImage.ToString());
            var parvendorCategory = new SqlParameter("@VendorCategoryId", vendorCategoryId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorUpdateClient @VendorId,@Title,@SubTitle,@Category_Name,@Phone,@Email,@About,@VendorImage,@Location,@CreatedBy,@WebsiteLink,@VendorCategoryId,@DealVendorId,@TransactionId", parVendorId, parTitle, parSubTitle, parCategory_Id, parPhone, parEmail, parAbout, parVendorImage, parLocation, parCreatedBy, parWeb, parvendorCategory, parDealVendorId, parTransactionId).FirstOrDefault();
            return objStatus.ErrorMessage;
        }

        public List<VendorCategoryDDl> GetVendorCategoryDDl()
        {
            List<VendorCategoryDDl> model = new List<VendorCategoryDDl>();
            model = objDB.Database.SqlQuery<VendorCategoryDDl>("select CategoryId[Category_Id],Name from VendorCategory order by Name").ToList();
            return model;
        }

        public Vendor GetVendorByID(string VendorId)
        {
            Vendor objDetails = new Vendor();
            var parVendorId = new SqlParameter("@VendorId", VendorId);
            objDetails = objDB.Database.SqlQuery<Vendor>("udspGetVendorByID @VendorId", parVendorId).FirstOrDefault();
            return objDetails;
        }
   

        public VendorContacts GetVendorContactByID(int VendorContactId)
        {
            VendorContacts objDetails = new VendorContacts();
            var parVendorContactId = new SqlParameter("@VendorContactId", VendorContactId);
            objDetails = objDB.Database.SqlQuery<VendorContacts>("udspGetVendorContactByID @VendorContactId", parVendorContactId).FirstOrDefault();
            return objDetails;
        }


        public IEnumerable<VendorContacts> GetVendorContacts(string VendorId)
        {
            List<VendorContacts> objDetails = new List<VendorContacts>();
            var parVendorContactId = new SqlParameter("@VendorId", VendorId);
            objDetails = objDB.Database.SqlQuery<VendorContacts>("udspGetVendorContacts @VendorId", parVendorContactId).ToList();
            return objDetails;
        }



        public VendorView GetVendorByVendorID(string VendorId)
        {
            VendorView objDetails = new VendorView();
            var parVendorId = new SqlParameter("@VendorId", VendorId);
                     objDetails = objDB.Database.SqlQuery<VendorView>("udspGetVendorByVendorID @VendorId", parVendorId).FirstOrDefault();
            return objDetails;
        }
        public bool GetVendorByLocation(string VendorId,string Location)
        {
            bool objDetails = false;
            var parVendorId = new SqlParameter("@VendorId", VendorId);
            var parLocation = new SqlParameter("@Location", Location==null?"":Location);
            objDetails = objDB.Database.SqlQuery<bool>("udspCheckVendor @VendorId,@Location", parVendorId, parLocation).FirstOrDefault();
            return objDetails;
        }
        public VendorView GetVendorByIDClient(string VendorId)
        {
            VendorView objDetails = new VendorView();
            var parVendorId = new SqlParameter("@VendorId", VendorId);
            objDetails = objDB.Database.SqlQuery<VendorView>("udspGetVendorByID @VendorId", parVendorId).FirstOrDefault();
            return objDetails;
        }
        //for client paging vendor category single
        public VendorPaging GetVendorPrevNext(string VendorId)
        {
            VendorPaging objDetails = new VendorPaging();
            var parVendorId = new SqlParameter("@VendorId", VendorId);
            objDetails = objDB.Database.SqlQuery<VendorPaging>("udspVendorGetPaging @parVendorId", parVendorId).FirstOrDefault();
            return objDetails;
        }
        public string Delete(string VendorId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendorId = new SqlParameter("@VendorId", VendorId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorDelete @VendorId", parVendorId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        //public string DeleteVendorContacts(string VendorContactId)
        //{
        //    int Id = 0;
        //    if (!string.IsNullOrEmpty(VendorContactId))
        //        Id = Convert.ToInt32(VendorContactId);
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parVendorContactId = new SqlParameter("@VendorContactId", Id);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorContactsDeleteTest @VendorContactId", parVendorContactId).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}
        public string DeleteVendorContacts(string VendorId,string VendorContactId)
        {
            int Id = 0;
            if (!string.IsNullOrEmpty(VendorContactId))
               Id = Convert.ToInt32(VendorContactId);
                SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendorId = new SqlParameter("@VendorId", VendorId);
            var parVendorContactId = new SqlParameter("@VendorContactId", Id);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorContactsDelete @VendorId,@VendorContactId", parVendorId, parVendorContactId).FirstOrDefault();
            return objStatus.ErrorCode+"_"+ objStatus.ErrorMessage;
        }

        public string DeleteVendorContactsV2(string VendorContactId)
        {
            int Id = 0;
            if (!string.IsNullOrEmpty(VendorContactId))
                Id = Convert.ToInt32(VendorContactId);
            SPErrorViewModel objStatus = new SPErrorViewModel();
       
            var parVendorContactId = new SqlParameter("@VendorContactId", Id);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspDeleteVendorContacts @VendorContactId", parVendorContactId).FirstOrDefault();
            return objStatus.ErrorCode  + objStatus.ErrorMessage;
        }
        /// <summary>
        /// Added by sonika
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public string DeleteNew(string VendorId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendorId = new SqlParameter("@DealVendorId", VendorId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorDeleteNew @DealVendorId", parVendorId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public string ChangeStatus(string VendorId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendorId = new SqlParameter("@VendorId", VendorId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorChangeStatus @VendorId", parVendorId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<VendorView> GetVendorByVendorType(string VendorType)
        {
            List<VendorView> objList = new List<VendorView>();
            var parCategory = new SqlParameter("@Category", VendorType);
            objList = objDB.Database.SqlQuery<VendorView>("udspGetVendorByVendorType @Category", parCategory).ToList();
            return objList;
        }
        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IEnumerable<SearchAutoCompleteViewModel> VendorAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspVendorGetPagedAutoComplete @SearchTerm", searchParam).ToList();
            return obj;
        }



        public IEnumerable<SearchMultiAutoCompleteViewModel> AdminVendorAutoComplete(string searchTerm, string VendorType)
        {
            List<SearchMultiAutoCompleteViewModel> obj = new List<SearchMultiAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var parVendorType = new SqlParameter("@Category_Id", VendorType);
            obj = objDB.Database.SqlQuery<SearchMultiAutoCompleteViewModel>("udspGetAdminVendorGAutoComplete @SearchTerm,@Category_Id", searchParam, parVendorType).ToList();
            return obj;
        }

        //public IEnumerable<VendorAddressAutocompleteViewModel> VendorAddressAutocomplete(string searchTerm, string Vendor_Id)
        //{
        //    List<VendorAddressAutocompleteViewModel> obj = new List<VendorAddressAutocompleteViewModel>();
        //    var searchParam = new SqlParameter("@SearchTerm", searchTerm);
        //    var parVendorType = new SqlParameter("@Vendor_Id", Vendor_Id);
        //    obj = objDB.Database.SqlQuery<VendorAddressAutocompleteViewModel>("udspGetVendorAddressAutocomplete @SearchTerm,@Vendor_Id", searchParam, parVendorType).ToList();
        //    return obj;
        //}

    }
}
