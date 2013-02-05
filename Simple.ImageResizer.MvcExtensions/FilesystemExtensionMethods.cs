using System.IO;

namespace Simple.ImageResizer.MvcExtensions
{
    public static class FilesystemExtensionMethods
    {
        public static string FileExtensionForContentType(this string fileName)
        {
            var pieces = fileName.Split('.');
            var extension = pieces.Length > 1 ? pieces[pieces.Length - 1]
                : string.Empty;
            return (extension.ToLower() == "jpg") ? "jpeg" : extension;
        }

        public static string GetPathForResizedImage(this string orgPath, int width = 0, int height = 0)
        {
            var fileInfo = new FileInfo(orgPath);
            string resizedPath = Path.Combine(fileInfo.DirectoryName, "resized", width + "x" + height,
                                              Path.GetFileName(orgPath));
            return resizedPath;
        }
    }
}
