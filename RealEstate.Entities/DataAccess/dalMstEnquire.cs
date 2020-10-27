using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalMstEnquire
    {
        private EFDBContext objDB = new EFDBContext();
        public string EnquireInfo(utblMstEnquire Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Name = Regex.Replace(Item.Name.Trim(), @"\s+", " ");
            var parName = new SqlParameter("@Name", Item.Name);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parPhone = new SqlParameter("@Phone", Item.Phone);
            var parMessage = new SqlParameter("@Message", Item.Message);
            var parEnquireDate = new SqlParameter("@EnquireDate", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstEnquireInsert @Name,@Email,@Phone,@Message,@EnquireDate", parName, parEmail, parPhone, parMessage, parEnquireDate).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<utblMstContact> GetContactPaged(int PageNo, int PageSize, out int TotalRows)
        {
            List<utblMstContact> list = new List<utblMstContact>();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            list = objDB.Database.SqlQuery<utblMstContact>("udspGetContactPaged @Start,@PageSize,@TotalCount out", parStart, parPageSize, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return list;
        }

        public IEnumerable<utblMstEnquire> GetEnquiryPaged(int PageNo, int PageSize, out int TotalRows)
        {
            List<utblMstEnquire> list = new List<utblMstEnquire>();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            list = objDB.Database.SqlQuery<utblMstEnquire>("udspGetEnquirePaged @Start,@PageSize,@TotalCount out", parStart, parPageSize, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return list;
        }


        public string ArticleEnquire(utblMstArticleEnquire Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Name = Regex.Replace(Item.Name.Trim(), @"\s+", " ");
            var parName = new SqlParameter("@Name", Item.Name);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parPhone = new SqlParameter("@Phone", "");
            if (Item.Phone != null)
                parPhone = new SqlParameter("@Phone", Item.Phone);

            var parMessage = new SqlParameter("@Message", Item.Message);
            var parPostedOn = new SqlParameter("@PostedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstArticleEnquireInsert @Name,@Email,@Phone,@Message,@PostedOn", parName, parEmail, parPhone, parMessage, parPostedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public IEnumerable<utblMstArticleEnquire> GetSugesstionPaged(int PageNo, int PageSize, out int TotalRows)
        {
            List<utblMstArticleEnquire> list = new List<utblMstArticleEnquire>();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            list = objDB.Database.SqlQuery<utblMstArticleEnquire>("udspGetArticleSuggestionPaged @Start,@PageSize,@TotalCount out", parStart, parPageSize, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return list;
        }

    }
}
