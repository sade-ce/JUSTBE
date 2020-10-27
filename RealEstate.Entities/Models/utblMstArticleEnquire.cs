using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Entities.Models
{
    public class utblMstArticleEnquire
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public long ArticleID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public DateTime PostedOn { get; set; }
    }
}
