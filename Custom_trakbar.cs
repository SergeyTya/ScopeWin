using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evm_visu_net
{
    internal class Custom_trakbar
    {
        private Label label = new Label();
        private TextBox textBox = new TextBox();
        private TrackBar trackBar = new TrackBar();

        private int _value = 0;
        public int Value 
        { get { return _value; } set { textBox.Text = value.ToString(); trackBar.Value = value; _value = value; } }

        private bool _enable = true;
        public bool Enable { 
            set {
                _enable = value;
                if (_enable){
                    textBox.Enabled = true;
                    trackBar.Enabled = true;
                }
                else {
                    textBox.Enabled = false;
                    trackBar.Enabled = false;
                }
            } 
            get {
                return _enable;
            } 
        }

        public Custom_trakbar(Control parent, int min=-100, int max=100) {

          
            trackBar.Dock = DockStyle.Fill;
            trackBar.TabIndex = 4;
            trackBar.Minimum = min;
            trackBar.Maximum = max;

            textBox.Dock = DockStyle.Fill;
            textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            textBox.AutoSize = true;


            textBox.Text = "0";

            textBox.KeyUp   += textbox_tracbar_textchanged;
            trackBar.ValueChanged += (_, __) => { textBox.Text = ((TrackBar)_).Value.ToString(); _value = ((TrackBar)_).Value; };

                TableLayoutPanel main_panel = new TableLayoutPanel();
            main_panel.Dock = DockStyle.Fill;

            TableLayoutPanel header_panel = new TableLayoutPanel();
            header_panel.Dock = DockStyle.Fill;

            header_panel.ColumnStyles.Add(new ColumnStyle
            {
                SizeType = SizeType.Percent,
                Width = 0.7f,
            });
            header_panel.ColumnStyles.Add(new ColumnStyle
            {
                SizeType = SizeType.Percent,
                Width = 0.3f,
            });
            header_panel.Controls.Add(textBox, 1, 0);
            header_panel.Controls.Add(trackBar, 0, 0);
            

            main_panel.Controls.Add(header_panel, 0, 0);
          //  main_panel.Controls.Add(trackBar, 0, 1);

            parent.Controls.Add(main_panel);
        }

        void textbox_tracbar_textchanged(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode != Keys.Enter) return;
            var val = ((TextBox)sender).Text;
            int int_val = 0;
            bool lres = int.TryParse(val, out int_val);
            if (!lres)
            {
                ((TextBox)sender).Text = trackBar.Value.ToString();
            }
            else
            {
                if (int_val > trackBar.Maximum) int_val = trackBar.Maximum;
                if (int_val < trackBar.Minimum) int_val = trackBar.Minimum;

                trackBar.Value = int_val;

                ((TextBox)sender).Text = int_val.ToString();
                _value = int_val;
            }
        }
    }


}
