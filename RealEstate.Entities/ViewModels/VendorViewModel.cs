using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class VendorViewModel
    {
        public Vendor Vendor { get; set; }
        public VendorView VendorViews { get; set; }
        public VendorContacts VendorContacts { get; set; }
        //public VendorContactView VendorContactView { get; set; }
        public IEnumerable<VendorView> VendorList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public VendorPaging VendorPaging { get; set; }
        public List<VendorCategoryDDl> CategoryDropDown { get; set; }
        public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }

        //New Version Start
        public VendorSortingInfo VendorSortingInfo { get; set; }
        public VendorFilterInfo VendorFilterInfo { get; set; }
        //New Version End

    }

    //New Version Start
    public class VendorSortingInfo
    {
        public string CurrentSort { get; set; }
        public string TitleSort { get; set; }
        public string VendorTypeSort { get; set; }
        public string CreatedOnSort { get; set; }
        public string CreatedBySort { get; set; }
        public string UserTypeSort { get; set; }
        public string IsRecommendedSort { get; set; }
    }

    public class VendorFilterInfo
    {
        public string SearchFilter { get; set; }
    }
    //New Version End
}
