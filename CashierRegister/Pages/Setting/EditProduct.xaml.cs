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
using System.Globalization;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class EditProduct : ModernDialog
    {
        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();
        private BUS_tb_InputHistory bus_tb_input_history = new BUS_tb_InputHistory();
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        public string category_name = "";

        public string barcodeid = "";
        public string short_name = "";
        public string long_name = "";
        public string cost = "0";
        public string price = "0";
        public string inventory_count = "0";
        public string country = "";
        public string size_weight = "";
        public bool tax = false;
        public bool active = false;

        //delegate
        public delegate void btnEdit_Click_Delegate(bool flag_edited);
        public event btnEdit_Click_Delegate btnedit_delegate;

        //AddProduct
        public EditProduct()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            barcodeid = txbBarcode.Text.Trim().ToString();
            short_name = txbShortName.Text.Trim().ToString();
            long_name = txbLongName.Text.Trim().ToString();
            cost = txbCost.Text.Trim().ToString();
            price = txbPrice.Text.Trim().ToString();
            inventory_count = txbInventoryCount.Text.Trim().ToString();
            country = txbCountry.Text.Trim().ToString();
            size_weight = txbSize_Weight.Text.Trim().ToString();
            tax = chkTax.IsChecked.Value;
            active = chkActive.IsChecked.Value;

            txbShortName.Focus();
        }

        //imgProduct_MouseDown
        private string path_image = "";
        private void imgProduct_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txbBarcode.IsReadOnly = true;

            //choseen image for new product
            System.Windows.Forms.OpenFileDialog openfiledialog = new System.Windows.Forms.OpenFileDialog();
            openfiledialog.Title = FindResource("img_new_product").ToString();// "Select image for new product";
            openfiledialog.Filter = "Image file(*.png, *.jpg)|*.png; *.jpg"; 
            openfiledialog.RestoreDirectory = true;

            if (openfiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path_image = openfiledialog.FileName;
                BitmapImage bitmap_image = new BitmapImage(new Uri(path_image));
                imgProduct.Source = bitmap_image;
            }
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CashierRegisterEntity.EC_tb_Product ec_tb_product = CheckInput();

                if (ec_tb_product != null)
                {
                    if (bus_tb_product.UpdateProduct(ec_tb_product, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {

                        if (path_image != "")
                        {
                            if (!System.IO.Directory.Exists(current_directory + @"\images"))
                                System.IO.Directory.CreateDirectory(current_directory + @"\images");

                            if (path_image != StaticClass.GeneralClass.productpathimage_general)
                                System.IO.File.Copy(path_image, current_directory + ec_tb_product.PathImage, true);
                        }

                        if ((barcodeid != txbBarcode.Text.Trim().ToString()) || (short_name != txbShortName.Text.Trim().ToString()) || (long_name != txbLongName.Text.Trim().ToString())
                                    || (cost != txbCost.Text.Trim().ToString()) || (price != txbPrice.Text.Trim().ToString()) || (inventory_count != txbInventoryCount.Text.Trim().ToString()) != (country != txbCountry.Text.Trim().ToString())
                                    || (size_weight != txbSize_Weight.Text.Trim().ToString()) || (tax != chkTax.IsChecked.Value) || (active != chkActive.IsChecked.Value))
                        {
                            //insert history
                            EC_tb_InputHistory history = new EC_tb_InputHistory();
                            history.ProductID = ec_tb_product.ProductID;
                            history.ProductName = ec_tb_product.ShortName;
                            history.InputDate = System.DateTime.Now.ToString();
                            history.UserID = StaticClass.GeneralClass.id_user_general;
                            history.UserName = StaticClass.GeneralClass.name_user_general;
                            history.Cost = ec_tb_product.Cost;
                            history.Price = ec_tb_product.Price;
                            history.InventoryCount = ec_tb_product.InventoryCount;
                            history.CategoryID = ec_tb_product.CategoryID;
                            history.CategoryName = category_name;
                            history.Tax = ec_tb_product.Tax;
                            history.Active = ec_tb_product.Active;
                            history.Country = ec_tb_product.Country;
                            history.SizeWeight = ec_tb_product.SizeWeight;

                            bus_tb_input_history.InsertInputHistory(history, StaticClass.GeneralClass.flag_database_type_general);
                        }

                        if (btnedit_delegate != null)
                        {
                            StaticClass.GeneralClass.flag_add_edit_delete_product_general = true;
                            btnedit_delegate(true);
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

        //check input data
        private CashierRegisterEntity.EC_tb_Product CheckInput()
        {
            var product = new CashierRegisterEntity.EC_tb_Product();

            try
            {
                if (txbShortName.Text == "")
                {
                    tblNotification.Text = FindResource("have_not_enter_short_name").ToString();
                    txbShortName.Focus();
                    return null;
                }

                if (txbLongName.Text == "")
                {
                    tblNotification.Text = FindResource("have_not_enter_long_name").ToString();
                    txbLongName.Focus();
                    return null;
                }

                if (txbCost.Text == "")
                {
                    tblNotification.Text = FindResource("have_not_enter_cost").ToString();
                    txbCost.Focus();
                    return null;
                }

                double cost_temp;
                if (double.TryParse(txbCost.Text, NumberStyles.AllowDecimalPoint, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US"): new CultureInfo("fr-FR"),  out cost_temp) == false || cost_temp.ToString().Length > 12)
                {
                    tblNotification.Text = FindResource("cost_invalid").ToString();
                    txbCost.Focus();
                    return null;
                }

                if (txbPrice.Text == "")
                {
                    tblNotification.Text = FindResource("have_not_enter_price").ToString();
                    txbPrice.Focus();
                    return null;
                }

                double price_temp;
                if (double.TryParse(txbPrice.Text, NumberStyles.AllowDecimalPoint, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR"), out price_temp) == false || price_temp.ToString().Length > 12)
                {
                    tblNotification.Text = FindResource("price_invalid").ToString();
                    txbPrice.Focus();
                    return null;
                }

                if (txbInventoryCount.Text == "")
                {
                    tblNotification.Text = FindResource("have_not_enter_inventory_count").ToString();
                    txbInventoryCount.Focus();
                    return null;
                }

                double inventory_count_temp;
                if (double.TryParse(txbInventoryCount.Text, out inventory_count_temp) == false || inventory_count_temp.ToString().Length > 10)
                {
                    tblNotification.Text = FindResource("inventory_count_invalid").ToString();
                    txbInventoryCount.Focus();
                    return null;
                }

                product.ProductID = StaticClass.GeneralClass.productid_general;
                product.BarcodeID = txbBarcode.Text.Trim().ToString();
                product.ShortName = StaticClass.GeneralClass.HandlingSpecialCharacter(txbShortName.Text.Trim().ToString());
                product.LongName = StaticClass.GeneralClass.HandlingSpecialCharacter(txbLongName.Text.Trim().ToString());
                product.CategoryID = StaticClass.GeneralClass.categoryid_general;
                product.Capture = -1;
                product.Country = StaticClass.GeneralClass.HandlingSpecialCharacter(txbCountry.Text.Trim().ToString());
                product.SizeWeight = StaticClass.GeneralClass.HandlingSpecialCharacter(txbSize_Weight.Text.Trim().ToString());

                if (chkTax.IsChecked == true)
                    product.Tax = 1;
                else
                    product.Tax = 0;

                if (chkActive.IsChecked == true)
                    product.Active = 1;
                else
                    product.Active = 0;

                if (path_image != "" || StaticClass.GeneralClass.productpathimage_general != @"pack://application:,,,/Resources/default_01_default_02_default_03_default.png")
                    product.PathImage = @"\images\" + StaticClass.GeneralClass.productid_general.ToString() + @".png";
                else
                    product.PathImage = "";

                product.Cost = Math.Round(Convert.ToDecimal(txbCost.Text, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new System.Globalization.CultureInfo("fr-FR")), 2);
                product.Price = Math.Round(Convert.ToDecimal(txbPrice.Text, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new System.Globalization.CultureInfo("fr-FR")), 2);
                product.InventoryCount = Convert.ToInt32(txbInventoryCount.Text.ToString());

                return product;
            }

            catch (OverflowException)
            {
                tblNotification.Text = FindResource("inventory_count_invalid").ToString() + " (Max: 2147483647)";
                txbInventoryCount.Focus();
                return null;
            }

            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
                return null;
            }

        }

        //muiBtnScanBarcode_Click
        private void muiBtnScanBarcode_Click(object sender, RoutedEventArgs e)
        {
            txbBarcode.IsReadOnly = false;
            txbBarcode.Focus();
        }

        //txbTextBox_GotFocus
        private void txbTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            txbBarcode.IsReadOnly = true;
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

        //txbInventoryCount_PreviewTextInput
        private void txbInventoryCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[0-9]$");
            e.Handled = !regex.IsMatch(e.Text);
        }

    }
}
