using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class VendorView
    {
        public string VendorId { get; set; }
        public string Title { get; set; }
        public string TitleNew { get; set; }
        public string SubTitle { get; set; }
        public string VendorType { get; set; }
        public string Category_Id { get; set; }
        public string Category_Name { get; set; } // added by sonika
        public string Phone { get; set; }
        public string Email { get; set; }
        public string About { get; set; }
        public string VendorImage { get; set; }
        public string Location { get; set; }
        public bool IsRecommended { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string WebsiteLink { get; set; }
        public string DisplayType { get; set; }
        public string UserType { get; set; } // Added by Sonika

      //  Vendor Reviews
        public string ClientId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ContactName { get; set; }
        public string VendorContactId { get; set; }
    }
    public class VendorPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
    public class VendorCategoryDDl
    {
        public string Category_Id { get; set; }
        public string Name { get; set; }
    }


  
}
