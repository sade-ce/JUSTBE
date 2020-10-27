using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalJobDepartment
    {
        public EFDBContext objDB = new EFDBContext();


        public IEnumerable<utblMstCareerDepartment> MstDepartmentList
        {
            get
            {
                return objDB.utblMstCareerDepartments;
            }
        }
        public string Save(utblMstCareerDepartment model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parPosition = new SqlParameter("@Position", model.Position);
            var parDescription = new SqlParameter("@Description", model.Description);
            var parTotalPosition = new SqlParameter("@TotalPosition", model.TotalPosition);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            if (model.DepartmentID != 0)
            {
                var parDepartmentID = new SqlParameter("@DepartmentID", model.DepartmentID);
                objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCareerDepartmentUpdate @DepartmentID,@Position,@Description,@TotalPosition,@UpdatedOn", parDepartmentID, parPosition, parDescription, parTotalPosition, parUpdatedOn).FirstOrDefault();
            }
            else
            {
                objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCareerDepartmentInsert @Position,@Description,@TotalPosition,@UpdatedOn", parPosition, parDescription, parTotalPosition, parUpdatedOn).FirstOrDefault();
            }
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public string Delete(int DepartmentID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parDepartmentID = new SqlParameter("@DepartmentID", DepartmentID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCareerDepartmentDelete @DepartmentID", parDepartmentID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public utblMstCareerDepartment GetByID(int ID)
        {
            utblMstCareerDepartment objItem = objDB.utblMstCareerDepartments.FirstOrDefault(p => p.DepartmentID == ID);
            return objItem;
        }

    }
}
