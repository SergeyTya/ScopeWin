using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestGen;
using ZedGraph;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace evm_VISU
{
    public partial class FormChart2 : Form
    {
        private RollingPointPairList data;
        private int capacity = 1000;
        private double _time = 0.0;
        private double _scale = 100.0;
        private ControlsTable controlTable_setups;
      
        public FormChart2(int ind)
        {
            data = new RollingPointPairList(capacity);
            _time = 0.0;
            InitializeComponent();
            zedGraph_createCruves();
            zedGraph_applyStyle(zedGraph1);
            splitContainer1.Panel1Collapsed = true;
            panel_setup.Visible = true;
            controlTable_setups = new ControlsTable(dataGridView_setups);
            foreach (DataGridViewColumn col in dataGridView_setups.Columns) {
                col.DefaultCellStyle = CellApplyStyle();
            }

            // Развертка
            CustomControl tmp = new CustomControl(
               "Scale",
               max: 100,
               min: 0.5,
               def: 100
            );
            tmp.OnControlEvetn += ()=> {
                _scale = tmp.Value;
            };
            tmp.Value = _scale;
            controlTable_setups.addControl(tmp);
            controlTable_setups.RenderTable();
        }

        private DataGridViewCellStyle CellApplyStyle()
        {
            DataGridViewCellStyle cell_style = new DataGridViewCellStyle();
            cell_style.BackColor = Color.Black;
            cell_style.ForeColor = Color.LemonChiffon;
            cell_style.SelectionBackColor = Color.ForestGreen;
            cell_style.SelectionForeColor = Color.LemonChiffon;
            return cell_style;
        }

        private void FormChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            data.Clear();
            e.Cancel = true;
            this.Hide();
        }

        private void zedGraph_createCruves()
        {

            GraphPane pane1 = zedGraph1.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane1.CurveList.Clear();

            // Добавим кривую пока еще без каких-либо точек
            var myCurve = pane1.AddCurve(" ", data,
               System.Drawing.ColorTranslator.FromHtml("#FFFF11"), 
               SymbolType.Circle
               );
            myCurve.Symbol.Size = 2;

            zedGraph1.AxisChange();
            zedGraph1.Invalidate();
        }

        private void zedGraph_applyStyle(ZedGraphControl zedGraph)
        {
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

            pane1.XAxis.Scale.MinAuto = true;
            pane1.XAxis.Scale.MaxAuto = true;

            zedGraph1.AxisChange();
            zedGraph1.Invalidate();
        }

        private Stopwatch startTime = Stopwatch.StartNew();
        private bool _pause = false;

        public void AddPoint(double val) {
            try
            {   if (_pause == true) return;
                if (zedGraph1 == null) return;
                startTime.Stop();
                double timeStep = (double)startTime.ElapsedMilliseconds / 1000;
                startTime.Restart();
                if (timeStep == 0) return;
                _time += timeStep;

                data.Add(new PointPair(_time, val));

                GraphPane pane1 = zedGraph1.GraphPane;
                if (pane1 == null) return;

                double min = _time - _scale;
                pane1.XAxis.Scale.Min = min;
                pane1.XAxis.Scale.Max = _time;

                zedGraph1.AxisChange();
                zedGraph1.Invalidate();
            }
            catch (NullReferenceException ex)
            {
            }
        }

        public string Label
        {
            set { this.Text = value; }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
            if (splitContainer1.Panel1Collapsed){
                label1.Text = "►";
            }
            else {
                label1.Text = "◄";
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            panel_setup.Visible = !panel_setup.Visible;
            if (panel_setup.Visible)
            {
                label2.Text = "Setups ▲";
            }
            else
            {
                label2.Text = "Setups ▼";
            }
        }

        private void dataGridView_setups_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_pan_Click(object sender, EventArgs e)
        {
            zedGraph_autoscale();
        }

        private void zedGraph_autoscale()
        {
            GraphPane pane = zedGraph1.GraphPane;
            zedGraph1.RestoreScale(pane);
            zedGraph1.AxisChange();
            zedGraph1.Invalidate();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            _pause = !_pause;
            if (_pause)
            {
                data.Add(PointPairBase.Missing, PointPairBase.Missing);
            }
        }

        private void FormChart2_Load(object sender, EventArgs e)
        {

        }

        private void FormChart2_Shown(object sender, EventArgs e)
        {
            zedGraph_autoscale();
        }
    }
}
