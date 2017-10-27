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
using System.Data;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : UserControl
    {
        //using for user
        private BUS_tb_User bus_tb_user = new BUS_tb_User();
        private DataTable dttb_user = new DataTable();
        private bool flag_zero_load = false;
        private System.Windows.Media.Imaging.BitmapImage ok_bitmapimage = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(@"pack://application:,,,/Resources/ok.png", UriKind.Absolute));
        private System.Windows.Media.Imaging.BitmapImage failed_bitmapimage = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(@"pack://application:,,,/Resources/failed.png", UriKind.Absolute));

        //Account
        public Account()
        {
            InitializeComponent();
        }

        //hplChangePassWord_Click
        private void hplChangePassWord_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                BBCodeBlock bbcodeblock = new BBCodeBlock();
                bbcodeblock.LinkNavigator.Navigate(new Uri("/Pages/Setting/ChangePassword.xaml", UriKind.Relative), this);
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
            flag_zero_load = false;
            //handle control
            HandleControl();
            try
            {
                dttb_user = bus_tb_user.GetUser("WHERE [ID]=" + StaticClass.GeneralClass.id_user_general);
                if (dttb_user.Rows.Count > 0)
                {
                    tblID.Text = dttb_user.Rows[0]["ID"].ToString();
                    tblUserName.Text = dttb_user.Rows[0]["Name"].ToString();
                    txbEmail.Text = dttb_user.Rows[0]["Email"].ToString();
                    txbAddress.Text = dttb_user.Rows[0]["Address"].ToString();
                    txbQuestion.Text = dttb_user.Rows[0]["Question"].ToString();
                    txbAnswer.Text = dttb_user.Rows[0]["Answer"].ToString();

                    flag_zero_load = true;
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //HandleControl
        private void HandleControl()
        {
            imgEmail.Visibility = System.Windows.Visibility.Hidden;
            imgAddress.Visibility = System.Windows.Visibility.Hidden;
            imgQuestion.Visibility = System.Windows.Visibility.Hidden;
            imgAnswer.Visibility = System.Windows.Visibility.Hidden;
        }

        //txbEmail_TextChanged
        private void txbEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (flag_zero_load == true)
            {
                try
                {
                    EC_tb_User ec_tb_user = GetUser();
                    HandleControl();
                    imgEmail.Visibility = System.Windows.Visibility.Visible;

                    if (bus_tb_user.UpdateUser(ec_tb_user, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        imgEmail.Source = ok_bitmapimage;
                    else
                        imgEmail.Source = failed_bitmapimage;
                }
                catch (Exception ex)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = ex.Message;
                    page.ShowDialog();
                }
            }
        }

        //txbAddress_TextChanged
        private void txbAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (flag_zero_load == true)
            {
                try
                {
                    EC_tb_User ec_tb_user = GetUser();
                    HandleControl();
                    imgAddress.Visibility = System.Windows.Visibility.Visible;

                    if (bus_tb_user.UpdateUser(ec_tb_user, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        imgAddress.Source = ok_bitmapimage;
                    else
                        imgAddress.Source = failed_bitmapimage;
                }
                catch (Exception ex)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = ex.Message;
                    page.ShowDialog();
                }
            }
        }

        //txbQuestion_TextChanged
        private void txbQuestion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (flag_zero_load == true)
            {
                try
                {
                    EC_tb_User ec_tb_user = GetUser();
                    HandleControl();
                    imgQuestion.Visibility = System.Windows.Visibility.Visible;

                    if (bus_tb_user.UpdateUser(ec_tb_user, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        imgQuestion.Source = ok_bitmapimage;
                    else
                        imgQuestion.Source = failed_bitmapimage;
                }
                catch (Exception ex)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = ex.Message;
                    page.ShowDialog();
                }
            }
        }

        //txbAnswer_TextChanged
        private void txbAnswer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (flag_zero_load == true)
            {
                try
                {
                    EC_tb_User ec_tb_user = GetUser();
                    HandleControl();
                    imgAnswer.Visibility = System.Windows.Visibility.Visible;

                    if (bus_tb_user.UpdateUser(ec_tb_user, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        imgAnswer.Source = ok_bitmapimage;
                    else
                        imgAnswer.Source = failed_bitmapimage;
                }
                catch (Exception ex)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = ex.Message;
                    page.ShowDialog();
                }
            }
        }

        //GetUser
        private EC_tb_User GetUser()
        {
            EC_tb_User ec_tb_user = new EC_tb_User();
            ec_tb_user.ID = Convert.ToInt32(tblID.Text.Trim().ToString());
            ec_tb_user.Name = StaticClass.GeneralClass.HandlingSpecialCharacter(tblUserName.Text.Trim().ToString());
            ec_tb_user.Email = StaticClass.GeneralClass.HandlingSpecialCharacter(txbEmail.Text.Trim().ToString());
            ec_tb_user.Address = StaticClass.GeneralClass.HandlingSpecialCharacter(txbAddress.Text.Trim().ToString());
            ec_tb_user.Password = StaticClass.GeneralClass.HandlingSpecialCharacter(StaticClass.GeneralClass.password_user_general);
            ec_tb_user.Question = StaticClass.GeneralClass.HandlingSpecialCharacter(txbQuestion.Text.Trim().ToLower());
            ec_tb_user.Answer = StaticClass.GeneralClass.HandlingSpecialCharacter(txbAnswer.Text.Trim().ToLower());

            return ec_tb_user;
        }

    }
}
