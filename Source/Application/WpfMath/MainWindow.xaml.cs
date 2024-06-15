using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Window = System.Windows.Window;

namespace WpfMath
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += this.MainWindow_Loaded;

            //  Do ：曲线拟合 https://numerics.mathdotnet.com/Regression
            //double[] xdata = new double[] { 10, 20, 30 };
            //double[] ydata = new double[] { 15, 20, 25 };

            //Tuple<double, double> p = Fit.Line(xdata, ydata);
            //double a = p.Item1; // == 10; intercept
            //double b = p.Item2; // == 0.5; slope

            //  Do ：线性方程组求解 https://numerics.mathdotnet.com/LinearEquations
            var A = Matrix<double>.Build.DenseOfArray(new double[,] {
    { 3, 2, -1 },
    { 2, -2, 4 },
    { -1, 0.5, -1 }
});
            var b = Vector<double>.Build.Dense(new double[] { 1, -2, 0 });
            var x = A.Solve(b);
        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(500, 500, 96, 96, PixelFormats.Bgr32, null);
            this.im.Source = writeableBitmap;
            //Int32Rect rect = new Int32Rect(0, 0, (int)Width, Height);

            //using (writeableBitmap.GetBitmapContext())

            /// byte[] pixels = new byte[(int)Width * Height * Source.Format.BitsPerPixel / 8];
            /// 

            //  Do ：强度半径

            List<PointPower> pointPowers = new List<PointPower>();
            pointPowers.Add(new PointPower() { Point = new Point(250, 250), Power = 255 });
            pointPowers.Add(new PointPower() { Point = new Point(50, 30), Power = 100 });
            pointPowers.Add(new PointPower() { Point = new Point(300, 200), Power = 100 });
            pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 100 });


            byte rTop = 50;
            //double p1 = 255;
            //double p2 = 200;
            //double[] targetPoint1 = new double[] { 250, 250 };
            //double[] targetPoint2 = new double[] { 50, 30 };
            for (int y = 0; y < writeableBitmap.PixelHeight; y++)
            {
                for (int x = 0; x < writeableBitmap.PixelWidth; x++)
                {
                    //pixels[pixelOffset] = (byte)blue;
                    //pixels[pixelOffset + 1] = (byte)green;
                    //pixels[pixelOffset + 2] = (byte)red;
                    //pixels[pixelOffset + 3] = (byte)alpha;

                    double[] xy = new double[] { x, y };
                    var ds = pointPowers.Select(x =>
                    {
                        double d = Distance.Euclidean(xy, new double[] { x.Point.X, x.Point.Y });
                        d = 1 - d / x.Power;
                        d = d < 0.0 ? (byte)0 : (byte)(d * rTop);
                        return d;
                    });
                    //double d = Distance.SAD(xy, targetPoint);
                    //d = Distance.SSD(xy, targetPoint);
                    //d = Distance.MSE(xy, targetPoint);
                    //double d1 = Distance.Euclidean(xy, targetPoint1);
                    //double d2 = Distance.Euclidean(xy, targetPoint2);
                    //d = Distance.Minkowski(3, xy, targetPoint);
                    //d = Distance.Canberra(xy, targetPoint);
                    //d = Math.Sqrt(d);
                    //d1 = 1 - d1 / p1;
                    //d2 = 1 - d2 / p2;

                    //d1 = d1 < 0.0 ? (byte)0 : (byte)(d1 * 255);
                    //d2 = d2 < 0.0 ? (byte)0 : (byte)(d2 * 255);
                    //double d = (d1 + d2) / 2;
                    double d = ds.Sum();
                    byte alpha = 255;
                    byte red = (byte)d;
                    byte green = (byte)d;
                    byte blue = 0;

                    byte[] ColorData = { blue, 0, red, alpha }; // B G R

                    Int32Rect rect = new Int32Rect(x, y, 1, 1);
                    writeableBitmap.WritePixels(rect, ColorData, 4, 0);



                    //int weight = x;
                    //int height = y;

                    ////red = (int)((double)height / Source.PixelWidth * 255);
                    ////green = rand.Next(100, 255);
                    ////blue = (int)((double)weight / wb.PixelHeight * 255);
                    //alpha = 50;

                    //foreach (var item in this.Brushs)
                    //{
                    //    item.Draw(weight, height, ref red, ref green, ref blue);
                    //}

                    //int pixelOffset = (x + y * Source.PixelWidth) * Source.Format.BitsPerPixel / 8;

                    //pixels[pixelOffset] = (byte)blue;
                    //pixels[pixelOffset + 1] = (byte)green;
                    //pixels[pixelOffset + 2] = (byte)red;
                    //pixels[pixelOffset + 3] = (byte)alpha;

                }

                //int stride = (Source.PixelWidth * Source.Format.BitsPerPixel) / 8;

                //writeableBitmap.WritePixels(rect, pixels, stride, 0);

            }

        }
    }

    public class PointPower
    {
        public Point Point { get; set; }

        public double Power { get; set; }
    }
}
