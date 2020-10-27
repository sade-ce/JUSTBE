using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize(Roles = "Agent,Admin")]
    public class MyGalleryController : Controller
    {
        UserGalleryModel objMediaModel = new UserGalleryModel();
        dalMstAgentPhotoGallery objMediaDal = new dalMstAgentPhotoGallery();
        public ActionResult Index()
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            var AgentID = User.Identity.GetUserId();
            objMediaModel.UserGalleryList = objMediaDal.GetUserGalleryList(AgentID);
            ViewBag.AgentID = AgentID;
            return View(objMediaModel);
        }



        #region media gallery
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection form, UserGalleryModel ItemData)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";
            var AgentID = User.Identity.GetUserId();
            objMediaModel.UserGalleryList = objMediaDal.GetUserGalleryList(AgentID);
            if (!ModelState.IsValid)
            {
                return View(objMediaModel);
            }
           
            var image = form["upfiles"];
            //var EditStatus = form["IsEdited"];
            if (!string.IsNullOrEmpty(image))
            {
                if (image == "PhotoDeleted")
                {

                    ItemData.UserGallery.Photo = "";
                }
                else
                {
                    dynamic data = JObject.Parse(image);
                    string imagename = data.output.name;
                    string imageext = imagename.Substring(imagename.LastIndexOf('.'));
                    string imagedata = data.output.image;
                    string imageType = imagedata.Substring(0, imagedata.IndexOf(',', imagedata.IndexOf(',')) + 1);
                    string convert = imagedata.Replace(imageType, String.Empty);


                    var guid = Guid.NewGuid().ToString().Substring(0, 10);


                    ItemData.UserGallery.Photo = guid + imageext;
                    TempData["ErrMsg"] = objMediaDal.SaveUserGallery(ItemData.UserGallery);
                    if ((TempData["ErrMsg"].ToString()) == "0")
                    {
                        TempData["ErrMsg"] = null;
                        byte[] image64 = Convert.FromBase64String(convert);
                        using (var ms = new MemoryStream(image64))
                        {

                            var images = System.Drawing.Image.FromStream(ms);
                            System.Drawing.Image img = images.GetThumbnailImage(Convert.ToInt32(data.output.width), Convert.ToInt32(data.output.height), null, IntPtr.Zero);
                            img.Save(Server.MapPath("/UploadedFiles/PhotoGallery/" + guid + imageext), System.Drawing.Imaging.ImageFormat.Jpeg);

                        }

                    }

                }


            }
            else
            {

                TempData["ErrMsg"] = objMediaDal.SaveUserGallery(ItemData.UserGallery);
                if ((TempData["ErrMsg"].ToString()) == "0")
                {
                    TempData["ErrMsg"] = null;

                }
            }
            return RedirectToAction("Index");
        }

    
        public ActionResult DeleteUserGallery(Guid UserGalleryId)
        {
            ViewBag.ActiveURL = "/Coordinator/Agent/Index";

            UserGalleryView objGallary = new UserGalleryView();
            objGallary = objMediaDal.GetUserGalleryByID(UserGalleryId);
            TempData["ErrMsg"] = objMediaDal.DeleteUserGallery(UserGalleryId);
            var path = string.Concat(Server.MapPath("~/UploadedFiles/PhotoGallery/" +  objGallary.Photo));
            if ((TempData["ErrMsg"].ToString()) == "0")
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                TempData["ErrMsg"] = null;
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}