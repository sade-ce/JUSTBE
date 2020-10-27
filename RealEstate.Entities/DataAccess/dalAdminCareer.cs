using RealEstate.Entities.Models;
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
    public class dalAdminCareer
    {
        public EFDBContext objDB = new EFDBContext();
        public IEnumerable<DepartmentView> DepartmentDDL()
        {
            List<DepartmentView> objList = new List<DepartmentView>();
            objList = objDB.Database.SqlQuery<DepartmentView>("udspJobDepartmentList").ToList();
            return objList;
        }
        public IEnumerable<AdminCareerView> JobGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<AdminCareerView> objList = new List<AdminCareerView>();
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
            objList = objDB.Database.SqlQuery<AdminCareerView>("udspJobGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public string Save(utblMstJobPosition model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parDepartmentID = new SqlParameter("@DepartmentID", model.DepartmentID);
            var parJobContent = new SqlParameter("@JobContent", model.JobContent);
            var parEndDate = new SqlParameter("@EndDate", model.EndDate);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            if (model.JobID != 0)
            {
                var parJobID = new SqlParameter("@JobID", model.JobID);
                objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspJobUpdate @JobID,@DepartmentID,@JobContent,@EndDate,@UpdatedOn", parJobID, parDepartmentID, parJobContent, parEndDate, parUpdatedOn).FirstOrDefault();
            }
            else
            {
                objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspJobInsert @DepartmentID,@JobContent,@EndDate,@UpdatedOn", parDepartmentID, parJobContent, parEndDate, parUpdatedOn).FirstOrDefault();
            }
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public utblMstJobPosition GetJobID(int JobID)
        {
            utblMstJobPosition objDetails = new utblMstJobPosition();
            var parJobID = new SqlParameter("@JobID", JobID);
            objDetails = objDB.Database.SqlQuery<utblMstJobPosition>("udspGetJobByID @JobID", parJobID).FirstOrDefault();
            return objDetails;
        }
        public string Delete(int JobID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parJobID = new SqlParameter("@JobID", JobID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspJobDelete @JobID", parJobID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


    }
}
