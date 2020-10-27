using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class ClientToDoListView
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedOn { get; set; }
        public String TransactionID { get; set; }
    }
    public class ClientToDoListPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
}
