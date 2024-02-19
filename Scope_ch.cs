using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace evm_VISU
{


    internal class Scope_ch
    {
        public const int ch_num_max = 6;

        private class Channel
        {
            public double gain = 1;
            public double offset = 0;
            public HoldingTypes.HoldingType type = HoldingTypes.HoldingType.NONE;
            private RollingPointPairList _data_ch;

            public Channel(int capacity)
            {
                _data_ch = new RollingPointPairList(capacity);
            }

            public int capacity
            {
                set {
                    _data_ch = new RollingPointPairList(value);
                }
            }

            public RollingPointPairList points 
            {
                get { return _data_ch; }
            }

            public void AddPoint(double x, double y) {
                _data_ch.Add(x, y);
            }

            public void ResetPoints() {
                _data_ch.Clear();
            }
        }

        public delegate void MethodContainer();
       // Событие OnCount c типом делегата MethodContainer.
        public event MethodContainer onChanged;
        public double _frame_time = 0;
        public int _ch_num = 0;
        public bool fifo_mpty = false;
        public double _chnls_time_step = 0;

        private Channel[] chanells;
        private static int frame_size = 240 * 4;
        private int _capacity = frame_size * 10;

        


        public int capacity
        {
            set
            {
                _capacity = value;
                if (chanells == null) return;
                foreach (Channel c in chanells)
                {
                    c.capacity = _capacity;
                }
            }
            get { return _capacity; }
        }

        public Scope_ch()
        {
            chanells = new Channel[ch_num_max];

            for (int i = 0; i < ch_num_max; i++)
            {
                chanells[i] = new Channel(_capacity);
            }
        }

        public void SetGains(double[] newValue)
        {
            if (chanells == null) return;
            for (int i = 0; i < ch_num_max; i++) {
                chanells[i].gain = newValue[i];
            }
        }

        public void ResetGainsOffsets()
        {
            if (chanells == null) return;
            for (int i = 0; i < ch_num_max; i++)
            {
                chanells[i].gain   = 1;
                chanells[i].offset = 0;
            }
        }


        public void SetOffsets(double[] newValue)
        {
            if (chanells == null) return;
            for (int i = 0; i < ch_num_max; i++)
            {
                chanells[i].offset = newValue[i];
            }
        }

        public void SetChnType(int ch, HoldingTypes.HoldingType ch_type)
        {
            if (chanells == null || ch >= chanells.Length ) return;
            chanells[ch].type = ch_type;
        }

        public HoldingTypes.HoldingType GetChnType(int ch)
        {
            if (chanells == null || ch >= chanells.Length) return HoldingTypes.HoldingType.NONE;
            return chanells[ch].type;
        }

        public double AddMissingFrame()
        {
            if (chanells == null) return 0;
            for (int i = 0; i < ch_num_max; i++)
            {
                chanells[i].AddPoint(PointPairBase.Missing, PointPairBase.Missing);
            }
            return _frame_time;
        }

        public RollingPointPairList GetChannelData(int ch) {
            if(chanells == null || ch > chanells.Length) return null;
            return chanells[ch].points;
        }

        public double AddData(byte[] RXbuf, double time_now, int frame_size)
        {

            /* _______________________________MODBUS SCOPE FRAME (TCP)______________________
             *
             *       +------+-----+----+---------------+-------+-----------+------------+
             * index |0-5   | 6   | 7  |8  ...  F_SIZE |  +8   |    +9     |  +10       |  
             *       +------+-----+----+---------------+-------+-----------+------------+
             * FRAME |HEADER| ADR |CMD |     DATA      | delay | ch num    |  FIFO LEN  | 
             * 
             */

            int delay_pos = frame_size + 8;
            int chn_pos = frame_size + 9;
            int fifolen_pos = frame_size + 10;

            int new_ch_num = RXbuf[chn_pos];
            double new_timestep = RXbuf[delay_pos] * 0.001;
            int fifo_len = RXbuf[fifolen_pos];
            double _time_now = time_now;

            if (chanells == null) return 0;

            if (new_ch_num != _ch_num | new_timestep != _chnls_time_step | _ch_num == 0 | _chnls_time_step == 0)
            {

                _time_now += AddMissingFrame();
                _ch_num = new_ch_num;
                _chnls_time_step = new_timestep;
                onChanged();
            }

            int count = 0;
            var res = RXbuf.ToList().GetRange(8, frame_size).GroupBy(_ => count++ / 2).Select(v => (double)IPAddress.NetworkToHostOrder((BitConverter.ToInt16(v.ToArray(), 0)))).ToArray();
            count = 0;
            if (_ch_num == 0) {
                return _time_now;
            }
            double[][] res2 = res.GroupBy(_ => count++ % _ch_num).Select(v => v.ToArray()).ToArray();
            _frame_time = _chnls_time_step * res2[0].Length;

            for (int i = 0; i < res2[0].Length; i++)
            {
                for (int k = 0; k < ch_num_max; k++)
                {
                    if (k < res2.Length && k < chanells.Length)
                    {
                        switch (chanells[k].type) // need to know type
                        {
                            case HoldingTypes.HoldingType.NONE:
                                chanells[k].AddPoint(PointPairBase.Missing, PointPairBase.Missing);
                                break;
                            case HoldingTypes.HoldingType.HTYPE_INT16:
                            case HoldingTypes.HoldingType.HTYPE_UINT16:
                                chanells[k].AddPoint(_time_now, res2[k][i] * chanells[k].gain + chanells[k].offset);
                                break;

                            case HoldingTypes.HoldingType.HTYPE_UINT32:
                                if (k + 1 >= res2.Length)
                                {
                                    Debug.WriteLine("INT32 restore fault");
                                    chanells[k].ResetPoints();
                                }
                                else
                                {
                                    UInt32 val = (UInt16)res2[k][i] + (UInt32)(((UInt16)res2[k + 1][i]) << 16);
                                    chanells[k].AddPoint(_time_now, val * chanells[k].gain + chanells[k].offset);
                                }
                                break;

                            case HoldingTypes.HoldingType.HTYPE_INT32:
                                if (k + 1 >= res2.Length)
                                {
                                    Debug.WriteLine("UINT32 restore fault");
                                    chanells[k].ResetPoints();
                                }
                                else
                                {
                                    Int32 val = (Int32)((UInt16)res2[k][i] + (UInt32)(((UInt16)res2[k + 1][i]) << 16));
                                    chanells[k].AddPoint(_time_now, val * chanells[k].gain + chanells[k].offset);
                                }
                                break;

                            case HoldingTypes.HoldingType.HTYPE_FLOAT:
                                if (k + 1 >= res2.Length)
                                {
                                    Debug.WriteLine("float restore fault");
                                    chanells[k].ResetPoints();
                                }
                                else
                                {
                                    if (i > res2[k].Length) break;
                                    UInt32 uint_val = (UInt16)res2[k][i] + (UInt32)(((UInt16)res2[k + 1][i]) << 16);
                                    byte[] bytes = BitConverter.GetBytes(uint_val);
                                    float float_val = BitConverter.ToSingle(bytes, 0);
                                    chanells[k].AddPoint(_time_now, float_val * chanells[k].gain + chanells[k].offset);
                                    break;
                                }
                                break;

                            default:

                                break;
                        }
                    }
                    else
                    {
                        chanells[k].ResetPoints();
                    }
                }
                _time_now += new_timestep;
            }

            return _time_now;
        }
    }
}
