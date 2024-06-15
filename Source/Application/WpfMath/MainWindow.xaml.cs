using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
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

            //for (int i = 0; i < 100; i++)
            //{
            //    //var pp = new PointPower();
            //    //pp.Point = new Point(Random.Shared.Next(0, 500), Random.Shared.Next(0, 500));
            //    //pp.Power = Random.Shared.Next(255);
            //    //pointPowers.Add(pp);
            //    var l = Random.Shared.Next(1000);
            //    Ellipse ellipse= new Ellipse();
            //    ellipse.Width= l;
            //    ellipse.Height = l;
            //    Canvas.SetLeft(ellipse, Random.Shared.Next(0, 500));
            //    Canvas.SetTop(ellipse, Random.Shared.Next(0, 500));
            //    this.CC.Children.Add(ellipse);
            //}
        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(500, 500, 96, 96, PixelFormats.Bgra32, null);
            List<PointPower> pointPowers = new List<PointPower>();
            //pointPowers.Add(new PointPower() { Point = new Point(250, 250), Power = 25 });
            //pointPowers.Add(new PointPower() { Point = new Point(50, 30), Power = 15 });
            //pointPowers.Add(new PointPower() { Point = new Point(400, 200), Power = 10 });
            //pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 15 });
            //pointPowers.Add(new PointPower() { Point = new Point(0, 300), Power = 10 });
            //pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 10 });
            //pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 10 });

            for (int i = 0; i < 100; i++)
            {
                var pp = new PointPower();
                pp.Point = new Point(Random.Shared.Next(0, 500), Random.Shared.Next(0, 500));
                pp.Power = Random.Shared.Next(120);
                pointPowers.Add(pp);
            }

            double totolValue = 1;
            for (int y = 0; y < writeableBitmap.PixelHeight; y++)
            {
                for (int x = 0; x < writeableBitmap.PixelWidth; x++)
                {
                    double[] xy = new double[] { x, y };
                    var ds = pointPowers.Select(x =>
                    {
                        double cd = Distance.Euclidean(xy, new double[] { x.Point.X, x.Point.Y });
                        //cd = Math.Sqrt(cd);
                        //d = 1 - d / x.Power;
                        //d = d < 0.0 ? (byte)0 : (byte)(d * rTop);
                        return Math.Max(x.Power - cd, 0);
                    });
                    double d = ds.Sum() / totolValue;
                    byte alpha = (byte)(d > 255 ? 255 : d - 255);
                    byte red = 0;
                    byte blue = 0;
                    byte green = 0;
                    if (d > 255 * 255 * 255)
                    {
                        red = 255;
                    }
                    else if (d > 255 * 255)
                    {
                        red = (byte)Math.Max(d - 255 * 255, 0);
                    }
                    else if (d > 255)
                    {
                        blue = (byte)Math.Max(d - 255 , 0);
                    }
                    else 
                    {
                        green = (byte)d;
                    }
                    //byte red = (byte)(d > 255 * 255 * 255 ? 255 : Math.Max(d - 255 * 255, 0));
                    //byte blue = red > 0 ? (byte)0 : (byte)(d > 255 * 255 ? 0 : Math.Max(d - 255, 0));
                    //byte green = red > 0 || blue > 0 ? (byte)0 : (byte)(d > 0 ? 255 : d);
                    byte[] ColorData = { 0, 255, 0, alpha }; // B G R
                    byte[] ColorData1 = { 0, 0, 255, 255 }; // B G R
                    byte[] ColorData2 = { 0, 0, 0, 255 }; // B G R

                    Int32Rect rect = new Int32Rect(x, y, 1, 1);

                    //if (alpha == 50)
                    //    writeableBitmap.WritePixels(rect, new byte[] { 255, 255, 0, 255 }, 4, 0);
                    //else if (alpha == 40)
                    //    writeableBitmap.WritePixels(rect, new byte[] { 255, 0, 255, 255 }, 4, 0);
                    //else if (alpha == 30)
                    //    writeableBitmap.WritePixels(rect, new byte[] { 0, 255, 255, 255 }, 4, 0);
                    //else if (alpha == 20)
                    //    writeableBitmap.WritePixels(rect, new byte[] { 255, 0, 0, 255 }, 4, 0);
                    //else if (alpha == 10)
                    //    writeableBitmap.WritePixels(rect, new byte[] { 0, 255, 0, 255 }, 4, 0);
                    if (alpha % 20 == 0)
                        writeableBitmap.WritePixels(rect, new byte[] { 100, 100, 100, 100 }, 4, 0);
                    else
                        writeableBitmap.WritePixels(rect, ColorData, 4, 0);
                }
            }

            //foreach (var item in pointPowers)
            //{
            //    writeableBitmap.WritePixels(new Int32Rect((int)item.Point.X, (int)item.Point.Y, 1, 1), new byte[] { 255, 255, 255, 255 }, 4, 0);
            //}

            this.im.Source = writeableBitmap;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class PointPower
    {
        public Point Point { get; set; }

        public double Power { get; set; }
    }
}
