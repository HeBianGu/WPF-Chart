using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfWriteableBitmap
{
    /// <summary>
    /// WindowChange.xaml 的交互逻辑
    /// </summary>
    public partial class WindowChange : Window
    {
        WriteableBitmap writeableBmp = null;

        Random random = new Random();

        public WindowChange()
        {
            InitializeComponent();

            writeableBmp = BitmapFactory.New(1200, 800);

            this.image.Source = writeableBmp;

            this.InitSource();

            this.DrawCoordinateGrid();



            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(2000);

                    this.Dispatcher.Invoke(() =>
                    {
                        this.list.SelectedIndex = random.Next(this.list.Items.Count);
                    });
                }

            });
        }
        void InitSource()
        {
            int center = this.writeableBmp.PixelHeight / 2;

            {
                Fx fx = new Fx()
                {
                    Name = "Math.Cos(x * 0.05) * 100 + center",
                    F = x => (int)(Math.Cos(x * 0.05) * 100 + center)
                };

                this.list.Items.Add(fx);
            }

            {
                Fx fx = new Fx()
                {
                    Name = "x * 0.5",
                    F = x => (int)(x * 0.5)
                };

                this.list.Items.Add(fx);
            }

            {
                Fx fx = new Fx()
                {
                    Name = "Math.Sin(l * 0.01) * 100 + (int)(Math.Cos(l * 0.02) * 200) + center",
                    F = x => (int)(Math.Sin(x * 0.01) * 100 + Math.Cos(x * 0.02) * 200 + center)
                };

                this.list.Items.Add(fx);
            }

            {
                Fx fx = new Fx()
                {
                    Name = "Math.Sin(x * 0.1) * 100 + (int)(Math.Cos(x * 0.02) * 200) + (int)(Math.Cos(x * 0.2) * 200) + center",
                    F = x => (int)(Math.Sin(x * 0.1) * 100 + Math.Cos(x * 0.02) * 200 + Math.Cos(x * 0.2) * 200 + center)
                };

                this.list.Items.Add(fx);
            }
            {
                Fx fx = new Fx()
                {
                    Name = "Math.Sin(x * 0.1) * 100 + (int)(Math.Cos(x * 0.02) * 200) + (int)(Math.Cos(x * 0.2) * 200) + center",
                    F = x => (int)(Math.Sin(x * 0.5) * 100 + Math.Cos(x * 0.3) * 200 + center)
                };

                this.list.Items.Add(fx);
            }

            {

                List<int> rsource = new List<int>();

                for (int i = 0; i < this.writeableBmp.Width; i++)
                {
                    rsource.Add(random.Next((int)this.writeableBmp.Height));
                }
                Fx fx = new Fx()
                {
                    Name = "Random",
                    F = x => rsource[x]
                };

                this.list.Items.Add(fx);
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

                        writeableBmp.DrawLine(x0, y0, x1, y1, Colors.White);
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


                        writeableBmp.DrawLine(x0, y0, x1, y1, Colors.White);
                    }
                }
            }
        }

        double w = 0;

        void Draw(Func<int, int> fx)
        {
            this.Dispatcher.Invoke(() =>
            {
                writeableBmp.Clear();
            });

            this.DrawCoordinateGrid();

            this.DrawCurve(fx);

        }


        void DrawCurve(Func<int, int> fx)
        {
            List<int> source = new List<int>();

            int start = (int)margin.Left;

            int end = this.writeableBmp.PixelWidth - (int)margin.Right;

            for (int i = start; i < end; i++)
            {
                source.Add(i);

                int value = fx(i);

                source.Add(value);
            }

            this.Dispatcher.Invoke(() =>
            {
                writeableBmp.DrawPolyline(source.ToArray(), Colors.LightGreen);
            });
        }

        /// <summary> 从一个曲线变换到另一个曲线 </summary>
        void DrawAnimal(Func<int, int> from, Func<int, int> to, double frame = 50.0)
        {
            int center = this.writeableBmp.PixelHeight / 2;

            Task.Run(() =>
            {
                for (double i = 0; i < frame; i = i + 1)
                {
                    Func<int, int> convert = l =>
                    {
                        if (i == frame - 1)
                        {
                            return to(l);
                        }
                        else
                        {
                            return (int)(((to(l) - from(l)) / frame) * i + from(l));
                        }

                    };

                    this.Dispatcher.Invoke(() =>
                    {
                        this.Draw(convert);
                    });

                    Thread.Sleep(10);
                }
            });
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Fx pre = e.RemovedItems.Count > 0 ? e.RemovedItems[0] as Fx : null;

            Fx current = e.AddedItems.Count > 0 ? e.AddedItems[0] as Fx : null;

            if (pre == null && current != null)
            {
                this.Draw(current.F);
            }

            if (pre != null && current != null)
            {
                this.DrawAnimal(pre.F, current.F);
            }
        }
    }

    public class Fx
    {
        public string Name { get; set; }

        public Func<int, int> F { get; set; }
    }
}
