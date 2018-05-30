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

namespace WpfCardiogramReport
{
    /// <summary>
    /// CardiogramUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class CardiogramUserControl : UserControl
    {
        public CardiogramUserControl()
        {
            InitializeComponent();
        }



        public List<ICurveEntitySource> DataSource
        {
            get { return (List<ICurveEntitySource>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(List<ICurveEntitySource>), typeof(CardiogramUserControl), new PropertyMetadata(null, (s, e) => {

                CardiogramUserControl control = s as CardiogramUserControl;

                control.cardiogram.DataSource = e.NewValue as List<ICurveEntitySource>;
            }));

        
    }
}
