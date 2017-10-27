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
using System.Threading;

namespace CashierRegister.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Languages.xaml
    /// </summary>
    public partial class Languages : UserControl
    {
        private App app = Application.Current as App;
        private System.Windows.Media.Imaging.BitmapImage bitmap_language_current = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/current_language.png", UriKind.Absolute));
        private System.Windows.Media.Imaging.BitmapImage bitmap_language = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/language.png", UriKind.Absolute));

        //Languages
        public Languages()
        {
            InitializeComponent();

            imgEnglish.Source = bitmap_language;
            imgVietnamese.Source = bitmap_language;
            imgCzech.Source = bitmap_language;
            Image img = (Image)stpLanguage.FindName("img" + StaticClass.GeneralClass.app_settings["language"].ToString());
            img.Source = bitmap_language_current;
        }

        //imgEnglish_MouseDown
        private void imgEnglish_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeLanguage("English");
        }

        //imgVietnamese_MouseDown
        private void imgVietnamese_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeLanguage("Vietnamese");
        }

        private void imgCzech_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeLanguage("Czech");
        }

        //ChangeLanguage
        private void ChangeLanguage(string str_language)
        {
            if (StaticClass.GeneralClass.app_settings["language"].ToString() != str_language)
            {
                Image img = (Image)stpLanguage.FindName("img" + StaticClass.GeneralClass.app_settings["language"].ToString());
                img.Source = bitmap_language;

                Image img_current = (Image)stpLanguage.FindName("img" + str_language);
                img_current.Source = bitmap_language_current;

                if (StaticClass.GeneralClass.dict_language_current != null)
                    Application.Current.Resources.Remove(StaticClass.GeneralClass.dict_language_current);

                StaticClass.GeneralClass.dict_language_current.Source = new Uri("..\\Languages\\" + str_language + ".xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(StaticClass.GeneralClass.dict_language_current);

                //save current language
                StaticClass.GeneralClass.app_settings["language"] = str_language;
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);

                //change language for links
                app.linkGroupHome.DisplayName = FindResource("lg_home").ToString();
                app.linkGroupReport.DisplayName = FindResource("lg_report").ToString();
                app.linkGroupSetting.DisplayName = FindResource("lg_setting").ToString();
                app.linkGroupChart.DisplayName = FindResource("lg_chart").ToString();
                app.linkGroupStatistic.DisplayName = FindResource("lg_statistic").ToString();
                app.linkGroupOption.DisplayName = FindResource("lg_option").ToString();
                app.linkLogin.DisplayName = FindResource("l_login").ToString();
                app.linkLogout.DisplayName = FindResource("l_logout").ToString();

                app.linkCategory.DisplayName = FindResource("category").ToString();
                app.linkCurrency.DisplayName = FindResource("currency").ToString();
                app.linkCustomer.DisplayName = FindResource("customer").ToString();
                app.linkSalesperson.DisplayName = FindResource("salesperson").ToString();
                app.linkPayment.DisplayName = FindResource("payment").ToString();
                app.linkUser.DisplayName = FindResource("user").ToString();
                app.linkBackup.DisplayName = FindResource("backup_restore").ToString();
                app.linkAppSetting.DisplayName = FindResource("app_setting").ToString();
                app.lnkGiftCard.DisplayName = FindResource("gift_card").ToString();
                SettingsPage.ResetLanguageHandler(null, null);
            }
        }
       
    }
}
