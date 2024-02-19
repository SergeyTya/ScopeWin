using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;

namespace evm_VISU
{
    internal class CustomDropDown
    {
        Dictionary<Label, Panel> controls;
        Control container;
        int cnt = 0;

        TableLayoutPanel tableLayoutPanel_TabPanelMain = new TableLayoutPanel();
  
        public CustomDropDown(int count, Control container)
        {
            cnt = count;
            this.container = container;
            container.SuspendLayout();
            container.BackColor = style.GetBackColor();
            tableLayoutPanel_TabPanelMain.SuspendLayout();
            controls = new Dictionary<Label, Panel>(count);

            for (int i = 0; i < count; i++)
            {
                Label label = new Label();
                Panel panel = new Panel();
                label.Font = new Font(container.Font, FontStyle.Underline);
                label.Click += label_Tab_Click;
                label.Text = String.Format("Tab {0} ", i);
                label.Dock = System.Windows.Forms.DockStyle.Fill;
                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                panel.BackColor = style.GetBackColor();
                controls.Add(label, panel);
                close(label);
            }
            container.Controls.Clear();



            // Main
            this.tableLayoutPanel_TabPanelMain.ColumnCount = 1;
            this.tableLayoutPanel_TabPanelMain.RowCount = count*2+1;

            this.tableLayoutPanel_TabPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_TabPanelMain.Location = new System.Drawing.Point(3, 87);
            this.tableLayoutPanel_TabPanelMain.Name = "tableLayoutPanel_TabPanelMain";
            this.tableLayoutPanel_TabPanelMain.Size = new System.Drawing.Size(184, 586);
            this.tableLayoutPanel_TabPanelMain.TabIndex = 4;
            this.tableLayoutPanel_TabPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_TabPanelMain.RowStyles.Clear();
            //this.tableLayoutPanel_TabPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));

            int k = 0;
            foreach (KeyValuePair<Label, Panel> el in controls)
            {
                el.Key.TextAlign = ContentAlignment.TopLeft;
                RowStyle rowHeaderStyle = new RowStyle();
                rowHeaderStyle.SizeType = SizeType.Absolute;
                rowHeaderStyle.Height = 20;
                RowStyle rowPanelStyle = new RowStyle();
                rowPanelStyle.SizeType = SizeType.AutoSize;
                rowPanelStyle.Height = 500;


                this.tableLayoutPanel_TabPanelMain.Controls.Add(el.Key,   0, k++); 
                this.tableLayoutPanel_TabPanelMain.RowStyles.Add(rowHeaderStyle);
                this.tableLayoutPanel_TabPanelMain.Controls.Add(el.Value, 0, k++);
                this.tableLayoutPanel_TabPanelMain.RowStyles.Add(rowPanelStyle);
            }
            this.tableLayoutPanel_TabPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel_TabPanelMain.AutoScroll = true;    

            
            container.Controls.Add(tableLayoutPanel_TabPanelMain);
            tableLayoutPanel_TabPanelMain.ResumeLayout(true);

            container.ResumeLayout(false);
            container.PerformLayout();

        }

        void label_Tab_Click(object sender, EventArgs e){
            Label lb = (Label)sender;
            if (controls[lb].Visible){
                close(lb);
            }
            else {
                open(lb);   
            }
        }

        void open(Label lb) {
            controls[lb].Visible = true;
            lb.Text = lb.Text.Trim('▲') + '▼';
            if (controls[lb].Controls.Count != 0)
            {
               // controls[lb].Controls[0].Height = 1000;
                controls[lb].Height = container.Height;
            }
            container.PerformLayout();

        }

        void close(Label lb) {
            controls[lb].Visible = false;
            lb.Text = lb.Text.Trim('▼') + '▲';
        }

        public void addControls(Control control, int index, string name = null)
        {
            int i = 0;
            foreach (KeyValuePair<Label, Panel> el in controls)
            {
                if (i == index)
                {
                    control.Dock = DockStyle.Fill;
                    el.Value.Controls.Add(control);
                    control.Width = container.Width - 150;
                    control.Height = container.Height-100;
                    // el.Value.AutoSize = true;
                    if (name != null)
                    {
                        el.Key.Text = name+ " ▲";
                    }
                }
                i++;
            }
        }


    }
}
