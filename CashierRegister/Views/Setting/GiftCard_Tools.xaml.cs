using CashierRegister.Helpers;
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

namespace CashierRegister.Views.Setting
{
    /// <summary>
    /// Interaction logic for GiftCard_Tools.xaml
    /// </summary>
    public partial class GiftCard_Tools : ModernDialog
    {
        public GiftCard_Tools()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {  };
            ViewModel.GiftCard_AddViewModel.RequestClose += (s, e) => this.Close();
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += OnFocusRequested;
        }
        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ToolCreatedOn":
                    db_createdOn.Focus();
                    break;
                case "ToolCreatedBefore":
                    db_createdBefore.Focus();
                    break;
                case "ToolCreatedAfter":
                    db_createdAfter.Focus();
                    break;
                case "ToolCreatedStart":
                    db_createdStart.Focus();
                    break;
                case "ToolCreatedEnd":
                    db_createdEnd.Focus();
                    break;
            }
        }
    }
}
