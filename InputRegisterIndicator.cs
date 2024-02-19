using evm_VISU;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public class InputRegisterIndicator
    {

        public Label label;
        public TextBox indicator;
        public FormChart2 chart;
        private float _value = 0;

        private string RegSing = " ";

        public string Name
        { // name of input register
            get { return label.Text; }
            set
            {
                label.Text = RegSing + " " + value;
                chart.Label = label.Text;
            }
        }

        public bool RegSingEnable
        {

            set
            {
                if (value) RegSing = "adr." + Adr.ToString();
                if (!value) RegSing = "";
            }
        }

        private int scale;
        public int Scale
        {
            get { return scale; }
            set { scale = value; if (scale <= 0) scale = 1; }
        }

        public int Adr { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string Dimension { get; set; }

        public float value
        {
            get { return _value; }
            set
            {
                _value = (float)value / Scale;
                indicator.Text = _value.ToString("#####0.##");

                //if (Scale == 1) { indicator.Text = value.ToString(); }
                //else
                //{
                //    indicator.Text = temp.ToString("#####0.0") + " " + Dimension;
                //}

                chart.AddPoint(value);
                indicator.ForeColor = Color.LightGreen;
                if (_value < Min) indicator.ForeColor = Color.Cyan;
                if (_value > Max) indicator.ForeColor = Color.LightCoral;
            }
        }


        public InputRegisterIndicator(int pos)
        {

            Adr = pos;
            chart = new FormChart2(Adr - 3);
            Scale = 1;
            Min = -1;
            Max = 1;
            RegSingEnable = true;

            var max_size = new System.Drawing.Size(150, 150); 

            label = new Label();
            label.Dock = DockStyle.Fill;
            label.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            Name = "";
            label.ForeColor = Color.LemonChiffon;
            label.AutoSize = true;
            label.MaximumSize = max_size;

            indicator = new TextBox();
            indicator.Name = "IRTextBox_" + Adr.ToString("D2");
            indicator.Dock = DockStyle.Fill;
            indicator.BackColor = System.Drawing.SystemColors.MenuText;
            indicator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            indicator.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            indicator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            indicator.HideSelection = false;
            indicator.Location = new System.Drawing.Point(15, 203);
            indicator.Margin = new System.Windows.Forms.Padding(15, 3, 50, 3);
            indicator.MaximumSize = max_size;
            indicator.ReadOnly = false;
            indicator.Size = new System.Drawing.Size(50, 33);
            indicator.TabIndex = 6;
            indicator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            indicator.Click += new System.EventHandler((s, e) =>
            {
                this.chart.Show();
                this.chart.BringToFront();
            });

            
        }
    }


}
