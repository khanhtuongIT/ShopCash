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
using System.Threading;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for FindCustomer.xaml
    /// </summary>
    public partial class FindCustomer : ModernDialog
    {
        //using for customer
        private BUS_tb_Customer bus_tb_customer = new BUS_tb_Customer();
        private List<EC_tb_Customer> list_ec_tb_customer = new List<EC_tb_Customer>();

        //thread
        private Thread thread_content = null;
        private Thread thread_paging = null;
        private Thread thread_calculatetotalpages = null;

        //delegate
        public delegate void muiBtnCustomer_Click_Delegate();
        public event muiBtnCustomer_Click_Delegate btncustomer_delegate;

        public FindCustomer()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            this.dtgCustomer.ItemsSource = list_ec_tb_customer;
        }

        //muiBtnAdd_Click
        private void muiBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Pages.Setting.AddCustomer page = new Pages.Setting.AddCustomer();
            page.muibtnadd_delegate += muiBtnAdd_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnAdd_Click_Delegate
        private void muiBtnAdd_Click_Delegate(bool flag_added)
        {
            if (flag_added == true)
            {
                //set flag_added_customer_general when added
                StaticClass.GeneralClass.flag_added_customer_general = true;

                txbCustomer.Text = "";
                txbPhone.Text = "";
                con1 = "";
                con2 = "";
                spPaging.Children.Remove(stp);
                txbCustomer.Focus();
                CalculateTotalPages("");

                GetCustomer((total_page - 1) * page_size, page_size, con1, con2, true);

                if ((page_current + 1) * paging_size * page_size > count_rows)
                {
                    int _paging_size = (count_rows - (page_current * paging_size * page_size)) / page_size;

                    if ((count_rows - (page_current * paging_size * page_size)) % page_size > 0)
                        _paging_size += 1;

                    PagingPage(page_current, _paging_size, total_page);
                }
                else
                    PagingPage(page_current, paging_size, total_page);
            }
        }

        //muiBtnClose_Click
        private void muiBtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //ModernDialog_Loaded
        private int count_rows = 0;
        private int total_page = 0;
        private int page_size = 100;
        private int paging_size = 10;
        private int page_current = 0;
        private int paging_number_previous = 1;
        private string con1 = "";
        private string con2 = "";
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle.png", UriKind.Absolute));
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging_focus = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_focus.png", UriKind.Absolute));

        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txbCustomer.Focus();
            GetCustomer(0 * page_size, page_size, con1, con2, false);
            CalculateTotalPages("");

            if (thread_paging != null && thread_paging.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging = new Thread(() =>
                {
                    try
                    {
                        if ((page_current + 1) * paging_size * page_size > count_rows)
                        {
                            int _paging_size = (count_rows - (page_current * paging_size * page_size)) / page_size;

                            if ((count_rows - (page_current * paging_size * page_size)) % page_size > 0)
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
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message + "!";
                            md.CloseButton.Content = FindResource("close").ToString();
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
                    count_rows = bus_tb_customer.GetSumCustomer(con);
                    this.total_page = count_rows / page_size;
                    if (count_rows % page_size > 0)
                        this.total_page += 1;
                });
                thread_calculatetotalpages.Start();
                thread_calculatetotalpages.Join();
            }
        }

        //calculateTotalPages
        private StackPanel stp;
        private bool flag_stp_added = false;
        private void PagingPage(int page, int _paging_size, int total_page)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (flag_stp_added == true)
                    spPaging.Children.Remove(stp);

                if (total_page > 1)
                {
                    stp = new StackPanel();
                    stp.Orientation = Orientation.Horizontal;

                    //set stp is added
                    flag_stp_added = true;

                    if (page > 0)
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

                    for (int i = (page * paging_size); i < ((page * paging_size) + _paging_size); i++)
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
                        if (_img_paging.Name == "img" + paging_number_previous)
                            _img_paging.Source = bitmap_image_paging;
                    }
                }
            }
            paging_number_previous = int_current_page;
            Image img_focus = (Image)grid_paging.Children[0];
            img_focus.Source = bitmap_image_paging_focus;
            GetCustomer((int_current_page - 1) * page_size, page_size, con1, con2, false);
        }

        //Imge_Previous_MouseDown
        private void Img_Previous_MouseDown(object sender, MouseButtonEventArgs e)
        {
            page_current--;

            if (thread_paging != null && thread_paging.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging = new Thread(() =>
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
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message + "!";
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_paging.Start();
            }
        }

        //Img_Next_MouseDown
        private void Img_Next_MouseDown(object sender, MouseButtonEventArgs e)
        {
            page_current++;

            if (thread_paging != null && thread_paging.ThreadState == ThreadState.Running) { }
            else
            {
                thread_paging = new Thread(() =>
                {
                    try
                    {
                        if ((page_current + 1) * paging_size * page_size > count_rows)
                        {
                            int _paging_size = (count_rows - (page_current * paging_size * page_size)) / page_size;

                            if ((count_rows - (page_current * paging_size * page_size)) % page_size > 0)
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
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message + "!";
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_paging.Start();
            }
        }

        //HplPaging_Click
        private void HplPaging_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hpl = (Hyperlink)sender;
            string str_current_page = hpl.Name.Substring(3);
            int int_current_page = Convert.ToInt32(str_current_page);
            GetCustomer((int_current_page - 1) * page_size, page_size, con1, con2, false);
        }

        //get data for Customer listbox
        private DataTable dt_customer = new DataTable();
        private void GetCustomer(int be_limit, int af_limit, string con1, string con2, bool add)
        {
            if (thread_content != null && thread_content.ThreadState == ThreadState.Running) { }
            else
            {
                thread_content = new Thread(() => 
                {
                    try
                    {
                        int i = 0;
                        list_ec_tb_customer.Clear();
                        dt_customer.Clear();
                        this.Dispatcher.Invoke((Action)(() => { dtgCustomer.Items.Refresh(); }));
                        dt_customer = bus_tb_customer.GetCustomerFollowPaging(be_limit, af_limit, con1, con2, StaticClass.GeneralClass.flag_database_type_general);

                        foreach (DataRow datarow in dt_customer.Rows)
                        {
                            i++;

                            EC_tb_Customer ec_tb_customer = new EC_tb_Customer();
                            ec_tb_customer.CustomerID = Convert.ToInt32(datarow["CustomerID"].ToString());
                            ec_tb_customer.FirstName = datarow["FirstName"].ToString();
                            ec_tb_customer.LastName = datarow["LastName"].ToString();
                            ec_tb_customer.FullName = ec_tb_customer.FirstName + " " + ec_tb_customer.LastName;
                            ec_tb_customer.Address1 = datarow["Address1"].ToString();
                            ec_tb_customer.Address2 = datarow["Address2"].ToString();
                            ec_tb_customer.City = datarow["City"].ToString();
                            ec_tb_customer.State = datarow["State"].ToString();
                            ec_tb_customer.Zipcode = datarow["Zipcode"].ToString();
                            ec_tb_customer.Phone = datarow["Phone"].ToString();
                            ec_tb_customer.Email = datarow["Email"].ToString();
                            ec_tb_customer.ImageUrl = @"pack://application:,,,/Resources/select_customer.png";

                            if (i % 2 != 0)
                                ec_tb_customer.Background = "AliceBlue";

                            list_ec_tb_customer.Add(ec_tb_customer);
                        }
                        this.Dispatcher.Invoke((Action)(() => { dtgCustomer.Items.Refresh(); }));

                        //if is added
                        if (add == true)
                        {
                            this.Dispatcher.Invoke((Action)(() => 
                            { 
                                dtgCustomer.SelectedIndex = list_ec_tb_customer.Count - 1;
                                dtgCustomer.ScrollIntoView(dtgCustomer.Items.GetItemAt(list_ec_tb_customer.Count - 1));
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message + "!";
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_content.Start();
            }
        }

        //FindCustomer
        private void FindCustomers()
        {
            try
            {
                con1 = "WHERE ([FirstName] like '%" + txbCustomer.Text.Trim() + "%' Or [LastName] like'%" + txbCustomer.Text.Trim() + "%') And [Phone] like '%" + txbPhone.Text.Trim().ToString() + "%'";
                con2 = "AND ([FirstName] like '%" + txbCustomer.Text.Trim() + "%' Or [LastName] like'%" + txbCustomer.Text.Trim() + "%') And [Phone] like '%" + txbPhone.Text.Trim().ToString() + "%'";
                page_current = 0;

                GetCustomer(0 * page_size, page_size, con1, con2, false);
                spPaging.Children.Remove(stp);
                CalculateTotalPages("WHERE ([FirstName] like '%" + txbCustomer.Text.Trim() + "%' Or LastName like'%" + txbCustomer.Text.Trim() + "%') And [Phone] like '%" + txbPhone.Text.Trim().ToString() +"%'");

                if ((page_current + 1) * paging_size * page_size > count_rows)
                {
                    int _paging_size = (count_rows - (page_current * paging_size * page_size)) / page_size;

                    if ((count_rows - (page_current * paging_size * page_size)) % page_size > 0)
                        _paging_size += 1;

                    PagingPage(page_current, _paging_size, total_page);
                }
                else
                    PagingPage(page_current, paging_size, total_page);
            }
            catch (Exception ex)
            {
                ModernDialog md = new ModernDialog();
                md.Title = FindResource("notification").ToString();
                md.Content = ex.Message + "!";
                md.CloseButton.Content = FindResource("close").ToString();
                md.ShowDialog();
            }
        }

        //lbCustomer_MouseDoubleClick
        private void dtgCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgCustomer.SelectedIndex > -1)
            {
                StaticClass.GeneralClass.customerid_general = list_ec_tb_customer[dtgCustomer.SelectedIndex].CustomerID;
                StaticClass.GeneralClass.customername_general = list_ec_tb_customer[dtgCustomer.SelectedIndex].FullName;
                StaticClass.GeneralClass.customeremail_general = list_ec_tb_customer[dtgCustomer.SelectedIndex].Email;
                StaticClass.GeneralClass.customerphone_general = list_ec_tb_customer[dtgCustomer.SelectedIndex].Phone;

                if (btncustomer_delegate != null)
                {
                    btncustomer_delegate();
                    this.Close();
                }
            }
        }

        //btnSelect_Click
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgCustomer.SelectedIndex > -1)
            {
                StaticClass.GeneralClass.customerid_general = list_ec_tb_customer[dtgCustomer.SelectedIndex].CustomerID;
                StaticClass.GeneralClass.customername_general = list_ec_tb_customer[dtgCustomer.SelectedIndex].FullName;
                StaticClass.GeneralClass.customeremail_general = list_ec_tb_customer[dtgCustomer.SelectedIndex].Email;
                StaticClass.GeneralClass.customerphone_general = list_ec_tb_customer[dtgCustomer.SelectedIndex].Phone;

                if (btncustomer_delegate != null)
                {
                    btncustomer_delegate();
                    this.Close();
                }
            }
        }

        //muiBtnSearch_Click
        private void muiBtnSearch_Click(object sender, RoutedEventArgs e)
        {
            paging_number_previous = 1;
            FindCustomers();
        }

        //txbCustomer_KeyDown
        private void txbCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                paging_number_previous = 1;
                FindCustomers();
            }
        }

        //txbPhone_KeyDown
        private void txbPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                paging_number_previous = 1;
                FindCustomers();
            }
        }

    }
}
