using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class utblMstAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string Email { get; set; }

        [Display(Name = "Event Title")]
        [Required(ErrorMessage ="Event Title is required")]
        public string Description { get; set; }

        [Display(Name ="Start Date")]
        [Required(ErrorMessage = "Start Date Time is required")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "End Date Time is required")]

        public DateTime EndDate { get; set; }

        [Display(Name = "Event Color")]

        [Required(ErrorMessage = "Set Event Color")]
        public string color { get; set; }

        [Display(Name = "Send email alert")]
        public bool IsContingency { get; set; }
        public bool IsEmailSent { get; set; }

        public string AgentID { get; set; }
        public string TrackingID { get; set; } //added by sonika
        public string TransactionID { get; set; } //added by neha

        public bool RepeatEvent { get; set; }
        public string RepeatFrequency { get; set; } //added by sonika
        public int RepeatInterval { get; set; } //added by neha

        public string createdby { get; set; }
    }
}
