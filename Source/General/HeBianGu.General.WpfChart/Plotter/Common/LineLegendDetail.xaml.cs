using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace HeBianGu.WPF.EChart
{
    /// <summary> 直线图例 </summary>
    public partial class LineLegendDetail : LegendItem
    {

        ICurveEntitySource _curve;
        public LineLegendDetail(ICurveEntitySource c)
        {
            _curve = c;

            this.Color = c.Color;
            this.Text = c.Text;
            this.Marker = c.Marker?.Clone();

            this.Checked += LineLegendItem_Checked;
            this.Unchecked += LineLegendItem_Checked;
        }

        private void LineLegendItem_Checked(object sender, RoutedEventArgs e)
        {
            Curve.Visibility = this.IsChecked.Value ? Visibility.Hidden : Visibility.Visible;
        }

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(LineLegendDetail), new PropertyMetadata(Brushes.Black));


        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(LineLegendDetail), new PropertyMetadata(""));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LineLegendDetail), new PropertyMetadata(""));




        public ShapePointMarker Marker
        {
            get { return (ShapePointMarker)GetValue(MarkerProperty); }
            set { SetValue(MarkerProperty, value); }
        }

        public ICurveEntitySource Curve { get => _curve; set => _curve = value; }

        // Using a DependencyProperty as the backing store for Marker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkerProperty =
            DependencyProperty.Register("Marker", typeof(ShapePointMarker), typeof(LineLegendDetail), new PropertyMetadata(null));


        public void RefreshValue(string text)
        {
            //v.Text = text;

            this.Value = " : " + text;
        }

        static LineLegendDetail()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineLegendDetail), new FrameworkPropertyMetadata(typeof(LineLegendDetail)));
        }
    }
}
