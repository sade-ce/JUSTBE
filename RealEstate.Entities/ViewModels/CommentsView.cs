using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Entities.ViewModels
{
    public class CommentsView
    {
        public int CommentId { get; set; }
        public int Link_Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DateCreated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedOn { get; set; }
        public bool IsPin { get; set; }
        public Nullable<DateTime> PinedOn { get; set; }
        public string PinedBy { get; set; }
    }

    public class CommentsPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
}
