using AsyncSocketTest;
using evm_VISU;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsFormsApp4;
using static AsyncSocketTest.ServerModbusTCP;



namespace evm_VISU
{
    internal class HoldingOptions
    {

        public static List<HoldingOptions> holding_options = new List<HoldingOptions>();

        [JsonProperty("adr")]
        internal uint adr;
        [JsonProperty("options")]
        internal List<List<string>> options { get; set; }

        public static void Read() {
            try
            {
                string jsonString = File.ReadAllText("holdings.json", Encoding.Default);
                holding_options = JsonConvert.DeserializeObject<List<HoldingOptions>>(jsonString);

            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                Debug.WriteLine(e);
            }
        }

    }

    public static class HoldingTypes {

        public enum HoldingType : byte
        {
            HTYPE_UINT16,
            HTYPE_INT16,
            HTYPE_UINT32,
            HTYPE_INT32,
            HTYPE_FLOAT,
            NONE = 0xff
        }

        static private Dictionary<HoldingType, string> descp = new Dictionary<HoldingType, string>
        {
            [HoldingType.HTYPE_UINT16] = "uint16_t",
            [HoldingType.HTYPE_UINT32] = "uint32_t",
            [HoldingType.HTYPE_INT16] = "int16_t",
            [HoldingType.HTYPE_INT32] = "int32_t",
            [HoldingType.HTYPE_FLOAT] = "float",
        };

        static public string ToString(HoldingType type) {

            return descp[type];
           
        }

        static public HoldingType GetType(string str)
        {
           foreach (var kvp in descp)
           {
                if(kvp.Value == str) return kvp.Key;
           }

           return HoldingType.NONE;
        }
    }

    public class HoldingTable {

        public class HoldingValue
        {


            internal bool isReadOnly;

            internal InputRegisterIndicator indicator;
            internal DataGridViewRow row;

            [JsonProperty("adr")]
            internal uint adr;
            [JsonProperty("type")]
            internal HoldingTypes.HoldingType type;

            private object _val;
            public object Value
            {
                get { return _val; }
                set
                {
                    if (value == null) return;
                    _val = value;
                    if (isReadOnly == false)
                    {
                        CustomTextBoxCell cell = (CustomTextBoxCell)row.Cells[4];
                        cell.setCellOnlyValue(_val);
                    }
                    else
                    {
                        indicator.value = ToFloat();
                    }

                }
            }

            //
            //
            // Use this function to externaly write to device via holding table
            public void WriteValue(object value) 
            {
                if (value == null) return;
                if (isReadOnly == false)
                {
                    CustomTextBoxCell cell = (CustomTextBoxCell)row.Cells[4];
                    
                    if (cell.Value.ToString() != value.ToString()) {
                        cell.setOnChangeState();
                        cell.Value = value;
                    } 
                }
            }

            [JsonProperty("info")]
            internal string info;


