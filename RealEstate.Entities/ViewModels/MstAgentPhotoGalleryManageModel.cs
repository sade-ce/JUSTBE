using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstAgentPhotoGalleryManageModel
    {
        public UserProfileView UserProfile { get; set; }

        public IEnumerable<MstPhotoGalleryView> MstGalleryList { get; set; }
        public utblMstClientGallerie utblMstClientGalleries { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
