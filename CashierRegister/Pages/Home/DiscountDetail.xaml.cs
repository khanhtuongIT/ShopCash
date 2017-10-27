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
using System.Globalization;
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for DiscountDetail.xaml
    /// </summary>
    public partial class DiscountDetail : ModernDialog
    {
        private int discountDetailType = 0;
        private decimal discountDetail = 0;

        //delegate
        public delegate void btnDiscountDetail_Click_Delegate();
        public event btnDiscountDetail_Click_Delegate btndiscountdetail_delegate;

        public DiscountDetail()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            txbAmountPercent.Focus();
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //btnOK_Click
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbAmountPercent.Text.Trim().ToString() != "")
                {
                    if (!decimal.TryParse(txbAmountPercent.Text.Trim(), NumberStyles.AllowDecimalPoint, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR"), out discountDetail))
                    {
                        tblNotification.Text = FindResource("discount_invalid").ToString();
                        txbAmountPercent.Focus();
                        return;
                    }
                    else
                    {
                        //discount is percent
                        if (discountDetailType == 1)
                        {
                            if (discountDetail > 100 || discountDetail < 0)
                            {
                                tblNotification.Text = FindResource("percent_invalid").ToString();
                                txbAmountPercent.Focus();
                                return;
                            }
                        }

                        //discount is amount
                        if (discountDetailType == 0)
                        {
                            if (discountDetail < 0)
                            {
                                tblNotification.Text = FindResource("quantity_invalid").ToString();
                                txbAmountPercent.Focus();
                                return;
                            }

                            if (discountDetail > StaticClass.GeneralClass.list_ec_tb_orderdetail_general[StaticClass.GeneralClass.dtgorderdetail_selectedindex_general].Price)
                            {
                                tblNotification.Text = FindResource("quantity_greater").ToString();
                                txbAmountPercent.Focus();
                                return;
                            }
                        }

                        GeneralClass.discountdetailtype_general = discountDetailType;
                        GeneralClass.discountdetail_general = discountDetail;
                    }
                }
                else
                {
                    GeneralClass.discountdetailtype_general = 0;
                    GeneralClass.discountdetail_general = 0;
                }

                if (btndiscountdetail_delegate != null)
                {
                    btndiscountdetail_delegate();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            discountDetailType = GeneralClass.discountdetailtype_general;
            if(discountDetailType == 0)
            {
                tblAmountPercent.Text = FindResource("enter_amount").ToString();
                tblDiscountDetailType.Text = StaticClass.GeneralClass.currency_setting_general;
                btnAmount.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2b6017");
                btnPercent.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3f7224");
            }
            else
            {
                tblAmountPercent.Text = FindResource("enter_percent").ToString();
                tblDiscountDetailType.Text = "%";
                btnPercent.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2b6017");
                btnAmount.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3f7224");
            }

            txbAmountPercent.Text = StaticClass.GeneralClass.GetNumFormatEdit(StaticClass.GeneralClass.discountdetail_general);
        }

        //btnPercent_Click
        private void btnPercent_Click(object sender, RoutedEventArgs e)
        {
            discountDetailType = 1;
            btnPercent.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2b6017");
            btnAmount.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3f7224");

            tblAmountPercent.Text = FindResource("enter_percent").ToString();
            tblDiscountDetailType.Text = "%";
            txbAmountPercent.Focus();
        }

        //btnAmount_Click
        private void btnAmount_Click(object sender, RoutedEventArgs e)
        {
            discountDetailType = 0;
            btnAmount.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2b6017");
            btnPercent.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3f7224");

            tblAmountPercent.Text = FindResource("enter_amount").ToString();
            tblDiscountDetailType.Text = StaticClass.GeneralClass.currency_setting_general;
            txbAmountPercent.Focus();
        }

        //txbAmountPercent_KeyDown
        private void txbAmountPercent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
                btnOK_Click(null, null);
        }

        //txbAmountPercent_PreviewTextInput
        private void txbAmountPercent_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string value = (sender as TextBox).Text;

            System.Text.RegularExpressions.Regex regex = null;
            if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0")
            {
                if (e.Text == "." && value.IndexOf(e.Text) > -1)
                {
                    e.Handled = true;
                    return;
                }

                regex = new System.Text.RegularExpressions.Regex("^[0-9 .]$");
                e.Handled = !regex.IsMatch(e.Text);
            }
            else
            {
                if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "1")
                {
                    if (e.Text == "," && value.IndexOf(e.Text) > -1)
                    {
                        e.Handled = true;
                        return;
                    }

                    regex = new System.Text.RegularExpressions.Regex("^[0-9 ,]$");
                    e.Handled = !regex.IsMatch(e.Text);
                }
            }
        }

    }
}
