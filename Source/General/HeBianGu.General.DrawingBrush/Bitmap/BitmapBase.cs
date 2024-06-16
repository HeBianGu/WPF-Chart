using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeBianGu.General.DrawingBrush.Bitmap
{
   
    public class BitmapBase : IBitmap
    {
        public WriteableBitmap Source { get; set; }

        public BitmapBase(int width,int height)
        {
            Source = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);

            this.Width = width;

            this.Height = height;
        }

        public int Height { get; set; }

        public int Width { get; } 

        public List<IBrush> Brushs { get; set; } = new List<IBrush>();

        public IBitmap Draw()
        {
            Int32Rect rect = new Int32Rect(0, 0, (int)Width, Height);

            byte[] pixels = new byte[(long)Width * Height * Source.Format.BitsPerPixel / 8];

            for (int y = 0; y < Source.PixelHeight; y++)
            {
                for (int x = 0; x < Source.PixelWidth; x++)
                {
                    int alpha = 0;
                    int red = 0;
                    int green = 0;
                    int blue = 0;

                    int weight = x;
                    int height = y;

                    //red = (int)((double)height / Source.PixelWidth * 255);
                    //green = rand.Next(100, 255);
                    //blue = (int)((double)weight / wb.PixelHeight * 255);
                    alpha = 50;

                    foreach (var item in this.Brushs)
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

        public void GetColor(int x, int y, ref int red, ref int green, ref int blue)
        {
            foreach (var item in this.Brushs)
            {
                item.Draw(x, y, ref red, ref green, ref blue);
            }
        }
    }
}
