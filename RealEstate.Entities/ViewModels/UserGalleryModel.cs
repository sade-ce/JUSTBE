using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class UserGalleryModel
    {
        public IEnumerable<UserGalleryView> UserGalleryList { get; set; }
        public UserGallery UserGallery { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
