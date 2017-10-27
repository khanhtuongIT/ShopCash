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
    /// Interaction logic for AddPayment.xaml
    /// </summary>
    public partial class AddPayment : ModernDialog
    {
        //using for AddPayment
        private BUS_tb_Payment bus_payment = new BUS_tb_Payment();


        //delegate
        public delegate void Add_Delete_Edit_Delegate();
        public event Add_Delete_Edit_Delegate add_delegate;

        //AddPayment
        public AddPayment()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            txbCard.Focus();
        }

        //btnAdd_Click
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbCard.Text.Trim() == "")
                {
                    tblNotification.Text = FindResource("card_null").ToString();
                    return;
                }

                EC_tb_Payment ec_payment = new EC_tb_Payment();
                //ec_payment.PaymentID = bus_payment.GetMaxPaymentID("") + 1;
                ec_payment.Card = txbCard.Text.Trim();

                if (bus_payment.InsertPayment(ec_payment, StaticClass.GeneralClass.flag_database_type_general) == 1)
                {
                    if (add_delegate != null)
                    {
                        add_delegate();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