            public HoldingValue(HoldingTypes.HoldingType type, bool isReadOnly, uint adr, string info)
            {
                this.type = type;
                this.isReadOnly = isReadOnly;
                this.adr = adr;
                this.info = info;
                this._val = GetObjetFromHoldingType(type);

                indicator = new InputRegisterIndicator((int)adr);
                var cellAdr = new DataGridViewTextBoxCell { Value = adr.ToString() };
                var cellInfo = new DataGridViewTextBoxCell { Value = info };
                var cellType = new DataGridViewTextBoxCell { Value = this.Value.GetType().Name };
                var cellTitle = new DataGridViewTextBoxCell();
                var cellValue = new CustomTextBoxCell(logs, type);
                var cellValList = new DataGridViewComboBoxCell();
                var cellBtnSend = new DataGridViewButtonCell();
                cellBtnSend.Value = "Apply";

                // Write holding from table to device
                cellValue.OnWriteFromTableToDevice += (__, ___) => {
                    if (__ == null) return;
                    CustomTextBoxCell cell = (CustomTextBoxCell)__;
                    if (___ == null) return;
                    byte[] bval = GetByteFromString(___.ToString(), this.Value.GetType());
                    if (bval == null) return;
                    Log_data(String.Format("reg#{0} -> {1}", adr, ___.ToString()));
                    _ = Task.Run(async () => // write holding task
                    {
                        try
                        {

                            int count = 0;
                            var buf = bval.ToList().GroupBy(_ => count++ / 2).Select(v => (BitConverter.ToUInt16(v.ToArray(), 0))).ToArray();
                            ServerModbusTCP tmp = new ServerModbusTCP(connection_setups.ServerName, connection_setups.ServerPort);
                            tmp.Timeout = 1000;
                            await tmp.WriteHoldingsAsync((byte)connection_setups.SlaveAdr, (byte)adr, buf);
                            var ret_buf = await tmp.ReadHoldingsAsync((byte)connection_setups.SlaveAdr, (byte)adr, buf.Length); // read back
                            if (ret_buf.SequenceEqual(buf))
                            {
                                cell.aknOnWriteReq();
                            }
                            else
                            {
                                cell.setFalultState();
                            }
                            tmp.close();
                        }
                        catch (ServerModbusTCPException ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Log_data(ex.Message);
                        }

                    });
                };
                row = new DataGridViewRow();

                row.Cells.AddRange(cellAdr, cellInfo, cellType, cellTitle, cellValue);

                foreach (HoldingOptions el in HoldingOptions.holding_options) { 
                    if(el != null)
                    {
                        if (el.adr == adr)
                        {
                            foreach ( var opt in el.options) {

                                cellValList.Items.Add(opt[0]);
                            }

                          
                            row.Cells.Add(cellValList);
                            row.Cells.Add(cellBtnSend);

                        }
                    }
                
                }
            }

            public new string ToString()
            {
                return _val.ToString();
            }


            public void WriteToDevice(object val)
            {
                if (val == null) return;
                if (isReadOnly == true) { return; };
                ((CustomTextBoxCell)row.Cells[4]).Value = val;
            }


            public float ToFloat()
            {
                float ret_val = 0.0f;
                if (this.Value != null) switch (type)
                    {
                        case HoldingTypes.HoldingType.NONE:
                            break;

                        case HoldingTypes.HoldingType.HTYPE_INT16:
                            ret_val = (float)((Int16)this.Value);
                            break;

                        case HoldingTypes.HoldingType.HTYPE_UINT16:
                            ret_val = (float)((UInt16)this.Value);
                            break;

                        case HoldingTypes.HoldingType.HTYPE_INT32:
                            ret_val = (float)((Int32)this.Value);
                            break;

                        case HoldingTypes.HoldingType.HTYPE_UINT32:
                            ret_val = (float)((UInt32)this.Value);
                            break;

                        case HoldingTypes.HoldingType.HTYPE_FLOAT:
                            ret_val = (float)this.Value;
                            break;

                        default:
                            break;
                    }
                return ret_val;
            }

            public byte[] ToBytes()
            {
                byte[] ret_val = null;
                switch (type)
                {
                    case HoldingTypes.HoldingType.NONE:
                        break;

                    case HoldingTypes.HoldingType.HTYPE_INT16:
                        ret_val = BitConverter.GetBytes((Int16)this.Value);
                        break;

                    case HoldingTypes.HoldingType.HTYPE_UINT16:
                        ret_val = BitConverter.GetBytes(((UInt16)this.Value));
                        break;

                    case HoldingTypes.HoldingType.HTYPE_INT32:
                        ret_val = BitConverter.GetBytes(((Int32)this.Value));
                        break;

                    case HoldingTypes.HoldingType.HTYPE_UINT32:
                        ret_val = BitConverter.GetBytes(((UInt32)this.Value));
                        break;

                    case HoldingTypes.HoldingType.HTYPE_FLOAT:
                        ret_val = BitConverter.GetBytes((float)this.Value);
                        break;

                    default:
                        break;
                }
                return ret_val;
            }
        }

        public Dictionary<uint, HoldingValue> hld_table = new Dictionary<uint, HoldingValue>();
        public int hreg_count = 0;
        public int error_cnt = 0;

