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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfWriteableBitmap
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //writeableBmp = BitmapFactory.New(1920, 1080);

            writeableBmp = BitmapFactory.New(1200, 800);

            this.image.Source = writeableBmp;

            //writeableBmp.Clear(Colors.Black);

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        /// <summary> 绘制坐标系 </summary>
        void DrawCoordinate()
        {
            {
                Thickness margin = new Thickness(100);

                //  Do ：绘制X坐标
                int x0 = (int)margin.Left;
                int y0 = this.writeableBmp.PixelHeight / 2;

                int x1 = this.writeableBmp.PixelWidth - (int)margin.Right;
                int y1 = this.writeableBmp.PixelHeight / 2;

                writeableBmp.DrawLine(x0, y0, x1, y1, Colors.LightBlue);
            }


            {
                Thickness margin = new Thickness(100, 20, 200, 20);

                //  Do ：绘制Y坐标
                int x0 = (int)margin.Left;
                int y0 = (int)margin.Top;

                int x1 = (int)margin.Left;
                int y1 = this.writeableBmp.PixelHeight - (int)margin.Bottom;


                writeableBmp.DrawLine(x0, y0, x1, y1, Colors.Red);
            }
        }

        Thickness margin = new Thickness(80, 40, 80, 40);
        /// <summary> 绘制坐标系 </summary>
        void DrawCoordinateGrid()
        {
            {
                int span = 20;

                for (int i = 0; i < this.writeableBmp.PixelHeight; i++)
                {

                    if (i < (int)margin.Top) continue;
                    if (i > (this.writeableBmp.PixelHeight - (int)margin.Bottom)) continue;


                    if ((i - (int)margin.Top) % span == 0)
                    {
                        //  Do ：绘制X坐标
                        int x0 = (int)margin.Left;
                        int y0 = i;

                        int x1 = this.writeableBmp.PixelWidth - (int)margin.Right;
                        int y1 = i;

                        writeableBmp.DrawLine(x0, y0, x1, y1, Colors.LightGreen);
                    }

                }
            }

            {
                int span = 40;

                for (int i = 0; i < this.writeableBmp.PixelWidth; i++)
                {
                    if (i < (int)margin.Left) continue;
                    if (i > (this.writeableBmp.PixelWidth - (int)margin.Right)) continue;

                    if ((i - (int)margin.Left) % span == 0)
                    {
                        //  Do ：绘制Y坐标
                        int x0 = i;
                        int y0 = (int)margin.Top;

                        int x1 = i;
                        int y1 = this.writeableBmp.PixelHeight - (int)margin.Bottom;


                        writeableBmp.DrawLine(x0, y0, x1, y1, Colors.Red);
                    }
                }
            }
        }


        int index;
        double speed = 0.2;

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (index == 10000) index = 0;

            index++;

            w = index * speed;

            this.Draw();
        }

        WriteableBitmap writeableBmp = null;

        double w = 0;

        void Draw()
        {
            this.Dispatcher.Invoke(() =>
            {
                writeableBmp.Clear();
            });

            this.DrawCoordinateGrid();

            this.DrawCurve();

        }
     

        void DrawCurve()
        {
            List<int> source = new List<int>();

            int center = this.writeableBmp.PixelHeight / 2;

            int start = (int)margin.Left;

            int end = this.writeableBmp.PixelWidth - (int)margin.Right;

            for (int i = start; i < end; i++)
            {
                source.Add(i);

                int value = (int)(Math.Sin(i * 0.01 + w) * 100) + (int)(Math.Cos(i * 0.02 + w) * 200) + center;

                source.Add(value);
            }

            this.Dispatcher.Invoke(() =>
            {
                writeableBmp.DrawPolyline(source.ToArray(), Colors.White);
            });
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;

            this.speed = Convert.ToDouble(radio.Content.ToString());


        }
    }
}
