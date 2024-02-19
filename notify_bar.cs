
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evm_VISU
{
    public class Notify_bar
    {
        System.Windows.Forms.Label label;
        System.Windows.Forms.Panel icon;
        private delegate void MyDelegate();

        public Notify_bar(System.Windows.Forms.Label label, System.Windows.Forms.Panel icon)
        {
            this.label = label;
            this.icon = icon;

        }

        public void setDiconectedState()
        {
            label.Text = "Diconnected";
         //   icon.BackgroundImage = Resources.Red_X_in_Circle_7;
        }

        public void setConnectedState()
        {
            label.Text = "Connected";
         //   icon.BackgroundImage = Resources.connected;
        }

        public void setPausedState()
        {
            label.Text = "Paused";
         //   icon.BackgroundImage = Resources.paused;
        }

    }
}


