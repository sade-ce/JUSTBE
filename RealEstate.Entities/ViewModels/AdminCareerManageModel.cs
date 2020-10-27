using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
    public
         class AdminCareerManageModel
    {
        public utblMstJobPosition utblMstJobPositions { get; set; }
        public AdminCareerView AdminCareerView { get; set; }
        public IEnumerable<AdminCareerView> CareerList { get; set; }
        public IEnumerable<DepartmentView> DepartmentDDL { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
