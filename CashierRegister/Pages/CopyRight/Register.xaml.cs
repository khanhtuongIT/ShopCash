using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using FirstFloor.ModernUI.Windows.Controls;
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

namespace CashierRegister.Pages.CopyRight
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        //Register
        public Register()
        {
            InitializeComponent();
        }

        //btnOK_Click
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbYourEmail.Text.Trim().ToString() == "")
                {
                    tblNotification.Text = FindResource("your_email_null").ToString();
                    txbYourEmail.Focus();
                    return;
                }

                if (txbSerialNumber.Text.Trim().ToString() == "")
                {
                    tblNotification.Text = FindResource("serial_number_null").ToString();
                    txbSerialNumber.Focus();
                    return;
                }

                else
                {
                    SecurityManager scm = new SecurityManager();
                    string softname = "Cash Register";
                    string customer_email = txbYourEmail.Text.Trim().ToString();
                    string license_key_info = softname + "_" + StaticClass.GeneralClass.software_version + "_" + customer_email;
                    string serial_number = txbSerialNumber.Text.Trim().ToString();
                    if (scm.CheckSerialNumber(license_key_info, serial_number))
                    {
                        //create register status in regedit
                        Registry.SetValue(StaticClass.GeneralClass.keyname_register_general, StaticClass.GeneralClass.valuename_register_general, "TRUE");

                        //create register info file
                        Pages.CopyRight.SecurityManager security_manager = new Pages.CopyRight.SecurityManager();
                        security_manager.EncryptFile(customer_email, serial_number, "ResInfo", "EncryptReg", StaticClass.GeneralClass.key_register_general);

                        Pages.Notification page = new Pages.Notification();
                        page.tblNotification.Text = FindResource("activation_success").ToString();
                        page.ShowDialog();

                        //set registerd successful
                        StaticClass.GeneralClass.flag_registered_general = true;
                        StaticClass.GeneralClass.youremail_registered_general = txbYourEmail.Text.Trim().ToString();
                        Application.Current.Resources["cash_register"] = "Cash Register Pro";

                        BBCodeBlock bbcodeblock = new BBCodeBlock();
                        bbcodeblock.LinkNavigator.Navigate(new Uri(@"/Pages/Settings/About.xaml", UriKind.Relative), this);
                    }
                    else
                    {
                        //if serial number is invalid
                        Registry.SetValue(StaticClass.GeneralClass.keyname_register_general, StaticClass.GeneralClass.valuename_register_general, "FALSE");

                        Pages.Notification page = new Pages.Notification();
                        page.tblNotification.Text = FindResource("activation_unsuccess").ToString();
                        page.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                //if serial number is invalid and have error
                Registry.SetValue(StaticClass.GeneralClass.keyname_register_general, StaticClass.GeneralClass.valuename_register_general, "FALSE");

                tblNotification.Text = ex.Message;
            }
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tblNotification.Text = "";
            txbYourEmail.Text = "";
            txbSerialNumber.Text = "";
            txbYourEmail.Focus();
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BBCodeBlock bbcodeblock = new BBCodeBlock();
                bbcodeblock.LinkNavigator.Navigate(new Uri(@"/Pages/Settings/About.xaml", UriKind.Relative), this);
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnPurchaseNow_Click
        private void btnPurchaseNow_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ipcamsoft.com/?download&purchasecashregister");
        }
    }
}
