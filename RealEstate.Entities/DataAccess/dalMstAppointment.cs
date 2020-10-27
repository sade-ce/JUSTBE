using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.Models;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace RealEstate.Entities.DataAccess
{
    public class dalMstAppointment
    {
        EFDBContext objDB = new EFDBContext();

        public IEnumerable<utblMstAppointment> GetCalenderList(string AgentID, string TransactionID)
        {
            List<utblMstAppointment> objList = new List<utblMstAppointment>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<utblMstAppointment>("udspAppointmentCalenderSelect @AgentID,@TransactionID", parAgentID, parTransactionID).ToList();
            return objList;
        }

        //Added by Neha 03-06-2019
        public IEnumerable<utblMstAppointment> GetCalenderListNew(string AgentID, string Email)
        {
            List<utblMstAppointment> objList = new List<utblMstAppointment>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            var parEmail = new SqlParameter("@Email", Email);
            objList = objDB.Database.SqlQuery<utblMstAppointment>("udspAppointmentCalenderSelectNew @AgentID,@Email", parAgentID, parEmail).ToList();
            return objList;
        }

        //Added by Neha 30-05-2019
        public IEnumerable<utblMstAppointment> GetCalenderListByTransactionId(string TransactionID)
        {
            List<utblMstAppointment> objList = new List<utblMstAppointment>();
            if (!string.IsNullOrEmpty(TransactionID))
            {
                var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
                objList = objDB.Database.SqlQuery<utblMstAppointment>("udspAppointmentCalenderSelectNewByTransactionId @TransactionID", parTransactionID).ToList();
            }
            return objList;
        }
   
        public IEnumerable<utblMstAppointment> GetAdminCalenderList(string Email)
        {
            List<utblMstAppointment> objList = new List<utblMstAppointment>();
            var parEmail = new SqlParameter("@Email", Email);
            objList = objDB.Database.SqlQuery<utblMstAppointment>("udspAdminAppointmentCalenderSelect @Email", parEmail).ToList();
            return objList;
        }

        //Old Version Agent Calendar list
        public IEnumerable<MstClientCalenderView> GetAgentCalenderList(string AgentID)
        {
            List<MstClientCalenderView> objList = new List<MstClientCalenderView>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            objList = objDB.Database.SqlQuery<MstClientCalenderView>("udspAppointmentAgentCalenderSelect @AgentID", parAgentID).ToList();
            return objList;
        }

        //New Version Agent Calendar list
        public IEnumerable<MstClientCalenderView> GetAgentCalenderListVersion2(string AgentID)
        {
            List<MstClientCalenderView> objList = new List<MstClientCalenderView>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            objList = objDB.Database.SqlQuery<MstClientCalenderView>("udspAppointmentAgentCalenderSelectVersion2 @AgentID", parAgentID).ToList();
            return objList;
        }

        //New Version Agent Calendar list
        public IEnumerable<MstClientCalenderView> GetAdminCalenderListVersion2()
        {
            List<MstClientCalenderView> objList = new List<MstClientCalenderView>();
            objList = objDB.Database.SqlQuery<MstClientCalenderView>("udspAppointmentAdminCalenderSelectVersion2").ToList();
            return objList;
        }



        //New Version Agent Upcoming Event list
        public IEnumerable<MstClientCalenderView> GetAgentUpcomingEvents(string AgentID)
        {
            List<MstClientCalenderView> objList = new List<MstClientCalenderView>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            objList = objDB.Database.SqlQuery<MstClientCalenderView>("GetupcomingEvents @AgentID", parAgentID).ToList();
            return objList;
        }

        //New Version Admin Upcoming Event list
        public IEnumerable<MstClientCalenderView> GetAdminUpcomingEvents()
        {
            List<MstClientCalenderView> objList = new List<MstClientCalenderView>();
            objList = objDB.Database.SqlQuery<MstClientCalenderView>("GetAdminupcomingEvents").ToList();
            return objList;
        }

        //New Version Agent Event list by Date
        public IEnumerable<MstClientCalenderView> GetAgentEventsByDate(string AgentID,DateTime?date)
        {
            List<MstClientCalenderView> objList = new List<MstClientCalenderView>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            var parDate = new SqlParameter("@Date", date);
            objList = objDB.Database.SqlQuery<MstClientCalenderView>("GetEventsByDate @AgentID,@Date", parAgentID,parDate).ToList();
            return objList;
        }

        //New Version Admin Event list by Date
        public IEnumerable<MstClientCalenderView> GetAdminEventsByDate(DateTime? date)
        {
            List<MstClientCalenderView> objList = new List<MstClientCalenderView>();
            var parDate = new SqlParameter("@Date", date);
            objList = objDB.Database.SqlQuery<MstClientCalenderView>("GetAdminEventsByDate @Date", parDate).ToList();
            return objList;
        }

        //New Version  Agent Event Deal Graph
        public IEnumerable<GraphPoints> GetGraphData(string AgentID)
        {
            List<GraphPoints> objList = new List<GraphPoints>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            objList = objDB.Database.SqlQuery<GraphPoints>("GetDealChartData @AgentID", parAgentID).ToList();
            return objList;
        }


     

        //New Version  Admin Event Deal Graph
        public IEnumerable<GraphPoints> GetAdminGraphData()
        {
            List<GraphPoints> objList = new List<GraphPoints>();
            objList = objDB.Database.SqlQuery<GraphPoints>("GetAdminDealChartData").ToList();
            return objList;
        }

        //New Version  Agent Client  Graph
        public IEnumerable<GraphPoints> GetClientGraphData(string AgentID)
        {
            List<GraphPoints> objList = new List<GraphPoints>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            objList = objDB.Database.SqlQuery<GraphPoints>("GetClientChartData @AgentID", parAgentID).ToList();
            return objList;
        }

        //New Version  Agent Client  Graph
        public IEnumerable<GraphPoints> GetAdminClientGraphData()
        {
            List<GraphPoints> objList = new List<GraphPoints>();
            objList = objDB.Database.SqlQuery<GraphPoints>("GetAdminClientChartData").ToList();
            return objList;
        }

        //New Version  Agent Total Deals 
        public int AgentTotalDeals(string AgentID)
        {
            int TotalDeals;
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            TotalDeals = objDB.Database.SqlQuery<int>("GetTotalAgetDeals @AgentID", parAgentID).FirstOrDefault();
            return TotalDeals;
        }
        //New Version  Admin Total Deals 
        public int AdminTotalDeals()
        {
            int TotalDeals;
            TotalDeals = objDB.Database.SqlQuery<int>("GetTotalAdminDeals").FirstOrDefault();
            return TotalDeals;
        }
        //New Version  Agent Total Deals 
        public int AgentTotalClients(string AgentID)
        {
            int TotalDeals;
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            TotalDeals = objDB.Database.SqlQuery<int>("GetTotalAgetClients @AgentID", parAgentID).FirstOrDefault();
            return TotalDeals;
        }

        //New Version  Admin Total Deals 
        public int AdminTotalClients()
        {
            int TotalDeals;
            TotalDeals = objDB.Database.SqlQuery<int>("GetTotalAdminClients").FirstOrDefault();
            return TotalDeals;
        }

        //New Version  Admin Total Deals 
        public int AdminTotalAgents()
        {
            int TotalDeals;
            TotalDeals = objDB.Database.SqlQuery<int>("GetTotalAdminAgents").FirstOrDefault();
            return TotalDeals;
        }
        //New Version Agent Autocomplete
        public IEnumerable<SearchAutoCompleteAgent> AgentAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteAgent> obj = new List<SearchAutoCompleteAgent>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteAgent>("AgentAutocomplete @SearchTerm", searchParam).ToList();
            return obj;
        }

        public IEnumerable<utblMstAppointment> GetAminCalenderList()
        {
            List<utblMstAppointment> objList = new List<utblMstAppointment>();
            objList = objDB.Database.SqlQuery<utblMstAppointment>("udspAppointmentAdminCalenderSelect").ToList();
            return objList;
        }




        public bool emailPreferences(string Email)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var result = objDB.Database.SqlQuery<bool>("select EventCalendar from AspNetUsers where UserName=@Email", parEmail).FirstOrDefault();
            return result;
        }

        public MstAgentClientNameSelect GetNameEmail(string ClientID)
        {
            MstAgentClientNameSelect obj = new MstAgentClientNameSelect();
            var parEmail = new SqlParameter("@Email", ClientID);
            obj = objDB.Database.SqlQuery<MstAgentClientNameSelect>("udspEventCalAgentNameSelect @Email", parEmail).FirstOrDefault();
            return obj;
        }

        public MstAdminClientNameSelect GetAdminClientName(string AdminEmail, string ClientEmail)
        {
            MstAdminClientNameSelect obj = new MstAdminClientNameSelect();
            var parAdminEmail = new SqlParameter("@AdminEmail", AdminEmail);
            var parClientEmail = new SqlParameter("@ClientEmail", ClientEmail);

            obj = objDB.Database.SqlQuery<MstAdminClientNameSelect>("udspUserAdminNameSelect @AdminEmail,@ClientEmail", parAdminEmail, parClientEmail).FirstOrDefault();
            return obj;
        }

        public UserProfileView GetUserDetails(string ClientID)
        {
            var model = new UserProfileView();
            if (!string.IsNullOrEmpty(ClientID))
            { //If statement added by Neha
                var parClientID = new SqlParameter("@ClientID", ClientID);
                model = objDB.Database.SqlQuery<UserProfileView>("udspDealUserProfileSelect @ClientID", parClientID).FirstOrDefault();
            }
            return model;
        }

        public IEnumerable<Events> EventList()
        {
            List<Events> objlist = new List<Events>();
            objlist = objDB.Database.SqlQuery<Events>("select CalEventID,EventText from utblMstEventCalMasters").ToList();
            return objlist;
        }

        public string EditEvent(utblMstAppointment Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Item.Id);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parAgentID = new SqlParameter("@AgentID", Item.AgentID);
            var parIsContingency = new SqlParameter("@IsContingency", Item.IsContingency);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parStartDate = new SqlParameter("@StartDate", Item.StartDate);
            var parEndDate = new SqlParameter("@EndDate", Item.EndDate);
            var parcolor = new SqlParameter("@color", Item.color);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstAppointmentUpdate @Id,@Email,@AgentID,@IsContingency,@Description,@StartDate,@EndDate,@color", parId, parEmail, parAgentID, parIsContingency, parDescription, parStartDate, parEndDate, parcolor).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public string EditEventClient(utblMstAppointment Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Item.Id);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parAgentID = new SqlParameter("@AgentID", Item.AgentID);
            var parIsContingency = new SqlParameter("@IsContingency", Item.IsContingency);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parStartDate = new SqlParameter("@StartDate", Item.StartDate);
            var parEndDate = new SqlParameter("@EndDate", Item.EndDate);
            var parcolor = new SqlParameter("@color", Item.color);
            var parRepeatEvent = new SqlParameter("@RepeatEvent", Item.RepeatEvent);
            var parRepeatFrequency = new SqlParameter("@RepeatFrequency", Item.RepeatFrequency);
            var parRepeatInterval = new SqlParameter("@RepeatInterval", Item.RepeatInterval);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstAppointmentUpdateClient @Id,@Email,@AgentID,@IsContingency,@Description,@StartDate,@EndDate,@color,@RepeatEvent,@RepeatFrequency,@RepeatInterval", parId, parEmail, parAgentID, parIsContingency, parDescription, parStartDate, parEndDate, parcolor, parRepeatEvent, parRepeatFrequency, parRepeatInterval).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }



        public utblMstAppointment GetAppointmentByID(int ID)
        {
            utblMstAppointment objItem = objDB.utblMstAppointments.FirstOrDefault(p => p.Id == ID);
            return objItem;
        }
        //Added by Sonika 12-07-2019
        public IEnumerable<MstAppointmentCalendarModel> GetCalenderListByTransactionIdNew(string TransactionID)
        {
            List<MstAppointmentCalendarModel> objList = new List<MstAppointmentCalendarModel>();
            if (!string.IsNullOrEmpty(TransactionID))
            {
                var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
                objList = objDB.Database.SqlQuery<MstAppointmentCalendarModel>("udspAppointmentCalenderSelectNewByTransactionIdNew @TransactionID", parTransactionID).ToList();
            }
            return objList;
        }

        public IEnumerable<SearchAutoCompleteEventViewModel> EventListSearch(string SearchTerm)
        {
            List<SearchAutoCompleteEventViewModel> objlist = new List<SearchAutoCompleteEventViewModel>();
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            objlist = objDB.Database.SqlQuery<SearchAutoCompleteEventViewModel>("udspEventsGetAutoComplete  @SearchTerm", parSearchTerm).ToList();
            return objlist;
        }
        /// <summary>
        /// Added by sonika
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public string AddEvent(utblMstAppointment Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            //  var parId = new SqlParameter("@Id", Item.Id);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parAgentID = new SqlParameter("@AgentID", Item.AgentID);
            var parIsContingency = new SqlParameter("@IsContingency", Item.IsContingency);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parStartDate = new SqlParameter("@StartDate", Item.StartDate);
            var parEndDate = new SqlParameter("@EndDate", Item.EndDate);
            var parcolor = new SqlParameter("@color", Item.color);
            var parTransactionID = new SqlParameter("@TransactionId", Item.TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstAppointmentInsert @Email,@AgentID,@IsContingency,@Description,@StartDate,@EndDate,@color,@TransactionId", parEmail, parAgentID, parIsContingency, parDescription, parStartDate, parEndDate, parcolor, parTransactionID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        //Added by Neha
        public string AddClientEvent(utblMstAppointment Item)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            //  var parId = new SqlParameter("@Id", Item.Id);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parAgentID = new SqlParameter("@AgentID", Item.AgentID);
            var parIsContingency = new SqlParameter("@IsContingency", Item.IsContingency);
            var parDescription = new SqlParameter("@Description", Item.Description);
            var parStartDate = new SqlParameter("@StartDate", Item.StartDate);
            var parEndDate = new SqlParameter("@EndDate", Item.EndDate);
            var parcolor = new SqlParameter("@color", Item.color);
            var parTransactionID = new SqlParameter("@TransactionId", Item.TransactionID);
            var parRepeatEvent = new SqlParameter("@RepeatEvent", Item.RepeatEvent);
            var parRepeatFrequency = new SqlParameter("@RepeatFrequency", Item.RepeatFrequency);
            var parRepeatInterval = new SqlParameter("@RepeatInterval", Item.RepeatInterval);
            var parcreatedby = new SqlParameter("@createdby", Item.createdby);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstAppointmentInsertClientDealEvent @Email,@AgentID,@IsContingency,@Description,@StartDate,@EndDate,@color,@TransactionId,@RepeatEvent,@RepeatFrequency,@RepeatInterval,@createdby", parEmail, parAgentID, parIsContingency, parDescription, parStartDate, parEndDate, parcolor, parTransactionID, parRepeatEvent, parRepeatFrequency, parRepeatInterval, parcreatedby).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        //added by sonika
        public string RemoveDealEventNew(int Id)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Id);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstLiveDealDeleteventNew @Id", parId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public AppointmentViewModelClientEvent GetAppointmentByIDNew(int ID)
        {
            AppointmentViewModelClientEvent objList = new AppointmentViewModelClientEvent();
            var parID = new SqlParameter("@Id", ID);
            objList = objDB.Database.SqlQuery<AppointmentViewModelClientEvent>("Usp_utblMstAppointments_GetById @Id", parID).FirstOrDefault();


            return objList;
        }


        public IEnumerable<AppointmentViewModel> GetCalenderListWithUserRole(string AgentID, string TransactionID)
        {
            List<AppointmentViewModel> objList = new List<AppointmentViewModel>();
            var parAgentID = new SqlParameter("@AgentID", AgentID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<AppointmentViewModel>("udspAppointmentCalenderSelectWithUserRole @AgentID,@TransactionID", parAgentID, parTransactionID).ToList();
            return objList;
        }
        public IEnumerable<AppointmentViewModel> GetCalenderListByTransactionIdNew1(string TransactionID)
        {
            List<AppointmentViewModel> objList = new List<AppointmentViewModel>();
            if (!string.IsNullOrEmpty(TransactionID))
            {
                var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
                objList = objDB.Database.SqlQuery<AppointmentViewModel>("udspAppointmentCalenderSelectNewByTransactionIdNew1 @TransactionID", parTransactionID).ToList();
            }
            return objList;
        }
        public IEnumerable<AppointmentViewModel> GetAgentCalenderList(int PageNo, int PageSize, out int TotalRows, string AgentID, string SearchTerm = "")
        {
            List<AppointmentViewModel> objList = new List<AppointmentViewModel>();
            if (SearchTerm != null)
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }

            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var parAgentID = new SqlParameter("@AgentID", AgentID);

            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<AppointmentViewModel>("udspAgentCalendarGetPaged @Start,@PageSize,@SearchTerm,@AgentID,@TotalCount out", parStart, parPageSize, parSearchTerm, parAgentID, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }

        //New Version Client Activity
        public IEnumerable<MstViewEmailActivityLog> GetClientActivityAdmin()
        {
            return objDB.Database.SqlQuery<MstViewEmailActivityLog>("udspTrackClientActivityEmailLogsAdmin").ToList();
        }
    }
}
