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
using System.Threading;
using System.Data;
using CashierRegisterEntity;
using CashierRegisterBUS;
using FirstFloor.ModernUI.Windows.Controls;

namespace CashierRegister.Pages.Report
{
    /// <summary>
    /// Interaction logic for Salesperson.xaml
    /// </summary>
    public partial class Salesperson : UserControl
    {
        //using for order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();
        private List<EC_tb_Order> list_ec_tb_order = new List<EC_tb_Order>();

        //using for salesperson
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();
        private List<EC_tb_SalesPerson> list_ec_tb_salesperson = new List<EC_tb_SalesPerson>();
        private bool flag_check_loaded = false;

        //using for thread
        private Thread thread_salesperson = null;

        private string _orderTime = string.Empty;
        private string _orderDate = string.Empty;

        //Salesperson
        public Salesperson()
        {
            InitializeComponent();
            lbSalesperson.ItemsSource = list_ec_tb_salesperson;
            dtgOrders.ItemsSource = list_ec_tb_order;
            _orderTime = (!string.IsNullOrEmpty(StaticClass.GeneralClass.app_settings["timeFormat"].ToString())) ? " " + StaticClass.GeneralClass.timeFromatSettings[StaticClass.GeneralClass.app_settings["timeFormat"]].ToString() : "";
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                GetSalesperson();
            }
        }

        //GetSalesperson
        DataTable dt_tb_salesperson = new DataTable();
        private void GetSalesperson()
        {
            if (thread_salesperson != null && thread_salesperson.ThreadState == ThreadState.Running) { }
            else
            {
                thread_salesperson = new Thread(() =>
                {
                    try
                    {
                        list_ec_tb_salesperson.Clear();
                        this.Dispatcher.Invoke((Action)(() => { lbSalesperson.Items.Refresh(); }));

                        dt_tb_salesperson.Clear();
                        dt_tb_salesperson = bus_tb_salesperson.GetSalesPerson("");

                        foreach (DataRow datarow in dt_tb_salesperson.Rows)
                        {
                            EC_tb_SalesPerson ec_tb_salesperson = new EC_tb_SalesPerson();
                            ec_tb_salesperson.SalespersonID = Convert.ToInt32(datarow["SalespersonID"].ToString());
                            ec_tb_salesperson.Name = datarow["Name"].ToString();
                            ec_tb_salesperson.Birthday = datarow["Birthday"].ToString();
                            ec_tb_salesperson.Address = datarow["Address"].ToString();
                            ec_tb_salesperson.Email = datarow["Email"].ToString();
                            ec_tb_salesperson.Password = datarow["Password"].ToString();
                            ec_tb_salesperson.Active = Convert.ToInt32(datarow["Active"].ToString());

                            list_ec_tb_salesperson.Add(ec_tb_salesperson);
                        }

                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            tblSalespersonTotal.Text = list_ec_tb_salesperson.Count.ToString();
                            lbSalesperson.Items.Refresh(); ;
                        }));

                        if (list_ec_tb_salesperson.Count > 0)
                            this.Dispatcher.Invoke((Action)(() => { this.lbSalesperson.SelectedIndex = 0; }));
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
                thread_salesperson.Start();
            }
        }

        //lbSalesperson_SelectionChanged
        //get data for order data
        private int total_page_order = 0;
        private int current_page_order = 1;
        private int page_size_order = 100;
        private int paging_size_order = 20;
        private int count_rows_order = 0;
        private bool flag_stp_added = false;
        private int salespersonid_select = -1;
        private string condition_order1 = "";
        private string condition_order2 = "";
        private int paging_number_previous = 1;
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle.png", UriKind.Absolute));
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging_focus = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_focus.png", UriKind.Absolute));

        //thread
        private Thread thread_paging_order = null;
        private Thread thread_paging_order_next = null;
        private Thread thread_paging_order_previous = null;
        private Thread thread_calculatetotalpages_order = null;
        private Thread thread_order = null;

