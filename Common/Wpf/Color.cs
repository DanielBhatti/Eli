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
        White,
        Maroon,
        Crimson,
        OrangeRed,
        Khaki,
        Olive,
        DarkGreen,
        Teal,
        Indigo,
        SlateBlue,
        Magenta,
        HotPink,
        Beige,
        Sienna,
        RosyBrown,
        SlateGray
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
                case Color.Maroon:
                    return WinMedia.Color.FromArgb(255, 128, 0, 0);
                case Color.Crimson:
                    return WinMedia.Color.FromArgb(255, 220, 20, 60);
                case Color.OrangeRed:
                    return WinMedia.Color.FromArgb(255, 255, 69, 0);
                case Color.Khaki:
                    return WinMedia.Color.FromArgb(255, 240, 230, 140);
                case Color.Olive:
                    return WinMedia.Color.FromArgb(255, 128, 128, 0);
                case Color.DarkGreen:
                    return WinMedia.Color.FromArgb(255, 0, 100, 0);
                case Color.Teal:
                    return WinMedia.Color.FromArgb(255, 0, 128, 128);
                case Color.Indigo:
                    return WinMedia.Color.FromArgb(255, 75, 0, 130);
                case Color.SlateBlue:
                    return WinMedia.Color.FromArgb(255, 106, 90, 205);
                case Color.Magenta:
                    return WinMedia.Color.FromArgb(255, 255, 0, 255);
                case Color.HotPink:
                    return WinMedia.Color.FromArgb(255, 255, 105, 180);
                case Color.Beige:
                    return WinMedia.Color.FromArgb(255, 245, 245, 220);
                case Color.Sienna:
                    return WinMedia.Color.FromArgb(255, 160, 82, 45);
                case Color.RosyBrown:
                    return WinMedia.Color.FromArgb(255, 188, 143, 143);
                case Color.SlateGray:
                    return WinMedia.Color.FromArgb(255, 112, 128, 144);
                default:
                    return WinMedia.Color.FromArgb(0, 0, 0, 0);
            }
        }
    }
}
