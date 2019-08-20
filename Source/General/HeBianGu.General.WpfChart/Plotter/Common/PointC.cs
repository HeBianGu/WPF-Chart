#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 四川*******公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[HeBianGu]   时间：2018/1/18 12:37:03 
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

namespace HeBianGu.WPF.EChart
{

    /// <summary> 数据源点 </summary>
    public class PointC : DependencyObject
    {
        private double _x;
        /// <summary> 说明 </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        private double _y;
        /// <summary> 说明 </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private string _text;
        /// <summary> 说明 </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Point ToPoint()
        {
            return new Point(this.X, this.Y);
        }
    }
}
