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
using CashierRegister.StaticClass;
using System.Globalization;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for DiscountDetail.xaml
    /// </summary>
    public partial class Discount : ModernDialog
    {
        private int discountType = 0;
        private decimal discount = 0;

        //delegate
        public delegate void btnDiscount_Click_Delegate();
        public event btnDiscount_Click_Delegate btndiscount_delegate;

        //Discount
        public Discount()
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

        //txbAmountPercent_KeyDown
        private void txbAmountPercent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
                btnOK_Click(null, null);
        }

        //btnOK_Click
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbAmountPercent.Text.Trim() != "")
                {
                    if (!decimal.TryParse(txbAmountPercent.Text.Trim(), NumberStyles.AllowDecimalPoint, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR"), out discount))
                    {
                        tblNotification.Text = FindResource("discount_invalid").ToString();
                        txbAmountPercent.Focus();
                        return;
                    }

                    else
                    {
                        //discount is percent
                        if (discountType == 1)
                        {
                            if (discount > 100 || discount < 0)
                            {
                                tblNotification.Text = FindResource("percent_invalid").ToString();
                                txbAmountPercent.Focus();
                                return;
                            }
                        }

                        //discount is amount
                        else
                        {
                            if (discount < 0)
                            {
                                tblNotification.Text = FindResource("quantity_invalid").ToString();
                                txbAmountPercent.Focus();
                                return;
                            }
                            else
                            {
                                if (discount > GeneralClass.subtotal_general)
                                {
                                    tblNotification.Text = FindResource("subtotal_greater").ToString();
                                    txbAmountPercent.Focus();
                                    return;
                                }
                            }
                        }
                    }

                    GeneralClass.discounttype_general = discountType;
                    GeneralClass.discount_general = discount;
                }
                else
                {
                    GeneralClass.discounttype_general = 0;
                    GeneralClass.discount_general = 0;
                }

                if (btndiscount_delegate != null)
                {
                    btndiscount_delegate();
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
            try
            {
                discountType = GeneralClass.discounttype_general;
                
                //discount is amount
                if(discountType == 0)
                {
                    tblAmountPercent.Text = FindResource("enter_amount").ToString();
                    tblDiscountType.Text = StaticClass.GeneralClass.currency_setting_general;

                    btnAmount.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2b6017");
                    btnPercent.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3f7224");
                }

                //discount is percent
                else
                {
                    tblAmountPercent.Text = FindResource("enter_percent").ToString();
                    tblDiscountType.Text = "%";

                    btnPercent.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2b6017");
                    btnAmount.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3f7224");
                }

                txbAmountPercent.Text = StaticClass.GeneralClass.GetNumFormatEdit(StaticClass.GeneralClass.discount_general);
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        //btnAmount_Click
        private void btnAmount_Click(object sender, RoutedEventArgs e)
        {
            discountType = 0;
            btnAmount.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2b6017");
            btnPercent.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3f7224");

            tblAmountPercent.Text = FindResource("enter_amount").ToString();
            tblDiscountType.Text = StaticClass.GeneralClass.currency_setting_general;
            txbAmountPercent.Focus();
        }

        //btnPercent_Click
        private void btnPercent_Click(object sender, RoutedEventArgs e)
        {
            discountType = 1;
            btnPercent.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2b6017");
            btnAmount.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3f7224");

            tblAmountPercent.Text = FindResource("enter_percent").ToString();
            tblDiscountType.Text = "%";
            txbAmountPercent.Focus();
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
