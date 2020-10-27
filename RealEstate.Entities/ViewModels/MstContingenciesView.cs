using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstContingenciesView
    {
        public int StatusID { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsContingencies { get; set; }

        public int NoOfDays { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

  
}
