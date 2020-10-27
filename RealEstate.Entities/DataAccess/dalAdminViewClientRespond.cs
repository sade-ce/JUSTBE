using RealEstate.Entities.Models;
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
   public class dalAdminViewClientRespond
    {
        public EFDBContext objDB = new EFDBContext();

        public IEnumerable<MstClientRespondView> ClientRespondDealGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<MstClientRespondView> objList = new List<MstClientRespondView>();
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
            objList = objDB.Database.SqlQuery<MstClientRespondView>("udspMstGetClientRespondDealPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;

        }

        public IEnumerable<utblMstClientTrackDealDoc> GetClientLiveDealDocList(string ID)
        {
            List<utblMstClientTrackDealDoc> objList = new List<utblMstClientTrackDealDoc>();
            objList = objDB.utblMstClientTrackDealDocs.Where(x => x.TrackingID == ID).ToList();
            return objList;
        }

    }
}
