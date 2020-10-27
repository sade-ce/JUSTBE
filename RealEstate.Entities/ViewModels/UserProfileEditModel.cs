using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class UserProfileEditModel
    {
        public UserProfileEdit ProfileEdit { get; set; }
    }

    public class UserEdit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserRole { get; set; }
        public string UserPhotoNormal { get; set; }
        public string UserPhotoThumb { get; set; }

    }

    public class UserProfileEdit
    {
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter Your First Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "* Email")]

        public string Email { get; set; }
        [Required(ErrorMessage = "* Phone")]
        public string Phone { get; set; }
        public string UserPhotoThumb { get; set; }
        public string UserPhotoNormal { get; set; }
    }
}
