using RealEstate.Entities.Models;
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
    public class dalNeighborhood
    {
        private EFDBContext objDB = new EFDBContext();
        public IEnumerable<utblMstNeighborhoodState> MstStateList
        {
            get
            {
                return objDB.utblMstNeighborhoodStates;
            }
        }
        public IEnumerable<CityDropDown> CityDropDownList(int StateID)
        {
            return objDB.utblMstNeighborhoodCities.Where(x => x.StateID == StateID).Select(x => new CityDropDown { StateID = x.StateID, CityID = x.CityID,CityName=x.CityName }).OrderBy(x => x.CityName).ToList();
        }
        public IEnumerable<NeighborhoodView> GetNeighborhoodPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<NeighborhoodView> objList = new List<NeighborhoodView>();
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
            objList = objDB.Database.SqlQuery<NeighborhoodView>("udspNeighborhoodGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        public string Save(utblNeighborhood model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parStateID = new SqlParameter("@StateID", model.StateID);
            var parCityID = new SqlParameter("@CityID", model.CityID);
            var parPostContent = new SqlParameter("@PostContent", model.PostContent);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            if (model.NeighborhoodID != 0)
            {
                var parNeighborhoodID = new SqlParameter("@NeighborhoodID", model.NeighborhoodID);
                objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspNeighborhoodUpdate @NeighborhoodID,@StateID,@CityID,@PostContent,@UpdatedOn", parNeighborhoodID, parStateID, parCityID, parPostContent, parUpdatedOn).FirstOrDefault();
            }
            else
            {
                objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspNeighborhoodInsert @StateID,@CityID,@PostContent,@UpdatedOn", parStateID, parCityID, parPostContent, parUpdatedOn).FirstOrDefault();
            }
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public utblNeighborhood GetDetailsByNeighborhoodID(int ID)
        {
            utblNeighborhood objItem = objDB.utblNeighborhoods.FirstOrDefault(p => p.NeighborhoodID == ID);
            return objItem;
        }
        
        public string Delete(int ID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parNeighborhoodID = new SqlParameter("@NeighborhoodID", ID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspNeighborhoodDelete @NeighborhoodID", parNeighborhoodID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    }
}
