using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class VendorsReviewViewModel
    {
        public VendorsReview VendorsReview { get; set; }
        public VendorsReviewModel VendorsReviewModel { get; set; }
        public IEnumerable<VendorsReviewModel> VendorsReviewList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public VendorsReviewPaging VendorsReviewPaging { get; set; }
        //public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }
    }
    public class VendorsReviewModel
    {
        public int ClientId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
    public class VendorsReviewPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
}
