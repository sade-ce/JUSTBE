using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstClientHomeGallerie
    {
        [Key]
        public string HomePhotoID { get; set; }
        public string TransactionID { get; set; }
        public string FileExt { get; set; }
        public string Description { get; set; }
        public string ClientID { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
