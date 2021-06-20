using WinMedia = System.Windows.Media;

namespace Common.Wpf
{
    public enum Color
    {
        Red,
        Green,
        Blue,
        Orange,
        Violet,
        Black,
        White
    }

    public static class ColorExtensions
    {
        public static WinMedia.Color ToWindowsMediaColor(this Color color)
        {
            switch(color)
            {
                case Color.Red:
                    return WinMedia.Color.FromArgb(255, 255, 0, 0);
                case Color.Green:
                    return WinMedia.Color.FromArgb(255, 0, 255, 0);
                case Color.Blue:
                    return WinMedia.Color.FromArgb(255, 0, 0, 255);
                case Color.Orange:
                    return WinMedia.Color.FromArgb(255, 255, 115, 0);
                case Color.Violet:
                    return WinMedia.Color.FromArgb(255, 180, 80, 255);
                case Color.Black:
                    return WinMedia.Color.FromArgb(255, 0, 0, 0);
                case Color.White:
                    return WinMedia.Color.FromArgb(255, 255, 255, 255);
                default:
                    return WinMedia.Color.FromArgb(0, 0, 0, 0);
            }
        }
    }
}
