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
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;
using System.Globalization;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for AddCurrency.xaml
    /// </summary>
    public partial class AddCurrency : ModernDialog
    {
        //using for setting
        private BUS_tb_Setting bus_tb_setting = new BUS_tb_Setting();


        //delegate
        public delegate void Add_Delete_Edit_Delegate();
        public event Add_Delete_Edit_Delegate add_delegate;

        //AddCurrency
        public AddCurrency()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            txbCurrency.Focus();
            chkActive.IsChecked = true;
        }

        //btnAdd_Click
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbCurrency.Text.Trim() == "")
                {
                    tblNotification.Text = FindResource("currency_null").ToString();
                    return;
                }

                if (txbTaxRate.Text.Trim() == "")
                {
                    tblNotification.Text = FindResource("tax_rate_null").ToString();
                    return;
                }

                decimal taxrate = 0;
                if (!decimal.TryParse(txbTaxRate.Text.Trim(), NumberStyles.AllowDecimalPoint, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR"), out taxrate))
                {
                    tblNotification.Text = FindResource("tax_rate_invalid").ToString();
                    return;
                }

                if (taxrate >= 100 || taxrate < 0)
                {
                    tblNotification.Text = FindResource("tax_rate_value_invalid").ToString();
                    return;
                }

                int version = 1;
                if(int.TryParse(txbVersion.Text.Trim().ToString(), out version) == false)
                {
                    tblNotification.Text = FindResource("version_invalid").ToString();
                    return;
                }
                else
                {
                    EC_tb_Setting setting = new EC_tb_Setting();
                    setting.Currency = StaticClass.GeneralClass.HandlingSpecialCharacter(txbCurrency.Text.Trim().ToString());
                    setting.TaxRate = taxrate;
                    setting.Version = version;
                    if (chkActive.IsChecked == true)
                    {
                        setting.Active = 1;
                        bus_tb_setting.UpdateSettingActive();
                    }
                    else
                        setting.Active = 0;

                    if (bus_tb_setting.InsertSetting(setting, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {
                        if(setting.Active == 1)
                        {
                            DataTable tb_setting = bus_tb_setting.GetSetting("WHERE [SettingID] = (SELECT MAX([SettingID]) FROM [tb_Currency])");
                            if (tb_setting.Rows.Count == 1)
                            {
                                StaticClass.GeneralClass.flag_add_edit_setting_general = true;
                                StaticClass.GeneralClass.settingid_setting_general = Convert.ToInt32(tb_setting.Rows[0]["SettingID"].ToString());
                                StaticClass.GeneralClass.currency_setting_general = tb_setting.Rows[0]["Currency"].ToString() + " ";
                                StaticClass.GeneralClass.taxrate_setting_general = Convert.ToDecimal(tb_setting.Rows[0]["TaxRate"].ToString());
                                StaticClass.GeneralClass.active_setting_general = Convert.ToInt32(tb_setting.Rows[0]["Active"].ToString());
                                StaticClass.GeneralClass.version_setting_general = Convert.ToInt32(tb_setting.Rows[0]["Version"].ToString());
                            }
                        }

                        if (add_delegate != null)
                        {
                            add_delegate();
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Textbox_PreviewTextInput
        private void Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
