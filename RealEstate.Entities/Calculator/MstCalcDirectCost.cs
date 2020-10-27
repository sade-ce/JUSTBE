using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Calculator
{
    public class MstCalcDirectCost
    {
        public double Mortgage { get; set; }
        public double Principal { get; set; }
        public double Interest { get; set; }
        public double PrincipalAndInterest { get; set; }
        public double PrincipalAndInterestWidth { get; set; }
        public double Utilities { get; set; }
        public double HomeInsurance { get; set; }
        public double HomeInsuranceWidth { get; set; }
        public double PMI { get; set; }
        public double Maintenance { get; set; }
        public double HOAFees { get; set; }
        public double HOAFeesWidth { get; set; }
        public double PropertyTax { get; set; }
        public double PropertyTaxWidth { get; set; }
        public double DownPercent { get; set; }
        public double DownPayment { get; set; }
        public int TimeFrame { get; set; }

        public double SubTotal { get; set; }

        


    }
}
