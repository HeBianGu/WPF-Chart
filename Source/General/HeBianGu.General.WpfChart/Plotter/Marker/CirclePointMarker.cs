#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/1/19 16:41:51 
 * 文件名：Class2 
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
using System.Windows.Media;

namespace HeBianGu.WPF.EChart
{

    /// <summary>圆环Marker</summary>
    public class CirclePointMarker : ShapePointMarker
    {

        /// <summary> 描绘形状 </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                EllipseGeometry e = new EllipseGeometry(ScreenPoint, this.Size / 2, this.Size / 2);
                return e;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Pen p = new Pen(this.Fill, 1);

            drawingContext.DrawGeometry(Brushes.White, p, DefiningGeometry);

            //drawingContext.DrawGeometry(this.Fill, this.Pen, DefiningGeometry);
        }
    }
}
