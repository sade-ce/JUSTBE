using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Calculator
{
    public class MstCalcInputs
    {
        public int StateID { get; set; }
        public int TimeFrame { get; set; }
        public double HousePrice { get; set; }
        public double MortgageRate { get; set; }
        public double DownPercent { get; set; }
        public double DownPayment { get; set; }
        public double Utilities { get; set; }
        public double HomeInsurance { get; set; }
        public double HomeInsurancePercent { get; set; }
        public double PMI { get; set; }
        public double Maintenance { get; set; }
        public double HOAFees { get; set; }
        public double PropertyTax { get; set; }
        public double PropertyTaxPercent { get; set; }

    }
}
