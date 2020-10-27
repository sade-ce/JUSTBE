using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Calculator
{
    public class MstMortgageCalculator
    {
        public IEnumerable<MstCalcRegion> Region { get; set; }
        public IEnumerable<MstCalcTimePeriod> TimePeriod { get; set; }
        public MstCalcInputs Inputs { get; set; }
        public MstCalcDirectCost DirectCost { get; set; }
        public MstCalcOperations Operations { get; set; }


    }
}
