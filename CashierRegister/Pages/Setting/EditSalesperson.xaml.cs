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
    public partial class EditSalesperson : ModernDialog
    {
        //using for salesperson
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();

        //delegate
        public delegate void btnEdit_Click_Delegate();
        public event btnEdit_Click_Delegate edit_delegate;

        //AddSalesperson
        public EditSalesperson()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            txbName.Focus();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txbName.Text.Trim() == "")
            {
                tblNotification.Text = FindResource("salesperson_name_null").ToString();
                return;
            } 

            else
            {
                try
                {
                    if ((bus_tb_salesperson.GetSalesPerson("WHERE [Active]=1").Rows.Count <= 1) && (chkActive.IsChecked == false))
                        tblNotification.Text = FindResource("choose_a_salesperson").ToString();

                    EC_tb_SalesPerson ec_tb_salesperson = new EC_tb_SalesPerson();
                    ec_tb_salesperson.SalespersonID = Convert.ToInt32(tblSalespersonID.Text.Trim().ToString());
                    ec_tb_salesperson.Name = StaticClass.GeneralClass.HandlingSpecialCharacter(txbName.Text.Trim());
                    ec_tb_salesperson.Birthday = dtpBirthday.Text;
                    ec_tb_salesperson.Address = StaticClass.GeneralClass.HandlingSpecialCharacter(txbAddress.Text.Trim());
                    ec_tb_salesperson.Email = StaticClass.GeneralClass.HandlingSpecialCharacter(txbEmail.Text.Trim());
                    ec_tb_salesperson.Defaul = 0;

                    if (pwbPassword.Password.Trim() == "")
                    {
                        ec_tb_salesperson.Password = StaticClass.GeneralClass.salespersonpassword_general;
                        StaticClass.GeneralClass.salespersonpassword_general = "";
                    }
                    else
                        ec_tb_salesperson.Password = StaticClass.GeneralClass.MD5Hash(pwbPassword.Password.Trim().ToString());

                    if (chkActive.IsChecked == true)
                        ec_tb_salesperson.Active = 1;
                    else
                        ec_tb_salesperson.Active = 0;

                    if (bus_tb_salesperson.UpdateSalesPerson(ec_tb_salesperson, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {
                        if (edit_delegate != null)
                        {
                            StaticClass.GeneralClass.flag_add_edit_delete_salesperson_general = true;
                            edit_delegate();
                        }

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
