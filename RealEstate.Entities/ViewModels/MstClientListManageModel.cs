using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientListManageModel
    {
        public IEnumerable<MstClientListView> MstClientList { get; set; }
        public PagingInfo PagingInfo { get; set; }


    }
}
