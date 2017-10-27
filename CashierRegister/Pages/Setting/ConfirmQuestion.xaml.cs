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

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for ConfirmQuestion.xaml
    /// </summary>
    public partial class ConfirmQuestion : UserControl
    {
        //using for user
        private BUS_tb_User bus_tb_user = new BUS_tb_User();

        //ConfirmQuestion
        public ConfirmQuestion()
        {
            InitializeComponent();
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txbQuestion.Focus();
            tblNotification.Text = "";
            txbQuestion.Text = "";
            txbAnswer.Text = "";
        }

        //btnOK_Click
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txbQuestion.Text.Trim().ToString() == "")
            {
                tblNotification.Text = FindResource("question_null").ToString(); ;
                txbQuestion.Focus();
                return;
            }

            if (txbAnswer.Text.Trim().ToString() == "")
            {
                tblNotification.Text = FindResource("answer_null").ToString(); ;
                txbAnswer.Focus();
                return;
            }

            if (CheckResetPassword(txbQuestion.Text.Trim(), txbAnswer.Text.Trim()))
            {
                BBCodeBlock bbcodeblock = new BBCodeBlock();
                bbcodeblock.LinkNavigator.Navigate(new Uri(@"/Pages/Setting/CreateNewPassword.xaml", UriKind.Relative), this);
            }
            else
            {
                System.Data.DataTable tb_user = bus_tb_user.GetUser("WHERE [Name] = 'Admin' AND [Question] = '" + txbQuestion.Text.Trim().ToLower() + "' AND [Answer] = '" + txbAnswer.Text.Trim().ToLower() + "'");

                if (tb_user.Rows.Count == 1)
                {
                    StaticClass.GeneralClass.id_user_general = Convert.ToInt32(tb_user.Rows[0]["ID"].ToString());
                    BBCodeBlock bbcodeblock = new BBCodeBlock();
                    bbcodeblock.LinkNavigator.Navigate(new Uri(@"/Pages/Setting/CreateNewPassword.xaml", UriKind.Relative), this);
                }
                else
                {
                    tblNotification.Text = FindResource("user_info_incorrect").ToString();
                    txbQuestion.Focus();
                }
            }
        }

        //txbQuestion_KeyDown
        private void txbQuestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
                btnOK_Click(null, null);
        }

        //CheckResetPassword
        private bool CheckResetPassword(string customer_mail, string serial_number)
        {
            Pages.CopyRight.SecurityManager scm = new CopyRight.SecurityManager();
            string softname = "Cash Register";
            string license_key_info = softname + "_" + StaticClass.GeneralClass.software_version + "_" + customer_mail;
            if (scm.CheckSerialNumber(license_key_info, serial_number))
                return true;
            else
                return false;
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
