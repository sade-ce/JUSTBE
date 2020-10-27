using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalCheckUserDeal
    {
        public EFDBContext objDB = new EFDBContext();


        public IEnumerable<CheckDeal> CheckDeal(string Email)
        {
            List<CheckDeal> objList = new List<CheckDeal>();
            var parEmail = new SqlParameter("@Email", Email);
            objList = objDB.Database.SqlQuery<CheckDeal>("select TransactionID from utblMstTrackDeals where email=@Email union select TransactionID from utblMstSellerTrackDeals where email=@Email", parEmail).ToList();
            return objList;
        }
        public IEnumerable<CheckDeal> CheckSharedDeal(string Email)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var Id = objDB.Database.SqlQuery<string>("select Id from aspnetusers where email=@Email", parEmail).FirstOrDefault();

            List<CheckDeal> objList = new List<CheckDeal>();
            var parId= new SqlParameter("@Id", Id);
            var parUserEmail = new SqlParameter("@Email", Email);

            objList = objDB.Database.SqlQuery<CheckDeal>("select TransactionID from utblMstSharedDeals where (SharedWithClient=@Email or SharedWithClient=@Id) AND IsCancelled=0", parUserEmail, parId).ToList();
            return objList;
        }
        public IEnumerable<CheckCalendar> CheckCalendar(string Email)
        {
            List<CheckCalendar> objList = new List<CheckCalendar>();
            var parEmail = new SqlParameter("@Email", Email);
            objList = objDB.Database.SqlQuery<CheckCalendar>("select Id from utblMstAppointments where email=@Email", parEmail).ToList();
            return objList;
        }

        public IEnumerable<CheckAgent> CheckAgent(string ClientID)
        {
            List<CheckAgent> ObjList = new List<ViewModels.CheckAgent>();
            var ParClientID = new SqlParameter("@ClientID", ClientID);
            ObjList = objDB.Database.SqlQuery<CheckAgent>("udspMstAgentDropDownSelect @ClientID", ParClientID).ToList();
            return ObjList;
        }
    }
}
