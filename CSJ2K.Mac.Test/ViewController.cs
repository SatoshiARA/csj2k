using System;

using AppKit;
using Foundation;
using CoreGraphics;

namespace CSJ2K.Mac.Test
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			// Do any additional setup after loading the view.
			using (var stream = typeof(ViewController).Assembly.GetManifestResourceStream("CSJ2K.Mac.Test.Files.file1.jp2"))
			{
				var uiImage = J2kImage.FromStream(stream).As<NSImage>();
				var imageView = new NSImageView(new CGRect(0, 0, uiImage.Size.Width, uiImage.Size.Height)) { Image = uiImage };
				this.View.AddSubview(imageView);
			}
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
