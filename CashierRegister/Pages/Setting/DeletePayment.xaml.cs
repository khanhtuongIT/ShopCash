using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CashierRegisterEntity;
using CashierRegisterBUS;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for DeletePayment.xaml
    /// </summary>
    public partial class DeletePayment : ModernDialog
    {
        //using for DeletePayment
        private BUS_tb_Payment bus_tb_payment = new BUS_tb_Payment();
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();
        public int paymentid = 0;
        public string card = "";

        //delegate
        public delegate void Add_Delete_Edit_Delegate();
        public event Add_Delete_Edit_Delegate delete_delegate;

        //DeletePayment
        public DeletePayment()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { };
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bus_tb_order.GetSumOrder("WHERE [PaymentID] = " + paymentid) > 0)
                {
                    tblNotification.Text = FindResource("card").ToString() + (" ") + card + " " + FindResource("already_uses").ToString();
                    return;
                }
                else
                {
                    if (bus_tb_payment.DeletePayment(new EC_tb_Payment() { PaymentID = paymentid }) == 1)
                    {
                        if (delete_delegate != null)
                        {
                            StaticClass.GeneralClass.list_ec_tb_setting_general.Clear();
                            delete_delegate();
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }
    }
}
