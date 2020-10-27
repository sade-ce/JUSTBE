using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class CommentsViewModel
    {
        public Comments Comments { get; set; }
        public CommentsView CommentsView { get; set; }
        public IEnumerable<CommentsView> CommentsList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public CommentsPaging CommentsPaging { get; set; }
    }
}
