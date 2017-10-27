using System;
using System.Collections.Generic;
using FirstFloor.ModernUI.Windows.Controls;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CashierRegister.Pages.Home
{
    public partial class frmQuantity : Form
    {
        public int inventory = 0;
        public string str_notification = "";
        public string str_warning1 = "";
        public string str_warning2 = "";
        public string str_warning3 = "";

        //delegate
        public delegate void btnQty_MouseDown_Delegate(int qty);
        public event btnQty_MouseDown_Delegate btnqty_delegate;

        //Constants
        const int AW_SLIDE = 0X40000;
        const int AW_HOR_POSITIVE = 0X1;
        const int AW_HOR_NEGATIVE = 0X2;
        const int AW_BLEND = 0X80000;

        [DllImport("user32")]
        static extern bool AnimateWindow(IntPtr hwnd, int time, int flags);

        //frmQuantity
        public frmQuantity()
        {
            InitializeComponent();
            System.Drawing.Color accent_color = System.Drawing.ColorTranslator.FromHtml(StaticClass.GeneralClass.app_settings["accentColor"].ToString());
            this.BackColor = accent_color;

            txbQuantity.Text = StaticClass.GeneralClass.orderdetailqty_general.ToString() ;
            txbQuantity.Focus();
            txbQuantity.Height = 25;
            imgWarning.Visible = false;
            lblWarning.Visible = false;
        }

        //OnLoad
        protected override void OnLoad(EventArgs e)
        {
            lblNotication.Text = str_notification;

            this.Location = new Point((int)StaticClass.GeneralClass.locationx_muibtnCustomer, (int)StaticClass.GeneralClass.locationy_muibtnCustomer);

            //animate for form
            AnimateWindow(this.Handle, 500, AW_SLIDE | AW_HOR_POSITIVE);
        }

        //Quantity_Enter
        private void Quantity_Enter(object sender, EventArgs e)
        {
            this.Close();
        }

        //txbQuantity_KeyDown
        private void txbQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData.ToString() == "Return") || e.KeyData.ToString() == "Escape" || e.KeyData.ToString() == "Space")
            {
                int qty;
                if (int.TryParse(txbQuantity.Text.Trim().ToString(), out qty) == true)
                {
                    if (qty <= 0)
                    {
                        lblNotication.Visible = false;
                        imgWarning.Visible = true;
                        lblWarning.Visible = true;
                        lblWarning.Text = str_warning1;
                        return;
                    }

                    if (Convert.ToInt32(txbQuantity.Text.Trim().ToString()) > inventory)
                    {
                        lblNotication.Visible = false;
                        imgWarning.Visible = true;
                        lblWarning.Visible = true;
                        lblWarning.Text = str_warning2 + ": " + inventory.ToString();
                        txbQuantity.Text = StaticClass.GeneralClass.orderdetailqty_general.ToString();
                    }

                    else
                    {
                        if (btnqty_delegate != null)
                        {
                            btnqty_delegate(Convert.ToInt32(txbQuantity.Text.Trim()));
                            this.Close();
                        }
                    }
                }
                else
                {
                    lblNotication.Visible = false;
                    imgWarning.Visible = true;
                    lblWarning.Visible = true;
                    lblWarning.Text = str_warning3;
                }
            }
        }

    }
}
