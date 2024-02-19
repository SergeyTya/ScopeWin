using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace evm_VISU
{
    public class CustomTextBoxCell : DataGridViewTextBoxCell  // Cell
                                                              // storing holding value
    {
        protected enum CellState { 
            STADYSTATE, // value never changed
            ONCHANGE  , // value changed from user, need to be sent to device
            ONACNOW,    // value has been sent to device, need to be acknowledged
            ONFAULT     // value changed with no reason
        };

        protected CellState state = CellState.STADYSTATE;    

        private Queue<string> Log;
        private int fault_cnt = 0;
        private HoldingTypes.HoldingType type = HoldingTypes.HoldingType.NONE;

        public delegate void MContainer(object sender, object value);
        public event MContainer OnWriteFromTableToDevice; //Write holding from table to device

        public CustomTextBoxCell(Queue<string> log, HoldingTypes.HoldingType type) : base() {
            Log =log;   
            this.type = type;
            this.Style.ForeColor = Color.White;
            FontStyle fs = new FontStyle();
        }

        protected override bool SetValue(int rowIndex, object value) // override function for set value. Need to capture cell edit event
        {
            if (value == null)  value = Value;
            int poin_indx = value.ToString().IndexOf('.');
            if (poin_indx != -1) {
                string pointless = value.ToString().Replace(".", ",");
                value = pointless;
            }
            object new_obj = HoldingTable.GetObjectFromString(value.ToString(), type); // try to parse value to defined type
            object ret_obj = Value;
            if (new_obj == null) new_obj = Value;

            string str_value_now = getValueNowStr();
            string str_value_new = new_obj.ToString();

            if (str_value_now != str_value_new)
            {
                if (state == CellState.STADYSTATE)
                {
                    // State is stady, but got new value, so send new value to device
                    state = CustomTextBoxCell.CellState.ONCHANGE;
                    if (OnWriteFromTableToDevice != null)
                    {
                        OnWriteFromTableToDevice(this, new_obj);
                    }
                }
                ret_obj = new_obj;
            }

            //if (OnWriteFromTableToDevice != null && state == CellState.ONCHANGE)
            //{
            //    OnWriteFromTableToDevice(this, new_obj);
            //}
            Style.BackColor = getColorFromState(state);

            return base.SetValue(rowIndex, ret_obj); 
        }

        static Color getColorFromState(CellState state) {
            switch (state)
            {
                case CellState.ONCHANGE:
                   return  Color.Red;
   
                case CellState.ONACNOW:
                    return Color.Green;

                case CellState.ONFAULT:
                    return Color.Blue;
 
                case CellState.STADYSTATE:
                    return style.GetBackColor();
                default: 
                    return style.GetBackColor();
            }
        }

        public void setCellOnlyValue(object value)
        { // for internal use, to change cell value without sending to device

            if (value == null) {return;}

            Style.BackColor = getColorFromState(state);

            string value_now = getValueNowStr();
            string value_new = value.ToString();

            if (state == CellState.ONCHANGE) // changing value at device side, need to be aknowledge
            {   state = CellState.ONFAULT;
                return;
            }

            if (value_now != value_new)
            {
                if (state == CellState.ONACNOW) //it is not possible, so skip 
                {
                    state = CellState.ONFAULT;
                    return;
                }
                // value changed we don't know why
                //state = CellState.ONCHANGE;
                state = CellState.ONFAULT;
                Value = value;
                var str = String.Format("cell <- {0}", value_new);
                Log.Enqueue(str);
                return;

            }else {

                if (state == CellState.ONACNOW)
                {
                    // acknowledge value from device
                    var str = String.Format("cellAkn <- {0}", value_new);
                    Log.Enqueue(str);

                }

                if (state == CellState.ONFAULT)
                {
                    // need keep highlite certainly changed value 
                    fault_cnt++;
                    if (fault_cnt > 5) { fault_cnt = 0; } else { return; };
                }

                Value = value;
                state = CellState.STADYSTATE;

                return;
            }
        }
        private string getValueNowStr()
        {
            string ret_val = string.Empty;
            if (Value != null) { ret_val = Value.ToString(); }
            return ret_val;
        }
        public void aknOnWriteReq() // write procedure need tobe acknowledged from async task
        {  
            if (state == CellState.ONCHANGE)
            {
                state = CustomTextBoxCell.CellState.ONACNOW;
            }
        }
        public void setFalultState() // write procedure need tobe acknowledged from async task
        {
                state = CustomTextBoxCell.CellState.ONFAULT;
        }

        public void setOnChangeState() {

            state = CustomTextBoxCell.CellState.ONCHANGE;
        }
    }
}
