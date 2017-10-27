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
    /// Interaction logic for EditPayment.xaml
    /// </summary>
    public partial class EditPayment : ModernDialog
    {
        //using for EditPayment
        private BUS_tb_Payment bus_payment = new BUS_tb_Payment();


        //delegate
        public delegate void Add_Delete_Edit_Delegate();
        public event Add_Delete_Edit_Delegate edit_delegate;

        //EditPayment
        public EditPayment()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            txbCard.Focus();
        }

        //btnSave_Click
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbCard.Text.Trim() == "")
                {
                    tblNotification.Text = FindResource("card_null").ToString();
                    return;
                }

                EC_tb_Payment ec_payment = new EC_tb_Payment();
                ec_payment.PaymentID = Convert.ToInt32(tblPaymentID.Text.Trim());
                ec_payment.Card = txbCard.Text.Trim();

                if (bus_payment.UpdatePayment(ec_payment, StaticClass.GeneralClass.flag_database_type_general) == 1)
                {
                    if (edit_delegate != null)
                    {
                        edit_delegate();
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
