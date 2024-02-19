namespace evm_VISU
{
    partial class FormScope2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labe_info = new System.Windows.Forms.Label();
            this.zedGraph1 = new ZedGraph.ZedGraphControl();
            this.zedGraph_FFT = new ZedGraph.ZedGraphControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.button_refresh = new System.Windows.Forms.ToolStripButton();
            this.button_pause = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.button_pan = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel_insideflow = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.adr1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.adr2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label_log = new System.Windows.Forms.Label();
            this.label_chnls = new System.Windows.Forms.Label();
            this.label_setups = new System.Windows.Forms.Label();
            this.dataGridView_setups = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel_notify = new System.Windows.Forms.TableLayoutPanel();
            this.panel_notify = new System.Windows.Forms.Panel();
            this.label_notify = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel_insideflow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_setups)).BeginInit();
            this.tableLayoutPanel_notify.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel_notify, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(817, 523);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(223, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 497F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(591, 497);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labe_info);
            this.splitContainer1.Panel1.Controls.Add(this.zedGraph1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AllowDrop = true;
            this.splitContainer1.Panel2.Controls.Add(this.zedGraph_FFT);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(585, 491);
            this.splitContainer1.SplitterDistance = 306;
            this.splitContainer1.TabIndex = 0;
            // 
            // labe_info
            // 
            this.labe_info.AutoSize = true;
            this.labe_info.BackColor = System.Drawing.Color.Transparent;
            this.labe_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labe_info.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labe_info.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labe_info.ForeColor = System.Drawing.Color.White;
            this.labe_info.Location = new System.Drawing.Point(0, 0);
            this.labe_info.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labe_info.Name = "labe_info";
            this.labe_info.Size = new System.Drawing.Size(133, 24);
            this.labe_info.TabIndex = 9;
            this.labe_info.Text = "Reading data";
            this.labe_info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zedGraph1
            // 
            this.zedGraph1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.zedGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraph1.Location = new System.Drawing.Point(0, 0);
            this.zedGraph1.Name = "zedGraph1";
            this.zedGraph1.ScrollGrace = 0D;
            this.zedGraph1.ScrollMaxX = 0D;
            this.zedGraph1.ScrollMaxY = 0D;
            this.zedGraph1.ScrollMaxY2 = 0D;
            this.zedGraph1.ScrollMinX = 0D;
            this.zedGraph1.ScrollMinY = 0D;
            this.zedGraph1.ScrollMinY2 = 0D;
            this.zedGraph1.Size = new System.Drawing.Size(585, 306);
            this.zedGraph1.TabIndex = 7;
            this.zedGraph1.UseExtendedPrintDialog = true;
            // 
            // zedGraph_FFT
            // 
            this.zedGraph_FFT.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.zedGraph_FFT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraph_FFT.Location = new System.Drawing.Point(0, 0);
            this.zedGraph_FFT.Name = "zedGraph_FFT";
            this.zedGraph_FFT.ScrollGrace = 0D;
            this.zedGraph_FFT.ScrollMaxX = 0D;
            this.zedGraph_FFT.ScrollMaxY = 0D;
            this.zedGraph_FFT.ScrollMaxY2 = 0D;
            this.zedGraph_FFT.ScrollMinX = 0D;
            this.zedGraph_FFT.ScrollMinY = 0D;
            this.zedGraph_FFT.ScrollMinY2 = 0D;
            this.zedGraph_FFT.Size = new System.Drawing.Size(585, 181);
            this.zedGraph_FFT.TabIndex = 8;
            this.zedGraph_FFT.UseExtendedPrintDialog = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.01002F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.98998F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(214, 497);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.button_refresh,
            this.button_pause,
            this.toolStripSeparator1,
            this.button_pan,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(214, 24);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 24);
            // 
            // button_refresh
            // 
            this.button_refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_refresh.Image = global::ScopeWin.Properties.Resources.refresh_ico;
            this.button_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(23, 21);
            this.button_refresh.Text = "toolStripButton1";
            this.button_refresh.ToolTipText = "Reconnect";
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // button_pause
            // 
            this.button_pause.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.button_pause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_pause.Image = global::ScopeWin.Properties.Resources.pause_ico;
            this.button_pause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_pause.Name = "button_pause";
            this.button_pause.Size = new System.Drawing.Size(23, 21);
            this.button_pause.Text = "toolStripButton3";
            this.button_pause.ToolTipText = "Pause";
            this.button_pause.Click += new System.EventHandler(this.button_pause_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 24);
            // 
            // button_pan
            // 
            this.button_pan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_pan.Image = global::ScopeWin.Properties.Resources.pan_ico;
            this.button_pan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_pan.Name = "button_pan";
            this.button_pan.Size = new System.Drawing.Size(23, 21);
            this.button_pan.Text = "toolStripButton2";
            this.button_pan.ToolTipText = "Pan";
            this.button_pan.Click += new System.EventHandler(this.button_pan_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::ScopeWin.Properties.Resources.FFT;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 21);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "FFT";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AllowDrop = true;
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel_insideflow);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 27);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(208, 467);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel_insideflow
            // 
            this.tableLayoutPanel_insideflow.ColumnCount = 1;
            this.tableLayoutPanel_insideflow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_insideflow.Controls.Add(this.dataGridView2, 0, 1);
            this.tableLayoutPanel_insideflow.Controls.Add(this.textBox1, 0, 5);
            this.tableLayoutPanel_insideflow.Controls.Add(this.label_log, 0, 4);
            this.tableLayoutPanel_insideflow.Controls.Add(this.label_chnls, 0, 0);
            this.tableLayoutPanel_insideflow.Controls.Add(this.label_setups, 0, 2);
            this.tableLayoutPanel_insideflow.Controls.Add(this.dataGridView_setups, 0, 3);
            this.tableLayoutPanel_insideflow.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel_insideflow.Name = "tableLayoutPanel_insideflow";
            this.tableLayoutPanel_insideflow.RowCount = 6;
            this.tableLayoutPanel_insideflow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_insideflow.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_insideflow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_insideflow.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_insideflow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_insideflow.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_insideflow.Size = new System.Drawing.Size(184, 586);
            this.tableLayoutPanel_insideflow.TabIndex = 3;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2.ColumnHeadersVisible = false;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.adr1,
            this.adr2});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 23);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dataGridView2.ShowRowErrors = false;
            this.dataGridView2.Size = new System.Drawing.Size(178, 275);
            this.dataGridView2.TabIndex = 22;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.LemonChiffon;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.ForestGreen;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.LemonChiffon;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn1.HeaderText = "Par";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.LemonChiffon;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.ForestGreen;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.LemonChiffon;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn2.HeaderText = "Val";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 20;
            // 
            // adr1
            // 
            this.adr1.HeaderText = "adr1";
            this.adr1.Name = "adr1";
            this.adr1.Visible = false;
            // 
            // adr2
            // 
            this.adr2.HeaderText = "adr2";
            this.adr2.Name = "adr2";
            this.adr2.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox1.Location = new System.Drawing.Point(3, 524);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(178, 59);
            this.textBox1.TabIndex = 20;
            // 
            // label_log
            // 
            this.label_log.AutoSize = true;
            this.label_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_log.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_log.ForeColor = System.Drawing.SystemColors.Info;
            this.label_log.Location = new System.Drawing.Point(3, 501);
            this.label_log.Name = "label_log";
            this.label_log.Size = new System.Drawing.Size(178, 20);
            this.label_log.TabIndex = 19;
            this.label_log.Text = "▲ Scope log";
            this.label_log.Click += new System.EventHandler(this.label3_Click);
            // 
            // label_chnls
            // 
            this.label_chnls.AutoSize = true;
            this.label_chnls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_chnls.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_chnls.ForeColor = System.Drawing.SystemColors.Info;
            this.label_chnls.Location = new System.Drawing.Point(3, 0);
            this.label_chnls.Name = "label_chnls";
            this.label_chnls.Size = new System.Drawing.Size(178, 20);
            this.label_chnls.TabIndex = 17;
            this.label_chnls.Text = "▼ Channels";
            this.label_chnls.Click += new System.EventHandler(this.label2_Click);
            // 
            // label_setups
            // 
            this.label_setups.AutoSize = true;
            this.label_setups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_setups.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_setups.ForeColor = System.Drawing.SystemColors.Info;
            this.label_setups.Location = new System.Drawing.Point(3, 301);
            this.label_setups.Name = "label_setups";
            this.label_setups.Size = new System.Drawing.Size(178, 20);
            this.label_setups.TabIndex = 12;
            this.label_setups.Text = "▲Setups";
            this.label_setups.Click += new System.EventHandler(this.label1_Click);
            // 
            // dataGridView_setups
            // 
            this.dataGridView_setups.AllowUserToAddRows = false;
            this.dataGridView_setups.AllowUserToDeleteRows = false;
            this.dataGridView_setups.AllowUserToResizeColumns = false;
            this.dataGridView_setups.AllowUserToResizeRows = false;
            this.dataGridView_setups.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridView_setups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_setups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_setups.Location = new System.Drawing.Point(3, 324);
            this.dataGridView_setups.Name = "dataGridView_setups";
            this.dataGridView_setups.Size = new System.Drawing.Size(178, 174);
            this.dataGridView_setups.TabIndex = 21;
            // 
            // tableLayoutPanel_notify
            // 
            this.tableLayoutPanel_notify.ColumnCount = 2;
            this.tableLayoutPanel_notify.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_notify.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_notify.Controls.Add(this.panel_notify, 0, 0);
            this.tableLayoutPanel_notify.Controls.Add(this.label_notify, 1, 0);
            this.tableLayoutPanel_notify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_notify.Location = new System.Drawing.Point(3, 506);
            this.tableLayoutPanel_notify.Name = "tableLayoutPanel_notify";
            this.tableLayoutPanel_notify.RowCount = 1;
            this.tableLayoutPanel_notify.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_notify.Size = new System.Drawing.Size(214, 14);
            this.tableLayoutPanel_notify.TabIndex = 3;
            this.tableLayoutPanel_notify.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel_notify_Paint);
            // 
            // panel_notify
            // 
            this.panel_notify.BackgroundImage = global::ScopeWin.Properties.Resources.Red_X_in_Circle_7;
            this.panel_notify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_notify.Location = new System.Drawing.Point(3, 3);
            this.panel_notify.Name = "panel_notify";
            this.panel_notify.Size = new System.Drawing.Size(8, 8);
            this.panel_notify.TabIndex = 4;
            // 
            // label_notify
            // 
            this.label_notify.AutoSize = true;
            this.label_notify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_notify.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_notify.Location = new System.Drawing.Point(23, 0);
            this.label_notify.Name = "label_notify";
            this.label_notify.Size = new System.Drawing.Size(188, 14);
            this.label_notify.TabIndex = 5;
            this.label_notify.Text = "Disconnected";
            this.label_notify.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "adr1";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "adr2";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Column2";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // FormScope2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(817, 523);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormScope2";
            this.Text = "MCU Scope";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThisFormClosing);
            this.Shown += new System.EventHandler(this.FormShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel_insideflow.ResumeLayout(false);
            this.tableLayoutPanel_insideflow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_setups)).EndInit();
            this.tableLayoutPanel_notify.ResumeLayout(false);
            this.tableLayoutPanel_notify.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton button_refresh;
        private System.Windows.Forms.ToolStripButton button_pan;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton button_pause;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_insideflow;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label_log;
        private System.Windows.Forms.Label label_setups;
        private System.Windows.Forms.DataGridView dataGridView_setups;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Label label_chnls;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn adr1;
        private System.Windows.Forms.DataGridViewTextBoxColumn adr2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_notify;
        private System.Windows.Forms.Panel panel_notify;
        private System.Windows.Forms.Label label_notify;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ZedGraph.ZedGraphControl zedGraph1;
        private ZedGraph.ZedGraphControl zedGraph_FFT;
        private System.Windows.Forms.Label labe_info;
    }
}