        private static ConnectionSetups connection_setups = new ConnectionSetups();
        private TableLayoutPanel Indi_container;
        private DataGridView Param_container;
        private delegate void MyDelegate();
        private TextBox TextBox_log;
        private UInt16[] buff;
        private bool _isInitDone = false;
        private static Queue<string> logs = new Queue<string>();


        public bool isInitDone
        {
            get { return _isInitDone; }
            set { _isInitDone = value; }
        }


        public HoldingTable(TableLayoutPanel indi_container, DataGridView param_container, TextBox textBox_log)
        {
            HoldingOptions.Read();

            param_container.BackgroundColor = style.GetBackColor();
            textBox_log.BackColor = style.GetBackColor();
            connection_setups = ConnectionSetups.read();
            this.Param_container = param_container;
            this.TextBox_log = textBox_log;
            TextBox_log = textBox_log;

            this.Indi_container = indi_container;
            Indi_container.SuspendLayout();
            Indi_container.Dock =DockStyle.Fill;
            Indi_container.BackColor = style.GetBackColor();

            Indi_container.ColumnStyles.Clear();
            Indi_container.Controls.Clear();
            Indi_container.RowStyles.Clear();
            Indi_container.ColumnStyles.Clear();
            Indi_container.ColumnCount = 1;
            Indi_container.RowCount = 0;
            Indi_container.ColumnStyles.Add(new ColumnStyle()
            {
                SizeType = SizeType.Absolute,
                Width = 1
            });
            
      

            Indi_container.ResumeLayout(false);
            Indi_container.PerformLayout();

            param_container.CellContentClick += (_, __) =>
            {

                if (__.ColumnIndex == 6)
                {

                    DataGridViewComboBoxCell item = (param_container.Rows[__.RowIndex].Cells[5] as DataGridViewComboBoxCell);
                    if (item != null && item.Value != null)
                    {
                        int adr = -1;
                        int.TryParse(param_container.Rows[__.RowIndex].Cells[0].Value.ToString(), out adr);
                        if (adr == -1) return;

                        int val = -1;
                        foreach (var option in HoldingOptions.holding_options)
                        {
                            if (option.adr == adr)
                            {

                                foreach (List<string> el in option.options)
                                {
                                    if (el[0] == item.Value.ToString())
                                    {
                                        int.TryParse(el[1], out val);
                                        if (val == -1) return;
                                        param_container.Rows[__.RowIndex].Cells[4].Value = val;
                                        item.Value = null;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private int row_cnt = 0;
        private int col_cnt = 0;
        bool tmp_bool = false;
        bool tmp_bool2 = false;
        private void Add(HoldingTypes.HoldingType type, uint adr, bool isReadOnly = false, string info = "")
        {

            HoldingValue tmp = new HoldingValue(type, isReadOnly, adr, info);
            if (tmp != null)
            {
                hld_table.Add(adr, tmp);
                if (tmp.isReadOnly)
                {
                    if (true) {

             
                        tmp_bool2 = !tmp_bool2;

                        tmp.indicator.label.ForeColor = style.GetForeColor();
                        tmp.indicator.indicator.Dock = DockStyle.Fill;
                        tmp.indicator.Name = info;
                        tmp.indicator.label.AutoSize = true;
                        tmp.indicator.label.Width = 300;

                        Panel pnl = new Panel();
                        pnl.Dock = DockStyle.Fill;
                        pnl.AutoSize = true;
                      //  pnl.Width = tmp.indicator.label.Width*2;
                        TableLayoutPanel tb = new TableLayoutPanel();
                        tb.AutoSize = true;
                        tb.Dock = DockStyle.Fill;
                        tb.ColumnCount = 2;
                        tb.ColumnStyles.Clear();
                        tb.RowStyles.Clear();
                        tb.ColumnStyles.Add(new ColumnStyle()
                        {
                            SizeType = SizeType.Absolute,
                            Width = tmp.indicator.label.Width
                        });
                        tb.ColumnStyles.Add(new ColumnStyle()
                        {
                            SizeType = SizeType.Absolute,
                            Width = tmp.indicator.indicator.Width
                        });
                        tb.RowCount = 1;
                        tb.Controls.Add(tmp.indicator.label, 0, row_cnt);
                        tb.Controls.Add(tmp.indicator.indicator, 1, row_cnt);
                        pnl.Controls.Add(tb);

                        if (tmp_bool2) pnl.BackColor = style.GetBackColor2();

                        Indi_container.RowCount++;

                        Indi_container.RowStyles.Add(new RowStyle()
                        {
                            SizeType = SizeType.Absolute,
                            Height = tmp.indicator.indicator.Height + 10
                          
                        });
                        row_cnt = Indi_container.RowCount - 1;
                        Indi_container.Controls.Add(pnl, 0, row_cnt);
                        
                    }
                }
                else
                {

                    DataGridViewCellStyle st = style.GetCellStyle();
                    if (tmp_bool)  st.BackColor = style.GetBackColor2();
                    tmp.row.DefaultCellStyle = st;
                    Param_container.Rows.Add(tmp.row);
                    tmp_bool = !tmp_bool;
                }
            }
        }

      
        public void Clear()
        {
            error_cnt = 0;
            hld_table.Clear();
            Indi_container.Controls.Clear();
            Param_container.Rows.Clear();
        }

        public static void Log_data(string msg)
        {
            logs.Enqueue(msg);
        }


        async public void LoadHoldingAsync()
        {

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
            Clear();
            connection_setups = ConnectionSetups.read();
            this._isInitDone = false;
            try
            {

                ServerModbusTCP tmp = new ServerModbusTCP(connection_setups.ServerName, connection_setups.ServerPort);

                var RXbuf = await tmp.SendRawDataAsync(new byte[] { 0, 0, 0, 0, 0, 2, (byte)connection_setups.SlaveAdr, 26 }); // get device holding count
                if (RXbuf[7] != 26)
                {
                    Debug.WriteLine(String.Format("Holding count response error FC = {0}", RXbuf[7]));
                    return;
                }

                hreg_count = RXbuf[9] + (RXbuf[8] << 8);

                buff = await tmp.ReadHoldingsAsync((byte)connection_setups.SlaveAdr, 0, hreg_count);


                for (int i = 0; i < hreg_count; i++)
                {
                    RXbuf = await tmp.SendRawDataAsync(new byte[] { 0, 0, 0, 0, 0, 4, (byte)connection_setups.SlaveAdr, 27, 0, (byte)i });

                    if (RXbuf[7] != 27)
                    {
                        Debug.WriteLine(String.Format("Holding info response error FC = {0}", RXbuf[7]));
                        return;
                    }
                    UInt32 hl_adr = BitConverter.ToUInt16(RXbuf.ToArray(), 9);
                    UInt32 adr = BitConverter.ToUInt32(RXbuf.ToArray(), 10);
                    //Debug.WriteLine(String.Format("HR location = {0}", adr));
                    var type = RXbuf[14];
                    Debug.WriteLine(String.Format("HR type = {0}", type));
                    var index = RXbuf[15];
                    //Debug.WriteLine(String.Format("HR index = {0}", index));
                    var isRO = RXbuf[16];

                    string info = Encoding.UTF8.GetString(RXbuf.ToList().GetRange(17, RXbuf[5] - 11).ToArray());
                    Debug.WriteLine(String.Format("HR Info = {0}", info) + " \n");


                    if (
                          (HoldingTypes.HoldingType)type != HoldingTypes.HoldingType.HTYPE_INT16
                      && (HoldingTypes.HoldingType)type != HoldingTypes.HoldingType.HTYPE_UINT16
                     )
                    {
                        Add((HoldingTypes.HoldingType)type, (uint)i, info: info, isReadOnly: isRO == 1);
                        i++;
                    }
                    else {
                        Add((HoldingTypes.HoldingType)type, (uint)i, info: info, isReadOnly: isRO == 1);
                    }

                }
                tmp.close();
            }
            catch (ServerModbusTCPException ex)
            {

                Debug.WriteLine(ex.Message);
                Log_data(ex.Message);

            }

            _isInitDone = true;

        }

        async public void ReadAllFromDeviceAsync() {
            if(_isInitDone == false) { return; }
            try
            {
                ServerModbusTCP tmp = new ServerModbusTCP(connection_setups.ServerName, connection_setups.ServerPort);

                buff = await tmp.ReadHoldingsAsync((byte)connection_setups.SlaveAdr, 0, hreg_count);
                tmp.close();
                error_cnt = 0;
            }
            catch (ServerModbusTCPException ex) {
                Debug.WriteLine(ex);
                Log_data(ex.Message);
                error_cnt++;
            }
            
          
        }

        public object ReadValue(uint adr) {
            try {
                if(hld_table.Count == 0) return null;
                return hld_table[adr].Value;
            }
            catch (KeyNotFoundException ex)
            {
                //log_data(ex.Message);
                return null;
            }

        }

        public async void WriteToDeviceAsync(uint adr, byte[] val)
        {
            int count = 0;
            var ret_val = val.ToList().GroupBy(_ => count++ / 2).Select(v => (ushort) IPAddress.NetworkToHostOrder((BitConverter.ToUInt16(v.ToArray(), 0)))).ToArray(); ;

            try
            {
                ServerModbusTCP tmp = new ServerModbusTCP(connection_setups.ServerName, connection_setups.ServerPort);
                var RXbuf = await tmp.WriteHoldingsAsync((byte)connection_setups.SlaveAdr, (byte)adr, ret_val);
                tmp.close();
            }
            catch (ServerModbusTCPException ex)
            {
                Debug.WriteLine(ex);
                Log_data(ex.Message);
            }
        }

        public static object GetObjetFromHoldingType(HoldingTypes.HoldingType type) {
            switch (type)
            {
                case HoldingTypes.HoldingType.NONE:
                    return null;

                case HoldingTypes.HoldingType.HTYPE_INT16:
                    return (Int16)0;

                case HoldingTypes.HoldingType.HTYPE_UINT16:
                    return  (UInt16)0;

                case HoldingTypes.HoldingType.HTYPE_INT32:
                    return (Int32)0;

                case HoldingTypes.HoldingType.HTYPE_UINT32:
                    return (UInt32)0 ;

                case HoldingTypes.HoldingType.HTYPE_FLOAT:
                    return (float)0 ;

                default:
                    return null;
            }
        }   

        public static byte[] GetByteFormObj(object val) {
            byte[] ret_val = null;
            if (val.GetType() == typeof(Int16))
            {
                ret_val = BitConverter.GetBytes((Int16)val);
            }
            else if (val.GetType() == typeof(UInt16))
            {
                ret_val = BitConverter.GetBytes((UInt16)val);
            }
            else if (val.GetType() == typeof(Int32))
            {
                ret_val = BitConverter.GetBytes((Int32)val);
            }
            else if (val.GetType() == typeof(UInt32))
            {
                ret_val = BitConverter.GetBytes((UInt32)val);
            }
            else if (val.GetType() == typeof(float))
            {
                ret_val = BitConverter.GetBytes((float)val);
            }
            return ret_val;
        }

        public static byte[] GetByteFromString(string str, Type type)
        {
            byte[] ret_val = null;
            if (type == typeof(Int16))
            {
                if (Int16.TryParse(str, out Int16 res_val) == false)  return ret_val;
                ret_val = BitConverter.GetBytes(res_val);
            }
            else if (type == typeof(UInt16))
            {
                if (UInt16.TryParse(str, out UInt16 res_val) == false) return ret_val;
                ret_val = BitConverter.GetBytes(res_val);
            }
            else if (type == typeof(Int32))
            {
                if (Int32.TryParse(str, out Int32 res_val) == false) return ret_val;
                ret_val = BitConverter.GetBytes(res_val);
            }
            else if (type == typeof(UInt32))
            {
                if (UInt32.TryParse(str, out UInt32 res_val) == false) return ret_val;
                ret_val = BitConverter.GetBytes(res_val);
            }
            else if (type == typeof(float))
            {
                if (Single.TryParse(str, out float res_val) == false) return ret_val;
                ret_val = BitConverter.GetBytes(res_val);
            }
            return ret_val;
        }

        public static object GetObjectFromString(string str, HoldingTypes.HoldingType type)
        {
            object ret_val = null;
            if (type == HoldingTypes.HoldingType.HTYPE_INT16)
            {
                if (Int16.TryParse(str, out Int16 res_val) == false) return ret_val;
                ret_val = res_val;
            }
            else if (type == HoldingTypes.HoldingType.HTYPE_UINT16)
            {
                if (UInt16.TryParse(str, out UInt16 res_val) == false) return ret_val;
                ret_val = res_val;
            }
            else if (type == HoldingTypes.HoldingType.HTYPE_INT32)
            {
                if (Int32.TryParse(str, out Int32 res_val) == false) return ret_val;
                ret_val = res_val;
            }
            else if (type == HoldingTypes.HoldingType.HTYPE_UINT32)
            {
                if (UInt32.TryParse(str, out UInt32 res_val) == false) return ret_val;
                ret_val = res_val;
            }
            else if (type == HoldingTypes.HoldingType.HTYPE_FLOAT)
            {
                if (Single.TryParse(str, out float res_val) == false) return ret_val;
                ret_val = res_val;
            }
            return ret_val;
        }

        public void Refresh()
        {
            // Write log messages
            while (logs.Count > 0)
            {
                var str = String.Format(Environment.NewLine + "{0} {1} ", DateTime.Now.ToLongTimeString(), logs.Dequeue());
                TextBox_log.AppendText(str);
            }

            foreach (var reg in hld_table)
            {
                var type = reg.Value.type;
                UInt32 val0 = buff[reg.Value.adr];

                switch (type)
                {
                    case HoldingTypes.HoldingType.HTYPE_INT16:
                        reg.Value.Value = (Int16)val0 ;
                        break;
                    case HoldingTypes.HoldingType.HTYPE_UINT16:
                        reg.Value.Value = (UInt16)val0;
                        break;

                    case HoldingTypes.HoldingType.HTYPE_INT32:
                        UInt32 val1 = (UInt32)buff[reg.Value.adr] + (UInt32)(((UInt16)buff[reg.Value.adr + 1]) << 16);
                        reg.Value.Value = (Int32)val1;
                        break;
                    case HoldingTypes.HoldingType.HTYPE_UINT32:
                        UInt32 val2 = (UInt32)buff[reg.Value.adr] + (UInt32)(((UInt16)buff[reg.Value.adr + 1]) << 16);
                        reg.Value.Value = (UInt32)val2;
                        break;
                    case HoldingTypes.HoldingType.HTYPE_FLOAT:
                        UInt32 val3 = (UInt32)buff[reg.Value.adr] + (UInt32)(((UInt16)buff[reg.Value.adr + 1]) << 16);
                        reg.Value.Value = (float)BitConverter.ToSingle(BitConverter.GetBytes((UInt32)val3), 0);
                        break;
                    default:
                    case HoldingTypes.HoldingType.NONE:
                        break;
                }
            }
        }

        public string Save_to_file(string path, string dev_id = null)
        {
            List<string> tmp = new List<string>() { dev_id,  DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToLongTimeString()};

            foreach (KeyValuePair<uint, HoldingValue> entry in hld_table)
            {
                if (entry.Value.isReadOnly == false) {
                    string jsonString = JsonConvert.SerializeObject(entry.Value);
                    tmp.Add(jsonString);
                }
            }

            // string name = string.Format("prm_{0}.json", dev_id.Split(' ')[0]);
            // File.WriteAllLines(name, tmp.ToArray());
            File.WriteAllLines(path, tmp.ToArray());
            return path;
        }

        public void Read_from_file(string dev_id = null)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Файл параметров|*.prm";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            int i = 0;
            foreach (string line in File.ReadAllLines(filename, Encoding.Default))
            { i++;
                try
                {
                    HoldingValue tmp = JsonConvert.DeserializeObject<HoldingValue>(line);

                    var new_val = GetObjectFromString(tmp.Value.ToString(), hld_table[tmp.adr].type);
                    hld_table[tmp.adr].WriteToDevice(new_val);
                }
                catch (Exception e)
                {
                    if (i > 2) {
                        Debug.WriteLine(e);
                        MessageBox.Show(e.Message);
                        break; // first line is ID, second is time
                    }

                }
            }

        }

    }

}
