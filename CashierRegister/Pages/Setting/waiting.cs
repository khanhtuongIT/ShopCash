using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CashierRegister.Pages.Setting
{
    public partial class Waiting : Form
    {
        private bool flag_cancel = false;
        private System.Drawing.Color accent_color = System.Drawing.ColorTranslator.FromHtml(StaticClass.GeneralClass.app_settings["accentColor"].ToString());
        public delegate void Connecting_Delegate(bool result);
        public event Connecting_Delegate connecting_delegate;

        public Waiting()
        {
            InitializeComponent();
            panel1.BackColor = accent_color;
            panel2.BackColor = accent_color;
            panel3.BackColor = accent_color;
            panel4.BackColor = accent_color;
        }

        //linkLabel1_Click
        private void linkLabel1_Click(object sender, EventArgs e)
        {
            flag_cancel = true;
            if (flag_cancel)
            {
                if (connecting_delegate != null)
                    connecting_delegate(true);
                linkLabel1.Hide();
                label1.Text = "closing...";
            }
        }

    }
}
