using System.Collections.Generic;
using System.IO;
using Simple.ImageResizer.MvcDemo.Controllers;

namespace Simple.ImageResizer.MvcDemo.ViewModels
{
    public class AmazonViewModel
    {
        public AmazonViewModel()
        {
            ResizeSettings = new ResizeSettings
            {
                Settings = new List<ResizeSetting>()
            };
        }

        public string Filename { get; set; }
        public string S3BaseUrl { get; set; }
        public string CloudfrontBaseUrl { get; set; }
        public string Bucket { get; set; }
        public ResizeSettings ResizeSettings { get; set; }

        public string GetS3Url(ResizeSetting resizeSetting)
        {
            string fileName = Path.GetFileNameWithoutExtension(Filename) + "_" + resizeSetting.Name + ".jpg";
            return Path.Combine(S3BaseUrl, Bucket, fileName);
        }

        public string GetCloudfrontUrl(ResizeSetting resizeSetting)
        {
            string fileName = Path.GetFileNameWithoutExtension(Filename) + "_" + resizeSetting.Name + ".jpg";
            return Path.Combine(CloudfrontBaseUrl, fileName);
        }
    }
}