using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{

    public class MstNeighborhoodDetails
    {
        public IEnumerable<MstClientNeighborhoodView> MstNeighborhood { get; set; }
    }
    public class MstClientNeighborhoodView
    {
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string PostContent { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
