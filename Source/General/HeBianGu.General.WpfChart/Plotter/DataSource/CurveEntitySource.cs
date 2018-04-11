#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/1/17 16:43:40 
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

        public override void Draw(CurveChartPlotter chart)
        {
            List<ShapePointMarker> es = new List<ShapePointMarker>();
            
            var ps = chart.FindResource("DefaultCurvePath") as Style;
            var ms = chart.FindResource("DefaultMarker") as Style;
            
            Path path = new Path();
            path.Style = ps;
            path.Stroke = this.Color;
            PolyLineSegment pls = new PolyLineSegment();
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
                    var f = es.Find(l => Math.Abs(l.ScreenPoint.X - ptem.X) < _markTextVisbleLeight && Math.Abs(l.ScreenPoint.Y - ptem.Y) < _markTextVisbleLeight);

                    if (f != null && !string.IsNullOrEmpty(f.Text))
                    {
                        m.Text = string.Empty;
                    }
                    es.Add(m);
                }

                pls.Points.Add(ptem);
            }

            PathFigure pf = new PathFigure();
            pf.StartPoint = pls.Points.FirstOrDefault();
            pf.Segments.Add(pls);
            PathGeometry pg = new PathGeometry(new List<PathFigure>() { pf });
            path.Data = pg;
            chart.PathCanvas.Children.Add(path);

            this.Elements.Clear();
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
