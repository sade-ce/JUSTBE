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
    public class dalComments
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();


        public IEnumerable<CommentsView> CommentsGetPaged(int LinkId,int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<CommentsView> objList = new List<CommentsView>();
            if (SearchTerm != "")
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            var parLinkId = new SqlParameter("@Link_Id", LinkId);
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<CommentsView>("udspCommentsGetPaged @Link_Id,@Start,@PageSize,@SearchTerm,@TotalCount out",parLinkId, parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }


        public string Save(CommentsView model, string CreatedBy)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parLink_Id = new SqlParameter("@Link_Id", model.Link_Id);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parDescription = new SqlParameter("@Description", model.Description);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCommentInsert @Link_Id,@Title,@Description,@CreatedBy",parLink_Id, parTitle,parDescription,parCreatedBy).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public CommentsView GetCommentByID(int CommentId)
        {
            CommentsView objDetails = new CommentsView();
            var parCommentId = new SqlParameter("@CommentId", CommentId);
            objDetails = objDB.Database.SqlQuery<CommentsView>("GetCommentById @CommentId", parCommentId).FirstOrDefault();
            return objDetails;
        }

        public string Edit(CommentsView model, string UpdatedBy)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parCommentId = new SqlParameter("@CommentId", model.CommentId);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parLink_Id = new SqlParameter("@Link_Id", Convert.ToInt32(model.Link_Id));
            var parDescription = new SqlParameter("@Description", model.Description);
            var parCreatedBy = new SqlParameter("@UpdatedBy", UpdatedBy);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCommentUpdate @CommentId,@Title,@Link_Id,@Description,@UpdatedBy", parCommentId, parTitle, parLink_Id, parDescription, parCreatedBy).FirstOrDefault();    
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        
        public string Delete(int CommentId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parCommentId = new SqlParameter("@CommentId", CommentId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCommentDelete @CommentId", parCommentId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string Pin(int CommentId, string PinedBy, bool IsPin)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parCommentId = new SqlParameter("@CommentId",CommentId);
 
            var parPinedBy = new SqlParameter("@PinedBy", PinedBy);
            var parIsPin = new SqlParameter("@IsPin", IsPin);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspPinComment @CommentId,@PinedBy,@IsPin", parCommentId, parPinedBy, parIsPin).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    }
}
