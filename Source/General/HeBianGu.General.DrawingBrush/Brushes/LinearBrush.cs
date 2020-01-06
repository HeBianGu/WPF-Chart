using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HeBianGu.General.DrawingBrush
{
    public class LinearBrush : IBrush
    {
        public GradientStopCollection GradientStops { get; set; } = new GradientStopCollection();

        public int Start { get; set; }

        public int End { get; set; }

        List<Tuple<GradientStop, GradientStop>> _gradientStopTuple;

        public List<Tuple<GradientStop, GradientStop>> GradientStopTuple
        {
            get
            {
                if (_gradientStopTuple == null)
                {
                    _gradientStopTuple = this.BeginDraw();
                }

                return _gradientStopTuple;
            }
        }

        public void Draw(int x, int y, ref int r, ref int g, ref int b)
        {
            if (x < Start || x > End) return;

            var red = this.Get(x, (l, k) => k.Color.R - l.Color.R, l => l.Color.R);

            if (red != null) r = red.Value;

            var green = this.Get(x, (l, k) => k.Color.G - l.Color.G, l => l.Color.G);

            if (green != null) g = green.Value;

            var blue = this.Get(x, (l, k) => k.Color.B - l.Color.B, l => l.Color.B);

            if (blue != null) b = blue.Value;

        }

        int? Get(int x, Func<GradientStop, GradientStop, int> func, Func<GradientStop, int> func1)
        {
            int lenght = this.End - this.Start;

            foreach (var item in GradientStopTuple)
            {
                int s = (int)(item.Item1.Offset * lenght) + this.Start;
                int e = (int)(item.Item2.Offset * lenght) + this.Start;

                if (x < s || x > e) continue;

                var ss = (double)(x - s) / (double)(e - s);

                var sss = func(item.Item1, item.Item2);

                return (int)((double)(x - s) / (double)(e - s) * func(item.Item1, item.Item2)) + func1(item.Item1);
            }

            return null;
        }

        List<Tuple<GradientStop, GradientStop>> BeginDraw()
        {
            List<Tuple<GradientStop, GradientStop>> tuples = new List<Tuple<GradientStop, GradientStop>>();

            GradientStop last = null;

            foreach (var item in GradientStops)
            {
                if (last != null)
                {
                    tuples.Add(Tuple.Create(last, item));
                }

                last = item;
            }

            return tuples;
        }

        public LinearBrush Clone()
        {
            LinearBrush linearBrush = new LinearBrush();

            foreach (var item in GradientStops)
            {
                linearBrush.GradientStops.Add(item);
            }

            linearBrush.Start = this.Start;
            linearBrush.End = this.End;

            return linearBrush;
        }

    }
}
