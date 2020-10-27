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
    public class dalTeamMember
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();

        public IEnumerable<TeamMemberView> TeamMemberGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<TeamMemberView> objList = new List<TeamMemberView>();
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
            objList = objDB.Database.SqlQuery<TeamMemberView>("udspTeamMemberGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public IEnumerable<TeamMemberView> GetTeamMembers()
        {
            List<TeamMemberView> objList = new List<TeamMemberView>();
            objList = objDB.Database.SqlQuery<TeamMemberView>("udspGetTeamMember").ToList(); 
            return objList;
        }
        public string Save(TeamMembers model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            model.Id = CurrentYear + objUtil.generateUniqueCode("TeamMembers");
            var parId = new SqlParameter("@Id", model.Id);
            var parName = new SqlParameter("@Name", model.Name);
            var parDesignation= new SqlParameter("@Designation", model.Designation);
            var parFacebook = new SqlParameter("@Facebook", model.Facebook==null?"":model.Facebook);
            var parTwitter = new SqlParameter("@Twitter", model.Twitter == null ? "" : model.Twitter);
            var parInstagram = new SqlParameter("@Instagram", model.Instagram == null ? "" : model.Instagram);
            var parPinterest = new SqlParameter("@Pinterest", model.Pinterest == null ? "" : model.Pinterest);
            var parProfileLink = new SqlParameter("@ProfileLink", model.ProfileLink);
            var parShortDescription = new SqlParameter("@ShortDescription", model.ShortDescription);
            var parOrderNumber = new SqlParameter("@OrderNumber", model.OrderNumber);
            var parMemberImage = new SqlParameter("@MemberImage", DBNull.Value);
            if (model.MemberImage != null)
                parMemberImage = new SqlParameter("@MemberImage", model.Id.ToString() + model.MemberImage.ToString());
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);

            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parContactEmail = new SqlParameter("@ContactEmail", model.ContactEmail == null ? "" : model.ContactEmail);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspTeamMemberInsert @Id,@Name,@Designation,@MemberImage,@Facebook,@Twitter,@Instagram,@Pinterest,@ProfileLink ,@ShortDescription,@OrderNumber,@CreatedBy,@About,@ContactEmail,@Phone", 
                                                                                    parId, parName, parDesignation, parMemberImage, parFacebook, parTwitter, parInstagram, parPinterest, parProfileLink, parShortDescription,parOrderNumber, parCreatedBy,parAbout,parContactEmail,parPhone).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string Edit(TeamMembers model, string CreatedBy)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", model.Id);
            var parName = new SqlParameter("@Name", model.Name);
            var parDesignation = new SqlParameter("@Designation", model.Designation);
            var parFacebook = new SqlParameter("@Facebook", model.Facebook == null ? "" : model.Facebook);
            var parTwitter = new SqlParameter("@Twitter", model.Twitter == null ? "" : model.Twitter);
            var parInstagram = new SqlParameter("@Instagram", model.Instagram == null ? "" : model.Instagram);
            var parPinterest = new SqlParameter("@Pinterest", model.Pinterest == null ? "" : model.Pinterest);
            var parProfileLink = new SqlParameter("@ProfileLink", model.ProfileLink);
            var parShortDescription = new SqlParameter("@ShortDescription", model.ShortDescription);
            var parOrderNumber = new SqlParameter("@OrderNumber", model.OrderNumber);
            var parMemberImage = new SqlParameter("@MemberImage", DBNull.Value);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            if (model.MemberImage != null)
                parMemberImage = new SqlParameter("@MemberImage",  model.MemberImage.ToString());
            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parContactEmail = new SqlParameter("@ContactEmail", model.ContactEmail == null ? "" : model.ContactEmail);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspTeamMemberUpdate @Id,@Name,@Designation,@MemberImage,@CreatedBy,@Facebook,@Twitter,@Instagram,@Pinterest,@ProfileLink ,@ShortDescription,@OrderNumber,@About,@ContactEmail,@Phone", parId, parName, parDesignation, parMemberImage, parCreatedBy, parFacebook, parTwitter, parInstagram, parPinterest, parProfileLink, parShortDescription,parOrderNumber,parAbout,parContactEmail,parPhone).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

    
        public TeamMembers GetTeamMemberByID(string Id)
        {
            TeamMembers objDetails = new TeamMembers();
            var parId = new SqlParameter("@Id", Id);
            objDetails = objDB.Database.SqlQuery<TeamMembers>("udspGetTeamMemberByID @Id", parId).FirstOrDefault();
            return objDetails;
        }

        public TeamMemberView GetTeamMemberByName(string Name)
        {
            TeamMemberView objDetails = new TeamMemberView();
            var parId = new SqlParameter("@Name", Name);
            objDetails = objDB.Database.SqlQuery<TeamMemberView>("udspGetTeamMemberByName @Name", parId).FirstOrDefault();
            return objDetails;
        }

        public TeamMemberPaging GetTeamMemberPrevNext(string Id)
        {
            TeamMemberPaging objDetails = new TeamMemberPaging();
            var parId = new SqlParameter("@Id", Id);
            objDetails = objDB.Database.SqlQuery<TeamMemberPaging>("udspTeamMemberGetPaging @parId", parId).FirstOrDefault();
            return objDetails;
        }
        public string Delete(string Id)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Id);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspTeamMemberDelete @Id", parId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    
    
    }
}
