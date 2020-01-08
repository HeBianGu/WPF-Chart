using HeBianGu.General.DrawingBrush;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfDrawingBrushDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() => InitCollection()));
            this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() => InitPlanarGridBitmap()));

        }

        public ObservableCollection<LinearBitmap> LinearWriteableBitmaps
        {
            get { return (ObservableCollection<LinearBitmap>)GetValue(LinearWriteableBitmapsProperty); }
            set { SetValue(LinearWriteableBitmapsProperty, value); }
        }

        public static readonly DependencyProperty LinearWriteableBitmapsProperty =
            DependencyProperty.Register("LinearWriteableBitmaps", typeof(ObservableCollection<LinearBitmap>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<LinearBitmap>(), (d, e) =>
            {
                MainWindow control = d as MainWindow;

                if (control == null) return;

                ObservableCollection<LinearBitmap> config = e.NewValue as ObservableCollection<LinearBitmap>;

            }));

        public void InitCollection()
        {
            List<Color> colors = new List<Color>()
            {
                Color.FromArgb(255,255,0,0),
                 Color.FromArgb(255,0,255,0),
                  Color.FromArgb(255,0,0,255),
                   Color.FromArgb(255,0,0,0),
                    Color.FromArgb(255,0,255,255),
                     Color.FromArgb(255,255,255,0),
                      Color.FromArgb(255,255,0,255),
                       Color.FromArgb(255,255,255,255)
            };


            foreach (var item1 in colors)
            {
                foreach (var item2 in colors)
                {
                    LinearBitmap linearWriteableBitmap = new LinearBitmap(128 * 2);

                    {
                        LinearBrush linearBrush = new LinearBrush();

                        linearBrush.GradientStops.Add(new GradientStop(item1, 0.0));

                        linearBrush.GradientStops.Add(new GradientStop(item2, 1.0));

                        linearBrush.Start = 0;

                        linearBrush.End = 128 * 2;

                        linearWriteableBitmap.LinearBrushs.Add(linearBrush);
                    }

                    linearWriteableBitmap.Draw();

                    this.LinearWriteableBitmaps.Add(linearWriteableBitmap);
                }
            }
        }

        public void InitPlanarGridBitmap()
        {
            PlanarGridBitmap planar = new PlanarGridBitmap(1000, 1000);

            PlanarGridBrush brush = new PlanarGridBrush();

            double[,] values = new double[50, 50];

            Random random = new Random();

            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    values[i, j] = (20 - Math.Abs((i - 20))) * (20 - Math.Abs((j - 20)));
                }
            }

            brush.Values = values;

            brush.ItemHeigh = 25;
            brush.ItemWidth = 25;

            brush.MaxColor = Color.FromArgb(255, 255, 0, 0);
            brush.MinColor = Color.FromArgb(255, 255, 255, 0);
            brush.MaxValue = 20 * 20;
            brush.MinValue = 1;

            planar.Brushs.Add(brush);

            planar.Draw();

            PlanarGridBitmap = planar;

        }

        public PlanarGridBitmap PlanarGridBitmap
        {
            get { return (PlanarGridBitmap)GetValue(PlanarGridBitmapProperty); }
            set { SetValue(PlanarGridBitmapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlanarGridBitmapProperty =
            DependencyProperty.Register("PlanarGridBitmap", typeof(PlanarGridBitmap), typeof(MainWindow), new PropertyMetadata(default(PlanarGridBitmap), (d, e) =>
             {
                 MainWindow control = d as MainWindow;

                 if (control == null) return;

                 PlanarGridBitmap config = e.NewValue as PlanarGridBitmap;

             }));

    }
}
