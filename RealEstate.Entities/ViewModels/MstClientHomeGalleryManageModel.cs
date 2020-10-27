using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
    public class MstClientHomeGalleryManageModel
    {
        public UserProfileView UserProfile { get; set; }

        public IEnumerable<HomeGalleryView> HomeGalleryView { get; set; }
        public utblMstClientHomeGallerie utblMstClientHomeGalleries { get; set; }
    }

    public class HomeGalleryView
    {
        public string HomePhotoID { get; set; }
        public string TransactionID { get; set; }
        public string ClientID { get; set; }
        public string Description { get; set; }
        public string FileExt { get; set; }
        public string Email { get; set; }

    }
}
