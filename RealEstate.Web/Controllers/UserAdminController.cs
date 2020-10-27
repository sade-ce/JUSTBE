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
    [Authorize(Roles = "Admin")]
    public class UserAdminController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        EFDBContext db = new EFDBContext();
        dalDealAdmin ObjDal = new dalDealAdmin();
        dalUser objUser = new dalUser();

        #region Membership Initialization
        public UserAdminController()
        {
        }
        public UserAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        public ActionResult List(int PageNo = 1, int PageSize = 10, string searchterm = "")
        {
            ViewBag.ActiveURL = "/UserAdmin/List";

            int TotalRow;
            UserListViewModel objUsers = new UserListViewModel();
            if (searchterm != null)
            {
                searchterm = searchterm.Trim();
                searchterm = Regex.Replace(searchterm, @"\s+", " ");
            }
            string AgentEmail = User.Identity.Name;
            if (User.IsInRole("Agent"))
            {
                //objUsers.UserList = UserManager.Users.Where(x => searchterm == null || x.UserName.StartsWith(searchterm) && x.AgentEmail == AgentEmail).OrderByDescending(x => x.UserRole).Skip((PageNo - 1) * PageSize).Take(PageSize).ToList();
                //<--Added by Neha(16-02-2019)--->
                objUsers.UserList = UserManager.Users.Where(x => searchterm == null || x.UserName.StartsWith(searchterm) || x.Name.StartsWith(searchterm) ||x.LastName.StartsWith(searchterm) && x.AgentEmail == AgentEmail).OrderByDescending(x => x.UserRole).ThenBy(x => x.Name).Skip((PageNo - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                // objUsers.UserList = UserManager.Users.Where(x => searchterm == null || x.UserName.StartsWith(searchterm)).OrderByDescending(x => x.UserRole).Skip((PageNo - 1) * PageSize).Take(PageSize).ToList();
                //<--Added by Neha(16-02-2019)--->
                objUsers.UserList = UserManager.Users.Where(x => searchterm == null || x.UserName.StartsWith(searchterm) || x.Name.StartsWith(searchterm) || x.LastName.StartsWith(searchterm)).OrderByDescending(x => x.UserRole).ThenBy(x => x.Name).Skip((PageNo - 1) * PageSize).Take(PageSize).ToList();

            }
            TotalRow = objUsers.UserList.Count();
            objUsers.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            if (Request.IsAjaxRequest())
            {
                return PartialView("pvUserList", objUsers);
            }
            return View(objUsers);
        }


        public ActionResult Create()
        {
            MstCreateClientAndAssignAgentModel Model = new MstCreateClientAndAssignAgentModel();
            Model.AssignAgent = new DealAdminAssignAgentManageModel();

            ViewBag.ActiveURL = "/UserAdmin/List";
            if (User.IsInRole("Agent"))
            {
                Model.Register = new Entities.ViewModels.UserAdminRegisterModel();

                Model.Register.UserRole = "Client";
                Model.Register.AgentEmail = User.Identity.Name;

            }
            Model.AssignAgent.AgentDropDown = ObjDal.GetAgentDDL();
            Model.AssignAgent.ClientTypeDropDown = ObjDal.GetClientTypeDDL();
            AddRolesToViewData();
            return View(Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MstCreateClientAndAssignAgentModel userViewModel, string UserBy, FormCollection form)
        {
            ViewBag.ActiveURL = "/UserAdmin/List";
            userViewModel.AssignAgent = new DealAdminAssignAgentManageModel();
            userViewModel.AssignAgent.AgentDropDown = ObjDal.GetAgentDDL();
            userViewModel.AssignAgent.ClientTypeDropDown = ObjDal.GetClientTypeDDL();

            string TempPassword = "JUSTBE123";//GeneratePassword();//Membership.GeneratePassword(6, 0);
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
                    UserRole = userViewModel.Register.UserRole,
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
                        using (var db_ = new EFDBContext())
                            {
                                utblUserDetail ObjDetail = new utblUserDetail()
                                {
                                    UserName = userViewModel.Register.Email,
                                    UserPhotoNormal = fileName,
                                    UserPhotoThumb = fileName,
                            //UserPhotoNormal = userViewModel.Register.UserPhotoNormal,
                            //UserPhotoThumb = userViewModel.Register.UserPhotoThumb
                        };

                                try
                                {

                                    db_.utblUserDetails.Add(ObjDetail);
                                    db_.SaveChanges();
                                   

                                }
                                catch (Exception ex)
                                {
                                    TempData["ErrMsg"] = ex.Message;
                                }
                            }
                       // }


                        var Agent = UserManager.FindById(userViewModel.Register.AgentID);
                        var Client = UserManager.FindById(user.Id);

                        if (userViewModel.Register.UserRole == "Client")
                        {
                            userViewModel.Register.Status = "Active";
                            userViewModel.Register.ClientID = user.Id;
                            TempData["ErrMsg"] = ObjDal.CreateClientAndAssignAgent(userViewModel.Register);
                            if (TempData["ErrMsg"].ToString() == "0")
                            {
                                 int ClientMail = SendEmailConfirmationToken(user.Id, userViewModel.Register.Name, "Thanks for registering with Just BE.", user.Email, Agent.Id, TempPassword);
                                int HtmlMail = SignUpEmailConfirmation(Agent.Name, Agent.Email, "Just BE | Real Estate", Client.Name, Client.Email);

                                return RedirectToAction("ClientDetails", "DealAgent", new { ClientID = user.Id, AgentID = userViewModel.Register.AgentID, area = "Coordinator" });
                            }

                        }

                        int MailStatus = SendEmailConfirmationToken(user.Id, userViewModel.Register.Name, "Welcome to Just BE | Real Estate", user.Email, user.Id,  TempPassword);
                        if (MailStatus == 0)
                        {
                            TempData["MailMsg"] = 1;
                            return RedirectToAction("list");
                        }
                        else
                        {
                            TempData["MailMsg"] = 2;
                            return RedirectToAction("list");
                        }
                        //return RedirectToAction("list");
                    }
                    else
                    {
                        TempData["ErrMsg"] = 0;
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


        public async Task<ActionResult> Edit(string id)
        {
            UserEdit model = new UserEdit();

            var user = await UserManager.FindByIdAsync(id);
            model = objUser.GetAdminEdit(user.Email);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEdit Itemdata,FormCollection form)
        {
            ViewBag.ActiveURL = "/UserAdmin/List";
        
            if (ModelState.IsValid)
            {
                //Image
                var image = form["upfiles"];
                if (!string.IsNullOrEmpty(image))
                {
                    if (Itemdata.UserPhotoNormal != null)
                    {
                        if (Itemdata.UserPhotoNormal.IndexOf("data:image") == -1)
                        {
                            string ExistingFilePath = Server.MapPath(Itemdata.UserPhotoNormal);
                            FileInfo ExistingFileInfo = new FileInfo(ExistingFilePath);
                            if (ExistingFileInfo.Exists)
                                ExistingFileInfo.Delete();
                        }
                    }
                    if (image == "PhotoDeleted")
                    {

                        Itemdata.UserPhotoNormal = "";
                        Itemdata.UserPhotoThumb = "";
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

                        Itemdata.UserPhotoNormal = fileName;
                        Itemdata.UserPhotoThumb = fileName;
                    }
                }
                //Image
                TempData["ErrMsg"] = objUser.AdminEditProfile(Itemdata);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    return RedirectToAction("list");

                }
            }
            return View(Itemdata);
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

        #region Send Email Confirmation Code block
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResendEmail(string Id)
        {
            var user = await UserManager.FindByIdAsync(Id);
            if (user == null)
            {
                TempData["ErrMsg"] = 0;
                return RedirectToAction("list");
            }
            string TempPassword = GeneratePassword();//Membership.GeneratePassword(6, 0);
            string token = UserManager.GeneratePasswordResetToken(Id);
            var result = await UserManager.ResetPasswordAsync(Id, token, TempPassword);

            if (result.Succeeded)
            {
                int MailStatus = SendEmailConfirmationToken(Id, user.Name, "Forgot Password From Just BE | Real Estate", user.Email,"", TempPassword);
                if (MailStatus == 0)
                {
                    TempData["MailMsg"] = 1;
                    return RedirectToAction("list", "useradmin");
                }
                else
                {
                    TempData["MailMsg"] = 0;
                }
            }
            else
            {
                TempData["ErrMsg"] = 0;
                return RedirectToAction("list", "useradmin");
            }
            return RedirectToAction("list", "useradmin");

        }


        private string PopulateBody(string Name, string userName,string AgentID, string password)
        {

            var Client = UserManager.FindByEmail(userName);

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/Email.html")))
            {
                body = reader.ReadToEnd();
            }


            string Url = "http://justbere.com/Coordinator/ActivityLogs/Track?";
            var uriBuilder = new UriBuilder(Url);
            uriBuilder.Port = -1;
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["ClientID"] = Client.Id;
            query["AgentID"] = AgentID;
            query["Remarks"] = "Seen";
            query["ActivityTitle"] = "Welcome Email";
            query["TrackingSource"] = "Email";
            uriBuilder.Query = query.ToString();
            string ActualUrl = uriBuilder.ToString();

            body = body.Replace("{TrackURL}", ActualUrl);


            body = body.Replace("{FirstName}", Name);
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Password}", password);
            body = body.Replace("{Url}", "http://justbere.com/Account/WebLogin?Email=" + Uri.EscapeDataString(userName));
            return body;
        }

        private int SendEmailConfirmationToken(string userID, string Name, string subject, string email,string AgentID, string pass)
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

            mm.Subject = "Welcome to Just BE | Real Estate";
            mm.Body = this.PopulateBody(Name, email,AgentID, pass);
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
        #endregion

        //
        // POST: /UserAdmin/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, int PgNo, int PgSize, int ListCount, string Email)
        {
            ViewBag.ActiveURL = "/UserAdmin/List";

            if (id == User.Identity.GetUserId())
            {
                TempData["ErrMsg"] = 4;
            }
            else if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                try
                {

                    TempData["ErrMsg"] = objUser.DeleteUser(user.Id,user.Email);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                    }
                    ListCount--;
                    if (ListCount == 0)
                        PgNo = 1;
                }

                catch (Exception)
                {
                    TempData["ErrMsg"] = "....Unable to delete user,Probably there is ongoing deal for this user,delete all the transaction and try again...";
                    //ModelState.AddModelError("", result.Errors.First());

                }

            }
            return RedirectToAction("List", new { PageNo = PgNo, PageSize = PgSize });
        }

        public string color()
        {
            var random = new Random();
            string color = String.Format("#{0:X6}", random.Next(0x1000000));
            return color;
        }


        #region PasswordGenerator
        public string GeneratePassword()
        {
            string PasswordLength = "2";
            string NewPassword = "";
            string allowedChars = "";
            allowedChars = "home,love,nice,deal,fine,site,toft,base,cosy,digs,fate,roof,seat,soil,ward";
            //allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            //allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string IDString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;
            }

            return NewPassword;
        }

        #endregion


        #region SignUpEmailConfirmation
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
            mm.From = new MailAddress(settings.Smtp.Network.UserName, "Just BE");

            StringBuilder mailbody = new StringBuilder();

            mm.To.Add(AgentEmail);
            mm.Priority = MailPriority.High;

            mm.Subject = "You have a new client assignment";
            mm.Body = this.HtmlBody(AgentName, ClientName, ClientEmail);
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

        #endregion
        #region HtmlEmail
        private string HtmlBody(string AgentName, string ClientName, string ClientEmail)
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// added by sonika
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult autocomplete(string term)
        {

            UserListViewModel objUsers = new UserListViewModel();
            objUsers.ListSearchAutoComplete = objUser.userAdminAutoComplete(term);
            var result = new List<KeyValuePair<string, string>>();
            // foreach (var item in Obj.response.addresses_info)
            foreach (var item in objUsers.ListSearchAutoComplete)
            {
                result.Add(new KeyValuePair<string, string>(item.searchResult, item.searchResult));

            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }

    }
}