using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class BlogViewModel
    {
        public utblBlog utblBlogs { get; set; }
        public BlogView BlogViews { get; set; }
        public IEnumerable<BlogView> BlogList { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public BlogPaging BlogPaging { get; set; }
    }
}
