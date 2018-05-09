using HeBianGu.WPF.EChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeBianGu.General.WpfChart
{
    /// <summary> 浮动标尺控件 </summary>
    public partial class CursorGraphLine : UserControl
    {

        CurveChartPlotter _curve;

        List<LineLegendDetail> _items = new List<LineLegendDetail>();
        public  CursorGraphLine(CurveChartPlotter c)
        {
            InitializeComponent();

            _curve = c;

            this.content.Visibility = Visibility.Hidden;
        }

        #region - 绘图方法 -

        ///// <summary> 绑定到指定曲线图中 </summary>
        //public static void BindingInTo(CurveChartPlotter c)
        //{
        //    CursorGraphLine cursorgraphline = new CursorGraphLine();

        //    cursorgraphline._curve = c;

        //    cursorgraphline.SetParent(c.ParallelCanvas);
        //}

        public void SetParent(Canvas c)
        {
            this.content.Width = c.ActualWidth;
            this.content.Height = c.ActualHeight;

            c.MouseMove += parent_MouseMove;
            c.MouseEnter += Parent_MouseEnter;
            c.MouseLeave += Parent_MouseLeave;

            // Todo ：增加蒙版用于响应事件 
            Rectangle r1 = new Rectangle();
            r1.Width = c.ActualWidth;
            r1.Height = c.ActualHeight;
            r1.Fill = Brushes.Transparent;
            Canvas.SetTop(r1, 0);
            c.Children.Add(r1);
            c.Children.Add(this);

            foreach (var item in _curve.DataSource)
            {
                if (item.Visibility != Visibility.Visible) continue;

                LineLegendDetail con = new LineLegendDetail(item);
                Style s = this.FindResource("DefaultLineLegendDetail") as Style;
                con.Style = s;

                this._items.Add(con);

                // ToEdit：2018-05-08 02:49:53 修改有父节点 
                if (item.Marker.Parent != null)
                {
                    this.content.Children.Add(item.Marker.Clone());
                }
                else
                {
                    this.content.Children.Add(item.Marker);
                }
                this.stackPanel.Children.Add(con);
            }
            
        }
        
        public void Refresh()
        {
            this._items.Clear();
            this.stackPanel.Children.Clear();

            foreach (var item in _curve.DataSource)
            {
                if (item.Visibility != Visibility.Visible) continue;

                LineLegendDetail con = new LineLegendDetail(item);
                Style s = this.FindResource("DefaultLineLegendDetail") as Style;
                con.Style = s;

                this._items.Add(con);
                //this.content.Children.Add(item.Marker);
                this.stackPanel.Children.Add(con);
            }
        }

        void parent_MouseMove(object sender, MouseEventArgs e)
        {
            if(_items.Count==0)
            {
                this.Visibility = Visibility.Hidden;return;
            }

            this.Visibility = Visibility.Visible;
            // Todo ：检测设置是否只显示有效值
            Point mousePos = Mouse.GetPosition(this);
            double param = 5;
            bool isHaveValue = false;

            foreach (var item in _items)
            {
                var f = item.Curve.Source.Find(l => Math.Abs(_curve.GetX(l.X) - mousePos.X) < param);

                if (f != null)
                {
                    isHaveValue = true;
                    mousePos.X = _curve.GetX(f.X);
                    this.tb_title.Text = f.Text;
                    break;
                }
            }

            

            if (this.IsOnlyHasValue)
            {
                if(!isHaveValue)
                {
                    return;
                }
            }

            // Todo ：说明 
            RefreshRepresentation(mousePos);

            // Todo ：说明 
            RefreshCurveSource(mousePos);
        }
        
        /// <summary> 绘制标尺线 </summary>
        private void RefreshRepresentation(Point mousePos)
        {
            // Todo ：移动坐标线 
            horizLine.X1 = 0;
            horizLine.X2 = this.content.ActualWidth;
            horizLine.Y1 = mousePos.Y;
            horizLine.Y2 = mousePos.Y;

            vertLine.X1 = mousePos.X;
            vertLine.X2 = mousePos.X;
            vertLine.Y1 = 0;
            vertLine.Y2 = this.content.ActualHeight;

            double space = 10;

            Canvas.SetLeft(this.grid_center, mousePos.X + space);
            Canvas.SetTop(this.grid_center, mousePos.Y + space);

            if (this.content.ActualWidth - mousePos.X < this.stackPanel.ActualWidth + space)
            {
                Canvas.SetLeft(this.grid_center, mousePos.X - this.stackPanel.ActualWidth - space);
            }

            if (this.content.ActualHeight - mousePos.Y < this.stackPanel.ActualHeight + space)
            {
                Canvas.SetTop(this.grid_center, mousePos.Y - this.stackPanel.ActualHeight - space);
            }
        }

        /// <summary> 绘制和曲线的焦点标记 </summary>
        void RefreshCurveSource(Point mousePos)
        {
            foreach (var item in _items)
            {
                var minXC = item.Curve.Source.FindLast(l => _curve.GetX(l.X) <= mousePos.X);
                var maxXC = item.Curve.Source.Find(l => _curve.GetX(l.X) >= mousePos.X);

                Point minX = new Point(_curve.GetX(minXC.X), _curve.GetY(minXC.Y));

                Point maxX = new Point(_curve.GetX(maxXC.X), _curve.GetY(maxXC.Y));

                double xpercent = maxX.X == minX.X ? 0 : (mousePos.X - minX.X) / (maxX.X - minX.X);

                // Todo ：焦点的Y值 
                double resultY = xpercent * (maxX.Y - minX.Y) + minX.Y;

                item.Curve.Marker.Stroke = this.horizLine.Stroke;
                item.Curve.Marker.Fill = this.horizLine.Stroke;

                Point cross = new Point(mousePos.X, resultY);
                Canvas.SetLeft(item.Curve.Marker, cross.X);
                Canvas.SetTop(item.Curve.Marker, cross.Y);

                string value = Math.Round(this._curve.GetYValue(resultY), 2).ToString();

                item.RefreshValue(value);
            }

        }

        private void Parent_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AutoHide)
            {
                horizLine.Visibility = ShowHorizontalLine ? Visibility.Visible : Visibility.Hidden;
                vertLine.Visibility = ShowVerticalLine ? Visibility.Visible : Visibility.Hidden;
                this.grid_center.Visibility = Visibility;

                this.content.Visibility = Visibility;
            }
        }

        private void Parent_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AutoHide)
            {
                horizLine.Visibility = Visibility.Hidden;
                vertLine.Visibility = Visibility.Hidden;
                this.grid_center.Visibility = Visibility.Hidden;
                this.content.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        #region - 绑定属性 -


        #region LineStroke property

        public Brush LineStroke
        {
            get { return (Brush)GetValue(LineStrokeProperty); }
            set { SetValue(LineStrokeProperty, value); }
        }

        public static readonly DependencyProperty LineStrokeProperty = DependencyProperty.Register(
          "LineStroke",
          typeof(Brush),
          typeof(CursorGraphLine),
          new PropertyMetadata(new SolidColorBrush(Color.FromArgb(170, 86, 86, 86))));

        #endregion

        #region LineStrokeThickness property

        public double LineStrokeThickness
        {
            get { return (double)GetValue(LineStrokeThicknessProperty); }
            set { SetValue(LineStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty LineStrokeThicknessProperty = DependencyProperty.Register(
          "LineStrokeThickness",
          typeof(double),
          typeof(CursorGraphLine),
          new PropertyMetadata(2.0));

        #endregion

        #region LineStrokeDashArray property

        public DoubleCollection LineStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(LineStrokeDashArrayProperty); }
            set { SetValue(LineStrokeDashArrayProperty, value); }
        }

        public static readonly DependencyProperty LineStrokeDashArrayProperty = DependencyProperty.Register(
          "LineStrokeDashArray",
          typeof(DoubleCollection),
          typeof(CursorGraphLine),
          new FrameworkPropertyMetadata(new DoubleCollection(new double[] { 3, 3 })));

        #endregion

        #endregion

        #region - 控制变量 -

        bool showHorizontalLine = true;
        bool showVerticalLine = true;
        bool autoHide = true;
        bool _isOnlyHasValue = true;

        /// <summary> 设置成true则只显示有效值部分的浮动标尺 </summary>
        public bool IsOnlyHasValue { get => _isOnlyHasValue; set => _isOnlyHasValue = value; }


        /// <summary> 设置标尺离开绘图区域是否自动隐藏 </summary>
        public bool AutoHide { get => autoHide; set => autoHide = value; }


        /// <summary> 是否显示竖直方向标尺 </summary>
        public bool ShowVerticalLine { get => showVerticalLine; set => showVerticalLine = value; }

        /// <summary> 是否显示水平方向标尺 </summary>
        public bool ShowHorizontalLine { get => showHorizontalLine; set => showHorizontalLine = value; }

        #endregion

    }


}
