using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Calculator;
namespace RealEstate.Entities.DataAccess
{
    public class dalMortgagecalculator
    {
        EFDBContext ObjDB = new EFDBContext();
        MstMortgageCalculator Obj = new MstMortgageCalculator();
        public List<MstCalcRegion> StateDropDown()
        {
            List<MstCalcRegion> Obj = new List<MstCalcRegion>();
            Obj.Add(new MstCalcRegion { StateID = 1, State = "District Of Columbia" });
            Obj.Add(new MstCalcRegion { StateID = 2, State = "Maryland" });
            Obj.Add(new MstCalcRegion { StateID = 3, State = "Virginia" });

            return Obj;

        }

        public List<MstCalcTimePeriod> YearDropDown()
        {

            List<MstCalcTimePeriod> ObjTime = new List<MstCalcTimePeriod>();
            ObjTime.Add(new MstCalcTimePeriod { TimeFrame = 15 });
            ObjTime.Add(new MstCalcTimePeriod { TimeFrame = 30 });
            return ObjTime;
        }

        public MstMortgageCalculator Calculate(MstMortgageCalculator Data)
        {
            Data.Operations = new MstCalcOperations();
            Data.DirectCost = new MstCalcDirectCost();
            if (Data.Inputs.DownPayment <= 0 && Data.Inputs.DownPercent > 0)
            {
                Data.Inputs.DownPayment = (Data.Inputs.HousePrice * Data.Inputs.DownPercent) / 100;
            }

            if (Data.Inputs.DownPercent <= 0 && Data.Inputs.DownPayment > 0)
            {
                Data.Inputs.DownPercent = (Data.Inputs.DownPayment / Data.Inputs.HousePrice) * 100;

            }


            if (Data.Inputs.DownPercent >= 0)
            {
                Data.Inputs.DownPayment = (Data.Inputs.HousePrice * Data.Inputs.DownPercent) / 100;

                //Data.Inputs.DownPercent = (Data.Inputs.DownPayment / Data.Inputs.HousePrice) * 100;
            }

            Data.Operations.Principal = Data.Inputs.HousePrice - Data.Inputs.DownPayment;
            Data.Operations.Rate = Data.Inputs.MortgageRate;
            Data.Operations.TotalMonths = Data.Inputs.TimeFrame * 12;
            Data.Operations.RatePerMonth = (Data.Inputs.MortgageRate / 12)/100;
            Data.Operations.CompoundRate = Math.Pow((1 + Data.Operations.RatePerMonth), Data.Operations.TotalMonths);
            Data.Operations.Multiplier = (Data.Operations.RatePerMonth * Data.Operations.CompoundRate) / (Data.Operations.CompoundRate - 1);
            Data.Operations.Monthly = Data.Operations.Multiplier * Data.Operations.Principal;
            Data.Operations.InterestPerMonth = Data.Operations.Principal * Data.Operations.RatePerMonth;
            Data.Operations.PrincipalPerMonth = Data.Operations.Monthly - Data.Operations.InterestPerMonth;

            //Display Results
            Data.DirectCost.DownPayment = Data.Inputs.DownPayment;
            Data.DirectCost.DownPercent = Data.Inputs.DownPercent;
            Data.DirectCost.TimeFrame = Data.Inputs.TimeFrame;
            Data.DirectCost.Mortgage = Math.Round(Data.Operations.Monthly);
            Data.DirectCost.Principal = Math.Round(Data.Operations.PrincipalPerMonth);
            Data.DirectCost.Interest = Math.Round(Data.Operations.InterestPerMonth);
            Data.DirectCost.Utilities = Data.Inputs.Utilities;
            Data.DirectCost.HomeInsurance = Data.Inputs.HomeInsurance;
            Data.DirectCost.PMI = Data.Inputs.PMI;
            Data.DirectCost.Maintenance = Data.Inputs.Maintenance;
            Data.DirectCost.HOAFees = Data.Inputs.HOAFees;
            //Data.DirectCost.PropertyTax = Math.Round(((Data.Inputs.HousePrice * 0.85) / 100) / 12);

            //Data.DirectCost.PropertyTax =Math.Round(((Data.Inputs.HousePrice * 0.57) / 100) / 12);
            Data.DirectCost.PropertyTax =Math.Round(((Data.Inputs.HousePrice * Data.Inputs.PropertyTaxPercent) / 100) / 12);

            Data.DirectCost.PrincipalAndInterest = Data.DirectCost.Principal + Data.DirectCost.Interest;
            Data.DirectCost.SubTotal = Math.Round(Data.DirectCost.Mortgage + Data.Inputs.Utilities + Data.Inputs.HomeInsurance + Data.Inputs.PMI + Data.Inputs.Maintenance + Data.Inputs.HOAFees + Data.DirectCost.PropertyTax);

            double Total = Data.DirectCost.PrincipalAndInterest + Data.DirectCost.PropertyTax + Data.DirectCost.HOAFees + Data.DirectCost.HomeInsurance;
            Data.DirectCost.PrincipalAndInterestWidth = (Data.DirectCost.PrincipalAndInterest / Total) * 100;
            Data.DirectCost.PropertyTaxWidth = (Data.DirectCost.PropertyTax / Total) * 100;
            Data.DirectCost.HOAFeesWidth = (Data.DirectCost.HOAFees / Total) * 100;
            Data.DirectCost.HomeInsuranceWidth = (Data.DirectCost.HomeInsurance / Total) * 100;

            return Data;
        }
    }
}
