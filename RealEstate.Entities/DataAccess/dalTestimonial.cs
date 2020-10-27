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
    public class dalTestimonial
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public IEnumerable<TestimonialView> TestimonialGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<TestimonialView> objList = new List<TestimonialView>();
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
         
            objList = objDB.Database.SqlQuery<TestimonialView>("udspTestimonialsGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public IEnumerable<TestimonialView> TestimonialGetPagedVersion2(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "", string SortOrder = "")
        {
            List<TestimonialView> objList = new List<TestimonialView>();
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
            var parSortOrder = new SqlParameter("@SortOrder", SortOrder);
            objList = objDB.Database.SqlQuery<TestimonialView>("udspTestimonialsGetPagedV2 @Start,@PageSize,@SearchTerm,@SortOrder,@TotalCount out", parStart, parPageSize, parSearchTerm,parSortOrder, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        public string Save(Testimonial model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parUserType = new SqlParameter("@UserType", model.UserType);
            var parUsername = new SqlParameter("@UserName", model.UserName);
            var parDescription = new SqlParameter("@Description", model.Description==null?"":model.Description);
            var parRating = new SqlParameter("@Rating", model.Rating);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspTestimonialInsert @UserName,@UserType,@Description,@Rating,@CreatedBy",parUsername, parUserType, parDescription, parRating, parCreatedBy).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string Edit(Testimonial model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", model.Id);
            var parUserType = new SqlParameter("@UserType", model.UserType);
            var parUsername = new SqlParameter("@UserName", model.UserName);
            var parDescription = new SqlParameter("@Description", model.Description==null?"":model.Description);
            var parRating = new SqlParameter("@Rating", model.Rating);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspTestimonialUpdate @Id,@UserType,@UserName,@Description,@Rating,@CreatedBy",parId, parUserType, parUsername, parDescription, parRating, parCreatedBy).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public Testimonial GetTestimonialByID(int Id)
        {
            Testimonial objDetails = new Testimonial();
            var parID = new SqlParameter("@Id", Id);
            objDetails = objDB.Database.SqlQuery<Testimonial>("udspGetTestimonialByID @Id", parID).FirstOrDefault();
            return objDetails;
        }
        public TestimonialPaging GetTestimonialPrevNext(int Id)
        {
            TestimonialPaging objDetails = new TestimonialPaging();
            var parID = new SqlParameter("@Id", Id);
            objDetails = objDB.Database.SqlQuery<TestimonialPaging>("udspTestimonialGetPaging @Id", parID).FirstOrDefault();
            return objDetails;
        }
        public string Delete(int Id)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parID = new SqlParameter("@Id", Id);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspTestimonialDelete @Id", parID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    
}
}
