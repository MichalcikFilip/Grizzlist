using System.Windows.Media;

namespace Grizzlist.Client.Extensions
{
    public static class ColorExtension
    {
        public static Color SetAlpha(this Color color, byte a)
        {
            return Color.FromArgb(a, color.R, color.G, color.B);
        }
    }
}
