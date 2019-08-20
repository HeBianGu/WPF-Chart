#region <版 本 注 释>
/*
 * ========================================================================
 * Copyright(c) 四川*******公司, All Rights Reserved.
 * ========================================================================
 *    
 * 作者：[HeBianGu]   时间：2018/1/10 11:50:15 
 * 文件名：ICurveChart 
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
using System.Windows.Controls;

namespace HeBianGu.WPF.EChart
{
    public interface ICurveChart
    {
        double GetY(double value);

        double GetX(double value);

        Canvas ParallelCanvas { get; }
    }
}
