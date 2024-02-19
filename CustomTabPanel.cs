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
    internal class CustomTabPanel
    {

        Label active_label;
        Dictionary<Label, Panel> controls;
        Control container;

        TableLayoutPanel tableLayoutPanel_TabPanelHeader = new TableLayoutPanel();
        TableLayoutPanel tableLayoutPanel_TabPanelTabs = new TableLayoutPanel();
        TableLayoutPanel tableLayoutPanel_TabPanelMain = new TableLayoutPanel();

        public CustomTabPanel(int count, Control container) {
            container.SuspendLayout();
            container.BackColor = style.GetBackColor();
            this.container = container;
            controls = new Dictionary<Label, Panel>(count);
            int i  = 0; 
            for ( i = 0; i < count; i++ )
            {
                Label label = new Label();
                Panel panel = new Panel();
                label.Click += label_Tab_Click;
                label.Text = String.Format("Tab {0}", i);
                label.Dock = System.Windows.Forms.DockStyle.Fill;
                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                panel.BackColor = style.GetBackColor(); 
                controls.Add(label, panel);

                var el = new KeyValuePair<Label, Panel>(label, panel);
                if (i == 0)
                {
                    make_active(el.Key);
                }
                else {
                    make_notactive(el.Key);
                }
            }
            container.Controls.Clear();

            //Tabs
            this.tableLayoutPanel_TabPanelTabs.ColumnCount = count;
            this.tableLayoutPanel_TabPanelTabs.RowCount = 1;
            this.tableLayoutPanel_TabPanelTabs.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel_TabPanelTabs.Name = "TabPanelTabs";
            this.tableLayoutPanel_TabPanelTabs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 100F));
            this.tableLayoutPanel_TabPanelTabs.TabIndex = 1;
           // tableLayoutPanel_TabPanelTabs.Size = new Size(1000, 1000);
            i = 0; foreach (KeyValuePair<Label, Panel> el in controls) {
                ColumnStyle col = new ColumnStyle();
                col.SizeType = SizeType.AutoSize;
                this.tableLayoutPanel_TabPanelTabs.ColumnStyles.Add(col);
                this.tableLayoutPanel_TabPanelTabs.Controls.Add(el.Value, i, 0);
                i++;
            }
            tableLayoutPanel_TabPanelTabs.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel_TabPanelTabs.AutoSize = true;


            //Headers
            this.tableLayoutPanel_TabPanelHeader.ColumnCount = count+1;
            this.tableLayoutPanel_TabPanelHeader.RowCount = 1;
            this.tableLayoutPanel_TabPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));         
            this.tableLayoutPanel_TabPanelHeader.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel_TabPanelHeader.Name = "TabPanelHeader";
           

            this.tableLayoutPanel_TabPanelHeader.Size = new System.Drawing.Size(537, 19);
            this.tableLayoutPanel_TabPanelHeader.TabIndex = 0;
            i = 0; foreach (KeyValuePair<Label, Panel> el in controls)
            {
                el.Key.TextAlign = ContentAlignment.BottomCenter;
                this.tableLayoutPanel_TabPanelHeader.Controls.Add(el.Key, i, 0);
                ColumnStyle col = new ColumnStyle();
                col.SizeType    = SizeType.Absolute;
                col.Width = 70;
                
                
                this.tableLayoutPanel_TabPanelHeader.ColumnStyles.Add(col);
                i++;
            }
            tableLayoutPanel_TabPanelHeader.BackColor = Color.Black;
    
            // Main
            this.tableLayoutPanel_TabPanelMain.ColumnCount = 1;
          
            this.tableLayoutPanel_TabPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_TabPanelMain.Controls.Add(this.tableLayoutPanel_TabPanelHeader, 0, 0);
            this.tableLayoutPanel_TabPanelMain.Controls.Add(this.tableLayoutPanel_TabPanelTabs, 0, 1);
            this.tableLayoutPanel_TabPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_TabPanelMain.Location = new System.Drawing.Point(3, 87);
            this.tableLayoutPanel_TabPanelMain.Name = "tableLayoutPanel_TabPanelMain";
            this.tableLayoutPanel_TabPanelMain.RowCount = 2;
            this.tableLayoutPanel_TabPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel_TabPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_TabPanelMain.Size = new System.Drawing.Size(543, 393);
            this.tableLayoutPanel_TabPanelMain.TabIndex = 4;
            container.Controls.Add(tableLayoutPanel_TabPanelMain);

            this.tableLayoutPanel_TabPanelTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_TabPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_TabPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;

            container.ResumeLayout(false);
            container.PerformLayout();

            container.SizeChanged += (_, __) => {
                make_active(active_label);
            };
        }

        public void addControls(Control control, int index,string name = null)
        {
            int i = 0;
            foreach (KeyValuePair<Label, Panel> el in controls)
            {
                if (i == index) { 
                    control.Dock = DockStyle.Fill;
                    el.Value.Controls.Add(control);
                    if(name != null)
                    {
                        el.Key.Text = name;
                    }
                }
                i++;
            }
        }

        private void label_Tab_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<Label, Panel> el in controls)
            {
                make_notactive(el.Key);
            }

            make_active((Label)sender );

        }

        private void make_active(Label key) {
            var panel = controls[key];
            key.BackColor = style.GetBackColor();
            panel.Visible = true;
            //el.Key.ForeColor = Color.Red;
            Font fnt =new System.Drawing.Font(
                "Microsoft Sans Serif", 
                10, 
                System.Drawing.FontStyle.Underline, 
                System.Drawing.GraphicsUnit.Point, 
                ((byte)(204))
            );
            key.Font = fnt;
            panel.Width = container.Width - 10;
            //el.Value.AutoSize = true;
            active_label =key;

        }

        private void make_notactive(Label key)
        {
            var panel = controls[key];
            key.BackColor = Color.Black;
            key.ForeColor = container.ForeColor;
            panel.Visible = false;
            panel.AutoSize = false;
            panel.Width = 0;
            key.Font = container.Font;
        }


    }
}
