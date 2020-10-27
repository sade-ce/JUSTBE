using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class AspNetUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public bool ISClient { get; set; }
        public bool IsBlocked { get; set; }
        public string UserRole { get; set; }
        public string AgentEmail { get; set; }
        public string ColorCode { get; set; }
        public bool EventCalendar { get; set; }
        public bool StatusTimeline { get; set; }
        public bool SettlementDate { get; set; }
        public bool DocumentEmail { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }


    }
}
