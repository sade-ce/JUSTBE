using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class BlogView
    {
        public string BlogID { get; set; }
        public string BlogTitle { get; set; }
        public string BlogDescription { get; set; }
        public string BlogContent { get; set; }
        public string BlogFile { get; set; }
        public string BlogCreatedBy { get; set; }
        public DateTime BlogCreatedOn { get; set; }
    }

    public class BlogPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
}
