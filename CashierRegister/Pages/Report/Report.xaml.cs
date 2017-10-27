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

namespace CashierRegister.Pages.Report
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : UserControl
    {
        //thread
        private Thread thread_content = null;
        private bool flag_check_loaded = false;

        //Report
        public Report()
        {
            InitializeComponent();

            if (thread_content != null && thread_content.ThreadState == ThreadState.Running) { }
            else
            {
                thread_content = new Thread(() =>
                {
                    this.mtReport.Dispatcher.Invoke((Action)(() =>
                    {
                        this.mtReport.SelectedSource = new Uri("/Pages/Report/All.xaml", UriKind.Relative);
                    }));
                });
                thread_content.Start();
            }
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                lAll.DisplayName = FindResource("all").ToString();
                lCustomers.DisplayName = FindResource("customers").ToString();
                lPayments.DisplayName = FindResource("payments").ToString();
                lSalesperson.DisplayName = FindResource("salespersons").ToString();
            }
        }

    }
}
