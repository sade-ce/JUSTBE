using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class TeamMemberView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string MemberImage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Pinterest { get; set; }
        public string ProfileLink { get; set; }
        public string ShortDescription { get; set; }
        public int OrderNumber { get; set; }
        public string About { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
    }
    public class TeamMemberPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
  
}
