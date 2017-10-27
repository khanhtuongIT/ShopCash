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
    /// Interaction logic for GiftCard_Edit.xaml
    /// </summary>
    public partial class GiftCard_Edit : ModernDialog
    {
        public GiftCard_Edit(List<Int32> param)
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {  };
            GiftCard_AddViewModel.RequestClose += (s, e) => this.Close();
            GiftCard_AddViewModel.getId = param[0];
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += OnFocusRequested;
        }
        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "NumOfGiftCard":
                    NumofTexbox.Focus();
                    //NumofTexbox.SelectAll();
                    break;
                case "NumberFocus":
                    AmountTexbox.Focus();
                    //AmountTexbox.SelectAll();
                    break;
            }
        }
    }
}
