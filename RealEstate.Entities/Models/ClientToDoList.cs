using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class ClientToDoList
    {
        [Key]
        public int Id { get; set; }

       
        [Required(ErrorMessage = "This field is required")]
        public string Task { get; set; }
    }
}
