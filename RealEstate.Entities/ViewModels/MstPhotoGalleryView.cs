using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstPhotoGalleryView
    {
        public string GallaryID { get; set; }
        public string ClientID { get; set; }
        public string TransactionID { get; set; }
        public string Description { get; set; }
        public string PhotoNormal { get; set; }
        public string PhotoThumb { get; set; }
        public string Email { get; set; }
    }
}
