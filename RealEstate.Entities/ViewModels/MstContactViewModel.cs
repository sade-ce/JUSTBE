using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
    public class MstContactViewModel
    {
        public IEnumerable<utblMstContact> MstContactList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
