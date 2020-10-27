using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{

    public class MstAgentView
    {
        public MstAgentViewModel MstAgentViewModel { get; set; }
        public utblMstContact MstAgentContact { get; set; }
        public IEnumerable<UserGalleryView> UserGalleryList { get; set; }
    }
    public class MstAgentViewModel
    {

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserPhotoNormal { get; set; }

    }

   

}
