using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class ClientDealDocumentsView
    {
        public int DocId { get; set; }
        public string ClientDocId { get; set; }
        // public int Id { get; set; }
        public string DocFile { get; set; }
        public string TransactionID { get; set; }
        public string ClientId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DocumentType { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string dateupdated { get; set; }
    }
    public class MstAppointmentCalendarModel //added by sonika
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string color { get; set; }
        public bool IsContingency { get; set; }
        public bool IsEmailSent { get; set; }
        public string AgentID { get; set; }
        public string TrackingID { get; set; }
        public string TransactionID { get; set; }

        public string Date { get; set; }
        public string UserRole { get; set; }
    }


    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string color { get; set; }

        public bool IsContingency { get; set; }
        public bool IsEmailSent { get; set; }

        public string AgentID { get; set; }
        public string TrackingID { get; set; } //added by sonika
        public string TransactionID { get; set; } //added by neha

        public bool RepeatEvent { get; set; }
        public string RepeatFrequency { get; set; } //added by sonika
        public int RepeatInterval { get; set; } //added by neha}
        public string UserRole { get; set; } //added by neha}
    }

    public class AppointmentViewModelClientEvent
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
        public string color { get; set; }

        public bool IsContingency { get; set; }
        public bool IsEmailSent { get; set; }

        public string AgentID { get; set; }
        public string TrackingID { get; set; } //added by sonika
        public string TransactionID { get; set; } //added by neha

        public bool RepeatEvent { get; set; }
        public string RepeatFrequency { get; set; } //added by sonika
        public int RepeatInterval { get; set; } //added by neha}
        public int UserRole { get; set; } //added by neha}
    }
}
