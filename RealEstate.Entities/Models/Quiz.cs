using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Unit { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsRegistered { get; set; }
        public string QuizType { get; set; }
        public int QuizOrder { get; set; }
        public int AlterQuestion { get; set; }
    }
}
