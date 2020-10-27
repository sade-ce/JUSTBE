using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class QuizViewModel
    {
        public QuizView BuyerQuiz { get; set; }
        public IEnumerable<QuizView> BuyerQuizList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public QuizPaging BuyerQuizPaging { get; set; }
        public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }

    }
}
