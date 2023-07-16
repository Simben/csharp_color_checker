using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorPickers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float  toto = -55.535353f;
            MessageBox.Show(((int)toto).ToString());
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.chroma_Circle1.HSV_Value = (float)this.trackBar1.Value / 100.0f;
        }

        private void chroma_Circle1_SelectedColorChanged(object sender, bool isValid, ColorPickers_Controls.Model.Color_RGB rgb, ColorPickers_Controls.Model.Color_HSV hsv)
        {
            if (isValid)
            {
                this.textBox1.Text = rgb.R.ToString();
                this.textBox2.Text = rgb.G.ToString();
                this.textBox3.Text = rgb.B.ToString();
            }
            else
            {
                this.textBox1.Text = "0";
                this.textBox2.Text = "0";
                this.textBox3.Text = "0";
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.chroma_Circle1.SaturationEmphasis = this.trackBar2.Value / 100.0f;
        }
    }
}
