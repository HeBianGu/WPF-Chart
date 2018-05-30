#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 长虹智慧健康有限公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[李海军]   时间：2018/5/17 16:02:05 
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
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfChartDemo
{
    /// <summary> 打印支持类 </summary>
    class PrintProvider 
    {

        /// <summary> 打印表格 </summary>
        public static void PrintGrid(Grid grid, bool isShowForm = true)
        {
            Action action = () =>
            {
                PrintDialog dialog = new PrintDialog();
                dialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Portrait;
                dialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4Extra);
           

                    dialog.PrintVisual(grid, "Print Test");
           
            };

            Application.Current.Dispatcher.BeginInvoke(action, DispatcherPriority.SystemIdle, null);


        }
    }
}
