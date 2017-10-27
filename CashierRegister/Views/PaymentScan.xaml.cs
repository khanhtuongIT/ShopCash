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

namespace CashierRegister.Views
{
    /// <summary>
    /// Interaction logic for PaymentScan.xaml
    /// </summary>
    public partial class PaymentScan : ModernDialog
    {
        public PaymentScan(Int32 _scanId, string _strRef, string _strComp)
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {  };
            PaymentScanViewModel.RequestClose += (s, e) => this.Close();
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += OnFocusRequested;
            PaymentScanViewModel.ScanIndex = _scanId;
            PaymentScanViewModel.ScanRef = _strRef;
            PaymentScanViewModel.StrCompare = _strComp;
        }
        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Barcode":
                    try { txtBarcode.Focus(); }
                    catch { }
                    break;
                case "UseValue":
                    try { txtUseValue.Focus(); }
                    catch { }
                    break;
            }
        }
    }
}
