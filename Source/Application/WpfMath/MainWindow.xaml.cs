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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Window = System.Windows.Window;

namespace WpfMath
{
    public class ListCout : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.GetValues();
        }

        private IEnumerable<int> GetValues()
        {
            yield return 1;
            yield return 5;
            yield return 10;
            yield return 20;
            yield return 50;
            yield return 100;
            yield return 500;
            yield return 1000;
        }
    }

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


        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(MainWindow), new FrameworkPropertyMetadata(50, (d, e) =>
            {
                MainWindow control = d as MainWindow;

                if (control == null) return;

                if (e.OldValue is int o)
                {

                }

                if (e.NewValue is int n)
                {

                }

            }));


        public double SAD(double[] a, double[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException("All vectors must have the same dimensionality.");
            }

            double num = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                num += Math.Abs(a[i] - b[i]);
            }

            return num;
        }



        public void Build(Func<byte, byte[]> buildPixel, int a = 255, int count = 50, int w = 500, int h = 500)
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgra32, null);
            List<PointPower> pointPowers = new List<PointPower>();
            //pointPowers.Add(new PointPower() { Point = new Point(250, 250), Power = 25 });
            //pointPowers.Add(new PointPower() { Point = new Point(50, 30), Power = 15 });
            //pointPowers.Add(new PointPower() { Point = new Point(400, 200), Power = 10 });
            //pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 15 });
            //pointPowers.Add(new PointPower() { Point = new Point(0, 300), Power = 10 });
            //pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 10 });
            //pointPowers.Add(new PointPower() { Point = new Point(100, 300), Power = 10 });
            for (int i = 0; i < count; i++)
            {
                var pp = new PointPower();
                pp.Point = new Point(Random.Shared.Next(0, w), Random.Shared.Next(0, h));
                pp.Power = Random.Shared.Next(200);
                pointPowers.Add(pp);
            }

            Func<double, double, double> getDistance = (x, y) =>
            {
                double[] xy = new double[] { x, y };
                var ds = pointPowers.Select(x =>
                {
                    //double cd = Distance.SSD(xy, new double[] { x.Point.X, x.Point.Y });
                    //cd= Math.Sqrt(cd);

                    double cd = Distance.Euclidean(xy, new double[] { x.Point.X, x.Point.Y });
                    return Math.Max(x.Power - cd, 0);
                });
                return ds.Sum();
            };

            List<double> maps = new List<double>();
            for (int y = 0; y < writeableBitmap.PixelHeight; y++)
            {
                for (int x = 0; x < writeableBitmap.PixelWidth; x++)
                {
                    maps.Add(getDistance.Invoke(x, y));
                }
            }

            double min = maps.Min(x => x);
            double max = maps.Max(x => x);

            Func<double, byte> func = x =>
            {
                double p = (x - min) / (max - min);
                return (byte)(p * a);
            };

            for (int y = 0; y < writeableBitmap.PixelHeight; y++)
            {
                for (int x = 0; x < writeableBitmap.PixelWidth; x++)
                {
                    byte alpha = func.Invoke(getDistance.Invoke(x, y));
                    Int32Rect rect = new Int32Rect(x, y, 1, 1);
                    var bs = buildPixel?.Invoke(alpha);
                    writeableBitmap.WritePixels(rect, bs, 4, 0);
                }
            }
            //foreach (var item in pointPowers)
            //{
            //    writeableBitmap.WritePixels(new Int32Rect((int)item.Point.X, (int)item.Point.Y, 1, 1), new byte[] { 255, 255, 255, 255 }, 4, 0);
            //}

            this.im.Source = writeableBitmap;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Build(alpha =>
            {
                return new byte[] { 0, 0, 255, alpha };
            }, 255, this.Count);
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            this.Build(alpha =>
            {
                if (alpha > 200)
                {
                    return new byte[] { 0, 0, 255, alpha };
                }
                else if (alpha > 150)
                {
                    return new byte[] { 0, 255, 255, alpha };
                }
                else if (alpha > 100)
                {
                    return new byte[] { 0, 255, 0, alpha };
                }
                else if (alpha > 50)
                {
                    return new byte[] { 255, 255, 0, alpha };
                }
                else
                {
                    return new byte[] { 255, 0, 0, alpha };
                }
            }, 255, this.Count);
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            this.Build(alpha =>
            {
                if (alpha > 150)
                {
                    return new byte[] { 0, 0, 255, alpha };
                }
                else if (alpha > 100)
                {
                    return new byte[] { 0, 255, 255, alpha };
                }
                else if (alpha > 50)
                {
                    return new byte[] { 0, 255, 0, alpha };
                }
                else
                {
                    return new byte[] { 255, 0, 0, alpha };
                }
            }, 200, this.Count);
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            this.Build(alpha =>
            {
                if (alpha == 200)
                {
                    return new byte[] { 0, 0, 255, alpha };
                }
                else if (alpha == 150)
                {
                    return new byte[] { 0, 255, 255, alpha };
                }
                else if (alpha == 100)
                {
                    return new byte[] { 0, 255, 0, alpha };
                }
                else if (alpha == 50)
                {
                    return new byte[] { 255, 255, 0, alpha };
                }
                else
                {
                    return new byte[] { 255, 0, 0, alpha };
                }
            }, 255, this.Count);
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {

            this.Build(alpha =>
            {
                if (alpha % 50 == 0)
                    return new byte[] { 150, 150, 150, 255 };
                return new byte[] { 0, 0, 255, alpha };
            }, 255, this.Count);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            this.Build(alpha =>
            {
                var a = alpha * 5;
                if (a < 255)
                    return new byte[] { (byte)a, 0, 0, alpha };//蓝 B
                else if (a < 255 * 2)
                    return new byte[] { 255, (byte)(a - 255), 0, alpha };//青 B G
                else if (a < 255 * 3)
                    return new byte[] { (byte)(255 * 3 - a), 255, 0, alpha };//绿 G
                else if (a < 255 * 4)
                    return new byte[] { 0, 255, (byte)(a - 255 * 3), alpha };//黄  G R
                else if (a < 255 * 5)
                    return new byte[] { 0, (byte)(255 * 5 - a), 255, alpha };//红 R
                else
                    return new byte[] { 0, 0, 255, alpha };
            }, 255, this.Count);
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            this.Build(alpha =>
            {
                var a = alpha * 5;
                if (a < 255)
                    return new byte[] { (byte)a, 0, 0, (byte)(alpha + 30) };//蓝 B
                else if (a < 255 * 2)
                    return new byte[] { 255, (byte)(a - 255), 0, (byte)(alpha + 30) };//青 B G
                else if (a < 255 * 3)
                    return new byte[] { (byte)(255 * 3 - a), 255, 0, alpha };//绿 G
                else if (a < 255 * 4)
                    return new byte[] { 0, 255, (byte)(a - 255 * 3), alpha };//黄  G R
                else if (a < 255 * 5)
                    return new byte[] { 0, (byte)(255 * 5 - a), 255, alpha };//红 R
                else
                    return new byte[] { 0, 0, 255, 255 };
            }, 255, this.Count);
        }
    }

    public class PointPower
    {
        public Point Point { get; set; }

        public double Power { get; set; }
    }
}
