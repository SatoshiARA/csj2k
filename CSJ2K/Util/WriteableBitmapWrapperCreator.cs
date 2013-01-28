﻿using System.Composition;

namespace CSJ2K.Util
{
	[Export(typeof(IBitmapWrapperCreator))]
	public class WriteableBitmapWrapperCreator : IBitmapWrapperCreator
	{
		public WriteableBitmapWrapperCreator()
		{
			BitmapWrapperFactory.RegisterCreator(this);
		}

		public IBitmapWrapper Create(int width, int height, int numberOfComponents)
		{
			return new WriteableBitmapWrapper(width, height, numberOfComponents);
		}
	}
}