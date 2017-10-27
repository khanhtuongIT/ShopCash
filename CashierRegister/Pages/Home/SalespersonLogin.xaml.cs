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
using System.Data;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for SalespersonLogin.xaml
    /// </summary>
    public partial class SalespersonLogin : ModernDialog
    {
        //using for salesperson
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();

        //delegate
        public delegate void btnLogin_Click_Delegate();
        public btnLogin_Click_Delegate btnlogin_delegate;
        
        public SalespersonLogin()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            StaticClass.GeneralClass.salespersonid_login_general = 0;
            StaticClass.GeneralClass.salespersonname_login_general = "None";
            this.Close();
        }

        //btnLogin_Click
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (pwbPassword.Password.ToString() == "")
            {
                tblNotification.Text = FindResource("password_null").ToString();
                pwbPassword.Focus();
                return;
            }
            else
            {
                try
                {
                    DataTable dt_salesperson = bus_tb_salesperson.GetSalesPerson("WHERE [SalespersonID] = " + StaticClass.GeneralClass.salespersonid_login_general + " AND [Password] = '" + StaticClass.GeneralClass.MD5Hash(pwbPassword.Password.Trim().ToString()) + "'");
                    if (dt_salesperson.Rows.Count == 1)
                    {
                        if (btnlogin_delegate != null)
                        {
                            StaticClass.GeneralClass.flag_salespersonlogin_general = true;
                            StaticClass.GeneralClass.salespersonname_login_general = dt_salesperson.Rows[0]["Name"].ToString();

                            btnlogin_delegate();
                            this.Close();

                            //save logs
                            if (StaticClass.GeneralClass.app_settings["isSaveLogs"].ToString()=="True")
                                new System.Threading.Thread(() => { StaticClass.GeneralClass.SaveLogs(StaticClass.GeneralClass.salespersonname_login_general + " (salesperson)", "Logged in", DateTime.Now); }).Start();
                        }
                    }
                    else
                    {
                        tblNotification.Text = FindResource("password_incorrect").ToString();
                        return;
                    }
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
            pwbPassword.Focus();
        }

        //pwbPassword_KeyDown
        private void pwbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                if (pwbPassword.Password.ToString() == "")
                {
                    tblNotification.Text = FindResource("password_null").ToString();
                    pwbPassword.Focus();
                    return;
                }
                else
                {
                    try
                    {
                        DataTable dt_salesperson = bus_tb_salesperson.GetSalesPerson("WHERE [SalespersonID] = " + StaticClass.GeneralClass.salespersonid_login_general + " AND [Password] = '" + StaticClass.GeneralClass.MD5Hash(pwbPassword.Password.Trim().ToString()) + "'");
                        if (dt_salesperson.Rows.Count == 1)
                        {
                            if (btnlogin_delegate != null)
                            {
                                StaticClass.GeneralClass.flag_salespersonlogin_general = true;
                                StaticClass.GeneralClass.salespersonname_login_general = dt_salesperson.Rows[0]["Name"].ToString();

                                btnlogin_delegate();
                                this.Close();
                            }
                        }
                        else
                        {
                            tblNotification.Text = FindResource("password_incorrect").ToString();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        tblNotification.Text = ex.Message;
                    }
                }
            }
        }

    }
}
