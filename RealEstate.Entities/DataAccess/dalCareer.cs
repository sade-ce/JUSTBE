using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.Models;
using System.Data.SqlClient;

namespace RealEstate.Entities.DataAccess
{
    public class dalCareer
    {
        public EFDBContext objDB = new EFDBContext();
        public List<MstDepartmentList> GetDepartmentList()
        {
            return objDB.Database.SqlQuery<MstDepartmentList>("SELECT utblMstCareerDepartments.Position,CONVERT(VARCHAR(12), utblMstCareerDepartments.UpdatedOn, 107) AS [UpdatedOn] ,utblMstCareerDepartments.Description, utblMstCareerDepartments.DepartmentID, utblMstCareerDepartments.TotalPosition FROM utblMstCareerDepartments INNER JOIN utblMstJobPositions ON utblMstCareerDepartments.DepartmentID = utblMstJobPositions.DepartmentID").ToList();
        }

        public MstJobPosition GetJobByDepartmentID(int DepartmentID)
        {
            MstJobPosition objDetails = new MstJobPosition();
            var parDepartmentID = new SqlParameter("@DepartmentID", DepartmentID);
            objDetails = objDB.Database.SqlQuery<MstJobPosition>("udspGetJobByDepartmentID @DepartmentID", parDepartmentID).FirstOrDefault();
            return objDetails;
        }
    }
}
