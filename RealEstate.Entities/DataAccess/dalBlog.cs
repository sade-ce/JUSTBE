using RealEstate.Entities.Models;
using RealEstate.Entities.Utility;
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
    public class dalBlog
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public IEnumerable<BlogView> BlogGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<BlogView> objList = new List<BlogView>();
            if (SearchTerm != "")
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
            objList = objDB.Database.SqlQuery<BlogView>("udspBlogsGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public string Save(utblBlog model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            model.BlogID = CurrentYear + objUtil.generateUniqueCode("utblBlogs");
            var parBlogID = new SqlParameter("@BlogID", model.BlogID);
            var parBlogTitle = new SqlParameter("@Title", model.BlogTitle);
            var parBlogDescription = new SqlParameter("@Description", model.BlogDescription);
            var parBlogContent = new SqlParameter("@Content", model.BlogContent);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parBlogFile = new SqlParameter("@BlogFile", DBNull.Value);
            if (model.BlogFile != null)
                parBlogFile = new SqlParameter("@BlogFile", model.BlogFile);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBlogInsert @BlogID,@Title,@Description,@Content,@BlogFile,@CreatedBy", parBlogID, parBlogTitle, parBlogDescription, parBlogContent, parBlogFile, parCreatedBy).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string Edit(utblBlog model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parBlogID = new SqlParameter("@BlogID", model.BlogID);
            var parBlogTitle = new SqlParameter("@Title", model.BlogTitle);
            var parBlogDescription = new SqlParameter("@Description", model.BlogDescription);
            var parBlogContent = new SqlParameter("@Content", model.BlogContent);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parBlogFile = new SqlParameter("@BlogFile", DBNull.Value);
            if (model.BlogFile != null)
                parBlogFile = new SqlParameter("@BlogFile", model.BlogFile);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBlogUpdate @BlogID,@Title,@Description,@Content,@BlogFile,@CreatedBy", parBlogID, parBlogTitle, parBlogDescription, parBlogContent, parBlogFile, parCreatedBy).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public utblBlog GetBlogByID(string BlogID)
        {
            utblBlog objDetails = new utblBlog();
            var parBlogID = new SqlParameter("@BlogID", BlogID);
            objDetails = objDB.Database.SqlQuery<utblBlog>("udspGetBlogByID @BlogID", parBlogID).FirstOrDefault();
            return objDetails;
        }
        //for client paging blog single
        public BlogPaging GetBlogPrevNext(string BlogID)
        {
            BlogPaging objDetails = new BlogPaging();
            var parBlogID = new SqlParameter("@BlogID", BlogID);
            objDetails = objDB.Database.SqlQuery<BlogPaging>("udspBlogsGetPaging @BlogID", parBlogID).FirstOrDefault();
            return objDetails;
        }
        public string Delete(string BlogID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parBlogID = new SqlParameter("@BlogID", BlogID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBlogDelete @BlogID", parBlogID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    }
}
