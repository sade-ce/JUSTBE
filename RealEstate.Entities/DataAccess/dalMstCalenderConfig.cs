using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System.Data.SqlClient;

namespace RealEstate.Entities.DataAccess
{
    public class dalMstCalenderConfig
    {
        EFDBContext objDB = new EFDBContext();
        SPErrorViewModel objStatus = new SPErrorViewModel();

        public string Save(utblMstCalenderConfiguration Item)
        {
            var parStatusID = new SqlParameter("@StatusID", Item.StatusID);
            var parConfigDate = new SqlParameter("@ConfigDate", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCalenderConfigInsert @StatusID, @ConfigDate", parStatusID, parConfigDate).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<CalenderConfigList> GetCalenderConfigList(int PageNo, int PageSize, out int TotalRows)
        {
            List<CalenderConfigList> list = new List<CalenderConfigList>();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            list = objDB.Database.SqlQuery<CalenderConfigList>("udspGetCalenderConfigPaged @Start,@PageSize,@TotalCount out", parStart, parPageSize, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return list;
        }

        public IEnumerable<utblMstStatus> MstStatusList
        {
            get
            {
                return objDB.utblMstStatus;
            }
        }


        public string Delete(int CalenderConfigID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parCalenderConfigID = new SqlParameter("@CalenderConfigID", CalenderConfigID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstCalenderConfigDelete @CalenderConfigID", parCalenderConfigID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

    }
}
