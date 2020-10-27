using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstArticleSuggestionManageModel
    {
        public IEnumerable<utblMstArticleEnquire> MstSuggestionList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
