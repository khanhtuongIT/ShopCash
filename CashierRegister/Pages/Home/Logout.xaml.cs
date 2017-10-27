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
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for Logout.xaml
    /// </summary>
    public partial class Logout : UserControl
    {
        private App app = Application.Current as App;

        //Logout
        public Logout()
        {
            InitializeComponent();
        }

        //Grid_Loaded
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            hplYes.Focus();
        }

        //hplYes_Click
        private void hplYes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //add and remove title link
                app.mainWindow.TitleLinks.Remove(app.linkAccount);
                app.mainWindow.TitleLinks.Remove(app.linkLogout);
                app.mainWindow.TitleLinks.Add(app.linkLogin);

                switch (GeneralClass.user_permission)
                {
                    case (int)GeneralClass.UserPermission.admin: //admin permission
                        app.mainWindow.MenuLinkGroups.Remove(app.linkGroupStatistic);
                        app.mainWindow.MenuLinkGroups.Remove(app.linkGroupChart);
                        app.mainWindow.MenuLinkGroups.Remove(app.linkGroupSetting);
                        app.mainWindow.MenuLinkGroups.Remove(app.linkGroupReport);
                        break;
                    case (int)GeneralClass.UserPermission.inventory: //inventory permission
                        app.mainWindow.MenuLinkGroups.Remove(app.linkGroupSetting);
                        break;
                    case (int)GeneralClass.UserPermission.report: //report permission
                        app.mainWindow.MenuLinkGroups.Remove(app.linkGroupReport);
                        break;
                }

                string user_name = GeneralClass.name_user_general;
                if (!StaticClass.GeneralClass.flag_salespersonlogin_general)
                    GeneralClass.user_permission = (int)GeneralClass.UserPermission.user;
                else GeneralClass.user_permission = 5;
                GeneralClass.id_user_general = 0;
                GeneralClass.name_user_general = "";
                GeneralClass.password_user_general = "";

                //save logs
                if (StaticClass.GeneralClass.app_settings["isSaveLogs"].ToString()=="True")
                    new System.Threading.Thread(() => { GeneralClass.SaveLogs(user_name, "Logged out", DateTime.Now); }).Start();
                app.mainWindow.ContentSource = new Uri(@"/Pages/Home/Home.xaml", UriKind.Relative);
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //hplNo_Click
        private void hplNo_Click(object sender, RoutedEventArgs e)
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

    }
}
