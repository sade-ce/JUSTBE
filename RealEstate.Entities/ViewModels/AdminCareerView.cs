using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;

namespace RealEstate.Entities.ViewModels
{
    public class AdminCareerView
    {
        public long RowID { get; set; }
        public int JobID { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class DepartmentView
    {
        public int DepartmentID { get; set; }
        public string Position { get; set; }
    }


}
