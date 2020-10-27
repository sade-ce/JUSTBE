using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalScheduleMeeting
    {
        public EFDBContext objDB = new EFDBContext();


        public IEnumerable<Agent> AgentList()
        {
            List<Agent> objList = new List<Agent>();
            objList = objDB.Database.SqlQuery<Agent>("SELECT utblMstGmailTokens.UserEmail as Email, AspNetUsers.Name FROM utblMstGmailTokens INNER JOIN AspNetUsers ON utblMstGmailTokens.UserEmail = AspNetUsers.Email").ToList();
            return objList;
        }

    }
}
