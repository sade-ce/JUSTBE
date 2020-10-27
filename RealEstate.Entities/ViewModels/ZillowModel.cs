using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Services;
using RealEstate.Services.Schema;

namespace RealEstate.Entities.ViewModels
{
    public class ZillowModel
    {
        public searchresults results { get; set; }
        public zestimateResultType zest { get; set; }
        public chart chart { get; set; }
        public regionchart regionchart { get; set; }
        public comps comp { get; set; }

        public updatedPropertyDetails Propdetails { get; set; }
    }
}
