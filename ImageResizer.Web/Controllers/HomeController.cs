using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageResizer.Web.Models;

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
            var images = new List<ImageModel>();
            const int width = 600;
            string path = Server.MapPath("~/Images/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if(file != null)
            {
                var bytes = file.ToByte();

                DateTime start = DateTime.Now;
                var imageResizer = new ImageResizer(bytes);
                imageResizer.Resize(width, ImageEncoding.Jpg100);
                string f = "Jpg100_w" + width + ".jpg";
                imageResizer.SaveToFile(Path.Combine(path, f));
                imageResizer.Dispose();
                images.Add(new ImageModel
                               {
                                   ParseTime = (DateTime.Now - start).Milliseconds,
                                   Size = new FileInfo(Path.Combine(path, f)).Length, 
                                   Url = f
                               });

                start = DateTime.Now;
                var imageResizer2 = new ImageResizer(bytes);
                imageResizer2.Resize(300, 300, true, ImageEncoding.Jpg90);
                f = "Jpg90_w300_h300.jpg";
                imageResizer2.SaveToFile(Path.Combine(path, f));
                imageResizer2.Dispose();
                images.Add(new ImageModel
                {
                    ParseTime = (DateTime.Now - start).Milliseconds,
                    Size = new FileInfo(Path.Combine(path, f)).Length,
                    Url = f
                });

                start = DateTime.Now;
                var imageResizer6 = new ImageResizer(bytes);
                imageResizer6.Resize(600, 300, true, ImageEncoding.Jpg90);
                f = "Jpg90_w600_h300.jpg";
                imageResizer6.SaveToFile(Path.Combine(path, f));
                imageResizer6.Dispose();
                images.Add(new ImageModel
                {
                    ParseTime = (DateTime.Now - start).Milliseconds,
                    Size = new FileInfo(Path.Combine(path, f)).Length,
                    Url = f
                });

                start = DateTime.Now;
                var imageResizer3 = new ImageResizer(bytes);
                imageResizer3.Resize(width, ImageEncoding.Jpg);
                f = "Jpg75_w" + width + ".jpg";
                imageResizer3.SaveToFile(Path.Combine(path, f));
                imageResizer3.Dispose();
                images.Add(new ImageModel
                {
                    ParseTime = (DateTime.Now - start).Milliseconds,
                    Size = new FileInfo(Path.Combine(path, f)).Length,
                    Url = f
                });

                start = DateTime.Now;
                var imageResizer4 = new ImageResizer(bytes);
                imageResizer4.Resize(width, ImageEncoding.Png);
                f = "Png_w" + width + ".png";
                imageResizer4.SaveToFile(Path.Combine(path, f));
                imageResizer4.Dispose();
                images.Add(new ImageModel
                {
                    ParseTime = (DateTime.Now - start).Milliseconds,
                    Size = new FileInfo(Path.Combine(path, f)).Length,
                    Url = f
                });

                start = DateTime.Now;
                var imageResizer5 = new ImageResizer(bytes);
                imageResizer5.Resize(width, ImageEncoding.Gif);
                f = "Gif_w" + width + ".gif";
                imageResizer5.SaveToFile(Path.Combine(path, f));
                imageResizer5.Dispose();
                images.Add(new ImageModel
                {
                    ParseTime = (DateTime.Now - start).Milliseconds,
                    Size = new FileInfo(Path.Combine(path, f)).Length,
                    Url = f
                });
            }

            ViewBag.Files = images;

            return View(images);
        }

        public ActionResult About()
        {
            return View();
        }

        public FileStreamResult Download()
        {
            var fullQualifiedPathToDll = Path.Combine(Server.MapPath("~/"), "bin/ImageResizer.dll");
            var fileInfo = new FileInfo(fullQualifiedPathToDll);
            var myFileStream = new FileStream(fullQualifiedPathToDll, FileMode.Open);
            return File(myFileStream, "application/octet-stream", fileInfo.Name);
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
