using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
   public class dalClientNeighborhood
    {
        private EFDBContext objDB = new EFDBContext();

        public IEnumerable<MstClientNeighborhoodView>  GetNeighborhood(string CityName = "")
        {
           List<MstClientNeighborhoodView>  objList = new List<MstClientNeighborhoodView>();
            if (CityName != "")
            {
                CityName = Regex.Replace(CityName, @"\s+", " ");
            }
            var parCityName = new SqlParameter("@CityName", CityName);
            objList = objDB.Database.SqlQuery<MstClientNeighborhoodView>("udspNeighborhoodSelect @CityName", parCityName).ToList();
            return objList;
        }
    }
}
