using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
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

using System.Windows.Media.Animation;
using CashierRegister.Model;

namespace CashierRegister.Pages.Report
{
    /// <summary>
    /// Interaction logic for All.xaml
    /// </summary>
    public partial class All : UserControl
    {
        //using for orders
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        private List<EC_tb_Order> list_ec_tb_order = new List<EC_tb_Order>();

        //using for month and year and field
        private List<EC_tb_Date> list_ec_tb_month = new List<EC_tb_Date>();
        private List<EC_tb_Date> list_ec_tb_year = new List<EC_tb_Date>();
        private List<cls_Field> list_ec_tb_field = new List<cls_Field>();

        //thread
        private Thread thread_menu = null;
        private Thread thread_order = null;
        private Thread thread_paging = null;
        private Thread thread_paging_next = null;
        private Thread thread_paging_previous = null;
        private Thread thread_calculatetotalpages = null;

        //UserControl_Loaded
        private int count_rows = 0;
        private int total_page = 0;
        private int page_size = 100;
        private int paging_size = 20;
        private int page_current = 1;
        private string con1 = "";
        private string con2 = "";
        private string con3 = "";
        private bool flag_stp_added = false;
        private bool flag_check_loaded = false;
        private int paging_number_previous = 1;
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle.png", UriKind.Absolute));
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging_focus = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_focus.png", UriKind.Absolute));
        private string _orderTime = string.Empty;
        private string _orderDate = string.Empty;

        //ReportGeneral
        public All()
        {
            InitializeComponent();
            imgArrow.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/arrow_left.png"));
            imgShowAll.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/all.png"));
            imgSearch.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/search.png"));
            imgDate.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/date_month_year.png"));
            imgMonthYear.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/month_year.png"));
            imgYear.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/year.png"));
            _orderTime = (!string.IsNullOrEmpty(StaticClass.GeneralClass.app_settings["timeFormat"].ToString())) ? " " + StaticClass.GeneralClass.timeFromatSettings[StaticClass.GeneralClass.app_settings["timeFormat"]].ToString() : "";

            #region conver string color to media brush
            //System.IO.StreamReader streamreader = new System.IO.StreamReader("Setting");
            //string color = streamreader.ReadLine().Split(':')[1].ToString();
            //BrushConverter brush_convert = new BrushConverter();
            //Color _color = (System.Windows.Media.Color)ColorConverter.ConvertFromString(color);
            //SolidColorBrush accent_color = new SolidColorBrush(_color);
            //gridMenu.Background = accent_color;
            //streamreader.Close();
            #endregion

            cboMonth.ItemsSource = list_ec_tb_month;
            cboYear.ItemsSource = list_ec_tb_year;
            cboOnlyYear.ItemsSource = list_ec_tb_year;
            dtgOrders.ItemsSource = list_ec_tb_order;
            cboField.ItemsSource = list_ec_tb_field;

            if (thread_menu != null && thread_menu.ThreadState == ThreadState.Running) { }
            else
            {
                thread_menu = new Thread(() =>
                {
                    CreateMonthData();
                    CreateYearData();
                });
                thread_menu.Start();
            }
        }

        //CreateFieldData
        private void CreateFieldData()
        {
            list_ec_tb_field.Add(new cls_Field { Field_Display = FindResource("or_customer_name").ToString(), Field = "CustomerName", });
            list_ec_tb_field.Add(new cls_Field { Field_Display = FindResource("salesperson_name").ToString(), Field = "SalespersonName", });
            list_ec_tb_field.Add(new cls_Field { Field_Display = FindResource("payment_name").ToString(), Field = "PaymentName", });
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            isHide = true;
            txbArrow.Text = FindResource("expanded").ToString();
            imgArrow.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/arrow_left.png"));
            EasingFunctionBase function = StoryboardLibrary.ListOfFunctions[6];
            EasingMode mode = EasingMode.EaseOut;
            StoryboardLibrary.MenuAnim(gridMenu, StoryboardLibrary.MenuAnimOption.Hide, gridMenu.RenderSize.Width - 200, function, mode, StoryboardLibrary.MoveDirection.RightLeft).Begin();
            for (int i = 200; i > 50; i--)
            {
                col2.Width = new GridLength(i);
            }

            chkAll.IsChecked = false;

            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                LoadData();

                new Thread(() =>
                {
                    list_ec_tb_field.Clear();
                    CreateFieldData();

                    this.Dispatcher.Invoke((Action)(() => { this.cboField.Items.Refresh(); }));
                }).Start();
            }
        }

