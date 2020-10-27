using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RealEstate.Web.Models;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using RealEstate.Entities.ViewModels;
using RealEstate.Entities.DataAccess;
using System.IO;
using System.Web.Security;
using RealEstate.Entities.Models;
using GoogleApiUtils;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace RealEstate.Web.Controllers
{
    //[AllowAnonymous]
    [Authorize]

    public class AccountController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        dalUser objUser = new dalUser();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login(string returnUrl, string revalidate = "")
        {
            ViewBag.Current = "Index";
            if (revalidate == "true")
            {
                ModelState.AddModelError("", "For security reasons, please re-enter your log-in info.");
                ViewBag.Loginerror = "For security reasons, please re-enter your log-in info.";
            }
            else
            {
                ViewBag.Loginerror = "";
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewBag.Loginerror = "";
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                  //  ModelState.AddModelError("", "Invalid login attempt.");
                    ViewBag.Loginerror = "Invalid login attempt.";
                    return View(model);
            }
        }





        [AllowAnonymous]
        public ActionResult WebLogin(string returnUrl, string Email)
        {
            ViewBag.Current = "Index";
            LoginViewModel objmodel = new LoginViewModel();
            ViewBag.ReturnUrl = returnUrl;
            objmodel.Email = Email;
            return View(objmodel);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WebLogin(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return FirstLogin(returnUrl, ViewBag.message = "WelcomeBack");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }





        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {

            ViewBag.Current = "Index";
            //var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = new ApplicationRole("Client", "Client");
            ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            _db.CreateRole(_roleManager, "Client", "Client");

            //var role = new IdentityRole();
            //role.Name = "Admin";
            //RoleManager.Create(role);

            //If user comes from quiz page
            if (model.Name!= null)
            {
                RegisterManagelModel user = new RegisterManagelModel();
                user.RegisterViewModel = model;
                return View(user);
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterManagelModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {

                    UserName = model.RegisterViewModel.Email,
                    Email = model.RegisterViewModel.Email,
                    Phone = model.RegisterViewModel.PhoneNumber,
                    PhoneNumber = model.RegisterViewModel.PhoneNumber,
                    Name = model.RegisterViewModel.Name,
                    LastName = model.RegisterViewModel.LastName,
                    ISClient = false,
                    UserRole = "Client",
                    EmailConfirmed = true,
                    EventCalendar = true,
                    StatusTimeline = false,
                    SettlementDate = true,
                    DocumentEmail = true,
                    IsBlocked = false
                };
                var result = await UserManager.CreateAsync(user, model.RegisterViewModel.Password);
                if (result.Succeeded)
                {
                    var rolemap = await this.UserManager.AddToRolesAsync(user.Id, "Client");

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    int MailStatus = SignUpEmailConfirmation(user.Name, user.Id, "Thanks for registering with Just BE | Real Estate", user.Email, model.RegisterViewModel.Password);

                    return RedirectToAction("EditProfile", "Account", new { Message = "FirstLogin" });
                }
                AddErrors(result);

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            ViewBag.Current = "Index";

            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ModelState.AddModelError("", "We couldn't find an account associated with that e-mail address. Please try again or create a new account.");
                    return View();
                }


                string TempPassword = GeneratePassword();//Membership.GeneratePassword(6, 0);
                string token = UserManager.GeneratePasswordResetToken(user.Id);
                var result = await UserManager.ResetPasswordAsync(user.Id, token, TempPassword);

                if (result.Succeeded)
                {
                    int MailStatus = SendEmailConfirmationToken(user.Name, user.Id, "Forgot Password", user.Email, TempPassword);
                    if (MailStatus == 0)
                    {
                        TempData["MailMsg"] = 1;
                        return RedirectToAction("ForgotPasswordConfirmation", "Account");
                    }
                    else
                    {
                        TempData["MailMsg"] = 0;
                    }
                }
                else
                {
                    TempData["ErrMsg"] = 0;
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public JsonResult ForgotYourPassword(LoginViewModel model)
        {
            string message = "";

            var user = UserManager.FindByName(model.Email);
            if (user == null || !(UserManager.IsEmailConfirmed(user.Id)))
            {
                // Don't reveal that the user does not exist or is not confirmed

                message = "We couldn't find an account associated with that e-mail address. Please try again or create a new account.";

            }
            else
            {

                string TempPassword = GeneratePassword();//Membership.GeneratePassword(6, 0);
                string token = UserManager.GeneratePasswordResetToken(user.Id);
                var result = UserManager.ResetPassword(user.Id, token, TempPassword);

                if (result.Succeeded)
                {
                    int MailStatus = SendEmailConfirmationToken(user.Name, user.Id, "Forgot Password", user.Email, TempPassword);
                    if (MailStatus == 0)
                    {
                        message = "success";

                    }
                    else
                    {
                        message = "Email is not sent. The server is unable to connect.";
                    }
                }
                else
                {
                   
                    message = "Sorry, Something went wrong. Please try again.";
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        public ActionResult LogOff(string Url)
        {
            AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Brand", "Home");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Brand", "Home");
        }

        public ActionResult EmailPreferences(string ClientID = "", string Message = "")
        {
            //ViewBag.ActiveURL = "Account";
            ViewBag.ActiveURL = "Notification";
            MstUpdateEmailPreferences model = new MstUpdateEmailPreferences();

            if (User.IsInRole("Agent"))
            {
                var Client = UserManager.FindById(ClientID);
                Session["ClientID"] = ClientID;
                model = objUser.GetEmailPreferences(Client.Email);
                ViewBag.message = null;
            }
            else
            {

                model = objUser.GetEmailPreferences(User.Identity.Name);
                ViewBag.message = Message;
            }

            return View(model);
        }

        [HttpPost]

        public async Task<ActionResult> EmailPreferences(MstUpdateEmailPreferences Itemdata)
        {
            //var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (User.IsInRole("Agent"))
            {

                user = await UserManager.FindByIdAsync(Session["ClientID"].ToString());


            }
            if (user != null)
            {





                user.EventCalendar = Itemdata.EventCalendar;
                // user.SettlementDate = Itemdata.SettlementDate;//Commented by Neha
                user.SettlementDate = Itemdata.StatusTimeline;//Added by Neha
                user.StatusTimeline = Itemdata.StatusTimeline;
                user.DocumentEmail = true;
                UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
                {
                    RequireUniqueEmail = false
                };
                var result = await UserManager.UpdateAsync(user);
                //IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {

                    if (User.IsInRole("Agent"))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    else
                    {
                        string Message = (TempData["message"]).ToString();

                        ViewBag.message = Message;
                        if (Message.ToString() == "FirstLogin")
                        {
                            return RedirectToAction("Welcome", "JustBE", new { area = "Client", Message = Message });
                        }
                        if (Message.ToString() == "WelcomeBack")
                        {
                            return RedirectToAction("Welcome", "JustBE", new { area = "Client", Message = Message });
                        }
                        else
                        {
                            //return RedirectToAction("List", "MyDeal", new { area = "Client" });
                            return RedirectToAction("EmailPreferences", "Account", new { area = "" });
                            
                            //return RedirectToAction("Greetings", "Home");

                        }
                    }

                }
            }

            return RedirectToAction("List", "MyDeal", new { area = "Client" });
        }
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                string errorMessage = error;
                if (error.EndsWith("is already taken."))
                {
                    errorMessage = "Email id already registered with us,try login to your account";
                    ModelState.AddModelError("", errorMessage);
                    break;
                }
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { returnUrl = returnUrl });
        }
        private ActionResult FirstLogin(string returnUrl, dynamic ViewBag = null)
        {

            return RedirectToAction("UserAgreement", "About", new { Message = ViewBag });

        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        #region SignUpEmailConfirmation
        private int SignUpEmailConfirmation(string Name, string userID, string subject, string email, string pass)
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

            mm.To.Add(email);
            mm.Priority = MailPriority.High;

            mm.Subject = "Thanks for registering with Just BE.";
            mm.Body = this.PopulateBody(Name, email, "Your registered Password");
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

        #region Edit Profile
        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult EditProfile(string Message = "")
        {
            // ViewBag.ActiveURL = "Account";
            ViewBag.ActiveURL = "EditProfile";
            ViewBag.message = Message;
            UserProfileEditModel model = new UserProfileEditModel();

            model.ProfileEdit = objUser.GetDetailsEdit(User.Identity.Name);
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(UserProfileEditModel model, FormCollection form)
        {
            string Message = "";
            if (TempData["message"] != null)
            {
                Message = (TempData["message"]).ToString(); ;
            }

            var image = form["upfiles"];
            if (image.StartsWith(",")) { image = image.Substring(1); }
            if (ModelState.IsValid)
            {

                //if (!string.IsNullOrEmpty(model.ProfileEdit.UserPhotoNormal) ||
                //    !string.IsNullOrEmpty(model.ProfileEdit.UserPhotoThumb))
                //{
                //    if ((model.ProfileEdit.UserPhotoNormal.StartsWith("https://")) || (model.ProfileEdit.UserPhotoThumb.StartsWith("https://")))
                //    {
                //        string img = "data:image/jpeg;base64," + ConvertImageURLToBase64(model.ProfileEdit.UserPhotoNormal);
                //        model.ProfileEdit.UserPhotoNormal = img;
                //        model.ProfileEdit.UserPhotoThumb = img;
                //    }

                //}

                if (!string.IsNullOrEmpty(image))
                {
                    if (model.ProfileEdit.UserPhotoNormal != null)
                    {
                        if (model.ProfileEdit.UserPhotoNormal.IndexOf("data:image") == -1)
                        {
                            string ExistingFilePath = Server.MapPath(model.ProfileEdit.UserPhotoNormal);
                            FileInfo ExistingFileInfo = new FileInfo(ExistingFilePath);
                            if (ExistingFileInfo.Exists)
                                ExistingFileInfo.Delete();
                        }
                    }
                    if (image == "PhotoDeleted")
                    {

                        model.ProfileEdit.UserPhotoNormal = "";
                        model.ProfileEdit.UserPhotoThumb = "";
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

                        model.ProfileEdit.UserPhotoNormal = fileName;
                        model.ProfileEdit.UserPhotoThumb = fileName;
                    }
                }




                TempData["ErrMsg"] = objUser.UpdateProfile(model.ProfileEdit);

                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "ControlPanel", new { area = "Admin" });
                    }
                    else if (User.IsInRole("Agent"))
                    {
                        return RedirectToAction("List", "DealAgent", new { area = "Coordinator" });

                    }
                    else
                        if (!string.IsNullOrEmpty(Message))
                    {
                        return RedirectToAction("EmailPreferences", "Account", new { Message = Message, area = "" });

                    }

                    //  return RedirectToAction("List", "MyDeal", new { area = "Client" });
                    return RedirectToAction("EditProfile", "Account", new { area = "" });

                }



            }

            return View(model);
        }
        #endregion

        public String ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();

            Byte[] _byte = this.GetImage(url);

            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return _sb.ToString();
        }
        private byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }

            return (buf);
        }

        #region ChangePassword
        public ActionResult ChangePassword(string Message = "")
        {
            //ViewBag.ActiveURL = "Account";
            ViewBag.ActiveURL = "ChangePassword";
            ViewBag.message = Message;
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            string Message = (TempData["message"]).ToString();
            if (!ModelState.IsValid)
            {
                ViewBag.message = Message;

                return View(model);
            }
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
            {
                RequireUniqueEmail = false
            };
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword.Trim(), model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                //if (Message.ToString() == "")
                //{
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "ControlPanel", new { area = "Admin" });
                    }
                    else if (User.IsInRole("Agent"))
                    {
                        return RedirectToAction("List", "DealAgent", new { area = "Coordinator" });

                    }
                    else
                    {
                    return RedirectToAction("List", "MyDeal", new { area = "Client" });

                }
                  

                //}

                //return RedirectToAction("EditProfile", new { Message = Message });
            }
            foreach (var error in result.Errors)
            {
                ViewBag.ValError = "error";
            }
            AddErrors(result);
            ViewBag.message = Message;

            return View(model);
        }

        public ActionResult PasswordChange(string Message = "")
        {
            ViewBag.message = Message;
            return View();
        }
        #endregion


        #region Forgotpasswordemail

        private string PopulateBody(string Name, string userName, string password)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/Email.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FirstName}", Name);
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Password}", password);
            body = body.Replace("{Url}", "http://justbere.com/Account/Login");
            return body;
        }

        private string ForgotPassword(string Name, string userName, string password)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/HtmlEmail/ForgotPassword.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FirstName}", Name);
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Password}", password);
            body = body.Replace("{Url}", "http://justbere.com/Account/Login");
            return body;
        }


        private int SendEmailConfirmationToken(string Name, string userID, string subject, string email, string pass)
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

            mm.To.Add(email);
            mm.Priority = MailPriority.High;

            mm.Subject = "Forgot Password- Just BE.";
            mm.Body = this.ForgotPassword(Name, email, pass);
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


        public ActionResult GoogleAuthorization(string code)
        {
            // Retrieve the authenticator and save it in session for future use
            var authenticator = GoogleAuthorizationHelper.GetAuthenticator(code);
            Session["authenticator"] = authenticator;

            // Save the refresh token locally
            using (var dbContext = new EFDBContext())
            {
                var userName = User.Identity.Name;
                var userRegistry = dbContext.utblMstGmailTokens.FirstOrDefault(c => c.UserEmail == userName);
                var GmailAcc = Session["Gmail"];//"robert86100@gmail.com"; //Session["Gmail"];
                if (userRegistry == null)
                {
                    dbContext.utblMstGmailTokens.Add(
                        new utblMstGmailToken()
                        {
                            GmailAccount = GmailAcc.ToString(),
                            UserEmail = userName,
                            RefreshToken = authenticator.RefreshToken

                        });
                }
                else
                {
                    userRegistry.GmailAccount = GmailAcc.ToString();
                    userRegistry.RefreshToken = authenticator.RefreshToken;
                }

                dbContext.SaveChanges();
                TempData["ErrMsg"] = "....Successfully linked google account...";
            }


            return RedirectToAction("Index", "Home");
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

        [HttpPost]
        public string UploadPicture(string User, string Image)
        {
            UserProfileEdit model = new UserProfileEdit();
            model.UserName = User;
            string returnImageName = "";
            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;

                for (int i = 0; i < files.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Image))
                    {
                        string ExistingFilePath = Server.MapPath(Image);
                        FileInfo ExistingFileInfo = new FileInfo(ExistingFilePath);
                        if (ExistingFileInfo.Exists)
                            ExistingFileInfo.Delete();
                    }

                    var guid = Guid.NewGuid().ToString().Substring(0, 6);
                    HttpPostedFileBase file = files[i];
                    string fileName = "/img/Users/" + guid + file.FileName;
                    string fname = Server.MapPath(fileName);
                    file.SaveAs(fname);
                    model.UserPhotoNormal = fileName;
                    model.UserPhotoThumb = fileName;
                    TempData["ErrMsg"] = objUser.UpdateProfileImage(model);
                    returnImageName = fileName;
                }
            }
            return returnImageName;
        }

       


    }
}