using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class TestimonialViewModel
    {

        public Testimonial Testimonial { get; set; }
        public TestimonialView TestimonialView { get; set; }
        public IEnumerable<TestimonialView> TestimonialList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public TestimonialPaging TestimonialPaging { get; set; }
        //New Version Start
        public TestimonialSortingInfo TestimonialSortingInfo { get; set; }
        public TestimonialFilterInfo TestimonialFilterInfo { get; set; }
        //New Version End
    }

    //New Version Start
    public class TestimonialSortingInfo
    {
        public string CurrentSort { get; set; }
        public string TitleSort { get; set; }
        public string TypeSort { get; set; }
        public string CreatedOnSort { get; set; }
        public string RatingSort { get; set; }
    }

    public class TestimonialFilterInfo
    {
        public string SearchFilter { get; set; }
    }
    //New Version End
}
