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
    /// Interaction logic for EditCustomer.xaml
    /// </summary>
    public partial class EditCustomer : ModernDialog
    {
        //using for customer
        private BUS_tb_Customer bus_tb_customer = new BUS_tb_Customer();

        //delegate
        public delegate void btnEdit_Click_Delegate();
        public event btnEdit_Click_Delegate muibtnedit_delegate;

        public EditCustomer()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txbFirstname.Text == "")
            {
                tblNotification.Text = FindResource("first_name_null").ToString();
                txbFirstname.Focus();
                return;
            }

            if (txbLastname.Text == "")
            {
                tblNotification.Text = FindResource("last_name_null").ToString();
                txbLastname.Focus();
                return;
            }

            else
            {
                try
                {
                    EC_tb_Customer ec_tb_customer = new EC_tb_Customer();
                    ec_tb_customer.CustomerID = Convert.ToInt32(tblCustomerID.Text.Trim().ToString());
                    ec_tb_customer.FirstName = StaticClass.GeneralClass.HandlingSpecialCharacter(txbFirstname.Text.Trim().ToString());
                    ec_tb_customer.LastName = StaticClass.GeneralClass.HandlingSpecialCharacter(txbLastname.Text.Trim().ToString());
                    ec_tb_customer.Address1 = StaticClass.GeneralClass.HandlingSpecialCharacter(txbAddress1.Text.Trim().ToString());
                    ec_tb_customer.Address2 = StaticClass.GeneralClass.HandlingSpecialCharacter(txbAddress2.Text.Trim().ToString());
                    ec_tb_customer.City = StaticClass.GeneralClass.HandlingSpecialCharacter(txbCity.Text.Trim().ToString());
                    ec_tb_customer.State = StaticClass.GeneralClass.HandlingSpecialCharacter(txbState.Text.Trim().ToString());
                    ec_tb_customer.Zipcode = StaticClass.GeneralClass.HandlingSpecialCharacter(txbZipcode.Text.Trim().ToString());
                    ec_tb_customer.Phone = StaticClass.GeneralClass.HandlingSpecialCharacter(txbPhone.Text.Trim().ToString());
                    ec_tb_customer.Email = StaticClass.GeneralClass.HandlingSpecialCharacter(txbEmail.Text.Trim().ToString());

                    if (bus_tb_customer.UpdateCustomer(ec_tb_customer, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {
                        if (muibtnedit_delegate != null)
                        {
                            StaticClass.GeneralClass.flag_add_edit_delete_customer_general = true;
                            muibtnedit_delegate();
                        }
                        this.Close();
                    }
                    else
                        tblNotification.Text = FindResource("error_updating").ToString();
                }
                catch (Exception ex)
                {
                    tblNotification.Text = ex.Message;
                }
            }
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txbFirstname.Focus();
        }
    }
}
