using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class ResourceLinksView
    {
        public int LinkId { get; set; }
        public int Title_Id { get; set; }
        public int Type_Id { get; set; }
        public string Link { get; set; }
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public DateTime CreatedOn { get; set; }

    }

    public class ResourceLinksPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
}
