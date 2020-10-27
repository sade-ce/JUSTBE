using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
   public class MstClientShareDealManageModel
    {
        public ClientDetails TransactionDetails { get; set; }
        public IEnumerable<SharedWith> SharedClientList { get; set; }

    }
    public class SharedWith
    {
        public string ClientID { get; set; }
        public string ClientEmail { get; set; }
        public string ClientName { get; set; }
        public string Image { get; set; }
        public string NotRegistered { get; set; }
    }
}
