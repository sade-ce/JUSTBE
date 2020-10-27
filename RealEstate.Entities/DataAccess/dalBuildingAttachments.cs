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
    public class dalBuildingAttachments
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public IEnumerable<BuildingAttachmentsModel> BuildingAttachmentsGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<BuildingAttachmentsModel> objList = new List<BuildingAttachmentsModel>();
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
            objList = objDB.Database.SqlQuery<BuildingAttachmentsModel>("udspBuildingAttachmentsGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public string Save(BuildingAttachments model, string CreatedBy)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parName = new SqlParameter("@Name", model.Name);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("Usp_BuildingAttachments_Insert @Name,@CreatedBy", parName, parCreatedBy).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string Edit(BuildingAttachmentsModel model, string CreatedBy)
        {
        
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parCategoryId = new SqlParameter("@BuildingAttachmentId", model.BuildingAttachmentId);
            var parName = new SqlParameter("@Name", model.Name);
            var parCreatedBy = new SqlParameter("@UpdatedBy", CreatedBy);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("Usp_BuildingAttachments_Update @BuildingAttachmentId,@Name,@UpdatedBy", parCategoryId, parName, parCreatedBy).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public BuildingAttachmentsModel GetBuildingAttachmentsByID(int BuildingAttachmentId)
        {
            BuildingAttachmentsModel objDetails = new BuildingAttachmentsModel();
            var parBuildingAttachmentId = new SqlParameter("@BuildingAttachmentId", BuildingAttachmentId);
            objDetails = objDB.Database.SqlQuery<BuildingAttachmentsModel>("Usp_BuildingAttachments_GetById @BuildingAttachmentId", parBuildingAttachmentId).FirstOrDefault();
            return objDetails;
        }
        //for client paging vendor category single
        public BuildingAttachmentsPaging GetBuildingAttachmentsPrevNext(int BuildingAttachmentId)
        {
            BuildingAttachmentsPaging objDetails = new BuildingAttachmentsPaging();
            var parBuildingAttachmentId = new SqlParameter("@BuildingAttachmentId", BuildingAttachmentId);
            objDetails = objDB.Database.SqlQuery<BuildingAttachmentsPaging>("udspBuildingAttachmentsetPaging @BuildingAttachmentId", parBuildingAttachmentId).FirstOrDefault();
            return objDetails;
        }
        public string Delete(int BuildingAttachmentId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parBuildingAttachmentId = new SqlParameter("@BuildingAttachmentId", BuildingAttachmentId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("Usp_BuildingAttachments_Delete @BuildingAttachmentId", parBuildingAttachmentId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
      
        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// 
        public IEnumerable<SearchAutoCompleteBuildingViewModel> BuildingAttachmentsAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteBuildingViewModel> obj = new List<SearchAutoCompleteBuildingViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteBuildingViewModel>("udspBuildingAttachmentsGetPagedAutoComplete @SearchTerm", searchParam).ToList();
            return obj;
        }

      }
}
