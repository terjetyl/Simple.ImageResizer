using System;
using System.Diagnostics.Contracts;

namespace ImageResizer
{
    public class ImageSizeCalculator
    {
        private readonly double _orgWidth;
        private readonly double _orgHeight;

        public ImageSizeCalculator(double orgWidth, double orgHeight)
        {
            _orgHeight = orgHeight;
            _orgWidth = orgWidth;
        }

        public ImageSize Scale(int newWidth)
        {
            if(newWidth <= 0)
                throw new ArgumentException(string.Format("Invalid new width: {0}", newWidth));

            if (newWidth > _orgWidth)
                throw new ArgumentException("Cannot scale up, newWidth is larger than orgWidth");
            
            double ratio = newWidth / _orgWidth;
            int newHeight = (int)(_orgHeight*ratio);
            return new ImageSize(newWidth, newHeight, 0, 0);
        }
        
        public ImageSize ScaleToFit(int newWidth, int newHeight)
        {
            if (newWidth <= 0)
                throw new ArgumentException(string.Format("Invalid new width: {0}", newWidth));
            if (newHeight <= 0)
                throw new ArgumentException(string.Format("Invalid new height: {0}", newHeight));

            if (newWidth > _orgWidth || newHeight > _orgHeight)
                throw new ArgumentException("Cannot scale up, new size is larger than org size");

            double widthRatio = newWidth / _orgWidth;
            double heightRatio = newHeight / _orgHeight;

            if(heightRatio > widthRatio)
                return new ImageSize(newWidth, (int)(_orgHeight * widthRatio), 0, 0);
            return new ImageSize((int)(_orgWidth * heightRatio), newHeight, 0, 0);
        }

        public ImageSize ScaleToFill(int newWidth, int newHeight)
        {
            if (newWidth <= 0)
                throw new ArgumentException(string.Format("Invalid new width: {0}", newWidth));
            if (newHeight <= 0)
                throw new ArgumentException(string.Format("Invalid new height: {0}", newHeight));

            if (newWidth > _orgWidth || newHeight > _orgHeight)
                throw new ArgumentException("Cannot scale up, new size is larger than org size");

            double widthRatio = newWidth / _orgWidth;
            double heightRatio = newHeight / _orgHeight;

            if (widthRatio > heightRatio)
                return new ImageSize(newWidth, newHeight, 0, ((int)(Math.Abs(_orgHeight - _orgWidth)) / 2));
            return new ImageSize(newWidth, newHeight, ((int)(Math.Abs(_orgWidth - _orgHeight)) / 2), 0);
        }
    }

    public class ImageSize
    {
        private readonly int _width;
        public int Width
        {
            get { return _width; }
        }

        private readonly int _height;
        public int Height
        {
            get { return _height; }
        }

        private readonly int _xOffset;
        public int XOffset
        {
            get { return _xOffset; }
        }

        private readonly int _yOffset;

        public ImageSize(int width, int height, int xOffset, int yOffset)
        {
            Contract.Requires(width >= 0);
            Contract.Requires(height >= 0);
            Contract.Requires(xOffset >= 0);
            Contract.Requires(yOffset >= 0);

            _width = width;
            _height = height;
            _xOffset = xOffset;
            _yOffset = yOffset;
        }

        public int YOffset
        {
            get { return _yOffset; }
        }
    }
}
