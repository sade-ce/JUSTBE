using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class QuizView
    {
      public int QuizId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Unit { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsRegistered { get; set; }
        public DateTime CreatedOn { get; set; }
        public string QuizType { get; set; }
        public int AlterQuestion { get; set; }
        public string AlternateQuestion { get; set; }
    }
    public class QuizPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
}
