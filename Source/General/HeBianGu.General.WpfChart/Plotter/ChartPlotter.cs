#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/1/18 11:28:41 
 * 文件名：ChartPlotter 
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

namespace HeBianGu.WPF.EChart
{

    /// <summary> 图表基类 </summary>
    public abstract class ChartPlotter : ViewPlotter
    {
        #region - 基础方法 -

        /// <summary> 获取值对应Canvas的位置 </summary>
        public double GetY(double value)
        {
            var bottom = this.ParallelCanvas.ActualHeight - ((value - this.MinValueY) / (this.MaxValueY - this.MinValueY)) * this.ParallelCanvas.ActualHeight;

            return bottom;
        }

        /// <summary> 获取值对应Canvas的位置 </summary>
        public double GetX(double value)
        {

            var bottom = ((value - this.MinValueX) / (this.MaxValueX - this.MinValueX)) * this.ParallelCanvas.ActualWidth;


            return bottom;
        }

        /// <summary> 获取值对应Canvas的位置应有的 Y 值 </summary>
        public double GetYValue(double value)
        {
            //var bottom = this.ParallelCanvas.ActualHeight - ((value - this.MinValueY) / (this.MaxValueY - this.MinValueY)) * this.ParallelCanvas.ActualHeight;

            var bottom = (((this.ParallelCanvas.ActualHeight - value) / this.ParallelCanvas.ActualHeight) * (this.MaxValueY - this.MinValueY)) + this.MinValueY;
            return bottom;
        }

        #endregion

        #region - 控制参数 -

        private double _maxValueY;
        /// <summary> 要显示的最大值 </summary>
        public double MaxValueY
        {
            get { return _maxValueY; }
            set { _maxValueY = value; }
        }


        private double _minValueY;
        /// <summary> 要显示的最小值 </summary>
        public double MinValueY
        {
            get { return _minValueY; }
            set { _minValueY = value; }
        }


        private double _maxValueX;
        /// <summary> 要显示的最大值 </summary>
        public double MaxValueX
        {
            get { return _maxValueX; }
            set { _maxValueX = value; }
        }

        private double _minValueX;
        /// <summary> 要显示的最小值 </summary>
        public double MinValueX
        {
            get { return _minValueX; }
            set { _minValueX = value; }
        }


        private List<SplitItem> _splitItemYs = new List<SplitItem>();
        /// <summary> Y范围分割线 </summary>
        public List<SplitItem> SplitItemYs
        {
            get { return _splitItemYs; }
            set { _splitItemYs = value; }
        }

        private List<SplitItem> _slpitItemXs = new List<SplitItem>();
        /// <summary> 说明 </summary>
        public List<SplitItem> SlpitItemXs
        {
            get { return _slpitItemXs; }
            set { _slpitItemXs = value; }
        }



        //public List<SplitItem> SlpitItemXs
        //{
        //    get { return (List<SplitItem>)GetValue(SlpitItemXsProperty); }
        //    set { SetValue(SlpitItemXsProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for SlpitItemXs.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SlpitItemXsProperty =
        //    DependencyProperty.Register("SlpitItemXs", typeof(List<SplitItem>), typeof(ChartPlotter), new PropertyMetadata(new List<SplitItem>()));



        #endregion

    }


}
