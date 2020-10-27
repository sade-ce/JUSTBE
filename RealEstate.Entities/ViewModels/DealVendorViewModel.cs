using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class DealVendorViewModel
    {
        public UserProfileView UserProfile { get; set; }
        public IEnumerable<DealVendorView> DealVendorList { get; set; }
        public VendorContactView VendorContact { get; set; }
        public DealVendor DealVendor { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public List<DealVendorCategoryDDl> CategoryDropDown { get; set; }
        public List<DealVendorDDl> VendorDropDown { get; set; }
        public List<VendorContactView> VendorContactDropDown { get; set; }
    }
}
