#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/1/19 16:39:09 
 * 文件名：Class1 
 * 说明：
 * 
 * 
 * 修改者：           时间：               
 * 修改说明：
 * ========================================================================
*/
#endregion
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
    /// <summary> 心电曲线图数据源 </summary>
    public class CardiogramCurveEntitySource : BaseEntitySource
    {
        public override void Draw(CurveChartPlotter chart)
        {

            var ps = chart.FindResource("CardiogramDefaultPath") as Style;
            Path path = new Path();
            path.Style = ps;

            if (this.Color != null)
            {
                path.Stroke = this.Color;
            }

            PolyLineSegment pls = new PolyLineSegment();

            foreach (var item in this.Source)
            {
                // Todo ：增加线 
                Point ptem = new Point(chart.GetX(item.X), chart.GetY(item.Y));
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
        }

    }
}
