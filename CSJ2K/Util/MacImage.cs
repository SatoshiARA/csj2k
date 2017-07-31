// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using CoreGraphics;

    using CoreImage;

    using AppKit;

    internal class MacImage : ImageBase<CGImage>
    {
        #region CONSTRUCTORS

        internal MacImage(int width, int height, byte[] bytes)
            : base(width, height, bytes)
        {
        }

        #endregion

        #region METHODS

        public override T As<T>()
        {
            if (typeof(T) == typeof(NSImage))
            {
                var cgImage = (CGImage)this.GetImageObject();
                return (T)(object)new NSImage(cgImage, new CGSize(cgImage.Width, cgImage.Height));
            }

            if (typeof(T) == typeof(CIImage))
            {
                return (T)(object)new CIImage((CGImage)this.GetImageObject());
            }

            return (T)this.GetImageObject();
        }

        protected override object GetImageObject()
        {
            var length = this.Bytes.Length;
            var bytes = new byte[length];

            // Switch byte representation from BGRA to RGBA
            for (var k = 0; k < length; k += 4)
            {
                bytes[k] = this.Bytes[k + 2];
                bytes[k + 1] = this.Bytes[k + 1];
                bytes[k + 2] = this.Bytes[k];
                bytes[k + 3] = this.Bytes[k + 3];
            }

            using (
                var context = new CGBitmapContext(
                    bytes,
                    this.Width,
                    this.Height,
                    8,
                    SizeOfArgb * this.Width,
                    CGColorSpace.CreateDeviceRGB(),
                    CGImageAlphaInfo.PremultipliedLast))
            {
                return context.ToImage();
            }
        }

        #endregion
    }
}
