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
using CashierRegisterBUS;
using CashierRegisterEntity;
using System.Data;
using System.Threading;

namespace CashierRegister.Views
{
    /// <summary>
    /// Interaction logic for AddPaymentToPayCash.xaml
    /// </summary>
    public partial class AddPaymentToPayCash : ModernDialog
    {
        private BUS_tb_Payment bus_tb_payment = new BUS_tb_Payment();
        private List<EC_tb_Payment> list_ec_payment = new List<EC_tb_Payment>();
        public delegate void btnAddPayment_Click_Delegate();
        public event btnAddPayment_Click_Delegate btn_addPayment_delegate;
        public AddPaymentToPayCash()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            GetPayment();
        }
        private void GetPayment()
        {
            list_ec_payment.Clear();
            int no = 0;
            foreach (DataRow dr in bus_tb_payment.GetPayment("").Rows)
            {
                    TextBlock txtBlck = new TextBlock();
                    EC_tb_Payment ec_payment = new EC_tb_Payment();
                    ec_payment.No = ++no;
                    ec_payment.PaymentID = Convert.ToInt32(dr["PaymentID"].ToString());
                    ec_payment.Card = dr["Card"].ToString();
                    ec_payment.RdoCard = new RadioButton();
                    if (no == 1)
                        ec_payment.RdoCard.IsChecked = true;
                    txtBlck.Text = ec_payment.Card;
                    stpCard.Children.Add(ec_payment.RdoCard);
                    //lstPayment.Items.Add(txtBlck);
                    ec_payment.SepCard = new Separator();
                    //lstPayment.Items.Add(ec_payment.SepCard);
                    stpCard.Children.Add(ec_payment.SepCard);
                    list_ec_payment.Add(ec_payment);
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnOkAddPayment_Click(object sender, RoutedEventArgs e)
        {
            if(btn_addPayment_delegate != null)
            {
                btn_addPayment_delegate();
                this.Close();
            }
        }
    }
}
