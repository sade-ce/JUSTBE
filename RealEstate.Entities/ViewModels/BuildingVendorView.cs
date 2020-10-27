using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class BuildingVendorView
    {
        public int BuildingVendorId { get; set; }
        public int Building_ID { get; set; }
        public string Vendor { get; set; }
        public string VendorType { get; set; }
        public string Vendor_Id { get; set; }
        public string Email { get; set; }
        public string VendorEmail { get; set; }
        public string VendorPhone { get; set; }
        public string VendorTypeImage { get; set; }
        public string VendorCompany { get; set; }
        public string CreatedBy { get; set; }
        public string UserRole { get; set; }  
        public string VendorImage { get; set; }

    }
    
}
