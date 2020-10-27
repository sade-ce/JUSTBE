using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblBlog
    {
        [Key]
        public string BlogID { get; set; }
        [Required(ErrorMessage="Title is required")]
        [DisplayName("Title")]
        public string BlogTitle { get; set; }
        [DisplayName("Description")]
        public string BlogDescription { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [DisplayName("Content")]
        public string BlogContent { get; set; }

        public string BlogFile { get; set; }
        public string BlogCreatedBy { get; set; }
        public DateTime BlogCreatedOn { get; set; }

    }
}
