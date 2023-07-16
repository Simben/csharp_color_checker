using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorPickers_Controls.Model
{
    public class Color_RGB
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public byte Red   { get => R; set => R = value; }
        public byte Green { get => G; set => G = value; }
        public byte Blue  { get => B; set => B = value; }


        public int iRed 
        {
            get => R;
            set => R = (byte)Math.Max(0, Math.Min(255, value));
        }
        public int iGreen
        {
            get => G;
            set => G = (byte)Math.Max(0, Math.Min(255, value));
        }
        public int iBlue
        {
            get => B;
            set => B = (byte)Math.Max(0, Math.Min(255, value));
        }

        public float fRed
        {
            get => R;
            set => iRed = (int)value;
        }
        public float fGreen
        {
            get => G;
            set => iGreen = (int)value;
        }
        public float fBlue
        {
            get => B;
            set => iBlue = (int)value;
        }

    }

    public class Color_HSV
    {
        private float _h;
        private float _s;
        private float _v;

        public float H
        {
            get { return this._h; }
            set { this._h = Math.Min(360.0f, Math.Max(0.0f, value)); }
        }
        public float S
        {
            get { return this._s; }
            set { this._s = Math.Min(6.0f, Math.Max(0.0f, value)); }
        }
        public float V
        {
            get { return this._v; }
            set { this._v = Math.Min(2.0f, Math.Max(0.0f, value)); }
        }

        public float Hue
        {
            get => this.H;
            set => this.H = value;
        }

        public float Saturation
        {
            get => this.S;
            set => this.S = value;
        }

        public float Value
        {
            get => this.V;
            set => this.V = value;
        }




    }
}
