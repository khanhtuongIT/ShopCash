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
using System.Threading;
using System.Globalization;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : ModernDialog
    {
        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();
        private BUS_tb_InputHistory bus_tb_input_history = new BUS_tb_InputHistory();
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        public string category_name = "";

        //delegate
        public delegate void muiBtnAdd_Click_Delegate(bool flag_added);
        public event muiBtnAdd_Click_Delegate muibtnadd_delegate;

        //AddProduct
        public AddProduct()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { };
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txbShortName.Focus();
            BitmapImage bitmap = new BitmapImage(new Uri(@"pack://application:,,,/Resources/default_01_default_02_default_03_default.png"));
            imgProduct.Source = bitmap;

            //MessageBox.Show(Properties.Settings.Default.DecimalSeparator.ToString());
        }

        //imgProduct_MouseDown
        private string path_image = "";
        private void imgProduct_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txbBarcode.IsReadOnly = true;

            //choseen image for new product
            System.Windows.Forms.OpenFileDialog openfiledialog = new System.Windows.Forms.OpenFileDialog();
            openfiledialog.Title = "Select image for new product";
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
                var product = CheckInput();

                if (product != null)
                {
                    if (bus_tb_product.InsertProduct(product, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {
                        if (!System.IO.Directory.Exists(current_directory + @"\images"))
                            System.IO.Directory.CreateDirectory(current_directory + @"\images");
                        if (path_image != "")
                            System.IO.File.Copy(path_image, current_directory + @"\images\" + (product.ProductID).ToString() + @".png", true);

                        //insert history
                        EC_tb_InputHistory history = new EC_tb_InputHistory();
                        history.ProductID = product.ProductID;
                        history.ProductName = product.ShortName;
                        history.InputDate = System.DateTime.Now.ToString();
                        history.UserID = StaticClass.GeneralClass.id_user_general;
                        history.UserName = StaticClass.GeneralClass.name_user_general;
                        history.Cost = product.Cost;
                        history.Price = product.Price;
                        history.InventoryCount = product.InventoryCount;
                        history.CategoryID = product.CategoryID;
                        history.CategoryName = StaticClass.GeneralClass.HandlingSpecialCharacter(category_name);
                        history.Tax = product.Tax;
                        history.Active = product.Active;
                        history.Country = product.Country;
                        history.SizeWeight = product.SizeWeight;

                        bus_tb_input_history.InsertInputHistory(history, StaticClass.GeneralClass.flag_database_type_general);

                        if (muibtnadd_delegate != null)
                        {
                            StaticClass.GeneralClass.flag_add_edit_delete_product_general = true;
                            muibtnadd_delegate(true);
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
            CashierRegisterEntity.EC_tb_Product ec_tb_product = new CashierRegisterEntity.EC_tb_Product();

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
                if (double.TryParse(txbCost.Text, NumberStyles.AllowDecimalPoint, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("fr-FR"), out cost_temp) == false || cost_temp.ToString().Length > 12)
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
                if (double.TryParse(txbPrice.Text, NumberStyles.AllowDecimalPoint, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("fr-FR"), out price_temp) == false || price_temp.ToString().Length > 12)
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

                ec_tb_product.ProductID = bus_tb_product.GetMaxProductID("") + 1;
                ec_tb_product.Capture = -1;

                if (chkTax.IsChecked == true)
                    ec_tb_product.Tax = 1;
                else
                    ec_tb_product.Tax = 0;

                if (chkActive.IsChecked == true)
                    ec_tb_product.Active = 1;
                else
                    ec_tb_product.Active = 0;

                if (path_image != "")
                    ec_tb_product.PathImage = @"\images\" + ec_tb_product.ProductID.ToString() + @".png";
                else
                    ec_tb_product.PathImage = "";

                ec_tb_product.Country = StaticClass.GeneralClass.HandlingSpecialCharacter(txbCountry.Text.Trim().ToString());
                ec_tb_product.SizeWeight = StaticClass.GeneralClass.HandlingSpecialCharacter(txbSize_Weight.Text.Trim().ToString());

                ec_tb_product.BarcodeID = txbBarcode.Text.Trim().ToString();
                ec_tb_product.ShortName = StaticClass.GeneralClass.HandlingSpecialCharacter(txbShortName.Text.Trim().ToString());
                ec_tb_product.LongName = StaticClass.GeneralClass.HandlingSpecialCharacter(txbLongName.Text.Trim().ToString());
                ec_tb_product.CategoryID = StaticClass.GeneralClass.categoryid_general;

                ec_tb_product.Cost = Convert.ToDecimal(txbCost.Text, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR"));
                ec_tb_product.Price = Convert.ToDecimal(txbPrice.Text, StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR"));
                ec_tb_product.InventoryCount = Convert.ToInt32(txbInventoryCount.Text);

                return ec_tb_product;
            }
            catch(OverflowException)
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
            muibtnSearch.Visibility = System.Windows.Visibility.Visible;
        }

        //txbTextBox_GotFocus
        private void txbTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            txbBarcode.IsReadOnly = true;
            muibtnSearch.Visibility = System.Windows.Visibility.Collapsed;
        }

        //muibtnSearch_Click
        private Thread thread_search = null;
        private string url = "http://www.upcdatabase.com/item/";
        private string table_class = "<table class=" + '"' + "data" + '"' + ">";
        private string description = "Description";
        private string size_weight = "Size/Weight";
        private string country = "Issuing Country";

        private string str_desciption = "";
        private string str_weight_size = "";
        private string str_country = "";


        private void muibtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (thread_search != null && thread_search.ThreadState == ThreadState.Running) { }
            else
            {
                thread_search = new Thread(() =>
                {
                    try
                    {
                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ""; }));
                        string str_barcode = "";
                        this.txbBarcode.Dispatcher.Invoke((Action)(() => { str_barcode = txbBarcode.Text.Trim().ToString(); }));
                        if (str_barcode == "")
                        {
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("barcode_null").ToString(); }));
                            return;
                        }
                        else
                        {
                            this.muibtnSearch.Dispatcher.Invoke((Action)(() => { this.muibtnSearch.Visibility = System.Windows.Visibility.Collapsed; }));
                            this.mprSearch.Dispatcher.Invoke((Action)(() => { this.mprSearch.IsActive = true; }));
                            this.tblCancel.Dispatcher.Invoke((Action)(() => { this.tblCancel.Visibility = System.Windows.Visibility.Visible; }));

                            if (bus_tb_product.GetProduct("WHERE [BarcodeID] = " + str_barcode).Rows.Count > 0)
                                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("barcode_uses_existing").ToString(); }));
                            else
                            {
                                if (!GetProductInfo(url + str_barcode))
                                {
                                    this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("information_not_found").ToString(); }));
                                    this.tblNotification.Dispatcher.Invoke((Action)(() => { this.txbLongName.Text = ""; }));
                                    this.tblNotification.Dispatcher.Invoke((Action)(() => { this.txbSize_Weight.Text = ""; }));
                                    this.tblNotification.Dispatcher.Invoke((Action)(() => { this.txbCountry.Text = ""; }));
                                }
                            }
                        }
                        this.mprSearch.Dispatcher.Invoke((Action)(() => { this.mprSearch.IsActive = false; }));
                        this.tblCancel.Dispatcher.Invoke((Action)(() => { this.tblCancel.Visibility = System.Windows.Visibility.Collapsed; }));
                        this.muibtnSearch.Dispatcher.Invoke((Action)(() => { this.muibtnSearch.Visibility = System.Windows.Visibility.Visible; }));

                    }
                    catch (Exception ex)
                    {
                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ex.Message; }));
                    }
                });
                thread_search.Start();
            }
        }

        //txbBarcode_KeyDown
        private void txbBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                if (thread_search != null && thread_search.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_search = new Thread(() =>
                    {
                        try
                        {
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ""; }));
                            string str_barcode = "";
                            this.txbBarcode.Dispatcher.Invoke((Action)(() => { str_barcode = txbBarcode.Text.Trim().ToString(); }));
                            if (str_barcode == "")
                            {
                                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("barcode_null").ToString(); }));
                                return;
                            }
                            else
                            {
                                this.muibtnSearch.Dispatcher.Invoke((Action)(() => { this.muibtnSearch.Visibility = System.Windows.Visibility.Collapsed; }));
                                this.mprSearch.Dispatcher.Invoke((Action)(() => { this.mprSearch.IsActive = true; }));
                                this.tblCancel.Dispatcher.Invoke((Action)(() => { this.tblCancel.Visibility = System.Windows.Visibility.Visible; }));

                                if (bus_tb_product.GetProduct("WHERE [BarcodeID] = " + str_barcode).Rows.Count > 0)
                                    this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("barcode_uses_existing").ToString(); }));
                                else
                                {
                                    if (!GetProductInfo(url + str_barcode))
                                    {
                                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("information_not_found").ToString(); }));
                                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.txbLongName.Text = ""; }));
                                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.txbSize_Weight.Text = ""; }));
                                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.txbCountry.Text = ""; }));
                                    }
                                }
                            }
                            this.mprSearch.Dispatcher.Invoke((Action)(() => { this.mprSearch.IsActive = false; }));
                            this.tblCancel.Dispatcher.Invoke((Action)(() => { this.tblCancel.Visibility = System.Windows.Visibility.Collapsed; }));
                            this.muibtnSearch.Dispatcher.Invoke((Action)(() => { this.muibtnSearch.Visibility = System.Windows.Visibility.Visible; }));

                        }
                        catch (Exception ex)
                        {
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ex.Message; }));
                        }
                    });
                    thread_search.Start();
                }
            }
        }

        //hplCancel_Click
        private void hplCancel_Click(object sender, RoutedEventArgs e)
        {
            if (thread_search != null && thread_search.ThreadState == ThreadState.Running)
            {
                thread_search.Abort();
                tblCancel.Visibility = System.Windows.Visibility.Collapsed;
                this.mprSearch.IsActive = false;
                this.muibtnSearch.Visibility = System.Windows.Visibility.Visible;
                txbBarcode.Focus();
                thread_search.Abort();
            }
        }

        //GetProductInfo
        private bool GetProductInfo(string str_url)
        {
            bool flag_check = false;

            try
            {
                using (System.IO.StreamReader str = GetStreamReader(str_url))
                {
                    string line;
                    while ((line = str.ReadLine()) != null)
                    {
                        if (line.Contains(table_class))
                        {
                            while ((line = str.ReadLine()) != null)
                            {
                                if (line.Contains(description))
                                {
                                    flag_check = true;
                                    str_desciption = line;

                                    while ((line = str.ReadLine()) != null)
                                    {
                                        if (line.Contains(size_weight))
                                        {
                                            flag_check = true;
                                            str_weight_size = line;

                                            while ((line = str.ReadLine()) != null)
                                            {
                                                if (line.Contains(country))
                                                {
                                                    flag_check = true;
                                                    str_country = line;
                                                    break;
                                                }
                                            }
                                        }

                                        if (line.Contains(country))
                                        {
                                            flag_check = true;
                                            str_country = line;

                                            while ((line = str.ReadLine()) != null)
                                            {
                                                if (line.Contains(size_weight))
                                                {
                                                    flag_check = true;
                                                    str_weight_size = line;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ex.Message; }));
            }
            return flag_check;
        }

        //getStreamReader
        public System.IO.StreamReader GetStreamReader(string Url)
        {
            System.Net.HttpWebRequest myRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Url);
            myRequest.Method = "GET";
            System.Net.WebResponse myResponse = myRequest.GetResponse();
            System.IO.StreamReader str = new System.IO.StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            return str;
        }

        //ModernDialog_LayoutUpdated
        private void ModernDialog_LayoutUpdated(object sender, EventArgs e)
        {
            if (str_desciption != "")
            {
                int l = str_desciption.Length;
                int app = str_desciption.IndexOf(@"</td><td></td><td>") + 18;
                txbLongName.Text = str_desciption.Substring(app, l - app - 10);
                str_desciption = "";
            }

            if (str_weight_size != "")
            {
                int l = str_weight_size.Length;
                int app = str_weight_size.IndexOf(@"</td><td></td><td>") + 18;
                txbSize_Weight.Text = str_weight_size.Substring(app, l - app - 10);
                str_weight_size = "";
            }

            if (str_country != "")
            {
                int l = str_country.Length;
                int app = str_country.IndexOf(@"</td><td></td><td>") + 18;
                txbCountry.Text = str_country.Substring(app, l - app - 10);
                str_country = "";
            }
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
