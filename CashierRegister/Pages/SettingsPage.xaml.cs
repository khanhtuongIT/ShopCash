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
using FirstFloor.ModernUI.Presentation;


namespace CashierRegister.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        //SettingsPage
        static public EventHandler ResetLanguageHandler = null;
        public SettingsPage()
        {
            InitializeComponent();
            if(ResetLanguageHandler==null)
                ResetLanguageHandler += onResetLanguageHandler;
        }
        private void onResetLanguageHandler(object sender, EventArgs e)
        {
            l_about.DisplayName = FindResource("about").ToString();
            l_appearance.DisplayName = FindResource("appearance").ToString();
            l_language.DisplayName = FindResource("language").ToString();
            l_help.DisplayName = FindResource("help").ToString();
        }
        //UserControl_Loaded
        private bool flag_check_loaded = false;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;

                if (StaticClass.GeneralClass.help_request)
                {
                    StaticClass.GeneralClass.help_request = false;
                    this.mtSettingsPage.SelectedSource = new Uri(@"/Pages/Settings/Help.xaml", UriKind.Relative);
                }
                else
                {
                    if (StaticClass.GeneralClass.youremail_registered_general != "")
                        this.mtSettingsPage.SelectedSource = new Uri(@"/Pages/Settings/About.xaml", UriKind.Relative);

                    if (StaticClass.GeneralClass.mousedown_logodata)
                    {
                        StaticClass.GeneralClass.mousedown_logodata = false;
                        this.mtSettingsPage.SelectedSource = new Uri(@"/Pages/Settings/Appearance.xaml", UriKind.Relative);
                    }
                }

                //update display name
                l_about.DisplayName = FindResource("about").ToString();
                l_appearance.DisplayName = FindResource("appearance").ToString();
                l_language.DisplayName = FindResource("language").ToString();
                l_help.DisplayName = FindResource("help").ToString();
            }
        }

    }
}
