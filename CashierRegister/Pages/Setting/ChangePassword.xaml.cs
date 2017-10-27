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
using FirstFloor.ModernUI.Windows.Controls;
using CashierRegisterBUS;
using CashierRegisterEntity;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : UserControl
    {
        //using for user
        private BUS_tb_User bus_tb_user = new BUS_tb_User();

        //ChangePassword
        public ChangePassword()
        {
            InitializeComponent();
        }

        //btnOK_Click
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pwbCurrentPassword.Password.Trim() == "")
                {
                    tblNotification.Text = FindResource("current_password_null").ToString();
                    pwbCurrentPassword.Focus();
                    return;
                }

                if (pwbNewPassword.Password.Trim() == "")
                {
                    tblNotification.Text = FindResource("new_password_null").ToString();
                    pwbNewPassword.Focus();
                    return;
                }

                if (pwbNewPassword.Password.Trim().Length < 3)
                {
                    tblNotification.Text = FindResource("password_short").ToString();
                    pwbNewPassword.Focus();
                    return;
                }

                if (pwbConfirmPassword.Password.Trim() == "")
                {
                    tblNotification.Text = FindResource("confirm_password_null").ToString();
                    pwbConfirmPassword.Focus();
                    return;
                }
               
                else
                {
                    if (StaticClass.GeneralClass.MD5Hash(pwbCurrentPassword.Password.Trim().ToString()) != StaticClass.GeneralClass.password_user_general)
                    {
                        tblNotification.Text = FindResource("current_password_incorrect").ToString();
                        pwbCurrentPassword.Focus();
                        return;
                    }

                    if (StaticClass.GeneralClass.MD5Hash(pwbNewPassword.Password.Trim().ToString()) != StaticClass.GeneralClass.MD5Hash(pwbConfirmPassword.Password.Trim().ToString()))
                    {
                        tblNotification.Text = FindResource("new_password_confirm_password_incorrect").ToString();
                        pwbConfirmPassword.Focus();
                        return;
                    }

                    else
                    {
                        EC_tb_User ec_tb_user = new EC_tb_User();
                        ec_tb_user.ID = StaticClass.GeneralClass.id_user_general;
                        ec_tb_user.Password = StaticClass.GeneralClass.MD5Hash(pwbNewPassword.Password.Trim().ToString());

                        if (bus_tb_user.UpdatePasswordUser(ec_tb_user, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        {
                            StaticClass.GeneralClass.password_user_general = ec_tb_user.Password;

                            //return account page
                            BBCodeBlock bbcodeblock = new BBCodeBlock();
                            bbcodeblock.LinkNavigator.Navigate(new Uri("/Pages/Setting/Account.xaml", UriKind.Relative), this);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //pwbCurrentPassword_KeyDown
        private void pwbCurrentPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
                btnOK_Click(null, null);
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BBCodeBlock bbcodeblock = new BBCodeBlock();
                bbcodeblock.LinkNavigator.Navigate(new Uri("/Pages/Setting/Account.xaml", UriKind.Relative), this);
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tblNotification.Text = "";
            pwbCurrentPassword.Password = "";
            pwbNewPassword.Password = "";
            pwbConfirmPassword.Password = "";
            pwbCurrentPassword.Focus();
        }

    }
}
