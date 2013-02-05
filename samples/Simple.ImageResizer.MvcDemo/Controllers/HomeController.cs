using System.IO;
using System.Web;
using System.Web.Mvc;
using Simple.ImageResizer.MvcDemo.ViewModels;

namespace Simple.ImageResizer.MvcDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string filename = "Azalea.jpg")
        {
            var vm = new IndexViewModel
                         {
                             ImageName = filename
                         };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string path = Server.MapPath("~/Images2/");
            file.SaveAs(Path.Combine(path, file.FileName));
            return RedirectToAction("Index", new { filename = file.FileName });
        }

        public FileStreamResult Download()
        {
            var fullQualifiedPathToDll = Path.Combine(Server.MapPath("~/"), "bin/Simple.ImageResizer.dll");
            var fileInfo = new FileInfo(fullQualifiedPathToDll);
            var myFileStream = new FileStream(fullQualifiedPathToDll, FileMode.Open);
            return File(myFileStream, "application/octet-stream", fileInfo.Name);
        }
    }
}
