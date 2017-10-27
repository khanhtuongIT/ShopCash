using System;
using System.Collections.Generic;
using FirstFloor.ModernUI.Windows.Controls;
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
using System.Threading;
using CashierRegisterBUS;
using CashierRegisterEntity;
using System.Data;
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private App app = Application.Current as App;

        //using for login
        private Thread thread_login = null;
        private BUS_tb_User bus_tb_user = new BUS_tb_User();

        //Login
        public Login()
        {
            InitializeComponent();
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txbUsername.Focus();
            tblNotification.Text = "";
            txbUsername.Text = "";
            pwbPassword.Password = "";
            mpr.IsActive = false;
            btnLogin.IsEnabled = true;
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            BBCodeBlock bbcodeblock = new BBCodeBlock();
            try
            {
                bbcodeblock.LinkNavigator.Navigate(new Uri(@"/Pages/Home/Home.xaml", UriKind.Relative), this);
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnLogin_Click
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tblNotification.Text = "";
                if (txbUsername.Text.Trim().ToString() == "")
                {
                    tblNotification.Text = FindResource("username_null").ToString();
                    txbUsername.Focus();
                    return;
                }

                /*if (pwbPassword.Password.Trim() == "")
                {
                    tblNotification.Text = FindResource("password_null").ToString();
                    pwbPassword.Focus();
                    return;
                }*/

                else
                {
                    string _user_name = txbUsername.Text.Trim().ToString();
                    string user_name = _user_name.Substring(0, 1).ToUpper() + _user_name.Substring(1).ToLower();
                    string password = GeneralClass.MD5Hash(pwbPassword.Password.Trim().ToString());
                    bool _isLogin = false;
                    thread_login = new Thread(() =>
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            mpr.IsActive = true;
                            btnLogin.IsEnabled = false;
                        }));

                        DataTable dt_user = new DataTable();
                        dt_user = bus_tb_user.GetUser("WHERE [Name] = '" + user_name + "'");
                        if (dt_user.Rows.Count == 1)
                        {
                            if(string.IsNullOrEmpty(pwbPassword.Password.Trim().ToString()) && string.IsNullOrEmpty(dt_user.Rows[0]["Password"].ToString()))
                            {
                                _isLogin = true;
                            }
                            else if(dt_user.Rows[0]["Password"].ToString() == password || pwbPassword.Password.Trim().ToString() == dt_user.Rows[0]["Password"].ToString())
                            {
                                _isLogin = true;
                            }
                        }
                        if (_isLogin)
                        {
                            GeneralClass.user_permission = Convert.ToInt32(dt_user.Rows[0]["ID"].ToString());
                            GeneralClass.id_user_general = Convert.ToInt32(dt_user.Rows[0]["ID"].ToString());
                            GeneralClass.name_user_general = user_name;
                            GeneralClass.password_user_general = dt_user.Rows[0]["Password"].ToString();

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                app.mainWindow.TitleLinks.Remove(app.linkLogin);
                                app.mainWindow.TitleLinks.Add(app.linkAccount);
                                app.linkAccount.DisplayName = Application.Current.FindResource("hi").ToString() + " " + GeneralClass.name_user_general;
                                app.mainWindow.TitleLinks.Add(app.linkLogout);
                                app.mainWindow.MenuLinkGroups.Remove(app.linkGroupLogInOut);
                                app.mainWindow.MenuLinkGroups.Remove(app.linkGroupOption);
                            }));
                            switch (GeneralClass.user_permission)
                            {
                                case (int)GeneralClass.UserPermission.admin: //admin permission
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        app.mainWindow.MenuLinkGroups.Add(app.linkGroupReport);
                                        app.mainWindow.MenuLinkGroups.Add(app.linkGroupSetting);
                                        app.mainWindow.MenuLinkGroups.Add(app.linkGroupChart);
                                        app.mainWindow.MenuLinkGroups.Add(app.linkGroupStatistic);
                                        
                                    }));
                                    break;
                                case (int)GeneralClass.UserPermission.inventory: //inventory permission
                                    this.Dispatcher.Invoke((Action)(() => { app.mainWindow.MenuLinkGroups.Add(app.linkGroupSetting);
                                        
                                    }));
                                    break;
                                case (int)GeneralClass.UserPermission.report: //report permission
                                    this.Dispatcher.Invoke((Action)(() => { app.mainWindow.MenuLinkGroups.Add(app.linkGroupReport);
                                        
                                    }));
                                    break;
                                default:
                                    break;
                            }

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                app.mainWindow.MenuLinkGroups.Add(app.linkGroupOption);
                                app.mainWindow.MenuLinkGroups.Add(app.linkGroupLogInOut);
                            }));

                            //save logs
                            if (StaticClass.GeneralClass.app_settings["isSaveLogs"].ToString()=="True")
                                new System.Threading.Thread(() => { GeneralClass.SaveLogs(user_name, "Logged in", DateTime.Now); }).Start();

                            this.Dispatcher.Invoke((Action)(() => { app.mainWindow.ContentSource = new Uri(@"/Pages/Home/Home.xaml", UriKind.Relative); }));
                            _isLogin = false;
                        }
                        else
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                mpr.IsActive = false;
                                tblNotification.Text = FindResource("username_password_incorrect").ToString();
                                btnLogin.IsEnabled = true;
                                txbUsername.Focus();
                            }));
                        }
                    });
                    thread_login.Start();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //txbUsername_KeyDown
        private void txbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().ToLower() == "return")
                btnLogin_Click(null, null);
        }

        //hplForgotPassword_Click
        private void hplForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BBCodeBlock bbcodeblock = new BBCodeBlock();
                bbcodeblock.LinkNavigator.Navigate(new Uri(@"/Pages/Setting/ConfirmQuestion.xaml", UriKind.Relative), this);
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

    }
}
