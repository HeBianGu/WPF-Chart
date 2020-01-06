using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeBianGu.General.DrawingBrush
{
    public class LinearBitmap : IBitmap
    {
        public WriteableBitmap Source { get; set; }

        public LinearBitmap(int width)
        {
            Source = new WriteableBitmap((int)width, Height, 96, 96, PixelFormats.Bgr32, null);

            this.Width = width;
        }

        public int Height { get; set; } = 1;

        public int Width { get; }

        public int Mosaic { get; set; } = 2;

        public List<LinearBrush> LinearBrushs { get; set; } = new List<LinearBrush>();

        public IBitmap Draw()
        {
            Int32Rect rect = new Int32Rect(0, 0, (int)Width, Height);

            byte[] pixels = new byte[(int)Width * Height * Source.Format.BitsPerPixel / 8];

            for (int y = 0; y < Source.PixelHeight; y++)
            {
                for (int x = 0; x < Source.PixelWidth; x++)
                {
                    int alpha = 0;
                    int red = 0;
                    int green = 0;
                    int blue = 0;

                    int weight = x - x % Mosaic;
                    int height = y - y % Mosaic;

                    //red = (int)((double)height / Source.PixelWidth * 255);
                    //green = rand.Next(100, 255);
                    //blue = (int)((double)weight / wb.PixelHeight * 255);
                    alpha = 50;

                    foreach (var item in this.LinearBrushs)
                    {
                        item.Draw(weight, height, ref red, ref green, ref blue);
                    }

                    int pixelOffset = (x + y * Source.PixelWidth) * Source.Format.BitsPerPixel / 8;

                    pixels[pixelOffset] = (byte)blue;
                    pixels[pixelOffset + 1] = (byte)green;
                    pixels[pixelOffset + 2] = (byte)red;
                    pixels[pixelOffset + 3] = (byte)alpha;

                }

                int stride = (Source.PixelWidth * Source.Format.BitsPerPixel) / 8;

                Source.WritePixels(rect, pixels, stride, 0);

            }

            return this;
        }

        public static LinearBitmap operator +(LinearBitmap b, LinearBitmap c)
        {
            int width = b.Width + c.Width;

            LinearBitmap result = new LinearBitmap(width);

            foreach (var item in b.LinearBrushs)
            {
                result.LinearBrushs.Add(item);
            }

            foreach (var item in c.LinearBrushs)
            {
                LinearBrush linearBrush = item.Clone();

                linearBrush.Start = linearBrush.Start + b.Width;
                linearBrush.End = linearBrush.End + b.Width;

                result.LinearBrushs.Add(linearBrush);
            }

            result.Draw();

            return result;
        }

        public static LinearBitmap operator -(LinearBitmap b, LinearBitmap c)
        {
            int width = b.Width - c.Width;

            LinearBitmap result = new LinearBitmap(width);

            var collection = b.LinearBrushs.Take(b.LinearBrushs.Count - c.LinearBrushs.Count);

            foreach (var item in collection)
            {
                result.LinearBrushs.Add(item);
            }

            result.Draw();

            return result;
        }

        public LinearBitmap RemoveLast()
        {
            var last = this.LinearBrushs.LastOrDefault();

            if (last == null || this.LinearBrushs.Count == 1) return null;

            int width = this.Width - last.End + last.Start;

            LinearBitmap result = new LinearBitmap(width);

            var collection = this.LinearBrushs.Take(this.LinearBrushs.Count - 1);

            foreach (var item in collection)
            {
                result.LinearBrushs.Add(item);
            }

            result.Draw();

            return result;

        }


        public void GetColor(int x, int y, ref int red, ref int green, ref int blue)
        {
            foreach (var item in this.LinearBrushs)
            {
                item.Draw(x, y, ref red, ref green, ref blue);
            }
        }
    }
}
