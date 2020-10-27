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
    public class dalResources
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public IEnumerable<ResourceLinksView> BuyerResourcesGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<ResourceLinksView> objList = new List<ResourceLinksView>();
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
            objList = objDB.Database.SqlQuery<ResourceLinksView>("udspResourceGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        public IEnumerable<ResourceLinksView> GetResourcesLinks(int Id)
        {
            List<ResourceLinksView> objList = new List<ResourceLinksView>();
            var parTypeId = new SqlParameter("@Type_Id", Id);
            objList = objDB.Database.SqlQuery<ResourceLinksView>("udspGetResourceLinks @Type_Id", parTypeId).ToList();
   
            return objList;
        }
        public IEnumerable<ResourceTitlesView> GetResourceTitles(int Id)
        {
            List<ResourceTitlesView> objList = new List<ResourceTitlesView>();
            var parTypeId = new SqlParameter("@Type_Id", Id);
            objList = objDB.Database.SqlQuery<ResourceTitlesView>("udspGetResourcesTitle @Type_Id", parTypeId).ToList();

            return objList;
        }

        public ResourceTypeView Getype(int Id)
        {
           
         
            return objDB.Database.SqlQuery<ResourceTypeView>("select * from  ResourceType where TypeId="+Id).FirstOrDefault();


        }


        public string GeLinkTitle(int Id)
        {


            return objDB.Database.SqlQuery<string>("select Link from  ResourceLinks where LinkId=" + Id).FirstOrDefault();


        }
        //public string Save(BuyerResources model, string CreatedBy)
        //{
        //    DateTime dt = DateTime.Now;
        //    var CurrentYear = dt.ToString("yyyy");
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parUserType = new SqlParameter("@UserType", model.UserType);
        //    var parUsername = new SqlParameter("@UserName", model.UserName);
        //    var parDescription = new SqlParameter("@Description", model.Description == null ? "" : model.Description);
        //    var parRating = new SqlParameter("@Rating", model.Rating);
        //    var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuyerResourcesInsert @UserName,@UserType,@Description,@Rating,@CreatedBy", parUsername, parUserType, parDescription, parRating, parCreatedBy).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}


        //public string Edit(BuyerResources model, string CreatedBy)
        //{
        //    DateTime dt = DateTime.Now;
        //    var CurrentYear = dt.ToString("yyyy");
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parId = new SqlParameter("@Id", model.Id);
        //    var parUserType = new SqlParameter("@UserType", model.UserType);
        //    var parUsername = new SqlParameter("@UserName", model.UserName);
        //    var parDescription = new SqlParameter("@Description", model.Description == null ? "" : model.Description);
        //    var parRating = new SqlParameter("@Rating", model.Rating);
        //    var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuyerResourcesUpdate @Id,@UserType,@UserName,@Description,@Rating,@CreatedBy", parId, parUserType, parUsername, parDescription, parRating, parCreatedBy).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}

        //public BuyerResources GetBuyerResourcesByID(int Id)
        //{
        //    BuyerResources objDetails = new BuyerResources();
        //    var parID = new SqlParameter("@Id", Id);
        //    objDetails = objDB.Database.SqlQuery<BuyerResources>("udspGetBuyerResourcesByID @Id", parID).FirstOrDefault();
        //    return objDetails;
        //}
        //public BuyerResourcesPaging GetBuyerResourcesPrevNext(int Id)
        //{
        //    BuyerResourcesPaging objDetails = new BuyerResourcesPaging();
        //    var parID = new SqlParameter("@Id", Id);
        //    objDetails = objDB.Database.SqlQuery<BuyerResourcesPaging>("udspBuyerResourcesGetPaging @Id", parID).FirstOrDefault();
        //    return objDetails;
        //}
        //public string Delete(int Id)
        //{
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parID = new SqlParameter("@Id", Id);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuyerResourcesDelete @Id", parID).FirstOrDefault();
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;
        //}
    }
}
