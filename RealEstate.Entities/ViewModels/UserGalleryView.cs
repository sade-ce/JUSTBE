using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class UserGalleryView
    {
        public Guid UserGalleryId { get; set; }
        public string UserId { get; set; }
        public string Photo { get; set; }
    }
}
