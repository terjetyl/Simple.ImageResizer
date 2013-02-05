Simple C# ImageResizer library using wpf classes

Available on NuGet:
PM> Install-Package Simple.ImageResizer

Usage:
<pre>
ImageResizer.ImageResizer resizer = new ImageResizer.ImageResizer(@"c:\path\to\image.jpg");

// resize to 400 px, jpg quality 90
var byteArray1 = resizer.Resize(400, ImageEncoding.Jpg90);

// resize to 400 px, height 200, ScaleToFill, png
var byteArray2 = resizer.Resize(400, 200, true, ImageEncoding.Png);

// resize to 400 px, height 200, ScaleToFit, gif
var byteArray3 = resizer.Resize(400, 200, false, ImageEncoding.Gif);

// save last resized image to file
resizer.SaveToFile(@"c:\path\to\image_resized.gif");
</pre>

Or
<pre>
public ActionResult Upload(HttpPostedFileBase file)
{
      var imageResizer = new ImageResizer.ImageResizer(file.InputStream.ToByteArray());
      imageResizer.Resize(800, ImageEncoding.Jpg100);
      imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/upload"), file.FileName));
      return View();
}
</pre>

Now also with a MvcExtensions package making it super easy to add dynamic image resizing into your asp.net mvc site.

Step 1:
PM> Install-Package Simple.ImageResizer.MvcExtensions

Step 2:
Add a ImagesController. Notice the ImageResult class which inherits FileResult and lets you add a custom width and height in the constructor. Setting both height and width to 0 will skip resizing of the image. Note: Your app will need write permission to the image folder specified as it caches resized images on disk in a subfolder.
<pre>
public class ImagesController : Controller{        [OutputCache(VaryByParam = "*", Duration = 60 * 60 * 24 * 365)]        public ImageResult Index(string filename, int w = 0, int h = 0)        {            string filepath = Path.Combine(Server.MapPath("~/images2"), filename);            return new ImageResult(filepath, w, h);        }}
</pre>

Step 3:
Add a custom route for images allowing requests to images to be handled by our controller action.
<pre>
routes.MapRoute(        "Image", "images/{filename}",        new { controller = "Images", action = "Index", filename = "" });
</pre>

Step 4:
Add the following to web.config in the system.webserver section to allow calls to static files to be handled by the mvc pipeline.
<pre>
&lt;modules runAllManagedModulesForAllRequests="true" /&gt;
</pre>

