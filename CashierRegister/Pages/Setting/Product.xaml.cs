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
using System.Threading;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : UserControl
    {
        //using for directory
        private string cur_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        //using for category
        private BUS_tb_Category bus_tb_category = new BUS_tb_Category();
        private List<EC_tb_Category> list_ec_tb_category = new List<EC_tb_Category>();

        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();
        private List<EC_tb_Product> list_ec_tb_product = new List<EC_tb_Product>();
        private bool flag_check_loaded = false;
        //thread
        private Thread thread_cat = null;
        private Thread thread_pro = null;

        private bool _exportIsRuning = false;
        private bool _exportStatus = false;
        private System.Windows.Threading.DispatcherTimer _myTimer_ = null;
        private Waiting _Waiting = null;
        private Thread _threadWaiting = null;
        Microsoft.Win32.SaveFileDialog _dlg = null;
        public List<ProductSort> ProdSort { get; set; }
        public List<DirectSort> ProdDirect { get; set; }

        //Product
        public Product()
        {
            InitializeComponent();
            
            dtgProduct.ItemsSource = list_ec_tb_product;
            lbCategory.ItemsSource = list_ec_tb_category;
            ProdSort = new List<ProductSort>()
            {
                new ProductSort() { SortBy = prodSort.Cost, DisplayName = "Cost" },
                new ProductSort() { SortBy = prodSort.Country, DisplayName = "Country" },
                new ProductSort() { SortBy = prodSort.DateInserted, DisplayName = "Date inserted" },
                new ProductSort() { SortBy = prodSort.InventoryCount, DisplayName = "Inventory" },
                new ProductSort() { SortBy = prodSort.LongName, DisplayName = "Long Name" },
                new ProductSort() { SortBy = prodSort.Seller, DisplayName = "No. items sold" },
                new ProductSort() { SortBy = prodSort.Price, DisplayName = "Price" },
                new ProductSort() { SortBy = prodSort.ShortName, DisplayName = "Short Name" },
                new ProductSort() { SortBy = prodSort.SizeWeight, DisplayName = "Size Weight" }
            };
            ProdDirect = new List<DirectSort>()
            {
                new DirectSort() { DirectValue = directSort.ASC, DirectString = "Ascending" },
                new DirectSort() { DirectValue = directSort.DESC, DirectString = "Descending" }
            };
            cb_SortBy_prod.ItemsSource = ProdSort;
            cb_SortBy_prod.SelectedValue = prodSort.ShortName;
            cb_SortBy_prod.DisplayMemberPath = "DisplayName";
            cb_SortBy_prod.SelectedValuePath = "SortBy";
            /*cb_SortDirect_prod.ItemsSource = ProdDirect;
            cb_SortDirect_prod.DisplayMemberPath = "DirectString";
            cb_SortDirect_prod.SelectedValuePath = "DirectValue";
            cb_SortDirect_prod.SelectedValue = directSort.ASC;*/
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
        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                GetCategory(false, false);
            }
        }

        //get category
        private DataTable dt_category = new DataTable();
        private string be_image_edit = @"pack://application:,,,/Resources/be_edit_category.png";
        private string af_image_edit = @"pack://application:,,,/Resources/af_edit_category.png";

        private string be_image_delete = @"pack://application:,,,/Resources/be_delete_category.png";
        private string af_image_delete = @"pack://application:,,,/Resources/af_delete_category.png";
        private void GetCategory(bool flag_add, bool flag_edit)
        {
            if (thread_cat != null && thread_cat.ThreadState == ThreadState.Running) { }
            else
            {
                thread_cat = new Thread((() =>
                {
                    try
                    {
                        list_ec_tb_category.Clear();
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.lbCategory.Items.Refresh();
                            this.lbCategory.Visibility = System.Windows.Visibility.Hidden;

                            this.mprcat.Visibility = System.Windows.Visibility.Visible;
                            this.mprcat.IsActive = true;
                        }));

                        dt_category.Clear();
                        int _index = -1;
                        dt_category = bus_tb_category.GetCatagorySetting("");
                        foreach (DataRow dr in dt_category.Rows)
                        {
                            list_ec_tb_category.Add(new EC_tb_Category { Index = ++_index, CategoryID = Convert.ToInt32(dr["CategoryID"].ToString()), CategoryName = dr["CategoryName"].ToString(), BeImageEdit = be_image_edit, AfImageEdit = af_image_edit, BeImageDelete = be_image_delete, AfImageDelete = af_image_delete, });
                        }

                        this.tblNumCategory.Dispatcher.Invoke((Action)(() => { tblNumCategory.Text = FindResource("categorys").ToString() + "(" + list_ec_tb_category.Count + ")"; }));
                        this.lbCategory.Dispatcher.Invoke((Action)(() => { lbCategory.Items.Refresh(); }));

                        Thread.Sleep(500);
                        this.mprcat.Dispatcher.Invoke((Action)(() => { this.mprcat.IsActive = false; }));
                        this.lbCategory.Dispatcher.Invoke((Action)(() => { this.lbCategory.Visibility = System.Windows.Visibility.Visible; }));

                        this.lbCategory.Dispatcher.Invoke((Action)(() =>
                        {
                            this.lbCategory.SelectionChanged -= new SelectionChangedEventHandler(lbCategory_SelectionChanged);
                            this.lbCategory.SelectionChanged += new SelectionChangedEventHandler(lbCategory_SelectionChanged);
                        }));

                        if ((list_ec_tb_category.Count > 0) && (flag_add == false) && (flag_edit == false))
                        {
                            this.lbCategory.Dispatcher.Invoke((Action)(() =>
                            {
                                this.lbCategory.SelectedIndex = 0;
                                this.lbCategory.ScrollIntoView(lbCategory.Items.GetItemAt(0));
                            }));
                        }

                        if ((list_ec_tb_category.Count > 0) && (flag_add == true) && (flag_edit == false))
                        {
                            this.lbCategory.Dispatcher.Invoke((Action)(() =>
                            {
                                lbCategory.SelectedIndex = list_ec_tb_category.Count - 1; ;
                                lbCategory.ScrollIntoView(lbCategory.Items.GetItemAt(list_ec_tb_category.Count - 1));
                            }));
                        }

                        if ((list_ec_tb_category.Count > 0) && (flag_add == false) && (flag_edit == true))
                        {
                            this.lbCategory.Dispatcher.Invoke((Action)(() =>
                            {
                                lbCategory.SelectedIndex = lbcategory_cur_selected;
                                lbCategory.ScrollIntoView(lbCategory.Items.GetItemAt(lbcategory_cur_selected));
                            }));
                        }

                        if (list_ec_tb_category.Count == 0)
                        {
                            list_ec_tb_product.Clear();
                            this.tblNumProduct.Dispatcher.Invoke((Action)(() => { this.tblNumProduct.Text = FindResource("products").ToString() + "(0)"; }));
                            this.dtgProduct.Dispatcher.Invoke((Action)(() => { this.dtgProduct.Items.Refresh(); }));
                        }

                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                }));
                thread_cat.Start();
            }
        }

        //lbCategory_SelectionChanged
        //get data for product data
        private int total_page = 0;
        private int current_page = 1;
        private int page_size = 100;
        private int paging_size = 20;
        private int count_rows = 0;
        private bool flag_stp_added = false;
        private string categoryname_selected = "";
        private string condition_product1 = "";
        private string condition_product2 = "";

        //thread
        private Thread thread_paging = null;
        private Thread thread_paging_next = null;
        private Thread thread_paging_previous = null;
        private Thread thread_calculatetotalpages_inventorystatus = null;
        private int paging_number_previous = 1;
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle.png", UriKind.Absolute));
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging_focus = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_focus.png", UriKind.Absolute));

        int lbcategory_cur_selected;
        private void lbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbCategory.SelectedIndex > -1)
            {
                paging_number_previous = 1;
                current_page = 1;
                lbcategory_cur_selected = lbCategory.SelectedIndex;
                categoryname_selected = list_ec_tb_category[lbcategory_cur_selected].CategoryName;
                condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                condition_product2 = " And [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                LoadDataProduct(condition_product1);
            }
        }

        //LoadDataForOrder
        private void LoadDataProduct(string condition_calculate)
        {
            CalculateTotalPagesForCategory(condition_calculate);
            GetProduct(categoryname_selected, (paging_number_previous - 1) * page_size, page_size, condition_product1, condition_product2);

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
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
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
                if (flag_stp_added)
                    this.spPaging.Dispatcher.Invoke((Action)(() => { spPaging.Children.Remove(stp_paging); }));

                if (total_page > 1)
                {
                    stp_paging = new StackPanel();
                    stp_paging.Orientation = Orientation.Horizontal;

                    //set stp is added
                    flag_stp_added = true;

                    if ((page - 1) > 0)
                    {
                        Image img_previous = new Image();
                        img_previous.Height = 20;
                        img_previous.Width = 20;
                        img_previous.Margin = new Thickness(0, 0, 5, 0);
                        Uri uri_previous = new Uri(@"pack://application:,,,/Resources/circle_previous.png", UriKind.Absolute);
                        System.Windows.Media.Imaging.BitmapImage bitmap_previous = new System.Windows.Media.Imaging.BitmapImage(uri_previous);
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
                        img_next.Height = 20;
                        img_next.Width = 20;
                        img_next.Margin = new Thickness(10, 0, 0, 0);
                        Uri uri_next = new Uri(@"pack://application:,,,/Resources/circle_next.png", UriKind.Absolute);
                        System.Windows.Media.Imaging.BitmapImage bitmap_next = new System.Windows.Media.Imaging.BitmapImage(uri_next);
                        img_next.Source = bitmap_next;
                        img_next.MouseDown += new MouseButtonEventHandler(Img_Next_MouseDown);
                        stp_paging.Children.Add(img_next);
                    }
                }
            }));
        }

        //Grid_Paging_MouseDown
        private int int_current_paging = 1;
        private void Grid_Paging_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid_paging = (Grid)sender;
            string str_current_page = grid_paging.Name.Substring(3);
            int_current_paging = Convert.ToInt32(str_current_page);

            if (paging_number_previous != int_current_paging)
            {
                StackPanel _stp_paging = (StackPanel)spPaging.Children[0];
                for (int i = 0; i < _stp_paging.Children.Count; i++)
                {
                    if (_stp_paging.Children[i].GetType() != typeof(Image))
                    {
                        Grid _grid_paging = (Grid)_stp_paging.Children[i];
                        Image _img_paging = (Image)_grid_paging.Children[0];
                        if (_img_paging.Name == "img" + paging_number_previous)
                            _img_paging.Source = bitmap_image_paging;
                    }
                }
            }
            paging_number_previous = int_current_paging;
            Image img_focus = (Image)grid_paging.Children[0];
            img_focus.Source = bitmap_image_paging_focus;
            GetProduct(categoryname_selected, (int_current_paging - 1) * page_size, page_size, condition_product1, condition_product2);
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
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
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
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_paging_previous.Start();
            }
        }

        //get product
        private DataTable dt_product = new DataTable();
        private void GetProduct(string category_name, int be_limit, int af_limit, string _condition_product1, string _condition_product2)
        {
            if (thread_pro != null && thread_pro.ThreadState == ThreadState.Running) { }
            else
            {
                thread_pro = new Thread(() =>
                {
                    try
                    {
                        list_ec_tb_product.Clear();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.chkCheckAll.IsChecked = false;
                            this.dtgProduct.Visibility = System.Windows.Visibility.Hidden;
                            this.dtgProduct.Items.Refresh();
                            this.mprpro.Visibility = System.Windows.Visibility.Visible;
                            this.mprpro.IsActive = true;
                        }));

                        dt_product = bus_tb_product.GetProductFollowPaging(be_limit, af_limit, _condition_product1, _condition_product2, StaticClass.GeneralClass.flag_database_type_general, _buildStringOderBy());
                        foreach (DataRow dr in dt_product.Rows)
                        {
                            EC_tb_Product ec_tb_product = new EC_tb_Product();

                            ec_tb_product.ProductID = Convert.ToInt32(dr["ProductID"].ToString());
                            ec_tb_product.ShortName = dr["ShortName"].ToString();
                            ec_tb_product.LongName = dr["LongName"].ToString();

                            ec_tb_product.Cost = Convert.ToDecimal(dr["Cost"].ToString());
                            ec_tb_product.StrCost = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_product.Cost);

                            ec_tb_product.Price = Convert.ToDecimal(dr["Price"].ToString());
                            ec_tb_product.StrPrice = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_product.Price);

                            ec_tb_product.InventoryCount = Convert.ToInt32(dr["InventoryCount"].ToString());
                            ec_tb_product.CategoryID = Convert.ToInt32(dr["CategoryID"].ToString());
                            ec_tb_product.Tax = Convert.ToInt32(dr["Tax"].ToString());

                            //check exist image
                            if (System.IO.File.Exists(cur_directory + dr["PathImage"].ToString().ToString()) == true)
                            {
                                ec_tb_product.PathImage = cur_directory + dr["PathImage"].ToString();

                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    BitmapImage image = new BitmapImage();

                                    using (System.IO.FileStream stream = System.IO.File.OpenRead(ec_tb_product.PathImage))
                                    {
                                        image.BeginInit();
                                        image.CacheOption = BitmapCacheOption.OnLoad;
                                        image.StreamSource = stream;
                                        image.EndInit();
                                        ec_tb_product.BitmapImage = image;
                                        stream.Close();
                                        image.Freeze();
                                    }
                                }));
                            }

                            else
                            {
                                ec_tb_product.PathImage = @"pack://application:,,,/Resources/default_01_default_02_default_03_default.png";

                                Uri uri = new Uri(ec_tb_product.PathImage);
                                BitmapImage bitmap_image = new BitmapImage();
                                bitmap_image.BeginInit();
                                bitmap_image.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap_image.UriSource = uri;
                                ec_tb_product.BitmapImage = bitmap_image;
                                bitmap_image.EndInit();
                                bitmap_image.Freeze();
                            }

                            ec_tb_product.Capture = Convert.ToInt32(dr["Capture"].ToString());
                            ec_tb_product.Active = Convert.ToInt32(dr["Active"].ToString());
                            ec_tb_product.BarcodeID = dr["BarcodeID"].ToString();
                            ec_tb_product.Currency = StaticClass.GeneralClass.currency_setting_general;
                            ec_tb_product.Country = dr["Country"].ToString();
                            ec_tb_product.SizeWeight = dr["Size_Weight"].ToString();

                            ec_tb_product.CheckDel = false;
                            ec_tb_product.ImageSource = @"pack://application:,,,/Resources/edit.png";

                            if (ec_tb_product.InventoryCount <= 5)
                                ec_tb_product.Foreground = "IndianRed";

                            if ((ec_tb_product.InventoryCount > 5) && (ec_tb_product.InventoryCount <= 10))
                                ec_tb_product.Foreground = "LightSeaGreen";

                            if (ec_tb_product.InventoryCount > 10)
                                ec_tb_product.Foreground = "DarkGray";

                            list_ec_tb_product.Add(ec_tb_product);
                        }

                        //Comparison<EC_tb_Product> comparision = new Comparison<EC_tb_Product>(EC_tb_Product.CompareProductInventoryCount);
                        //list_ec_tb_product.Sort(comparision);

                        this.dtgProduct.Dispatcher.Invoke((Action)(() =>
                        {
                            this.dtgProduct.Items.Refresh();
                        }));

                        this.tblNumProduct.Dispatcher.Invoke((Action)(() => { this.tblNumProduct.Text = category_name + "(" + list_ec_tb_product.Count + ")"; }));

                        Thread.Sleep(500);
                        this.mprpro.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mprpro.Visibility = System.Windows.Visibility.Hidden;
                            this.mprpro.IsActive = false;
                        }));

                        this.dtgProduct.Dispatcher.Invoke((Action)(() =>
                        {
                            this.dtgProduct.Visibility = System.Windows.Visibility.Visible;
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_pro.Start();
            }
        }

        //expRowDetail_Expanded
        private void expRowDetail_Expanded(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
            }
        }

        //expRowDetail_Collapsed
        private void expRowDetail_Collapsed(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
            }
        }

        //chkCheckDel_Checked
        private void chkCheckDel_Checked(object sender, RoutedEventArgs e)
        {
            if (dtgProduct.SelectedIndex > -1)
            {
                list_ec_tb_product[dtgProduct.SelectedIndex].CheckDel = true;

                StaticClass.GeneralClass.list_ec_tb_product_general.Add(list_ec_tb_product[dtgProduct.SelectedIndex]);
            }
        }

        //chkCheckDel_Unchecked
        private void chkCheckDel_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dtgProduct.SelectedIndex > -1)
            {
                list_ec_tb_product[dtgProduct.SelectedIndex].CheckDel = false;
                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_product_general.Count; i++)
                {
                    if (StaticClass.GeneralClass.list_ec_tb_product_general[i].ProductID == list_ec_tb_product[dtgProduct.SelectedIndex].ProductID)
                        StaticClass.GeneralClass.list_ec_tb_product_general.RemoveAt(i);
                }
            }
        }

        //chkCheckAll_Checked
        private void chkCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            dtgProduct.SelectedIndex = -1;

            if (dtgProduct.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_product.Count; i++)
                {
                    list_ec_tb_product[i].CheckDel = true;
                }

                StaticClass.GeneralClass.list_ec_tb_product_general.AddRange(list_ec_tb_product);
                dtgProduct.Items.Refresh();
            }
        }

        //chkCheckAll_Unchecked
        private void chkCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            dtgProduct.SelectedIndex = -1;

            if (dtgProduct.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_product.Count; i++)
                {
                    list_ec_tb_product[i].CheckDel = false;
                }

                StaticClass.GeneralClass.list_ec_tb_product_general.Clear();
                dtgProduct.Items.Refresh();
            }
        }

        //muiBtnAdd_Click
        private void muiBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!StaticClass.GeneralClass.isFullVersion)
            {
                if (StaticClass.GeneralClass.youremail_registered_general == "")
                {
                    if (bus_tb_product.GetTotalProduct("") >= 10)
                    {
                        ModernDialog md = new ModernDialog();
                        md.Title = FindResource("notification").ToString();
                        md.Content = "This Cash Register application only support up to 10 products. \nPlease upgrade to Cash Register Pro. Cash Register Pro has a more powerful database engine \nand supports unlimited products.";
                        md.Buttons = new[] { md.OkButton, md.CloseButton };
                        md.CloseButton.Content = FindResource("close").ToString();
                        md.OkButton.Content = FindResource("purchase_now").ToString();
                        md.OkButton.Click += new RoutedEventHandler(btnPurchaseRe_Click);
                        md.ShowDialog();
                        return;
                    }
                }
            }
            
            StaticClass.GeneralClass.categoryid_general = list_ec_tb_category[lbCategory.SelectedIndex].CategoryID;
            AddProduct page = new AddProduct();
            page.category_name = list_ec_tb_category[lbcategory_cur_selected].CategoryName;
            page.muibtnadd_delegate += muiBtnAdd_Click_Delegate;
            page.ShowDialog();
        }

        //btnPurchase_Click
        private void btnPurchaseRe_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).mainWindow.ContentSource = new Uri(@"/Pages/SettingsPage.xaml", UriKind.Relative);
            StaticClass.GeneralClass.register_request = true;
            System.Diagnostics.Process.Start("http://ipcamsoft.com/?download&purchasecashregister");
        }

        //muiBtnAdd_Click_Delegate
        private void muiBtnAdd_Click_Delegate(bool flag_added)
        {
            if (flag_added == true)
            {
                current_page = 1;
                paging_number_previous = 1;
                condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                condition_product2 = " And [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                LoadDataProduct(condition_product1);
            }
        }

        //muiBtnDelete_Click
        private void muiBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            paging_number_previous = 1;
            current_page = 1;
            DeleteProduct page = new DeleteProduct();
            page.muibtndelete_delegate += muiBtnDelete_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnDelete_Click_Delegate
        private void muiBtnDelete_Click_Delegate(bool flag_deleted)
        {
            if (flag_deleted == true)
            {
                current_page = 1;
                paging_number_previous = 1;
                condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                condition_product2 = " And [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                LoadDataProduct(condition_product1);
            }
        }

        //muiBtnSearch_Click
        private void muiBtnSearch_Click(object sender, RoutedEventArgs e)
        {
            current_page = 1;
            paging_number_previous = 1;
            condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
            condition_product2 = " And [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
            LoadDataProduct(condition_product1);
        }

        //btnEdit_Click
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dtgProduct.SelectedIndex > -1)
            {
                EditProduct page = new EditProduct();
                int index_selected = dtgProduct.SelectedIndex;
                StaticClass.GeneralClass.productid_general = list_ec_tb_product[index_selected].ProductID;
                StaticClass.GeneralClass.productpathimage_general = list_ec_tb_product[index_selected].PathImage;
                StaticClass.GeneralClass.categoryid_general = list_ec_tb_product[index_selected].CategoryID;


                if (list_ec_tb_product[index_selected].PathImage == @"pack://application:,,,/Resources/default_01_default_02_default_03_default.png")
                {
                    BitmapImage bitmap_image = new BitmapImage(new Uri(list_ec_tb_product[index_selected].PathImage));
                    page.imgProduct.Source = bitmap_image;
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        BitmapImage image = new BitmapImage();

                        using (System.IO.FileStream stream = System.IO.File.OpenRead(list_ec_tb_product[index_selected].PathImage))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                            page.imgProduct.Source = image;
                            stream.Close();
                            image.Freeze();
                        }
                    }));
                }

                page.txbBarcode.Text = list_ec_tb_product[index_selected].BarcodeID;
                page.txbShortName.Text = list_ec_tb_product[index_selected].ShortName;
                page.txbLongName.Text = list_ec_tb_product[index_selected].LongName;

                page.txbCost.Text = StaticClass.GeneralClass.GetNumFormatEdit(list_ec_tb_product[index_selected].Cost);
                page.txbPrice.Text = StaticClass.GeneralClass.GetNumFormatEdit(list_ec_tb_product[index_selected].Price);

                page.txbInventoryCount.Text = list_ec_tb_product[index_selected].InventoryCount.ToString();
                page.category_name = list_ec_tb_category[lbcategory_cur_selected].CategoryName;
                page.txbCountry.Text = list_ec_tb_product[index_selected].Country;
                page.txbSize_Weight.Text = list_ec_tb_product[index_selected].SizeWeight;

                if (list_ec_tb_product[index_selected].Tax == 1)
                    page.chkTax.IsChecked = true;
                else
                    page.chkTax.IsChecked = false;

                if (list_ec_tb_product[index_selected].Active == 1)
                    page.chkActive.IsChecked = true;
                else
                    page.chkActive.IsChecked = false;

                page.btnedit_delegate += btnEdit_Click_Delegate;
                page.ShowDialog();
            }
        }

        //dtgProduct_MouseDoubleClick
        private void dtgProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgProduct.SelectedIndex > -1)
            {
                EditProduct page = new EditProduct();
                int index_selected = dtgProduct.SelectedIndex;
                StaticClass.GeneralClass.productid_general = list_ec_tb_product[index_selected].ProductID;
                StaticClass.GeneralClass.productpathimage_general = list_ec_tb_product[index_selected].PathImage;
                StaticClass.GeneralClass.categoryid_general = list_ec_tb_product[index_selected].CategoryID;

                if (list_ec_tb_product[index_selected].PathImage == @"pack://application:,,,/Resources/default_01_default_02_default_03_default.png")
                {
                    BitmapImage bitmap_image = new BitmapImage(new Uri(list_ec_tb_product[index_selected].PathImage));
                    page.imgProduct.Source = bitmap_image;
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        BitmapImage image = new BitmapImage();

                        using (System.IO.FileStream stream = System.IO.File.OpenRead(list_ec_tb_product[index_selected].PathImage))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                            page.imgProduct.Source = image;
                            stream.Close();
                            image.Freeze();
                        }
                    }));
                }

                page.txbBarcode.Text = list_ec_tb_product[index_selected].BarcodeID;
                page.txbShortName.Text = list_ec_tb_product[index_selected].ShortName;
                page.txbLongName.Text = list_ec_tb_product[index_selected].LongName;

                page.txbCost.Text = StaticClass.GeneralClass.GetNumFormatEdit(list_ec_tb_product[index_selected].Cost);
                page.txbPrice.Text = StaticClass.GeneralClass.GetNumFormatEdit(list_ec_tb_product[index_selected].Price);

                page.txbInventoryCount.Text = list_ec_tb_product[index_selected].InventoryCount.ToString();
                page.category_name = list_ec_tb_category[lbcategory_cur_selected].CategoryName;
                page.txbCountry.Text = list_ec_tb_product[index_selected].Country;
                page.txbSize_Weight.Text = list_ec_tb_product[index_selected].SizeWeight;

                if (list_ec_tb_product[index_selected].Tax == 1)
                    page.chkTax.IsChecked = true;
                else
                    page.chkTax.IsChecked = false;

                if (list_ec_tb_product[index_selected].Active == 1)
                    page.chkActive.IsChecked = true;
                else
                    page.chkActive.IsChecked = false;

                page.btnedit_delegate += btnEdit_Click_Delegate;
                page.ShowDialog();
            }
        }

        //btnEdit_Click_Delegate
        private void btnEdit_Click_Delegate(bool flag_edit)
        {
            if (flag_edit == true)
            {
                condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                condition_product2 = " And [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                LoadDataProduct(condition_product1);
            }
        }

        //UserControl_SizeChanged
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            lbCategory.MaxHeight = this.RenderSize.Height - 75;
        }

        //btnAdd_Click
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCategory page = new AddCategory();
            page.addcategory_delegate += btnAdd_Click_Delegate;
            page.ShowDialog();
        }

        //btnAdd_Click_Delegate
        private void btnAdd_Click_Delegate()
        {
            GetCategory(true, false);
        }

        //imgEdit_MouseDown
        private void imgEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            int __index = -1;
            if (int.TryParse(img.Uid.ToString(), out __index) == true)
            {
                lbcategory_cur_selected = __index;
                StaticClass.GeneralClass.categoryid_general = list_ec_tb_category[__index].CategoryID;
                StaticClass.GeneralClass.categoryname_general = list_ec_tb_category[__index].CategoryName;

                EditCategory page = new EditCategory();
                page.lbcategory_edit_delegate += lbCategory_EditDelete_Delegate;
                page.ShowDialog();
            }
        }

        //imdDelete_MouseDown
        private void imdDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            int __index = -1;
            if (int.TryParse(img.Uid.ToString(), out __index) == true)
            {
                lbCategory.SelectionChanged -= new SelectionChangedEventHandler(lbCategory_SelectionChanged);
                lbcategory_cur_selected = __index;
                StaticClass.GeneralClass.categoryid_general = list_ec_tb_category[__index].CategoryID;
                StaticClass.GeneralClass.categoryname_general = list_ec_tb_category[__index].CategoryName;
                DeleteCategory page = new DeleteCategory();
                page.lbcategory_delete_delegate += lbCategory_EditDelete_Delegate;
                page.ShowDialog();
            }
        }

        //lbCategory_MouseRightButtonUp_Delegate
        private void lbCategory_EditDelete_Delegate(bool edit_flag)
        {
            if (edit_flag == true)
            {
                lbCategory.SelectionChanged -= new SelectionChangedEventHandler(lbCategory_SelectionChanged);
                GetCategory(false, true);
            }
            else
            {
                lbCategory.SelectionChanged -= new SelectionChangedEventHandler(lbCategory_SelectionChanged);
                GetCategory(false, false);
            }
        }

        //txbSearch_KeyDown
        private void txbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                paging_number_previous = 1;
                current_page = 1;
                condition_product1 = " Where [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                condition_product2 = " And [CategoryID] = " + list_ec_tb_category[lbcategory_cur_selected].CategoryID + " And ([ShortName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [LongName] like '%" + txbSearch.Text.Trim().ToString() + "%' OR [BarcodeID] like '%" + txbSearch.Text.Trim().ToString() + "%')";
                LoadDataProduct(condition_product1);
            }
        }

        //btnHistory_Click
        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (dtgProduct.SelectedIndex > -1)
            {
                Pages.Setting.InputHistory page = new Pages.Setting.InputHistory();
                page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                page.categoryid = 0;
                page.productid = list_ec_tb_product[dtgProduct.SelectedIndex].ProductID;
                var m = Application.Current.MainWindow;
                page.ShowInTaskbar = false;
                page.Owner = m;

                if (m.RenderSize.Width > StaticClass.GeneralClass.width_screen_general)
                    page.Width = StaticClass.GeneralClass.width_screen_general * 90 / 100;
                else
                    page.Width = m.RenderSize.Width * 90 / 100;

                if (m.RenderSize.Height > StaticClass.GeneralClass.height_screen_working_general)
                    page.Height = StaticClass.GeneralClass.height_screen_working_general;
                else
                    page.Height = m.RenderSize.Height;

                page.ShowDialog();
            }
        }

        //hplHistory_Click
        private void hplHistory_Click(object sender, RoutedEventArgs e)
        {
            if (lbCategory.SelectedIndex > -1)
            {
                Pages.Setting.InputHistory page = new Pages.Setting.InputHistory();
                page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                page.categoryid = list_ec_tb_category[lbcategory_cur_selected].CategoryID;
                page.product_name = txbSearch.Text.Trim().ToString();
                page.productid = 0;

                var m = Application.Current.MainWindow;
                page.ShowInTaskbar = false;
                page.Owner = m;

                if (m.RenderSize.Width > StaticClass.GeneralClass.width_screen_general)
                    page.Width = StaticClass.GeneralClass.width_screen_general * 90 / 100;
                else
                    page.Width = m.RenderSize.Width * 90 / 100;

                if (m.RenderSize.Height > StaticClass.GeneralClass.height_screen_working_general)
                    page.Height = StaticClass.GeneralClass.height_screen_working_general;
                else
                    page.Height = m.RenderSize.Height;

                page.ShowDialog();
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            _dlg = new Microsoft.Win32.SaveFileDialog();
            _dlg.FileName = string.Format("tb_Category_{0:MMMddyyyy_HHmmssfff}", DateTime.Now); // Default file name
            _dlg.DefaultExt = ".csv"; // Default file extension
            _dlg.Filter = "Text documents (.csv)|*.csv"; // Filter files by extension
            Nullable<bool> result = _dlg.ShowDialog();
            if (result == true)
            {
                _myTimer_ = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
                _Waiting = new Waiting();
                _Waiting.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                _Waiting.ShowInTaskbar = false;
                _threadWaiting = new Thread(() =>
                {
                    _myTimer_.Tick += _eventTimerExport;
                    _myTimer_.Start();
                });
                _threadWaiting.SetApartmentState(ApartmentState.STA);
                _threadWaiting.Start();
                _threadWaiting.Join();
                _Waiting.ShowDialog();
            }
        }
        private string _getListColumnName(string _tblName)
        {
            string _rs = string.Empty;
            if (string.IsNullOrEmpty(_tblName)) return _rs;
            DataTable _cols = Model.Setting.TableModel.getColumnsByTable(_tblName);
            if (_cols.Rows.Count > 0)
            {
                foreach (DataRow _dr in _cols.Rows)
                {
                    _rs += "[" + _dr["name"].ToString() + "], ";
                }
            }
            if (!string.IsNullOrEmpty(_rs))
                _rs = _rs.Remove(_rs.Length - 2, 2);
            return _rs;
        }
        private void _eventTimerExport(object sender, EventArgs e)
        {
            if (_exportIsRuning)
            {
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                _exportIsRuning = false;
                _exportStatus = false;
            }
            else
            {
                if (!_exportStatus)
                {
                    _exportStatus = true;
                    _runExportCategory();
                }
            }
        }
        private async void _runExportCategory()
        {
            var slowTask = Task<string>.Factory.StartNew(() => this._startExport());
            await slowTask;
            _exportIsRuning = true;
            if (slowTask.Result.ToString() == "Success")
            {
                _Waiting.Close();
                ModernDialog md = new ModernDialog();
                md.CloseButton.FindResource("close").ToString();
                md.Content = "Export successfully to " + _dlg.FileName;
                md.Title = App.Current.FindResource("notification").ToString();
                md.ShowDialog();
            }
            else
            {
                _Waiting.Close();
                ModernDialog md = new ModernDialog();
                md.CloseButton.FindResource("close").ToString();
                md.Content = slowTask.Result.ToString();
                md.Title = App.Current.FindResource("notification").ToString();
                md.ShowDialog();
            }
        }
        private string _startExport()
        {
            string _info = string.Empty;
            try
            {
                using (System.IO.StreamWriter stream_writer = new System.IO.StreamWriter(_dlg.FileName, false, Encoding.UTF8))
                {
                    string _lstColumn = this._getListColumnName("tb_Category").Replace("[", "");
                    _lstColumn = _lstColumn.Replace("]", "");
                    _lstColumn = _lstColumn.Replace(", ", ",");
                    stream_writer.WriteLine(_lstColumn.Substring(11) + ",CategoryDiscount");
                    DataTable _dt = Model.Setting.TableModel.getAllDataTable("tb_Category");
                    if (_dt.Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dt.Rows)
                        {
                            string _rs = string.Empty;
                            DataTable _dt_ = Model.Setting.TableModel.getColumnsByTable("tb_Category");
                            foreach (DataRow _dr_ in _dt_.Rows)
                            {
                                if (_dr_["name"].ToString() != "CategoryID")
                                {
                                    string _strComp = _dr_["type"].ToString().ToLower();
                                    if (_strComp.Contains("text") || _strComp.Contains("ntext") || _strComp.Contains("varchar") || _strComp.Contains("nvarchar") || _strComp.Contains("char") || _strComp.Contains("nchar") || _strComp.Contains("xml"))
                                    {
                                        _rs += "\"" + @_dr[_dr_["name"].ToString()].ToString().Replace("\"", @"\u0022") + "\",0.00,";
                                    }
                                    else _rs += _dr[_dr_["name"].ToString()].ToString() + ",0.00,";
                                }
                            }
                            _rs = _rs.Remove(_rs.Length - 1, 1);
                            stream_writer.WriteLine(_rs);
                        }
                    }
                    stream_writer.Close();
                }
                _info = "Success";
            }
            catch (Exception e)
            {
                _info = e.Message;
            }
            return _info;
        }

        private void muiBtnImport_Click(object sender, RoutedEventArgs e)
        {
            Views.Setting.ImportProduct page = new Views.Setting.ImportProduct();
            page.ShowDialog();
        }

        private void muiBtnExport_Click(object sender, RoutedEventArgs e)
        {
            _dlg = new Microsoft.Win32.SaveFileDialog();
            _dlg.FileName = string.Format("tb_Product_{0:MMMddyyyy_HHmmssfff}", DateTime.Now); // Default file name
            _dlg.DefaultExt = ".csv"; // Default file extension
            _dlg.Filter = "Text documents (.csv)|*.csv"; // Filter files by extension
            Nullable<bool> result = _dlg.ShowDialog();
            if (result == true)
            {
                _myTimer_ = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
                _Waiting = new Waiting();
                _Waiting.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                _Waiting.ShowInTaskbar = false;
                _threadWaiting = new Thread(() =>
                {
                    _myTimer_.Tick += _eventTimerExportProduct;
                    _myTimer_.Start();
                });
                _threadWaiting.SetApartmentState(ApartmentState.STA);
                _threadWaiting.Start();
                _threadWaiting.Join();
                _Waiting.ShowDialog();
            }
        }
        private void _eventTimerExportProduct(object sender, EventArgs e)
        {
            if (_exportIsRuning)
            {
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                _exportIsRuning = false;
                _exportStatus = false;
            }
            else
            {
                if (!_exportStatus)
                {
                    _exportStatus = true;
                    _runExportProduct();
                }
            }
        }
        private async void _runExportProduct()
        {
            var slowTask = Task<string>.Factory.StartNew(() => this._startExportProduct());
            await slowTask;
            _exportIsRuning = true;
            if (slowTask.Result.ToString() == "Success")
            {
                _Waiting.Close();
                ModernDialog md = new ModernDialog();
                md.CloseButton.FindResource("close").ToString();
                md.Content = "Export successfully to " + _dlg.FileName;
                md.Title = App.Current.FindResource("notification").ToString();
                md.ShowDialog();
            }
            else
            {
                _Waiting.Close();
                ModernDialog md = new ModernDialog();
                md.CloseButton.FindResource("close").ToString();
                md.Content = slowTask.Result.ToString();
                md.Title = App.Current.FindResource("notification").ToString();
                md.ShowDialog();
            }
        }
        private string _startExportProduct()
        {
            string _info = string.Empty;
            try
            {
                using (System.IO.StreamWriter stream_writer = new System.IO.StreamWriter(_dlg.FileName, false, Encoding.UTF8))
                {
                    stream_writer.WriteLine("Category,ShortName,LongName,Cost,Price,Taxable,Qty,Barcode");
                    DataTable _dt = Model.Setting.TableModel.getDataProductToExport();
                    if (_dt.Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dt.Rows)
                        {
                            string _rs = string.Empty;
                            string _cat = "\"" + _dr["CategoryName"].ToString().Replace("\"", @"\u0022") + "\"";
                            string _short = "\"" + _dr["ShortName"].ToString().Replace("\"", @"\u0022") + "\"";
                            string _long = "\"" + _dr["LongName"].ToString().Replace("\"", @"\u0022") + "\"";
                            string _cost = _dr["Cost"].ToString();
                            string _price = _dr["Price"].ToString();
                            string _tax = _dr["Tax"].ToString();
                            string _qty = _dr["InventoryCount"].ToString();
                            string _barcode = (string.IsNullOrEmpty(_dr["BarcodeID"].ToString())) ? "\"\"" : "\"" + _dr["BarcodeID"].ToString() + "\"";
                            _rs = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", _cat, _short, _short, _cost, _price, _tax, _qty, _barcode);
                            stream_writer.WriteLine(_rs);
                        }
                    }
                    stream_writer.Close();
                }
                _info = "Success";
            }
            catch (Exception e)
            {
                _info = e.Message;
            }
            return _info;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            Views.Setting.ImportCategory page = new Views.Setting.ImportCategory();
            page.ShowDialog();
        }
        private directSort _homeDirectSort = directSort.ASC;
        public directSort HomeDirectSort
        {
            get { return _homeDirectSort; }
            set { _homeDirectSort = value; }
        }
        private prodSort _homeProdSort = prodSort.ShortName;
        public prodSort HomeProdSort
        {
            get { return _homeProdSort; }
            set { _homeProdSort = value; }
        }
        private string _buildStringOderBy()
        {
            string _strOderBy = string.Empty;
            string _strTemp = string.Empty;
            string _subOrder = "lower(ShortName) asc, lower(LongName) asc";
            if (HomeProdSort == prodSort.ShortName)
            {
                _strTemp += "lower(ShortName)";
                _subOrder = "lower(LongName) asc";
            }
            else if (HomeProdSort == prodSort.LongName)
            {
                _strTemp += "lower(LongName)";
                _subOrder = "lower(ShortName) asc";
            }
            else if (HomeProdSort == prodSort.Price)
                _strTemp += "Price";
            else if (HomeProdSort == prodSort.DateInserted)
                _strTemp += "ProductID";
            else if (HomeProdSort == prodSort.Seller)
                _strTemp += "Total";
            else if (HomeProdSort == prodSort.Cost)
                _strTemp += "Cost";
            else if (HomeProdSort == prodSort.Country)
                _strTemp += "lower(Country)";
            else if (HomeProdSort == prodSort.InventoryCount)
                _strTemp += "InventoryCount";
            else if (HomeProdSort == prodSort.SizeWeight)
                _strTemp += "lower(Size_Weight)";
            if (!string.IsNullOrEmpty(_strTemp))
            {
                _strOderBy = string.Format("order by {0} {1}, {2}", _strTemp, HomeDirectSort.ToString(), _subOrder);
            }
            return _strOderBy;
        }

        private void cb_SortBy_prod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox cbb = (ComboBox)sender;
                HomeProdSort = (prodSort)Enum.Parse(typeof(prodSort), cbb.SelectedValue.ToString());
                LoadDataProduct(condition_product1);
            }
            catch { }
        }
    }
    public enum prodSort
    {
        ShortName, LongName, Cost, Price, InventoryCount, DateInserted, Country, SizeWeight, Seller
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
