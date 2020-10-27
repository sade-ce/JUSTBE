using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstHomeEstimationViewModel
    {
        public ZillowModel ZillowModel { get; set; }
        public utblMstEnquire utblMstEnquire { get; set; }
    }
}
