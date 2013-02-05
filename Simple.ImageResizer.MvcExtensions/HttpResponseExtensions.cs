using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Simple.ImageResizer.MvcExtensions
{
    public static class HttpResponseExtensions
    {
        public static void SetDefaultImageHeaders(this HttpResponseBase response, string fileName)
        {
            response.Cache.SetETag(CalculateMD5Hash(fileName));
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            response.Cache.SetLastModifiedFromFileDependencies();
        }

        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            var md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
