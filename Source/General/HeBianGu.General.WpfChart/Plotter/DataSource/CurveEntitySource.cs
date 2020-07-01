#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 四川*******公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[HeBianGu]   时间：2018/1/17 16:43:40 
 * 文件名：Class1 
 * 说明：
 * 
 * 
 * 修改者：           时间：               
 * 修改说明：
 * ========================================================================
*/
#endregion
using HeBianGu.WPF.EChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HeBianGu.WPF.EChart
{
    /// <summary>  普通曲线数据实体 </summary>
    public class CurveEntitySource : BaseEntitySource
    {
        private int _markTextVisbleLeight = 15;
        /// <summary> 设置要显示Mark文本的距离 防止过密显示 </summary>
        public int MarkTextVisbleLeight
        {
            get { return _markTextVisbleLeight; }
            set { _markTextVisbleLeight = value; }
        }

        /// <summary> 标识当前曲线是否显示向下动画 </summary>
        public bool IsAnimal { get; set; } = true;

        public override void Draw(CurveChartPlotter chart)
        {
            List<ShapePointMarker> es = new List<ShapePointMarker>();

            var ps = IsAnimal ? chart.TryFindResource("DefaultCurvePath") as Style : chart.TryFindResource("SingleCurvePath") as Style;
            var ms = chart.TryFindResource("DefaultMarker") as Style;

            var ts = chart.TryFindResource("XCenterLable") as Style;
            var d = chart.TryFindResource("XdashCapline") as Style;

            Path path = new Path();
            path.Style = ps;
            path.Stroke = this.Color;
            PolyLineSegment pls = new PolyLineSegment();

            List<Point> vs = new List<Point>();

            double minSpace = (chart.MaxValueX - chart.MinValueX) / chart.AutoXAxisCount;

            double param = 5;

            this.Elements.Clear();

            foreach (var item in this.Source)
            {
                // Todo ：增加线 
                Point ptem = new Point(chart.GetX(item.X), chart.GetY(item.Y));

                if (this.Marker != null && this.Marker.Visibility == Visibility.Visible)
                {
                    ShapePointMarker m = Activator.CreateInstance(this.Marker.GetType()) as ShapePointMarker;
                    m.ScreenPoint = ptem;
                    m.Fill = this.Marker.Fill;
                    m.Stroke = this.Marker.Stroke;
                    m.Text = Math.Round(item.Y, 1).ToString();
                    m.Style = ms;

                    // Todo ：如果周边有小于5的值不显示文本
                    var f = es.Find(k => Math.Abs(k.ScreenPoint.X - ptem.X) < _markTextVisbleLeight && Math.Abs(k.ScreenPoint.Y - ptem.Y) < _markTextVisbleLeight);

                    if (f != null && !string.IsNullOrEmpty(f.Text))
                    {
                        m.Text = string.Empty;
                    }
                    es.Add(m);
                }

                pls.Points.Add(ptem);

                // Todo ：增加虚线 

                // Todo ：存在在范围之内的不添加坐标 
                if (vs.Exists(k => Math.Abs(k.X - item.X) < minSpace)) continue;

                Line l = new Line();
                l.X1 = 0;
                l.X2 = 0;
                l.Y1 = 0;
                l.Y2 = param;
                l.Stroke = chart.Foreground;
                Canvas.SetLeft(l, chart.GetX(item.X));
                chart.BottomCanvas.Children.Add(l);

                this.Elements.Add(l);

                Label t = new Label();
                t.Content = item.Text;
                t.Style = ts;
                t.Foreground = chart.Foreground;
                t.FontSize = chart.FontSize - 3;
                Canvas.SetLeft(t, chart.GetX(item.X) - t.Width / 2);
                Canvas.SetTop(t, 2 * param - t.Height / 2);
                chart.BottomCanvas.Children.Add(t);

                this.Elements.Add(t);

                // Todo ：增加虚线 
                Line lx = new Line();
                lx.X1 = 0;
                lx.X2 = 0;
                lx.Y1 = chart.CenterBottomCanvas.ActualHeight - chart.GetY(item.Y);
                lx.Y2 = 0;
                lx.Width = 5;
                lx.Style = d;
                lx.Stroke = chart.Foreground;
                lx.StrokeThickness = 0.5;
                Canvas.SetLeft(lx, chart.GetX(item.X));
                Canvas.SetBottom(lx, 0);

                if (lx.Y1 > lx.Y2)
                {
                    chart.CenterBottomCanvas.Children.Add(lx);

                    this.Elements.Add(lx);
                }

                vs.Add(item.ToPoint());

            }

            PathFigure pf = new PathFigure();
            pf.StartPoint = pls.Points.FirstOrDefault();
            pf.Segments.Add(pls);
            PathGeometry pg = new PathGeometry(new List<PathFigure>() { pf });
            path.Data = pg;
            chart.PathCanvas.Children.Add(path);


            this.Elements.Add(path);

            // Todo ：绘制Marker 
            foreach (var item in es)
            {
                chart.ParallelCanvas.Children.Add(item);

                this.Elements.Add(item);
            }

        }


    }


}
