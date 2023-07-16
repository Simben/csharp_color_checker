using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace ColorPickers_Controls.Helper
{
    public class Polar_Coordinate
    {
        public float Theta { get; set; }
        public float Radius { get; set; }
    }

    public static class Maths_Helper
    {
        public static Polar_Coordinate ConvCarthesianToPolar(Point pt, Point origin)
        {
            int X = pt.X - origin.X;
            int Y = origin.Y - pt.Y;
            float tmpR = (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

            return new Polar_Coordinate()
            {
                Radius = tmpR,
                Theta = (float)((  Math.Atan2(X,Y)) + Math.PI)
            };
        
        }


    }
}