        private void lbSalesperson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSalesperson.SelectedIndex > -1)
            {
                paging_number_previous = 1;
                current_page_order = 1;
                salespersonid_select = list_ec_tb_salesperson[lbSalesperson.SelectedIndex].SalespersonID;
                condition_order1 = " where [SalespersonID] = " + salespersonid_select;
                condition_order2 = " and [SalespersonID] = " + salespersonid_select;
                LoadDataForOrder(salespersonid_select);
            }
        }

        //LoadDataForOrder
        private void LoadDataForOrder(int _salespersonid_select)
        {
            CalculateTotalPagesForOrder(salespersonid_select);
            GetOrders((paging_number_previous - 1) * page_size_order, page_size_order, condition_order1, condition_order2);

            if (thread_paging_order != null && thread_paging_order.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging_order = new Thread(() =>
                {
                    try
                    {
                        if (current_page_order * paging_size_order * page_size_order > count_rows_order)
                        {
                            int _paging_size = (count_rows_order - ((current_page_order - 1) * paging_size_order * page_size_order)) / page_size_order;

                            if ((count_rows_order - ((current_page_order - 1) * paging_size_order * page_size_order)) % page_size_order > 0)
                                _paging_size += 1;

                            PagingPageForOrder(current_page_order, _paging_size, total_page_order);
                        }
                        else
                            PagingPageForOrder(current_page_order, paging_size_order, total_page_order);
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
                thread_paging_order.Start();
            }
        }

        //CalculateTotalPages
        private void CalculateTotalPagesForOrder(int salespersonid)
        {
            if (thread_calculatetotalpages_order != null && thread_calculatetotalpages_order.ThreadState == ThreadState.Running) { }
            else
            {
                thread_calculatetotalpages_order = new Thread(() =>
                {
                    current_page_order = 1;
                    string con = "where [SalespersonID] = " + salespersonid;
                    count_rows_order = bus_tb_order.GetSumOrder(con);
                    this.total_page_order = count_rows_order / page_size_order;
                    if (count_rows_order % page_size_order > 0)
                        this.total_page_order += 1;
                });
                thread_calculatetotalpages_order.Start();
                thread_calculatetotalpages_order.Join();
            }
        }

        //PagingPage
        private StackPanel stp;
        private void PagingPageForOrder(int page, int _paging_size, int total_page)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (flag_stp_added == true)
                    this.spPaging.Dispatcher.Invoke((Action)(() => { spPaging.Children.Remove(stp); }));

                if (total_page_order > 1)
                {
                    stp = new StackPanel();
                    stp.Orientation = Orientation.Horizontal;

                    //set stp is added
                    flag_stp_added = true;

                    if ((page - 1) > 0)
                    {
                        Image img_previous = new Image();
                        img_previous.Height = 20;
                        img_previous.Width = 20;
                        img_previous.Margin = new Thickness(0, 0, 5, 0);
                        System.Windows.Media.Imaging.BitmapImage bitmap_previous = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_previous.png", UriKind.Absolute));
                        img_previous.Source = bitmap_previous;
                        img_previous.MouseDown += new MouseButtonEventHandler(Img_Previous_MouseDown);
                        stp.Children.Add(img_previous);
                    }

                    for (int i = ((page - 1) * paging_size_order); i < (((page - 1) * paging_size_order) + _paging_size); i++)
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
                        stp.Children.Add(gr_paging);

                    }
                    //GC.Collect();

                    spPaging.Children.Add(stp);

                    if ((page < total_page) && (paging_size_order == _paging_size))
                    {
                        Image img_next = new Image();
                        img_next.Height = 20;
                        img_next.Width = 20;
                        img_next.Margin = new Thickness(10, 0, 0, 0);
                        System.Windows.Media.Imaging.BitmapImage bitmap_next = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_next.png", UriKind.Absolute));
                        img_next.Source = bitmap_next;
                        img_next.MouseDown += new MouseButtonEventHandler(Img_Next_MouseDown);
                        stp.Children.Add(img_next);
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
            GetOrders((int_current_paging - 1) * page_size_order, page_size_order, condition_order1, condition_order2);
        }

        //Img_Next_MouseDown
        private void Img_Next_MouseDown(object sender, MouseButtonEventArgs e)
        {
            current_page_order++;

            if (thread_paging_order_next != null && thread_paging_order_next.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging_order_next = new Thread(() =>
                {
                    try
                    {
                        if (current_page_order * paging_size_order * page_size_order > count_rows_order)
                        {
                            int _paging_size = (count_rows_order - ((current_page_order - 1) * paging_size_order * page_size_order)) / page_size_order;

                            if ((count_rows_order - ((current_page_order - 1) * paging_size_order * page_size_order)) % page_size_order > 0)
                                _paging_size += 1;

                            PagingPageForOrder(current_page_order, _paging_size, total_page_order);
                        }
                        else
                            PagingPageForOrder(current_page_order, paging_size_order, total_page_order);
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
                thread_paging_order_next.Start();
            }
        }

        //Imge_Previous_MouseDown
        private void Img_Previous_MouseDown(object sender, MouseButtonEventArgs e)
        {
            current_page_order--;
            if (thread_paging_order_previous != null && thread_paging_order_previous.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging_order_previous = new Thread(() =>
                {
                    try
                    {
                        PagingPageForOrder(current_page_order, paging_size_order, total_page_order);
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
                thread_paging_order_previous.Start();
            }
        }

        //get orders
        private DataTable dt_orders = new DataTable();
        private void GetOrders(int be_limit, int af_limit, string _con1, string _con2)
        {
            if (thread_order != null && thread_order.ThreadState == ThreadState.Running) { }
            else
            {
                thread_order = new Thread(() =>
                {
                    try
                    {
                        list_ec_tb_order.Clear();

                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            this.chkAll.IsChecked = false;
                            this.mpr.Visibility = System.Windows.Visibility.Visible;
                            this.mpr.IsActive = true;
                            this.dtgOrders.Visibility = System.Windows.Visibility.Hidden;
                            dtgOrders.Items.Refresh();
                        }));

                        dt_orders.Clear();
                        int no = paging_number_previous - 1;

                        using (dt_orders = bus_tb_order.GetOrderFollowPaging(be_limit, af_limit, _con1, _con2, StaticClass.GeneralClass.flag_database_type_general))
                        {
                            foreach (DataRow dr in dt_orders.Rows)
                            {
                                _orderDate = string.Format(@"{0:" + StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString() + _orderTime + "}", Convert.ToDateTime(dr["OrderDate"].ToString()));
                                no++;
                                EC_tb_Order ec_tb_order = new EC_tb_Order();
                                ec_tb_order.No = no;
                                ec_tb_order.OrderID = Convert.ToInt32(dr["OrderID"].ToString());
                                ec_tb_order.CustomerID = Convert.ToInt32(dr["CustomerID"].ToString());
                                ec_tb_order.CustomerName = dr["CustomerName"].ToString();
                                ec_tb_order.Quatity = Convert.ToInt32(dr["Quantity"].ToString());
                                ec_tb_order.OrderDate = _orderDate; //dr["OrderDate"].ToString();
                                ec_tb_order.SalesPersonID = Convert.ToInt32(dr["SalespersonID"].ToString());
                                ec_tb_order.SalesPersonName = dr["SalespersonName"].ToString();
                                ec_tb_order.PaymentID = Convert.ToInt32(dr["PaymentID"].ToString());
                                ec_tb_order.PaymentName = dr["PaymentName"].ToString();
                                ec_tb_order.DiscountType = Convert.ToInt32(dr["DiscountType"].ToString());
                                if (ec_tb_order.DiscountType == 1)
                                {
                                    ec_tb_order.DisAmount = "";
                                    ec_tb_order.DisPercent = "%";
                                }
                                else
                                {
                                    ec_tb_order.DisAmount = StaticClass.GeneralClass.currency_setting_general;
                                    ec_tb_order.DisPercent = "";
                                }
                                ec_tb_order.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                                ec_tb_order.StrDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_order.Discount);

                                ec_tb_order.TotalDiscount = Convert.ToDecimal(string.IsNullOrEmpty(dr["TotalDiscount"].ToString()) ? "0" : dr["TotalDiscount"].ToString());
                                ec_tb_order.StrTotalDiscount = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_order.TotalDiscount);

                                ec_tb_order.TotalTax = Convert.ToDecimal(dr["TotalTax"].ToString());
                                ec_tb_order.StrTotalTax = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_order.TotalTax);

                                ec_tb_order.TotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString());
                                ec_tb_order.StrTotalAmount = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_order.TotalAmount);

                                ec_tb_order.ImageSource = @"pack://application:,,,/Resources/ViewDetails.png";
                                ec_tb_order.CheckDel = false;
                                ec_tb_order.Currency = StaticClass.GeneralClass.currency_setting_general;

                                list_ec_tb_order.Add(ec_tb_order);
                            }
                        }

                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            tblTotal.Text = FindResource("total").ToString() + "(" + list_ec_tb_order.Count.ToString() + ")";
                            dtgOrders.Items.Refresh();
                        }));

                        Thread.Sleep(500);

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Hidden;
                            this.mpr.IsActive = false;
                            this.dtgOrders.Visibility = System.Windows.Visibility.Visible;
                        }));

                        //data statistic
                        DataStatistic();

                        //set condition report order
                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                            StaticClass.GeneralClass.condition_report_order = "select * from [tb_Order] " + _con1 + " limit " + be_limit + ", " + af_limit;
                        else
                            StaticClass.GeneralClass.condition_report_order = "select top(" + af_limit + ") * from [tb_Order] where [OrderID] not in (select top(" + be_limit + ") [OrderID] from [tb_Order] " + _con1 + ") " + _con2;
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
                thread_order.Start();
            }
        }

        //DataStatistic
        private void DataStatistic()
        {
            int quantity_total = 0;
            decimal tax_total = 0;
            decimal discount_total = 0;
            decimal total_total = 0;

            if (list_ec_tb_order.Count > 0)
            {
                foreach (EC_tb_Order ec_tb_order in list_ec_tb_order)
                {
                    quantity_total += ec_tb_order.Quatity;
                    tax_total += ec_tb_order.TotalTax;
                    discount_total += ec_tb_order.TotalDiscount;
                    total_total += ec_tb_order.TotalAmount;
                }
            }
            this.tblQuantity.Dispatcher.Invoke((Action)(() => { tblQuantity.Text = quantity_total.ToString(); }));
            this.tblTax.Dispatcher.Invoke((Action)(() => { tblTax.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(tax_total); }));
            this.tblDiscount.Dispatcher.Invoke((Action)(() => { tblDiscount.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(discount_total); }));
            this.tblDiscount.Dispatcher.Invoke((Action)(() => { _tblTotal.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(total_total); }));
        }


        //chkAll_Checked
        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            dtgOrders.SelectedIndex = -1;

            if (dtgOrders.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_order.Count; i++)
                {
                    list_ec_tb_order[i].CheckDel = true;
                }

                StaticClass.GeneralClass.list_ec_tb_order_general.AddRange(list_ec_tb_order);
                dtgOrders.Items.Refresh();
            }
        }

        //chkAll_Unchecked
        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            dtgOrders.SelectedIndex = -1;

            if (dtgOrders.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_order.Count; i++)
                {
                    list_ec_tb_order[i].CheckDel = false;
                }

                StaticClass.GeneralClass.list_ec_tb_order_general.Clear();
                dtgOrders.Items.Refresh();
            }
        }

        //chkDel_Checked
        private void chkDel_Checked(object sender, RoutedEventArgs e)
        {
            if (dtgOrders.SelectedIndex > -1)
            {
                list_ec_tb_order[dtgOrders.SelectedIndex].CheckDel = true;
                StaticClass.GeneralClass.list_ec_tb_order_general.Add(list_ec_tb_order[dtgOrders.SelectedIndex]);
            }
        }

        //chkDel_Unchecked
        private void chkDel_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dtgOrders.SelectedIndex > -1)
            {
                list_ec_tb_order[dtgOrders.SelectedIndex].CheckDel = false;

                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_order_general.Count; i++)
                {
                    if (StaticClass.GeneralClass.list_ec_tb_order_general[i].OrderID == list_ec_tb_order[dtgOrders.SelectedIndex].OrderID)
                        StaticClass.GeneralClass.list_ec_tb_order_general.RemoveAt(i);
                }
            }
        }

        //btnViewDetail_Click
        private void btnViewDetail_Click(object sender, RoutedEventArgs e)
        {
            if (dtgOrders.SelectedIndex > -1)
            {
                StaticClass.GeneralClass.orderdetailid_general = list_ec_tb_order[dtgOrders.SelectedIndex].OrderID;
                OrderDetail page = new OrderDetail();
                var m = Application.Current.MainWindow;
                page.Owner = m;
                page.ShowInTaskbar = false;
                page.btnviewdetail_delegate += btnViewDetail_Click_Delegate;
                if (m.RenderSize.Width > StaticClass.GeneralClass.width_screen_general)
                    page.Width = StaticClass.GeneralClass.width_screen_general * 90 / 100;

                if (m.RenderSize.Height > StaticClass.GeneralClass.height_screen_working_general)
                    page.Height = StaticClass.GeneralClass.height_screen_working_general;

                else
                {
                    page.Height = m.RenderSize.Height;
                    page.Width = m.RenderSize.Width * 90 / 100;
                }

                page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                page.ShowDialog();
            }
        }

        //dtgOrders_MouseDoubleClick
        private void dtgOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnViewDetail_Click(null, null);
        }

        //btnViewDetail_Click_Delegate
        private void btnViewDetail_Click_Delegate(bool flag_deleted)
        {
            if (flag_deleted == true)
            {
                paging_number_previous = 1;
                current_page_order = 1;
                LoadDataForOrder(salespersonid_select);
            }
        }

        //muiBtnPrint_Click
        private Thread order_thread = null;
        private void muiBtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (order_thread != null && order_thread.ThreadState == ThreadState.Running)
                    order_thread.Abort();
                    //return;
                else
                {
                    order_thread = new Thread(() =>
                    {
                        this.muiBtnPrint.Dispatcher.Invoke((Action)(() =>  { muiBtnPrint.Visibility = System.Windows.Visibility.Hidden; }));
                        this.mprPrint.Dispatcher.Invoke((Action)(() => { mprPrint.IsActive = true; }));

                        ReportGeneralPrint page = new ReportGeneralPrint();
                        page.titleParameter = FindResource("list_of_order").ToString();
                        page.idParameter = FindResource("id").ToString();
                        page.customerNameParameter = FindResource("customer_name").ToString();
                        page.quantityParameter = FindResource("quantity").ToString();
                        page.orderDateParameter = FindResource("order_date").ToString();
                        page.salespersonNameParameter = FindResource("salesperson_name").ToString();
                        page.paymentParameter = FindResource("payment").ToString();
                        page.discountTypeParameter = FindResource("discount_type").ToString();
                        page.discountParameter = FindResource("discount").ToString();
                        page.totalDiscountParameter = FindResource("total_discount").ToString();
                        page.discountParameter = FindResource("discount").ToString();
                        page.totalDiscountParameter = FindResource("total_discount").ToString();
                        page.totalTaxParameter = FindResource("total_tax").ToString();
                        page.totalAmountParameter = FindResource("total_amount").ToString();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            var m = Application.Current.MainWindow;
                            Point m_point_to_screen = StaticClass.GeneralClass.ElementPointToScreenPoint(m as UIElement, new Point(0, 0));

                            if (m.RenderSize.Width > StaticClass.GeneralClass.width_screen_general)
                            {
                                page.point_x = (int)(m_point_to_screen.X + StaticClass.GeneralClass.width_screen_general / 2 - page.Width / 2);
                                page.Height = (int)m.RenderSize.Height;
                            }

                            if (m.RenderSize.Height > StaticClass.GeneralClass.height_screen_working_general)
                            {
                                page.point_y = 0;
                                page.Height = (int)StaticClass.GeneralClass.height_screen_working_general;
                            }

                            else
                            {
                                page.point_x = (int)(m_point_to_screen.X + (m.RenderSize.Width / 2 - page.Width / 2));
                                page.point_y = (int)m_point_to_screen.Y;
                                page.Height = (int)m.RenderSize.Height;
                            }
                            page.ShowInTaskbar = false;
                        }));

                        //Thread.Sleep(500);
                        this.mprPrint.Dispatcher.Invoke((Action)(() => { mprPrint.IsActive = false; }));
                        this.muiBtnPrint.Dispatcher.Invoke((Action)(() => { muiBtnPrint.Visibility = System.Windows.Visibility.Visible; }));
                        page.ShowDialog();
                    });

                    order_thread.SetApartmentState(ApartmentState.STA);
                    order_thread.Start();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //muiBtnDelete_Click
        private void muiBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteOrder page = new DeleteOrder();
            page.btndelete_delegate += muiBtnDelete_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnDelete_Click_Delegate
        private void muiBtnDelete_Click_Delegate(bool flag_deleted)
        {
            if (flag_deleted == true)
            {
                paging_number_previous = 1;
                current_page_order = 1;
                LoadDataForOrder(salespersonid_select);
            }
        }

    }
}
