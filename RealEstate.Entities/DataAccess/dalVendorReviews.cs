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
    public class dalVendorReviews
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        //public IEnumerable<VendorsReviewModel> RulesGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        //{
        //    List<VendorsReviewModel> objList = new List<VendorsReviewModel>();
        //    if (SearchTerm != "")
        //    {
        //        SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
        //    }
        //    var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
        //    var parPageSize = new SqlParameter("@PageSize", PageSize);
        //    var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
        //    var spOutput = new SqlParameter
        //    {
        //        ParameterName = "@TotalCount",
        //        SqlDbType = System.Data.SqlDbType.Int,
        //        Direction = System.Data.ParameterDirection.Output
        //    };
        //    objList = objDB.Database.SqlQuery<VendorsReviewModel>("udspRulesGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
        //    TotalRows = int.Parse(spOutput.Value.ToString());
        //    return objList;
        //}
        public string Save(VendorsReview model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parName = new SqlParameter("@ClientId", model.ClientId);
            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var parRating = new SqlParameter("@Rating", model.Rating);
            var parComment = new SqlParameter("@Comment", model.Comment == null?"":model.Comment);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ReviewsInsert @ClientId,@VendorId,@Rating,@Comment", parName, parVendorId, parRating,parComment).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        //public string Edit(VendorsReviewModel model, string CreatedBy)
        //{

        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parCategoryId = new SqlParameter("@RuleId", model.RuleId);
        //    var parName = new SqlParameter("@Name", model.Name);
        //    var parCreatedBy = new SqlParameter("@UpdatedBy", CreatedBy);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("Usp_Rules_Update @RuleId,@Name,@UpdatedBy", parCategoryId, parName, parCreatedBy).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}

        //public VendorsReviewModel GetRulesByID(int RuleId)
        //{
        //    VendorsReviewModel objDetails = new VendorsReviewModel();
        //    var parRuleId = new SqlParameter("@RuleId", RuleId);
        //    objDetails = objDB.Database.SqlQuery<VendorsReviewModel>("Usp_Rules_GetById @RuleId", parRuleId).FirstOrDefault();
        //    return objDetails;
        //}
        ////for client paging vendor category single
        //public RulesPaging GetRulesPrevNext(int ruleId)
        //{
        //    RulesPaging objDetails = new RulesPaging();
        //    var parRuleId = new SqlParameter("@RuleId", ruleId);
        //    objDetails = objDB.Database.SqlQuery<RulesPaging>("udspRulesetPaging @RuleId", parRuleId).FirstOrDefault();
        //    return objDetails;
        //}
        //public string Delete(int RuleId)
        //{
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parRuleId = new SqlParameter("@RuleId", RuleId);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("Usp_Rules_Delete @RuleId", parRuleId).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}

        ///// <summary>
        ///// added by sonika
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="CreatedBy"></param>
        ///// <returns></returns>
        ///// 
        //public IEnumerable<SearchAutoCompleteViewModel> RuleSAutoComplete(string searchTerm)
        //{
        //    List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
        //    var searchParam = new SqlParameter("@SearchTerm", searchTerm);
        //    obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspRulesGetPagedAutoComplete @SearchTerm", searchParam).ToList();
        //    return obj;
        //}
    }
}
