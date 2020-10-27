using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class VendorCategoryView
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string CategoryImage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Count { get; set; }
        public string DisplayType { get; set; }
        public string UserRole { get; set; }
    }
    public class VendorCategoryPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
}
