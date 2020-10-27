using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalTrackActivity
    {
        public EFDBContext objDB = new EFDBContext();

        public string LogActivity(utblMstClientActivityLog model)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parActivityTitle = new SqlParameter("@ActivityTitle", model.ActivityTitle);
            var parClientID = new SqlParameter("@ClientID", model.ClientID);
            var parAgentID = new SqlParameter("@AgentID", model.AgentID);
            var parRemarks = new SqlParameter("@Remarks", model.Remarks);
            var parTrackingSource = new SqlParameter("@TrackingSource", model.TrackingSource);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspTrackClientActivity @ActivityTitle,@ClientID,@AgentID,@Remarks,@TrackingSource", parActivityTitle, parClientID, parAgentID, parRemarks, parTrackingSource).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<MstViewEmailActivityLog> GetActivityHistory(string AgentID)
        {
            var ParAgentID = new SqlParameter("@AgentID", AgentID);
            return objDB.Database.SqlQuery<MstViewEmailActivityLog>("udspTrackClientActivityEmailLogs @AgentID", ParAgentID).ToList();
        }
    }
}
