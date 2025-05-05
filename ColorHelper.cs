using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deskguise
{
    public static class ColorHelper
    {
        public static string ColorToString(Color color)
        {
            return $"{color.R} {color.G} {color.B}";
        }

        public static Color StringToColor(string str)
        {
            string[] parts = str.Split(' ');
            return Color.FromArgb(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }

        public static bool AreEqual(Color color1, Color color2)
        {
            int rDiff = Math.Abs(color1.R - color2.R);
            int gDiff = Math.Abs(color1.G - color2.G);
            int bDiff = Math.Abs(color1.B - color2.B);
            return rDiff < 2 && gDiff < 2 && bDiff < 2;
        }

        public static Color CreateSlightlyOffColor(Color color)
        {
            if (AreEqual(color, Color.Black))
            {
                return Color.FromArgb(1, 1, 64);
            }
            if (AreEqual(color, Color.White))
            {
                return Color.FromArgb(255, 255, 245);
            }

            double r = color.R;
            double g = color.G;
            double b = color.B;

            double sum = (r + g + b);

            double rs = r / sum;
            double gs = g / sum;
            double bs = b / sum;

            double diff = 63.0d / 3.0d;

            double rd = rs * diff;
            double gd = gs * diff;
            double bd = bs * diff;

            double mid = 127 * 3;

            double newR = r;
            double newG = g;
            double newB = b;

            if (sum >= mid)
            {
                // Reduce brightness
                newR -= rd;
                newG -= gd;
                newB -= bd;

                newR = Math.Round(newR);
                newG = Math.Round(newG);
                newB = Math.Round(newB);

                if (newR < byte.MinValue) { newR = r + rd; newR = Math.Round(newR); }
                if (newG < byte.MinValue) { newG = g + gd; newG = Math.Round(newG); }
                if (newB < byte.MinValue) { newB = b + bd; newB = Math.Round(newB); }

                newR = Math.Max(1, newR);
                newG = Math.Max(1, newG);
                newB = Math.Max(1, newB);
            }
            else
            {
                // Increase brightness
                newR += rd;
                newG += gd;
                newB += bd;

                newR = Math.Round(newR);
                newG = Math.Round(newG);
                newB = Math.Round(newB);

                if (newR > byte.MaxValue) { newR = r - rd; newR = Math.Round(newR); }
                if (newG > byte.MaxValue) { newG = g - gd; newG = Math.Round(newG); }
                if (newB > byte.MaxValue) { newB = b - bd; newB = Math.Round(newB); }
            }

            return Color.FromArgb((byte)newR, (byte)newG, (byte)newB);
        }
    }
}
