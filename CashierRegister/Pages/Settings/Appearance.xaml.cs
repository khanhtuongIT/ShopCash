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

namespace CashierRegister.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Appearance.xaml
    /// </summary>
    public partial class Appearance : UserControl
    {
        public Appearance()
        {
            InitializeComponent();

            // create and assign the appearance view model
            this.DataContext = new AppearanceViewModel();
        }

        //lbAccentColor_SelectionChanged
        private void lbAccentColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                StaticClass.GeneralClass.app_settings["accentColor"] = lbAccentColor.SelectedValue.ToString();
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                StaticClass.GeneralClass.accent_color_change = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error change accent color: " + ex.Message);
            }

        }

        //UCAppearance_Loaded
        private bool flag_check_loaded = false;
        private void UCAppearance_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                StaticClass.GeneralClass.current_page_active = @"/Pages/Settings/Appearance.xaml";
            }
        }
    }
}
