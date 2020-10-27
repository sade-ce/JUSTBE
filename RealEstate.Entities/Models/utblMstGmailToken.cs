using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstGmailToken
    {
        [Key]
        public int RefreshTokenId { get; set; }
        public string GmailAccount { get; set; }
        public string UserEmail { get; set; }
        public string RefreshToken { get; set; }
    }
}
