using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
namespace RealEstate.Entities.ViewModels
{
    public class MstNeighborhoodViewModel
    {

        public IEnumerable<utblMstNeighborhoodState> MstStateDDl { get; set; }
        public IEnumerable<CityDropDown> MstDDLCity { get; set; }
        public IEnumerable<NeighborhoodView> MstNeighborhoodList { get; set; }
        public utblNeighborhood utblNeighborhoods { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    public class NeighborhoodView
    {

        public long RowID { get; set; }
        public int NeighborhoodID { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class CityDropDown
    {
        public int StateID { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
    }
}
