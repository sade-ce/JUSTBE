using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblGenCodeSeed
    {
        [Key]
        public int CodeID { get; set; }
        public string TableName { get; set; }
        public string CharRange { get; set; }
        public int CodeSlNo { get; set; }
    }
}
