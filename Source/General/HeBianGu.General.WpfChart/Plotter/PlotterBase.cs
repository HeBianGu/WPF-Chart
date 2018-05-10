#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/1/18 11:08:41 
 * 文件名：Plotter 
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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HeBianGu.WPF.EChart
{

    /// <summary> 绘图机基类 </summary>
    [TemplatePart(Name = "PART_ParallelCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_TopCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_BottomCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_LeftCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_RightCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_ParallelBottomCanvas", Type = typeof(Canvas))]
    
    public abstract class PlotterBase : ContentControl
    {
        protected PlotterBase()
        {
            this.InitParts();
        }

        private void InitParts()
        {
            ResourceDictionary dict = new ResourceDictionary
            {
                Source = new Uri("/HeBianGu.General.WpfChart;component/Plotter/PlotterBaseStyle.xaml", UriKind.Relative)
            };

            Style = (Style)dict["DefaultPlotterStyle"];

            ApplyTemplate();
            
            //// Todo ：初始化淡出初始效果 
            //parallelCanvas.OpacityMask = this.FindResource("WindowOpMack") as Brush;
        }

        #region - 模板重写 -

        Canvas _topCanvas;
        Canvas _bottomCanvas;
        Canvas _leftCanvas;
        Canvas _rightCanvas;
        Canvas parallelCanvas;
        Canvas _parallelBottomCanvas;
        Canvas _pathCanvas;
        public Canvas ParallelCanvas { get => parallelCanvas; }
        public Canvas RightCanvas { get => _rightCanvas; set => _rightCanvas = value; }
        public Canvas LeftCanvas { get => _leftCanvas; set => _leftCanvas = value; }
        public Canvas BottomCanvas { get => _bottomCanvas; set => _bottomCanvas = value; }
        public Canvas TopCanvas { get => _topCanvas; set => _topCanvas = value; }
        public Canvas PathCanvas { get => _pathCanvas; set => _pathCanvas = value; }
        public Canvas ParallelBottomCanvas { get => _parallelBottomCanvas; set => _parallelBottomCanvas = value; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            parallelCanvas = GetPart<Canvas>("PART_ParallelCanvas");
            BottomCanvas = GetPart<Canvas>("PART_BottomCanvas");
            LeftCanvas = GetPart<Canvas>("PART_LeftCanvas");
            RightCanvas = GetPart<Canvas>("PART_RightCanvas");
            TopCanvas = GetPart<Canvas>("PART_TopCanvas");
            PathCanvas = GetPart<Canvas>("PART_PathCanvas");
            ParallelBottomCanvas = GetPart<Canvas>("PART_ParallelBottomCanvas");
        }
        private T GetPart<T>(string name)
        {
            return (T)Template.FindName(name, this);
        }


        #endregion

        #region - 动画 -

        protected void BeginStory()
        {
            DoubleAnimation double1 = new DoubleAnimation(0, this.ActualWidth, new Duration(TimeSpan.FromSeconds(1)));
            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(double1, this.PathCanvas);
            Storyboard.SetTargetProperty(double1, new PropertyPath("Width"));
            storyboard.Children.Add(double1);
            storyboard.Begin();
        }
        #endregion


    }
}
