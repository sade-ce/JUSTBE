using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
   public class MstClosingConfigViewModel
    {
        public IEnumerable<MstListingType> ListingTypeDDL { get; set; }
        public IEnumerable<HomeType> HomeTypeDDL { get; set; }
        public IEnumerable<State> StateDDL { get; set; }
       
        public utblMstClosingDate utblMstClosingDate { get; set; }
    }
    public class MstListingType
    {
        public int ListingTypeID { get; set; }
        public string ListingType { get; set; }
    }

    public class HomeType
    {
        public int HomeTypeId { get; set; }
        public string Name { get; set; }
    }
    public class State
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
   
}
