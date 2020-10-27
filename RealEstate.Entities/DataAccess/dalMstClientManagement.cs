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
    public class dalMstClientManagement
    {
        private EFDBContext objDB = new EFDBContext();
        public IEnumerable<UserProfileViewModel> UserGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<UserProfileViewModel> objList = new List<UserProfileViewModel>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<UserProfileViewModel>("udspUserSelect @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
    }
}
