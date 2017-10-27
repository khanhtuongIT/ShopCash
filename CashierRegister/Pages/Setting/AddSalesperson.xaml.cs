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
    /// Interaction logic for AddSalesperson.xaml
    /// </summary>
    public partial class AddSalesperson : ModernDialog
    {
        //using for salesperson
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();

        //delegate
        public delegate void muiBtnAdd_Click_Delegate();
        public event muiBtnAdd_Click_Delegate muibtnadd_delegate;

        //AddSalesperson
        public AddSalesperson()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            txbName.Focus();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
             if (txbName.Text == "")
            {
                tblNotification.Text = FindResource("name_null").ToString();
                txbName.Focus();
                return;
            }

             if (pwbPassword.Password.Trim() == "")
             {
                 tblNotification.Text = FindResource("password_null").ToString();
                 pwbPassword.Focus();
                 return;
             }

             else
             {
                 try
                 {
                     EC_tb_SalesPerson ec_tb_salesperson = new EC_tb_SalesPerson();
                     //ec_tb_salesperson.SalespersonID = bus_tb_salesperson.GetMaxSalespersonID("") + 1;
                     ec_tb_salesperson.Name = StaticClass.GeneralClass.HandlingSpecialCharacter(txbName.Text);
                     ec_tb_salesperson.Birthday = dtpBirthday.Text;
                     ec_tb_salesperson.Address = StaticClass.GeneralClass.HandlingSpecialCharacter(txbAddress.Text);
                     ec_tb_salesperson.Email = StaticClass.GeneralClass.HandlingSpecialCharacter(txbEmail.Text);
                     ec_tb_salesperson.Password = StaticClass.GeneralClass.MD5Hash(pwbPassword.Password);
                     ec_tb_salesperson.Defaul = 0;

                     if (chkActive.IsChecked == false)
                        ec_tb_salesperson.Active = 0;
                    else
                        ec_tb_salesperson.Active = 1;

                    if (bus_tb_salesperson.InsertSalesPerson(ec_tb_salesperson, StaticClass.GeneralClass.flag_database_type_general) == 1)
                     {
                         if (muibtnadd_delegate != null)
                            muibtnadd_delegate();

                        this.Close();
                     }
                 }
                 catch (Exception ex)
                 {
                     tblNotification.Text = ex.Message;
                 }
             }
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
