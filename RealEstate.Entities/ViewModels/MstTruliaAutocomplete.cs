using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstTruliaAutocomplete
    {
        public bool noneFound { get; set; }
        public string searchText { get; set; }
        public string message { get; set; }
        public string url { get; set; }
        public string ImgUrl { get; set; }
    }
}
