using RealEstate.Entities.Models;
using RealEstate.Entities.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalbepetworth
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();
        public string Save(bepetworth model)
        {
            //DateTime dt = DateTime.Now;
            //var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            //model.BlogID = CurrentYear + objUtil.generateUniqueCode("utblBlogs");
           // var parBlogID = new SqlParameter("@BlogID", model.BlogID);
            var parName = new SqlParameter("@Name", model.Name);
            var parEmail = new SqlParameter("@Email", model.Email);
           
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspbepetworthInsert @Name,@Email", parName, parEmail).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    }
}
