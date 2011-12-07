namespace ImageResizer
{
    public class ImageOptions
    {
        private readonly int _width;
        private readonly ImageFormat _outPutImageFormat;
        private readonly int _height;
        
        public ImageOptions(int width, ImageFormat outPutImageFormat, int height = 0)
        {
            _width = width;
            _height = height;
            _outPutImageFormat = outPutImageFormat;
        }
        
        public int Width
        {
            get { return _width; }
        }

        public ImageFormat OutPutImageFormat
        {
            get { return _outPutImageFormat; }
        }
        
        public int Height
        {
            get { return _height; }
        }
    }
}
