using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstBuyerClosingCosts
    {
        public MSTRefinance MSTRefinance { get; set; }
        public IEnumerable<TransactionTypes> TransactionTypes { get; set; }
        public IEnumerable<LoanTypes> MstLoanTypes { get; set; }
        public IEnumerable<PropertyTypes> PropertyTypes { get; set; }

        public IEnumerable<PropertyUses> PropertyUses { get; set; }
        public IEnumerable<NoOfMortgages> NoOfMortgages { get; set; }

    }
    public class MSTRefinance
    {
        public string sm_HiddenField { get; set; }
        public string __EVENTTARGET { get; set; }
        public string __EVENTARGUMENT { get; set; }
        public string __LASTFOCUS { get; set; }
        public string __VIEWSTATE { get; set; }
        public string __VIEWSTATEGENERATOR { get; set; }
        public string __EVENTVALIDATION { get; set; }
        public string ddlTransactionType { get; set; }
        public string ddlLoanType { get; set; }
        public string txtEmailAddress { get; set; }
        public string txtStreetAddress { get; set; }
        public string ddlState { get; set; }
        public string ddlPropertyType { get; set; }
        public string ddlPropertyUse { get; set; }
        public string txtPurchasePrice { get; set; }
        public string txtLoanAmount { get; set; }
        public string txtLoanAmount2 { get; set; }
        public string ddlTranferTaxesPayingBy { get; set; }
        public string txtCurrentLoanAmount { get; set; }
        public string ddlNumberMortgages { get; set; }
        public string txtNewLoanAmount { get; set; }
        public string txtSecondLoanAmount { get; set; }
        public string rblRepeatClient { get; set; }
        public string rblComplexTransaction { get; set; }
        public string rblIsSellerBuilderOrDeveloper { get; set; }
        public string btnGenerateQuote { get; set; }

    }


    public class TransactionTypes
    {
        public string ddlTransactionType { get; set; }
    }
    public class LoanTypes
    {
        public string ddlLoanType { get; set; }

    }
    public class PropertyTypes
    {
        public string PropertyType { get; set; }

    }
    public class PropertyUses
    {
        public string PropertyUse { get; set; }

    }
    public class NoOfMortgages
    {
        public string ddlNumberMortgages { get; set; }

    }
}
