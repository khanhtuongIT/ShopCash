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
    /// Interaction logic for SalespersonShow.xaml
    /// </summary>
    public partial class SalespersonShow : ModernDialog
    {
        private System.Windows.Threading.DispatcherTimer dispacher_timer = new System.Windows.Threading.DispatcherTimer();

        //delegate
        public delegate void btnSalespersonShow_Delegate();
        public event btnSalespersonShow_Delegate btnsalespersonshow_delegate;

        public SalespersonShow()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            this.Title = "Notification";
            this.Content = "You need to login before billing!";
        }

        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            dispacher_timer.Interval = TimeSpan.FromSeconds(1);
            dispacher_timer.Tick += new EventHandler(ModernDialog_Closed);
            dispacher_timer.Start();
        }

        private void ModernDialog_Closed(object sender, EventArgs e)
        {
            dispacher_timer.Stop();
            if (btnsalespersonshow_delegate != null)
            {
                this.Close();
                btnsalespersonshow_delegate();
            }
        }
    }
}
