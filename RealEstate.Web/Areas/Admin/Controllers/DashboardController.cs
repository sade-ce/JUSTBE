using GoogleApiUtils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        dalMstAppointment ObjdalDashboard = new dalMstAppointment();
        MstCalenderManageModel ObjDashboard = new MstCalenderManageModel();
        AgentResourceView objAgentResourceModel = new AgentResourceView();
        dalAgentResource objAgentResourceDal = new dalAgentResource();
        #region Membership Initialization
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion
        // GET: Admin/Dashboard
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        public ActionResult Index()
        {
            ViewBag.ActiveURL = "AdminDashboard";
            var Agent = UserManager.FindByEmail(User.Identity.Name);
           // try
           // {
                ViewBag.DataPoints = JsonConvert.SerializeObject(ObjdalDashboard.GetAdminGraphData(), _jsonSetting);
                ViewBag.ClientDataPoints = JsonConvert.SerializeObject(ObjdalDashboard.GetAdminClientGraphData(), _jsonSetting);
                ViewBag.TotalDeals = ObjdalDashboard.AdminTotalDeals();
                ViewBag.TotalClients = ObjdalDashboard.AdminTotalClients();
                ViewBag.TotalAgents = ObjdalDashboard.AdminTotalAgents();
                //  ObjDashboard.AgentResourceS = objAgentResourceDal.GetAgentResourceS();
                ViewBag.ClientActivity = ObjdalDashboard.GetClientActivityAdmin();
                return View();
           // }
            //catch (System.Data.Entity.Core.EntityException)
            //{
            //    return View("Error");
            //}
            //catch (System.Data.SqlClient.SqlException ex)
            //{
            //    string msg = ex.Message;
            //    return View("Error");
            //}

            //List<DataPoint> dataPoints1 = new List<DataPoint>();
            //    List<DataPoint> dataPoints2 = new List<DataPoint>();
            //    List<DataPoint> dataPoints3 = new List<DataPoint>();

            //    dataPoints1.Add(new DataPoint("Jan", 72));
            //    dataPoints1.Add(new DataPoint("Feb", 67));
            //    dataPoints1.Add(new DataPoint("Mar", 55));
            //    dataPoints1.Add(new DataPoint("Apr", 42));
            //    dataPoints1.Add(new DataPoint("May", 40));
            //    dataPoints1.Add(new DataPoint("Jun", 35));




            //    ViewBag.DataPoints1 = Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints1);


           
        }


        public JsonResult GetCalendarInfoByAgent(string Id)
        {
            var Events = ObjdalDashboard.GetAgentCalenderListVersion2(Id).ToList();

            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetCalendarInfo()
        {
          
            var Events = ObjdalDashboard.GetAdminCalenderListVersion2().ToList();

            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUpcomingCalendarInfo()
        {

            var Events = ObjdalDashboard.GetAdminUpcomingEvents().ToList();

            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUpcomingCalendarInfoByAgent(string Id)
        {
           

            var Events = ObjdalDashboard.GetAgentUpcomingEvents(Id).ToList();

            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetCalendarInfoByDate(DateTime? date)
        {
            var Events = ObjdalDashboard.GetAdminEventsByDate(date).ToList();

            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult GetCalendarInfoByDateAndAgent(DateTime? date,string Id)
        {
            var Events = ObjdalDashboard.GetAgentEventsByDate(Id, date).ToList();

            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetChartData()
        {
            var Agent = UserManager.FindByEmail(User.Identity.Name);


            return new JsonResult { Data = JsonConvert.SerializeObject(ObjdalDashboard.GetAdminGraphData(), _jsonSetting), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }





        public JsonResult AgentResourceList(string TransactionId)
        {

            return Json(objAgentResourceDal.GetAgentResourceS(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAgentResource(AgentResourceView data)
        {
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            data.CreatedBy = Agent.Id;
            string errorMessage = "";
            errorMessage = objAgentResourceDal.Save(data);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAgentResourceByID(int Id)
        {

            var AgentResource = objAgentResourceDal.GetAgentResourceByID(Id);
            AgentResource.Description = System.Web.HttpUtility.HtmlDecode(AgentResource.Description);
            return Json(AgentResource, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAgentResource(AgentResourceView data)
        {
            string errorMessage = "";
            errorMessage = objAgentResourceDal.Edit(data);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteResource(int Id)
        {
            string errorMessage = objAgentResourceDal.Delete(Id);

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult autocomplete(string term)
        {

            ObjDashboard.AgentSearchAutoComplete = ObjdalDashboard.AgentAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in ObjDashboard.AgentSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.Id));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
    }
}