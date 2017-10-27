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
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CashierRegisterBUS;
using CashierRegisterEntity;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for CreateNewPassword.xaml
    /// </summary>
    public partial class CreateNewPassword : UserControl
    {
        //using for user
        private BUS_tb_User bus_tb_user = new BUS_tb_User();

        //CreateNewPassword
        public CreateNewPassword()
        {
            InitializeComponent();
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tblNotification.Text = "";
            pwbNewPassword.Password = "";
            pwbConfirmPassword.Password = "";
            pwbNewPassword.Focus();
        }

        //btnChange_Click
        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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

                if (pwbNewPassword.Password.Trim() != pwbConfirmPassword.Password.Trim().ToString())
                {
                    tblNotification.Text = FindResource("new_password_confirm_password_incorrect").ToString();
                    pwbNewPassword.Focus();
                    return;
                }
              
                else
                {
                    EC_tb_User ec_tb_user = new EC_tb_User();
                    ec_tb_user.ID = 1;
                    ec_tb_user.Password = StaticClass.GeneralClass.MD5Hash(pwbNewPassword.Password.Trim());
                    if (bus_tb_user.UpdatePasswordUser(ec_tb_user, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {
                        BBCodeBlock bbcodeblock = new BBCodeBlock();
                        bbcodeblock.LinkNavigator.Navigate(new Uri(@"/Pages/Home/Login.xaml", UriKind.Relative), this);
                    }
                }
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        //pwbNewPassword_KeyDown
        private void pwbNewPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
                btnChange_Click(null, null);
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BBCodeBlock bbcodeblock = new BBCodeBlock();
                bbcodeblock.LinkNavigator.Navigate(new Uri(@"/Pages/Home/Login.xaml", UriKind.Relative), this);
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

    }
}