        //LoadData
        private void LoadData()
        {
            CalculateTotalPages(con3);
            GetOrders((paging_number_previous - 1) * page_size, page_size, con1, con2);
            this.spPaging.Dispatcher.Invoke((Action)(() => { chkAll.IsChecked = false; }));
            if (thread_paging != null && thread_paging.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging = new Thread(() =>
                {
                    try
                    {
                        if (page_current * paging_size * page_size > count_rows)
                        {
                            int _paging_size = (count_rows - ((page_current - 1) * paging_size * page_size)) / page_size;

                            if ((count_rows - ((page_current - 1) * paging_size * page_size)) % page_size > 0)
                                _paging_size += 1;

                            PagingPage(page_current, _paging_size, total_page);
                        }
                        else
                            PagingPage(page_current, paging_size, total_page);
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content =  FindResource("close").ToString();
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
        private void CalculateTotalPages(string con)
        {
            if (thread_calculatetotalpages != null && thread_calculatetotalpages.ThreadState == ThreadState.Running) { }
            else
            {
                thread_calculatetotalpages = new Thread(() =>
                {
                    count_rows = bus_tb_order.GetSumOrder(con);
                    this.total_page = count_rows / page_size;
                    if (count_rows % page_size > 0)
                        this.total_page += 1;
                });
                thread_calculatetotalpages.Start();
                thread_calculatetotalpages.Join();
            }
        }

        //PagingPage
        private StackPanel stp;
        private void PagingPage(int page, int _paging_size, int total_page)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (flag_stp_added == true)
                    this.spPaging.Dispatcher.Invoke((Action)(() => { spPaging.Children.Remove(stp); }));

                if (total_page > 1)
                {
                    stp = new StackPanel();
                    stp.Orientation = Orientation.Horizontal;

                    //set stp is added
                    flag_stp_added = true;

                    if ((page -1) > 0)
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
                        stp.Children.Add(gr_paging);

                    }
                    //GC.Collect();

                    spPaging.Children.Add(stp);

                    if ((page < total_page) && (paging_size == _paging_size))
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
            GetOrders((int_current_paging - 1) * page_size, page_size, con1, con2);
        }

        //Img_Next_MouseDown
        private void Img_Next_MouseDown(object sender, MouseButtonEventArgs e)
        {
            page_current++;

            if (thread_paging_next != null && thread_paging_next.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging_next = new Thread(() =>
                {
                    try
                    {
                        if (page_current * paging_size * page_size > count_rows)
                        {
                            int _paging_size = (count_rows - ((page_current - 1) * paging_size * page_size)) / page_size;

                            if ((count_rows - ((page_current - 1) * paging_size * page_size)) % page_size > 0)
                                _paging_size += 1;

                            PagingPage(page_current, _paging_size, total_page);
                        }
                        PagingPage(page_current, paging_size, total_page);
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content =  FindResource("close").ToString();
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
            page_current--;
            if (thread_paging_previous != null && thread_paging_previous.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging_previous = new Thread(() =>
                {
                    try
                    {
                        PagingPage(page_current, paging_size, total_page);
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title =  FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_paging_previous.Start();
            }
        }

        //muiBtnPrint_Click
        private Thread order_thread = null;

        private void muiBtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (order_thread != null && order_thread.ThreadState == ThreadState.Running)
                {
                    order_thread.Abort();
                    return;
                }
                else
                {
                    order_thread = new Thread(() =>
                    {
                        this.muiBtnPrint.Dispatcher.Invoke((Action)(() => { muiBtnPrint.Visibility = System.Windows.Visibility.Hidden; }));
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
                                page.point_x = (int)(m_point_to_screen.X + StaticClass.GeneralClass.width_screen_general / 2 - page.Width/2);
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

        private string getPaymentMethodForOrder(int orderId)
        {
            DataTable method = OrderPayment.getOrderPaymentName(orderId);
            string strMethod = "";
            foreach (DataRow dr in method.Rows)
            {
                strMethod += dr["Card"] + ", ";
            }
            strMethod = (strMethod != "") ? strMethod.Remove(strMethod.Length - 2, 2) : "Cash";
            return strMethod;
        }
        //get orders
        private DataTable dt_orders = new DataTable();
        private void GetOrders(int be_limit, int af_limit, string _con1, string _con2)
        {
            if (thread_order != null && thread_order.ThreadState == ThreadState.Running) { }
            else
            {
                thread_order = new Thread(()=>
                {
                    try
                    {
                        list_ec_tb_order.Clear();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Visible;
                            this.mpr.IsActive = true;
                            this.dtgOrders.Visibility = System.Windows.Visibility.Hidden;
                            dtgOrders.Items.Refresh();
                        }));

                        dt_orders.Clear();
                        int no = int_current_paging - 1;

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
                                //ec_tb_order.PaymentName = dr["PaymentName"].ToString();
                                ec_tb_order.PaymentName = getPaymentMethodForOrder(Convert.ToInt32(dr["OrderID"].ToString()));
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

                        Thread.Sleep(500);

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            tblTotal.Text = FindResource("total").ToString() + "(" + list_ec_tb_order.Count.ToString() + ")";
                            this.dtgOrders.Items.Refresh();
                            this.mpr.Visibility = System.Windows.Visibility.Hidden;
                            this.mpr.IsActive = false;
                            this.dtgOrders.Visibility = System.Windows.Visibility.Visible;
                        }));

                        //data statistic
                        DataStatistic();

                        //set condition report order
                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                            StaticClass.GeneralClass.condition_report_order = "select * from [tb_Order] " + con1 + " limit " + be_limit + ", " + af_limit;
                        else
                            StaticClass.GeneralClass.condition_report_order = "select top(" + af_limit + ") * from [tb_Order] where [OrderID] not in (select top(" + be_limit + ") [OrderID] from [tb_Order] " + con1 + ") " + con2;
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
            new Thread(() =>
            {
                this.dtgOrders.Dispatcher.Invoke((Action)(() => { dtgOrders.SelectedIndex = -1; }));
                this.dtgOrders.Dispatcher.Invoke((Action)(() =>
                {
                    if (dtgOrders.SelectedIndex == -1)
                    {
                        for (int i = 0; i < list_ec_tb_order.Count; i++)
                        {
                            list_ec_tb_order[i].CheckDel = true;
                        }

                        StaticClass.GeneralClass.list_ec_tb_order_general.AddRange(list_ec_tb_order);
                        this.dtgOrders.Items.Refresh();
                    }
                }));

            }).Start();
        }

        //chkAll_Unchecked
        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            this.dtgOrders.Dispatcher.Invoke((Action)(() => { dtgOrders.SelectedIndex = -1; }));

            this.dtgOrders.Dispatcher.Invoke((Action)(() =>
            {
                if (dtgOrders.SelectedIndex == -1)
                {
                    for (int i = 0; i < list_ec_tb_order.Count; i++)
                    {
                        list_ec_tb_order[i].CheckDel = false;
                    }

                    StaticClass.GeneralClass.list_ec_tb_order_general.Clear();
                    this.dtgOrders.Items.Refresh();
                }
            }));
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
                list_ec_tb_order[dtgOrders.SelectedIndex].CheckDel = false;

            for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_order_general.Count; i++)
            {
                if (StaticClass.GeneralClass.list_ec_tb_order_general[i].OrderID == list_ec_tb_order[dtgOrders.SelectedIndex].OrderID)
                    StaticClass.GeneralClass.list_ec_tb_order_general.RemoveAt(i);
            }
        }

        //btnViewDetail_Click
        private void btnViewDetail_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //dtgOrders_MouseDoubleClick
        private void dtgOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnViewDetail_Click(null, null);
        }

        //btnViewDetail_Click_Delegate
        private void btnViewDetail_Click_Delegate(bool flag_delete)
        {
            if (flag_delete == true)
            {
                page_current = 1;
                paging_number_previous = 1;
                LoadData();
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
        private void muiBtnDelete_Click_Delegate(bool flag_deleteed)
        {
            if (flag_deleteed == true)
            {
                page_current = 1;
                paging_number_previous = 1;
                LoadData();
            }
        }

        //btnArrow_Click
        private bool isHide = false;
        private void btnArrow_Click(object sender, RoutedEventArgs e)
        {
            EasingFunctionBase function = StoryboardLibrary.ListOfFunctions[6];
            EasingMode mode = EasingMode.EaseOut;

            if (isHide)
            {
                imgArrow.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/arrow_right.png"));
                txbArrow.Text = FindResource("collapsed").ToString();
                isHide = !isHide;
                StoryboardLibrary.MenuAnim(gridMenu, StoryboardLibrary.MenuAnimOption.Show, gridMenu.RenderSize.Width - 100, function, mode, StoryboardLibrary.MoveDirection.RightLeft).Begin();

                for (int i = 50; i < 200; i++)
                {
                    col2.Width = new GridLength(i);
                }
            }
            else
            {
                imgArrow.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/arrow_left.png"));
                txbArrow.Text = FindResource("expanded").ToString();
                isHide = !isHide;
                StoryboardLibrary.MenuAnim(gridMenu, StoryboardLibrary.MenuAnimOption.Hide, gridMenu.RenderSize.Width - 200, function, mode, StoryboardLibrary.MoveDirection.RightLeft).Begin();

                for (int i = 200; i > 50; i--)
                {
                    col2.Width = new GridLength(i);
                }
            }
        }

        //ShowAll_Click
        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con1 = "";
                con2 = "";
                con3 = "";
                page_current = 1;
                paging_number_previous = 1;
                LoadData();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
            }
        }

        //imgShowAll_MouseDown
        private void imgShowAll_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                con1 = "";
                con2 = "";
                con3 = "";
                page_current = 1;
                paging_number_previous = 1;
                LoadData();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
            }
        }

        //txbSearch_KeyDown
        private void txbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                try
                {
                    if (cboField.SelectedIndex > -1)
                    {
                        int _indexselected = cboField.SelectedIndex;
                        con1 = "WHERE " + list_ec_tb_field[_indexselected].Field + " LIKE '%" + txbSearch.Text.Trim() + "%'";
                        con2 = "AND " + list_ec_tb_field[_indexselected].Field + " LIKE '%" + txbSearch.Text.Trim() + "%'";
                        con3 = "WHERE " + list_ec_tb_field[_indexselected].Field + " LIKE '%" + txbSearch.Text.Trim() + "%'";
                    }
                    else
                    {
                        con1 = "";
                        con2 = "";
                        con3 = "";
                    }
                    page_current = 1;
                    paging_number_previous = 1;
                    LoadData();
                }
                catch (Exception ex)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = ex.Message;
                    page.ShowDialog();
                }
            }
        }

        //imgSearch_MouseDown
        private void imgSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (cboField.SelectedIndex > -1)
                {
                    int _indexselected = cboField.SelectedIndex;
                    con1 = "WHERE " + list_ec_tb_field[_indexselected].Field + " LIKE '%" + txbSearch.Text.Trim() + "%'";
                    con2 = "AND " + list_ec_tb_field[_indexselected].Field + " LIKE '%" + txbSearch.Text.Trim() + "%'";
                    con3 = "WHERE " + list_ec_tb_field[_indexselected].Field + " LIKE '%" + txbSearch.Text.Trim() + "%'";
                }
                else
                {
                    con1 = "";
                    con2 = "";
                    con3 = "";
                }
                page_current = 1;
                paging_number_previous = 1;
                LoadData();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //cboField_SelectionChanged
        private void cboField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboField.SelectedIndex > -1)
                {
                    int _indexselected = cboField.SelectedIndex;
                    con1 = "WHERE " + list_ec_tb_field[_indexselected].Field + " LIKE '%" + txbSearch.Text.Trim() + "%'";
                    con2 = "AND " + list_ec_tb_field[_indexselected].Field + " LIKE '%" + txbSearch.Text.Trim() + "%'";
                    con3 = "WHERE " + list_ec_tb_field[_indexselected].Field + " LIKE '%" + txbSearch.Text.Trim() + "%'";
                    page_current = 1;
                    paging_number_previous = 1;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //dtpDateSearch_SelectedDateChanged
        private void dtpDateSearch_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DateTime date_time;
                if(DateTime.TryParse(dtpDateSearch.Text.Trim(), out date_time))
                {
                    string ymd = date_time.ToString("yyyy-MM-dd");
                    con1 = "WHERE OrderDate LIKE '%" + ymd + "%'";
                    con2 = "AND OrderDate LIKE '%" + ymd + "%'";
                    con3 = "WHERE OrderDate LIKE '%" + ymd + "%'";
                    page_current = 1;
                    paging_number_previous = 1;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //imgDate_MouseDown
        private void imgDate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dtpDateSearch_SelectedDateChanged(null, null);
        }

        //cboMonth_SelectionChanged
        private void cboMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboMonth.SelectedIndex > -1)
                {
                    if (cboYear.SelectedIndex == -1)
                    {
                        con1 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%'";
                        con2 = "AND OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%'";
                        con3 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%'";
                    }
                    else
                    {
                        con1 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%' AND OrderDate LIKE '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                        con2 = "AND OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%' AND OrderDate LIKE '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                        con3 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%' AND OrderDate LIKE '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                    }
                    page_current = 1;
                    paging_number_previous = 1;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //cboYear_SelectionChanged
        private void cboYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboYear.SelectedIndex > -1)
                {
                    if (cboMonth.SelectedIndex == -1)
                    {
                        con1 = "WHERE OrderDate Like '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                        con2 = "AND OrderDate Like '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                        con3 = "WHERE OrderDate Like '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                    }
                    else
                    {
                        con1 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%' AND OrderDate LIKE '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                        con2 = "AND OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%' AND OrderDate LIKE '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                        con3 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%' AND OrderDate LIKE '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                    }
                    page_current = 1;
                    paging_number_previous = 1;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //imgMonthYear_MouseDown
        private void imgMonthYear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (cboMonth.SelectedIndex > -1)
                {
                    con1 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%'";
                    con2 = "AND OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "%'";
                    con3 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%'";
                }

                if (cboYear.SelectedIndex > -1)
                {
                    con1 = "WHERE OrderDate Like '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                    con2 = "AND OrderDate Like '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                    con3 = "WHERE OrderDate Like '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                }

                if ((cboMonth.SelectedIndex > -1) && cboYear.SelectedIndex > -1)
                {
                    con1 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%' AND OrderDate LIKE '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                    con2 = "AND OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%' AND OrderDate LIKE '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                    con3 = "WHERE OrderDate Like '%-" + list_ec_tb_month[cboMonth.SelectedIndex].Month + "-%' AND OrderDate LIKE '%" + list_ec_tb_year[cboYear.SelectedIndex].Year + "-%'";
                }

                if (cboMonth.SelectedIndex == -1 && cboYear.SelectedIndex == -1)
                {
                    con1 = "";
                    con2 = "";
                    con3 = "";
                }
                page_current = 1;
                paging_number_previous = 1;
                LoadData();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //CreateMonthData
        private void CreateMonthData()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i < 10)
                    list_ec_tb_month.Add(new EC_tb_Date { Month = "0" + i.ToString() });
                else
                    list_ec_tb_month.Add(new EC_tb_Date { Month = i.ToString() });
            }
        }

        //CreateYearData
        private DataTable dt_year = new DataTable();
        private void CreateYearData()
        {
            dt_year.Clear();
            list_ec_tb_year.Clear();
            dt_year = bus_tb_order.GetYearFromOrder("", StaticClass.GeneralClass.flag_database_type_general);

            foreach (DataRow dr in dt_year.Rows)
            {
                list_ec_tb_year.Add(new EC_tb_Date { Year = dr["Year"].ToString() });
            }
        }

        //cboOnlyYear_SelectionChanged
        private void cboOnlyYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboOnlyYear.SelectedIndex > -1)
            {
                try
                {
                    con1 = "WHERE OrderDate Like '%" + list_ec_tb_year[cboOnlyYear.SelectedIndex].Year + "-%'";
                    con2 = "AND OrderDate Like '%" + list_ec_tb_year[cboOnlyYear.SelectedIndex].Year + "-%'";
                    con3 = "WHERE OrderDate Like '%" + list_ec_tb_year[cboOnlyYear.SelectedIndex].Year + "-%'";
                    page_current = 1;
                    paging_number_previous = 1;
                    LoadData();
                }
                catch (Exception ex)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = ex.Message;
                    page.ShowDialog();
                }
            }
        }

        //imgYear_MouseDown
        private void imgYear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (cboOnlyYear.SelectedIndex > -1)
                {
                    con1 = "WHERE OrderDate Like '%" + list_ec_tb_year[cboOnlyYear.SelectedIndex].Year + "-%'";
                    con2 = "AND OrderDate Like '%" + list_ec_tb_year[cboOnlyYear.SelectedIndex].Year + "-%'";
                    con3 = "WHERE OrderDate Like '%" + list_ec_tb_year[cboOnlyYear.SelectedIndex].Year + "-%'";
                }
                else
                {
                    con1 = "";
                    con2 = "";
                    con3 = "";
                }

                page_current = 1;
                paging_number_previous = 1;
                LoadData();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }
    }

    //cls_Field
    public class cls_Field
    {
        private string _Field;

        public string Field
        {
            get { return _Field; }
            set { _Field = value; }
        }

        private string _Field_Display;

        public string Field_Display
        {
            get { return _Field_Display; }
            set { _Field_Display = value; }
        }
    }
}
