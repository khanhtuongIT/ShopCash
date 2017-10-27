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

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : UserControl
    {
        private BUS_tb_Customer bus_tb_customer = new BUS_tb_Customer();
        private List<EC_tb_Customer> list_ec_tb_customer = new List<EC_tb_Customer>();

        //thread
        private Thread thread_content = null;
        private Thread thread_paging = null;
        private Thread thread_calculatetotalpages = null;

        //curremt directory
        string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        //Customer
        public Customer()
        {
            InitializeComponent();
            dtgCustomer.ItemsSource = list_ec_tb_customer;
        }

        //UserControl_Loaded
        private int count_rows = 0;
        private int total_page = 0;
        private int page_size = 100;
        private int paging_size = 20;
        private int page_current = 1;
        private string con1 = "";
        private string con2 = "";
        private bool flag_check_loaded = false;
        private int paging_number_previous = 1;
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle.png", UriKind.Absolute));
        private System.Windows.Media.Imaging.BitmapImage bitmap_image_paging_focus = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_focus.png", UriKind.Absolute));

        private bool _exportIsRuning = false;
        private bool _exportStatus = false;
        private System.Windows.Threading.DispatcherTimer _myTimer_ = null;
        private Waiting _Waiting = null;
        private Thread _threadWaiting = null;
        Microsoft.Win32.SaveFileDialog _dlg = null;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                LoadData();
            }
        }

        //LoadData
        private void LoadData()
        {
            CalculateTotalPages(con1);

            GetCustomer((paging_number_previous - 1) * page_size, page_size, con1, con2, false, -1);

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

        //PagingPage
        private StackPanel stp;
        private bool flag_stp_added = false;
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
            GetCustomer((int_current_paging - 1) * page_size, page_size, con1, con2, false, -1);

        }

        //Imge_Previous_MouseDown
        private void Img_Previous_MouseDown(object sender, MouseButtonEventArgs e)
        {
            page_current--;
            PagingPage(page_current, paging_size, total_page);
        }

        //Img_Next_MouseDown
        private void Img_Next_MouseDown(object sender, MouseButtonEventArgs e)
        {
            page_current++;

            if (page_current * paging_size * page_size > count_rows)
            {
                int _paging_size = (count_rows - ((page_current - 1) * paging_size * page_size)) / page_size;

                if ((count_rows - ((page_current -1) * paging_size * page_size)) % page_size > 0)
                    _paging_size += 1;

                PagingPage(page_current, _paging_size, total_page);
            }
            else
                PagingPage(page_current, paging_size, total_page);
        }

        //muiBtnDelete_Click
        private void muiBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteCustomer page = new DeleteCustomer();
            page.muibtndelete_delegate += muiBtnDelete_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnDelete_Click_Delegate
        private void muiBtnDelete_Click_Delegate(bool flag_delete)
        {
            if (flag_delete == true)
            {
                page_current = 1;
                paging_number_previous = 1;
                LoadData();
            }
        }

        //muiBtnAdd_Click
        private void muiBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer page = new AddCustomer();
            page.muibtnadd_delegate += muiBtnAdd_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnAdd_Click_Delegate
        private void muiBtnAdd_Click_Delegate(bool flag_added)
        {
            txbSearch.Text = "";
            txbSearch.Focus();
            con1 = "";
            con2 = "";
            CalculateTotalPages(con1);
            GetCustomer((total_page - 1) * page_size, page_size, con1, con2, true, -1);

            new Thread(() =>
            {
                try
                {
                    paging_number_previous = total_page;
                    int page_current_final = 0;
                    int total = count_rows / page_size;
                    if (count_rows % page_size > 0)
                        total += 1;

                    int _paging_size = 0;
                    if (total <= 20)
                    {
                        page_current_final = 1;
                        _paging_size = total;
                    }
                    else
                    {
                        page_current_final = total / paging_size;

                        if (total % paging_size > 0)
                        {
                            page_current_final += 1;
                            _paging_size = count_rows - (page_current_final - 1) * paging_size * page_size;
                        }
                    }

                    int __paging_size = _paging_size / page_size;

                    if (_paging_size % page_size > 0)
                        __paging_size++;

                    page_current = page_current_final;
                    PagingPage(page_current, __paging_size, total_page);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        ModernDialog md = new ModernDialog();
                        md.CloseButton.Content = FindResource("close").ToString();
                        md.Title = FindResource("notification").ToString();
                        md.Content = ex.Message + "!";
                        md.ShowDialog();
                    }));
                }
            }).Start();
        }

        //get data for Customer listbox
        private DataTable dt_customer = new DataTable();
        private void GetCustomer(int be_limit, int af_limit, string con1, string con2, bool add, int _row_selected)
        {
            if (thread_content != null && thread_content.ThreadState == ThreadState.Running) { }
            else
            {
                thread_content = new Thread(() =>
                {
                    try
                    {
                        list_ec_tb_customer.Clear();
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.chkAll.IsChecked = false;
                            this.mpr.IsActive = true;
                            this.dtgCustomer.Visibility = System.Windows.Visibility.Hidden;
                            this.tblTotal.Text = FindResource("total_zero").ToString();
                            dtgCustomer.Items.Refresh();
                        }));

                        dt_customer.Clear();
                        dt_customer = bus_tb_customer.GetCustomerFollowPaging(be_limit, af_limit, con1, con2, StaticClass.GeneralClass.flag_database_type_general);

                        int no = paging_number_previous - 1;
                        foreach (DataRow datarow in dt_customer.Rows)
                        {
                            no++;
                            EC_tb_Customer ec_tb_customer = new EC_tb_Customer();
                            ec_tb_customer.No = no;
                            ec_tb_customer.CustomerID = Convert.ToInt32(datarow["CustomerID"].ToString());
                            ec_tb_customer.FirstName = datarow["FirstName"].ToString();
                            ec_tb_customer.LastName = datarow["LastName"].ToString();
                            ec_tb_customer.FullName = datarow["FirstName"].ToString() + " " + datarow["LastName"].ToString();
                            ec_tb_customer.Address1 = datarow["Address1"].ToString();
                            ec_tb_customer.Address2 = datarow["Address2"].ToString();
                            ec_tb_customer.City = datarow["City"].ToString();
                            ec_tb_customer.State = datarow["State"].ToString();
                            ec_tb_customer.Zipcode = datarow["Zipcode"].ToString();
                            ec_tb_customer.Phone = datarow["Phone"].ToString();
                            ec_tb_customer.Email = datarow["Email"].ToString();
                            ec_tb_customer.CheckDel = false;
                            ec_tb_customer.ImageUrl = @"pack://application:,,,/Resources/edit.png";

                            list_ec_tb_customer.Add(ec_tb_customer);
                        }

                        Thread.Sleep(500);
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            dtgCustomer.Items.Refresh();
                            tblTotal.Text = FindResource("total").ToString() + "(" + list_ec_tb_customer.Count.ToString() + ")";
                        }));

                        //if is added
                        if (add == true)
                        {
                            this.dtgCustomer.Dispatcher.Invoke((Action)(() =>
                            {
                                dtgCustomer.SelectedIndex = list_ec_tb_customer.Count - 1;
                                dtgCustomer.ScrollIntoView(dtgCustomer.Items.GetItemAt(list_ec_tb_customer.Count - 1));
                            }));
                        }
                        
                        //if edited
                        if (_row_selected > -1)
                        {
                            this.dtgCustomer.Dispatcher.Invoke((Action)(() =>
                            {
                                dtgCustomer.SelectedIndex = _row_selected;
                                dtgCustomer.ScrollIntoView(dtgCustomer.Items.GetItemAt(_row_selected));
                            }));
                        }

                        this.mpr.Dispatcher.Invoke((Action)(() => { this.mpr.IsActive = false; }));
                        this.dtgCustomer.Dispatcher.Invoke((Action)(() => { this.dtgCustomer.Visibility = System.Windows.Visibility.Visible; }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message + "!";
                            md.ShowDialog();
                        }));
                    }
                });
                thread_content.Start();
            }
        }

        //chkAll_Checked
        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            dtgCustomer.SelectedIndex = -1;

            if (dtgCustomer.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_customer.Count; i++)
                {
                    list_ec_tb_customer[i].CheckDel = true;
                }

                StaticClass.GeneralClass.list_ec_tb_customer_general.AddRange(list_ec_tb_customer);
                dtgCustomer.Items.Refresh();
            }
        }

        //chkAll_Unchecked
        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            dtgCustomer.SelectedIndex = -1;

            if (dtgCustomer.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_customer.Count; i++)
                {
                    list_ec_tb_customer[i].CheckDel = false;
                }

                StaticClass.GeneralClass.list_ec_tb_customer_general.Clear();
                dtgCustomer.Items.Refresh();
            }
        }

        //chkDel_Checked
        private void chkDel_Checked(object sender, RoutedEventArgs e)
        {
            if (dtgCustomer.SelectedIndex > -1)
            {
                list_ec_tb_customer[dtgCustomer.SelectedIndex].CheckDel = true;
                StaticClass.GeneralClass.list_ec_tb_customer_general.Add(list_ec_tb_customer[dtgCustomer.SelectedIndex]);
            }
        }

        //chkDel_Unchecked
        private void chkDel_Unchecked(object sender, RoutedEventArgs e)
        {
            if(dtgCustomer.SelectedIndex > -1)
            {
                list_ec_tb_customer[dtgCustomer.SelectedIndex].CheckDel = false;
                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_customer_general.Count; i++)
                {
                    if (StaticClass.GeneralClass.list_ec_tb_customer_general[i].CustomerID == list_ec_tb_customer[dtgCustomer.SelectedIndex].CustomerID)
                        StaticClass.GeneralClass.list_ec_tb_customer_general.RemoveAt(i);
                }
            }
        }

        //btnEdit_Click
        private int index_selected = -1;
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dtgCustomer.SelectedIndex > -1)
            {
                index_selected = dtgCustomer.SelectedIndex;

                EditCustomer page = new EditCustomer();
                page.tblCustomerID.Text = list_ec_tb_customer[index_selected].CustomerID.ToString();
                page.txbFirstname.Text = list_ec_tb_customer[index_selected].FirstName;
                page.txbLastname.Text = list_ec_tb_customer[index_selected].LastName;
                page.txbAddress1.Text = list_ec_tb_customer[index_selected].Address1;
                page.txbAddress2.Text = list_ec_tb_customer[index_selected].Address2;
                page.txbCity.Text = list_ec_tb_customer[index_selected].City;
                page.txbState.Text = list_ec_tb_customer[index_selected].State;
                page.txbZipcode.Text = list_ec_tb_customer[index_selected].Zipcode;
                page.txbPhone.Text = list_ec_tb_customer[index_selected].Phone;
                page.txbEmail.Text = list_ec_tb_customer[index_selected].Email;

                page.muibtnedit_delegate += btnEdit_Click_Delegate;
                page.ShowDialog();
            }
        }

        //dtgCustomer_MouseDoubleClick
        private void dtgCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgCustomer.SelectedIndex > -1)
            {
                index_selected = dtgCustomer.SelectedIndex;

                EditCustomer page = new EditCustomer();
                page.tblCustomerID.Text = list_ec_tb_customer[index_selected].CustomerID.ToString();
                page.txbFirstname.Text = list_ec_tb_customer[index_selected].FirstName;
                page.txbLastname.Text = list_ec_tb_customer[index_selected].LastName;
                page.txbAddress1.Text = list_ec_tb_customer[index_selected].Address1;
                page.txbAddress2.Text = list_ec_tb_customer[index_selected].Address2;
                page.txbCity.Text = list_ec_tb_customer[index_selected].City;
                page.txbState.Text = list_ec_tb_customer[index_selected].State;
                page.txbZipcode.Text = list_ec_tb_customer[index_selected].Zipcode;
                page.txbPhone.Text = list_ec_tb_customer[index_selected].Phone;
                page.txbEmail.Text = list_ec_tb_customer[index_selected].Email;

                page.muibtnedit_delegate += btnEdit_Click_Delegate;
                page.ShowDialog();
            }
        }

        //btnEdit_Click_Delegate
        private void btnEdit_Click_Delegate()
        {
            GetCustomer((paging_number_previous - 1) * page_size, page_size, con1, con2, false, index_selected);
        }

        //txbSearch_KeyDown
        private void txbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
                FindCustomers();
        }

        //muiBtnSearch_Click
        private void muiBtnSearch_Click(object sender, RoutedEventArgs e)
        {
            FindCustomers();
        }

        //FindCustomer
        private void FindCustomers()
        {
            paging_number_previous = 1;
            page_current = 1;
            con1 = "WHERE ([FirstName] like '%" + txbSearch.Text.Trim() + "%' Or [LastName] like'%" + txbSearch.Text.Trim() + "%' Or [Phone] like '%" + txbSearch.Text.Trim().ToString() + "%')";
            con2 = "AND ([FirstName] like '%" + txbSearch.Text.Trim() + "%' Or [LastName] like'%" + txbSearch.Text.Trim() + "%' Or [Phone] like '%" + txbSearch.Text.Trim().ToString() + "%')";
            LoadData();
        }

        private void muiBtnImport_Click(object sender, RoutedEventArgs e)
        {
            Views.Setting.ImportCustomer page = new Views.Setting.ImportCustomer();
            page.ShowDialog();
        }

        private void muiBtnExport_Click(object sender, RoutedEventArgs e)
        {
            _dlg = new Microsoft.Win32.SaveFileDialog();
            _dlg.FileName = string.Format("tb_Customer_{0:MMMddyyyy_HHmmssfff}", DateTime.Now); // Default file name
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
                    _runExportCustomer();
                }
            }
        }
        private async void _runExportCustomer()
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
                    string _lstColumn = this._getListColumnName("tb_Customer").Replace("[", "");
                    _lstColumn = _lstColumn.Replace("]", "");
                    _lstColumn = _lstColumn.Replace(", ", ",");
                    stream_writer.WriteLine(_lstColumn.Substring(11));
                    DataTable _dt = Model.Setting.TableModel.getAllDataTable("tb_Customer");
                    if (_dt.Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dt.Rows)
                        {
                            string _rs = string.Empty;
                            DataTable _dt_ = Model.Setting.TableModel.getColumnsByTable("tb_Customer");
                            foreach (DataRow _dr_ in _dt_.Rows)
                            {
                                if (_dr_["name"].ToString() != "CustomerID")
                                {
                                    string _strComp = _dr_["type"].ToString().ToLower();
                                    if (_strComp.Contains("text") || _strComp.Contains("ntext") || _strComp.Contains("varchar") || _strComp.Contains("nvarchar") || _strComp.Contains("char") || _strComp.Contains("nchar") || _strComp.Contains("xml"))
                                    {
                                        _rs += "\"" + @_dr[_dr_["name"].ToString()].ToString().Replace("\"", @"\u0022") + "\",";
                                    }
                                    else _rs += _dr[_dr_["name"].ToString()].ToString() + ",";
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
            catch(Exception e)
            {
                _info = e.Message;
            }
            return _info;
        }
    }
}
