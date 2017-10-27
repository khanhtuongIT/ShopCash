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
    /// Interaction logic for AccessSecureApp.xaml
    /// </summary>
    public partial class AccessSecureApp : ModernDialog
    {
        public AccessSecureApp()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.CloseButton };
            this.CloseButton.Content = FindResource("close").ToString();
            this.CloseButton.Width = 80;
        }

        //hplAccessSucureApp_Click
        private void hplAccessSucureApp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.google.com/settings/security/lesssecureapps");
        }
    }
}
