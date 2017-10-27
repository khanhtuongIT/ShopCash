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
    /// Interaction logic for ChangeInfoOrder.xaml
    /// </summary>
    public partial class ChangeInfoOrder : ModernDialog
    {
        //delegate
        public delegate void hplChangeInfo_Click_Delegate(string str_storename, string str_storeaddress, string str_storephone);
        public event hplChangeInfo_Click_Delegate hplchangeinfo_delegate;

        //ChangeInfoOrder
        public ChangeInfoOrder()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { };
        }

        //btnOK_Click
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbStoreName.Text.Trim().ToString() == "")
                {
                    tblNotification.Text = FindResource("store_name_null").ToString();
                    txbStoreName.Focus();
                    return;
                }

                if (txbStoreAddress.Text.Trim().ToString() == "")
                {
                    tblNotification.Text = FindResource("shop_address_null").ToString();
                    txbStoreAddress.Focus();
                    return;
                }

                if (txbPhone.Text.Trim().ToString() == "")
                {
                    tblNotification.Text = FindResource("phone_null").ToString();
                    txbPhone.Focus();
                    return;
                }

                //save sote info
                StaticClass.GeneralClass.app_settings["storeName"] = txbStoreName.Text.Trim();
                StaticClass.GeneralClass.app_settings["storeAddress"] = txbStoreAddress.Text.Trim();
                StaticClass.GeneralClass.app_settings["storePhone"] = txbPhone.Text.Trim();
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                if (hplchangeinfo_delegate != null)
                {
                    hplchangeinfo_delegate(txbStoreName.Text.Trim().ToString(), txbStoreAddress.Text.Trim().ToString(), txbPhone.Text.Trim().ToString());
                    this.Close();
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

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.txbStoreName.Text = StaticClass.GeneralClass.app_settings["storeName"].ToString();
                this.txbStoreAddress.Text = StaticClass.GeneralClass.app_settings["storeAddress"].ToString();
                this.txbPhone.Text = StaticClass.GeneralClass.app_settings["storePhone"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Notifications", MessageBoxButton.OK);
            }

        }
    }
}
