﻿// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;

    public abstract class ImageBase<TBase> : IImage
    {
        #region INNER TYPES

        protected enum ByteOrder
        {
            BGRA,

            RGBA
        }

        #endregion

        #region FIELDS

        protected const int SizeOfArgb = 4;

        protected readonly int Width;

        protected readonly int Height;

        protected readonly int NumberOfComponents;

        protected readonly byte[] Bytes;

        #endregion

        #region CONSTRUCTORS

        protected ImageBase(int width, int height, int numberOfComponents)
        {
            this.Width = width;
            this.Height = height;
            this.NumberOfComponents = numberOfComponents;
            this.Bytes = new byte[SizeOfArgb * width * height];
        }

        #endregion

        #region PROPERTIES

        protected virtual ByteOrder Order
        {
            get
            {
                return ByteOrder.BGRA;
            }
        }

        #endregion

        #region METHODS

        public virtual T As<T>()
        {
            if (!typeof(TBase).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidCastException(
                    string.Format(
                        "Cannot cast to '{0}'; type must be assignable from '{1}'",
                        typeof(T).Name,
                        typeof(TBase).Name));
            }

            return (T)this.GetImageObject();
        }

        public virtual void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues)
        {
            switch (this.NumberOfComponents)
            {
                case 1:
                case 3:
                    if (this.Order == ByteOrder.RGBA)
                    {
                        for (var k = 0; k < rowValues.Length; k += 3)
                        {
                            var temp = rowValues[k + 2];
                            rowValues[k + 2] = rowValues[k];
                            rowValues[k] = temp;
                        }
                    }
                    var i = SizeOfArgb * (rowIndex + lineIndex * rowWidth);
                    var j = 0;
                    for (var k = 0; k < rowWidth; ++k)
                    {
                        this.Bytes[i++] = rowValues[j++];
                        this.Bytes[i++] = rowValues[j++];
                        this.Bytes[i++] = rowValues[j++];
                        this.Bytes[i++] = 0xff;
                    }
                    break;
                case 4:
                    if (this.Order == ByteOrder.RGBA)
                    {
                        for (var k = 0; k < rowValues.Length; k += 3)
                        {
                            var temp = rowValues[k + 2];
                            rowValues[k + 2] = rowValues[k];
                            rowValues[k] = temp;
                        }
                    }
                    Array.Copy(
                        rowValues,
                        0,
                        this.Bytes,
                        SizeOfArgb * (rowIndex + lineIndex * rowWidth),
                        SizeOfArgb * rowWidth);
                    break;
                default:
                    throw new InvalidOperationException("Number of components must be one of 1, 3 or 4.");
            }
        }

        protected abstract object GetImageObject();

        #endregion
    }
}