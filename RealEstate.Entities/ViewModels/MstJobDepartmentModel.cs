using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
    public class MstJobDepartmentModel
    {
        public IEnumerable<utblMstCareerDepartment> MstDepartmentList { get; set; }
        public utblMstCareerDepartment utblMstCareerDepartments { get; set; }
    }

    public class MstDepartmentView
    {
        public int DepartmentID { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
