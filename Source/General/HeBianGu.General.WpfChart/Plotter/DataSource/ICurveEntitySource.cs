#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/1/19 16:39:59 
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
using System.Windows.Media;

namespace HeBianGu.WPF.EChart
{
    /// <summary> 曲线数据源接口 </summary>
    public interface ICurveEntitySource
    {
        /// <summary> 绘制方法 </summary>
        void Draw(CurveChartPlotter chart);

        /// <summary> 数据集合 </summary>
        List<PointC> Source { get; set; }

        /// <summary> 曲线颜色 </summary>
        Brush Color { get; set; }

        /// <summary> 标记 </summary>
        ShapePointMarker Marker { get; set; }

        /// <summary> 名称 </summary>
        string Text { get; set; }


        /// <summary> 是否可见 </summary>
        Visibility Visibility { get; set; }


        event Action VisibleChanged;

    }


    /// <summary> 抽象基类 </summary>
    public abstract class BaseEntitySource : ICurveEntitySource
    {
        private List<PointC> _source = new List<PointC>();
        /// <summary> 说明 </summary>
        public List<PointC> Source
        {
            get { return _source; }
            set { _source = value; }
        }

        private Brush _color = Brushes.Black;
        /// <summary> 曲线颜色 </summary>
        public Brush Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private ShapePointMarker _marker = new CirclePointMarker();
        /// <summary> 说明 </summary>
        public ShapePointMarker Marker
        {
            get { return _marker; }
            set { _marker = value; }
        }


        private string _text;
        /// <summary> 文本 </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        List<UIElement> _elements = new List<UIElement>();
        /// <summary> 包含的所有元素 </summary>
        public List<UIElement> Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }

        private Visibility _vsibility = Visibility.Visible;
        /// <summary> 是否可见 </summary>
        public Visibility Visibility
        {
            get { return _vsibility; }
            set
            {
                foreach (var item in _elements)
                {
                    item.Visibility = value;
                }

                if (_vsibility != value)
                {
                    _vsibility = value;

                    if (_visibleChanged != null)
                    {
                        _visibleChanged();
                    }
                }

             


            }
        }


        Action _visibleChanged;

        event Action ICurveEntitySource.VisibleChanged
        {
            add
            {
                _visibleChanged += value;
            }

            remove
            {
                _visibleChanged -= value;
            }
        }

        public abstract void Draw(CurveChartPlotter chart);
    }

}
