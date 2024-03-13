using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsyncSocketTest;
using static AsyncSocketTest.ServerModbusTCP;
using System.Diagnostics;
using ZedGraph;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;
using System.Numerics;
using TestGen;
using WindowsFormsApp4;
using ScopeWin.Properties;

namespace evm_VISU
{
    public partial class FormScope2 : Form
    {
       private Color[] colors = new Color[10] {
           System.Drawing.ColorTranslator.FromHtml("#FFFF11"),
           System.Drawing.ColorTranslator.FromHtml("#139FFF"),
           System.Drawing.ColorTranslator.FromHtml("#FF6929"),
           System.Drawing.ColorTranslator.FromHtml("#64D413"),
           System.Drawing.ColorTranslator.FromHtml("#B746FF"),
           Color.Green, 
           Color.Black, 
           Color.DarkGoldenrod, 
           Color.Chocolate, 
           Color.Azure, 
           };


        private ControlsTable controlTable_chnls;
        private ControlsTable controlTable_setups;

        private ServerModbusTCP? server;
        private delegate void MyDelegate();

        private ConnectionSetups connection_setups = new ConnectionSetups();

        Form Form_ChannelSetup = new System.Windows.Forms.Form();


        double _time = 0;

        double _time_step = 1.0;

        private System.Timers.Timer _timer;

        private bool _paused = false;
        private bool Paused { 
            get { return _paused; } 
            set {
                _paused = value; 
                chnls.AddMissingFrame();
                zedGraph1.IsShowPointValues = value;
            }
        }



        Scope_ch chnls;


        private static string gainName   = "Gain Ch{0}";
        private static string offsetName = "Offset Ch{0}";
        private static string timeScaleName = "Time line, s";

        //private FormChSetup formChSetup;

        private int task_cnt = 0;
        private int ch_in_use = 0;
        Notify_bar notify_bar;

        public FormScope2()
        {
            chnls = new Scope_ch();
            InitializeComponent();
            notify_bar = new Notify_bar(label_notify, panel_notify);

            zedGraph_PrepareGraph();
            //CreateControlTable1(dataGridView1, chnls);
            get_channels();

            //formChSetup = new FormChSetup(chnls, controlTable1);
            //formChSetup.OnTimeScaleChange += TimeScaleValueChenged;

            //setups table
            //    controlTable_setups = new ControlsTable(dataGridView_setups);

            DataGridViewCellStyle CellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridView_setups.AllowUserToAddRows = false;
            dataGridView_setups.AllowUserToDeleteRows = false;
            dataGridView_setups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView_setups.ColumnHeadersVisible = false;
            var cell1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            var cell2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            CellStyle.BackColor = System.Drawing.Color.Black;
            CellStyle.ForeColor = System.Drawing.Color.LemonChiffon;
            CellStyle.SelectionBackColor = System.Drawing.Color.ForestGreen;
            CellStyle.SelectionForeColor = System.Drawing.Color.LemonChiffon;
            cell1.DefaultCellStyle = CellStyle;
            cell2.DefaultCellStyle = CellStyle;
            cell1.Width = 105;
            cell2.Width = 70;
            dataGridView_setups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cell1, cell2 });
            dataGridView_setups.RowHeadersVisible = false;
            dataGridView_setups.ShowRowErrors = false;
            dataGridView_setups.TabIndex = 1;
            dataGridView_setups.Dock = System.Windows.Forms.DockStyle.Fill;


            dataGridView_setups.Visible = false;
            textBox1.Visible = false;

            labe_info.Parent = zedGraph1;
            labe_info.BackColor = System.Drawing.Color.Transparent;
            labe_info.AutoSize = false;

            splitContainer1.Panel2Collapsed = true;

            _ = Task.Run(async () =>
            {
                if (server == null) return;
                try
                {
                  //  server?.close();
                  //  server = new ServerModbusTCP(connection_setups.ServerName, connection_setups.ServerPort);
                    await server.SendRawDataAsync(new byte[] { 0, 0, 0, 0, 0, 4, (byte)connection_setups.SlaveAdr, 25, 0, 3 });
                   // server.close();
                }
                catch (ServerModbusTCPException ex) { };
            });


