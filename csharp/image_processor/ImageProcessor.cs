﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// new class ImageProcessor
/// </summary>
class ImageProcessor
{
    /// <summary>
    /// method to invert colors
    /// </summary>
    /// <param name="filenames"></param>
    public static void Inverse(string[] filenames)
    {

        Parallel.ForEach(filenames, (myFile) =>
        {
            var ext = Path.GetExtension(myFile);
            var fName = Path.GetFileNameWithoutExtension(myFile);
            fName += "_inverse" + ext;

            Bitmap bmp = new Bitmap(myFile);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < rgbValues.Length; i++)
                rgbValues[i] = (byte)(255 - rgbValues[i]);

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            bmp.UnlockBits(bmpData);

            bmp.Save($"{fName}");
        });
    }

    /// <summary>
    /// method to convert to grayscale
    /// </summary>
    /// <param name="filenames"></param>
    public static void Grayscale(string[] filenames)
    {
        Parallel.ForEach(filenames, (myFile) =>
        {
            var ext = Path.GetExtension(myFile);
            var fName = Path.GetFileNameWithoutExtension(myFile);
            fName += "_grayscale" + ext;

            Bitmap bmp = new Bitmap(myFile);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < rgbValues.Length - 3; i += 3)
            {
                byte gray = (byte)((rgbValues[i] * 0.21 + rgbValues[i + 1] * 0.71 + rgbValues[i + 2] * 0.071));
                rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = gray;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            bmp.UnlockBits(bmpData);

            bmp.Save($"{fName}");
        });
    }
    /// <summary>
    /// method to convert to black and white
    /// </summary>
    /// <param name="filenames"></param>

    public static void BlackWhite(string[] filenames, double threshold)
    {
        Parallel.ForEach(filenames, (myFile) =>
        {
            var ext = Path.GetExtension(myFile);
            var fName = Path.GetFileNameWithoutExtension(myFile);
            fName += "_bw" + ext;

            Bitmap bmp = new Bitmap(myFile);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < rgbValues.Length - 3; i += 3)
            {
                byte gray = (byte)((rgbValues[i] * 0.21 + rgbValues[i + 1] * 0.71 + rgbValues[i + 2] * 0.071));

                if (gray >= threshold)
                    rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = 255;
                else rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = 0;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            bmp.UnlockBits(bmpData);

            bmp.Save($"{fName}");
        });
    }

    public static void Thumbnail(string[] filenames, int height)
    {
        Parallel.ForEach(filenames, myFile =>
        {
            var ext = Path.GetExtension(myFile);
            var fName = Path.GetFileNameWithoutExtension(myFile);
            fName += "_bw" + ext;

            Bitmap bmp = new Bitmap(myFile);
            int width = height * bmp.Width / bmp.Height;
            Image img = bmp.GetThumbnailImage(width, height, null, IntPtr.Zero);
            bmp.Save($"{fName}");

        });
    }
}