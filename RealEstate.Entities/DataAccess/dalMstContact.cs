using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class dalMstContact
    {
        private EFDBContext objDB = new EFDBContext();
        public string ContactInfo(utblMstContact Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.ContactPerson = Regex.Replace(Item.ContactPerson.Trim(), @"\s+", " ");
            var parContactPerson = new SqlParameter("@ContactPerson", Item.ContactPerson);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parMessage = new SqlParameter("@Message", Item.Message);
            var parContactDate = new SqlParameter("@ContactDate", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstContactInsert @ContactPerson,@Email,@Message,@ContactDate", parContactPerson, parEmail, parMessage, parContactDate).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string AgentContactInfo(utblMstAgentContacts Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            Item.ContactPerson = Regex.Replace(Item.ContactPerson.Trim(), @"\s+", " ");
            var parContactPerson = new SqlParameter("@ContactPerson", Item.ContactPerson);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parSubject = new SqlParameter("@Subject", Item.Subject);
            var parMessage = new SqlParameter("@Message", Item.Message);
            var parContactDate = new SqlParameter("@ContactDate", System.DateTime.Now);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstAgentContactInsert @ContactPerson,@Email,@Subject,@Message,@ContactDate", parContactPerson, parEmail, parSubject, parMessage, parContactDate).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    }
}
