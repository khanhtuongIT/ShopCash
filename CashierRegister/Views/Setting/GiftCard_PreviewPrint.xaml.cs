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
    /// Interaction logic for GiftCard_PreviewPrint.xaml
    /// </summary>
    public partial class GiftCard_PreviewPrint : ModernDialog
    {
        public GiftCard_PreviewPrint(List<Int32> param)
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {  };
            GiftCard_AddViewModel.RequestClose += (s, e) => this.Close();
            GiftCard_AddViewModel.getId = param[0];
            GiftCard_AddViewModel.printId.Clear();
            foreach (Int32 j in param)
            {
                GiftCard_AddViewModel.printId.Add(j);
            }
        }
    }
}
