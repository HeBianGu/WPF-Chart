using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HeBianGu.General.DrawingBrush
{
    public class PlanarGridBrush : IBrush
    {

        public double[,] Values;

        public int Start { get; set; }

        public int End { get; set; }

        public int ItemWidth { get; set; }

        public int ItemHeigh { get; set; }

        public Color MaxColor { get; set; }

        public Color MinColor { get; set; }

        public double MaxValue { get; set; }

        public double MinValue { get; set; }

        public void Draw(int x, int y, ref int r, ref int g, ref int b)
        {
            int wIndex = x / ItemWidth;

            int hIndex = y / ItemHeigh;

            var value = Values[wIndex, hIndex];

            r = this.GetColor(value, (l, k) => l.R - k.R, l => l.R);
            g = this.GetColor(value, (l, k) => l.G - k.G, l => l.G);
            b = this.GetColor(value, (l, k) => l.B - k.B, l => l.B);
        }

        int GetColor(double value, Func<Color, Color, int> func, Func<Color, int> func1)
        {
            double lenght = this.MaxValue - this.MinValue + 1;

            double persent = (double)(value - this.MinValue) / lenght;

            return (int)(persent * func(this.MaxColor, this.MinColor) + func1(this.MinColor));


        }
    }
}
