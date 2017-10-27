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
using FirstFloor.ModernUI.Windows.Controls;
using CashierRegisterDAL;
using System.Data.SqlClient;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for AppSetting.xaml
    /// </summary>
    public partial class AppSetting : UserControl
    {
        //directory
        private bool flag_loaded = false;
        private bool flag_check_connected = false;
        private bool flag_check_sql_server_connected = false;
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        private string server_name = "";
        private int authentication = 0;
        private string user_name = "";
        private string password = "";
        private string sqltype = "";
        private UserControls.UCConnectServer uc_connect_server = new UserControls.UCConnectServer();
        private TextBox txbServerName = new TextBox();
        private ComboBox cboAuthentication = new ComboBox();
        private TextBox txbUserName = new TextBox();
        private PasswordBox pwbPassword = new PasswordBox();
        private Button btnConnect = new Button();
        private Button btnDisconnect = new Button();
        private TextBlock tblNotificationSQLServer = new TextBlock();

        //thread
        private Thread thread_setting = null;
        private Thread thread_connect = null;
        private Thread thread_disconnect = null;
        private Thread thread_sqlite = null;
        private dateFormat _settingDate = dateFormat.slashMdyyyy;
        public dateFormat SettingDate
        {
            get { return _settingDate; }
            set { _settingDate = value; }
        }
        private timeFormat _settingTimes = timeFormat.None;
        public timeFormat SettingTimes
        {
            get { return _settingTimes; }
            set { _settingTimes = value; }
        }
        public List<DateFormat> settingDateFormat { get; set; }
        public List<TimeFormat> settingTimesFormat { get; set; }
        //AppSetting
        public AppSetting()
        {
            InitializeComponent();
            cboDecimalSeparator.SelectedIndex = Convert.ToInt32(StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString());
            settingDateFormat = new List<DateFormat>()
            {
                new DateFormat() { DateValue = dateFormat.commadddddMMMMyyyy, DateDisplay = string.Format(@"dddd, d MMMM, yyyy ({0:dddd, d MMMM, yyyy})", DateTime.Now) },
                new DateFormat() { DateValue = dateFormat.commaddddMMMMdyyyy, DateDisplay = string.Format(@"dddd, MMMM d, yyyy ({0:dddd, MMMM d, yyyy})", DateTime.Now) },
                new DateFormat() { DateValue = dateFormat.commadMMMMyyyy, DateDisplay = string.Format(@"d MMMM, yyyy ({0:d MMMM, yyyy})", DateTime.Now) },
                new DateFormat() { DateValue = dateFormat.commaMMMMdyyyy, DateDisplay = string.Format(@"MMMM d, yyyy ({0:MMMM d, yyyy})", DateTime.Now) },
                new DateFormat() { DateValue = dateFormat.commadMMMyyyy, DateDisplay = string.Format(@"d MMM, yyyy ({0:d MMM, yyyy})", DateTime.Now) },
                new DateFormat() { DateValue = dateFormat.commaMMMdyyyy, DateDisplay = string.Format(@"MMM d, yyyy ({0:MMM d, yyyy})", DateTime.Now) },
                new DateFormat() { DateValue = dateFormat.slashddMMMyyyy, DateDisplay = string.Format(@"dd-MMM-yyyy ({0:dd-MMM-yyyy})", DateTime.Now) },
                new DateFormat() { DateValue = dateFormat.slashMdyyyy, DateDisplay = string.Format(@"M/d/yyyy ({0:M/d/yyyy})", DateTime.Now) },
                new DateFormat() { DateValue = dateFormat.slashMMddyyyy, DateDisplay = string.Format(@"MM/dd/yyyy ({0:MM/dd/yyyy})", DateTime.Now) },
                new DateFormat() { DateValue = dateFormat.slashyyyyMMdd, DateDisplay = string.Format(@"yyyy/MM/dd ({0:yyyy/MM/dd})", DateTime.Now) },
            };
            

            settingTimesFormat = new List<TimeFormat>()
            {
                new TimeFormat() { TimeValue = timeFormat.None, TimeDisplay = "null" },
                new TimeFormat() { TimeValue = timeFormat.HHmm, TimeDisplay = string.Format(@"HH:mm ({0:HH:mm})", DateTime.Now) },
                new TimeFormat() { TimeValue = timeFormat.HHmmss, TimeDisplay = string.Format(@"HH:mm:ss ({0:HH:mm:ss})", DateTime.Now) },
                new TimeFormat() { TimeValue = timeFormat.hhmmsstt, TimeDisplay = string.Format(@"hh:mm:ss tt ({0:hh:mm:ss tt})", DateTime.Now) },
                new TimeFormat() { TimeValue = timeFormat.hhmmtt, TimeDisplay = string.Format(@"hh:mm tt ({0:hh:mm tt})", DateTime.Now) },
                new TimeFormat() { TimeValue = timeFormat.Hmm, TimeDisplay = string.Format(@"H:mm ({0:H:mm})", DateTime.Now) },
                new TimeFormat() { TimeValue = timeFormat.Hmmss, TimeDisplay = string.Format(@"H:mm:ss ({0:H:mm:ss})", DateTime.Now) },
                new TimeFormat() { TimeValue = timeFormat.hmmsstt, TimeDisplay = string.Format(@"h:mm:ss tt ({0:h:mm:ss tt})", DateTime.Now) },
                new TimeFormat() { TimeValue = timeFormat.hmmtt, TimeDisplay = string.Format(@"h:mm tt ({0:h:mm tt})", DateTime.Now) },
            };
            

            if (thread_setting != null && thread_setting.ThreadState == ThreadState.Running) { }
            else
            {
                thread_setting = new Thread(() =>
                {
                    try
                    {
                        this.stp_dateTimeFormat.Dispatcher.Invoke((Action)(() => {
                            SettingDate = (dateFormat)Enum.Parse(typeof(dateFormat), StaticClass.GeneralClass.app_settings["dateFormat"].ToString());
                            cb_date_Format.ItemsSource = settingDateFormat;
                            cb_date_Format.SelectedValue = SettingDate;
                            cb_date_Format.DisplayMemberPath = "DateDisplay";
                            cb_date_Format.SelectedValuePath = "DateValue";

                            SettingTimes = (timeFormat)Enum.Parse(typeof(timeFormat), StaticClass.GeneralClass.app_settings["timeFormat"].ToString());
                            cb_time_Format.ItemsSource = settingTimesFormat;
                            cb_time_Format.SelectedValue = SettingTimes;
                            cb_time_Format.DisplayMemberPath = "TimeDisplay";
                            cb_time_Format.SelectedValuePath = "TimeValue";
                        }));

                        this.stp_storeInfo.Dispatcher.Invoke((Action)(() => {
                            txt_store_name.Text = StaticClass.GeneralClass.app_settings["storeName"].ToString();
                            txt_store_address.Text = StaticClass.GeneralClass.app_settings["storeAddress"].ToString();
                            txt_store_phone.Text = StaticClass.GeneralClass.app_settings["storePhone"].ToString();
                        }));
                        this.stp_backupsetting.Dispatcher.Invoke((Action)(() => {
                            if (StaticClass.GeneralClass.app_settings["typeBackup"].ToString() == "0")
                                bk_manual.IsChecked = true;
                            else if (StaticClass.GeneralClass.app_settings["typeBackup"].ToString() == "1")
                                bk_start.IsChecked = true;
                            else if (StaticClass.GeneralClass.app_settings["typeBackup"].ToString() == "2")
                                bk_close.IsChecked = true;
                        }));
                        //setting for show out_of_stock products
                        if (StaticClass.GeneralClass.app_settings["outOfStock"].ToString()=="False")
                        {
                            this.UCShowStocks.Dispatcher.Invoke((Action)(() =>
                            {
                                Border bd_off = (Border)UCShowStocks.FindName("bdOff");
                                bd_off.MouseDown += new MouseButtonEventHandler(bdOff_MouseDown);
                                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                                tbl_off.Foreground = System.Windows.Media.Brushes.White;

                                Border bd_on = (Border)UCShowStocks.FindName("bdOn");
                                bd_on.MouseDown += new MouseButtonEventHandler(bdOn_MouseDown);
                                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                                tbl_on.Foreground = System.Windows.Media.Brushes.Silver;
                            }));
                        }
                        else
                        {
                            this.UCShowStocks.Dispatcher.Invoke((Action)(() =>
                            {
                                Border bd_off = (Border)UCShowStocks.FindName("bdOff");
                                bd_off.MouseDown += new MouseButtonEventHandler(bdOff_MouseDown);
                                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                                tbl_off.Foreground = System.Windows.Media.Brushes.Silver;

                                Border bd_on = (Border)UCShowStocks.FindName("bdOn");
                                bd_on.MouseDown += new MouseButtonEventHandler(bdOn_MouseDown);
                                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                                tbl_on.Foreground = System.Windows.Media.Brushes.White;
                            }));
                        }

                        //setting for save logs
                        if (StaticClass.GeneralClass.app_settings["isSaveLogs"].ToString()=="False")
                        {
                            this.UCSaveLogs.Dispatcher.Invoke((Action)(() =>
                            {
                                Border bd_off = (Border)UCSaveLogs.FindName("bdOff");
                                bd_off.MouseDown += new MouseButtonEventHandler(bdOffSaveLogs_MouseDown);
                                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                                tbl_off.Foreground = System.Windows.Media.Brushes.White;

                                Border bd_on = (Border)UCSaveLogs.FindName("bdOn");
                                bd_on.MouseDown += new MouseButtonEventHandler(bdOnSaveLogs_MouseDown);
                                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                                tbl_on.Foreground = System.Windows.Media.Brushes.Silver;
                            }));
                        }
                        else
                        {
                            this.UCSaveLogs.Dispatcher.Invoke((Action)(() =>
                            {
                                Border bd_off = (Border)UCSaveLogs.FindName("bdOff");
                                bd_off.MouseDown += new MouseButtonEventHandler(bdOffSaveLogs_MouseDown);
                                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                                tbl_off.Foreground = System.Windows.Media.Brushes.Silver;

                                Border bd_on = (Border)UCSaveLogs.FindName("bdOn");
                                bd_on.MouseDown += new MouseButtonEventHandler(bdOnSaveLogs_MouseDown);
                                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                                tbl_on.Foreground = System.Windows.Media.Brushes.White;
                            }));
                        }


                        this.txbServerName.Dispatcher.Invoke((Action)(() => { this.txbServerName = (TextBox)uc_connect_server.FindName("txbServerName"); }));
                        this.cboAuthentication.Dispatcher.Invoke((Action)(() => {
                            this.cboAuthentication = (ComboBox)uc_connect_server.FindName("cboAuthentication");
                            this.cboAuthentication.SelectionChanged += new SelectionChangedEventHandler(cboAuthentication_SelectiongChanged);
                        }));
                        this.txbUserName.Dispatcher.Invoke((Action)(() => { this.txbUserName = (TextBox)uc_connect_server.FindName("txbUserName"); }));
                        this.pwbPassword.Dispatcher.Invoke((Action)(() => { this.pwbPassword = (PasswordBox)uc_connect_server.FindName("pwbPassword"); }));

                        this.btnConnect.Dispatcher.Invoke((Action)(() => {
                            this.btnConnect = (Button)uc_connect_server.FindName("btnConnect");
                            this.btnConnect.Click += new RoutedEventHandler(btnConnect_Click);
                            this.btnConnect.IsEnabled = false;
                            this.btnConnect.Visibility = System.Windows.Visibility.Visible;
                        }));

                        this.btnDisconnect.Dispatcher.Invoke((Action)(() =>
                        {
                            this.btnDisconnect = (Button)uc_connect_server.FindName("btnDisconnect");
                            this.btnDisconnect.Click += new RoutedEventHandler(btnDisconnect_Click);
                            this.btnDisconnect.IsEnabled = false;
                            this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
                        }));
                        this.stp_login2sell.Dispatcher.Invoke((Action)(() => {
                            if(StaticClass.GeneralClass.app_settings["isLoginToSale"].ToString() == "0")
                            {
                                Border bd_off = (Border)UCLogin2Sell.FindName("bdOff");
                                bd_off.MouseDown += new MouseButtonEventHandler(bdOffLogin2Sell_MouseDown);
                                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                                tbl_off.Foreground = System.Windows.Media.Brushes.White;

                                Border bd_on = (Border)UCLogin2Sell.FindName("bdOn");
                                bd_on.MouseDown += new MouseButtonEventHandler(bdOnLogin2Sell_MouseDown);
                                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                                tbl_on.Foreground = System.Windows.Media.Brushes.Silver;
                            }
                            else
                            {
                                Border bd_off = (Border)UCLogin2Sell.FindName("bdOff");
                                bd_off.MouseDown += new MouseButtonEventHandler(bdOffLogin2Sell_MouseDown);
                                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                                tbl_off.Foreground = System.Windows.Media.Brushes.Silver;

                                Border bd_on = (Border)UCLogin2Sell.FindName("bdOn");
                                bd_on.MouseDown += new MouseButtonEventHandler(bdOnLogin2Sell_MouseDown);
                                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                                tbl_on.Foreground = System.Windows.Media.Brushes.White;
                            }
                        }));

                        this.tblNotificationSQLServer.Dispatcher.Invoke((Action)(() => { this.tblNotificationSQLServer = (TextBlock)uc_connect_server.FindName("tblNotificationSQLServer"); }));

                        //if sqltype file is not exist
                        if (!System.IO.File.Exists(current_directory + @"\sqltype"))
                        {
                            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(current_directory + @"\_sqltype");
                            streamwriter.WriteLine("sqltype:sqlite");
                            streamwriter.WriteLine("server:" + server_name);
                            streamwriter.WriteLine("authentication:" + authentication);
                            streamwriter.WriteLine("id:" + user_name);
                            streamwriter.WriteLine("password:" + password);
                            streamwriter.Close();
                            StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\_sqltype", current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                        }

                        if (System.IO.File.Exists(current_directory + @"\sqltype"))
                        {
                            System.IO.StreamReader stream_sqltype = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                            sqltype = stream_sqltype.ReadLine().Split(':')[1].ToString();
                            server_name = stream_sqltype.ReadLine().Split(':')[1].ToString();
                            Int32.TryParse(stream_sqltype.ReadLine().Split(':')[1].ToString(), out authentication);
                            user_name = stream_sqltype.ReadLine().Split(':')[1].ToString();
                            password = stream_sqltype.ReadLine().Split(':')[1].ToString();
                            stream_sqltype.Close();

                            this.spConnectServer.Dispatcher.Invoke((Action)(() => {
                                this.spConnectServer.Children.Add(uc_connect_server);
                                this.spConnectServer.Visibility = System.Windows.Visibility.Collapsed;
                            }));
                            this.txbServerName.Dispatcher.Invoke((Action)(() => {
                                this.txbServerName.Text = server_name;
                                this.txbServerName.IsEnabled = false;
                            }));
                            //check authentication
                            if (authentication == 0)
                            {
                                this.cboAuthentication.Dispatcher.Invoke((Action)(() => {
                                    this.cboAuthentication.SelectedIndex = 0;
                                    this.cboAuthentication.IsEnabled = false;
                                }));
                                this.txbUserName.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.txbUserName.Text = "";
                                    this.txbUserName.IsEnabled = false;
                                }));
                                this.pwbPassword.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.pwbPassword.Password = "";
                                    this.pwbPassword.IsEnabled = false;
                                }));
                            }
                            else
                            {
                                this.cboAuthentication.Dispatcher.Invoke((Action)(() => {
                                    this.cboAuthentication.SelectedIndex = 1;
                                    this.cboAuthentication.IsEnabled = false;
                                }));
                                this.txbUserName.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.txbUserName.Text = user_name;
                                    this.txbUserName.IsEnabled = false;
                                }));
                                this.pwbPassword.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.pwbPassword.Password = password;
                                    this.pwbPassword.IsEnabled = false;
                                }));
                            }

                            if (sqltype == "sqlite")
                            {
                                flag_check_connected = true;
                                flag_check_sql_server_connected = false;
                                this.rdoSQLite.Dispatcher.Invoke((Action)(() => { this.rdoSQLite.IsChecked = true; }));
                                this.spConnectServer.Dispatcher.Invoke((Action)(() => { this.spConnectServer.Visibility = System.Windows.Visibility.Collapsed; }));
                            }
                            else
                            {
                                this.spConnectServer.Dispatcher.Invoke((Action)(() => { this.spConnectServer.Visibility = System.Windows.Visibility.Visible; }));
                                this.rdoSqLServer.Dispatcher.Invoke((Action)(() => { this.rdoSqLServer.IsChecked = true; }));

                                //check connect
                                string result = CheckConnect();
                                if (result == "successfully")
                                {
                                    flag_check_connected = true;
                                    flag_check_sql_server_connected = true;

                                    this.Dispatcher.Invoke((Action)(() => 
                                    {
                                        this.tblNotificationSQLServer.Text = FindResource("connect_success").ToString();
                                        this.txbServerName.IsEnabled = false;
                                        this.txbUserName.IsEnabled = false;
                                        this.pwbPassword.IsEnabled = false;
                                        this.btnDisconnect.Visibility = System.Windows.Visibility.Visible;
                                        this.btnDisconnect.IsEnabled = true;
                                        this.btnConnect.Visibility = System.Windows.Visibility.Collapsed;
                                        this.btnConnect.IsEnabled = true;
                                    }));
                                }
                                else
                                {
                                    flag_check_connected = false;
                                    flag_check_sql_server_connected = false;

                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        this.tblNotificationSQLServer.Text = result;
                                        this.txbServerName.IsEnabled = true;
                                        this.txbUserName.IsEnabled = true;
                                        this.pwbPassword.IsEnabled = true;
                                        this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
                                        this.btnDisconnect.IsEnabled = true;
                                        this.btnConnect.Visibility = System.Windows.Visibility.Visible;
                                        this.btnConnect.IsEnabled = true;
                                    }));
                                }

                                this.Dispatcher.Invoke((Action)(() => { this.cboAuthentication.IsEnabled = true; }));
                            }
                            this.rdoSQLite.Dispatcher.Invoke((Action)(() => { this.rdoSQLite.Checked += new RoutedEventHandler(rdoSQLite_Checked); }));
                            this.rdoSqLServer.Dispatcher.Invoke((Action)(() => { this.rdoSqLServer.Checked += new RoutedEventHandler(rdoSqLServer_Checked); }));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            this.tblNotificationSQLServer.Text = FindResource("connect_failed").ToString();
                            this.txbServerName.IsEnabled = true;
                            this.cboAuthentication.IsEnabled = false;
                        }));

                        if (authentication == 1)
                        {
                            this.Dispatcher.Invoke((Action)(() => 
                            {
                                this.txbUserName.IsEnabled = true;
                                this.pwbPassword.IsEnabled = true;
                            }));
                        }

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.btnConnect.Visibility = System.Windows.Visibility.Visible;
                            this.btnConnect.IsEnabled = true;
                            this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
                            this.btnDisconnect.IsEnabled = true;

                            ModernDialog md = new ModernDialog();
                            md.Buttons = new Button[] { md.CloseButton, };
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_setting.Start();
            }

            //add closing event for main form
            var m = (ModernWindow)Application.Current.MainWindow;
            m.Closing += new System.ComponentModel.CancelEventHandler(ModernWindow_Closing);
        }

        //ModernWindow_Closing
        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //if rdosqlserver is checked and current database type is sqlite
                if (rdoSqLServer.IsChecked.Value == true && sqltype == "sqlite")
                {
                    ModernDialog md_cl = new ModernDialog();
                    md_cl.Buttons = new Button[] { md_cl.YesButton };
                    md_cl.Title = FindResource("notification").ToString();
                    md_cl.Content = FindResource("failed_connect_sqlserver").ToString();
                    md_cl.YesButton.Content = FindResource("ok").ToString();
                    md_cl.YesButton.Focus();
                    /*if (md_cl.ShowDialog().Value == true)
                        //cancel the closing event from closing the form
                        e.Cancel = false;*/
                }

                //if rdosqlserver is checked and current database type is sqlserver
                if (rdoSqLServer.IsChecked == true && flag_check_connected == false && sqltype == "sqlserver")
                {
                    var m = (ModernWindow)Application.Current.MainWindow;
                    m.ContentSource = new Uri(@"/Pages/Setting/Setting.xaml", UriKind.Relative);
                    bbcode_block.LinkNavigator.Navigate(new Uri(@"/Pages/Setting/AppSetting.xaml", UriKind.Relative), this);

                    ModernDialog md = new ModernDialog();
                    md.Buttons = new Button[] { md.YesButton };
                    md.Title = FindResource("notification").ToString();
                    md.Content = FindResource("failed_connect_sqlserver").ToString();
                    md.YesButton.Content = FindResource("ok").ToString();
                    md.YesButton.Focus();
                    if (md.ShowDialog().Value == true)
                    {
                        System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(current_directory + @"\_sqltype");
                        streamwriter.WriteLine("sqltype:sqlite");
                        streamwriter.WriteLine("server:" + server_name);
                        streamwriter.WriteLine("authentication:" + authentication);
                        streamwriter.WriteLine("id:" + user_name);
                        streamwriter.WriteLine("password:" + password);
                        streamwriter.Close();
                        StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\_sqltype", current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                    }
                }
            }
            catch (Exception)
            {
                e.Cancel = false;
            }
        }

        //cboAuthentication_SelectiongChanged
        private void cboAuthentication_SelectiongChanged(object sender, SelectionChangedEventArgs e)
        {
            this.tblNotificationSQLServer.Text = "";
            this.btnConnect.Visibility = System.Windows.Visibility.Visible;
            this.btnConnect.IsEnabled = true;
            this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
            this.btnDisconnect.IsEnabled = true;

            //windows authentication
            if (cboAuthentication.SelectedIndex == 0)
            {
                this.txbUserName.IsEnabled = false;
                this.pwbPassword.IsEnabled = false;
                this.txbUserName.Text = "";
                this.pwbPassword.Password = "";

                if (authentication == 1 && flag_check_sql_server_connected == true)
                {
                    this.btnConnect.Visibility = System.Windows.Visibility.Visible;
                    this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
                    this.txbServerName.Text = server_name;
                    this.txbServerName.IsEnabled = true;
                    this.txbUserName.Text = "";
                    this.pwbPassword.Password = "";
                }

                if (authentication == 0 && flag_check_sql_server_connected == true)
                {
                    this.btnConnect.Visibility = System.Windows.Visibility.Collapsed;
                    this.btnDisconnect.Visibility = System.Windows.Visibility.Visible;
                    this.txbServerName.Text = server_name;
                    this.txbServerName.IsEnabled = false;
                    this.txbUserName.Text = user_name;
                    this.pwbPassword.Password = password;
                }
            }
            else
            {
                this.txbUserName.IsEnabled = true;
                this.pwbPassword.IsEnabled = true;
                this.txbUserName.Text = user_name;
                this.pwbPassword.Password = password;

                if (authentication == 0 && flag_check_sql_server_connected == true)
                {
                    this.btnConnect.Visibility = System.Windows.Visibility.Visible;
                    this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
                    this.txbServerName.IsEnabled = true;
                    this.txbServerName.Text = server_name;
                    this.txbUserName.IsEnabled = true;
                    this.pwbPassword.IsEnabled = true;
                    this.txbUserName.Text = user_name;
                    this.pwbPassword.Password = password;
                }

                if (authentication == 1 && flag_check_sql_server_connected == true)
                {
                    this.btnConnect.Visibility = System.Windows.Visibility.Collapsed;
                    this.btnDisconnect.Visibility = System.Windows.Visibility.Visible;
                    this.txbServerName.IsEnabled = false;
                    this.txbServerName.Text = server_name;
                    this.txbUserName.IsEnabled = false;
                    this.pwbPassword.IsEnabled = false;
                    this.txbUserName.Text = user_name;
                    this.pwbPassword.Password = password;
                }
            }
        }

        //Connecting_Delegate
        private void Connecting_Delegate(bool result)
        {
            if (result)
            {
                if (thread_connect != null && thread_connect.ThreadState == ThreadState.Running)
                    new Thread(() =>
                    {
                        thread_connect.Abort();
                        this.Dispatcher.Invoke((Action)(() => { connecting.Close(); }));
                    }).Start();
            }
        }

        //btnConnect_Click
        private Connecting connecting;
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            //check empty
            if (CheckEmpty() == true)
            {
                this.btnConnect.IsEnabled = true;
                return;
            }

            flag_check_sql_server_connected = false;

            if (thread_connect != null && thread_connect.ThreadState == ThreadState.Running) { }
            else
            {
                this.tblNotificationSQLServer.Text = "";

                server_name = this.txbServerName.Text.Trim().ToString();
                this.txbServerName.IsEnabled = false;

                authentication = this.cboAuthentication.SelectedIndex;
                this.cboAuthentication.IsEnabled = false;

                user_name = this.txbUserName.Text.Trim().ToString();
                this.txbUserName.IsEnabled = false;

                password = this.pwbPassword.Password.Trim().ToString();
                this.pwbPassword.IsEnabled = false;

                this.btnConnect.IsEnabled = false;

                new Thread(() =>
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        connecting = new Connecting();
                        connecting.connecting_delegate += Connecting_Delegate;
                        connecting.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                        connecting.ShowInTaskbar = false;
                        connecting.ShowDialog();
                    }));
                }).Start();

                thread_connect = new Thread(() =>
                {
                    try
                    {
                        //connect success
                        string result = CheckConnect();
                        if (result == "successfully")
                        {
                            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(current_directory + @"\_sqltype");
                            streamwriter.WriteLine("sqltype:sqlserver");
                            streamwriter.WriteLine("server:" + server_name);
                            streamwriter.WriteLine("authentication:" + authentication);
                            streamwriter.WriteLine("id:" + user_name);
                            streamwriter.WriteLine("password:" + password);
                            streamwriter.Close();
                            StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\_sqltype", current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                            using (System.IO.StreamWriter file = System.IO.File.CreateText(current_directory + @"\appSettings.json"))
                            {
                                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                                serializer.Serialize(file, StaticClass.GeneralClass.app_settings);
                            }
                            //restart app
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                StaticClass.GeneralClass.app_settings["appIsRestart"] = "True";
                                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                                Application.Current.Shutdown();
                            }));
                        }

                        //connect failed
                        else
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                this.tblNotificationSQLServer.Text = result;
                                this.txbServerName.IsEnabled = true;
                                this.cboAuthentication.IsEnabled = true;
                            }));

                            if (authentication == 1)
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.txbUserName.IsEnabled = true;
                                    this.pwbPassword.IsEnabled = true;
                                }));
                            }

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                this.btnConnect.Visibility = System.Windows.Visibility.Visible;
                                this.btnConnect.IsEnabled = true;
                                this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
                                this.btnDisconnect.IsEnabled = true;
                            }));
                        }

                        this.Dispatcher.Invoke((Action)(() => { connecting.Close(); }));
                    }
                    catch (Exception)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.tblNotificationSQLServer.Text = FindResource("connect_failed").ToString();
                            this.txbServerName.IsEnabled = true;
                            this.cboAuthentication.IsEnabled = true;
                        }));

                        if (authentication == 1)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                this.txbUserName.IsEnabled = true;
                                this.pwbPassword.IsEnabled = true;
                            }));
                        }

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.btnConnect.Visibility = System.Windows.Visibility.Visible;
                            this.btnConnect.IsEnabled = true;
                            this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
                            this.btnDisconnect.IsEnabled = true;

                            connecting.Close();
                        }));
                    }
                });
                thread_connect.Start();
            }
            //}
        }

        //btnDisconnect_Click
        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (thread_disconnect != null && thread_disconnect.ThreadState == ThreadState.Running) { }
            else
            {
                thread_disconnect = new Thread(() =>
                {
                    this.cboAuthentication.Dispatcher.Invoke((Action)(() => { this.cboAuthentication.IsEnabled = false; }));
                    this.btnDisconnect.Dispatcher.Invoke((Action)(() => { this.btnDisconnect.IsEnabled = false; }));

                    System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(current_directory + @"\_sqltype");
                    streamwriter.WriteLine("sqltype:sqlserver");
                    streamwriter.WriteLine("server:" + "");
                    streamwriter.WriteLine("authentication:" + authentication);
                    streamwriter.WriteLine("id:" + "");
                    streamwriter.WriteLine("password:" + "");
                    streamwriter.Close();
                    StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\_sqltype", current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);

                    flag_check_connected = false;
                    flag_check_sql_server_connected = false;
                    sqltype = "sqlserver";
                    server_name = "";
                    user_name = "";
                    password = "";

                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
                        this.btnDisconnect.IsEnabled = true;

                        this.btnConnect.Visibility = System.Windows.Visibility.Visible;
                        this.IsEnabled = true;

                        this.txbServerName.IsEnabled = true;
                        this.txbServerName.Text = "";

                        this.cboAuthentication.IsEnabled = true;

                        this.txbUserName.Text = "";
                        this.pwbPassword.Password = "";
                    }));

                    if (authentication == 1)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.txbUserName.IsEnabled = true;
                            this.pwbPassword.IsEnabled = true;
                        }));
                    }

                    this.Dispatcher.Invoke((Action)(() => { this.tblNotificationSQLServer.Text = ""; }));
                });

                thread_disconnect.Start();
            }
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //abort thread connect
            if (thread_connect != null && thread_connect.ThreadState == ThreadState.Running)
            {
                new Thread(() =>
                {
                    thread_connect.Abort();

                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.txbServerName.IsEnabled = true;
                        this.cboAuthentication.IsEnabled = true;
                    }));

                    if (authentication == 1)
                    {
                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            this.txbUserName.IsEnabled = true;
                            this.pwbPassword.IsEnabled = true;
                        }));
                    }
                    this.Dispatcher.Invoke((Action)(() => { this.btnConnect.IsEnabled = true; }));
                }).Start();
            }

            //abort thread disconnect
            if (thread_disconnect != null && thread_disconnect.ThreadState == ThreadState.Running)
            {
                new Thread(() =>
                {
                    thread_disconnect.Abort();

                    this.Dispatcher.Invoke((Action)(() => 
                    {
                        this.txbServerName.IsEnabled = false;
                        this.cboAuthentication.IsEnabled = true;
                        this.txbUserName.IsEnabled = false;
                        this.pwbPassword.IsEnabled = false;
                        this.btnDisconnect.IsEnabled = true;
                    }));

                }).Start();
            }
        }

        private BBCodeBlock bbcode_block = new BBCodeBlock();
        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_loaded == true)
            {
                flag_loaded = false;

                if (thread_connect != null && thread_connect.ThreadState == ThreadState.Running) { }
                else
                {
                    //if rdosqlserver is checked and current database type is sqlite
                    if (rdoSqLServer.IsChecked == true && flag_check_connected == true && sqltype == "sqlite")
                    {
                        var m = (ModernWindow)Application.Current.MainWindow;
                        m.ContentSource = new Uri(@"/Pages/Setting/Setting.xaml", UriKind.Relative);
                        bbcode_block.LinkNavigator.Navigate(new Uri(@"/Pages/Setting/AppSetting.xaml", UriKind.Relative), this);

                        ModernDialog md = new ModernDialog();
                        md.Buttons = new Button[] { md.YesButton, };
                        md.Title = FindResource("notification").ToString();
                        md.Content = FindResource("failed_connect_sqlserver").ToString();
                        md.YesButton.Content = FindResource("ok").ToString();
                        md.YesButton.Focus();
                        if (md.ShowDialog().Value == true)
                        {
                            flag_loaded = true;
                            this.spConnectServer.Visibility = System.Windows.Visibility.Collapsed;
                            this.rdoSQLite.Checked -= rdoSQLite_Checked;
                            rdoSQLite.IsChecked = true;
                            this.rdoSQLite.Checked += new RoutedEventHandler(rdoSQLite_Checked);
                        }
                        else
                            flag_loaded = true;

                        return;
                    }

                    //if rdosqlserver is checked and current database type is sqlserver
                    if (rdoSqLServer.IsChecked == true && flag_check_connected == false && sqltype == "sqlserver")
                    {
                        var m = (ModernWindow)Application.Current.MainWindow;
                        m.ContentSource = new Uri(@"/Pages/Setting/Setting.xaml", UriKind.Relative);
                        bbcode_block.LinkNavigator.Navigate(new Uri(@"/Pages/Setting/AppSetting.xaml", UriKind.Relative), this);

                        ModernDialog md = new ModernDialog();
                        md.Buttons = new Button[] { md.YesButton };
                        md.Title = FindResource("notification").ToString();
                        md.Content = FindResource("failed_connect_sqlserver").ToString();
                        md.YesButton.Content = FindResource("ok").ToString();
                        md.YesButton.Focus();
                        if (md.ShowDialog().Value == true)
                        {
                            flag_loaded = true;

                            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(current_directory + @"\_sqltype");
                            streamwriter.WriteLine("sqltype:sqlite");
                            streamwriter.WriteLine("server:" + server_name);
                            streamwriter.WriteLine("authentication:" + authentication);
                            streamwriter.WriteLine("id:" + user_name);
                            streamwriter.WriteLine("password:" + password);
                            streamwriter.Close();
                            StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\_sqltype", current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);

                            //restart app
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                StaticClass.GeneralClass.app_settings["appIsRestart"] = "True";
                                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                                Application.Current.Shutdown();
                            }));
                        }
                        else
                            flag_loaded = true;

                        return;
                    }
                }

            }
            else
                flag_loaded = true;
        }

        //bdOff_MouseDown
        private void bdOff_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StaticClass.GeneralClass.app_settings["outOfStock"].ToString()=="True")
            {
                Border bd_off = (Border)UCShowStocks.FindName("bdOff");
                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                tbl_off.Foreground = System.Windows.Media.Brushes.White;

                Border bd_on = (Border)UCShowStocks.FindName("bdOn");
                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                tbl_on.Foreground = System.Windows.Media.Brushes.Silver;

                StaticClass.GeneralClass.flag_change_out_of_stock = true;
                StaticClass.GeneralClass.app_settings["outOfStock"] = "False";
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            }
        }

        //bdOn_MouseDown
        private void bdOn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StaticClass.GeneralClass.app_settings["outOfStock"].ToString()=="False")
            {
                Border bd_off = (Border)UCShowStocks.FindName("bdOff");
                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                tbl_off.Foreground = System.Windows.Media.Brushes.Silver;


                Border bd_on = (Border)UCShowStocks.FindName("bdOn");
                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                tbl_on.Foreground = System.Windows.Media.Brushes.White;

                StaticClass.GeneralClass.flag_change_out_of_stock = true;
                StaticClass.GeneralClass.app_settings["outOfStock"] = "True";
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            }
        }


        //bdOffSaveLogs_MouseDown
        private void bdOffSaveLogs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StaticClass.GeneralClass.app_settings["isSaveLogs"].ToString()=="True")
            {
                Border bd_off = (Border)UCSaveLogs.FindName("bdOff");
                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                tbl_off.Foreground = System.Windows.Media.Brushes.White;

                Border bd_on = (Border)UCSaveLogs.FindName("bdOn");
                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                tbl_on.Foreground = System.Windows.Media.Brushes.Silver;

                StaticClass.GeneralClass.app_settings["isSaveLogs"] = "False";
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            }
        }

        //bdOnSaveLogs_MouseDown
        private void bdOnSaveLogs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StaticClass.GeneralClass.app_settings["isSaveLogs"].ToString()=="False")
            {
                Border bd_off = (Border)UCSaveLogs.FindName("bdOff");
                bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
                TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
                tbl_off.Foreground = System.Windows.Media.Brushes.Silver;


                Border bd_on = (Border)UCSaveLogs.FindName("bdOn");
                bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
                TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
                tbl_on.Foreground = System.Windows.Media.Brushes.White;

                StaticClass.GeneralClass.app_settings["isSaveLogs"] = "True";
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            }
        }

        //rdoSQLite_Checked
        private void rdoSQLite_Checked(object sender, RoutedEventArgs e)
        {
            //if rdosqlserver is checked and current database type is sqlite
            if (flag_check_connected == true && sqltype == "sqlite")
                spConnectServer.Visibility = System.Windows.Visibility.Collapsed;
            else
            {
                if (thread_sqlite != null && thread_sqlite.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_sqlite = new Thread(() =>
                    {
                        try
                        {
                            this.spConnectServer.Dispatcher.Invoke((Action)(() => { this.spConnectServer.Visibility = System.Windows.Visibility.Collapsed; }));

                            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(current_directory + @"\_sqltype");
                            streamwriter.WriteLine("sqltype:sqlite");
                            streamwriter.WriteLine("server:" + server_name);
                            streamwriter.WriteLine("authentication:" + authentication);
                            streamwriter.WriteLine("id:" + user_name);
                            streamwriter.WriteLine("password:" + password);
                            streamwriter.Close();
                            StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\_sqltype", current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                            using (System.IO.StreamWriter file = System.IO.File.CreateText(current_directory + @"\appSettings.json"))
                                {
                                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                                serializer.Serialize(file, StaticClass.GeneralClass.app_settings);
                                }
                            //restart app
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                StaticClass.GeneralClass.app_settings["appIsRestart"] = "True";
                                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                                Application.Current.Shutdown();
                            }));
                        }
                        catch (Exception ex)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                ModernDialog md = new ModernDialog();
                                md.Buttons = new Button[] { md.CloseButton, };
                                md.CloseButton.Content = FindResource("close").ToString();
                                md.Title = FindResource("notification").ToString();
                                md.Content = ex.Message + "@@";
                                md.ShowDialog();
                            }));
                        }
                    });
                    thread_sqlite.Start();
                }
            }
        }

        //rdoSqLServer_Checked
        private void rdoSqLServer_Checked(object sender, RoutedEventArgs e)
        {
            //remove checked event for rdoSQLite
            //this.rdoSQLite.Checked -= rdoSQLite_Checked;

            this.spConnectServer.Visibility = System.Windows.Visibility.Visible;
            this.tblNotificationSQLServer.Text = "";
            this.txbServerName.IsEnabled = true;
            this.txbServerName.Text = server_name;
            this.cboAuthentication.IsEnabled = true;
            if (this.cboAuthentication.SelectedIndex == 0)
            {
                this.txbUserName.IsEnabled = false;
                this.txbUserName.Text = "";
                this.pwbPassword.IsEnabled = false;
                this.pwbPassword.Password = "";
            }
            else
            {
                this.txbUserName.IsEnabled = true;
                this.txbUserName.Text = user_name;
                this.pwbPassword.IsEnabled = true;
                this.pwbPassword.Password = password;
            }
            this.btnDisconnect.Visibility = System.Windows.Visibility.Collapsed;
            this.btnDisconnect.IsEnabled = false;
            this.btnConnect.Visibility = System.Windows.Visibility.Visible;
            this.btnConnect.IsEnabled = true;
            //this.btnCancel.IsEnabled = false;
        }

        //CheckEmpty
        private bool CheckEmpty()
        {
            if (txbServerName.Text.Trim().ToString() == "")
            {
                this.tblNotificationSQLServer.Text = FindResource("servername_null").ToString();
                this.txbServerName.IsEnabled = true;
                this.txbUserName.IsEnabled = false;
                this.pwbPassword.IsEnabled = false;
                this.txbServerName.Focus();
                return true;
            }

            //if authenticaion is sql server
            if (cboAuthentication.SelectedIndex == 1)
            {
                if (txbUserName.Text.Trim().ToString() == "")
                {
                    this.tblNotificationSQLServer.Text = FindResource("username_null").ToString();
                    this.txbUserName.IsEnabled = true;
                    this.txbUserName.Focus();
                    return true;
                }

                if (pwbPassword.Password.Trim().ToString() == "")
                {
                    this.tblNotificationSQLServer.Text = FindResource("password_null").ToString();
                    this.pwbPassword.IsEnabled = true;
                    this.pwbPassword.Focus();
                    return true;
                }
            }
            return false;
        }

        //CheckConnect
        private string CheckConnect()
        {
            if(server_name != "")
            {
                string str_connect_server = ConnectionDB.CheckConnectionServer(server_name, user_name, password, authentication);
                if(str_connect_server == "successfully")
                {
                    //check database exist
                    if(ConnectionDB.CheckDatabaseExists(server_name, user_name, password, authentication))
                    {
                        //check connectiong database
                        string str_connect_database = ConnectionDB.CheckConnectionDatabase(sqltype, server_name, user_name, password, authentication);
                        if (str_connect_database == "successfully")
                            return "successfully";
                        return str_connect_database;
                    }
                    else
                    {
                        if(CreateDatabase())
                            return "successfully";
                    }

                    return FindResource("database_was_not_found").ToString();
                }//end check connection to server
                return str_connect_server;
            }

            return FindResource("servername_null").ToString();
        }


        //CreateDatabase
        private bool CreateDatabase()
        {
            bool flag_check = false;
            try
            {
                string create_database_string = "CREATE DATABASE [CheckOut];";
                //+" CONTAINMENT = NONE" +
                //" ON  PRIMARY " +
                //@" ( NAME = N'CheckOut', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\CheckOut.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )" +
                //" LOG ON " +
                //@" ( NAME = N'CheckOut_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\CheckOut_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)";

                string connection_string = "";

                //if authentication is windows
                if (authentication == 0)
                    connection_string = "server = " + server_name + "; Trusted_Connection = true;";

                //if authentication is sql server
                else
                    connection_string = "server = " + server_name + "; user id = " + user_name + "; password = " + password + "; integrated security = true;";

                SqlConnection sqlcon = new SqlConnection();
                sqlcon.ConnectionString = connection_string;
                sqlcon.Open();
                SqlCommand sqlcom = new SqlCommand(create_database_string, sqlcon);
                sqlcom.ExecuteNonQuery();
                System.IO.StreamReader streamreader = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\script3", StaticClass.GeneralClass.key_register_general);
                string create_table_string = streamreader.ReadToEnd();
                if (streamreader != null)
                    streamreader.Close();

                SqlCommand sqlcom_create_table = new SqlCommand(create_table_string, sqlcon);
                sqlcom_create_table.ExecuteNonQuery();
                sqlcon.Close();

                flag_check = true;
            }
            catch (Exception)
            {
                flag_check = false;
            }
            return flag_check;
        }

        //cboDecimalSeparator_SelectionChanged
        private int decimal_separator_root = Convert.ToInt16(StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString());
        private void cboDecimalSeparator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboDecimalSeparator.SelectedIndex == 0)
                StaticClass.GeneralClass.app_settings["decimalSeparator"] = "0";
            else
                if (cboDecimalSeparator.SelectedIndex == 1)
                StaticClass.GeneralClass.app_settings["decimalSeparator"] = "1";
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);

            if (decimal_separator_root != Convert.ToInt16(StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString()))
                StaticClass.GeneralClass.flag_add_edit_setting_general = true;
        }

        //hplViewLogs_Click
        private void hplViewLogs_Click(object sender, RoutedEventArgs e)
        {
            Logs page = new Logs();
            var m  = Application.Current.MainWindow;
            page.Owner = m;
            page.ShowInTaskbar = false;
            page.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (m.RenderSize.Width > StaticClass.GeneralClass.width_screen_general)
                page.Width = StaticClass.GeneralClass.width_screen_general * 50 / 100;
            else
                page.Width = m.RenderSize.Width * 50 / 100;

            if (m.RenderSize.Height > StaticClass.GeneralClass.height_screen_working_general)
                page.Height = StaticClass.GeneralClass.height_screen_working_general;
            else
                page.Height = m.RenderSize.Height;

            page.ShowDialog();
        }

        private void muiBtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tblNotification.Text = "";
                tblNotification.Visibility = Visibility.Hidden;
                tblAlertSuccess.Visibility = Visibility.Hidden;
                if (txt_store_name.Text.Trim().ToString() == "")
                {
                    tblNotification.Text = FindResource("store_name_null").ToString();
                    tblNotification.Visibility = Visibility.Visible;
                    txt_store_name.Focus();
                    return;
                }

                if (txt_store_address.Text.Trim().ToString() == "")
                {
                    tblNotification.Text = FindResource("shop_address_null").ToString();
                    tblNotification.Visibility = Visibility.Visible;
                    txt_store_address.Focus();
                    return;
                }

                if (txt_store_phone.Text.Trim().ToString() == "")
                {
                    tblNotification.Text = FindResource("phone_null").ToString();
                    tblNotification.Visibility = Visibility.Visible;
                    txt_store_phone.Focus();
                    return;
                }

                //save sote info
                StaticClass.GeneralClass.app_settings["storeName"] = txt_store_name.Text.Trim();
                StaticClass.GeneralClass.app_settings["storeAddress"] = txt_store_address.Text.Trim();
                StaticClass.GeneralClass.app_settings["storePhone"] = txt_store_phone.Text.Trim();
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                tblNotification.Visibility = Visibility.Hidden;
                tblAlertSuccess.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.stp_storeInfo.Dispatcher.Invoke((Action)(() => {
                tblAlertSuccess.Visibility = Visibility.Hidden;
                tblNotification.Visibility = Visibility.Hidden;
            }));
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton _rd = sender as RadioButton;
            if(StaticClass.GeneralClass.app_settings["typeBackup"].ToString() != Convert.ToString(_rd.Uid.ToString()))
            {
                StaticClass.GeneralClass.app_settings["typeBackup"] = Convert.ToString(_rd.Uid.ToString());
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            }
        }

        private void muiBtnSaveDateTime_Click(object sender, RoutedEventArgs e)
        {
            string _appDate = StaticClass.GeneralClass.app_settings["dateFormat"].ToString();
            string _appTime = StaticClass.GeneralClass.app_settings["timeFormat"].ToString();
            tblAlertSuccessDateTime.Visibility = Visibility.Hidden;
            tblNotificationDateTime.Visibility = Visibility.Hidden;

            StaticClass.GeneralClass.app_settings["dateFormat"] = cb_date_Format.SelectedValue.ToString();
            StaticClass.GeneralClass.app_settings["timeFormat"] = cb_time_Format.SelectedValue.ToString();
            if (Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings) > 0)
            {
                tblAlertSuccessDateTime.Visibility = Visibility.Visible;
            }
            else
            {
                tblNotificationDateTime.Visibility = Visibility.Visible;
                StaticClass.GeneralClass.app_settings["timeFormat"] = _appTime;
                StaticClass.GeneralClass.app_settings["dateFormat"] = _appDate;
            }
        }
        private void bdOffLogin2Sell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border bd_off = (Border)UCLogin2Sell.FindName("bdOff");
            bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
            TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
            tbl_off.Foreground = System.Windows.Media.Brushes.White;

            Border bd_on = (Border)UCLogin2Sell.FindName("bdOn");
            bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
            TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
            tbl_on.Foreground = System.Windows.Media.Brushes.Silver;

            StaticClass.GeneralClass.app_settings["isLoginToSale"] = "0";
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
        }
        private void bdOnLogin2Sell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border bd_off = (Border)UCLogin2Sell.FindName("bdOff");
            bd_off.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
            TextBlock tbl_off = (TextBlock)bd_off.FindName("tblOff");
            tbl_off.Foreground = System.Windows.Media.Brushes.Silver;

            Border bd_on = (Border)UCLogin2Sell.FindName("bdOn");
            bd_on.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF004D40");
            TextBlock tbl_on = (TextBlock)bd_on.FindName("tblOn");
            tbl_on.Foreground = System.Windows.Media.Brushes.White;

            StaticClass.GeneralClass.app_settings["isLoginToSale"] = "1";
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
        }
    }
    
    public class DateFormat
    {
        public dateFormat DateValue { get; set; }
        public string DateDisplay { get; set; }
    }
    public class TimeFormat
    {
        public timeFormat TimeValue { get; set; }
        public string TimeDisplay { get; set; }
    }
}
