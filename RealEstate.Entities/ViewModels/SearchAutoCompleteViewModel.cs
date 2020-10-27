using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    /// <summary>
    /// Added by sonika
    /// </summary>

    public class SearchAutoCompleteViewModel
    {
        public string searchResult { get; set; }
    }

    public class SearchMultiAutoCompleteViewModel
    {
        public string searchResult { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CatgoryName { get; set; }
        public string Company { get; set; }
        public string WebsiteLink { get; set; }
        public string Location { get; set; }
        public string VendorImage { get; set; }
        public string Description { get; set; }
        public string VendorId { get; set; }
        public string Title { get; set; }
    }

    public class VendorContactsAutocompleteViewModel
    {
        public string searchResult { get; set; }
        public int VendorContactId { get; set; }
        public string Vendor_Id { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
    }

    //public class VendorAddressAutocompleteViewModel
    //{
    //    public string searchResult { get; set; }
    //    public string Vendor_Id { get; set; }
    //}

    public class SearchClientDocumentAutoCompleteViewModel
    {
        public string searchResult { get; set; }
        public string Description { get; set; }
    }
    public class SearchAutoCompleteEventViewModel
    {
        public string searchResult { get; set; }
        public int Id { get; set; }
    }
    public class SearchAutoCompleteBuildingViewModel
    {
        public string searchResult { get; set; }
        public int Id { get; set; }
    }
    public class SearchAutoCompleteNeighborhod
    {
        public string searchResult { get; set; }
        public int CityID { get; set; }
    }
    public class SearchAutoCompleteClient
    {
        public string searchResult { get; set; }
        public string Id { get; set; }
    }

    public class SearchAutoCompleteAgent
    {
        public string searchResult { get; set; }
        public string Id { get; set; }
    }
}
