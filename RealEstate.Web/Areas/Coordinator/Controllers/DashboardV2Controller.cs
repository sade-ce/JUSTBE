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


namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]
    public class DashboardV2Controller : Controller
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
        // GET: Coordinator/DashboardV2
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        public ActionResult Index()
        {
            ViewBag.ActiveURL = "AgentDashboard";
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            try
            {
                ViewBag.DataPoints = JsonConvert.SerializeObject(ObjdalDashboard.GetGraphData(Agent.Id), _jsonSetting);
                ViewBag.ClientDataPoints = JsonConvert.SerializeObject(ObjdalDashboard.GetClientGraphData(Agent.Id), _jsonSetting);
                ViewBag.TotalDeals = ObjdalDashboard.AgentTotalDeals(Agent.Id);
                ViewBag.TotalClients = ObjdalDashboard.AgentTotalClients(Agent.Id);

                ObjDashboard.AgentResourceS = objAgentResourceDal.GetAgentResourceS();
                return View();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return View("Error");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return View("Error");
            }

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


            //return View();
    }
       

        public JsonResult GetCalendarInfo()
        {
            var Agent = UserManager.FindByEmail(User.Identity.Name);

            var Events = ObjdalDashboard.GetAgentCalenderListVersion2(Agent.Id).ToList();

                return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       
        public JsonResult GetUpcomingCalendarInfo()
        {
            var Agent = UserManager.FindByEmail(User.Identity.Name);

            var Events = ObjdalDashboard.GetAgentUpcomingEvents(Agent.Id).ToList();

            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetCalendarInfoByDate(DateTime? date)
        {
            var Agent = UserManager.FindByEmail(User.Identity.Name);

            var Events = ObjdalDashboard.GetAgentEventsByDate(Agent.Id,date).ToList();

            return new JsonResult { Data = Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetChartData()
        {
            var Agent = UserManager.FindByEmail(User.Identity.Name);


            return new JsonResult { Data = JsonConvert.SerializeObject(ObjdalDashboard.GetGraphData(Agent.Id), _jsonSetting), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
    }
}