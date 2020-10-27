using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.CustomAttributes
{
    public class ImageCropperHelper
    {
        public string ConvertDatatURLToImage(string DataURL,string path)
        {
            //dynamic data = JObject.Parse(DataURL);
            //string imagename = data.output.name;
            //string imagedata = data.output.image;
            //string imageType = imagedata.Substring(0, imagedata.IndexOf(',', imagedata.IndexOf(',')) + 1);
            //string convert = imagedata.Replace(imageType, String.Empty);


            //var guid = Guid.NewGuid().ToString().Substring(0, 6);
            //string fileName = path + guid + imagename;

            //byte[] image64 = Convert.FromBase64String(convert);
            //using (var ms = new MemoryStream(image64))
            //{
            //    var images = System.Drawing.Image.FromStream(ms);
            //    System.Drawing.Image img = images.GetThumbnailImage(Convert.ToInt32(data.output.width), Convert.ToInt32(data.output.height), null, IntPtr.Zero);
            //    img.Save(Server.MapPath(fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
            //}
            return "";
        }
    }
}
