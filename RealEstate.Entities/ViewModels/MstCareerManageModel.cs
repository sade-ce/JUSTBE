using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstCareerManageModel
    {
        public IEnumerable<MstDepartmentList> MstPositionList { get; set; }
        public MstJobPosition Career { get; set; }
    }
    public class MstDepartmentList
    {
        public int DepartmentID { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public int TotalPosition { get; set; }
        public string UpdatedOn { get; set; }
}

    public class MstJobPosition
    {
        public int JobID { get; set; }
        public string JobContent { get; set; }
        public DateTime Enddate { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Position { get; set; }
    }
}
