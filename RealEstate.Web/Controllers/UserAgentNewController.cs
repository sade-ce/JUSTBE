using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using RealEstate.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using RealEstate.Entities.Models;
using Newtonsoft.Json.Linq;

namespace RealEstate.Web.Controllers
{
    [Authorize(Roles = "Agent,Admin")]
    public class UserAgentNewController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        EFDBContext db = new EFDBContext();
        dalDealAdmin ObjDal = new dalDealAdmin();
        dalUser objUser = new dalUser();

        #region Membership Initialization
        public UserAgentNewController()
        {
        }
        public UserAgentNewController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

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

        public ActionResult List( int PageNo = 1, int PageSize = 10, string sortOrder = "", string searchterm = "",string year="", string type = "",string stage="",string tier="")
        {
            ViewBag.ActiveURL = "ClientList";
          
            int TotalRow;
            UserListViewModel objUsers = new UserListViewModel();
            objUsers.AgentClientView = new MstAgentsClientView();
            objUsers.UserSortingInfo = new UserSortingInfo {
                CurrentSort = sortOrder,
                NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "",
                PhoneSort  = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc",
                TypeSort = sortOrder == "type_asc" ? "type_desc" : "type_asc",
                YearSort  = sortOrder == "year_asc" ? "year_desc" : "year_asc",
                StageSort = sortOrder == "stage_asc" ? "stage_desc" : "stage_asc",
                TierSort=sortOrder== "tier_asc" ? "tier_desc" : "tier_asc",
            };
            if (searchterm != null)
            {
                searchterm = searchterm.Trim();
                searchterm = Regex.Replace(searchterm, @"\s+", " ");
            }
            objUsers.UserListFiterInfo = new UserListFiterInfo
            {
                SearchFilter = searchterm,
                YearFilter = year,
                TypeFilter=type,
                StageFilter=stage,
            };
            //if (searchterm != null)
            //{
            //    searchterm = searchterm.Trim();
            //    searchterm = Regex.Replace(searchterm, @"\s+", " ");
            //}
            //else
            //{
            //    searchterm = currentFilter;
            //}
            //ViewBag.CurrentFilter = searchterm;
            string AgentEmail = User.Identity.Name;
            var AgentID = UserManager.FindByEmail(User.Identity.Name);
            objUsers.AgentClientView.ClientList = objUser.GetAgentClientListNew(PageNo, PageSize, out TotalRow, AgentID.Id, searchterm,year,type, sortOrder,stage,tier);
            //  TotalRow = objUsers.AgentClientView.ClientList.Count();
        
            objUsers.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
        
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvUserList", objUsers);
            }
            return View(objUsers);
        }



        public ActionResult DealList(string id, int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            MstCreateClientAndAssignAgentModel Model = new MstCreateClientAndAssignAgentModel();
            string AgentEmail = User.Identity.Name;
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            Model.ClientList = objUser.ClientDealListVersion2(PageNo, PageSize, out TotalRow, Agent.Id, id);
            return PartialView("pvClientDeals", Model.ClientList);
         
        }

        public async Task<ActionResult> ClientProfile(string id,int PageNo = 1, int PageSize = 10)

        {
            //UserEdit model = new UserEdit();

            var user = await UserManager.FindByIdAsync(id);
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            //model = objUser.GetAdminEdit(user.Email);
            //return View(model);

            ViewBag.ActiveURL = "ClientProfile";
            MstCreateClientAndAssignAgentModel Model = new MstCreateClientAndAssignAgentModel();
            Model.Register = objUser.GetClientProfileVersion2(user.Email);
            Model.Register.HomeAnniversary = ObjDal.GetClientHomeAnniversary(id);
            Model.Register.UserRole = "Client";
            ViewBag.AgentID = Agent.Id;
        
            ViewBag.ClientDeatils = user.Name + "" + user.LastName + "(" + user.Email + ")"; 
            //Model.Register.AgentEmail = User.Identity.Name;
            //Model.Register.AgentID = Agent.Id;
            //Model.utblMstTrackDealMasters.AgentID = Agent.Id; //Required to Add New Transaction
            Model.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            Model.ClientDropDown = ObjDal.GetClientDDL();
            Model.AgentDropDown = ObjDal.GetAgentDDL();
            Model.NeighborhoodDDL = ObjDal.GetNeighborhood();
          
            int TotalRow;
            if (User.IsInRole("Admin"))
            {
                Model.ClientActivity = objUser.GetAdminClientActivityVersion2(id);
                Model.ClientList = objUser.ClientAdminDealListVersion2(PageNo, PageSize, out TotalRow, id);
            }
            else
            {
                Model.ClientActivity = objUser.GetClientActivityVersion2(Agent.Id, id);
                Model.ClientList = objUser.ClientDealListVersion2(PageNo, PageSize, out TotalRow, Agent.Id, id);
            }
        
            Model.BuyerQuiz = objUser.GetClientBuyerQuizVersion2(id);
            Model.SellerQuiz = objUser.GetClientSellerQuizVersion2(id);
       
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("pvClientDeals", Model.ClientList);

            //}
            //AddRolesToViewData();



            return View(Model);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MstCreateClientAndAssignAgentModel Itemdata, FormCollection form)
        {

            
            ViewBag.ActiveURL = "ClientProfile";
           
            //if (ModelState.IsValid)
            //{
                //Image
                var image = form["upfiles"];
                if (!string.IsNullOrEmpty(image))
                {
                    if (Itemdata.Register.UserPhotoNormal != null)
                    {
                        if (Itemdata.Register.UserPhotoNormal.IndexOf("data:image") == -1)
                        {
                            string ExistingFilePath = Server.MapPath(Itemdata.Register.UserPhotoNormal);
                            FileInfo ExistingFileInfo = new FileInfo(ExistingFilePath);
                            if (ExistingFileInfo.Exists)
                                ExistingFileInfo.Delete();
                        }
                    }
                    if (image == "PhotoDeleted")
                    {

                        Itemdata.Register.UserPhotoNormal = "";
                        Itemdata.Register.UserPhotoThumb = "";
                    }
                    else
                    {
                        dynamic data = JObject.Parse(image);
                        string imagename = data.output.name;
                        string imagedata = data.output.image;
                        string imageType = imagedata.Substring(0, imagedata.IndexOf(',', imagedata.IndexOf(',')) + 1);
                        string convert = imagedata.Replace(imageType, String.Empty);


                        var guid = Guid.NewGuid().ToString().Substring(0, 6);
                        string fileName = "/img/Users/" + guid + imagename;

                        byte[] image64 = Convert.FromBase64String(convert);
                        using (var ms = new MemoryStream(image64))
                        {
                            var images = System.Drawing.Image.FromStream(ms);
                            System.Drawing.Image img = images.GetThumbnailImage(Convert.ToInt32(data.output.width), Convert.ToInt32(data.output.height), null, IntPtr.Zero);
                            img.Save(Server.MapPath(fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                        }

                        Itemdata.Register.UserPhotoNormal = fileName;
                        Itemdata.Register.UserPhotoThumb = fileName;
                    }
                }
                //Image
                TempData["ErrMsg"] = objUser.AdminEditProfileVersion2(Itemdata.Register);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                   // return RedirectToAction("ClientProfile", "UserAgentNew", new { id = Itemdata.Register.ClientID });

                }
            //}
            return RedirectToAction("ClientProfile", "UserAgentNew", new {id= Itemdata.Register.ClientID });
        }



        public ActionResult autocomplete(string term)
        {
            var AgentID = UserManager.FindByEmail(User.Identity.Name);
            UserListViewModel objUsers = new UserListViewModel();
            objUsers.ListSearchAutoComplete = objUser.UserAutoCompletNew(term, AgentID.Id);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objUsers.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReferalSource(string term)
        {
            var AgentID = UserManager.FindByEmail(User.Identity.Name);
            UserListViewModel objUsers = new UserListViewModel();
            objUsers.ListSearchAutoComplete = objUser.ReferalSourceAutoCompletNew(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objUsers.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Neighborhood(string term)
        {
            var AgentID = UserManager.FindByEmail(User.Identity.Name);
            UserListViewModel objUsers = new UserListViewModel();
            objUsers.NeighborhodSearchAutoComplete = objUser.NeighborhoodAutoCompletNew(term);
            var result = new List<KeyValuePair<string, int>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objUsers.NeighborhodSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, int>(item.searchResult, item.CityID));

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Partner(string term)
        {
            var AgentID = UserManager.FindByEmail(User.Identity.Name);
            UserListViewModel objUsers = new UserListViewModel();
            objUsers.ClientSearchAutoComplete = objUser.PartnerAutoCompletNew(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objUsers.ClientSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.Id));

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClientAutocomplete(string term)
        {
            var AgentID = UserManager.FindByEmail(User.Identity.Name);
            UserListViewModel objUsers = new UserListViewModel();
            objUsers.ClientSearchAutoComplete = objUser.ClientAutocompleteVersion2(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objUsers.ClientSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.Id));

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Archive(string Id, int PageNo ,int PageSize, int ListCount, string sortOrder = "", string searchterm = "", string year = "", string type = "", string stage = "")
        {
            ViewBag.ActiveURL = "ClientList";

        

                    TempData["ErrMsg"] = objUser.ArchiveUser(Id);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                    }
            ListCount--;
                    if (ListCount == 0)
                ListCount = 1;
        
            return RedirectToAction("List", new { PageNo = PageNo, PageSize = PageSize,  sortOrder= sortOrder,  searchterm= searchterm,  year= year,  type=type,  stage=stage });
        }


        public ActionResult Create()
        {
            ViewBag.ActiveURL = "ClientProfile";
            MstCreateClientAndAssignAgentModel Model = new MstCreateClientAndAssignAgentModel();
            //Model.AssignAgent = new DealAdminAssignAgentManageModel();

            

            Model.Register = new Entities.ViewModels.UserAdminRegisterModel();
            Model.Register.UserRole = "Client";
            Model.Register.AgentEmail = User.Identity.Name;
            Model.AgentDropDown = ObjDal.GetAgentDDL();
            Model.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            Model.NeighborhoodDDL = ObjDal.GetNeighborhood();


            AddRolesToViewData();
            return View(Model);
        }


        private void AddRolesToViewData()
        {
            var rolesList = new List<RoleViewModel>();
            foreach (var role in _db.Roles)
            {
                var roleModel = new RoleViewModel(role);
                rolesList.Add(roleModel);
            }
            var list = new SelectList(rolesList, "RoleName", "RoleName");
            ViewBag.Roles = list;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MstCreateClientAndAssignAgentModel userViewModel, string UserBy, FormCollection form)
        {
            ViewBag.ActiveURL = "ClientProfile";
            userViewModel.AssignAgent = new DealAdminAssignAgentManageModel();
            userViewModel.AgentDropDown = ObjDal.GetAgentDDL();
            userViewModel.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            userViewModel.NeighborhoodDDL = ObjDal.GetNeighborhood();
            string TempPassword = "JUSTBE123";//GeneratePassword();//Membership.GeneratePassword(6, 0);
            //string TempPassword = GeneratePassword();//Membership.GeneratePassword(6, 0);
            if (ModelState.IsValid)
            {
                String UserName = userViewModel.Register.Email;
                UserName = UserName.Trim();
                UserName = Regex.Replace(UserName, @"\s+", " ");
                var user = new ApplicationUser
                {
                    Name = userViewModel.Register.Name,
                    LastName = userViewModel.Register.LastName,
                    Phone = userViewModel.Register.PhoneNumber,
                    UserName = UserName,
                    Email = userViewModel.Register.Email,
                    PhoneNumber = userViewModel.Register.PhoneNumber,
                    EmailConfirmed = true,
                    UserRole = "Client",
                    EventCalendar = true,
                    StatusTimeline = false,
                    SettlementDate = true,
                    DocumentEmail = true,
                    ColorCode = color()

                };

                var adminresult = await UserManager.CreateAsync(user, TempPassword);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    var result = await this.UserManager.AddToRolesAsync(user.Id, userViewModel.Register.UserRole);
                    if (result.Succeeded)
                    {
                        //Image
                        var image = form["upfiles"];
                        string fileName = "";
                        if (!string.IsNullOrEmpty(image))
                        {
                            dynamic data = JObject.Parse(image);
                            string imagename = data.output.name;
                            string imagedata = data.output.image;
                            string imageType = imagedata.Substring(0, imagedata.IndexOf(',', imagedata.IndexOf(',')) + 1);
                            string convert = imagedata.Replace(imageType, String.Empty);


                            var guid = Guid.NewGuid().ToString().Substring(0, 6);
                            fileName = "/img/Users/" + guid + imagename;

                            byte[] image64 = Convert.FromBase64String(convert);
                            using (var ms = new MemoryStream(image64))
                            {
                                var images = System.Drawing.Image.FromStream(ms);
                                System.Drawing.Image img = images.GetThumbnailImage(Convert.ToInt32(data.output.width), Convert.ToInt32(data.output.height), null, IntPtr.Zero);
                                img.Save(Server.MapPath(fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                            }


                        }
                        //Image
                        //if (userViewModel.Register.UserPhotoThumb != null || userViewModel.Register.UserPhotoNormal != null)
                        //{
                        //using (var db_ = new EFDBContext())
                        //{
                            //utblUserDetail ObjDetail = new utblUserDetail()
                            //{
                            userViewModel.Register.Email = userViewModel.Register.Email;
                            userViewModel.Register.UserPhotoNormal = fileName;
                            userViewModel.Register.UserPhotoThumb = fileName;
                                //UserPhotoNormal = userViewModel.Register.UserPhotoNormal,
                                //UserPhotoThumb = userViewModel.Register.UserPhotoThumb
                           // };.

                            //try
                            //{
                                TempData["ErrMsg"] = objUser.AdminEditProfileVersion2(userViewModel.Register);
                                //db_.utblUserDetails.Add(ObjDetail);
                                //db_.SaveChanges();


                            //}
                            //catch (Exception ex)
                            //{
                            //    TempData["ErrMsg"] = ex.Message;
                            //}
                        //}
                        // }

                        var Agent = UserManager.FindByEmail(User.Identity.Name);
                        userViewModel.Register.Status = "Active";
                        userViewModel.Register.ClientID = user.Id;
                        userViewModel.Register.AgentID = Agent.Id;
                        TempData["ErrMsg"] = ObjDal.CreateClientAndAssignAgentVersion2(userViewModel.Register);
                        if (TempData["ErrMsg"].ToString() == "0")
                        {
                            TempData["ErrMsg"] = null;
                            var Client = UserManager.FindById(user.Id);
                            int HtmlMail = SignUpEmailConfirmation(Agent.Name, Agent.Email, "Thanks for registering with Just BE.", Client.Name, Client.Email);
                            int MailStatus = SendEmailConfirmationToken(user.Id, userViewModel.Register.Name, "Welcome to Just BE | Real Estate", user.Email, TempPassword);
                            return RedirectToAction("ClientProfile", "UserAgentNew", new { id = user.Id });
                            //return RedirectToAction("ClientProfile", "UserAgentNew", new { ClientID = user.Id, AgentID = userViewModel.Register.AgentID, area = "Coordinator" });
                        }

                        //return RedirectToAction("list");
                    }
                    else
                    {
                        //TempData["ErrMsg"] = 0;
                        TempData["ErrMsg"] = null;
                        AddRolesToViewData();
                        ModelState.AddModelError("", result.Errors.First());
                        return View(userViewModel);
                    }
                }
                else
                {
                    TempData["ErrMsg"] = null;
                    AddRolesToViewData();
                    ModelState.AddModelError("", "Unable to add client,record already exists...");
                    return View(userViewModel);
                }
            }
            else
            {
                AddRolesToViewData();
            }

            return View(userViewModel);
        }


        public string color()
        {
            var random = new Random();
            string color = String.Format("#{0:X6}", random.Next(0x1000000));
            return color;
        }

        public ActionResult AddTransaction()
        {
            ViewBag.ActiveURL = "AddDeal";
            MstCreateClientAndAssignAgentModel Model = new MstCreateClientAndAssignAgentModel();
            Model.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            var Agent = UserManager.FindByEmail(User.Identity.Name);
            ViewBag.AgentID = Agent.Id;
            return View(Model);
        }
       

        [HttpPost]
        public ActionResult CreateTransaction(MstCreateClientAndAssignAgentModel ItemData)
        {
            string errorMessage = "";
            ItemData.utblMstTrackDealMasters.Status = "Active";
      
                errorMessage = ObjDal.AssignAgentVersion2(ItemData.utblMstTrackDealMasters);
                if (errorMessage == "0")
                {
                var Agent = UserManager.FindById(ItemData.utblMstTrackDealMasters.AgentID);
                var Client = UserManager.FindById(ItemData.utblMstTrackDealMasters.ClientID);
                 int MailStatus = SignUpEmailConfirmation(Agent.Name, Agent.Email, "Thanks for registering with Just BE | Real Estate", Client.Name, Client.Email);
                // string url = Url.Action("ClientDetails", "DealAgent", new { ClientID = ItemData.utblMstTrackDealMasters.ClientID, AgentID = ItemData.utblMstTrackDealMasters.AgentID });

            }
            return Json(new { success = true });

        }

     
        public ActionResult DeleteTransaction(string TransactionID, string clientId, string agentID = "")
        {
            string errorMessage = "";
            errorMessage = ObjDal.DeleteDealVersion2(TransactionID);
            //return RedirectToAction("ClientProfile", new { Id = clientId });
            return Json(new { success = true });
        }
  

        #region Create Transaction NewTransactionConfirmation
        private int SignUpEmailConfirmation(string AgentName, string AgentEmail, string subject, string ClientName, string ClientEmail)
        {


            //string code = UserManager.GenerateEmailConfirmationToken(userID);
            //string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code, area = "" }, protocol: Request.Url.Scheme);

            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            //Create the SMTP Client
            SmtpClient client = new SmtpClient();
            client.Host = settings.Smtp.Network.Host;
            client.Port = settings.Smtp.Network.Port;
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = false;
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(settings.Smtp.Network.UserName, "Just BE.");

            StringBuilder mailbody = new StringBuilder();

            mm.To.Add(AgentEmail);
            mm.Priority = MailPriority.High;

            mm.Subject = "You have a new client assignment";
            mm.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
           // mm.SubjectEncoding = System.Text.Encoding.Default;
            mm.Body = this.PopulateBody(AgentName, ClientName, ClientEmail);
            mm.IsBodyHtml = true;

            try
            {
                client.Send(mm);
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        
        private string PopulateBody(string AgentName, string ClientName, string ClientEmail)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/AgentEmail.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{AgentName}", AgentName);
            body = body.Replace("{ClientName}", ClientName);
            body = body.Replace("{ClientEmail}", ClientEmail);
            body = body.Replace("{Url}", "http://justbere.com/Account/Login");
            return body;
        }
        #endregion

        #region Create User SignUpUserEmailConfirmation
        private int SendEmailConfirmationToken(string userID, string Name, string subject, string email, string pass)
        {


            //string code = UserManager.GenerateEmailConfirmationToken(userID);
            //string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code, area = "" }, protocol: Request.Url.Scheme);

            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            //Create the SMTP Client
            SmtpClient client = new SmtpClient();
            client.Host = settings.Smtp.Network.Host;
            client.Port = settings.Smtp.Network.Port;
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = false;
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(settings.Smtp.Network.UserName, "Just BE");

            StringBuilder mailbody = new StringBuilder();

            mm.To.Add(email);
            mm.Priority = MailPriority.High;

            mm.Subject = "Welcome to Just BE.";
            mm.Body = this.CreatePopulateBody(Name, email, pass);
            mm.IsBodyHtml = true;

            try
            {
                client.Send(mm);
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }


        private string CreatePopulateBody(string Name, string userName, string password)
        {


            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/Email.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FirstName}", Name);
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Password}", password);
            body = body.Replace("{Url}", "http://justbere.com/Account/WebLogin?Email=" + Uri.EscapeDataString(userName));
            return body;
        }
        #endregion
    }
}