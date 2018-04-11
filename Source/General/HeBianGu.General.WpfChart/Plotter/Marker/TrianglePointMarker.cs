#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/1/19 16:42:36 
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

namespace HeBianGu.WPF.EChart
{
      /// <summary>下三角Marker</summary>
    public class TrianglePointMarker : ShapePointMarker
    {

        /// <summary> 描绘形状 </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                double Size = 5;
                Point pt0 = Point.Add(ScreenPoint, new Vector(-Size / 2, -Size / 2));
                Point pt1 = Point.Add(ScreenPoint, new Vector(0, Size / 2));
                Point pt2 = Point.Add(ScreenPoint, new Vector(Size / 2, -Size / 2));

                StreamGeometry streamGeom = new StreamGeometry();
                using (var context = streamGeom.Open())
                {
                    context.BeginFigure(pt0, true, true);
                    context.LineTo(pt1, true, true);
                    context.LineTo(pt2, true, true);
                }
                return streamGeom;
            }
        }


        /// <summary> 绘制形状 </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawGeometry(Fill, Pen, DefiningGeometry);
        }
    }
}
