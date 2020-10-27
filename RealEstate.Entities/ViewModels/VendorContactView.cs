using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class VendorContactView
    {
        public int VendorContactId { get; set; }
        public string Vendor_Id { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string ContactPhone { get; set; }
    }
}
