#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/5/7 10:06:12 
 * 文件名：Class1 
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
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfChartDemo
{

    /// <summary> 说明 </summary>
    partial class MainWindowViewModel
    {
        private List<ICurveEntitySource> _collection = new List<ICurveEntitySource>();
        /// <summary> 曲线图数据 </summary>
        public List<ICurveEntitySource> Collection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                RaisePropertyChanged("Collection");
            }
        }

        void Init()
        {
            RefreshCurveData();
        }

       public  void RefreshCurveData()
        {

            List<ICurveEntitySource> collection = new List<ICurveEntitySource>();

            CurveEntitySource entity = new CurveEntitySource();
            entity.Text = "长度(km)";
            entity.Color = Brushes.Red;
            entity.Marker = new CirclePointMarker();

            entity.Marker.Fill = Brushes.Red;

            for (int i = 0; i < 20; i++)
            {
                PointC point = new PointC();
                point.X = i;
                point.Y = i*i;
                point.Text = DateTime.Now.AddDays(i).ToString("yyyy-MM-dd");
                entity.Source.Add(point);

                this.MinValue = this.MinValue > point.X ? point.X : this.MinValue;
                this.MaxValue = this.MaxValue < point.X ? point.X : this.MaxValue;

            }
            collection.Add(entity);

            entity = new CurveEntitySource();
            entity.Text = "重量(kg)";
            entity.Color = Brushes.Orange;
            entity.Marker = new  T5PointMarker();

            entity.Marker.Fill = Brushes.Orange;

            for (int i = 0; i < 20; i++)
            {
                PointC point = new PointC();
                point.X = i;
                point.Y = (20-i) * (20 - i);
                point.Text = DateTime.Now.AddDays(i).ToString("yyyy-MM-dd");
                entity.Source.Add(point);

                this.MinValue = this.MinValue > point.X ? point.X : this.MinValue;
                this.MaxValue = this.MaxValue < point.X ? point.X : this.MaxValue;

            }
            collection.Add(entity);

            this.Collection = collection;

        }

        private double _maxValue = double.MinValue;
        /// <summary> 说明 </summary>
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                RaisePropertyChanged("MaxValue");
            }
        }

        private double _minValue = double.MaxValue;
        /// <summary> 说明 </summary>
        public double MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                RaisePropertyChanged("MinValue");
            }
        }
    }

    partial class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
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
