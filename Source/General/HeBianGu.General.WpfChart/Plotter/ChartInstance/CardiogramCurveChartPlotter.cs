#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 四川*******公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[HeBianGu]   时间：2018/1/19 15:41:51 
 * 文件名：CardiogramCurveChartPlotter 
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

    /// <summary> 心电图图表 </summary>
    public class CardiogramCurveChartPlotter : CurveChartPlotter
    {

        public CardiogramCurveChartPlotter()
        {
            //this.InitData();

            //this.RunDemo();

            this.Loaded += CardiogramCurveChartPlotter_Loaded;
        }

        private void CardiogramCurveChartPlotter_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private Brush _gridLineColor = Brushes.Red;
        /// <summary> 网格线颜色 </summary>
        public Brush GridLineColor
        {
            get { return _gridLineColor; }
            set { _gridLineColor = value; }
        }


        /// <summary> 此方法的说明 </summary>
        public void RunDemo()
        {
            //CardiogramCurveEntitySource source = new CardiogramCurveEntitySource();

            //source.Color = this.Foreground;

            //Random r = new Random();

            //// Todo ：初始化X网线 
            //for (double i = this.MinValueX; i < this.MaxValueX; i = i + 0.1)
            //{
            //    PointC p = new PointC();

            //    p.Y = r.Next(1800, 2500);
            //    p.X = i;

            //    source.Source.Add(p);

            //}

            //this.DataSource.Add(source);
        }

        private bool _isShowX;
        /// <summary> 说明 </summary>
        public bool IsShowX
        {
            get { return _isShowX; }
            set { _isShowX = value; }
        }

        private bool _isShowY;
        /// <summary> 说明 </summary>
        public bool IsShowY
        {
            get { return _isShowY; }
            set { _isShowY = value; }
        }

        private bool _isShowBaseLine;
        /// <summary> 说明 </summary>
        public bool IsShowBaseLine
        {
            get { return _isShowBaseLine; }
            set { _isShowBaseLine = value; }
        }



        /// <summary> 初始化心电图表 </summary>
        void InitData()
        {
            //this.FontSize = 12;
            this.MaxValueY = 2448;
            this.MinValueY = 1848;
            this.MaxValueX = 11;
            this.MinValueX = 0;
            //this.Height = 150;


            int tempIndex = 0;
            // Todo ：初始化X网线 
            for (double i = this.MinValueX; i <= this.MaxValueX; i = i + 0.25)
            {
                SplitItem s = new SplitItem();
                s.Value = i;
                s.Text = (i * 80).ToString();
                s.Color = this._gridLineColor;
                int param = (int)(tempIndex % 4);

                s.SpliteType = param == 0 && this.IsShowX ? SplitItemType.Normal : SplitItemType.InnerOnly;

                this.SlpitItemXs.Add(s);
                tempIndex++;
            }

            // Todo ：2048基准线 
            if (this.IsShowBaseLine)
            {
                SplitItem sbase = new SplitItem();
                sbase.Value = 2048;
                sbase.Text = (2048).ToString();
                sbase.Color = this.Foreground;
                sbase.SpliteType = SplitItemType.HeighLight;
                this.SplitItemYs.Add(sbase);
            }


            tempIndex = 0;
            // Todo ：初始化Y网线 
            for (double i = this.MinValueY; i <= this.MaxValueY; i = i + 30)
            {
                SplitItem s = new SplitItem();
                s.Value = i;
                s.Text = i.ToString();
                s.Color = this._gridLineColor;

                int param = (int)(tempIndex % 4);

                s.SpliteType = param == 0 && this.IsShowY ? SplitItemType.Normal : SplitItemType.InnerOnly;

                this.SplitItemYs.Add(s);

                tempIndex++;
            }
        }
    }
}
