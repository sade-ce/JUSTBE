using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
using System.Data.SqlClient;
using RealEstate.Entities.ViewModels;

namespace RealEstate.Entities.DataAccess
{
    public class dalMstClosingConfig
    {
        EFDBContext objDB = new EFDBContext();
        SPErrorViewModel objStatus = new SPErrorViewModel();

        public IEnumerable<utblMstStatus> MstStatusList
        {
            get
            {
                return objDB.utblMstStatus;
            }
        }
        public string Save(utblMstClosingConfigutation Item)
        {
            var parStatusID = new SqlParameter("@StatusID", Item.StatusID);
            var parConfigDate = new SqlParameter("@ConfigDate", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspClosingConfigUpdate @StatusID, @ConfigDate", parStatusID, parConfigDate).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public List<GetClosingConfig> GetClosingConfig()
        {
            return objDB.Database.SqlQuery<GetClosingConfig>("SELECT utblMstClosingConfigutations.StatusID, utblMstStatus.Status, utblMstClosingConfigutations.ConfigDate FROM utblMstClosingConfigutations INNER JOIN utblMstStatus ON utblMstClosingConfigutations.StatusID = utblMstStatus.StatusID").ToList();
        }
    }
}
