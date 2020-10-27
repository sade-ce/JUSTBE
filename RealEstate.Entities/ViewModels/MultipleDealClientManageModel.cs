using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;

namespace RealEstate.Entities.ViewModels
{
   public class MultipleDealClientManageModel
    {
        public List<ClientHomeGallery> HomeGalleryView { get; set; }

        public IEnumerable<ClientView> MultipleDealList { get; set; }
        public utblMstHelpRequest utblMstHelpRequests { get; set; }
        public IEnumerable<TransactionIDDL> TranDDL { get; set; }
        public Root HomePic { get; set; }
        public MLSListingDetails DisplayListing { get; set; }

        public IEnumerable<CheckDeal> CheckDeal { get; set; }
        public IEnumerable<CheckCalendar> CheckCalendar { get; set; }
        public IEnumerable<utblMstKeyInfoLink> KeyInfoLinkList { get; set; }

        public MstUserAgentView MstUserAgentView { get; set; }

        //sulo
        public IEnumerable<SharedClientView> SharedDealList { get; set; }
        //added by sonika
        public List<VendorCategoryDDl> CategoryDropDown { get; set; }


    }

    public class ClientView
    {
        public string TransactionID { get; set; }
        public string Status { get; set; }
        public string Year { get; set; }
        public string Address { get; set; }
        public string ClientType { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string MLS { get; set; }
        public string ListingType { get; set; }
        public string URL { get; set; }
        public string Image { get; set; }
        public string IsTransactionShared { get; set; }

    }

    public class SharedClientView
    {
        public string TransactionID { get; set; }
        public string Status { get; set; }
        public string Year { get; set; }
        public string Address { get; set; }
        public string ClientType { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string MLS { get; set; }
        public string ListingType { get; set; }
        public string URL { get; set; }
        public string Image { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }

    }
    public class TransactionIDDL
    {
        public string TransactionID { get; set; }

        public string ClientID { get; set; }
    }

    public class MLSListingDetails
    {
        public string Address { get; set; }
        public string ImageURL { get; set; }
        public string MLSID { get; set; }
        public string Price { get; set; }
        public string ListingUrl { get; set; }
        public string ListingSource { get; set; }


    }
}
