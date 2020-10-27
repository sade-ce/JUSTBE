using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace RealEstate.Entities.DataAccess
{
    public class dalStatus
    {
        private EFDBContext objDB = new EFDBContext();
        public IEnumerable<utblMstStatus> MstStatusList
        {
            get
            {
                return objDB.utblMstStatus.OrderBy(m=>m.Hierarchy);
            }
        }
        public IEnumerable<utblMstSellerStatus> MstSellerStatusList
        {
            get
            {
                return objDB.utblMstSellerStatus.OrderBy(m => m.Hierarchy);
            }
        }
        public string Save(utblMstStatus Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Status = Regex.Replace(Item.Status.Trim(), @"\s+", " ");
            var parStatus = new SqlParameter("@Status", Item.Status);
            var parPercentage = new SqlParameter("@Percentage", Item.Percentage);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parVideoURL = new SqlParameter("@VideoURL", Item.VideoURL);
            var parIsContingencies = new SqlParameter("@IsContingencies", Item.IsContingencies);
            var parHierarchy = new SqlParameter("@Hierarchy", Item.Hierarchy);

            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstStatusInsert @Status,@Percentage,@Description,@VideoURL,@IsContingencies,@Hierarchy,@UpdatedOn", parStatus, parPercentage, parDescription, parVideoURL, parIsContingencies, parHierarchy,parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string SellerStatusSave(utblMstSellerStatus Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Status = Regex.Replace(Item.Status.Trim(), @"\s+", " ");
            var parStatus = new SqlParameter("@Status", Item.Status);
            var parPercentage = new SqlParameter("@Percentage", Item.Percentage);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parVideoURL = new SqlParameter("@VideoURL", Item.VideoURL);
            var parIsContingencies = new SqlParameter("@IsContingencies", Item.IsContingencies);
            var parHierarchy = new SqlParameter("@Hierarchy", Item.Hierarchy);

            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerStatusInsert @Status,@Percentage,@Description,@VideoURL,@IsContingencies,@Hierarchy,@UpdatedOn", parStatus, parPercentage, parDescription, parVideoURL, parIsContingencies, parHierarchy, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public utblMstStatus GetStatusByID(int ID)
        {
            utblMstStatus objItem = objDB.utblMstStatus.FirstOrDefault(p => p.StatusID == ID);
            return objItem;
        }

        public utblMstSellerStatus GetSellerStatusByID(int ID)
        {
            utblMstSellerStatus objItem = objDB.utblMstSellerStatus.FirstOrDefault(p => p.SellerStatusID == ID);
            return objItem;
        }

        public string Edit(utblMstStatus Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Status = Regex.Replace(Item.Status.Trim(), @"\s+", " ");
            var parStausID = new SqlParameter("@StausID", Item.StatusID);
            var parStatus = new SqlParameter("@Status", Item.Status);
            var parPercentage = new SqlParameter("@Percentage", Item.Percentage);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parVideoURL = new SqlParameter("@VideoURL", Item.VideoURL);
            var parIsContingencies = new SqlParameter("@IsContingencies", Item.IsContingencies);
            var parHierarchy = new SqlParameter("@Hierarchy", Item.Hierarchy);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstStatusUpdate @StausID,@Status,@Percentage,@Description,@VideoURL,@IsContingencies,@Hierarchy,@UpdatedOn", parStausID,parStatus, parPercentage, parDescription, parVideoURL, parIsContingencies, parHierarchy, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string Edit(utblMstSellerStatus Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.Status = Regex.Replace(Item.Status.Trim(), @"\s+", " ");
            var parStausID = new SqlParameter("@SellerStatusID", Item.SellerStatusID);
            var parStatus = new SqlParameter("@Status", Item.Status);
            var parPercentage = new SqlParameter("@Percentage", Item.Percentage);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parVideoURL = new SqlParameter("@VideoURL", Item.VideoURL);
            var parIsContingencies = new SqlParameter("@IsContingencies", Item.IsContingencies);
            var parHierarchy = new SqlParameter("@Hierarchy", Item.Hierarchy);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerStatusUpdate @SellerStatusID,@Status,@Percentage,@Description,@VideoURL,@IsContingencies,@Hierarchy,@UpdatedOn", parStausID, parStatus, parPercentage, parDescription, parVideoURL, parIsContingencies, parHierarchy, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string Delete(int ID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parStausID = new SqlParameter("@StausID", ID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstStatusDelete @StausID", parStausID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string SellerStatusDelete(int ID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parSellerStatusID = new SqlParameter("@SellerStatusID", ID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerStatusDelete @SellerStatusID", parSellerStatusID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


    }
}
