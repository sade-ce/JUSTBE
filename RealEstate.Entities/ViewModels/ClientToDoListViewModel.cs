using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RealEstate.Entities.ViewModels
{
    public class ClientToDoListViewModel
    {
        public ClientToDoList ClientToDoList { get; set; }
        public ClientToDoListView ClientToDoListViews { get; set; }
        public IEnumerable<ClientToDoListView> ClientToDoListAll { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public ClientToDoListPaging ClientToDoListPaging { get; set; }
    }
}
