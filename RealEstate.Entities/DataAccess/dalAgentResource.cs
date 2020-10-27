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
    public class dalAgentResource
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();

        

        public IEnumerable<AgentResourceView> List(string TransactionID)
        {
            List<AgentResourceView> objList = new List<AgentResourceView>();
            objList = objDB.Database.SqlQuery<AgentResourceView>("GetAgentResource").ToList();
            return objList;
        }

       

        public string Save(AgentResourceView model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parTitle = new SqlParameter("@Title", model.Title);
            var parDescription = new SqlParameter("@Description", model.Description == null ? "" : model.Description);
            var parCreatedBy = new SqlParameter("@CreatedBy", model.CreatedBy);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("AgentResourcesInsert @Title,@Description,@CreatedBy", parTitle, parDescription, parCreatedBy).FirstOrDefault();
            return objStatus.ErrorMessage;
        }

        public string Edit(AgentResourceView model)
        {
             SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", model.Id);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parDescription = new SqlParameter("@Description", model.Description == null ? "" : model.Description);
         

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("AgentResourceUpdate @Id,@Title,@Description", parId, parTitle, parDescription).FirstOrDefault();
            return objStatus.ErrorMessage;
        }

      

        public AgentResourceView GetAgentResourceByID(int Id)
        {
            AgentResourceView objDetails = new AgentResourceView();
            var parId = new SqlParameter("@Id", Id);
            objDetails = objDB.Database.SqlQuery<AgentResourceView>("GetAgentResourceById @Id", parId).FirstOrDefault();
            return objDetails;
        }

        public IEnumerable<AgentResourceView> GetAgentResourceS()
        {
            return objDB.Database.SqlQuery<AgentResourceView>("GetAgentResource").ToList();
        }

        public string Delete(int Id)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", @Id);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("AgentResourceDelete @Id", parId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

    }
}
