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
using System.Windows.Threading;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for EmailSent.xaml
    /// </summary>
    public partial class EmailSent : ModernDialog
    {
        //delegate
        public delegate void EmailSent_Delegate();
        public event EmailSent_Delegate emailsent_delegate;

        private DispatcherTimer dist = new DispatcherTimer();

        //EmailSent
        public EmailSent()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            dist.Interval = TimeSpan.FromSeconds(1);
            dist.Tick += new System.EventHandler(CloseForm);
            dist.Start();
        }

        //CloseForm
        private void CloseForm(object sender, EventArgs e)
        {
            if (emailsent_delegate != null)
            {
                dist.Stop();
                emailsent_delegate();
                this.Close();
            }
        }

        //ModernDialog_Closed
        private void ModernDialog_Closed(object sender, EventArgs e)
        {
            if (emailsent_delegate != null)
            {
                dist.Stop();
                emailsent_delegate();
                this.Close();
            }
        }
    }
}
