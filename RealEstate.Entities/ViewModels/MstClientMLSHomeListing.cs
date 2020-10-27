using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientMLSHomeListing
    {
        public IEnumerable<utblMstClientHomeListing> MstClientHomeList { get; set; }
        public MstInfoView MstInfoView { get; set; }
        public IEnumerable<ListingTypes> ListingDDL { get; set; }
        public utblMstClientHomeListing utblMstClientHomeListing { get; set; }
    }

    public class ListingTypes
    {
        public int ListingTypeID { get; set; }
        public string ListingType { get; set; }
    }
}
