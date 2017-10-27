using System;
using System.Collections.Generic;
using FirstFloor.ModernUI.Windows.Controls;
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
using System.Threading;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private bool first_time_closed = false;
        //using for category
        private BUS_tb_Category bus_tb_category = new BUS_tb_Category();
        private List<EC_tb_Category> list_ec_tb_category = new List<EC_tb_Category>();
        private int selected_categoryid = -1;
        private bool flag_check_loaded = false;

        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();
        private List<EC_tb_Product> list_ec_tb_product = new List<EC_tb_Product>();

        //using for salesperson
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();

        //using for customer
        private BUS_tb_Customer bus_tb_customer = new BUS_tb_Customer();

        //using for order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //using for orderdetail
        private List<EC_tb_OrderDetail> list_ec_tb_orderdetail_temp = new List<EC_tb_OrderDetail>();

        //using for current directory
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        //using for setting
        private BUS_tb_Setting bus_tb_setting = new BUS_tb_Setting();

        //using for barcode
        private System.Windows.Threading.DispatcherTimer dispatcher_timer = new System.Windows.Threading.DispatcherTimer();
        private System.Windows.Forms.Timer timer_delay;

        //using for order detail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //thread
        private Thread thread_getproduct = null;
        private Thread thread_get_category = null;
        private Thread thread_update_setting = null;
        private Thread thread_scan_barcode = null;
        public List<ProductSort> ProdSort { get; set; }
        public List<DirectSort> ProdDirect { get; set; }
        private prodSort _homeProdSort = prodSort.Name;
        public prodSort HomeProdSort
        {
            get { return _homeProdSort; }
            set { _homeProdSort = value; }
        }
        private directSort _homeDirectSort = directSort.ASC;
        public directSort HomeDirectSort
        {
            get { return _homeDirectSort; }
            set { _homeDirectSort = value; }
        }
        //Home
        public Home()
        {
            InitializeComponent();
            
            //set start control
            SetStartControl();

            LoadCategory();
            TotalStatistic();

            //setting for timer
            timer_delay = new System.Windows.Forms.Timer();
            timer_delay.Interval = 1000;
            timer_delay.Enabled = false;
            ProdSort = new List<ProductSort>()
            {
                new ProductSort() { SortBy = prodSort.DateInserted, DisplayName = "Date inserted" },
                new ProductSort() { SortBy = prodSort.Name, DisplayName = "Name" },
                new ProductSort() { SortBy = prodSort.Seller, DisplayName = "No. items sold" },
                new ProductSort() { SortBy = prodSort.Price, DisplayName = "Price" },
            };
            ProdDirect = new List<DirectSort>()
            {
                new DirectSort() { DirectValue = directSort.ASC, DirectString = "Ascending" },
                new DirectSort() { DirectValue = directSort.DESC, DirectString = "Descending" }
            };
            this.stp_home_sort.Dispatcher.Invoke((Action)(() =>
            {
                cb_SortBy.ItemsSource = ProdSort;
                cb_SortBy.SelectedValue = prodSort.Name;
                cb_SortBy.DisplayMemberPath = "DisplayName";
                cb_SortBy.SelectedValuePath = "SortBy";
                /*cb_SortDirect.ItemsSource = ProdDirect;
                cb_SortDirect.DisplayMemberPath = "DirectString";
                cb_SortDirect.SelectedValuePath = "DirectValue";
                cb_SortDirect.SelectedValue = directSort.ASC;*/
                Border bd_off = (Border)UCSortDirect.FindName("bdAscending");
                bd_off.MouseDown += new MouseButtonEventHandler(bdAscending_MouseDown);
                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblAscending");
                tbl_off.Foreground = System.Windows.Media.Brushes.White;

                Border bd_on = (Border)UCSortDirect.FindName("bdDescending");
                bd_on.MouseDown += new MouseButtonEventHandler(bdDescending_MouseDown);
                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblDescending");
                tbl_on.Foreground = System.Windows.Media.Brushes.Silver;
            }));
        }
        private void bdAscending_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border bd_off = (Border)UCSortDirect.FindName("bdAscending");
            bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
            TextBlock tbl_off = (TextBlock)bd_off.FindName("tblAscending");
            tbl_off.Foreground = System.Windows.Media.Brushes.White;

            Border bd_on = (Border)UCSortDirect.FindName("bdDescending");
            bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
            TextBlock tbl_on = (TextBlock)bd_on.FindName("tblDescending");
            tbl_on.Foreground = System.Windows.Media.Brushes.Silver;
            try
            {
                HomeDirectSort = (directSort)Enum.Parse(typeof(directSort), "ASC");
                LoadDataProduct(condition_product1);
            }
            catch { }
        }
        private void bdDescending_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border bd_off = (Border)UCSortDirect.FindName("bdAscending");
            bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
            TextBlock tbl_off = (TextBlock)bd_off.FindName("tblAscending");
            tbl_off.Foreground = System.Windows.Media.Brushes.Silver;

            Border bd_on = (Border)UCSortDirect.FindName("bdDescending");
            bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
            TextBlock tbl_on = (TextBlock)bd_on.FindName("tblDescending");
            tbl_on.Foreground = System.Windows.Media.Brushes.White;
            try
            {
                HomeDirectSort = (directSort)Enum.Parse(typeof(directSort), "DESC");
                LoadDataProduct(condition_product1);
            }
            catch { }
        }
        //change background color for button
        private void SetStartControl()
        {
            System.Windows.Media.Color accent_color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(StaticClass.GeneralClass.app_settings["accentColor"].ToString());
            SolidColorBrush brush = new SolidColorBrush(accent_color);
            ButtonColor btcolor = new ButtonColor { Solid_Color_Brush = brush };

            new Thread(() =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    bdUndo.DataContext = btcolor;
                    bdSendEmail.DataContext = btcolor;
                    bdClearAll.DataContext = btcolor;
                    bdPayCash.DataContext = btcolor;
                    bdDiscount.DataContext = btcolor;
                    //bdPayOther.DataContext = btcolor;
                    bdSalesperson.DataContext = btcolor;
                    //gs.Background = brush;

                    this.cd1.Width = new GridLength(Convert.ToDouble(StaticClass.GeneralClass.app_settings["orderDetailWidth"].ToString()));
                    this.cd1.MaxWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width * 75 / 100;

                    this.tblTotalProduct.Margin = new Thickness((Convert.ToDouble(StaticClass.GeneralClass.app_settings["orderDetailWidth"].ToString()) - 17.5) / 2, 0, 5, 0);
                    this.muiBtnCustomer.Margin = new Thickness(Convert.ToDouble(StaticClass.GeneralClass.app_settings["orderDetailWidth"].ToString()) - 17.5, 0, 5, 0);
                    this.tblSumnProduct_Category.Margin = new Thickness(Convert.ToDouble(StaticClass.GeneralClass.app_settings["orderDetailWidth"].ToString()) + (Application.Current.MainWindow.RenderSize.Width - Convert.ToDouble(StaticClass.GeneralClass.app_settings["orderDetailWidth"].ToString())) / 2 - 17.5, 0, 5, 0);

                    this.dtgOrderDetail.ItemsSource = StaticClass.GeneralClass.list_ec_tb_orderdetail_general;
                }));

                //disable control
                DisableContol();
            }).Start();
        }

        //load category, inventory and product barcode
        private void LoadCategory()
        {
            //get data for inventory status and barcode
            if (thread_get_category != null && thread_get_category.ThreadState == ThreadState.Running) { }
            else
            {
                thread_get_category = new Thread(() =>
                {
                    this.scvCategory.Dispatcher.Invoke((Action)(() => { this.scvCategory.Visibility = System.Windows.Visibility.Collapsed; }));
                    this.mprcat.Dispatcher.Invoke((Action)(() =>
                    {
                        this.mprcat.IsActive = true;
                        this.mprcat.Visibility = System.Windows.Visibility.Visible;
                    }));

                    GetCategory();

                    this.mprcat.Dispatcher.Invoke((Action)(() =>
                    {
                        this.mprcat.IsActive = false;
                        this.mprcat.Visibility = System.Windows.Visibility.Collapsed;
                    }));

                    this.scvCategory.Dispatcher.Invoke((Action)(() => { this.scvCategory.Visibility = System.Windows.Visibility.Visible; }));

                    if (list_ec_tb_category.Count > 0)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Button bt = (Button)wpCategory.Children[0];
                            bt.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2B6017");
                            bt_current_selected = Convert.ToInt32(bt.Name.Substring(3));
                            bt_previous_selected = Convert.ToInt32(bt.Name.Substring(3));
                        }));

                        //for paging
                        categoryname_selected = FindResource("category").ToString();
                        paging_number_previous = 1;

                        selected_categoryid = bt_current_selected;

                        if (selected_categoryid > -1)
                        {
                            categoryname_selected = list_ec_tb_category[selected_categoryid].CategoryName;
                            condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                            condition_product2 = " And [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                            LoadDataProduct(condition_product1);
                        }
                    }
                    else
                    {
                        this.wpProduct.Dispatcher.Invoke((Action)(() => { wpProduct.Children.Clear(); }));
                        list_ec_tb_product.Clear();
                        this.tblSumnProduct_Category.Dispatcher.Invoke((Action)(() => {
                            this.tblSumnProduct_Category.Text = FindResource("category").ToString() + "(0)";
                            stp_home_sort.Visibility = Visibility.Collapsed;
                        }));
                    }
                });
                thread_get_category.Start();
            }
        }

        //DisableContol
        private void DisableContol()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.btnClearAll.Foreground = System.Windows.Media.Brushes.Gainsboro;
                this.bdClearAll.IsEnabled = false;

                this.btnDiscount.Foreground = System.Windows.Media.Brushes.Gainsboro;
                this.bdDiscount.IsEnabled = false;

                this.btnPayCash.Foreground = System.Windows.Media.Brushes.Gainsboro;
                this.bdPayCash.IsEnabled = false;

                //this.btnPayOther.Foreground = System.Windows.Media.Brushes.Gainsboro;
                //this.bdPayOther.IsEnabled = false; 

                /*select od.ProductID, p.ShortName, p.Price, InventoryCount, count(od.ProductID) as Total from tb_OrderDetail as od inner join tb_Product as p on p.ProductID = od.ProductID 
                group by od.ProductID, p.ShortName, p.Price, InventoryCount
                order by Total desc*/
            }));
        }

        //EnableContol
        private void EnableContol()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.btnClearAll.Foreground = System.Windows.Media.Brushes.White;
                this.bdClearAll.IsEnabled = true;

                this.btnDiscount.Foreground = System.Windows.Media.Brushes.White;
                this.bdDiscount.IsEnabled = true;

                this.btnPayCash.Foreground = System.Windows.Media.Brushes.White;
                this.bdPayCash.IsEnabled = true;

                //this.btnPayOther.Foreground = System.Windows.Media.Brushes.White;
                //this.bdPayOther.IsEnabled = true;
            }));
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                //show first-time login guide
                if (!first_time_closed)
                {
                    first_time_closed = true;
                    if (StaticClass.GeneralClass.app_settings["showGuide"].ToString()=="True")
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Pages.LoginGuide.FirstTimeLoginGuide page = new Pages.LoginGuide.FirstTimeLoginGuide();
                            page.ShowInTaskbar = false;
                            page.Owner = Application.Current.MainWindow;
                            page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            page.ShowDialog();
                        }));
                    }
                }

                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (tblSumnProduct_Category.Text.Contains("(0)"))
                    {
                        this.tblSumnProduct_Category.Text = FindResource("category").ToString() + "(0)";
                        stp_home_sort.Visibility = Visibility.Collapsed;
                    }

                    this.tblTotalProduct.Text = FindResource("product").ToString() + "(" + StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count.ToString() + ")";
                    this.tblCustomer.Text = StaticClass.GeneralClass.customername_general;
                    this.btnSalesperson.Content = FindResource("salesperson").ToString() + ": " + StaticClass.GeneralClass.salespersonname_login_general;
                }));
            }).Start();
            if (flag_check_loaded == true)
            {
                flag_check_loaded = false;
            }
            else
            {
                flag_check_loaded = true;
                LoadData();
                if(StaticClass.GeneralClass.app_settings["isLoginToSale"].ToString() == "1" && (StaticClass.GeneralClass.user_permission == 4 || StaticClass.GeneralClass.user_permission == 3 || StaticClass.GeneralClass.user_permission == 2))
                {
                    System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                    t.Interval = 100;
                    t.Tick += new EventHandler(OnTimedEvent);
                    t.Start();
                }
                if (StaticClass.GeneralClass.app_settings["appIsRestart"].ToString() == "True")
                {
                    StaticClass.GeneralClass.app_settings["appIsRestart"] = "False";
                    Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                }
            }
        }
        private void OnTimedEvent(object source, EventArgs e)
        {
            System.Windows.Forms.Timer _source = (System.Windows.Forms.Timer)source;
            _source.Stop();
            //Views.Home.Login2Sell _page = new Views.Home.Login2Sell();
            Views.Home.LoginSystem _page = new Views.Home.LoginSystem();
            _page.btn_salelogin_delegate += newSalesperson_Click_Delegate;
            _page.ShowDialog();
        }
        private void newSalesperson_Click_Delegate()
        {
            btnSalesperson.Content = FindResource("salesperson").ToString() + ": " + StaticClass.GeneralClass.salespersonname_login_general.ToString();
        }
        //LoadData
        private void LoadData()
        {
            //set focus for txbBarcode
            if (muiBtnScanBarcode.Visibility == System.Windows.Visibility.Collapsed || flag_check_barcode == true)
            {
                this.KeyDown -= new KeyEventHandler(UserControl_KeyDown);
                this.KeyDown += new KeyEventHandler(UserControl_KeyDown);
                txbBarcode.Focus();
            }

            //update when salesperson change
            if (StaticClass.GeneralClass.flag_add_edit_delete_salesperson_general == true)
            {
                new Thread(() =>
                {
                    StaticClass.GeneralClass.flag_add_edit_delete_salesperson_general = false;
                    DataTable tb_salesperson_login = bus_tb_salesperson.GetSalesPerson("WHERE [SalespersonID]=" + StaticClass.GeneralClass.salespersonid_login_general.ToString() + " AND [Active] = 1");
                    if (tb_salesperson_login.Rows.Count == 1)
                    {
                        StaticClass.GeneralClass.salespersonname_login_general = tb_salesperson_login.Rows[0]["Name"].ToString();
                        this.btnSalesperson.Dispatcher.Invoke((Action)(() => { this.btnSalesperson.Content = FindResource("salesperson").ToString() + ": " + StaticClass.GeneralClass.salespersonname_login_general; }));
                    }
                    else
                    {
                        StaticClass.GeneralClass.salespersonid_login_general = 0;
                        StaticClass.GeneralClass.salespersonname_login_general = "None";
                        StaticClass.GeneralClass.flag_salespersonlogin_general = false;
                        this.Dispatcher.Invoke((Action)(() => { this.btnSalesperson.Content = FindResource("salesperson").ToString() + ": " + StaticClass.GeneralClass.salespersonname_login_general; }));
                    }
                }).Start();
            }

            //update when customer change
            if (StaticClass.GeneralClass.flag_add_edit_delete_customer_general == true)
            {
                new Thread(() =>
                {
                    StaticClass.GeneralClass.flag_add_edit_delete_customer_general = false;
                    DataTable tb_customer_select = bus_tb_customer.GetCustomer("WHERE [CustomerID]=" + StaticClass.GeneralClass.customerid_general.ToString());
                    if (tb_customer_select.Rows.Count == 1)
                    {
                        StaticClass.GeneralClass.customername_general = tb_customer_select.Rows[0]["FirstName"].ToString() + " " + tb_customer_select.Rows[0]["LastName"].ToString();
                        this.Dispatcher.Invoke((Action)(() => { this.tblCustomer.Text = StaticClass.GeneralClass.customername_general; }));
                    }
                    else
                    {
                        StaticClass.GeneralClass.customerid_general = 0;
                        StaticClass.GeneralClass.customername_general = "";
                        this.Dispatcher.Invoke((Action)(() => { this.tblCustomer.Text = "None"; }));
                    }
                }).Start();
            }

            //update when setting change
            if (StaticClass.GeneralClass.flag_add_edit_setting_general || StaticClass.GeneralClass.flag_add_edit_delete_product_general)
            {
                if (thread_update_setting != null && thread_update_setting.ThreadState == ThreadState.Running) { }
                else
                {
                    StaticClass.GeneralClass.flag_add_edit_setting_general = false;
                    StaticClass.GeneralClass.flag_add_edit_delete_category_general = false;
                    StaticClass.GeneralClass.flag_add_edit_delete_product_general = true;

                    thread_update_setting = new Thread(() =>
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.wpCategory.Visibility = System.Windows.Visibility.Collapsed;
                            this.mprcat.IsActive = true;
                            this.mprcat.Visibility = System.Windows.Visibility.Visible;
                        }));

                        //update list_order detail general
                        UpdateListOrderDetailGeneral();

                        //update list order detail temp general
                        UpdateListOrderDetailTempGeneral();

                        //update category
                        GetCategory();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mprcat.IsActive = false;
                            this.mprcat.Visibility = System.Windows.Visibility.Collapsed;
                            this.wpCategory.Visibility = System.Windows.Visibility.Visible;
                        }));

                        if (list_ec_tb_category.Count > 0)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                Button bt = (Button)wpCategory.Children[0];
                                bt.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2B6017");
                                bt_current_selected = Convert.ToInt32(bt.Name.Substring(3));
                                bt_previous_selected = Convert.ToInt32(bt.Name.Substring(3));
                            }));

                            //for paging
                            categoryname_selected = "Products";
                            paging_number_previous = 1;

                            selected_categoryid = bt_current_selected;

                            if (selected_categoryid > -1)
                            {
                                categoryname_selected = list_ec_tb_category[selected_categoryid].CategoryName;
                                condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                                condition_product2 = " And [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                                LoadDataProduct(condition_product1);
                            }
                        }
                        else
                        {
                            list_ec_tb_product.Clear();
                            this.Dispatcher.Invoke((Action)(() => 
                            {
                                wpProduct.Children.Clear();
                                this.tblSumnProduct_Category.Text = FindResource("category").ToString() + "(0)";
                                stp_home_sort.Visibility = Visibility.Collapsed;
                            }));
                        }

                    });
                    thread_update_setting.Start();
                }

                return;
            }

            //update when change out-of-stock
            if((StaticClass.GeneralClass.flag_change_out_of_stock == true) || (StaticClass.GeneralClass.flag_add_edit_delete_category_general == true))
            {
                StaticClass.GeneralClass.flag_add_edit_delete_category_general = false;
                StaticClass.GeneralClass.flag_change_out_of_stock = false;

                new Thread(() =>
                {
                    //update category
                    GetCategory();
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.wpCategory.Visibility = System.Windows.Visibility.Visible;
                        this.mprcat.IsActive = false;
                        this.mprcat.Visibility = System.Windows.Visibility.Collapsed;
                    }));

                    if (list_ec_tb_category.Count > 0)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Button bt = (Button)wpCategory.Children[0];
                            bt.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2B6017");
                            bt_current_selected = Convert.ToInt32(bt.Name.Substring(3));
                            bt_previous_selected = Convert.ToInt32(bt.Name.Substring(3));
                        }));

                        //for paging
                        categoryname_selected = FindResource("category").ToString();
                        paging_number_previous = 1;

                        selected_categoryid = bt_current_selected;

                        if (selected_categoryid > -1)
                        {
                            categoryname_selected = list_ec_tb_category[selected_categoryid].CategoryName;
                            condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                            condition_product2 = " And [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                            LoadDataProduct(condition_product1);
                        }
                    }
                    else
                    {
                        list_ec_tb_product.Clear();
                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            wpProduct.Children.Clear();
                            this.tblSumnProduct_Category.Text = FindResource("category").ToString() + "(0)";
                            stp_home_sort.Visibility = Visibility.Collapsed;
                        }));
                    }
                }).Start();
            }
        }

        //UpdateListOrderDetailGeneral
        private void UpdateListOrderDetailGeneral()
        {
            int i_temp = 0;
            bool flag_mark = false;
            do
            {
                flag_mark = false;
                if (i_temp < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count)
                {
                    for (int i = i_temp; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                    {
                        DataTable datatable = new DataTable();
                        datatable = bus_tb_product.GetProduct("where ProductID = " + StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID + " and Active = 1 and InventoryCount >= " + StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty);
                        if (datatable.Rows.Count == 0)
                        {
                            StaticClass.GeneralClass.list_ec_tb_orderdetail_general.RemoveAt(i);
                            i_temp = i;
                            flag_mark = true;
                            break;
                        }
                        else
                        {
                            StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Currency = StaticClass.GeneralClass.currency_setting_general;
                            StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].StrDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Discount);

                            if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].DiscountTypeUnit0 != "")
                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].DiscountTypeUnit0 = StaticClass.GeneralClass.currency_setting_general;

                            StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Cost = Convert.ToDecimal(datatable.Rows[0]["Cost"].ToString());

                            StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Price = Convert.ToDecimal(datatable.Rows[0]["Price"].ToString());
                            StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].StrPrice = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Price);

                            //update discount total and tax for order detail item
                            UpdateDiscountTotalTax(i);
                        }
                    }
                }
            } while (flag_mark == true);

            //update total statistic
            TotalStatistic();
        }

        //UpdateListOrderDetailTempGeneral
        private void UpdateListOrderDetailTempGeneral()
        {
            int i_temp = 0;
            bool flag_mark = false;
            bool flag_check = false;

            do
            {
                flag_mark = false;
                for (int i = i_temp; i < list_ec_tb_orderdetail_temp.Count; i++)
                {
                    flag_check = false;
                    for (int j = 0; j < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; j++)
                    {
                        if (list_ec_tb_orderdetail_temp[i].ProductID == StaticClass.GeneralClass.list_ec_tb_orderdetail_general[j].ProductID)
                        {
                            list_ec_tb_orderdetail_temp[i].Currency = StaticClass.GeneralClass.currency_setting_general;

                            if (list_ec_tb_orderdetail_temp[i].DiscountTypeUnit0 != "")
                                list_ec_tb_orderdetail_temp[i].DiscountTypeUnit0 = StaticClass.GeneralClass.currency_setting_general;

                            list_ec_tb_orderdetail_temp[i].Cost = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[j].Cost;
                            list_ec_tb_orderdetail_temp[i].Price = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[j].Price;

                            //update discount total and tax for order detail temp item
                            UpdateDiscountTotalTaxForListOrderDetailTemp(i);

                            flag_check = true;
                            i_temp = i;
                            break;
                        }
                    }

                    if (flag_check == false)
                    {
                        list_ec_tb_orderdetail_temp.RemoveAt(i);
                        break;
                    }
                }
            }
            while (flag_mark == true);
        }

        //GetCategory
        private DataTable dt_category = new DataTable();
        private int count_wpcategory_chil = 0;
        private System.Windows.Style style_bt_category = new System.Windows.Style();

        private void GetCategory()
        {
            try
            {
                style_bt_category = this.FindResource("ButtonTempleteCategory") as System.Windows.Style;
                this.wpCategory.Dispatcher.Invoke((Action)(() => { count_wpcategory_chil = wpCategory.Children.Count; }));

                if (count_rows > 0)
                    this.wpCategory.Dispatcher.Invoke((Action)(() => { wpCategory.Children.RemoveRange(0, wpCategory.Children.Count); }));

                dt_category.Clear();
                list_ec_tb_category.Clear();

                dt_category = bus_tb_category.GetCategoryMain(" where tb_Product.Active = 1");

                if (StaticClass.GeneralClass.app_settings["outOfStock"].ToString()=="True")
                {
                    foreach (DataRow dr in dt_category.Rows)
                    {
                        list_ec_tb_category.Add(new EC_tb_Category
                        {
                            CategoryID = Convert.ToInt32(dr[0].ToString()),
                            CategoryName = dr[1].ToString(),
                        });
                    }
                }
                else
                {
                    foreach (DataRow dr in dt_category.Rows)
                    {
                        DataTable datatable_row = new DataTable();
                        datatable_row = bus_tb_product.GetProduct("where CategoryID = " + dr["CategoryID"].ToString() + " and InventoryCount > 0 and Active = 1");

                        if (datatable_row.Rows.Count > 0)
                        {
                            list_ec_tb_category.Add(new EC_tb_Category
                            {
                                CategoryID = Convert.ToInt32(dr[0].ToString()),
                                CategoryName = dr[1].ToString(),
                            });
                        }
                    }
                }

                if (list_ec_tb_category.Count > 0)
                {
                    for (int i = 0; i < list_ec_tb_category.Count; i++)
                    {
                        this.Dispatcher.Invoke((Action)(()=>
                        {
                            Button bt = new Button();
                            bt.Name = "btn" + i.ToString();
                            bt.Style = style_bt_category;
                            bt.Content = list_ec_tb_category[i].CategoryName;
                            bt.Click += new RoutedEventHandler(btCategory_Click);
                            bt.MouseEnter += new MouseEventHandler(btCategory_MouseEnter);
                            bt.MouseLeave += new MouseEventHandler(btCategory_MouseLeave);
                            wpCategory.Children.Add(bt);
                        }));
                    }
                }

                this.tblSumnProduct_Category.Dispatcher.Invoke((Action)(() => {
                    this.tblSumnProduct_Category.Text = FindResource("category").ToString() + "(0)";
                    stp_home_sort.Visibility = Visibility.Collapsed;
                }));
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.Content = FindResource("close").ToString();
                    md.Content = "Error 1: " + ex.Message;
                    md.Title = FindResource("notification").ToString();
                    md.ShowDialog();
                }));
            }
        }

        //btCategory_MouseEnter
        private void btCategory_MouseEnter(object sender, MouseEventArgs e)
        {
            Button bt = (Button)sender;
            if (bt.Background.ToString() != "#FF2B6017")
                bt.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2B6017");
        }

        //btCategory_MouseLeave
        private void btCategory_MouseLeave(object sender, MouseEventArgs e)
        {
            Button bt = (Button)sender;
            int cur_chil = Convert.ToInt32(bt.Name.Substring(3));

            if ((bt.Background.ToString() != "#FF2B6017") || (bt_current_selected != cur_chil))
                bt.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3F7224");
        }

        //btCategory_Click
        private int bt_previous_selected = -1;
        private int bt_current_selected = -1;
        private void btCategory_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;

            if (bt_current_selected != Convert.ToInt32(bt.Name.Substring(3)))
            {
                bt.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2B6017");
                bt_current_selected = Convert.ToInt32(bt.Name.Substring(3));

                if (bt_previous_selected > -1)
                {
                    Button bt_pre = (Button)wpCategory.Children[bt_previous_selected];
                    bt_pre.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3F7224");
                }
                bt_previous_selected = Convert.ToInt32(bt.Name.Substring(3));

                //for paging
                categoryname_selected = "Products";
                paging_number_previous = 1;

                selected_categoryid = bt_current_selected;

                if (selected_categoryid > -1)
                {
                    categoryname_selected = list_ec_tb_category[selected_categoryid].CategoryName;
                    condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                    condition_product2 = " And [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                    LoadDataProduct(condition_product1);
                }
                else
                {
                    list_ec_tb_product.Clear();
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        wpProduct.Children.Clear();
                        this.tblSumnProduct_Category.Text = FindResource("category").ToString() + "(0)";
                        stp_home_sort.Visibility = Visibility.Collapsed;
                    }));
                }
            }
        }

        //UpdateDiscountTotalTax for list_ec_tb_orderdetail_general
        private void UpdateDiscountTotalTax(int i)
        {
            try
            {
                //update discount is percent
                if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].DiscountType == 1)
                {
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].TotalDiscount = (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Price * StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Discount / 100) * StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty;
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].StrTotalDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].TotalDiscount);

                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Total = (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Price * StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty) - StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].TotalDiscount;
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].StrTotal = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Total);
                }
                //update discount is amount
                else
                {
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].TotalDiscount = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Discount * StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty;
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].StrTotalDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].TotalDiscount);

                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Total = (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Price * StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty) - StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].TotalDiscount;
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].StrTotal = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Total);
                }

                //update tax
                int tax = Convert.ToInt32(bus_tb_product.GetProduct("WHERE [ProductID]=" + StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID).Rows[0]["Tax"].ToString());

                if (tax == 1)
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Tax = (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Total * StaticClass.GeneralClass.taxrate_setting_general / 100);
                else
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Tax = 0;
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.Content = FindResource("close").ToString();
                    md.Content = "Error 2: " + ex.Message;
                    md.Title = FindResource("notification").ToString();
                    md.ShowDialog();
                }));
            }
        }

        //UpdateDiscountTotalTax for list_ec_tb_orderdetail_temp
        private void UpdateDiscountTotalTaxForListOrderDetailTemp(int i)
        {
            try
            {
                //update discount is percent
                if (list_ec_tb_orderdetail_temp[i].DiscountType == 1)
                {
                    list_ec_tb_orderdetail_temp[i].TotalDiscount = (list_ec_tb_orderdetail_temp[i].Price * list_ec_tb_orderdetail_temp[i].Discount / 100) * list_ec_tb_orderdetail_temp[i].Qty;
                    list_ec_tb_orderdetail_temp[i].Total = (list_ec_tb_orderdetail_temp[i].Price * list_ec_tb_orderdetail_temp[i].Qty) - list_ec_tb_orderdetail_temp[i].TotalDiscount;
                }
                //update discount is amount
                else
                {
                    list_ec_tb_orderdetail_temp[i].TotalDiscount = list_ec_tb_orderdetail_temp[i].Discount * list_ec_tb_orderdetail_temp[i].Qty;
                    list_ec_tb_orderdetail_temp[i].Total = (list_ec_tb_orderdetail_temp[i].Price * list_ec_tb_orderdetail_temp[i].Qty) - list_ec_tb_orderdetail_temp[i].TotalDiscount;
                }

                //update tax
                int tax = Convert.ToInt32(bus_tb_product.GetProduct("WHERE [ProductID]=" + list_ec_tb_orderdetail_temp[i].ProductID).Rows[0]["Tax"].ToString());

                if (tax == 1)
                    list_ec_tb_orderdetail_temp[i].Tax = (list_ec_tb_orderdetail_temp[i].Total * StaticClass.GeneralClass.taxrate_setting_general / 100);
                else
                    list_ec_tb_orderdetail_temp[i].Tax = 0;
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.Content = FindResource("close").ToString();
                    md.Content = "Error 3: " + ex.Message;
                    md.Title = FindResource("notification").ToString();
                    md.ShowDialog();
                }));
            }
        }

        //SaveListOrderDetailTemp
        private void SaveListOrderDetailTemp(int index_orderdetail)
        {
            try
            {
                EC_tb_OrderDetail ec_tb_orderdetail = new EC_tb_OrderDetail();
                ec_tb_orderdetail.CategoryID = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].CategoryID;
                ec_tb_orderdetail.CategoryName = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].CategoryName;
                ec_tb_orderdetail.ProductID = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].ProductID;
                ec_tb_orderdetail.ProductName = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].ProductName;
                ec_tb_orderdetail.Price = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].Price;
                ec_tb_orderdetail.Qty = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].Qty;

                ec_tb_orderdetail.Tax = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].Tax;
                ec_tb_orderdetail.DiscountType = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].DiscountType;
                ec_tb_orderdetail.Discount = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].Discount;
                ec_tb_orderdetail.TotalDiscount = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].TotalDiscount;
                ec_tb_orderdetail.Total = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].Total;

                //add info
                ec_tb_orderdetail.Currency = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].Currency;
                ec_tb_orderdetail.DiscountTypeUnit0 = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].DiscountTypeUnit0;
                ec_tb_orderdetail.DiscountTypeUnit1 = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[index_orderdetail].DiscountTypeUnit1;

                list_ec_tb_orderdetail_temp.Add(ec_tb_orderdetail);
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //total calculation

        private void TotalStatistic()
        {
            try
            {
                //update data for lvListProductsOrder
                this.Dispatcher.Invoke((Action)(() => { this.dtgOrderDetail.Items.Refresh(); }));

                //subtotal
                decimal subtotal = 0;

                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count(); i++)
                {
                    subtotal += StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Total;
                }

                StaticClass.GeneralClass.subtotal_general = subtotal;

                this.Dispatcher.Invoke((Action)(() => 
                {
                    this.tblSubtotal.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(subtotal);
                }));

                //discount
                if (StaticClass.GeneralClass.discounttype_general == 1)
                {
                    StaticClass.GeneralClass.totaldiscount_general = StaticClass.GeneralClass.subtotal_general * StaticClass.GeneralClass.discount_general / 100;
                    this.Dispatcher.Invoke((Action)(() => 
                    {
                        this.tblDiscount.Text = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.discount_general) + "%";
                        this.tblDiscountMoney.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.totaldiscount_general);
                    }));
                }
                else
                {
                    StaticClass.GeneralClass.totaldiscount_general = StaticClass.GeneralClass.discount_general;
                    this.Dispatcher.Invoke((Action)(() => 
                    {
                        this.tblDiscount.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.discount_general);
                        this.tblDiscountMoney.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.totaldiscount_general);
                    }));
                }

                //tax rate
                decimal total_taxrate = 0;
                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                {
                    total_taxrate += StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Tax;
                }

                StaticClass.GeneralClass.totaltaxrate_general = total_taxrate;

                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.tblTax.Text = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.taxrate_setting_general) + "%";
                    this.tblTaxMoney.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.totaltaxrate_general);

                    //total
                    decimal _total_ = StaticClass.GeneralClass.ConverStringToDecimal(StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.subtotal_general)) - StaticClass.GeneralClass.ConverStringToDecimal(StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.totaldiscount_general)) + StaticClass.GeneralClass.ConverStringToDecimal(StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.totaltaxrate_general));
                    //this.tblTotal.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay((StaticClass.GeneralClass.subtotal_general - StaticClass.GeneralClass.totaldiscount_general) + StaticClass.GeneralClass.totaltaxrate_general);
                    this.tblTotal.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(_total_);
                    this.tblTotalProduct.Text = FindResource("product").ToString() + "(" + StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count.ToString() + ")";
                }));
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.Content = FindResource("close").ToString();
                    md.Content = "Error 4: " + ex.Message;
                    md.Title = FindResource("notification").ToString();
                    md.ShowDialog();
                }));
            }
        }

        //get data for product data
        private int total_page = 0;
        private int current_page = 1;
        private int page_size = 100;
        private int paging_size = 10;
        private int count_rows = 0;
        private bool flag_stp_loaded = false;
        private int paging_number_previous = 0;
        private string categoryname_selected = "";
        private string condition_product1 = "where [Active] = 1";
        private string condition_product2 = "where [Active] = 1";
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle.png", UriKind.Absolute));
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging_focus = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_focus.png", UriKind.Absolute));

        //thread
        private Thread thread_paging = null;
        private Thread thread_paging_next = null;
        private Thread thread_paging_previous = null;
        private Thread thread_calculatetotalpages_inventorystatus = null;

        //LoadDataForOrder
        private void LoadDataProduct(string condition_calculate)
        {
            CalculateTotalPagesForCategory(condition_calculate);
            GetProduct(categoryname_selected, 0 * page_size, page_size, condition_product1, condition_product2);

            if (thread_paging != null && thread_paging.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging = new Thread(() =>
                {
                    try
                    {
                        if (current_page * paging_size * page_size > count_rows)
                        {
                            int _paging_size = (count_rows - ((current_page - 1) * paging_size * page_size)) / page_size;

                            if ((count_rows - ((current_page - 1) * paging_size * page_size)) % page_size > 0)
                                _paging_size += 1;

                            PagingPageForProduct(current_page, _paging_size, total_page);
                        }
                        else
                            PagingPageForProduct(current_page, paging_size, total_page);
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Content = "Error 5: " + ex.Message;
                            md.Title = FindResource("notification").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_paging.Start();
            }
        }

        //CalculateTotalPages
        private void CalculateTotalPagesForCategory(string condition_calculate)
        {
            if (thread_calculatetotalpages_inventorystatus != null && thread_calculatetotalpages_inventorystatus.ThreadState == ThreadState.Running) { }
            else
            {
                thread_calculatetotalpages_inventorystatus = new Thread(() =>
                {
                    current_page = 1;
                    count_rows = bus_tb_product.GetSumProduct(condition_calculate);
                    this.total_page = count_rows / page_size;
                    if (count_rows % page_size > 0)
                        this.total_page += 1;

                    //this.total_page = (int)Math.Ceiling((Double)(count_rows/page_size));
                });
                thread_calculatetotalpages_inventorystatus.Start();
                thread_calculatetotalpages_inventorystatus.Join();
            }
        }

        //PagingPage
        private StackPanel stp_paging;
        private void PagingPageForProduct(int page, int _paging_size, int total_page)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (flag_stp_loaded == true)
                    this.spPaging.Dispatcher.Invoke((Action)(() => { spPaging.Children.Remove(stp_paging); }));

                if (total_page > 1)
                {
                    stp_paging = new StackPanel();
                    stp_paging.Orientation = Orientation.Horizontal;

                    //set stp is added
                    flag_stp_loaded = true;

                    if (page - 1 > 0)
                    {
                        Image img_previous = new Image();
                        img_previous.Height = 20;
                        img_previous.Width = 20;
                        img_previous.Margin = new Thickness(0, 0, 5, 0);
                        System.Windows.Media.Imaging.BitmapImage bitmap_previous = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_previous.png", UriKind.Absolute));
                        img_previous.Source = bitmap_previous;
                        img_previous.MouseDown += new MouseButtonEventHandler(Img_Previous_MouseDown);
                        stp_paging.Children.Add(img_previous);
                    }

                    for (int i = ((page - 1) * paging_size); i < (((page - 1) * paging_size) + _paging_size); i++)
                    {
                        TextBlock tbl = new TextBlock();
                        tbl.Margin = new Thickness(5, 0, 0, 0);
                        tbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        tbl.Text = (i + 1).ToString();
                        tbl.Foreground = System.Windows.Media.Brushes.White;
                        tbl.FontWeight = FontWeights.Medium;
                        //tbl.TextDecorations = TextDecorations.Underline;

                        Image img_paging = new Image();
                        img_paging.Name = "img" + (i + 1).ToString();
                        img_paging.Width = 20;
                        img_paging.Height = 20;
                        img_paging.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        img_paging.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        img_paging.Margin = new Thickness(5, 0, 0, 0);

                        if (paging_number_previous == (i + 1))
                            img_paging.Source = bitmap_image_paging_focus;
                        else
                            img_paging.Source = bitmap_image_paging;

                        Grid gr_paging = new Grid();
                        gr_paging.Name = "grd" + (i + 1).ToString();
                        gr_paging.Background = System.Windows.Media.Brushes.Transparent;
                        gr_paging.Children.Add(img_paging);
                        gr_paging.Children.Add(tbl);
                        gr_paging.MouseDown += new MouseButtonEventHandler(Grid_Paging_MouseDown);
                        stp_paging.Children.Add(gr_paging);
                    }
                    //GC.Collect();

                    spPaging.Children.Add(stp_paging);

                    if ((page < total_page) && (paging_size == _paging_size))
                    {
                        Image img_next = new Image();
                        img_next.Name = "img_next";
                        img_next.Height = 20;
                        img_next.Width = 20;
                        img_next.Margin = new Thickness(10, 0, 0, 0);
                        System.Windows.Media.Imaging.BitmapImage bitmap_next = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_next.png", UriKind.Absolute));
                        img_next.Source = bitmap_next;
                        img_next.MouseDown += new MouseButtonEventHandler(Img_Next_MouseDown);
                        stp_paging.Children.Add(img_next);
                    }
                }
            }));
        }

        //Grid_Paging_MouseDown
        private void Grid_Paging_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int int_current_page = 0;
            Grid grid_paging = (Grid)sender;
            string str_current_page = grid_paging.Name.Substring(3);
            int_current_page = Convert.ToInt32(str_current_page);

            if (paging_number_previous != int_current_page)
            {
                StackPanel _stp_paging = (StackPanel)spPaging.Children[0];
                for (int i = 0; i < _stp_paging.Children.Count; i++)
                {
                    if (_stp_paging.Children[i].GetType() != typeof(Image))
                    {
                        Grid _grid_paging = (Grid)_stp_paging.Children[i];
                        Image _img_paging = (Image)_grid_paging.Children[0];
                        if (_img_paging.Name == "img" +  paging_number_previous)
                            _img_paging.Source = bitmap_image_paging;
                    }
                }
            }
            paging_number_previous = int_current_page;
            Image img_focus = (Image)grid_paging.Children[0];
            img_focus.Source = bitmap_image_paging_focus;
            GetProduct(categoryname_selected, (int_current_page - 1) * page_size, page_size, condition_product1, condition_product2);
        }

        //Img_Next_MouseDown
        private void Img_Next_MouseDown(object sender, MouseButtonEventArgs e)
        {
            current_page++;

            if (thread_paging_next != null && thread_paging_next.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging_next = new Thread(() =>
                {
                    try
                    {
                        if (current_page * paging_size * page_size > count_rows)
                        {
                            int _paging_size = (count_rows - ((current_page - 1) * paging_size * page_size)) / page_size;

                            if ((count_rows - ((current_page - 1) * paging_size * page_size)) % page_size > 0)
                                _paging_size += 1;

                            PagingPageForProduct(current_page, _paging_size, total_page);
                        }
                        else
                            PagingPageForProduct(current_page, paging_size, total_page);
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Content = "Error 6: " + ex.Message;
                            md.Title = FindResource("notification").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_paging_next.Start();
            }
        }

        //Imge_Previous_MouseDown
        private void Img_Previous_MouseDown(object sender, MouseButtonEventArgs e)
        {
            current_page--;
            if (thread_paging_previous != null && thread_paging_previous.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging_previous = new Thread(() =>
                {
                    try
                    {
                        PagingPageForProduct(current_page, paging_size, total_page);
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Content = "Error 7: " + ex.Message;
                            md.Title = FindResource("notification").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_paging_previous.Start();
            }
        }

        //get list products
        DataTable dt_product = new DataTable();
        private void GetProduct(string category_name, int be_limit, int af_limit, string _condition_product1, string _condition_product2)
        {
            if (thread_getproduct != null && thread_getproduct.ThreadState == ThreadState.Running) { }
            else
            {
                thread_getproduct = new Thread(() =>
                {
                    try
                    {
                        int sum_product = 0;
                        int _index = -1;

                        //remove children for wpProduct
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (this.wpProduct.Children.Count > 0)
                            {
                                this.wpProduct.Children.Clear();
                                this.wpProduct.Visibility = System.Windows.Visibility.Hidden;
                            }

                            this.mprProduct.Visibility = System.Windows.Visibility.Visible;
                            this.mprProduct.IsActive = true;
                        }));

                        list_ec_tb_product.Clear();
                        dt_product.Clear();
                        dt_product = bus_tb_product.GetProductFollowPaging(be_limit, af_limit, condition_product1, condition_product2, StaticClass.GeneralClass.flag_database_type_general, _buildStringOderBy());

                        if (StaticClass.GeneralClass.app_settings["outOfStock"].ToString()=="False")
                        {
                            foreach (DataRow datarow in dt_product.Rows)
                            {
                                if (Convert.ToInt32(datarow["InventoryCount"].ToString()) > 0)
                                {
                                    sum_product++;
                                    EC_tb_Product ec_tb_product = new EC_tb_Product();

                                    ec_tb_product.Index = ++_index;
                                    ec_tb_product.ProductID = Convert.ToInt32(datarow["ProductID"].ToString());
                                    ec_tb_product.ShortName = datarow["ShortName"].ToString();
                                    ec_tb_product.LongName = datarow["LongName"].ToString();
                                    ec_tb_product.Cost = Convert.ToDecimal(datarow["Cost"].ToString());

                                    ec_tb_product.Price =  Convert.ToDecimal(datarow["Price"]);
                                    ec_tb_product.StrPrice = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_product.Price);

                                    if ((ec_tb_product.ShortName.Trim().Length + ec_tb_product.StrPrice.Length) > 25)
                                        ec_tb_product.StrShortName = ec_tb_product.ShortName.Substring(0, 25 - ec_tb_product.StrPrice.Length - 3) + "...";
                                    else
                                        ec_tb_product.StrShortName = ec_tb_product.ShortName;

                                    ec_tb_product.InventoryCount = Convert.ToInt32(datarow["InventoryCount"].ToString());
                                    ec_tb_product.CategoryID = Convert.ToInt32(datarow["CategoryID"].ToString());
                                    ec_tb_product.Tax = Convert.ToInt32(datarow["Tax"].ToString());

                                    //check exist image
                                    if (System.IO.File.Exists(current_directory + datarow["PathImage"].ToString()) == true)
                                    {
                                        ec_tb_product.PathImage = current_directory + datarow["PathImage"].ToString();

                                        this.Dispatcher.Invoke((Action)(() =>
                                        {
                                            using (System.IO.FileStream stream = System.IO.File.OpenRead(ec_tb_product.PathImage))
                                            {
                                                BitmapImage image = new BitmapImage();
                                                image.BeginInit();
                                                image.CacheOption = BitmapCacheOption.OnLoad;
                                                image.StreamSource = stream;
                                                image.EndInit();
                                                ec_tb_product.BitmapImage = image;
                                                image.Freeze();
                                                stream.Close();
                                            }
                                        }));
                                    }
                                    else
                                    {
                                        ec_tb_product.PathImage = @"pack://application:,,,/Resources/default_01_default_02_default_03_default.png";

                                        //product image
                                        Uri uri = new Uri(ec_tb_product.PathImage);
                                        BitmapImage bitmap_image = new BitmapImage();
                                        bitmap_image.BeginInit();
                                        bitmap_image.CacheOption = BitmapCacheOption.OnLoad;
                                        bitmap_image.UriSource = uri;
                                        bitmap_image.EndInit();
                                        ec_tb_product.BitmapImage = bitmap_image;
                                        bitmap_image.Freeze();
                                    }

                                    ec_tb_product.Capture = Convert.ToInt32(datarow["Capture"].ToString());
                                    ec_tb_product.Active = Convert.ToInt32(datarow["Active"].ToString());
                                    ec_tb_product.Currency = StaticClass.GeneralClass.currency_setting_general;

                                    //set amount inventory
                                    for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                                    {
                                        if (ec_tb_product.ProductID == StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID)
                                        {
                                            ec_tb_product.InventoryCount -= StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty;
                                            break;
                                        }
                                    }

                                    ec_tb_product.Image_Iventory_Status = @"pack://application:,,,/Resources/inventory_status.png";

                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        UserControls.UCProduct uc_product = new UserControls.UCProduct();
                                        uc_product.Name = "uc_product_name_" + _index.ToString();
                                        uc_product.Uid = "uc_product_uid_" + ec_tb_product.ProductID;
                                        uc_product.DataContext = ec_tb_product;
                                        uc_product.MouseDown += new MouseButtonEventHandler(UCProduct_MouseDown);
                                        wpProduct.Children.Add(uc_product);
                                    }));

                                    list_ec_tb_product.Add(ec_tb_product);
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow datarow in dt_product.Rows)
                            {
                                _index++;
                                sum_product++;
                                EC_tb_Product ec_tb_product = new EC_tb_Product();

                                ec_tb_product.Index = _index;
                                ec_tb_product.ProductID = Convert.ToInt32(datarow["ProductID"].ToString());
                                ec_tb_product.ShortName = datarow["ShortName"].ToString();
                                ec_tb_product.LongName = datarow["LongName"].ToString();
                                ec_tb_product.Cost = Convert.ToDecimal(datarow["Cost"].ToString());

                                ec_tb_product.Price = Convert.ToDecimal(datarow["Price"]);
                                ec_tb_product.StrPrice = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_product.Price);

                                if ((ec_tb_product.ShortName.Trim().Length + ec_tb_product.StrPrice.Length) > 25)
                                    ec_tb_product.StrShortName = ec_tb_product.ShortName.Substring(0, 25 - ec_tb_product.StrPrice.Length - 3) + "...";
                                else
                                    ec_tb_product.StrShortName = ec_tb_product.ShortName;

                                ec_tb_product.InventoryCount = Convert.ToInt32(datarow["InventoryCount"].ToString());
                                ec_tb_product.CategoryID = Convert.ToInt32(datarow["CategoryID"].ToString());
                                ec_tb_product.Tax = Convert.ToInt32(datarow["Tax"].ToString());

                                //check exist image
                                if (System.IO.File.Exists(current_directory + datarow["PathImage"].ToString()) == true)
                                    ec_tb_product.PathImage = current_directory + datarow["PathImage"].ToString();
                                else
                                    ec_tb_product.PathImage = @"pack://application:,,,/Resources/default_01_default_02_default_03_default.png";

                                ec_tb_product.Capture = Convert.ToInt32(datarow["Capture"].ToString());
                                ec_tb_product.Active = Convert.ToInt32(datarow["Active"].ToString());
                                ec_tb_product.Currency = StaticClass.GeneralClass.currency_setting_general;

                                //get amount inventory
                                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                                {
                                    if (ec_tb_product.ProductID == StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID)
                                    {
                                        ec_tb_product.InventoryCount -= StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty;

                                        break;
                                    }
                                }

                                //product image
                                Uri uri = new Uri(ec_tb_product.PathImage);
                                BitmapImage bitmap_image = new BitmapImage();
                                bitmap_image.BeginInit();
                                bitmap_image.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap_image.UriSource = uri;
                                bitmap_image.EndInit();
                                ec_tb_product.BitmapImage = bitmap_image;
                                bitmap_image.Freeze();

                                ec_tb_product.Image_Iventory_Status = @"pack://application:,,,/Resources/inventory_status.png";

                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    UserControls.UCProduct uc_product = new UserControls.UCProduct();
                                    uc_product.Name = "uc_product_name_" + _index.ToString();
                                    uc_product.Uid = "uc_product_uid_" + ec_tb_product.ProductID;
                                    uc_product.DataContext = ec_tb_product;
                                    uc_product.MouseDown += new MouseButtonEventHandler(UCProduct_MouseDown);
                                    wpProduct.Children.Add(uc_product);
                                }));

                                list_ec_tb_product.Add(ec_tb_product);
                            }
                        }

                        this.tblSumnProduct_Category.Dispatcher.Invoke((Action)(() => {
                            this.tblSumnProduct_Category.Text = category_name + "(" + sum_product + ")";
                            if (tblSumnProduct_Category.Text.Contains("(0)"))
                            {
                                stp_home_sort.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                stp_home_sort.Visibility = Visibility.Visible;
                            }
                        }));

                        //Thread.Sleep(500);
                        this.mprProduct.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mprProduct.Visibility = System.Windows.Visibility.Hidden;
                            this.mprProduct.IsActive = false;
                        }));
                        this.wpProduct.Dispatcher.Invoke((Action)(() => { this.wpProduct.Visibility = System.Windows.Visibility.Visible; }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Content = "Error 8: " + ex.Message;
                            md.Title = FindResource("notification").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_getproduct.Start();
            }
        }

        //muiBtnCustomer_Click
        private void muiBtnCustomer_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() => 
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Pages.Home.FindCustomer page = new Pages.Home.FindCustomer();
                    page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    page.btncustomer_delegate += muiBtnCustomer_Click_Delegate;
                    page.ShowDialog();
                }));
            }).Start();
        }

        //muiBtnCustomer_Click_Delegate
        private void muiBtnCustomer_Click_Delegate()
        {
            tblCustomer.Text = StaticClass.GeneralClass.customername_general.ToString();
        }

        //btnSalesperson_Click
        private void btnSalesperson_Click(object sender, RoutedEventArgs e)
        {
            if (StaticClass.GeneralClass.flag_salespersonlogin_general == false)
            {
                Pages.Home.SelectSalesperson page = new Pages.Home.SelectSalesperson();
                page.btnsalesperson_delegate += btnSalesperson_Click_Delegate;
                page.ShowDialog();
            }
            else
            {
                Pages.Home.ConfirmLogout page = new Pages.Home.ConfirmLogout();
                page.btnsalesperson_delegate += btnSalesperson_Click_Delegate;
                page.ShowDialog();
            }
        }

        //btnSalespersonShow_Delegate
        private void btnSalespersonShow_Delegate()
        {
            Pages.Home.SelectSalesperson page = new Pages.Home.SelectSalesperson();
            page.btnsalesperson_delegate += btnSalesperson_Click_Delegate;
            page.ShowDialog();
        }
        
        //btnSalesperson_Click_Delegate
        private void btnSalesperson_Click_Delegate()
        {
            btnSalesperson.Content = FindResource("salesperson").ToString() + ": " + StaticClass.GeneralClass.salespersonname_login_general.ToString();
            var m = Application.Current.MainWindow;
            m.Activate();
        }

        //btnPayCash_Click
        private void btnPayCash_Click(object sender, RoutedEventArgs e)
        {
            //Pages.Home.PayCash page = new Pages.Home.PayCash();
            Views.ShellOut page = new Views.ShellOut();
            page.btnpaycash_delegate += btnPayCash_Click_Delegate;
            page.ShowDialog();
        }

        //btnPayCash_Click_Delegate
        private void btnPayCash_Click_Delegate(bool flag_paycash)
        {
            if (flag_paycash == true)
            {
                //handling control
                if (StaticClass.GeneralClass.flag_paycash == true)
                {
                    StaticClass.GeneralClass.flag_paycash = false;
                    DisableContol();
                }

                //update list category
                GetCategory();

                if (list_ec_tb_category.Count > 0)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        Button bt = (Button)wpCategory.Children[0];
                        bt.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2B6017");
                        bt_current_selected = Convert.ToInt32(bt.Name.Substring(3));
                        bt_previous_selected = Convert.ToInt32(bt.Name.Substring(3));
                    }));

                    //for paging
                    categoryname_selected = FindResource("category").ToString();
                    paging_number_previous = 1;

                    selected_categoryid = bt_current_selected;

                    if (selected_categoryid > -1)
                    {
                        categoryname_selected = list_ec_tb_category[selected_categoryid].CategoryName;
                        condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                        condition_product2 = " And [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                        LoadDataProduct(condition_product1);
                    }
                }
                else
                {
                    this.wpProduct.Dispatcher.Invoke((Action)(() => { wpProduct.Children.Clear(); }));
                    list_ec_tb_product.Clear();
                    this.tblSumnProduct_Category.Dispatcher.Invoke((Action)(() => {
                        this.tblSumnProduct_Category.Text = FindResource("category").ToString() + "(0)";
                        stp_home_sort.Visibility = Visibility.Collapsed;
                    }));
                }

                //clear list_ec_tb_orderdetail_general
                StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Clear();
                this.Dispatcher.Invoke((Action)(() => { this.dtgOrderDetail.Items.Refresh(); }));

                //list_ec_tb_orderdetail_temp
                list_ec_tb_orderdetail_temp.Clear();

                //clear discount
                StaticClass.GeneralClass.discounttype_general = 0;
                StaticClass.GeneralClass.discount_general = 0;

                //update total statistic
                TotalStatistic();

                //enable control
                this.Dispatcher.Invoke((Action)(() => 
                {
                    this.btnSendEmail.IsEnabled = true;
                    this.muiBtnCustomer.IsEnabled = true;
                }));

                //update customer
                StaticClass.GeneralClass.customerid_general = 0;
                StaticClass.GeneralClass.customername_general = "None";
                StaticClass.GeneralClass.customeremail_general = "None";
                StaticClass.GeneralClass.customerphone_general = "None";
                this.tblCustomer.Dispatcher.Invoke((Action)(() => { this.tblCustomer.Text = StaticClass.GeneralClass.customername_general; }));
            }
        }

        //btnQty_Click
        private void btnQty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgOrderDetail.SelectedIndex > -1)
                {
                    if (sender is UIElement)
                    {
                        //get location for button qty
                        Point screenPoint = StaticClass.GeneralClass.ElementPointToScreenPoint(sender as UIElement, new Point(0, 0));

                        StaticClass.GeneralClass.dtgorderdetail_selectedindex_general = dtgOrderDetail.SelectedIndex;
                        StaticClass.GeneralClass.orderdetailqty_general = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].Qty;
                        StaticClass.GeneralClass.locationx_muibtnCustomer = screenPoint.X;
                        StaticClass.GeneralClass.locationy_muibtnCustomer = screenPoint.Y;

                        bool IsOpen = false;
                        System.Windows.Forms.FormCollection formcollection = System.Windows.Forms.Application.OpenForms;

                        foreach (System.Windows.Forms.Form frm in formcollection)
                        {
                            if (frm.Name == "Quantity")
                            {
                                IsOpen = true;
                                frm.Focus();
                                break;
                            }
                        }

                        if (IsOpen == false)
                        {
                            frmQuantity frm = new frmQuantity();
                            frm.str_notification = FindResource("exit_qty").ToString();
                            frm.str_warning1 = FindResource("warning_qty1").ToString();
                            frm.str_warning2 = FindResource("warning_qty2").ToString();
                            frm.str_warning3 = FindResource("warning_qty3").ToString();
                            frm.ShowInTaskbar = false;

                            DataTable datatable_row = new DataTable();
                            datatable_row = bus_tb_product.GetProduct("where ProductID = " + StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].ProductID);
                            frm.inventory = Convert.ToInt32(datatable_row.Rows[0]["InventoryCount"].ToString());

                            frm.btnqty_delegate += btnQty_Click_Delegate;
                            frm.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnQty_Click_Delegate
        private void btnQty_Click_Delegate(int qty)
        {
            if (dtgOrderDetail.SelectedIndex > -1)
            {
                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].Qty = qty;

                //update inventory for list product
                if (list_ec_tb_product.Count > 0)
                {
                    for (int i = 0; i < list_ec_tb_product.Count; i++)
                    {
                        if (list_ec_tb_product[i].ProductID == StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].ProductID)
                        {
                            //update inventory count for list_ec_tb_product
                            DataTable datatable_row = new DataTable();
                            datatable_row = bus_tb_product.GetProduct("where ProductID = " + StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].ProductID);
                            list_ec_tb_product[i].InventoryCount = Convert.ToInt32(datatable_row.Rows[0]["InventoryCount"].ToString()) - StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].Qty;

                            for (int c = 0; c < wpProduct.Children.Count; c++)
                            {
                                if (wpProduct.Children[c].Uid == "uc_product_uid_" + list_ec_tb_product[i].ProductID)
                                {
                                    UserControl uc = (UserControl)wpProduct.Children[c];
                                    uc.DataContext = null;
                                    uc.DataContext = list_ec_tb_product[i];
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }

                UpdateDiscountTotalTax(dtgOrderDetail.SelectedIndex);
                SaveListOrderDetailTemp(StaticClass.GeneralClass.dtgorderdetail_selectedindex_general);
                TotalStatistic();

                //help for barcode
                if (flag_check_barcode == true)
                    layout_update = true;
            }

            //help for barcode
            if (muiBtnScanBarcode.Visibility == System.Windows.Visibility.Collapsed)
                layout_update = true;
        }

        //btnDiscount_Click
        private void btnDiscount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Pages.Home.Discount page = new Pages.Home.Discount();
                page.btndiscount_delegate += btnDiscount_Click_Delegate;
                page.ShowDialog();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnDiscount_Click_Delegate
        private void btnDiscount_Click_Delegate()
        {
            try
            {
                TotalStatistic();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnDiscountDetail_Click
        private void btnDiscountDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgOrderDetail.SelectedIndex > -1)
                {
                    StaticClass.GeneralClass.dtgorderdetail_selectedindex_general = dtgOrderDetail.SelectedIndex;
                    StaticClass.GeneralClass.discountdetailtype_general = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].DiscountType;
                    StaticClass.GeneralClass.discountdetail_general = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].Discount;

                    Pages.Home.DiscountDetail page = new Pages.Home.DiscountDetail();
                    page.btndiscountdetail_delegate += btnDiscountDetail_Click_Delegate;
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnDiscountDetail_Click_Delegate
        private void btnDiscountDetail_Click_Delegate()
        {
            try
            {
                if (dtgOrderDetail.SelectedIndex > -1)
                {
                    if (StaticClass.GeneralClass.discountdetailtype_general == 1)
                    {
                        StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].DiscountTypeUnit0 = "";
                        StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].DiscountTypeUnit1 = "%";
                        StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].DiscountType = 1;
                    }
                    else
                    {
                        StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].DiscountTypeUnit0 = StaticClass.GeneralClass.currency_setting_general;
                        StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].DiscountTypeUnit1 = "";
                        StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].DiscountType = 0;
                    }

                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].Discount = StaticClass.GeneralClass.discountdetail_general;
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].StrDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].Discount);

                    UpdateDiscountTotalTax(dtgOrderDetail.SelectedIndex);
                    SaveListOrderDetailTemp(StaticClass.GeneralClass.dtgorderdetail_selectedindex_general);
                    TotalStatistic();

                    //check discount general
                    if (StaticClass.GeneralClass.discounttype_general == 0)
                    {
                        if (StaticClass.GeneralClass.subtotal_general < StaticClass.GeneralClass.discount_general)
                        {
                            Pages.Notification page = new Pages.Notification();
                            page.tblNotification.Text = FindResource("subtotal_greater").ToString();
                            page.ShowDialog();

                            //update discount
                            StaticClass.GeneralClass.discounttype_general = 0;
                            StaticClass.GeneralClass.discount_general = 0;
                            tblDiscount.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.discount_general);
                            tblDiscountMoney.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(0);
                            TotalStatistic();

                        }
                    }

                    //help for barcode
                    if (muiBtnScanBarcode.Visibility == System.Windows.Visibility.Collapsed)
                        layout_update = true;
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnDeleteOrderDetail_Click
        private void btnDeleteOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StaticClass.GeneralClass.dtgorderdetail_selectedindex_general = dtgOrderDetail.SelectedIndex;
                StaticClass.GeneralClass.dtgorderdetail_productid_general = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[dtgOrderDetail.SelectedIndex].ProductID;
                Pages.Home.DeleteOrderDetail page = new Pages.Home.DeleteOrderDetail();
                page.btndelete_orderdetail_delegate += btnDeleteOrderDetail_Click_Delegate;
                page.ShowDialog();

            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnDeleteOrderDetail_Click_Delegate
        private void btnDeleteOrderDetail_Click_Delegate()
        {
            try
            {
                TotalStatistic();

                //update list_ec_tb_orderdetail_temp
                int check_del = 0;
                do
                {
                    check_del = 0;

                    if (list_ec_tb_orderdetail_temp.Count > 0)
                    {
                        for (int i = 0; i < list_ec_tb_orderdetail_temp.Count; i++)
                        {
                            if (list_ec_tb_orderdetail_temp[i].ProductID == StaticClass.GeneralClass.dtgorderdetail_productid_general)
                            {
                                check_del = 1;
                                list_ec_tb_orderdetail_temp.RemoveAt(i);
                            }
                        }
                    }
                } while (check_del == 1);

                //update inventory for list product
                if (list_ec_tb_product.Count > 0)
                {
                    for (int i = 0; i < list_ec_tb_product.Count; i++)
                    {
                        if (list_ec_tb_product[i].ProductID == StaticClass.GeneralClass.dtgorderdetail_productid_general)
                        {
                            //update inventory count for list_ec_tb_product
                            DataTable datatable_row = new DataTable();
                            datatable_row = bus_tb_product.GetProduct("where ProductID = " + StaticClass.GeneralClass.dtgorderdetail_productid_general);

                            list_ec_tb_product[i].InventoryCount = Convert.ToInt32(datatable_row.Rows[0]["InventoryCount"].ToString());

                            for (int c = 0; c < wpProduct.Children.Count; c++)
                            {
                                if (wpProduct.Children[c].Uid == "uc_product_uid_" + list_ec_tb_product[i].ProductID)
                                {
                                    UserControl uc = (UserControl)wpProduct.Children[c];
                                    uc.DataContext = null;
                                    uc.DataContext = list_ec_tb_product[i];
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }

                //check for disable control
                if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count == 0)
                    DisableContol();

                //help for barcode
                if (muiBtnScanBarcode.Visibility == System.Windows.Visibility.Collapsed)
                    layout_update = true;
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnPayOther_Click
        private void btnPayOther_Click(object sender, RoutedEventArgs e)
        {
            Pages.Home.PayOther page = new Pages.Home.PayOther();
            page.btnpayother_delegate += btnPayOther_Click_Delegate;
            page.ShowDialog();
        }

        //btnPayOther_Click_Delegate
        private void btnPayOther_Click_Delegate(bool flag_paycash)
        {
            if (flag_paycash == true)
            {
                //handling control
                if (StaticClass.GeneralClass.flag_paycash == true)
                {
                    StaticClass.GeneralClass.flag_paycash = false;
                    DisableContol();
                }

                //update list category
                GetCategory();

                if (list_ec_tb_category.Count > 0)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        Button bt = (Button)wpCategory.Children[0];
                        bt.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2B6017");
                        bt_current_selected = Convert.ToInt32(bt.Name.Substring(3));
                        bt_previous_selected = Convert.ToInt32(bt.Name.Substring(3));
                    }));

                    //for paging
                    categoryname_selected = FindResource("category").ToString();
                    paging_number_previous = 1;

                    selected_categoryid = bt_current_selected;

                    if (selected_categoryid > -1)
                    {
                        categoryname_selected = list_ec_tb_category[selected_categoryid].CategoryName;
                        condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                        condition_product2 = " And [CategoryID] = " + list_ec_tb_category[selected_categoryid].CategoryID + " And [Active] = 1";
                        LoadDataProduct(condition_product1);
                    }
                }
                else
                {
                    this.wpProduct.Dispatcher.Invoke((Action)(() => { wpProduct.Children.Clear(); }));
                    list_ec_tb_product.Clear();
                    this.tblSumnProduct_Category.Dispatcher.Invoke((Action)(() => {
                        this.tblSumnProduct_Category.Text = FindResource("category").ToString() + "(0)";
                        stp_home_sort.Visibility = Visibility.Collapsed;
                    }));
                }

                //clear list_ec_tb_orderdetail_general
                StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Clear();
                this.Dispatcher.Invoke((Action)(() => { this.dtgOrderDetail.Items.Refresh(); }));

                //list_ec_tb_orderdetail_temp
                list_ec_tb_orderdetail_temp.Clear();

                //clear discount
                StaticClass.GeneralClass.discounttype_general = 0;
                StaticClass.GeneralClass.discount_general = 0;

                //update total statistic
                TotalStatistic();

                //enable control
                this.btnSendEmail.Dispatcher.Invoke((Action)(() => { this.btnSendEmail.IsEnabled = true; }));
                this.muiBtnCustomer.Dispatcher.Invoke((Action)(() => { this.muiBtnCustomer.IsEnabled = true; }));

                //update customer
                StaticClass.GeneralClass.customerid_general = 0;
                StaticClass.GeneralClass.customername_general = "None";
                StaticClass.GeneralClass.customeremail_general = "None";
                StaticClass.GeneralClass.customerphone_general = "None";
                this.tblCustomer.Dispatcher.Invoke((Action)(() => { this.tblCustomer.Text = StaticClass.GeneralClass.customername_general; }));
            }
        }

        //btnSendEmail_Click
        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SendEmail page = new SendEmail();
                    page.ShowInTaskbar = false;
                    var m = Application.Current.MainWindow;
                    page.Owner = m;

                    if (m.RenderSize.Width > StaticClass.GeneralClass.width_screen_general)
                        page.Width = StaticClass.GeneralClass.width_screen_general * 90 / 100;
                    else
                        page.Width = m.RenderSize.Width * 90 / 100;

                    if (m.RenderSize.Height > StaticClass.GeneralClass.height_screen_working_general)
                        page.Height = StaticClass.GeneralClass.height_screen_working_general;
                    else
                        page.Height = m.RenderSize.Height;

                    page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    page.btnsend_delegate += btnSendEmail_Click_Delegate;
                    page.ShowDialog();
                }));
            }).Start();
        }

        //btnSendEmail_Click_Delegate
        private void btnSendEmail_Click_Delegate()
        {
            var m = Application.Current.MainWindow;
            m.Activate();
        }

        //btnClearAll_Click
        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //clear discount
                StaticClass.GeneralClass.discounttype_general = 0;
                StaticClass.GeneralClass.discount_general = 0;

                //clear list_ec_tb_orderdetail_general
                StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Clear();
                dtgOrderDetail.Items.Refresh();

                //clear list_ec_tb_orderdetail_temp
                list_ec_tb_orderdetail_temp.Clear();

                ////update inventory for list product
                if (list_ec_tb_product.Count > 0)
                {
                    for (int i = 0; i < list_ec_tb_product.Count; i++)
                    {
                        //update inventory count for list_ec_tb_product
                        DataTable datatable_row = new DataTable();
                        datatable_row = bus_tb_product.GetProduct("where ProductID = " + list_ec_tb_product[i].ProductID);

                        list_ec_tb_product[i].InventoryCount = Convert.ToInt32(datatable_row.Rows[0]["InventoryCount"].ToString());

                        for (int c = 0; c < wpProduct.Children.Count; c++)
                        {
                            if (wpProduct.Children[c].Uid == "uc_product_uid_" + list_ec_tb_product[i].ProductID)
                            {
                                UserControl uc = (UserControl)wpProduct.Children[c];
                                uc.DataContext = null;
                                uc.DataContext = list_ec_tb_product[i];
                                break;
                            }
                        }
                    }
                }

                TotalStatistic();

                //help for barcode
                if (muiBtnScanBarcode.Visibility == System.Windows.Visibility.Collapsed)
                    layout_update = true;

                //disable control
                DisableContol();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnUndo_Click
        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int maxindex = list_ec_tb_orderdetail_temp.Count;

                if (maxindex > 0)
                {
                    maxindex--;
                    int productid_undo = list_ec_tb_orderdetail_temp[maxindex].ProductID;
                    int num_productid_temp = 0;

                    for (int i = 0; i < list_ec_tb_orderdetail_temp.Count; i++)
                    {
                        if (list_ec_tb_orderdetail_temp[i].ProductID == productid_undo)
                            num_productid_temp++;
                    }

                    double index_productid_temp_pre = -1;
                    if (num_productid_temp > 1)
                    {
                        for (int i = 0; i < list_ec_tb_orderdetail_temp.Count - 1; i++)
                        {
                            if (list_ec_tb_orderdetail_temp[i].ProductID == list_ec_tb_orderdetail_temp[maxindex].ProductID)
                                index_productid_temp_pre = i;
                        }
                    }

                    if (index_productid_temp_pre == -1)
                    {
                        for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                        {
                            if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID == list_ec_tb_orderdetail_temp[maxindex].ProductID)
                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general.RemoveAt(i);
                        }
                    }
                    else
                    {
                        int index_product_tem_pre = (int)index_productid_temp_pre;
                        for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                        {
                            if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID == list_ec_tb_orderdetail_temp[index_product_tem_pre].ProductID)
                            {
                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty = list_ec_tb_orderdetail_temp[index_product_tem_pre].Qty;
                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].DiscountType = list_ec_tb_orderdetail_temp[index_product_tem_pre].DiscountType;

                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Discount = list_ec_tb_orderdetail_temp[index_product_tem_pre].Discount;
                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].StrDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Discount);

                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].TotalDiscount = list_ec_tb_orderdetail_temp[index_product_tem_pre].TotalDiscount;
                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].StrTotalDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].TotalDiscount);

                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Total = list_ec_tb_orderdetail_temp[index_product_tem_pre].Total;
                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].StrTotal = StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Total);

                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].DiscountTypeUnit0 = list_ec_tb_orderdetail_temp[index_product_tem_pre].DiscountTypeUnit0;
                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].DiscountTypeUnit1 = list_ec_tb_orderdetail_temp[index_product_tem_pre].DiscountTypeUnit1;
                                StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Tax = list_ec_tb_orderdetail_temp[index_product_tem_pre].Tax;
                            }
                        }
                    }

                    if (list_ec_tb_product.Count > 0)
                    {
                        for (int _i = 0; _i < list_ec_tb_product.Count; _i++)
                        {
                            if (list_ec_tb_orderdetail_temp[maxindex].ProductID == list_ec_tb_product[_i].ProductID)
                            {
                                //update amount inventory for list_ec_tb_product
                                bool __flag = false;
                                if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count > 0)
                                {
                                    for (int odd = 0; odd < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; odd++)
                                    {
                                        if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[odd].ProductID == list_ec_tb_product[_i].ProductID)
                                        {
                                            DataTable datatable_row = new DataTable();
                                            datatable_row = bus_tb_product.GetProduct("where ProductID = " + StaticClass.GeneralClass.list_ec_tb_orderdetail_general[odd].ProductID);

                                            list_ec_tb_product[_i].InventoryCount = Convert.ToInt32(datatable_row.Rows[0]["InventoryCount"].ToString()) - StaticClass.GeneralClass.list_ec_tb_orderdetail_general[odd].Qty;
                                            __flag = true;
                                            break;
                                        }
                                    }
                                }

                                if (__flag == false)
                                {
                                    DataTable datatable_row = new DataTable();
                                    datatable_row = bus_tb_product.GetProduct("where ProductID = " + list_ec_tb_product[_i].ProductID);

                                    list_ec_tb_product[_i].InventoryCount = Convert.ToInt32(datatable_row.Rows[0]["InventoryCount"].ToString());
                                }

                                for (int c = 0; c < wpProduct.Children.Count; c++)
                                {
                                    if (wpProduct.Children[c].Uid == "uc_product_uid_" + list_ec_tb_product[_i].ProductID)
                                    {
                                        UserControl uc = (UserControl)wpProduct.Children[c];
                                        uc.DataContext = null;
                                        uc.DataContext = list_ec_tb_product[_i];
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }

                    list_ec_tb_orderdetail_temp.RemoveAt(maxindex);
                    dtgOrderDetail.Items.Refresh();
                    TotalStatistic();
                }

                //handling control
                if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count == 0)
                {
                    DisableContol();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //UserControl_KeyDown
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            this.txbBarcode.Dispatcher.Invoke((Action)(() => { this.txbBarcode.Focus(); }));
        }

        #region function temp
        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            //txbBarcode.Focus();
            //mprStatus.IsActive = true;
        }

        //txbBarcode_GotFocus
        private void txbBarcode_GotFocus(object sender, RoutedEventArgs e)
        {
            //mprStatus.IsActive = true;
        }

        //txbBarcode_LostFocus
        private void txbBarcode_LostFocus(object sender, RoutedEventArgs e)
        {
            //mprStatus.IsActive = false;
            //txbBarcode.IsEnabled = false;
        }
        #endregion

        //txbBarcode_TextChanged
        private void txbBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txbBarcode.Text.Trim().Length == 1)
                {
                    timer_delay.Enabled = true;
                    timer_delay.Start();
                    timer_delay.Tick += new EventHandler(TimerDelayTick);
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //TimerDelayTick
        private void TimerDelayTick(object sender, EventArgs e)
        {
            try
            {
                timer_delay.Stop();
                string barcode_string = txbBarcode.Text.Trim().ToString();
                if (barcode_string != "")
                {
                    //do something with the barcode enter
                    txbBarcode.Text = "";
                    AddProductBarcode(barcode_string);
                }
                txbBarcode.Focus();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //AddProductBarcode
        private void AddProductBarcode(string barcodeid)
        {
            if (thread_scan_barcode != null && thread_scan_barcode.ThreadState == ThreadState.Running) { }
            else
            {
                thread_scan_barcode = new Thread(() =>
                {
                    try
                    {
                        this.prbScanBarcode.Dispatcher.Invoke((Action)(() => { this.prbScanBarcode.IsIndeterminate = true; }));
                        DataTable datatable_row_barcode = new DataTable();
                        datatable_row_barcode = bus_tb_product.GetProduct("where BarcodeID = '" + barcodeid + "' and Active = 1");

                        if (datatable_row_barcode.Rows.Count > 0)
                        {
                            //check inventory
                            bool flag_exist = false;

                            if (Convert.ToInt32(datatable_row_barcode.Rows[0]["InventoryCount"].ToString()) == 0)
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    Pages.Notification page = new Pages.Notification();
                                    page.tblNotification.Text = FindResource("product_out_stock").ToString();
                                    page.tblNote.Text = FindResource("amount_inventory").ToString() + ": " + datatable_row_barcode.Rows[0]["InventoryCount"].ToString();
                                    page.ShowDialog();
                                }));
                            }
                            else
                            {
                                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                                {
                                    //product is exist in orderdetail
                                    if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID == Convert.ToInt32(datatable_row_barcode.Rows[0]["ProductID"].ToString()))
                                    {
                                        //this product is out of stock
                                        if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty == Convert.ToInt32(datatable_row_barcode.Rows[0]["InventoryCount"].ToString()))
                                        {
                                            this.Dispatcher.Invoke((Action)(() =>
                                            {
                                                Pages.Notification page = new Pages.Notification();
                                                page.tblNotification.Text = FindResource("product_out_stock").ToString();
                                                page.tblNote.Text = FindResource("amount_inventory").ToString() + ": " + datatable_row_barcode.Rows[0]["InventoryCount"].ToString();
                                                page.ShowDialog();
                                            }));
                                            this.prbScanBarcode.Dispatcher.Invoke((Action)(() => { this.prbScanBarcode.IsIndeterminate = false; }));
                                            return;
                                        }
                                        //this product isn't out fo stock
                                        else
                                        {
                                            StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty++;

                                            //update subtotal-discount-tax-total
                                            UpdateDiscountTotalTax(i);

                                            //save into list orderdetail temp
                                            SaveListOrderDetailTemp(i);
                                        }

                                        flag_exist = true;
                                        break;
                                    }
                                }

                                //this product isn't in orderdetail
                                if ((flag_exist == false) && (Convert.ToInt32(datatable_row_barcode.Rows[0]["InventoryCount"].ToString()) > 0))
                                {
                                    EC_tb_OrderDetail ec_tb_orderdetail = new EC_tb_OrderDetail();
                                    ec_tb_orderdetail.CategoryID = Convert.ToInt32(datatable_row_barcode.Rows[0]["CategoryID"].ToString());

                                    for (int i = 0; i < list_ec_tb_category.Count; i++)
                                    {
                                        if (ec_tb_orderdetail.CategoryID == list_ec_tb_category[i].CategoryID)
                                        {
                                            ec_tb_orderdetail.CategoryName = StaticClass.GeneralClass.HandlingSpecialCharacter(list_ec_tb_category[i].CategoryName);
                                            break;
                                        }
                                    }
                                    ec_tb_orderdetail.ProductID = Convert.ToInt32(datatable_row_barcode.Rows[0]["ProductID"].ToString());
                                    ec_tb_orderdetail.ProductName = datatable_row_barcode.Rows[0]["ShortName"].ToString();
                                    ec_tb_orderdetail.Cost = Convert.ToDecimal(datatable_row_barcode.Rows[0]["Cost"].ToString());

                                    ec_tb_orderdetail.Price = Convert.ToDecimal(datatable_row_barcode.Rows[0]["Price"].ToString());
                                    ec_tb_orderdetail.StrPrice = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Price);

                                    ec_tb_orderdetail.Qty = 1;

                                    if (Convert.ToInt32(datatable_row_barcode.Rows[0]["Tax"].ToString()) == 1)
                                        ec_tb_orderdetail.Tax = (Convert.ToDecimal(datatable_row_barcode.Rows[0]["Price"].ToString()) * StaticClass.GeneralClass.taxrate_setting_general / 100);
                                    else
                                        ec_tb_orderdetail.Tax = 0;

                                    ec_tb_orderdetail.DiscountType = 0;
                                    ec_tb_orderdetail.Discount = 0;
                                    ec_tb_orderdetail.StrDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Discount);

                                    ec_tb_orderdetail.TotalDiscount = 0;
                                    ec_tb_orderdetail.StrTotalDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(0);

                                    ec_tb_orderdetail.Total = Convert.ToDecimal(datatable_row_barcode.Rows[0]["Price"].ToString());
                                    ec_tb_orderdetail.StrTotal = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Total);

                                    //add info
                                    ec_tb_orderdetail.Currency = StaticClass.GeneralClass.currency_setting_general;
                                    ec_tb_orderdetail.DiscountTypeUnit0 = StaticClass.GeneralClass.currency_setting_general;
                                    ec_tb_orderdetail.DiscountTypeUnit1 = "";
                                    ec_tb_orderdetail.ImageDeleteUrl = @"pack://application:,,,/Resources/delete_orderdetail.png";

                                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Add(ec_tb_orderdetail);
                                    //save into list orderdetail temp
                                    SaveListOrderDetailTemp(StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count - 1);
                                }

                                for (int j = 0; j < list_ec_tb_product.Count; j++)
                                {
                                    if (list_ec_tb_product[j].ProductID == Convert.ToInt32(datatable_row_barcode.Rows[0]["ProductID"].ToString()))
                                    {
                                        list_ec_tb_product[j].InventoryCount--;
                                        this.Dispatcher.Invoke((Action)(() =>
                                        {
                                            for (int c = 0; c < wpProduct.Children.Count; c++)
                                            {
                                                if (wpProduct.Children[c].Uid == "uc_product_uid_" + list_ec_tb_product[j].ProductID)
                                                {
                                                    UserControl uc = (UserControl)wpProduct.Children[c];
                                                    uc.DataContext = null;
                                                    uc.DataContext = list_ec_tb_product[j];
                                                    break;
                                                }
                                            }
                                        }));
                                        break;
                                    }
                                }

                                //total statistic
                                TotalStatistic();

                                //enable control
                                EnableContol();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.Buttons = new Button[] { md.OkButton, };
                            md.OkButton.Content = FindResource("ok").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }

                    this.prbScanBarcode.Dispatcher.Invoke((Action)(() => { this.prbScanBarcode.IsIndeterminate = false; }));
                });
                thread_scan_barcode.Start();
            }
        }

        //btnQRCode_Click
        private void btnQRCode_Click(object sender, RoutedEventArgs e)
        {
            txbBarcode.IsEnabled = true;
            txbBarcode.Focus();
        }

        //FindVisualChild
        private ChildItem FindVisualChild<ChildItem>(DependencyObject obj) where ChildItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is ChildItem)
                    return (ChildItem)child;
                else
                {
                    ChildItem childOfChild = FindVisualChild<ChildItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }


        //UserControl_LayoutUpdated
        private bool layout_update = false;
        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            if (layout_update == true)
            {
                layout_update = false;
                txbBarcode.Focus();
            }

            //check change accent color
            if (StaticClass.GeneralClass.accent_color_change == true)
            {
                StaticClass.GeneralClass.accent_color_change = false;
                SetStartControl();
            }
        }

        //muiBtnSearch_Click
        private bool flag_check_barcode = false;
        private void muiBtnSearch_Click(object sender, RoutedEventArgs e)
        {
            flag_check_barcode = false;
            muiBtnSearch.Visibility = System.Windows.Visibility.Collapsed;
            this.KeyDown -= new KeyEventHandler(UserControl_KeyDown);
            muiBtnScanBarcode.Visibility = System.Windows.Visibility.Visible;
            txbSearch.Visibility = System.Windows.Visibility.Visible;
            txbSearch.Focus();
        }

        //muiBtnScanBarcode_Click
        private void muiBtnScanBarcode_Click(object sender, RoutedEventArgs e)
        {
            flag_check_barcode = true;
            muiBtnScanBarcode.Visibility = System.Windows.Visibility.Collapsed;
            txbSearch.Visibility = System.Windows.Visibility.Hidden;
            muiBtnSearch.Visibility = System.Windows.Visibility.Visible;
            this.KeyDown += new KeyEventHandler(UserControl_KeyDown);
            txbBarcode.Focus();
        }

        //txbSearch_KeyDown
        private void txbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                if (wpCategory.Children.Count > 0 && (wpCategory.Children.Count - 1) >= bt_previous_selected)
                {
                    if (bt_previous_selected >= 0)
                    {
                        Button bt = (Button)wpCategory.Children[bt_previous_selected];
                        bt.Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF3F7224");
                    }
                }

                bt_current_selected = -1;
                bt_previous_selected = -1;

                categoryname_selected = FindResource("category").ToString();
                condition_product1 = " Where ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')" + " And [Active] = 1";
                condition_product2 = " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')" + " And [Active] = 1";
                LoadDataProduct(condition_product1);
            }
        }

        //UCProduct_MouseDown
        private void UCProduct_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UserControl uc = (UserControl)sender;
            int product_index = Convert.ToInt32(uc.Name.Substring(16));
            int product_id = Convert.ToInt32(uc.Uid.Substring(15));

            try
            {
                //check inventory
                DataTable datable_row = new DataTable();
                datable_row = bus_tb_product.GetProduct("where ProductID=" + product_id);
                bool flag_exist = false;

                if (datable_row != null)
                {
                    if(Convert.ToInt32(datable_row.Rows[0]["InventoryCount"].ToString()) == 0)
                    {
                        Pages.Notification page = new Pages.Notification();
                        page.tblNotification.Text = FindResource("product_out_stock").ToString();
                        page.tblNote.Text = FindResource("amount_inventory").ToString() + ": " + datable_row.Rows[0]["InventoryCount"].ToString();
                        page.ShowDialog();
                    }
                    else
                    {
                        for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                        {
                            //product is exist in orderdetail
                            if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID == Convert.ToInt32(datable_row.Rows[0]["ProductID"].ToString()))
                            {
                                //this product is out of stock
                                if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty == Convert.ToInt32(datable_row.Rows[0]["InventoryCount"].ToString()))
                                {
                                    Pages.Notification page = new Pages.Notification();
                                    page.tblNotification.Text = FindResource("product_out_stock").ToString();
                                    page.tblNote.Text = FindResource("amount_inventory").ToString() + ": " + datable_row.Rows[0]["InventoryCount"].ToString();
                                    page.ShowDialog();
                                    return;
                                }
                                //this product isn't out fo stock
                                else
                                {
                                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty++;

                                    //update subtotal-discount-tax-total
                                    UpdateDiscountTotalTax(i);

                                    //save into list orderdetail temp
                                    SaveListOrderDetailTemp(i);
                                }

                                flag_exist = true;
                                break;
                            }
                        }

                        //this product isn't in orderdetail
                        if (flag_exist == false)
                        {
                            EC_tb_OrderDetail ec_tb_orderdetail = new EC_tb_OrderDetail();
                            ec_tb_orderdetail.CategoryID = list_ec_tb_product[product_index].CategoryID;
                            for (int i = 0; i < list_ec_tb_category.Count; i++)
                            {
                                if (list_ec_tb_product[product_index].CategoryID == list_ec_tb_category[i].CategoryID)
                                {
                                    ec_tb_orderdetail.CategoryName = StaticClass.GeneralClass.HandlingSpecialCharacter(list_ec_tb_category[i].CategoryName);
                                    break;
                                }
                            }
                            ec_tb_orderdetail.ProductID = list_ec_tb_product[product_index].ProductID;
                            ec_tb_orderdetail.ProductName = list_ec_tb_product[product_index].ShortName;
                            ec_tb_orderdetail.Cost = list_ec_tb_product[product_index].Cost;

                            ec_tb_orderdetail.Price = list_ec_tb_product[product_index].Price;
                            ec_tb_orderdetail.StrPrice = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Price);

                            ec_tb_orderdetail.Qty = 1;

                            if (list_ec_tb_product[product_index].Tax == 1)
                                ec_tb_orderdetail.Tax = (list_ec_tb_product[product_index].Price * StaticClass.GeneralClass.taxrate_setting_general / 100);
                            else
                                ec_tb_orderdetail.Tax = 0;

                            ec_tb_orderdetail.DiscountType = 0;
                            ec_tb_orderdetail.Discount = 0;
                            ec_tb_orderdetail.StrDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Discount);

                            ec_tb_orderdetail.TotalDiscount = 0;
                            ec_tb_orderdetail.StrTotalDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(0);

                            ec_tb_orderdetail.Total = list_ec_tb_product[product_index].Price;
                            ec_tb_orderdetail.StrTotal = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Total);

                            //add info
                            ec_tb_orderdetail.Currency = StaticClass.GeneralClass.currency_setting_general;
                            ec_tb_orderdetail.DiscountTypeUnit0 = StaticClass.GeneralClass.currency_setting_general;
                            ec_tb_orderdetail.DiscountTypeUnit1 = "";
                            ec_tb_orderdetail.ImageDeleteUrl = @"pack://application:,,,/Resources/delete_orderdetail.png";

                            StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Add(ec_tb_orderdetail);

                            //save into list orderdetail temp
                            SaveListOrderDetailTemp(StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count - 1);
                        }

                        list_ec_tb_product[product_index].InventoryCount--;
                        uc.DataContext = null;
                        uc.DataContext = list_ec_tb_product[product_index];

                        //total statistic
                        TotalStatistic();

                        //enable control
                        EnableContol();
                    }
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //UserControl_SizeChanged
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //help for barcode
            if (muiBtnScanBarcode.Visibility == System.Windows.Visibility.Collapsed)
            {
                layout_update = true;
            }
        }

        //scvCategory_PreviewMouseWheel
        private void scvCategory_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset - e.Delta);
            e.Handled = true;
        }

        //btnUndo_MouseEnter
        private void btnUndo_MouseEnter(object sender, MouseEventArgs e)
        {
            this.btnUndo.Foreground = (System.Windows.Media.Brush) new System.Windows.Media.BrushConverter().ConvertFromString("#FFCCBC");
        }

        //btnUndo_MouseLeave
        private void btnUndo_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnUndo.Foreground = System.Windows.Media.Brushes.White;
        }

        //btnSendEmail_MouseEnter
        private void btnSendEmail_MouseEnter(object sender, MouseEventArgs e)
        {
            this.btnSendEmail.Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFCCBC");
        }

        //btnSendEmail_MouseLeave
        private void btnSendEmail_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnSendEmail.Foreground = System.Windows.Media.Brushes.White;
        }

        //btnClearAll_MouseEnter
        private void btnClearAll_MouseEnter(object sender, MouseEventArgs e)
        {
            this.btnClearAll.Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFCCBC");
        }

        //btnClearAll_MouseLeave
        private void btnClearAll_MouseLeave(object sender, MouseEventArgs e)
        {
            if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count > 0)
            {
                this.btnClearAll.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        //btnPayCash_MouseEnter
        private void btnPayCash_MouseEnter(object sender, MouseEventArgs e)
        {
            this.btnPayCash.Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFCCBC");
        }

        //btnPayCash_MouseLeave
        private void btnPayCash_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnPayCash.Foreground = System.Windows.Media.Brushes.White;
        }

        //btnDiscount_MouseEnter
        private void btnDiscount_MouseEnter(object sender, MouseEventArgs e)
        {
            this.btnDiscount.Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFCCBC");
        }

        //btnDiscount_MouseLeave
        private void btnDiscount_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnDiscount.Foreground = System.Windows.Media.Brushes.White;
        }

        //btnPayOther_MouseEnter
        private void btnPayOther_MouseEnter(object sender, MouseEventArgs e)
        {
            //this.btnPayOther.Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFCCBC");
        }

        //btnPayOther_MouseLeave
        private void btnPayOther_MouseLeave(object sender, MouseEventArgs e)
        {
            //this.btnPayOther.Foreground = System.Windows.Media.Brushes.White;
        }

        //btnSalesperson_MouseEnter
        private void btnSalesperson_MouseEnter(object sender, MouseEventArgs e)
        {
            this.btnSalesperson.Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFCCBC");
        }

        //btnSalesperson_MouseLeave
        private void btnSalesperson_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnSalesperson.Foreground = System.Windows.Media.Brushes.White;
        }

        //bdOrderDetail_SizeChanged
        private void bdOrderDetail_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            StaticClass.GeneralClass.app_settings["orderDetailWidth"] = this.bdOrderDetail.RenderSize.Width;
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            this.tblTotalProduct.Margin = new Thickness((Convert.ToDouble(StaticClass.GeneralClass.app_settings["orderDetailWidth"].ToString()) - 17.5) / 2, 0, 5, 0);
            this.muiBtnCustomer.Margin = new Thickness(Convert.ToDouble(StaticClass.GeneralClass.app_settings["orderDetailWidth"].ToString()) - 17.5, 0, 5, 0);
            this.tblSumnProduct_Category.Margin = new Thickness(Convert.ToDouble(StaticClass.GeneralClass.app_settings["orderDetailWidth"].ToString()) + (Application.Current.MainWindow.RenderSize.Width - Convert.ToDouble(StaticClass.GeneralClass.app_settings["orderDetailWidth"].ToString())) / 2 - 17.5, 0, 5, 0);
        }

        private void cb_SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox cbb = (ComboBox)sender;
                HomeProdSort = (prodSort)Enum.Parse(typeof(prodSort), cbb.SelectedValue.ToString());
                LoadDataProduct(condition_product1);
            }
            catch { }
        }
        
        private string _buildStringOderBy()
        {
            string _strOderBy = string.Empty;
            string _strTemp = string.Empty;
            if (HomeProdSort == prodSort.Name)
                _strTemp += "lower(ShortName)";
            else if (HomeProdSort == prodSort.Price)
                _strTemp += "Price";
            else if (HomeProdSort == prodSort.DateInserted)
                _strTemp += "ProductID";
            else if (HomeProdSort == prodSort.Seller)
                _strTemp += "Total";
            if (!string.IsNullOrEmpty(_strTemp))
            {
                _strOderBy = string.Format("order by {0} {1}", _strTemp, HomeDirectSort.ToString());
            }
            return _strOderBy;
        }

        /*private void cb_SortDirect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (HomeProdSort == prodSort.Seller)
                {
                    _isSeller = true;
                }
                else _isSeller = false;
                ComboBox cbb = (ComboBox)sender;
                HomeDirectSort = (directSort)Enum.Parse(typeof(directSort), cbb.SelectedValue.ToString());
                LoadDataProduct(condition_product1);
            }
            catch { }
        }*/
    }

    /// <summary>
    /// ButtonColor
    /// </summary>
    public class ButtonColor
    {
        private System.Windows.Media.SolidColorBrush _Solid_Color_Brush;
        public System.Windows.Media.SolidColorBrush Solid_Color_Brush
        {
            get { return _Solid_Color_Brush; }
            set { _Solid_Color_Brush = value; }
        }
    }
    public enum prodSort
    {
        Name, Price, DateInserted, Seller
    }
    public enum directSort
    {
        ASC, DESC
    }
    public class ProductSort
    {
        public prodSort SortBy { get; set; }
        public string DisplayName { get; set; }
    }
    public class DirectSort
    {
        public directSort DirectValue { get; set; }
        public string DirectString { get; set; }
    }
}