            _ = Task.Run(async () =>
            {
                while(true)
                {
                    BeginInvoke(new MyDelegate(() => {

                        if (server == null || !server.Connected)
                        {
                            notify_bar.setDiconectedState();
                        }
                        else
                        {
                            notify_bar.setConnectedState();
                        }

                        if (Paused) notify_bar.setPausedState();

                    }));

                    await Task.Delay(500);
                }
            });
        }

        private void UpdateSetupsTable()
        {
            double time = GetTimeScaleValue();
            dataGridView_setups.Rows.Clear();
            chnls.ResetGainsOffsets();
            controlTable_setups = new ControlsTable(dataGridView_setups);
           
            // Развертка
            CustomControl tmp = new CustomControl(
               timeScaleName,
               max: 100,
               min: 0,
               def: 0.1
            );
            tmp.OnControlEvetn += TimeScaleValueChenged;
            tmp.Value = time;
            controlTable_setups.addControl(tmp);


            // Усиление
            Action GainEvent = () => {
                double[] tmp_gain = new double[Scope_ch.ch_num_max];
                for (int i = 0; i < Scope_ch.ch_num_max; i++)
                {
                    double val = 1;
                    try
                    {
                        val = controlTable_setups.getControlValue(String.Format(gainName, i));
                    }
                    catch (KeyNotFoundException ex) { }
                    tmp_gain[i] = val;
                }
                chnls.SetGains(tmp_gain);
            };

            // Смещение
            Action OffsetEvent = () => {
                double[] tmp_offset = new double[Scope_ch.ch_num_max];
                for (int i = 0; i < Scope_ch.ch_num_max; i++)
                {
                    double val = 0;
                    try
                    {
                        val = controlTable_setups.getControlValue(String.Format(offsetName, i));
                    }
                    catch (KeyNotFoundException ex) { }
                    tmp_offset[i] = val;
                }
                chnls.SetOffsets(tmp_offset);
            };

            int k = 0;
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                if (item.Cells[1].Style.BackColor != Color.Black) {
                    tmp = new CustomControl(String.Format(gainName, k), max: 1000.0, min: 0.0, def: 1.0);
                    tmp.OnControlEvetn += GainEvent.Invoke;
                    controlTable_setups.addControl(tmp);
                    

                    //dataGridView_setups.Rows[dataGridView_setups.Rows.Count - 1].Cells[0].Style.BackColor = item.Cells[1].Style.BackColor;

                    tmp = new CustomControl(String.Format(offsetName, k), max: 100000.0, min: -100000.0, def: 0.0);
                    tmp.OnControlEvetn += OffsetEvent.Invoke;
                    controlTable_setups.addControl(tmp);

                    //dataGridView_setups.Rows[dataGridView_setups.Rows.Count - 1].Cells[0].Style.BackColor = item.Cells[1].Style.BackColor;

                    k++;
                }

                
            }

            
            controlTable_setups.RenderTable();

        }

        private void log_data(string msg) {
            var mes = Environment.NewLine + DateTime.Now.ToLongTimeString() + " " + msg;
            try
            {
                BeginInvoke(new MyDelegate(() => {

                    this.textBox1.AppendText(mes);

                }));
            }
            catch (System.InvalidOperationException ex)
            {
               // MessageBox.Show(mes);
            }
        }



        private void TimerCallback()
        {
            if (server == null) {
                _timer.Stop();
                return;
            }
            
            if(task_cnt > 1) {
                _timer.Stop();
                _time_step = 3.0;
                TimerStart();
                log_data("Scope tasks in wait");
                return; 
            };

            if (ch_in_use == 0)
            {
                _timer.Stop();
                BeginInvoke(new MyDelegate(() => {
                    labe_info.Visible = true;
                    labe_info.Text = "Scope disabled";
                }));
                log_data("Scope disabled");
                return;
            }

            if (refres_channels == true)
            {
                BeginInvoke(new MyDelegate(() => {
                    labe_info.Visible = true;
                    labe_info.Text = "Refresh channels";
                }));

            }
            else
            {
                BeginInvoke(new MyDelegate(() =>
                {
                    labe_info.Visible = false;
                }));
            }

            if (!Paused)
            {
                double timeScaleValue = GetTimeScaleValue();

                if (timeScaleValue > 3.0)
                {
                    downSampleCnt++;
                    if (downSampleCnt > 3)
                    {
                        zedGraph_updateGraph();
                        downSampleCnt = 0;
                    }
                }
                else
                {
                    zedGraph_updateGraph();
                }

                if (splitContainer1.Panel2Collapsed == false && chnls.GetChannelData(0).Capacity != 0)
                {
                    //FFT ch0
                    perform_FFT(chnls.GetChannelData(0), fft_window_size);
                }

            }
            else {
                BeginInvoke(new MyDelegate(() => {
                    labe_info.Visible = true;
                    labe_info.Text = "Scope paused";
                }));
            }


            task_cnt++;
            _ = Task.Run(async () =>
            {
                try
                {
                    bool newResValue;
                    var myTask = OneTimeReadScopeAsync();
                    if (await Task.WhenAny(myTask, Task.Delay((int)(2000))) == myTask)
                    {
                 
                        newResValue = myTask.Result;

                        if (newResValue == false | Paused)
                        {
                            task_cnt--;
                            return;
                        };

                        if (_time_step != chnls._frame_time)
                        {
                            _timer.Stop();
                            _time_step = chnls._frame_time * 0.95;
                            TimerStart();
                        }


                   
                     }else {
                        _time += chnls.AddMissingFrame();
                        //log_data(String.Format("Time out: FrameStep={0} TimeStep={1} ", chnls._frame_time, chnls._chnls_time_step));
                        log_data(String.Format("Time out"));
                    }
                    task_cnt--;
                }
                catch (System.AggregateException ex)
                {
                  //  Debug.WriteLine(ex.ToString()); 
                    log_data("Server exception, connection closed");
                    //server.close();
                    //server = null;
                    BeginInvoke(new MyDelegate(() => {
                        labe_info.Visible = true;
                        labe_info.Text = "Connection lost";
                    }));
                   
                }
            });


            BeginInvoke(new MyDelegate(() => {

                if (!Paused)
                {
                    button_pause.Image = Resources.pause_ico;
                }
                else
                {
                    button_pause.Image = Resources.play_ico;
                }

            }));
           
        }


        static int fft_window_type = 0;
        static int fft_window_size = 4096;
        private void perform_FFT(RollingPointPairList data, int window_size=1024) {

            List<double> tmp_d = new List<double>();
            try
            {

                int N = window_size;//(int)Math.Pow(2, 12);
                int N_data = data.Capacity;

                for (int i = 0; i < N_data; i++)
                {
                    double a = 0;
                    if (i < N_data)
                    {
                        a = data[i].Y;
                    }
                    tmp_d.Add(a);
                }
                double[] signal = tmp_d.ToArray();

                Complex[] windowed_signal;

                switch (fft_window_type) {
                    case 1:
                        windowed_signal = FFT.ApplyHammingWindow(signal, N);
                        break;
                    case 2:
                        windowed_signal = FFT.ApplyFlatTopWindow(signal, N);
                        break;
                    default:
                        windowed_signal = FFT.ApplyUniformWindow(signal, N);
                        break;
                }

                Complex[] fftResult = FFT.CooleyTukeyFFT(windowed_signal);

                data = new RollingPointPairList(fftResult.Length / 2);
                double max_Y = 0;
                double max_X = 0;
                double time = GetTimeScaleValue();
                double freq = 0;

                double sum = 0;
                for (int i = 0; i < fftResult.Length / 2; i++)
                {
                    double N_div = N_data / 2;
                    if (N_data > N) N_div = N / 2;
                    double amp = fftResult[i].Magnitude / N_div;
                    freq = i / (chnls._chnls_time_step * N);
                    if (amp > max_Y)
                    {
                        max_Y = amp;
                        max_X = freq;
                    }
                    data.Add(new PointPair(freq, amp));
                    sum += amp * amp;
                    i++;
                }


                double thd = Math.Sqrt((sum - max_Y * max_Y)) / max_Y;

                GraphPane pane1 = zedGraph_FFT.GraphPane;
                pane1.CurveList.Clear();
                pane1.GraphObjList.Clear();

                LineItem curve_top = pane1.AddCurve(" ", data, Color.Red, SymbolType.None);
                curve_top.Line.Fill = new ZedGraph.Fill(Color.Yellow, Color.White, 90.0f);
                //pane1.XAxis.Scale.Max = freq;
                //pane1.YAxis.Scale.Min = 0;
                //pane1.YAxis.Scale.Max = max_Y;

                if (zedGraph_FFT != null)
                {
     
                    zedGraph_FFT.AxisChange();
                    TextObj text1 = new TextObj(string.Format("Freq: {0}Hz\nAmp: {1}", max_X.ToString("0.00"), max_Y.ToString("0.00")), 4 * freq / 5, max_Y / 2);
                    text1.FontSpec.StringAlignment = StringAlignment.Near;
                    pane1.GraphObjList.Add(text1);

                    zedGraph_FFT.Invalidate();
                }
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
            }

        }

        public void TimerStart()
        {
           
            if (_timer != null) _timer.Stop();
            if (_time_step == 0) _time_step = 0.5;

            _timer = new System.Timers.Timer(  (_time_step * 700) );
            _timer.Elapsed += (_, __) => TimerCallback();
            _timer.Start();

        }

        public void TimerStop()
        {
            if (_timer is null) return;
            _timer.Stop();
          
        }

        private void ThisFormClosing(object sender, FormClosingEventArgs e)
        {

            if (server != null) if (server.Connected)
                {
                    server.close();
                }
            TimerStop();
        }


        private void ServerStart() {
            try
            {
                connection_setups = ConnectionSetups.read();
                server?.close();
                server = new ServerModbusTCP(connection_setups.ServerName, connection_setups.ServerPort);
                server.Timeout = 300;

            }
            catch (ServerModbusTCPException ex)
            {
                Debug.WriteLine(ex.Message);
                log_data(ex.Message);
            }
            TimerStart();
        }

        private void FormShown(object sender, EventArgs e)
        {
            ServerStart();
        }

        private void TimeScaleValueChenged() {
            double  newScaleValue = GetTimeScaleValue();

            int cap = (int)(newScaleValue / chnls._chnls_time_step);
            if (cap <= 0) {
                cap = 10000;
            }
            if (chnls.capacity == cap) {
                return;
            };

            chnls.capacity = cap;
            log_data("new TimeScale=" + newScaleValue);

            _timer.Stop();
            GraphPane pane = zedGraph1.GraphPane;
            pane.CurveList.Clear();


            _time = 0;
            zedGraph_createCruves();
            _timer.Start();
        }
      

        private int downSampleCnt = 0;

        private async Task<bool> OneTimeReadScopeAsync() {

            int frame_size = 240*4;
            int delay_pos = frame_size + 8;
            int chn_pos = frame_size + 9;
            int fifolen_pos = frame_size + 10;
            if(server is null) { 
                return false; 
            };
            try
            {
                byte[] req = new byte[] { 0, 0, 0, 0, 0, 2,(byte) connection_setups.SlaveAdr, 20 };
                var RXbuf = await server.SendRawDataAsync(req);

                // check function code
                if (RXbuf[7] != 20 )
                {
                    if (RXbuf[7] != 148)
                    {
                        _time += chnls.AddMissingFrame();
                        log_data("Frame: " + Encoding.UTF8.GetString(RXbuf).Trim('\0') + String.Format(": FrameStep={0} TimeStep={1}", chnls._frame_time, chnls._chnls_time_step));
                    }
                    else {
                        //log_data("Scope: FIFO empty");
                    }
                    return false;
                };


                // add data
                if (!Paused)
                {
                    _time = chnls.AddData(RXbuf, _time, frame_size);
                }
              
                if (RXbuf[fifolen_pos] == 6)
                {
                    log_data("Scope: FIFO full");
                    _time += chnls.AddMissingFrame();
                    await OneTimeReadScopeAsync();
                }

                return true;
            }
            catch (ServerModbusTCPException ex)
            {
                _time += chnls.AddMissingFrame();
                log_data(ex.Message);
                return false;
            }
        }


        private void zedGraph_updateGraph()
        {

            try
            {
                GraphPane pane1 = zedGraph1.GraphPane;
                if (pane1 is null) return;
                if (zedGraph1 == null) return;
                if (_time != 0)
                {
                    double xmin = _time - chnls.capacity * chnls._chnls_time_step;
                    double xmax = _time;
                    pane1.XAxis.Scale.Min = xmin;
                    pane1.XAxis.Scale.Max = xmax;

                }


                if (zedGraph1 != null)
                {
                    zedGraph1.AxisChange();
                    zedGraph1.Invalidate();
                }

            }
            catch (System.InvalidOperationException ex)
            {
                Debug.WriteLine(ex);
                log_data(ex.Message);
            }
            catch (System.NullReferenceException ex)
            {
                Debug.WriteLine(ex);
                log_data(ex.Message);
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                Debug.WriteLine(ex);
                log_data(ex.Message);
            }
            catch (System.ArgumentNullException ex)
            {
                Debug.WriteLine(ex);
                log_data(ex.Message);
            }
            catch (System.StackOverflowException ex)
            {
                Debug.WriteLine(ex);
                log_data(ex.Message);
            }

        }

        private void zedGraph_createCruves()
        {

            GraphPane pane1 = zedGraph1.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane1.CurveList.Clear();

            // Добавим кривую пока еще без каких-либо точек

            for (int i = 0; i < Scope_ch.ch_num_max; i++)
            {
                
                var myCurve = pane1.AddCurve(null, chnls.GetChannelData(i), colors[i], SymbolType.Circle);
                myCurve.Symbol.Size = 2;
            }


        }
        void zedGraph_applyStyle(ZedGraphControl zedGraph) {
            // Получим панель для рисования
            GraphPane pane1 = zedGraph.GraphPane;

            // Show the horizontal scrollbar
            zedGraph.IsShowHScrollBar = true;
            // Tell ZedGraph to automatically set the range of the scrollbar according to the data range
            zedGraph.IsAutoScrollRange = true;
            // Make the scroll cover slightly more than the range of the data
            // (This is a brand new property in the development version -- leave this line out if you are using an older one).
            zedGraph.ScrollGrace = .05;

            pane1.XAxis.Title.Text = null;
            pane1.YAxis.Title.Text = null;
            pane1.Title.Text = string.Empty;

            int labelsfontSize = 12;
            pane1.XAxis.Scale.FontSpec.Size = labelsfontSize;
            pane1.YAxis.Scale.FontSpec.Size = labelsfontSize;
            pane1.XAxis.Title.FontSpec.Size = labelsfontSize;
            pane1.Title.FontSpec.Size = labelsfontSize;
            pane1.IsFontsScaled = false;
            pane1.YAxis.Scale.MinAuto = true;
            pane1.YAxis.Scale.MaxAuto = true;
            pane1.IsBoundedRanges = true;
            pane1.YAxis.Scale.Min = -1000;
            pane1.YAxis.Scale.Max = 1000;

            pane1.Fill.Type = FillType.Solid;
            pane1.Fill.Color = System.Drawing.ColorTranslator.FromHtml("#282828");

            pane1.Chart.Fill.Type = FillType.Solid;
            pane1.Chart.Fill.Color = Color.Black;


            pane1.XAxis.MajorGrid.IsVisible = true;
            pane1.YAxis.MajorGrid.IsVisible = true;
            pane1.XAxis.MajorGrid.Color = System.Drawing.ColorTranslator.FromHtml("#464646");
            pane1.YAxis.MajorGrid.Color = System.Drawing.ColorTranslator.FromHtml("#464646");
            pane1.XAxis.MajorGrid.IsZeroLine = true;
            pane1.YAxis.MajorGrid.IsZeroLine = true;
            pane1.YAxis.MajorGrid.DashOff = 0;
            pane1.XAxis.MajorGrid.DashOff = 0;

            pane1.XAxis.MajorTic.Color = System.Drawing.ColorTranslator.FromHtml("#AFAFAF");
            pane1.YAxis.MajorTic.Color = System.Drawing.ColorTranslator.FromHtml("#AFAFAF");
            pane1.XAxis.MajorTic.IsInside = true;
            pane1.XAxis.MajorTic.IsOutside = false;
            pane1.YAxis.MajorTic.IsInside = true;
            pane1.YAxis.MajorTic.IsOutside = false;

            pane1.XAxis.Title.FontSpec.FontColor = System.Drawing.ColorTranslator.FromHtml("#888888");
            pane1.YAxis.Title.FontSpec.FontColor = System.Drawing.ColorTranslator.FromHtml("#888888");

            pane1.XAxis.Scale.FontSpec.FontColor = System.Drawing.ColorTranslator.FromHtml("#888888");
            pane1.YAxis.Scale.FontSpec.FontColor = System.Drawing.ColorTranslator.FromHtml("#888888");

            pane1.XAxis.Color = Color.Gray;
            pane1.YAxis.Color = Color.Gray;
           
            pane1.Border.Color = System.Drawing.ColorTranslator.FromHtml("#AFAFAF");
            pane1.Chart.Border.Color = System.Drawing.ColorTranslator.FromHtml("#AFAFAF");
            pane1.Legend.IsVisible = false;
        }

        private void zedGraph_PrepareGraph()
        {

            zedGraph_applyStyle(zedGraph1);
            zedGraph_applyStyle(zedGraph_FFT);


            zedGraph_FFT.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(
              (_, __, ___, ____) => {
                  int pow =(int) Math.Log(fft_window_size, 2);
                  pow += 1;
                  if (pow > 14) pow = 6;
                  int sz = (int)Math.Pow(2, pow); 
                  ToolStripItem newMenuItem = new ToolStripMenuItem(string.Format("Change window size [{0}->{1}]", fft_window_size, (int)Math.Pow(2, pow)));
                  newMenuItem.Click += (x, xx) => {
                      fft_window_size = sz;
                  };
                  __.Items.Add(newMenuItem);
              }
              );

            zedGraph_FFT.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(
                (_, __, ___, ____) => {
                    string name = null;
                    switch (fft_window_type) {
                        case 0:
                            name = "Uniform -> Hamming";
                            break;
                        case 1:
                            name = "Hamming -> Flat top";
                            break;
                        case 2:
                            name = "Flat top -> Uniform";
                            break;
                    }
                    ToolStripItem newMenuItem = new ToolStripMenuItem(string.Format("Change window type [{0}]", name));
                    newMenuItem.Click += (x, xx) => {
                        fft_window_type++;
                        if (fft_window_type == 3) fft_window_type = 0;
                    };
                __.Items.Add(newMenuItem);
                    }
                );



            

            zedGraph1.ContextMenuBuilder +=
            new ZedGraphControl.ContextMenuBuilderEventHandler(zedGraph_ContextMenuBuilder);
            zedGraph1.ZoomEvent +=
                new ZedGraph.ZedGraphControl.ZoomEventHandler(this.zedGraph_ZoomEvent);


            zedGraph_createCruves();

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            zedGraph1.AxisChange();

            // Обновляем график
            zedGraph1.Invalidate();

            chnls.onChanged += TimeScaleValueChenged;

        }

        private void zedGraph_ContextMenuBuilder(

           ZedGraphControl sender,
           ContextMenuStrip menuStrip,
           Point mousePt,
           ZedGraphControl.ContextMenuObjectState objState
           )
        {
            // Добавим свой пункт меню
            ToolStripItem newMenuItem1 = new ToolStripMenuItem("Pause");
            newMenuItem1.Click += (_, __) => { this.Paused = !this.Paused; };
            menuStrip.Items.Add(newMenuItem1);

            menuStrip.Items[7].Click += (_, __) => { this.Paused = false; };
        }

        private void zedGraph_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            //Paused = true;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (Paused) return;

                GraphPane Pane = new GraphPane();
                oldState.ApplyState(Pane);


                double zoom_val = -0.1;
                if (Pane.YAxis.Scale.Min > sender.GraphPane.YAxis.Scale.Min) {
                    zoom_val = -zoom_val;
                }
                double new_val = GetTimeScaleValue() + zoom_val* GetTimeScaleValue();
                if (new_val < 0) new_val = 0.1;
                controlTable_setups.setControlValue(timeScaleName, new_val);
                TimeScaleValueChenged();
                oldState.ApplyState(sender.GraphPane);


            }
        }

        private double GetTimeScaleValue() {
            double timeScaleValue = 0.1;
            if (controlTable_setups != null) timeScaleValue = controlTable_setups.getControlValue(timeScaleName);
            return timeScaleValue;
        }


        private void zedGraph_autoscale()
        {


            GraphPane pane = zedGraph1.GraphPane;

            zedGraph1.RestoreScale(pane);
           
            // Обновим данные об осях
            zedGraph1.AxisChange();

            // Обновляем график
            zedGraph1.Invalidate();

        }

        async private void get_channelsInuse() {

            //0x18 function - Get scope channels
            if(server == null) { return; }
            try
            {
               // server = new ServerModbusTCP(connection_setups.ServerName, connection_setups.ServerPort);

                var RXbuf = await server.SendRawDataAsync(new byte[] { 0, 0, 0, 0, 0, 2, (byte)connection_setups.SlaveAdr, 0x18 }); // get device holding count

                if (RXbuf[7] != 0x18)
                {
                    Debug.WriteLine(String.Format("0x18 Response error FC = {0}", RXbuf[7]));
                    return;
                }

                var ch_count = RXbuf[8];

                for (int i = 0; i < ch_count; i++)
                {
                    var adr = BitConverter.ToUInt32(RXbuf.ToArray(), 9+4*i);

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if(row is null) continue;
                        string mes = row.Cells[0].ToolTipText;
                        string? info = row.Cells[0].Value.ToString();
                        var cell_adr = Convert.ToUInt32(mes.Substring(0, 8), 16);
                        if (cell_adr == adr) {
                            if ((DataGridViewCheckBoxCell)row.Cells[1] != null) {
                                ((DataGridViewCheckBoxCell)row.Cells[1] ).Value = true;
                            }
                        }
                    }
                    Chanels_CellEndEdit();
                }
            }
            catch (ServerModbusTCPException ex)
            {
                Debug.WriteLine(ex.Message);
                log_data(ex.Message);
            }



        }


        static bool refres_channels = false;
        async private void get_channels() {

            /**
            *   27 function - Get register info

                   ________________________________REQUEST FRAME________________________________
                 *
                 *       +-----+----+-----------------+-----------+
                 * index | 0   | 1  |  2    |  3      |  4  |  5  |
                 *       +-----+----+-----------------+-----------+
                 * FRAME | ADR |CMD | ARD_HI| ARD_LO  |    CRC    |
                 *       +-----+----+-----------------+-----------+
                 *

                    _______________________________RESPONSE FRAME_________________________________
                 *
                 *       +-----+----+--------------+-------+----+-----+----+------------------+---------+
                 * index | 0   | 1  |   2  |   3   |4|5|6|7| 8  |  9  | 10 |                  |    |    |
                 *       +-----+----+--------------+-------+--- +-----+----+------------------+---------+
                 * FRAME | ADR |CMD |adr_hi|adr_lo |MEM_ADR|type|index| RO | info string      |   CRC   |
                 *       +-----+----+--------------+-------+----+-----+----+------------------+---------+
*/
            refres_channels = true;
            
            ServerStart();

            try
            {
               // server = new ServerModbusTCP(connection_setups.ServerName, connection_setups.ServerPort);

                var RXbuf = await server.SendRawDataAsync(new byte[] { 0, 0, 0, 0, 0, 2, (byte)connection_setups.SlaveAdr, 26 }); // get device holding count
                if (RXbuf[7] != 26)
                {
                    Debug.WriteLine(String.Format("Holding count response error FC = {0}", RXbuf[7]));
                    return;
                }

                var hreg_count = RXbuf[9] + (RXbuf[8] << 8);


                BeginInvoke(new MyDelegate(() =>
                {
                    controlTable_chnls = new ControlsTable(this.dataGridView2);
                }));

                for (int i = 0; i < hreg_count; i++)
                {
                    RXbuf = await server.SendRawDataAsync(new byte[] { 0, 0, 0, 0, 0, 4, (byte)connection_setups.SlaveAdr, 27, 0, (byte)i });

                    if (RXbuf[7] != 27)
                    {
                        Debug.WriteLine(String.Format("Holding info response error FC = {0}", RXbuf[7]));
                        return;
                    }

                    UInt32 adr = BitConverter.ToUInt32(RXbuf.ToArray(), 10);
                    //Debug.WriteLine(String.Format("HR location = {0}", adr));
                    var type = RXbuf[14];
                    Debug.WriteLine(String.Format("HR type = {0}", type));
                    var index = RXbuf[15];
                    //Debug.WriteLine(String.Format("HR index = {0}", index));
                    var isRO = RXbuf[16];
                    if(isRO == 0) continue;
                    string info = Encoding.UTF8.GetString(RXbuf.ToList().GetRange(17, RXbuf[5] - 11).ToArray());
                    Debug.WriteLine(String.Format("HR Info = {0}", info) + " \n");

                    var header = String.Format("{0} [{1}]", info, HoldingTypes.ToString((HoldingTypes.HoldingType)type));

                    string mes = Convert.ToString(adr, 16);
                    if (
                          (HoldingTypes.HoldingType) type != HoldingTypes.HoldingType.HTYPE_INT16 
                      &&  (HoldingTypes.HoldingType) type != HoldingTypes.HoldingType.HTYPE_UINT16
                     )
                    {
                        if (index > 0) { continue; }
                        mes = String.Format("{0}&{1}", Convert.ToString(adr, 16), Convert.ToString(adr + 2, 16));
                    }


                    CustomControl tmp_row = new CustomControl(header, checkedbox: true, msg: mes);
                    tmp_row.OnControlEvetn += Chanels_CellEndEdit;
                    controlTable_chnls.addControl(tmp_row);


                }

                controlTable_chnls.OnRender += (_) => { 
                   foreach(DataGridViewRow row in ((DataGridView) _).Rows){
                        if (row.Cells[1].ValueType.Name == "Boolean")
                        {
                            ((DataGridViewCheckBoxCell)row.Cells[1]).Style.BackColor = Color.Black;

                        }

                    } 
                
                };

                BeginInvoke(new MyDelegate(() =>
                {
                    controlTable_chnls.RenderTable();
                  //  server.close();
                    refres_channels = false;
                }));

            }
            catch (ServerModbusTCPException ex)
            {
                Debug.WriteLine(ex.Message);
                log_data(ex.Message);
                refres_channels = false;
                return;
            }



            BeginInvoke(new MyDelegate(() =>
            {
                log_data("Read scope channels");
                get_channelsInuse();
                UpdateSetupsTable();
             }));

            log_data("Restart channels");
            task_cnt = 0;
            _timer.Stop();
            await Task.Delay(1000);
            TimerStart();
            
            // button_refresh.Enabled = false;

        }

        private void Chanels_CellEndEdit()
        {   

            List<byte> req = new List<byte> { 0, 0, 0, 0, 0, 3+4* Scope_ch.ch_num_max, 1, 22, Scope_ch.ch_num_max};
            int ch_num = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row is null) continue;
                if ((row.Cells[1] as DataGridViewCheckBoxCell) == null) continue;

                if ((bool)(row.Cells[1] as DataGridViewCheckBoxCell).Value)
                {
                    if (ch_num < Scope_ch.ch_num_max)
                    {
                        row.Cells[1].Style.BackColor = colors[ch_num];
                        string mes = row.Cells[0].ToolTipText;
                        string? info = row.Cells[0].Value.ToString();
                        if (info == null) continue; 
                        byte[] adr_hi = BitConverter.GetBytes(Convert.ToInt32(mes.Substring(0, 8), 16));
                        
                        // restore channel type
                        string type = "";
                        try {
                            type = info.Split('[')[1].Split(']')[0]; // get type from name
                        }
                        catch (System.IndexOutOfRangeException ex){
                            Debug.WriteLine("Type not found");
                        }
                        HoldingTypes.HoldingType tmp_type = HoldingTypes.GetType(type); 
                        if (
                            tmp_type == HoldingTypes.HoldingType.HTYPE_UINT16 // checking if space is enough for 32bit var
                            || tmp_type == HoldingTypes.HoldingType.HTYPE_INT16
                            || (Scope_ch.ch_num_max - ch_num) >= 2
                            )
                        {
                            chnls.SetChnType(ch_num, tmp_type);
                            req.AddRange(adr_hi);
                            ch_num++;
                            // ToolTipText contatain 16 variebale adress if it 32 bit addresses can splited by & 
                            if (mes.IndexOf('&') != -1) // needed to detect  32 bit numbers
                            {
                                byte[] adr_lo = BitConverter.GetBytes(Convert.ToInt32(mes.Substring(9, 8), 16));
                                req.AddRange(adr_lo);
                                chnls.SetChnType(ch_num, HoldingTypes.HoldingType.NONE);
                                ch_num++;
                            }
                        }
                        else {
                            (row.Cells[1] as DataGridViewCheckBoxCell).Value = false;
                            row.Cells[1].Style.BackColor = Color.Black;
                        }
                        
                    }
                    else
                    {
                        (row.Cells[1] as DataGridViewCheckBoxCell).Value = false;
                        row.Cells[1].Style.BackColor = Color.Black;
                    }
                }
                else
                {
                    row.Cells[1].Style.BackColor = Color.Black;
                }
               

            }
            int req_count = (6+3+4* Scope_ch.ch_num_max) - req.Count;
            for (int i = 0; i < req_count; i++) { req.Add(0); };

            _ = Task.Run(async () =>
            {

                try
                {
                    //if (server is null) return;
                    var tmp = new ServerModbusTCP(connection_setups.ServerName, connection_setups.ServerPort);
                    var RXbuf = await tmp.SendRawDataAsync(req.ToArray());


                    int freq = (byte)(ch_num) - 3;
                    if (freq < 0) freq = 0;
                    //  if (ind.Count <= 1) freq = 0;
                    req = new List<byte> { 0, 0, 0, 0, 0, 4, (byte)connection_setups.SlaveAdr, 25, (byte)(ch_num), (byte)freq };
                    RXbuf = await tmp.SendRawDataAsync(req.ToArray());
                    tmp.close();
                    TimeScaleValueChenged();
                    _timer.Start();
                    //BeginInvoke(new MyDelegate(() => {
                    //    
                    //}));

                   // server.close();
                }
                catch (ServerModbusTCPException ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            });
            if (ch_num != 0)
            {
                UpdateSetupsTable();

            }

            ch_in_use = ch_num;

            zedGraph_autoscale();
            Paused = false;

        }

        private void button_setup_Click(object sender, EventArgs e)
        {
           // formChSetup.show();
        }

        private void button_pan_Click(object sender, EventArgs e)
        {
            this.zedGraph_autoscale();
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            if (refres_channels) return;
            _ = Task.Run( () =>
            {
                get_channels();
            });
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            Paused = ! Paused;
            this.zedGraph_autoscale();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = !dataGridView2.Visible;
            label_chnls.Text = "▼ Channels";
            if (dataGridView2.Visible == false) {
                label_chnls.Text = "▲ Channels";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            dataGridView_setups.Visible = !dataGridView_setups.Visible;
            label_setups.Text = "▼ Setups";
            if (dataGridView_setups.Visible == false)
            {
                label_setups.Text = "▲ Setups";
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            textBox1.Visible = !textBox1.Visible;
            label_log.Text = "▼ Scope log";
            if (textBox1.Visible == false)
            {
                label_log.Text = "▲ Scope log";
            }
        }

        private void tableLayoutPanel_notify_Paint(object sender, PaintEventArgs e)
        {
            
        }

      

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
           
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }


}
