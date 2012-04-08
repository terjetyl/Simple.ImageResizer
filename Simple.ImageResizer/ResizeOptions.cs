namespace Simple.ImageResizer
{
    public class ResizeOptions
    {
        private readonly string _name;
        private readonly int _width;
        private readonly int _height;
        private readonly bool _crop;

        public ResizeOptions(int width)
        {
            _width = width;
            _name = string.Empty;
            _crop = false;
            _height = 0;
        }

        public ResizeOptions(string name, int width)
        {
            _width = width;
            _name = name;
            _crop = false;
            _height = 0;
        }

        public ResizeOptions(int width, int height, bool crop)
        {
            _width = width;
            _name = string.Empty;
            _crop = crop;
            _height = height;
        }

        /// <summary>
        /// Use to create a predefined format
        /// </summary>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="crop"></param>
        public ResizeOptions(string name, int width, int height, bool crop)
        {
            _width = width;
            _name = name;
            _crop = crop;
            _height = height;
        }

        public string Name
        {
            get { return _name; }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public bool Crop
        {
            get { return _crop; }
        }
    }
}
