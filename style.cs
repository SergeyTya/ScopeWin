using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace evm_VISU
{
    internal class style
    {
        public static DataGridViewCellStyle GetCellStyle()
        {
            Color fore_color = GetForeColor(); 
            DataGridViewCellStyle cell_style = new DataGridViewCellStyle();
            cell_style.BackColor = GetBackColor();
            cell_style.ForeColor = fore_color;
            cell_style.SelectionBackColor = GetSelectionColor();
            cell_style.SelectionForeColor = fore_color;
            cell_style.Font = new Font("Arial", 10, FontStyle.Regular);
            return cell_style;
        }


        public static Color GetBackColor()
        {
             return (Color)new ColorConverter().ConvertFromString("#1F1F1F"); 
        }

        public static Color GetBackColor2()
        {
            return (Color)new ColorConverter().ConvertFromString("#323232"); 
        }

        public static Color GetForeColor() {
            //Color fore_color = Color.WhiteSmoke;
            Color fore_color = (Color)new ColorConverter().ConvertFromString("#DDDDDD");
            return fore_color;
        }

        public static Color GetSelectionColor()
        {
            Color fore_color = Color.ForestGreen;

            return fore_color;
        }

    }
}
