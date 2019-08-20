#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 四川*******公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[HeBianGu]   时间：2018/1/18 11:29:45 
 * 文件名：CurveChartPlotter 
 * 说明：
 * 
 * 
 * 修改者：           时间：               
 * 修改说明：
 * ========================================================================
*/
#endregion
using HeBianGu.General.WpfChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HeBianGu.WPF.EChart
{

    /// <summary> 曲线图图表 </summary>
    public abstract class CurveChartPlotter : ChartPlotter
    {

        public CurveChartPlotter()
        {
            this.Loaded += CurveChartPlotter_Loaded;
        }

        private void CurveChartPlotter_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshCurve();
        }


        public void Clear()
        {
            this.CenterBottomCanvas.Children.Clear();

            this.LeftCanvas.Children.Clear();

            this.RightCanvas.Children.Clear();

            this.BottomCanvas.Children.Clear();

            this.TopCanvas.Children.Clear();

            this.ParallelCanvas.Children.Clear();
            

            this.PathCanvas.Children.Clear();
        }
        /// <summary> 刷新数据 </summary>
        void RefreshCurve()
        {
            this.Clear();

            this.RefreshXYZoom();

            this.RefreshSplitItemY();

            this.RefreshSplitItemX();

            this.RefreshDataSource();

            this.RefreshLegend();

            this.RefreshCursorGraph();

            // Todo ：注册刷新事件 
            foreach (var item in this.DataSource)
            {
                item.VisibleChanged += () =>
                {
                    if (_graphLine != null)
                    {
                        _graphLine.Refresh();

                        //this.RefreshXYZoom();
                    }
                };
            }
        }

        #region - 元素绘制 -

        /// <summary> 刷新标尺线 </summary>
        void RefreshSplitItemY()
        {
            var t = this.TryFindResource("line") as Style;

            var d = this.TryFindResource("dashCapline") as Style;

            var a = this.TryFindResource("pathTrangle") as Style;

            //var rs = this.TryFindResource("MarkBoardRectangle") as Style;

            var color = this.TryFindResource("MaxLineColor") as Brush;


            // Todo ：绘制普通网格线 
            var ns = this.SplitItemYs.FindAll(l => l.SpliteType == SplitItemType.Normal || l.SpliteType == SplitItemType.InnerOnly);

            foreach (var item in ns)
            {
                Line l = new Line();
                l.X1 = 0;
                l.Y1 = 0;
                l.Y2 = 0;
                l.Height = 5;
                if (item.Color != null) l.Stroke = item.Color;
                l.X2 = ParallelCanvas.ActualWidth;
                l.Style = this.InnerVerticalLineStyle == null ? t : this.InnerVerticalLineStyle;
                l.Style = item.LineStyle == null ? l.Style : item.LineStyle;
                Canvas.SetTop(l, this.GetY(item.Value));
                this.ParallelCanvas.Children.Add(l);
            }

            // Todo ：绘制范围网格线 
            var hs = this.SplitItemYs.FindAll(l => l.SpliteType == SplitItemType.HeighLight);

            foreach (var item in hs)
            {
                Line l = new Line();
                l.X1 = 0;
                l.Y1 = 0;
                l.Y2 = 0;
                l.Height = 5;
                if (item.Color != null) l.Stroke = item.Color;
                l.Style = item.LineStyle == null ? l.Style : item.LineStyle;

                l.X2 = ParallelCanvas.ActualWidth + ParallelCanvas.ActualWidth / 100;
                l.Style = d;
                Canvas.SetTop(l, this.GetY(item.Value));

                if (!this.IsShowTrangle) continue;

                // Todo ：绘制三角箭头 
                Path p = new Path();
                p.Width = 20;
                p.Height = 6;
                p.Style = a;
                p.Fill = item.Color;
                p.Stretch = Stretch.Fill;
                if (item.Color != null) l.Stroke = item.Color;
                Canvas.SetTop(p, this.GetY(item.Value) - p.Height / 2);
                Canvas.SetLeft(p, l.X2 - ParallelCanvas.ActualWidth);

                // Todo ：绘制文本 
                TextBlock text = new TextBlock();
                text.Text = item.Text;
                text.FontSize = this.FontSize;
                text.Foreground = item.Color;
                Canvas.SetLeft(text, l.X2 - ParallelCanvas.ActualWidth);

                if (item.IsShowTrangle)
                {
                    Canvas.SetTop(text, this.GetY(item.Value) + p.Height / 2);
                }
                else
                {
                    Canvas.SetTop(text, this.GetY(item.Value) - this.FontSize / 2);
                }

                Canvas.SetLeft(l, -ParallelCanvas.ActualWidth);

                // Todo ：不隐藏 
                if (item.IsShowTrangle)
                {
                    this.RightCanvas.Children.Add(p);
                    this.RightCanvas.Children.Add(l);
                }

                if (item.IsShowText)
                    this.RightCanvas.Children.Add(text);
            }

            if (hs.Count < 2) return;

            // Todo ：增加最大最小蒙版 
            //_maxMinRectVisible = true;
            if (_maxMinRectVisible)
            {
                if (!hs.Exists(l => (l.Value - this.MinValueY) < double.Epsilon))
                {
                    SplitItem sMin = new SplitItem();
                    sMin.Color = color;
                    sMin.Value = this.MinValueY;
                    hs.Add(sMin);
                }

                if (!hs.Exists(l => (this.MaxValueY - l.Value) < double.Epsilon))
                {
                    SplitItem sMax = new SplitItem();
                    sMax.Color = color;
                    sMax.Value = this.MaxValueY;
                    hs.Add(sMax);
                }
            }

            hs = hs.OrderByDescending(l => l.Value).ToList();

            var group = hs.GroupBy(l => l.Group);

            foreach (var item in group)
            {
                var collection = item.ToList();

                // Todo ：绘制蒙版 
                for (int i = 0; i < item.ToList().Count; i++)
                {
                    if (i == 0) continue;

                    Rectangle r = new Rectangle();
                    r.Width = this.ParallelCanvas.ActualWidth;
                    var height = this.GetY(collection[i].Value) - this.GetY(collection[i - 1].Value);
                    if (height < 0) continue;
                    r.Height = height;
                    r.Fill = Brushes.Orange;
                    r.Opacity = this.CoverOpacity;
                    //r.Style = rs;

                    Color color1 = (Color)ColorConverter.ConvertFromString(collection[i - 1].Color.ToString());
                    Color color2 = (Color)ColorConverter.ConvertFromString(collection[i].Color.ToString());

                    LinearGradientBrush brush = new LinearGradientBrush(color1, color2, new Point(0, 0), new Point(0, 1));
                    r.Fill = brush;

                    Canvas.SetTop(r, this.GetY(collection[i - 1].Value));

                    this.ParallelCanvas.Children.Add(r);
                }
            }

        }

        /// <summary> 刷新标尺线 </summary>
        void RefreshSplitItemX()
        {
            var t = this.TryFindResource("line") as Style;

            var d = this.TryFindResource("dashCapline") as Style;

            var a = this.TryFindResource("pathTrangle") as Style;

            var rs = this.TryFindResource("MarkBoardRectangle") as Style;

            var color = this.TryFindResource("MaxLineColor") as Brush;


            // Todo ：绘制普通网格线 
            var ns = this.SlpitItemXs.FindAll(l => l.SpliteType == SplitItemType.Normal || l.SpliteType == SplitItemType.InnerOnly);

            foreach (var item in ns)
            {
                Line l = new Line();
                l.X1 = 0;
                l.Y1 = 0;
                l.Y2 = ParallelCanvas.ActualHeight;
                l.Stroke = item.Color;
                l.X2 = 0;
                l.Width = 5;
                l.Style = this.InnerHorizontalLineStyle == null ? l.Style = t : this.InnerHorizontalLineStyle;
                l.Style = item.LineStyle == null ? l.Style : item.LineStyle;

                Canvas.SetLeft(l, this.GetX(item.Value));
                this.ParallelCanvas.Children.Add(l);
            }
            return;
        }

        private bool _maxMinRectVisible = false;
        /// <summary> 说明 </summary>
        public bool MaxMinRectVisible
        {
            get { return _maxMinRectVisible; }
            set { _maxMinRectVisible = value; }
        }
        /// <summary> 刷新坐标轴 </summary>
        void RefreshXYZoom()
        {
            this.BottomCanvas.Children.Clear();

            this.LeftCanvas.Children.Clear();

            double param = 5;

            var ts = this.TryFindResource("XCenterLable") as Style;
            var tsright = this.TryFindResource("YRightLable") as Style;
            //var d = this.TryFindResource("dashCapline") as Style;


            // Todo ：绘制轮廓 
            Line xleft = new Line();
            xleft.X1 = 0;
            xleft.X2 = 0;
            xleft.Y1 = 0;
            xleft.Y2 = this.ParallelCanvas.ActualHeight;
            xleft.Stroke = this.Foreground;
            xleft.StrokeThickness = 1;
            Canvas.SetLeft(xleft, 0);

            Line xright = new Line();
            xright.X1 = 0;
            xright.X2 = 0;
            xright.Y1 = 0;
            xright.Y2 = this.ParallelCanvas.ActualHeight;
            xright.Stroke = this.Foreground;
            xright.StrokeThickness = 1;
            Canvas.SetRight(xright, 0);

            Line yleft = new Line();
            yleft.X1 = 0;
            yleft.X2 = this.ParallelCanvas.ActualWidth;
            yleft.Y1 = 0;
            yleft.Y2 = 0;
            yleft.Stroke = this.Foreground;
            yleft.StrokeThickness = 1;
            Canvas.SetTop(yleft, 0);

            Line yright = new Line();
            yright.X1 = 0;
            yright.X2 = this.ParallelCanvas.ActualWidth;
            yright.Y1 = 0;
            yright.Y2 = 0;
            yright.Stroke = this.Foreground;
            yright.StrokeThickness = 1;
            Canvas.SetBottom(yright, 0);


            this.ParallelCanvas.Children.Add(xleft);
            this.ParallelCanvas.Children.Add(xright);
            this.ParallelCanvas.Children.Add(yleft);
            this.ParallelCanvas.Children.Add(yright);


            // Todo ：绘制X坐标轴 
            if (!this.IsAutoXAxis)
            {
                //X坐标
                foreach (var item in this.SlpitItemXs)
                {
                    if (item.SpliteType == SplitItemType.InnerOnly) continue;

                    Line l = new Line();
                    l.X1 = 0;
                    l.X2 = 0;
                    l.Y1 = 0;
                    l.Y2 = param;
                    l.Stroke = this.Foreground;
                    Canvas.SetLeft(l, this.GetX(item.Value));
                    this.BottomCanvas.Children.Add(l);

                    Label t = new Label();
                    t.Content = item.Text;
                    t.Style = ts;
                    t.Foreground = this.Foreground;
                    t.FontSize = this.FontSize;

                    Canvas.SetLeft(t, this.GetX(item.Value) - t.Width / 2);
                    Canvas.SetTop(t, 2 * param - t.Height / 2);
                    this.BottomCanvas.Children.Add(t);
                }
            }
            else
            {
                //List<Point> vs = new List<Point>();

                //double minSpace = (this.MaxValueX - this.MinValueX) / AutoXAxisCount;

                //// Todo ：按照实际值画坐标 
                //foreach (var item in this.DataSource)
                //{
                //    if (item.Visibility != Visibility.Visible) continue;

                //    foreach (var c in item.Source)
                //    {
                //        // Todo ：存在在范围之内的不添加坐标 
                //        if (vs.Exists(k => Math.Abs(k.X - c.X) < minSpace)) continue;

                //        Line l = new Line();
                //        l.X1 = 0;
                //        l.X2 = 0;
                //        l.Y1 = 0;
                //        l.Y2 = param;
                //        l.Stroke = this.Foreground;
                //        Canvas.SetLeft(l, this.GetX(c.X));
                //        this.BottomCanvas.Children.Add(l);

                //        Label t = new Label();
                //        t.Content = c.Text;
                //        t.Style = ts;
                //        t.FontSize = this.FontSize - 3;
                //        Canvas.SetLeft(t, this.GetX(c.X) - t.Width / 2);
                //        Canvas.SetTop(t, 2 * param - t.Height / 2);
                //        this.BottomCanvas.Children.Add(t);

                //        // Todo ：增加虚线 
                //        Line lx = new Line();
                //        lx.X1 = 0;
                //        lx.X2 = 0;
                //        lx.Y1 = this.ParallelCanvas.ActualHeight - this.GetY(c.Y);
                //        lx.Y2 = 0;
                //        lx.Style = d;
                //        lx.Stroke = this.Foreground;
                //        lx.StrokeThickness = 0.5;
                //        Canvas.SetLeft(lx, this.GetX(c.X));
                //        Canvas.SetBottom(lx, 0);

                //        if (lx.Y1 > lx.Y2)
                //        {
                //            this.ParallelCanvas.Children.Add(lx);

                //            item.ValueLines.Add(lx);
                //        }


                //        vs.Add(c.ToPoint());
                //    }
                //}
            }

            //Y坐标
            foreach (var item in this.SplitItemYs)
            {

                if (item.SpliteType == SplitItemType.InnerOnly) continue;

                if (item.SpliteType == SplitItemType.Normal)
                {
                    // Todo ：绘制线 
                    Line l = new Line();
                    l.X1 = 0;
                    l.X2 = param;
                    l.Y1 = 0;
                    l.Y2 = 0;
                    l.Stroke = this.Foreground;
                    Canvas.SetTop(l, this.GetY(item.Value));
                    Canvas.SetRight(l, 0);
                    this.LeftCanvas.Children.Add(l);

                    // Todo ：绘制文本 
                    Label t = new Label();
                    t.Content = item.Text;
                    t.FontSize = this.FontSize;
                    t.Foreground = this.Foreground;
                    t.Style = tsright;
                    Canvas.SetTop(t, this.GetY(item.Value) - t.Height / 2);
                    Canvas.SetRight(t, param);
                    this.LeftCanvas.Children.Add(t);
                }
                else
                {
                    // Todo ：绘制图标 
                    Ellipse e = new Ellipse();
                    e.Fill = Brushes.White;
                    e.Stroke = item.Color;
                    e.Height = param;
                    e.Width = param;
                    Canvas.SetTop(e, this.GetY(item.Value) - param / 2);
                    Canvas.SetRight(e, -param / 2);
                    this.LeftCanvas.Children.Add(e);
                }

            }
        }

        /// <summary> 绘制图例 </summary>
        void BindLegend(ICurveEntitySource item)
        {
            LineLegendItem l = new LineLegendItem(item);
            Style s = this.TryFindResource("DefaultLineLegendItem") as Style;
            l.Style = s;
            l.Background = this.Background;

            this.legend.AddLegendItem(l);
        }

        Legend legend;
        /// <summary> 刷新数据源 </summary>
        void RefreshDataSource()
        {
            // Todo ：绘制数据 
            foreach (var cs in this.DataSource)
            {
                cs.Draw(this);
            }
        }

        /// <summary> 刷新图例 </summary>
        public void RefreshLegend()
        {
            legend = new Legend();

            if (this.IsLegendVisible)
            {
                this.RightCanvas.Children.Add(legend);
            }

            if (this.IsLegendVisible)
            {
                // Todo ：绘制图例 
                foreach (var cs in this.DataSource)
                {
                    this.BindLegend(cs);
                }
            }
        }

        private CursorGraphLine _graphLine;

        /// <summary> 说明 </summary>
        public CursorGraphLine GraphLine
        {
            get { return _graphLine; }
            set { _graphLine = value; }
        }


        /// <summary> 绘制浮动标尺线 </summary>
        void RefreshCursorGraph()
        {
            if (!this.IsShowCursorLine) return;

            CursorGraphLine cursorgraphline = new CursorGraphLine(this);

            this._graphLine = cursorgraphline;

            cursorgraphline.SetParent(this.ParallelCanvas);
        }



        #endregion

        #region - 依赖属性 -



        public double CoverOpacity
        {
            get { return (double)GetValue(CoverOpacityProperty); }
            set { SetValue(CoverOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoverOpacityProperty =
            DependencyProperty.Register("CoverOpacity", typeof(double), typeof(CurveChartPlotter), new PropertyMetadata(0.1, (d, e) =>
             {
                 CurveChartPlotter control = d as CurveChartPlotter;

                 if (control == null) return;

                 //double config = e.NewValue as double;

             }));


        public List<ICurveEntitySource> DataSource
        {
            get { return (List<ICurveEntitySource>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(List<ICurveEntitySource>), typeof(CurveChartPlotter), new PropertyMetadata(new List<ICurveEntitySource>(), PropertyChangedCallback));



        /// <summary> 数据源改变是否触发动画 </summary>
        public bool DataSourceChangeBegionStory
        {
            get { return (bool)GetValue(DataSourceChangeBegionStoryProperty); }
            set { SetValue(DataSourceChangeBegionStoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSourceChangeBegionStory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSourceChangeBegionStoryProperty =
            DependencyProperty.Register("DataSourceChangeBegionStory", typeof(bool), typeof(CurveChartPlotter), new PropertyMetadata(true));




        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control
                = d as CurveChartPlotter;

            if (control.IsLoaded)
            {
                control.RefreshCurve();

                if (control.DataSourceChangeBegionStory)
                {
                    control.BeginStory();

                }
            }

        }


        /// <summary> 是否启用图例 </summary>
        public bool IsLegendVisible
        {
            get { return (bool)GetValue(IsLegendVisibleProperty); }
            set { SetValue(IsLegendVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLegendVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLegendVisibleProperty =
            DependencyProperty.Register("IsLegendVisible", typeof(bool), typeof(CurveChartPlotter), new PropertyMetadata(true));


        /// <summary> 是否显示上下左右坐标线 </summary>
        public bool IsAsixBoth
        {
            get { return (bool)GetValue(IsAsixBothProperty); }
            set { SetValue(IsAsixBothProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAsixBoth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAsixBothProperty =
            DependencyProperty.Register("IsAsixBoth", typeof(bool), typeof(CurveChartPlotter), new PropertyMetadata(false));


        /// <summary> 是否显示范围值箭头 </summary>
        public bool IsShowTrangle
        {
            get { return (bool)GetValue(ShowTrangleProperty); }
            set { SetValue(ShowTrangleProperty, value); }
        }



        // Using a DependencyProperty as the backing store for ShowTrangle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowTrangleProperty =
            DependencyProperty.Register("IsShowTrangle", typeof(bool), typeof(CurveChartPlotter), new PropertyMetadata(true));



        /// <summary> X坐标自动排列 </summary>
        public bool IsAutoXAxis
        {
            get { return (bool)GetValue(IsShowXAxisProperty); }
            set { SetValue(IsShowXAxisProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowXAxis.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowXAxisProperty =
            DependencyProperty.Register("IsAutoXAxis", typeof(bool), typeof(CurveChartPlotter), new PropertyMetadata(true));



        /// <summary> 自动排列数量 </summary>
        public int AutoXAxisCount
        {
            get { return (int)GetValue(AutoXAxisCountProperty); }
            set { SetValue(AutoXAxisCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AutoXAxisCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoXAxisCountProperty =
            DependencyProperty.Register("AutoXAxisCount", typeof(int), typeof(CurveChartPlotter), new PropertyMetadata(12));


        /// <summary> 内部竖直网线样式 </summary>
        public Style InnerVerticalLineStyle
        {
            get { return (Style)GetValue(InnerVerticalLineStyleProperty); }
            set { SetValue(InnerVerticalLineStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InnerVerticalLineStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InnerVerticalLineStyleProperty =
            DependencyProperty.Register("InnerVerticalLineStyle", typeof(Style), typeof(CurveChartPlotter), new PropertyMetadata(null));


        /// <summary> 内部水平网线样式 </summary>
        public Style InnerHorizontalLineStyle
        {
            get { return (Style)GetValue(InnerHorizontalLineStyleProperty); }
            set { SetValue(InnerHorizontalLineStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InnerHorizontalLineStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InnerHorizontalLineStyleProperty =
            DependencyProperty.Register("InnerHorizontalLineStyle", typeof(Style), typeof(CurveChartPlotter), new PropertyMetadata(null));

        private bool _isShowCursorLine = true;
        /// <summary> 是否显示浮动光标 </summary>
        public bool IsShowCursorLine
        {
            get { return _isShowCursorLine; }
            set { _isShowCursorLine = value; }
        }

        #endregion
    }
}
