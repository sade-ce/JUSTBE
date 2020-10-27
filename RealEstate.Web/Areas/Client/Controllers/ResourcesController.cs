using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Client.Controllers
{
    [Authorize(Roles = "Client,Agent,Admin")]
    public class ResourcesController : Controller
    {
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
        ResourceLinksViewModel objViewModel = new ResourceLinksViewModel();
        CommentsViewModel objResourceComments = new CommentsViewModel();
        dalComments objDalComments = new dalComments();
        dalResources objDal = new dalResources();
        // GET: Client/Resources
        public ActionResult Index()
        {
            ViewBag.ActiveURL = "Concierge";
            return View();
        }

        public ActionResult ResourceType(int Id)
        {
            ViewBag.ActiveURL = "Concierge";
            objViewModel.ResourceTypeView = objDal.Getype(Id);
            objViewModel.ResourceLinksList = objDal.GetResourcesLinks(Id);
            objViewModel.ResourcesTitles = objDal.GetResourceTitles(Id);
            return View(objViewModel);
        }


        //    public ActionResult Comments(int Link,string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        //{
        //    int TotalRow;
        //    objResourceComments.CommentsList = objDalComments.CommentsGetPaged(Link,PageNo, PageSize, out TotalRow, SearchTerm);
        //    objResourceComments.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };

        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("pvComments", objResourceComments);
        //    }
        //    return View(objResourceComments);
        //}



        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult PostComment(CommentsViewModel Model)
        //{
        //    var CreatedBy = UserManager.FindByEmail(User.Identity.Name).Id;

        //    if (ModelState.IsValid)
        //    {

        //        TempData["ErrMsg"] = objDalComments.Save(Model.Comments, CreatedBy);
        //        if ((TempData["ErrMsg"].ToString()) == "0")
        //        {
        //            TempData["ErrMsg"] = null;
        //            //return RedirectToAction("list");
        //        }

        //    }
        //    int LinkId = Model.Comments.Link_Id;
        //    return RedirectToAction("Comments", new { Link=LinkId});
        //}


        //----------------------Client Deal Document -------------------------------------------
        public ActionResult Comments(int Link)
        {
            ViewBag.ActiveURL = "Concierge";
            ViewBag.LinkTitle = objDal.GeLinkTitle(Link);
            return View();
        }

        public JsonResult GetComments(int Link, string SearchTerm = "", int PageNo = 1, int PageSize = 10)
        {
            int TotalRow;
            objResourceComments.CommentsList = objDalComments.CommentsGetPaged(Link, PageNo, PageSize, out TotalRow, SearchTerm);
            objResourceComments.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRow };
            return Json(objResourceComments, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostComment(CommentsView Model)
        {
            string errorMessage = "";
            var CreatedBy = UserManager.FindByEmail(User.Identity.Name).Id;
           // Model.Description = System.Web.HttpUtility.HtmlDecode(Model.Description);

            errorMessage = objDalComments.Save(Model, CreatedBy);
           

            //int LinkId = Model.Comments.Link_Id;
            //return RedirectToAction("Comments", new { Link = LinkId });
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetCommentByID(string ID)
        {
            var Employee = objDalComments.GetCommentByID(Convert.ToInt32(ID));
            return Json(Employee, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditComment(CommentsView Comment)
        {
            string errorMessage = "";
            var UpdatedBy = UserManager.FindByEmail(User.Identity.Name).Id;
            errorMessage = objDalComments.Edit(Comment, UpdatedBy);
                
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteComment(string CommentId)
        {
            //string ExistingFilePath = Server.MapPath("~/UploadedFiles/TrackDeal/" + objClientDocDal.GetClientFileName(ID));
            //FileInfo file = new FileInfo(ExistingFilePath);
            string errorMessage = objDalComments.Delete(Convert.ToInt32(CommentId));
            //if (errorMessage == "0")
            //{
            //    if (file.Exists)
            //        file.Delete();
            //}

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PinComment(string CommentId, bool IsPin)
        {
            var PinedBy = UserManager.FindByEmail(User.Identity.Name).Id;
            string errorMessage = objDalComments.Pin(Convert.ToInt32(CommentId), PinedBy, IsPin);
            //if (errorMessage == "0")
            //{
            //    if (file.Exists)
            //        file.Delete();
            //}

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase file)
        {
            //var uploadsPath = HostingEnvironment.MapPath($"/uploads");
            //var uploadsDir = new DirectoryInfo(uploadsPath);
            //if (!uploadsDir.Exists)
            //    uploadsDir.Create();



            //var imageRelativePath = $"/Comments/{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.jpg";
            //var imageAbsPath = HostingEnvironment.MapPath(imageRelativePath);
            //var imageBytes = file.InputStream.ReadToEnd();
            //System.IO.File.WriteAllBytes(imageAbsPath, imageBytes);

            var guid = Guid.NewGuid().ToString().Substring(0, 6);
            string fileName = "/img/Comments/" + guid + file.FileName;
            string fname = Server.MapPath(fileName);
            file.SaveAs(fname);
            return Json(new { location = fileName });
        }
        [HttpPost]
        public JsonResult DeleteImage(string Source)
        {
            string errorMessage = "0";
            
            FileInfo file = new FileInfo(Server.MapPath("/img/Comments/"+Source));
            if (file.Exists)
                file.Delete();
            else
                errorMessage = "";

            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult ClientDocumentautocomplete(string term, string ClientId, string TransactionId)
        //{

        //    objCategoryModel.ClientDocumentAutoComplete = objCategoryDal.ClientDocumentAutoComplete(term, ClientId, TransactionId);
        //    var result = new List<KeyValuePair<string, string>>();
        //    foreach (var item in objCategoryModel.ClientDocumentAutoComplete)
        //    {
        //        result.Add(new KeyValuePair<string, string>(item.searchResult, item.Description));

        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);


        //}
        //-------------------------Client Deal Document----------------------------------------
    }


}