using System.Windows.Media.Imaging;

namespace HeBianGu.General.DrawingBrush
{
    public interface IBitmap
    { 
        WriteableBitmap Source { get; set; }

        IBitmap Draw();

        void GetColor(int x, int y, ref int red, ref int green, ref int blue);
    }
}