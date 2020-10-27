using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;

namespace RealEstate.Entities.ViewModels
{
    public class VendorCategoryViewModel
    {
        public VendorCategory VendorCategory { get; set; }
        public VendorCategoryView VendorCategoryViews { get; set; }
        public IEnumerable<VendorCategoryView> VendorCategoryList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public VendorCategoryPaging VendorCategoryPaging { get; set; }
        public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }
        public IEnumerable<VendorContactsAutocompleteViewModel> VendorContactsAutocomplete { get; set; }
        //public IEnumerable<VendorAddressAutocompleteViewModel> VendorAddressAutocomplete { get; set; }
        public IEnumerable<SearchMultiAutoCompleteViewModel> ClientVendorsListSearchAutoComplete { get; set; }
        public IEnumerable<SearchClientDocumentAutoCompleteViewModel> ClientDocumentAutoComplete { get; set; }
        public IEnumerable<SearchAutoCompleteEventViewModel> EventListSearchAutoComplete { get; set; }
    }
}
