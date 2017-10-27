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

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for ConfirmLogout.xaml
    /// </summary>
    public partial class ConfirmLogout : ModernDialog
    {
        //delegate
        public delegate void btnSalesperson_Click_Delegate();
        public event btnSalesperson_Click_Delegate btnsalesperson_delegate;
        private App app = Application.Current as App;

        //ConfirmLogout
        public ConfirmLogout()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            btnLogout.Focus();
            tblConfirm.Text = FindResource("are_you_sure_logout").ToString();
        }

        //btnLogout_Click
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (btnsalesperson_delegate != null)
            {
                StaticClass.GeneralClass.user_permission = (int)StaticClass.GeneralClass.UserPermission.user;
                string user_name = StaticClass.GeneralClass.salespersonname_login_general;
                StaticClass.GeneralClass.flag_salespersonlogin_general = false;
                StaticClass.GeneralClass.salespersonid_login_general = 0;
                StaticClass.GeneralClass.salespersonname_login_general = "None";
                btnsalesperson_delegate();
                //save logs
                if (StaticClass.GeneralClass.app_settings["isSaveLogs"].ToString()=="True")
                    new System.Threading.Thread(() => { StaticClass.GeneralClass.SaveLogs(user_name + " (salesperson)", "Logged out", DateTime.Now); }).Start();
                this.Dispatcher.Invoke((Action)(() => { app.mainWindow.ContentSource = new Uri(@"/Pages/SettingsPage.xaml", UriKind.Relative); }));
                this.Close();
            }
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
