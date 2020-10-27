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
    public class dalQuiz
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();
        public string Save(Quiz model)
        {       
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parEmail = new SqlParameter("@Email", model.Email);
            var parFirstName = new SqlParameter("@FirstName", model.FirstName);
            var parLastName = new SqlParameter("@LastName", model.LastName);
            var parAddress = new SqlParameter("@Address", model.Address);
            var parUnit = new SqlParameter("@Unit", model.Unit);
            var parQues = new SqlParameter("@Question", model.Question);
            var parAns = new SqlParameter("@Answer", model.Answer);
            var parIsRegistered = new SqlParameter("@IsRegistered", model.IsRegistered);
            var parQuizType = new SqlParameter("@QuizType", model.QuizType);
            var parQuizOrder = new SqlParameter("@QuizOrder", model.QuizOrder);
            var parAlterQuiz = new SqlParameter("@AlterQuestion", model.AlterQuestion);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("QuizInsert @Email,@FirstName,@LastName,@Address,@Unit, @Question,@Answer,@QuizType,@QuizOrder,@AlterQuestion", parEmail,parFirstName,parLastName,parAddress,parUnit, parQues, parAns,parQuizType,parQuizOrder,parAlterQuiz).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<QuizView> QuizGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<QuizView> objList = new List<QuizView>();
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
            objList = objDB.Database.SqlQuery<QuizView>("udspQuizUsersGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        public IEnumerable<SearchAutoCompleteViewModel> QuizAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspQuizUserGetPagedAutoComplete @SearchTerm", searchParam).ToList();
            return obj;
        }

        public IEnumerable<QuizView> GetQuizByEmail(string Email)
        {
            List<QuizView> obj = new List<QuizView>();
            var searchParam = new SqlParameter("@Email", Email);
            obj = objDB.Database.SqlQuery<QuizView>("GetQuizByEmail @Email", searchParam).ToList();
            return obj;
        }

        public string AlternateQuestion(int Id)
        {
            var parId = new SqlParameter("@Id", Id);
           return objDB.Database.SqlQuery<string>("select Quiz from QuestionaireResult where Id=@Id",parId).FirstOrDefault();
        
        }
    }
}
