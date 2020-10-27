using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalAgentListing
    {
        public EFDBContext objDB = new EFDBContext();

        public IEnumerable<DepartmentView> DepartmentDDL()
        {
            List<DepartmentView> objList = new List<DepartmentView>();
            objList = objDB.Database.SqlQuery<DepartmentView>("udspJobDepartmentList").ToList();
            return objList;
        }
    }
}
