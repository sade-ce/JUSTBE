using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstScheduleMeeting
    {

        public IEnumerable<Agent> Agent { get; set; }

        [Required]
        public string Agenda { get; set; }
        [Required]

        public string Description { get; set; }

        [Required]
        public DateTime When { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage ="* Select Agent")]
        public string AgentEmail { get; set; }


    }
    public class Agent
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

}
