using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
    public class MstSellerOffer
    {
        public UserProfileView UserProfile { get; set; }

        public utblMstPotentialBuyer utblMstPotentialBuyer { get; set; }
        public utblMstSellerCounterOffer utblMstSellerCounterOffer { get; set; }
        public IEnumerable<ReceivedOffer> ReceivedOfferList { get; set; }

    }

    public class ReceivedOffer
    {
        public string TransactionID { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerPhone { get; set; }
        public string BuyerAddress { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

    }
}
