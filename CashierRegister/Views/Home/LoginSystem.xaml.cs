using CashierRegister.Helpers;
using CashierRegister.StaticClass;
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

namespace CashierRegister.Views.Home
{
    /// <summary>
    /// Interaction logic for LoginSystem.xaml
    /// </summary>
    public partial class LoginSystem : ModernDialog
    {
        public delegate void btnLogin_Sale_Click_Delegate();
        public btnLogin_Sale_Click_Delegate btn_salelogin_delegate;
        //delegate
        static public EventHandler LoginSystemSuccessHandler = null;
        private App app = Application.Current as App;
        static public EventHandler LoginCancelHandler = null;
        static public EventHandler AdminForgotHandler = null;
        public LoginSystem()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {  };
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += OnFocusRequested;
            ViewModel.Home.LoginSystem_VM.RequestClose += (s, e) => this.Close();
            if(LoginSystemSuccessHandler==null)
                LoginSystemSuccessHandler += onLoginSystemSuccessHandler;
            if (LoginCancelHandler == null)
                LoginCancelHandler += onLoginCancelHandler;
            if (AdminForgotHandler == null)
                AdminForgotHandler += onAdminForgotHandler;
        }
        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SellerName":
                    txbUsername.Focus();
                    break;
                case "SellerPassword":
                    pwbPassword.Focus();
                    break;
            }
        }
        private void onLoginSystemSuccessHandler(object sender, EventArgs e)
        {
            if(StaticClass.GeneralClass.user_permission != 5)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    app.mainWindow.TitleLinks.Remove(app.linkLogin);
                    app.mainWindow.TitleLinks.Add(app.linkAccount);
                    app.linkAccount.DisplayName = Application.Current.FindResource("hi").ToString() + " " + GeneralClass.name_user_general;
                    app.mainWindow.TitleLinks.Add(app.linkLogout);
                    app.mainWindow.MenuLinkGroups.Remove(app.linkGroupLogInOut);
                    app.mainWindow.MenuLinkGroups.Remove(app.linkGroupOption);
                }));
            }
            string _isPage = string.Empty;
            switch (GeneralClass.user_permission)
            {
                case (int)GeneralClass.UserPermission.admin: //admin permission
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        app.mainWindow.MenuLinkGroups.Add(app.linkGroupReport);
                        app.mainWindow.MenuLinkGroups.Add(app.linkGroupSetting);
                        app.mainWindow.MenuLinkGroups.Add(app.linkGroupChart);
                        app.mainWindow.MenuLinkGroups.Add(app.linkGroupStatistic);
                        _isPage = @"/Pages/Home/Home.xaml";
                    }));
                    break;
                case (int)GeneralClass.UserPermission.inventory: //inventory permission
                    this.Dispatcher.Invoke((Action)(() => { app.mainWindow.MenuLinkGroups.Add(app.linkGroupSetting);
                        _isPage = @"/Pages/Setting/Product.xaml";
                    }));
                    break;
                case (int)GeneralClass.UserPermission.report: //report permission
                    this.Dispatcher.Invoke((Action)(() => { app.mainWindow.MenuLinkGroups.Add(app.linkGroupReport);
                        _isPage = @"/Pages/Report/All.xaml";
                    }));
                    break;
                default:
                    this.Dispatcher.Invoke((Action)(() => {
                        _isPage = @"/Pages/Home/Home.xaml";
                        if (btn_salelogin_delegate != null)
                        {
                            btn_salelogin_delegate();
                        }
                    }));
                    
                    break;
            }
            if (StaticClass.GeneralClass.user_permission != 5)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    app.mainWindow.MenuLinkGroups.Add(app.linkGroupOption);
                    app.mainWindow.MenuLinkGroups.Add(app.linkGroupLogInOut);
                }));
            }

            //save logs
            if (StaticClass.GeneralClass.app_settings["isSaveLogs"].ToString() == "True")
                new System.Threading.Thread(() => { GeneralClass.SaveLogs((string)sender, "Logged in", DateTime.Now); }).Start();

            this.Dispatcher.Invoke((Action)(() => { app.mainWindow.ContentSource = new Uri(_isPage, UriKind.Relative); }));
        }

        private void onLoginCancelHandler(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => { app.mainWindow.ContentSource = new Uri(@"/Pages/SettingsPage.xaml", UriKind.Relative); }));
        }
        private void onAdminForgotHandler(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => { app.mainWindow.ContentSource = new Uri(@"/Pages/Setting/ConfirmQuestion.xaml", UriKind.Relative); }));
        }
    }
}
