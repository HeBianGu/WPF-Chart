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
//using UserControls.Reports.Engine;
//using UserControls.Reports.Provider;
//using UserControls.Reports.Service;

namespace WpfChartDemo
{
    /// <summary> 小站报告精简版 - 心电报告 </summary>
    public partial class TwelveReprotCardiogram : UserControl
    {
        TwelveReprotCardiogramViewModel _vm = new TwelveReprotCardiogramViewModel();

        public TwelveReprotCardiogram()
        {
            InitializeComponent();

            _vm.Init();

            this.DataContext = _vm;
            
        }

        public void Print(bool showform = true)
        {
            //PrintProvider.Instance.PrintGrid(this.grid_all, showform);
        }

        public void RefreshData()
        {
            throw new NotImplementedException();
        }



    }
    /// <summary> 说明 </summary>
    partial class TwelveReprotCardiogramViewModel
    {


        private List<ICurveEntitySource> _xdCollection = new List<ICurveEntitySource>();
        /// <summary> 心电 </summary>
        public List<ICurveEntitySource> XDCCollection
        {
            get { return _xdCollection; }
            set
            {
                _xdCollection = value;
                RaisePropertyChanged("XDCCollection");
            }
        }

        public void Init()
        {
            string str = WpfCardiogramReport.Properties.Resources.心电图;

            if (!string.IsNullOrEmpty(str))
            {
                List<string> collection = str.Split(',').ToList();

                this.XDCCollection.Clear();

                int pageCount = 1125;

                int index = 0;

                for (int i = 0; i < 4; i++)
                {
                    var items = collection.Skip(i * pageCount).Take(pageCount).ToList();

                    this.XDCCollection.Add(this.ShowLast(items, 150, items.Count, 240 * index - 200));
                    index++;
                }

                for (int i = 0; i < 4; i++)
                {
                    var items = collection.Skip(i * pageCount).Take(pageCount).ToList();

                    this.XDCCollection.Add(this.ShowLast(items, 150, items.Count, 240 * index - 200));

                    index++;
                }

                for (int i = 0; i < 4; i++)
                {
                    var items = collection.Skip(i * pageCount).Take(pageCount).ToList();

                    this.XDCCollection.Add(this.ShowLast(items, 150, items.Count, 240 * index - 200));

                    index++;
                }

            }
        }

        /// <summary> 刷新显示最后的几条 </summary>
        public ICurveEntitySource ShowLast(List<string> collection, int xMargin = 150, int count = 600, int addValue = 0)
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
                l = l + addValue;
                //return (50 * (l - 1848) + 0 * (2448 - l)) / (2448 - 1848);
                return ((l - 2048) / 150) * 10 + 50 / 2;

            };

            int total = collection.Count;

            int skip = total > count ? total - count : 0;

            //List<ICurveEntitySource> collections = new List<ICurveEntitySource>();



            var cs = collection.Skip(skip).ToList();

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
                    point.Text = i.ToString(); ;
                    entity.Source.Add(point);
                }
            }

            return entity;

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

    partial class TwelveReprotCardiogramViewModel : INotifyPropertyChanged
    {
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
