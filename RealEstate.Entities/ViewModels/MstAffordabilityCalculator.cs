using RealEstate.Entities.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstAffordabilityCalculator
    {
        public IEnumerable<PropertyType> PropertyType { get; set; }
        public IEnumerable<MortgageNumber> MortgageNumber { get; set; }

        public IEnumerable<TranferTaxesPayer> TranferTaxesPayer { get; set; }
        public IEnumerable<Radio> FIRPTA { get; set; }
        public NetProceed NetProceed { get; set;}

    }

    public class MortgageNumber
    {
        public int ddlNumberMortgages { get; set; }
    }
    public class PropertyType
    {
        public string ddlPropertyType { get; set; }
    }

    public class NetProceed
    {
        public string __EVENTTARGET { get; set; }
        public string __EVENTARGUMENT { get; set; }

        public string __LASTFOCUS { get; set; }
        public string __VIEWSTATE { get; set; }
        public string __VIEWSTATEGENERATOR { get; set; }
        public string __EVENTVALIDATION { get; set; }
        public string ddlState { get; set; }
        [Required(ErrorMessage = "Please select property type.")]
        public string ddlPropertyType { get; set; }
        [Required(ErrorMessage = "Please enter the Sale Price.")]

        public double txtSalePrice { get; set; }

        [Required(ErrorMessage = "Please enter the Loan/trust payoff Amount1.")]
        public double txtLoanPayOffs { get; set; }
        [Required(ErrorMessage = "Please enter the Loan/trust payoff Amount2.")]

        public double txtLoanPayOffs2 { get; set; }
        [Required(ErrorMessage = "Please Select Option.")]

        public int ddlNumberMortgages { get; set; }
        [Required(ErrorMessage = "Please Select Option.")]

        public string ddlTranferTaxesPayingBy { get; set; }

        [Required(ErrorMessage = "Please enter the Total amount of your Seller Credit.")]
        public double txtSellerCredit { get; set; }

        [Required(ErrorMessage = "Please enter the Total amount of the Real Estate Company Admin/Flat Fee.")]
        public double txtAdminFee { get; set; }

        [Required(ErrorMessage = "Please enter the Total amount of the Commission to the Real Estate Agent.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        [Percentage]
        public decimal txtCommission { get; set; }

        [Required(ErrorMessage = "Please enter the Settlement Date")]
        public string txtSettlementDate { get; set; }

        [Required(ErrorMessage = "Please enter the Real Estate Property Tax.")]
        public double txtRealEstatePropertyTax { get; set; }

        public double txtTermiteInspection { get; set; }

        public double txtWarranty { get; set; }

        public double txtOtherCharges { get; set; }
        public double txtHOAFee { get; set; }
        [Required(ErrorMessage = "Please Select Option.")]

        public string FIRPTA { get; set; }
        public string btnGenerateQuote { get; set; }

    }

    public class TranferTaxesPayer
    {
        public string ddlTranferTaxesPayingBy { get; set; }

    }

    public class Radio
    {
        public string FIRPTA { get; set;}
    }
}
