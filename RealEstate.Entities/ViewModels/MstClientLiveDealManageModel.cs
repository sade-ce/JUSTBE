using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientLiveDealManageModel
    {
        public IEnumerable<MstClientLiveDealView> MstClientLiveDealList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
