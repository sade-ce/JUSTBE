using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstCompassAutocompleteView
    {
        //public int status { get; set; }
        //public object errors { get; set; }
        // public Response response { get; set; }

        //-------------- Added by Neha ------------//
        public List<string> addresses { get; set; }
        public List<AddressesInfo> addresses_info { get; set; }
        //-------------- Added by Neha ------------//
    }

    public class AddressesInfo
    {
        public int listingType { get; set; }
        public double bathrooms { get; set; }
        public int salesStatus { get; set; }
        public double price { get; set; }
        public double bedrooms { get; set; }
        public string mrisId { get; set; }
        public string zipCode { get; set; }
        public string address { get; set; }
        public string listingIdSHA { get; set; }
        public string buildingIdSHA { get; set; }
        public bool exclusive { get; set; }
        public object lastStatusChangeDate { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Response
    {
        public List<string> addresses { get; set; }
        public List<AddressesInfo> addresses_info { get; set; }
    }
}
