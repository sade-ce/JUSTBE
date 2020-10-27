using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class DealVendorView
    {
        public string DealVendorId { get; set; }
        public string ClientID { get; set; }
        public string Transaction_ID { get; set; }
        public string Vendor { get; set; }
        public string VendorType { get; set; }
        public string Vendor_Id { get; set; }
        public string Email { get; set; }
        public string VendorEmail { get; set; }
        public string VendorPhone { get; set; }
        public string VendorTypeImage { get; set; }
        public string VendorCompany { get; set; }
        public string CreatedBy { get; set; }
        public string UserRole { get; set; }  //added by sonika
        public string VendorImage { get; set; } //added by sonika

        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
        public List<DealVendorContacts> VendorContacts { get; set; }
    }
    public class DealVendorCategoryDDl
    {
        public string Category_Id { get; set; }
        public string Name { get; set; }
    }
    public class DealVendorDDl
    {
        public string Vendor_Id { get; set; }
        public string Vendor { get; set; }
    }
    public class DealVendorContacts
    {
        public int VendorContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
    }
}
