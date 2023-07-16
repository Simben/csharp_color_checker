using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ColorPickers_Controls.Model;

namespace ColorPickers_Controls.Helper
{
    public static class ColorConv_Helper
    {
        public static Color_RGB Conver_HsvToRgb(Color_HSV hsv)
        {
            return Conver_HsvToRgb(hsv.H, hsv.S, hsv.V);
        }
        public static Color_RGB Conver_HsvToRgb(float H, float S, float V)
        {
            float M = 255.0f * V;
            float m = M * (1 - S);

            float z = (M - m) * (1 - Math.Abs(((H / 60) % 2) - 1));

            var res = new Color_RGB();
            if (H < 60)
            {
                res.fRed = M;
                res.fGreen = z + m;
                res.fBlue = m;
            }
            else if (H < 120)
            {
                res.fRed = z + m;
                res.fGreen = M;
                res.fBlue = m;
            }
            else if (H < 180)
            {
                res.fRed = m;
                res.fGreen = M;
                res.fBlue = z + m;
            }
            else if (H < 240)
            {
                res.fRed = m;
                res.fGreen = z + m;
                res.fBlue = M;
            }
            else if (H < 300)
            {
                res.fRed = z + m;
                res.fGreen = m;
                res.fBlue = M;
            }
            else
            {
                res.fRed = M;
                res.fGreen = m;
                res.fBlue = z + m;
            }

            return res;
        }

    }
}
