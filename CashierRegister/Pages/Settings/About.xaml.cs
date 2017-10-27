using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Presentation;
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
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        //check version
        private string old_version = "2.1.5";
        private Thread thread_checkversion = null;

        //About
        private Thread thread_initialize = null;

        public About()
        {
            InitializeComponent();
        }
        
        //UserAbout_Loaded
        private bool flag_check_loaded = false;
        private void UserAbout_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;

                if (thread_initialize != null && thread_initialize.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_initialize = new Thread(() =>
                    {
                        try
                        {
                            StaticClass.GeneralClass.current_page_active = @"/Pages/Settings/About.xaml";

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                tblCheckForUpdate.Visibility = System.Windows.Visibility.Visible;
                                stpCheckForUpdate.Visibility = System.Windows.Visibility.Collapsed;
                                tblOldVersion.Visibility = System.Windows.Visibility.Collapsed;
                                stpClickToDownload.Visibility = System.Windows.Visibility.Collapsed;
                                tblPleaseWait.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(StaticClass.GeneralClass.app_settings["accentColor"].ToString()));

                                tblVersion.Text = old_version;
                                if(StaticClass.GeneralClass.isFullVersion || StaticClass.GeneralClass.youremail_registered_general != "")
                                {
                                    tblSoftwareName.Text = "Cash Register Pro";
                                    tblLicense.Text = "Pro version";
                                    stpRegister.Visibility = Visibility.Collapsed;
                                    stpPurchase.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    tblSoftwareName.Text = "Cash Register";
                                    tblLicense.Text = "Unregistered version";
                                    stpRegister.Visibility = Visibility.Collapsed;
                                    stpPurchase.Visibility = Visibility.Visible;
                                }
                                String url = StaticClass.GeneralClass.isPreActiveVersion ? "https://www.amazon.com/s/ref=sr_nr_seeall_48?rh=k%3AIPCamSoft.com%2Ci%3Asoftware&keywords=IPCamSoft.com&ie=UTF8&qid=1465975423" : "http://ipcamsoft.com/?index&desktop";
                                hplClickToDownload.NavigateUri = new Uri(@url);

                                //move to register
                                if (StaticClass.GeneralClass.register_request)
                                {
                                    StaticClass.GeneralClass.register_request = false;
                                    hplRegister_Click(null, null);
                                }
                            }));
                        }
                        catch (Exception ex)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                ModernDialog md = new ModernDialog();
                                md.Title = FindResource("notification").ToString();
                                md.Content = ex.Message;
                                md.CloseButton.Content = FindResource("close").ToString();
                                md.ShowDialog();
                            }));
                        }
                    });
                    thread_initialize.Start();
                }
            }
        }

        //hplRegister_Click
        private void hplRegister_Click(object sender, RoutedEventArgs e)
        {
            BBCodeBlock bbcodeblock = new BBCodeBlock();
            bbcodeblock.LinkNavigator.Navigate(new Uri(@"/Pages/CopyRight/Register.xaml", UriKind.Relative), this);
        }

        //hplCheckForUpdate_Click
        private void hplCheckForUpdate_Click(object sender, RoutedEventArgs e)
        {
            //check version
            if (thread_checkversion != null && thread_checkversion.ThreadState == ThreadState.Running) { }
            else
            {
                thread_checkversion = new Thread(() =>
                {
                    try
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.tblCheckForUpdate.Visibility = System.Windows.Visibility.Collapsed;
                            this.stpCheckForUpdate.Visibility = System.Windows.Visibility.Visible;
                        }));

                        System.Xml.Linq.XDocument xdocument = System.Xml.Linq.XDocument.Load(StaticClass.GeneralClass.isPreActiveVersion? "http://ipcamsoft.com/CashRegister/versionPro":"http://ipcamsoft.com/CashRegister/version");
                        System.Xml.Linq.XElement xelement = xdocument.Element("CurrentVersion");
                        var new_version = xelement.Value;

                        if (new_version.CompareTo(old_version) == 1)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                stpCheckForUpdate.Visibility = System.Windows.Visibility.Collapsed;
                                stpClickToDownload.Visibility = System.Windows.Visibility.Visible;
                            }));
                        }
                        else
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                stpCheckForUpdate.Visibility = System.Windows.Visibility.Collapsed;
                                tblOldVersion.Visibility = System.Windows.Visibility.Visible;
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_checkversion.Start();
            }
        }

        //UserControl_LayoutUpdated
        private Thread thread_layoutupdate = null;
        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            if (thread_layoutupdate != null && thread_layoutupdate.ThreadState == ThreadState.Running) { }
            else
            {
                thread_layoutupdate = new Thread(() =>
                {
                    try
                    {
                        //check change accent color
                        if (StaticClass.GeneralClass.accent_color_change == true)
                            this.tblPleaseWait.Dispatcher.Invoke((Action)(() => { this.tblPleaseWait.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(StaticClass.GeneralClass.app_settings["accentColor"].ToString())); }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_layoutupdate.Start();
            }
        }

        //hplClickToDownload_RequestNavigate
        private void hplClickToDownload_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch (Exception ex)
            {
                ModernDialog md = new ModernDialog();
                md.Title = FindResource("notification").ToString();
                md.Content = ex.Message;
                md.CloseButton.Content = FindResource("close").ToString();
                md.ShowDialog();
            }
        }

        //hplPurchaseNow_Click
        private void hplPurchaseNow_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ipcamsoft.com/?download&purchasecashregister");
        }

        //hplWebsite_Click
        private void hplWebsite_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ipcamsoft.com");
        }

        //imgMicrosoftStore_MouseDown
        private void imgMicrosoftStore_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ipcamsoft.com/?index&desktop");
        }

        //imgGooglePlay_MouseDown
        private void imgGooglePlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ipcamsoft.com/?index&android");
        }

        //imgAppStore_MouseDown
        private void imgAppStore_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ipcamsoft.com/?index&ios");
        }

        //imgAmazonStore_MouseDown
        private void imgAmazonStore_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ipcamsoft.com/?index&kindle");
        }
    }
}
