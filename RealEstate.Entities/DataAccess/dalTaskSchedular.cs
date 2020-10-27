using RealEstate.Entities.Utility;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalTaskSchedular
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public IEnumerable<MstSelectTask> GetClientEmail()
        {
            List<MstSelectTask> ObjData = new List<MstSelectTask>();
            return objDB.Database.SqlQuery<MstSelectTask>("udspSelectTaskTOSendEmail").ToList();  // commented by sonika
           // return objDB.Database.SqlQuery<MstSelectTask>("udspGetAllAppointmentClientCalender").ToList();//Added by sonika

        }

        public string Update(int ID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var ParID = new SqlParameter("@Id", ID);
            objDB.Database.SqlQuery<SPErrorViewModel>("udspMstUpdateEmailTaskSchedular @Id", ParID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        /// <summary>
        /// Added by sonika
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string UpdateNewTable(int ID,DateTime StartDate, DateTime EndDate)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var ParID = new SqlParameter("@Id", ID);
            var ParStartDate = new SqlParameter("@StartDate", ID);
            var ParEndDate = new SqlParameter("@EndDate", ID);
            objDB.Database.SqlQuery<SPErrorViewModel>("Usp_addTRecurranceCheck @Id", ParID, ParStartDate, ParEndDate).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    }
}
