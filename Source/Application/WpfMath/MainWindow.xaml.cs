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
        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(500, 500, 96, 96, PixelFormats.Bgr32, null);
            List<PointPower> pointPowers = new List<PointPower>();
            pointPowers.Add(new PointPower() { Point = new Point(250, 250), Power = 255 });
            pointPowers.Add(new PointPower() { Point = new Point(50, 30), Power = 150 });
            //pointPowers.Add(new PointPower() { Point = new Point(400, 200), Power = 100 });
            //pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 150 });
            //pointPowers.Add(new PointPower() { Point = new Point(0, 300), Power = 100 });
            //pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 100 });
            //pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 100 });

            for (int i = 0; i < 3; i++)
            {
                var pp = new PointPower();
                pp.Point = new Point(Random.Shared.Next(0, 500), Random.Shared.Next(0, 500));
                pp.Power = Random.Shared.Next(255);
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
                    byte alpha = 255;
                    byte red = (byte)(d > 255 * 255 * 255 ? 255 : Math.Max(d - 255 * 255, 0));
                    byte blue = red > 0 ? (byte)0 : (byte)(d > 255 * 255 ? 255 : Math.Max(d - 255, 0));
                    byte green = red > 0 || blue > 0 ? (byte)0 : (byte)(d > 255 ? 255 : d - 255);
                    byte[] ColorData = { blue, green, red, alpha }; // B G R
                    Int32Rect rect = new Int32Rect(x, y, 1, 1);
                    writeableBitmap.WritePixels(rect, ColorData, 4, 0);
                }
            }

            foreach (var item in pointPowers)
            {
                writeableBitmap.WritePixels(new Int32Rect((int)item.Point.X, (int)item.Point.Y, 1, 1), new byte[] { 255, 255, 255, 255 }, 4, 0);
            }

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
