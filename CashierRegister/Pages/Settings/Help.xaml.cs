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
using System.Threading;

namespace CashierRegister.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : UserControl
    {
        private bool flag_check_loaded = false;
        private Thread thread_help;
        //Help
        public Help()
        {
            InitializeComponent();
        }

        //UserControl_Loaded
        private void UCHelp_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
            {
                flag_check_loaded = false;
                WBHHelp.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (thread_help != null && thread_help.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_help = new Thread(() =>
                    {
                        flag_check_loaded = true;
                        try
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                this.WBHHelp.Visibility = System.Windows.Visibility.Collapsed;
                                this.mpr.IsActive = true;
                                this.WBHHelp.Navigate("http://ipcamsoft.com/CashRegister/Help/help.html");
                            }));

                            Thread.Sleep(500);

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                this.mpr.IsActive = false;
                                this.WBHHelp.Visibility = System.Windows.Visibility.Visible;
                            }));
                        }
                        catch (Exception ex)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                ModernDialog md = new ModernDialog();
                                md.CloseButton.Content = "Close";
                                md.Title = "Notification";
                                md.Content = "Error: " + ex.Message;
                                md.ShowDialog();
                            }));
                        }
                    });
                    thread_help.Start();
                }
            }           
        }

    }
}
