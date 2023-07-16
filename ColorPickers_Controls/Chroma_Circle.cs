using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ColorPickers_Controls.Model;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using ColorPickers_Controls.Model;

namespace ColorPickers_Controls
{
    public delegate void newColorIsSelected(object sender, bool isValid, Color_RGB rgb, Color_HSV hsv);

    public class Chroma_Circle : System.Windows.Forms.Control
    {
        public event newColorIsSelected SelectedColorChanged;

        private float _forcedHsvValue = 1.0f;
        private Color_HSV _pickedColorHSV;
        private Color_RGB _pickedColorRGB;
        private Point? _pickedColorLocation = null;

        private int _radius;
        private Bitmap _backgroundImage;

        private float _saturationEmphasis = 1.0f;


        public float HSV_Value
        {
            get => this._forcedHsvValue;
            set
            {
                if (value != this._forcedHsvValue)
                {
                    this._forcedHsvValue = value;
                    this.Refresh();
                }
            }
        }
        public float SaturationEmphasis
        {
            get => this._saturationEmphasis;
            set
            {
                if (value != this._saturationEmphasis)
                {
                    this._saturationEmphasis = value;
                    this.Refresh();
                }
            }
        }


        public Chroma_Circle() : base()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint,
                true);
            this.UpdateStyles();
            //this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RedrawBackgroundCircleImage();

            int offset_X = (base.Width - (this._radius * 2)) / 2;
            int offset_Y = (base.Height - (this._radius * 2)) / 2;


            e.Graphics.DrawImage(this._backgroundImage, new Point(
                offset_X,
                offset_Y
                ));

            //Draw CrossAir
            if (this._pickedColorLocation != null)
            {
                e.Graphics.DrawLine(new Pen(new SolidBrush(Color.White), 3),
                    new Point((this._pickedColorLocation?.X + offset_X - 5) ?? 0, (this._pickedColorLocation?.Y + offset_Y) ?? 0),
                    new Point((this._pickedColorLocation?.X + offset_X + 6) ?? 0, (this._pickedColorLocation?.Y + offset_Y) ?? 0)
                    );

                e.Graphics.DrawLine(new Pen(new SolidBrush(Color.White), 3),
                    new Point((this._pickedColorLocation?.X + offset_X) ?? 0, (this._pickedColorLocation?.Y + offset_Y - 5) ?? 0),
                    new Point((this._pickedColorLocation?.X + offset_X) ?? 0, (this._pickedColorLocation?.Y + offset_Y + 6) ?? 0)
                    );


                e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Black), 1),
                    new Point((this._pickedColorLocation?.X + offset_X - 5) ?? 0, (this._pickedColorLocation?.Y + offset_Y) ?? 0),
                    new Point((this._pickedColorLocation?.X + offset_X + 5) ?? 0, (this._pickedColorLocation?.Y + offset_Y) ?? 0)
                    );

                e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Black), 1),
                    new Point((this._pickedColorLocation?.X + offset_X) ?? 0, (this._pickedColorLocation?.Y + offset_Y - 5) ?? 0),
                    new Point((this._pickedColorLocation?.X + offset_X) ?? 0, (this._pickedColorLocation?.Y + offset_Y + 5) ?? 0)
                    );
            }

        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            int margin_X = (base.Width - (this._radius * 2)) / 2;
            int margin_Y = (base.Height - (this._radius * 2)) / 2;

            this._pickedColorLocation = new Point(e.X - margin_X, e.Y - margin_Y);

            var coord = Helper.Maths_Helper.ConvCarthesianToPolar(this._pickedColorLocation ?? new Point(0, 0), new Point(this._radius, this._radius));

            if (coord.Radius > this._radius)
            {
                this._pickedColorLocation = null;
                if (SelectedColorChanged != null)
                    SelectedColorChanged(this, false, null, null);
                return;
            }

            _pickedColorHSV = new Color_HSV()
            {
                H = (float)(coord.Theta / (Math.PI) * 180.0f),
                S = (float)coord.Radius / (float)this._radius * this._saturationEmphasis,
                V = this._forcedHsvValue
            };

            this._pickedColorRGB = Helper.ColorConv_Helper.Conver_HsvToRgb(this._pickedColorHSV);

            if (SelectedColorChanged != null)
                SelectedColorChanged(this, true, this._pickedColorRGB, this._pickedColorHSV);

            var tt = this._backgroundImage.GetPixel(this._pickedColorLocation?.X??0, this._pickedColorLocation?.Y??0);
            MessageBox.Show($"{tt.R.ToString()} {tt.G.ToString()} {tt.B.ToString()}");
            this.Refresh();

        }

        //todo recompute onlmy when needed (H,S,V chnaged)
        private void RedrawBackgroundCircleImage()
        {
            _radius = Math.Min(base.Width, base.Height) / 2;

            if (_backgroundImage != null)
                _backgroundImage.Dispose();

            this._backgroundImage = new Bitmap(_radius * 2, _radius * 2, PixelFormat.Format24bppRgb);

            unsafe
            {
                Rectangle rect = new Rectangle(0, 0, _backgroundImage.Width, _backgroundImage.Height);
                System.Drawing.Imaging.BitmapData bData =
                    _backgroundImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    _backgroundImage.PixelFormat);

                byte* scan0 = (byte*)bData.Scan0.ToPointer();

                for (int i = 0; i < rect.Height; i++)
                {
                    for (int j = 0; j < rect.Height; j++)
                    {
                        var tmp = Helper.Maths_Helper.ConvCarthesianToPolar(new Point(j, i), new Point(_radius, _radius));

                        byte* data = scan0 + i * bData.Stride + j * 24 / 8;

                        if (tmp.Radius > _radius)
                        {
                            data[0] = 255;
                            data[1] = 255;
                            data[2] = 255;
                        }
                        else
                        {
                            var rgb = Helper.ColorConv_Helper.Conver_HsvToRgb(
                                (float)(tmp.Theta / (Math.PI) * 180.0f),
                                (float)tmp.Radius / (float)this._radius * this._saturationEmphasis,
                                this._forcedHsvValue);

                            data[0] = rgb.B;
                            data[1] = rgb.G;
                            data[2] = rgb.R;
                        }
                    }
                }
                this._backgroundImage.UnlockBits(bData);
            }
        }








    }
}
