using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class MstClientCalenderView
    {



        public int Id { get; set; }

        public string Description { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
        public string color { get; set; }
        public string TransactionID { get; set; }
        //New Version
        public string EventStartDate { get; set; }
        public string Agent { get; set; }
    }
}
