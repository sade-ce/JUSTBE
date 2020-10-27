using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalLead
    {
        EFDBContext db = new EFDBContext();

        public long SavePartialLead(utblPartialLead model)
        {
            long id = 0;
            var parAddress = new SqlParameter("@Address", model.Address);
            var parCity = new SqlParameter("@City", model.City);
            var parZip = new SqlParameter("@Zip", "");
            if (model.CityStateZip != null)
                parZip = new SqlParameter("@Zip", model.CityStateZip);
            var parUnit = new SqlParameter("UnitNo", model.UnitNo);
            var parDate = new SqlParameter("@Date", DateTime.Now);

            id = db.Database.SqlQuery<long>("udspPartialLeadInsert @Address,@City, @Zip, @UnitNo, @Date", parAddress, parCity, parZip, parUnit, parDate).First();
            return id;
        }
        public void SaveFullLead(utblFullLead model)
        {
            var parId = new SqlParameter("@ID", model.VisitorID);
            var parName = new SqlParameter("@Name", model.Name);
            var parPhone = new SqlParameter("@Phone", "");
            if (model.Phone != null)
                parPhone = new SqlParameter("@Phone", model.Phone);
            //var parPhone = new SqlParameter("@Phone", model.Phone);
            var parEmail = new SqlParameter("@Email", model.EmailID);

            db.Database.SqlQuery<int>("udspFullLeadInsert @ID, @Name, @Phone, @Email", parId, parName, parPhone, parEmail).FirstOrDefault();

        }
        public int GetTotalFullLeads()
        {
            int count = 0;
            count = db.Database.SqlQuery<int>("select count(*) from utblLeads where Name is not null").First();
            return count;
        }
        public int GetTotalPartialLeads()
        {
            int count = 0;
            count = db.Database.SqlQuery<int>("select count(*) from utblLeads where Name is null and Address !=''").First();
            return count;
        }
        public utblPartialLead GetLeadByID(long id)
        {
            string query = "select Address, CityStateZip, UnitNo, VisitedOn from utblLeads where VisitorID=" + id;
            return db.Database.SqlQuery<utblPartialLead>(query).FirstOrDefault();
        }
        public IEnumerable<utblPartialLead> GetPartialLeadPaged(int PageNo, int PageSize, out int TotalRows)
        {
            List<utblPartialLead> list = new List<utblPartialLead>();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            list = db.Database.SqlQuery<utblPartialLead>("udspGetPartialLeadPaged @Start,@PageSize,@TotalCount out", parStart, parPageSize, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return list;
        }
        public IEnumerable<utblFullLead> GetFullLeadPaged(int PageNo, int PageSize, out int TotalRows)
        {
            List<utblFullLead> list = new List<utblFullLead>();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            list = db.Database.SqlQuery<utblFullLead>("udspGetFullLeadPaged @Start,@PageSize,@TotalCount out", parStart, parPageSize, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return list;
        }
    }
}
