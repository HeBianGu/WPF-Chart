#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/1/19 16:45:44 
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
using System.Windows.Media;

namespace HeBianGu.WPF.EChart
{
    /// <summary> 网格分割线 </summary>
    public class SplitItem
    {
        private double _value;
        /// <summary> 值 </summary>
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private Brush _color;
        /// <summary> 颜色 </summary>
        public Brush Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private string _text;
        /// <summary> 文本 </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private SplitItemType _spliteType;
        /// <summary> 说明 </summary>
        public SplitItemType SpliteType
        {
            get { return _spliteType; }
            set { _spliteType = value; }
        }
    }


    /// <summary> 线坐标类型 </summary>
    public enum SplitItemType
    {
        /// <summary> 正常线 </summary>
        Normal = 0,
        /// <summary> 特殊线 </summary>
        HeighLight,

        /// <summary> 只显示内部网格线 </summary>
        InnerOnly
    }
}
