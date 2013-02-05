using System.IO;
using System.Web.Mvc;
using Simple.ImageResizer.MvcExtensions;

namespace Simple.ImageResizer.MvcDemo.Controllers
{
    public class ImagesController : Controller
    {
        [OutputCache(VaryByParam = "*", Duration = 60 * 60 * 24 * 365)]
        public ImageResult Index(string filename, int w = 0, int h = 0)
        {
            string filepath = Path.Combine(Server.MapPath("~/images2"), filename);
            return new ImageResult(filepath, w, h);
        }
    }
}