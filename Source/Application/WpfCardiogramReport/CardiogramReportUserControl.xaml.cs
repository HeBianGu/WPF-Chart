using HeBianGu.WPF.EChart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfChartDemo;

namespace WpfCardiogramReport
{
    /// <summary>
    /// CardiogramReportUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class CardiogramReportUserControl : UserControl
    {

        CardiogramReportViewModel _viewModel = new CardiogramReportViewModel();

        public CardiogramReportUserControl()
        {
            InitializeComponent();

            this.DataContext = _viewModel;
        }

        public void Print()
        {
            PrintProvider.PrintGrid(this.grid_all);
        }
    }


    /// <summary> 说明 </summary>
    partial class CardiogramReportViewModel
    {

        private List<List<ICurveEntitySource>> _collection = new List<List<ICurveEntitySource>>();
        /// <summary> 曲线图数据 </summary>
        public List<List<ICurveEntitySource>> Collection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                RaisePropertyChanged("Collection");
            }
        }

        /// <summary> 刷新显示最后的几条 </summary>
        public void ShowLast(List<string> collection, int xMargin = 150, int count = 600)
        {

            Func<int, double> convertFuncX = l =>
            {
                if (collection.Count < count)
                {
                    l = count - collection.Count + l;
                }

                return ((double)l / count) * xMargin + (150 - xMargin) / 2;
            };

            Func<double, double> convertFuncY = l =>
            {
                //return (50 * (l - 1848) + 0 * (2448 - l)) / (2448 - 1848);
                return ((l - 2048) / 150) * 10 + 30 / 2;

            };

            int total = collection.Count;

            int skip = total > count ? total - count : 0;

            Application.Current.Dispatcher.Invoke(() =>
            {
                var cs = collection.Skip(skip).ToList();

                List<ICurveEntitySource> collections = new List<ICurveEntitySource>();

                CardiogramCurveEntitySource entity = new CardiogramCurveEntitySource();

                for (int i = 0; i < cs.Count; i++)
                {
                    PointC point = new PointC();

                    point.X = convertFuncX(i);

                    double d;
                    bool result = double.TryParse(cs[i], out d);
                    if (result)
                    {
                        point.Y = convertFuncY(d);
                        point.Text = DateTime.Now.AddDays(i).ToString("yyyy-MM-dd");
                        entity.Source.Add(point);
                    }
                }

                collections.Add(entity);

                this.Collection.Add(collections);
            });



        }


        void Init()
        {
            int pageCount = 1125;

            string str = Properties.Resources.心电图;

            var collection = str.Split(',').ToList();

            for (int i = 0; i < 4; i++)
            {
                var items = collection.Skip(i * pageCount).Take(pageCount).ToList();

                this.ShowLast(items, 150, items.Count);
            }
        }
        private void ButtonClickFunc(object obj)
        {
            string buttonName = obj as string;

            switch (buttonName)
            {
                case "Case1":
                    {

                    }
                    break;
                case "Case2":
                    {

                    }
                    break;
                case "Case3":
                    {

                    }
                    break;
                case "Case4":
                    {

                    }
                    break;
                case "Case5":
                    {

                    }
                    break;
                case "Case6":
                    {

                    }
                    break;
                case "Case7":
                    {

                    }
                    break;
                case "Case8":
                    {

                    }
                    break;
                case "Case9":
                    {

                    }
                    break;
                case "Case10":
                    {

                    }
                    break;
                case "Case11":
                    {

                    }
                    break;
                case "Case12":
                    {

                    }
                    break;
                default:
                    break;
            }
        }
    }

    partial class CardiogramReportViewModel : INotifyPropertyChanged
    {
        public RelayCommand RelayCommand { get; set; }

        public CardiogramReportViewModel()
        {
            RelayCommand = new RelayCommand(new Action<object>(ButtonClickFunc));
            Init();
        }

        #region - MVVM -

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
