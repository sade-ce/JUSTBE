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
    public class dalVendorCategory
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public IEnumerable<VendorCategoryView> VendorCategoryGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<VendorCategoryView> objList = new List<VendorCategoryView>();
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
            objList = objDB.Database.SqlQuery<VendorCategoryView>("udspVendorCategoryGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public string Save(VendorCategory model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            model.CategoryId = CurrentYear + objUtil.generateUniqueCode("VendorCategory");
            var parCategoryId = new SqlParameter("@CategoryId", model.CategoryId);
            var parName = new SqlParameter("@Name", model.Name);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategoryImage = new SqlParameter("@CategoryImage", DBNull.Value);
            if (model.CategoryImage != null)
                parCategoryImage = new SqlParameter("@CategoryImage", model.CategoryId.ToString()+ model.CategoryImage.ToString());
            var parDisplayType = new SqlParameter("@DisplayType", "Double");
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorCategoryInsert @CategoryId,@Name,@CategoryImage,@CreatedBy,@DisplayType", parCategoryId, parName, parCategoryImage, parCreatedBy,parDisplayType).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string Edit(VendorCategory model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parCategoryId = new SqlParameter("@CategoryId", model.CategoryId);
            var parName = new SqlParameter("@Name", model.Name);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategoryImage = new SqlParameter("@CategoryImage", DBNull.Value);
            if (model.CategoryImage != null)
                parCategoryImage = new SqlParameter("@CategoryImage",  model.CategoryImage.ToString());
            var parDisplayType = new SqlParameter("@DisplayType", "Double");
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorCategoryUpdate @CategoryId,@Name,@CategoryImage,@CreatedBy,@DisplayType", parCategoryId, parName, parCategoryImage, parCreatedBy, parDisplayType).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public VendorCategory GetVendorCategoryByID(string CategoryId)
        {
            VendorCategory objDetails = new VendorCategory();
            var parCategoryId = new SqlParameter("@CategoryId", CategoryId);
            objDetails = objDB.Database.SqlQuery<VendorCategory>("udspGetVendorCategoryByID @CategoryId", parCategoryId).FirstOrDefault();
            return objDetails;
        }
        //for client paging vendor category single
        public VendorCategoryPaging GetVendorCategoryPrevNext(string CategoryId)
        {
            VendorCategoryPaging objDetails = new VendorCategoryPaging();
            var parCategoryId = new SqlParameter("@CategoryId", CategoryId);
            objDetails = objDB.Database.SqlQuery<VendorCategoryPaging>("udspVendorCategoryGetPaging @CategoryId", parCategoryId).FirstOrDefault();
            return objDetails;
        }
        public string Delete(string CategoryId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parCategoryId = new SqlParameter("@CategoryId", CategoryId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorCategoryDelete @CategoryId", parCategoryId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public IEnumerable<VendorCategoryView> GetVendorTypes()
        {
            List<VendorCategoryView> objList = new List<VendorCategoryView>();
            objList = objDB.Database.SqlQuery<VendorCategoryView>("udspGetVendorTypes").ToList();
            return objList;
        }
        public VendorView GetVendorById(string Vendor)
        {
            VendorView vendor = new VendorView();
            vendor = objDB.Database.SqlQuery<VendorView>("udspGetVendorByID").FirstOrDefault(); ;
            return vendor;
        }
        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public IEnumerable<SearchAutoCompleteViewModel> VendorTypeAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspVendorCategoryGetPagedAutoComplete @SearchTerm", searchParam).ToList();
            return obj;
        }

        public IEnumerable<SearchMultiAutoCompleteViewModel> ClientVendorAutoComplete(string searchTerm, string ClientId,string VendorType)
        {
            List<SearchMultiAutoCompleteViewModel> obj = new List<SearchMultiAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var parClientId = new SqlParameter("@ClientId", ClientId);
            var parVendorType = new SqlParameter("@CategoryName", VendorType);
            obj = objDB.Database.SqlQuery<SearchMultiAutoCompleteViewModel>("udspGetClientVendorGAutoComplete @SearchTerm,@ClientId,@CategoryName", searchParam, parClientId, parVendorType).ToList();
            return obj;
        }

        public IEnumerable<SearchClientDocumentAutoCompleteViewModel> ClientDocumentAutoComplete(string searchTerm, string ClientId,string TransactionId)
        {
            List<SearchClientDocumentAutoCompleteViewModel> obj = new List<SearchClientDocumentAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var parClientId = new SqlParameter("@ClientId", ClientId);
            var parTransactionId = new SqlParameter("@TransactionId", TransactionId);
            obj = objDB.Database.SqlQuery<SearchClientDocumentAutoCompleteViewModel>("udspGetClientDocumentAutoComplete @SearchTerm,@ClientId,@TransactionId", searchParam, parClientId, parTransactionId).ToList();
            return obj;
        }
    }
}
