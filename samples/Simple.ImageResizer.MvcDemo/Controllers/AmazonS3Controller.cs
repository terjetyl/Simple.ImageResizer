using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Simple.ImageResizer.MvcDemo.Properties;
using Simple.ImageResizer.MvcDemo.ViewModels;

namespace Simple.ImageResizer.MvcDemo.Controllers
{
    public class AmazonS3Controller : Controller
    {
        //
        // GET: /AmazonS3/
        public ActionResult Index(string filename = "Azalea.jpg")
        {
            var vm = new AmazonViewModel
            {
                Filename = filename
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            var imageBytes = ReadFully(file.InputStream);
            var s3Client = new S3Client();
            var resizeSettings = new ResizeSettings
            {
                Settings = new List<ResizeSetting>
                {
                    new ResizeSetting
                    {
                        Name = "Large",
                        Width = 700,
                        ImageEncoding = ImageEncoding.Jpg90
                    },
                    new ResizeSetting
                    {
                        Name = "Small",
                        Width = 300,
                        ImageEncoding = ImageEncoding.Jpg90
                    },
                    new ResizeSetting
                    {
                        Name = "Thumb",
                        Width = 100,
                        Height = 100,
                        ImageEncoding = ImageEncoding.Jpg
                    }
                }
            };

            Parallel.ForEach(resizeSettings.Settings, resizeSetting =>
            {
                var imageResizer = new ImageResizer(imageBytes);
            
                string fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + resizeSetting.Name + ".jpg";
                var resizedImage = resizeSetting.Height > 0
                    ? imageResizer.Resize(resizeSetting.Width, resizeSetting.Height, true, resizeSetting.ImageEncoding)
                    : imageResizer.Resize(resizeSetting.Width, resizeSetting.ImageEncoding);
                string result = s3Client.AddFile(fileName, new MemoryStream(resizedImage));
            });
            
            var vm = new AmazonViewModel
            {
                Filename = file.FileName,
                S3BaseUrl = Settings.Default.S3BaseUrl,
                CloudfrontBaseUrl = Settings.Default.CloudfrontBaseUrl,
                Bucket = Settings.Default.AWSPublicFilesBucket,
                ResizeSettings = resizeSettings
            };

            return View("Index", vm);
        }

        public static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
	}

    public class ResizeSetting
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ImageEncoding ImageEncoding { get; set; }

    }

    public class ResizeSettings
    {
        public List<ResizeSetting> Settings { get; set; }
    }

    public class S3Client
    {
        private readonly string _bucket = Settings.Default.AWSPublicFilesBucket;

        /// <summary>
        /// Create AmazonS3 from key in Application Config.
        /// </summary>
        private IAmazonS3 GetS3Client()
        {
            IAmazonS3 s3Client = Amazon.AWSClientFactory.CreateAmazonS3Client(
                    Settings.Default.AWSAccessKey,
                    Settings.Default.AWSSecretKey,
                    RegionEndpoint.EUWest1
                    );
            return s3Client;
        }

        public string AddFile(string filekey, Stream stream)
        {
            return AddFile(filekey, stream, _bucket);
        }

        public string AddFile(string filekey, Stream stream, string bucket)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucket,
                Key = filekey,
                InputStream = stream,
                CannedACL = S3CannedACL.PublicRead
            };

            using (var client = GetS3Client())
            {
                client.PutObject(request);
            }

            string presignedUrl = GetPreSignedUrl(filekey);

            return StripQueryString(presignedUrl);
        }

        public static string StripQueryString(string presignedUrl)
        {
            var uri = new Uri(presignedUrl);
            return uri.AbsoluteUri.Replace(uri.Query, "");
        }

        private string GetPreSignedUrl(string fileKey)
        {
            using (var client = GetS3Client())
            {
                return client.GetPreSignedURL(new GetPreSignedUrlRequest
                {
                    BucketName = _bucket,
                    Key = fileKey,
                    Expires = DateTime.UtcNow.AddDays(1)
                });
            }
        }

        public MemoryStream GetFile(string filekey)
        {
            using (var client = GetS3Client())
            {
                var file = new MemoryStream();
                GetObjectResponse r = client.GetObject(new GetObjectRequest
                {
                    BucketName = _bucket,
                    Key = filekey
                });
                var stream2 = new BufferedStream(r.ResponseStream);
                var buffer = new byte[0x2000];
                int count;
                while ((count = stream2.Read(buffer, 0, buffer.Length)) > 0)
                {
                    file.Write(buffer, 0, count);
                }
                return file;
            }
        }

        public DeleteObjectResponse DeleteFile(string filekey)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucket,
                Key = filekey
            };
            using (var client = GetS3Client())
            {
                return client.DeleteObject(request);
            }
        }
    }
}