using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageResizer.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(List<string> files = null)
        {
            ViewBag.Files = files;

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            var files = new List<string>();
            const int width = 600;
            string path = Server.MapPath("~/Images/"); 
            if(file != null)
            {
                var imageResizer = new ImageResizer(file.ToByte());
                imageResizer.Resize(width, ImageEncoding.Jpg100);
                string f = "Jpg100_w" + width + ".jpg";
                files.Add(f);
                imageResizer.SaveToFile(Path.Combine(path, f));

                imageResizer.Resize(width, ImageEncoding.Jpg);
                f = "Jpg75_w" + width + ".jpg";
                files.Add(f);
                imageResizer.SaveToFile(Path.Combine(path, f));

                imageResizer.Resize(width, ImageEncoding.Png);
                f = "Png_w" + width + ".png";
                files.Add(f);
                imageResizer.SaveToFile(Path.Combine(path, f));

                imageResizer.Resize(width, ImageEncoding.Gif);
                f = "Gif_w" + width + ".gif";
                files.Add(f);
                imageResizer.SaveToFile(Path.Combine(path, f));

                imageResizer.Resize(300, 300, true, ImageEncoding.Jpg90);
                f = "Jpg90_w300_h300.jpg";
                files.Add(f);
                imageResizer.SaveToFile(Path.Combine(path, f));
            }

            ViewBag.Files = files;

            return View(files);
        }

        public ActionResult About()
        {
            return View();
        }
    }

    public static class HttpPostedFileBaseExtensions
    {
        public static Byte[] ToByte(this HttpPostedFileBase value)
        {
            if (value == null)
                return null;

            var array = new Byte[value.ContentLength];
            value.InputStream.Position = 0;
            value.InputStream.Read(array, 0, value.ContentLength);
            return array;

        } // ToByte
    }
}
