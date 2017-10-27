using CashierRegister.Helpers;
using CashierRegister.ViewModel;
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
    /// Interaction logic for GiftCard_Add.xaml
    /// </summary>
    public partial class GiftCard_Add : ModernDialog
    {
        public GiftCard_Add(int param)
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {  };
            GiftCard_AddViewModel.RequestClose += (s, e) => this.Close();
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += OnFocusRequested;
        }
        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "NumOfGiftCard":
                    NumofTexbox.Focus();
                    break;
                case "NumberFocus":
                    AmountTexbox.Focus();
                    break;
                case "ExpireDays":
                    txtDays.Focus();
                    break;
                case "ExpirationDate":
                    dpDate.Focus();
                    break;
            }
        }
    }
}
