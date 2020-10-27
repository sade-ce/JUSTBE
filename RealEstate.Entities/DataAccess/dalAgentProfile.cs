using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalAgentProfile
    {
        EFDBContext ObjDb = new EFDBContext();

        public MstAgentViewModel AgentView(string AgentID)
        {
            MstAgentViewModel Obj = new MstAgentViewModel();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            Obj = ObjDb.Database.SqlQuery<MstAgentViewModel>("udspMstAgentProfileSelect @AgentID", parAgentID).FirstOrDefault();
            return Obj;
        }
    }
}
