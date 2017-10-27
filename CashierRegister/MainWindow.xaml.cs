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
using Microsoft.Win32;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;
using FirstFloor.ModernUI.Presentation;
using CashierRegisterDAL;
using System.Threading;
using System.IO;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace CashierRegister
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private App app = Application.Current as App;

        //current directory
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        //accent color
        private System.Windows.Media.Color accent_color;

        //create link logout
        private Link l_logout = new Link { DisplayName = "", Source = new Uri(@"/Pages/Home/Logout.xaml", UriKind.Relative), };

        //create link login
        private Link l_login = new Link { DisplayName = "", Source = new Uri(@"/Pages/Home/Login.xaml", UriKind.Relative), };

        //security
        Pages.CopyRight.SecurityManager scm = new Pages.CopyRight.SecurityManager();

        //database connect info
        private string sqltype = "";
        private string server = "";
        private int authentication = 0;
        private string id = "";
        private string password = "";
        private bool flag_check_loaded = false;

        //MainWindow
        public MainWindow()
        {
            //Properties.Settings.Default.Reset();
            app.mainWindow = this;
            //how to check if another instance of the application is running
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
                System.Diagnostics.Process.GetCurrentProcess().Kill();

            //check database type
            if (!System.IO.File.Exists(current_directory + @"\sqltype"))
            {
                System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(current_directory + @"\_sqltype");
                streamwriter.WriteLine("sqltype:sqlite");
                streamwriter.WriteLine("server:" + server);
                streamwriter.WriteLine("authentication:" + authentication);
                streamwriter.WriteLine("id:" + id);
                streamwriter.WriteLine("password:" + password);
                streamwriter.Close();
                StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\_sqltype", current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
            }

            if (System.IO.File.Exists(current_directory + @"\sqltype"))
            {
                System.IO.StreamReader stream_sqltype = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                sqltype = stream_sqltype.ReadLine().Split(':')[1].ToString();
                server = stream_sqltype.ReadLine().Split(':')[1].ToString();
                Int32.TryParse(stream_sqltype.ReadLine().Split(':')[1].ToString(), out authentication);
                id = stream_sqltype.ReadLine().Split(':')[1].ToString();
                password = stream_sqltype.ReadLine().Split(':')[1].ToString();

                if (stream_sqltype != null)
                    stream_sqltype.Close();

                if (sqltype == "sqlserver")
                {
                    //set sql type
                    StaticClass.GeneralClass.flag_database_type_general = true;

                    OpenConnectionForm();
                }
            }
            ConnectionDB.ConnectionDBInitialize();

            //check version app
            try
            {
                double _version = Convert.ToDouble(Properties.Settings.Default.currentVersion.Replace("2.1.", "2.1"));
                if (_version <= 2.15)
                {
                    FuncUpgradeDatabase();
                    Properties.Settings.Default.currentVersion = "2.1.6";
                    Properties.Settings.Default.Save();
                }
                else FuncUpgradeDatabase();
            }
            catch
            {
                FuncUpgradeDatabase();
            }
            if (File.Exists(current_directory + @"\appSettings.json"))
            {
                System.Collections.Hashtable _hash = null;
                using (StreamReader file = new StreamReader(current_directory + @"\appSettings.json", System.Text.Encoding.UTF8, true))
                {
                    _hash = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Hashtable>(file.ReadLine().ToString());
                    Model.UpgradeDatabase.updateAppSetting(_hash);
                    file.Close();
                }
                if (_hash != null)
                {
                    File.Delete(current_directory + @"\appSettings.json");
                }
            }
            DataTable _appSetting = Model.UpgradeDatabase.getAppSetting();
            if (_appSetting.Rows.Count > 0)
            {
                foreach (DataRow _dr in _appSetting.Rows)
                {
                    StaticClass.GeneralClass.app_settings.Add(_dr["SettingKey"].ToString(), _dr["SettingValue"]);
                }
            }
            //load app language
            StaticClass.GeneralClass.dict_language_current.Source = new Uri("..\\Languages\\" + StaticClass.GeneralClass.app_settings["language"].ToString() + ".xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(StaticClass.GeneralClass.dict_language_current);

            InitializeComponent();

            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.commadddddMMMMyyyy.ToString(), "dddd, d MMMM, yyyy");
            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.commaddddMMMMdyyyy.ToString(), "dddd, MMMM d, yyyy");
            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.commadMMMMyyyy.ToString(), "d MMMM, yyyy");
            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.commaMMMMdyyyy.ToString(), "MMMM d, yyyy");
            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.commadMMMyyyy.ToString(), "d MMM, yyyy");
            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.commaMMMdyyyy.ToString(), "MMM d, yyyy");
            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.slashddMMMyyyy.ToString(), "dd-MMM-yyyy");
            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.slashMdyyyy.ToString(), "M/d/yyyy");
            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.slashMMddyyyy.ToString(), "MM/dd/yyyy");
            StaticClass.GeneralClass.dateFromatSettings.Add(dateFormat.slashyyyyMMdd.ToString(), "yyyy/MM/dd");

            StaticClass.GeneralClass.timeFromatSettings.Add(timeFormat.None.ToString(), "null");
            StaticClass.GeneralClass.timeFromatSettings.Add(timeFormat.HHmm.ToString(), "HH:mm");
            StaticClass.GeneralClass.timeFromatSettings.Add(timeFormat.HHmmss.ToString(), "HH:mm:ss");
            StaticClass.GeneralClass.timeFromatSettings.Add(timeFormat.hhmmsstt.ToString(), "hh:mm:ss tt");
            StaticClass.GeneralClass.timeFromatSettings.Add(timeFormat.hhmmtt.ToString(), "hh:mm tt");
            StaticClass.GeneralClass.timeFromatSettings.Add(timeFormat.Hmm.ToString(), ":H:mm");
            StaticClass.GeneralClass.timeFromatSettings.Add(timeFormat.Hmmss.ToString(), "H:mm:ss");
            StaticClass.GeneralClass.timeFromatSettings.Add(timeFormat.hmmsstt.ToString(), "h:mm:ss tt");
            StaticClass.GeneralClass.timeFromatSettings.Add(timeFormat.hmmtt.ToString(), "h:mm tt");

            //create app menu
            app.linkGroupHome = new LinkGroup() { DisplayName = Application.Current.FindResource("lg_home").ToString()};
            app.linkGroupReport = new LinkGroup() { DisplayName = Application.Current.FindResource("lg_report").ToString() };
            app.linkGroupReport.Links.Add(new Link() { DisplayName = "", Source = new Uri(@"/Pages/Report/Report.xaml", UriKind.Relative) });

            app.linkGroupSetting = new LinkGroup() { DisplayName = Application.Current.FindResource("lg_setting").ToString() };
            app.linkGroupSetting.Links.Add(new Link() { DisplayName = "", Source = new Uri(@"/Pages/Setting/Setting.xaml", UriKind.Relative) });

            app.linkGroupChart = new LinkGroup() { DisplayName = Application.Current.FindResource("lg_chart").ToString() };
            app.linkGroupChart.Links.Add(new Link() { DisplayName = "", Source = new Uri(@"/Pages/Chart/RevenueProfitChart.xaml", UriKind.Relative) });

            app.linkGroupStatistic = new LinkGroup() { DisplayName = Application.Current.FindResource("lg_statistic").ToString() };
            app.linkGroupStatistic.Links.Add(new Link() { DisplayName = "", Source = new Uri(@"/Pages/Statistic/Home.xaml", UriKind.Relative) });

            app.linkGroupOption = new LinkGroup() { DisplayName = Application.Current.FindResource("lg_option").ToString() };
            app.linkGroupLogInOut = new LinkGroup();
            app.linkLogin = new Link() { DisplayName = Application.Current.FindResource("l_login").ToString(), Source = new Uri(@"/Pages/Home/Login.xaml", UriKind.Relative) };
            app.linkLogout = new Link() { DisplayName = Application.Current.FindResource("l_logout").ToString(), Source = new Uri(@"/Pages/Home/Logout.xaml", UriKind.Relative) };
            app.linkAccount = new Link() { DisplayName = Application.Current.FindResource("hi").ToString() + " " + StaticClass.GeneralClass.name_user_general, Source = new Uri(@"/Pages/Setting/Account.xaml", UriKind.Relative) };
            this.TitleLinks.Add(app.linkLogin);

            //create link for setting page
            new Thread(() =>
            {
                try
                {
                    app.linkCategory = new Link() { DisplayName = FindResource("category").ToString(), Source = new Uri(@"/Pages/Setting/Product.xaml", UriKind.Relative) };
                    app.linkCurrency = new Link() { DisplayName = FindResource("currency").ToString(), Source = new Uri(@"/Pages/Setting/Currency.xaml", UriKind.Relative) };
                    app.linkCustomer = new Link() { DisplayName = FindResource("customer").ToString(), Source = new Uri(@"/Pages/Setting/Customer.xaml", UriKind.Relative) };
                    app.linkSalesperson = new Link() { DisplayName = FindResource("salesperson").ToString(), Source = new Uri(@"/Pages/Setting/Salesperson.xaml", UriKind.Relative) };
                    app.linkPayment = new Link() { DisplayName = FindResource("payment").ToString(), Source = new Uri(@"/Pages/Setting/Payment.xaml", UriKind.Relative) };
                    app.linkUser = new Link() { DisplayName = FindResource("user").ToString(), Source = new Uri(@"/Pages/Setting/User.xaml", UriKind.Relative) };
                    app.linkBackup = new Link() { DisplayName = FindResource("backup_restore").ToString(), Source = new Uri(@"/Pages/Setting/BackupDB.xaml", UriKind.Relative) };
                    app.linkAppSetting = new Link() { DisplayName = FindResource("app_setting").ToString(), Source = new Uri(@"/Pages/Setting/AppSetting.xaml", UriKind.Relative) };
                    app.lnkGiftCard = new Link() { DisplayName = FindResource("gift_card").ToString(), Source = new Uri(@"Views/Setting/GiftCard.xaml", UriKind.Relative) };
                }
                catch(Exception ex)
                {
                    MessageBox.Show(FindResource("error").ToString() + ": " + ex.Message, FindResource("notification").ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }).Start();

            //load theme color
            accent_color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(StaticClass.GeneralClass.app_settings["accentColor"].ToString());
            AppearanceManager.Current.AccentColor = accent_color;
            
            CheckRegisterInto();
        }

        //OpenConnectionForm
        private void OpenConnectionForm()
        {
            Pages.ConnectServer page = new Pages.ConnectServer();
            page.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            page.txbServerName.Focus();
            page.txbServerName.Text = server;
            if (authentication == 0)
                page.cboAuthentication.SelectedIndex = 0;
            else
                page.cboAuthentication.SelectedIndex = 1;

            page.txbUserName.Text = id;
            page.pwbPassword.Password = password;
            page.Height = 375;
            page.Width = 600;
            page.ShowDialog();
        }

        //CheckRegisterInto
        private void CheckRegisterInto()
        {
            try
            {
                if (!StaticClass.GeneralClass.isFullVersion)
                {
                    string register_status = (string)Registry.GetValue(StaticClass.GeneralClass.keyname_register_general, StaticClass.GeneralClass.valuename_register_general, "FALSE");
                    if (register_status != null && register_status.Trim().ToUpper() == "TRUE")
                    {
                        //check serial number
                        string[] register_info = scm.DecryptFile("EncryptReg", StaticClass.GeneralClass.key_register_general);
                        string softname = "Cash Register";
                        string customer_email = register_info[0];
                        string license_key = register_info[1];
                        string softname_softversion_customername = softname + "_" + StaticClass.GeneralClass.software_version + "_" + customer_email;

                        if (scm.CheckSerialNumber(softname_softversion_customername, license_key))
                        {
                            StaticClass.GeneralClass.youremail_registered_general = customer_email;
                            Application.Current.Resources["cash_register"] = "Cash Register Pro";
                        }
                    }
                }
                else
                    Application.Current.Resources["cash_register"] = "Cash Register Pro";

                CreateLinkGroupHome();
                CreateLinkGroupOptions();
                CreateLinkGroupLoginLogout();
                this.ContentSource = new Uri(@"/Pages/Home/Home.xaml", UriKind.Relative);
            }
            catch (Exception)
            {
                CreateLinkGroupHome();
                CreateLinkGroupOptions();
                CreateLinkGroupLoginLogout();
                this.ContentSource = new Uri(@"/Pages/Home/Home.xaml", UriKind.Relative);
            }
        }

        //ModernWindow_Loaded
        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //get size screen and taskbar
            StaticClass.GeneralClass.height_screen_working_general = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            StaticClass.GeneralClass.width_screen_general = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            StaticClass.GeneralClass.height_taskbar_general = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Bottom - System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Bottom;
            DataInitialize();
            if (StaticClass.GeneralClass.app_settings["typeBackup"].ToString() == "1" && StaticClass.GeneralClass.app_settings["appIsRestart"].ToString()=="False")
            {
                string _rsBackup = _backupDatabase("Loaded");
                if ("Success" != _rsBackup)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = Application.Current.FindResource("auto_bk_error").ToString();
                    page.ShowDialog();
                }
            }
        }
        
        //DataInitialize
        private void DataInitialize()
        {
            try
            {
                //initialize for setting
                BUS_tb_Setting bus_tb_setting = new BUS_tb_Setting();
                using (DataTable dt_setting = bus_tb_setting.GetSetting("WHERE Active = 1"))
                {
                    StaticClass.GeneralClass.settingid_setting_general = Convert.ToInt32(dt_setting.Rows[0]["SettingID"].ToString());
                    StaticClass.GeneralClass.currency_setting_general = dt_setting.Rows[0]["Currency"].ToString() + " ";
                    StaticClass.GeneralClass.taxrate_setting_general = Convert.ToDecimal(dt_setting.Rows[0]["TaxRate"].ToString());
                    StaticClass.GeneralClass.active_setting_general = Convert.ToInt32(dt_setting.Rows[0]["Active"].ToString());
                    StaticClass.GeneralClass.version_setting_general = Convert.ToInt32(dt_setting.Rows[0]["Version"].ToString());
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
                
            }
        }

        //ModernWindow_Closed
        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (StaticClass.GeneralClass.app_settings["typeBackup"].ToString() == "2" && StaticClass.GeneralClass.app_settings["appIsRestart"].ToString() == "False")
                {
                    string _rsBackup = _backupDatabase("Closed");
                    if ("Success" != _rsBackup)
                    {
                        Pages.Notification page = new Pages.Notification();
                        page.tblNotification.Text = Application.Current.FindResource("auto_bk_error").ToString();
                        page.ShowDialog();
                    }
                }
            }));
            Environment.Exit(1);
        }

        //CreateLinkGroupHome
        private void CreateLinkGroupHome()
        {
            Link l_home = new Link() { DisplayName = "", Source = new Uri(@"/Pages/Home/Home.xaml", UriKind.Relative), };
            app.linkGroupHome.Links.Add(l_home);
            this.MenuLinkGroups.Add(app.linkGroupHome);
        }

        //CreateLinkGroupOptions
        private void CreateLinkGroupOptions()
        {
            Link l_option = new Link() { DisplayName = "", Source = new Uri(@"/Pages/SettingsPage.xaml", UriKind.Relative), };
            app.linkGroupOption.Links.Add(l_option);
            this.MenuLinkGroups.Add(app.linkGroupOption);
        }

        //CreateLinkGroupLoginLogout
        private void CreateLinkGroupLoginLogout()
        {
            Link l_login = new Link() { DisplayName = "", Source = new Uri(@"/Pages/Home/Login.xaml", UriKind.Relative) };
            app.linkGroupLogInOut.Links.Add(l_login);

            Link l_logout = new Link() { DisplayName = "", Source = new Uri(@"/Pages/Home/Logout.xaml", UriKind.Relative) };
            app.linkGroupLogInOut.Links.Add(l_logout);

            Link l_account = new Link() { DisplayName = "", Source = new Uri(@"/Pages/Setting/Account.xaml", UriKind.Relative) };
            app.linkGroupLogInOut.Links.Add(l_account);

            this.MenuLinkGroups.Add(app.linkGroupLogInOut);
        }

        private string _backupDatabase(string tt)
        {
            System.Diagnostics.Debug.WriteLine("This is backup by " + tt);
            string _info = "Success";
            string _bkFolderNameSQL = string.Format(@"{0}\DBRESSer_Local\{1:M.dd.yyyy HH..mm..ss tt}", Directory.GetCurrentDirectory(), DateTime.Now);
            string _bkFolderNameSQLite = string.Format(@"{0}\DBRES_Local\{1:M.dd.yyyy HH..mm..ss tt}", Directory.GetCurrentDirectory(), DateTime.Now);
            if (StaticClass.GeneralClass.flag_database_type_general)    //sql server
            {
                if (!Directory.Exists(_bkFolderNameSQL))
                {
                    Directory.CreateDirectory(_bkFolderNameSQL);
                }
                string _fileBackup = string.Format(@"{0}\{1}.bak", _bkFolderNameSQL, ConnectionDB.getSqlServerDataBaseName());
                SqlConnection sqlConnect = ConnectionDB.getSQLConnection();
                var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}' WITH NOFORMAT, NOINIT,  NAME = N'data-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", ConnectionDB.getSqlServerDataBaseName(), @_fileBackup);
                try
                {
                    using (var command = new SqlCommand(query, sqlConnect))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    _info = ex.Message;
                }
            }
            else    //sqlite
            {
                if (!Directory.Exists(_bkFolderNameSQLite))
                {
                    Directory.CreateDirectory(_bkFolderNameSQLite);
                }
                string _fileBackup = string.Format(@"{0}\{1}.db", _bkFolderNameSQLite, ConnectionDB.getSqlServerDataBaseName());
                using (var destination = new SQLiteConnection(@"Data Source=" + _fileBackup + @"; Version=3; Password = '"+ ConnectionDB.getKeyDecrypt() + "'"))
                {
                    try
                    {
                        destination.Open();
                        ConnectionDB.getSQLiteConnection().BackupDatabase(destination, "main", "main", -1, null, 0);
                        destination.Close();
                    }
                    catch (SQLiteException ex)
                    {
                        _info = ex.Message;
                    }
                }
            }
            return _info;
        }
        private string FuncUpgradeDatabase()
        {
            string _rs = "Success";
            StringBuilder _strInsert = new StringBuilder();
            _strInsert.Append("insert into [tb_Setting] values ('accentColor', '" + Properties.Settings.Default.accentColor.ToString() + "', '" + Properties.Settings.Default.accentColor.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('language', '" + Properties.Settings.Default.language.ToString() + "', '" + Properties.Settings.Default.language.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('outOfStock', '" + Properties.Settings.Default.outOfStock.ToString() + "', '" + Properties.Settings.Default.outOfStock.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('storeName', '" + Properties.Settings.Default.storeName.ToString() + "', '" + Properties.Settings.Default.storeName.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('storeAddress', '" + Properties.Settings.Default.storeAddress.ToString() + "', '" + Properties.Settings.Default.storeAddress.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('storePhone', '" + Properties.Settings.Default.storePhone.ToString() + "', '" + Properties.Settings.Default.storePhone.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('emailAcc', '" + Properties.Settings.Default.emailAcc.ToString() + "', '" + Properties.Settings.Default.emailAcc.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('emailPass', '" + Properties.Settings.Default.emailPass.ToString() + "', '" + Properties.Settings.Default.emailPass.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('showGuide', '" + Properties.Settings.Default.showGuide.ToString() + "', '" + Properties.Settings.Default.showGuide.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('unit', '" + Properties.Settings.Default.unit.ToString() + "', '" + Properties.Settings.Default.unit.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('unitVal', '" + Properties.Settings.Default.unitVal.ToString() + "', '" + Properties.Settings.Default.unitVal.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('decimalSeparator', '" + Properties.Settings.Default.decimalSeparator.ToString() + "', '" + Properties.Settings.Default.decimalSeparator.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('isSaveLogs', '" + Properties.Settings.Default.isSaveLogs.ToString() + "', '" + Properties.Settings.Default.isSaveLogs.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('fontSize', '" + Properties.Settings.Default.fontSize.ToString() + "', '" + Properties.Settings.Default.fontSize.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('orderDetailWidth', '" + Properties.Settings.Default.orderDetailWidth.ToString() + "', '" + Properties.Settings.Default.orderDetailWidth.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('CalStandard', '" + Properties.Settings.Default.CalStandard.ToString() + "', '" + Properties.Settings.Default.CalStandard.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('currentVersion', '2.1.6', '2.1.6');");
            _strInsert.Append("insert into [tb_Setting] values ('appIsRestart', '" + Properties.Settings.Default.appIsRestart.ToString() + "', '" + Properties.Settings.Default.appIsRestart.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('typeBackup', '" + Properties.Settings.Default.typeBackup.ToString() + "', '" + Properties.Settings.Default.typeBackup.ToString() + "');");
            _strInsert.Append("insert into [tb_Setting] values ('dateFormat', 'slashMdyyyy', 'slashMdyyyy');");
            _strInsert.Append("insert into [tb_Setting] values ('timeFormat', 'hmmtt', 'hmmtt');");
            _strInsert.Append("insert into [tb_Setting] values ('shopWebsite', '', '');");
            _strInsert.Append("insert into [tb_Setting] values ('shopLogo', '', '');");
            _strInsert.Append("insert into [tb_Setting] values ('receiptHeader', '', '');");
            _strInsert.Append("insert into [tb_Setting] values ('receiptFooter', '', '');");
            _strInsert.Append("insert into [tb_Setting] values ('isLoginToSale', '0', '0');");
            if (StaticClass.GeneralClass.flag_database_type_general)
            {
                SqlConnection sqlConnect = ConnectionDB.getSQLConnection();
                SqlTransaction tr = null;
                SqlCommand cmd = null;
                try
                {
                    using (tr = sqlConnect.BeginTransaction())
                    {
                        using (cmd = sqlConnect.CreateCommand())
                        {
                            cmd.Transaction = tr;
                            cmd.CommandText = "CREATE TABLE [tb_Currency]([SettingID] INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1), [Currency] nvarchar(200) NULL, [TaxRate] float NULL, [Active] int NULL, [Version] int NULL);";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "if exists (select column_id from sys.columns where object_id = OBJECT_ID('tb_Currency', 'U') and is_identity = 1) begin SET IDENTITY_INSERT tb_Currency ON; insert into tb_Currency([SettingID],[Currency],[TaxRate],[Active],[Version]) select * from tb_Setting SET IDENTITY_INSERT tb_Currency OFF; end else insert into tb_Currency([SettingID], [Currency], [TaxRate], [Active], [Version]) select * from tb_Setting; ";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "drop table [tb_Setting]";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "CREATE TABLE [tb_Setting]([SettingKey] nvarchar(255) NOT NULL PRIMARY KEY, [DefaultValue] nvarchar(255) NOT NULL, [SettingValue] nvarchar(255) NOT NULL); ";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = _strInsert.ToString();
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "CREATE TABLE [tb_GiftCard]([GiftCardID] INTEGER PRIMARY KEY NOT NULL IDENTITY(1,1), [Barcode] nvarchar(255) NOT NULL UNIQUE, [Serial] nvarchar(255) NOT NULL, [CreateDate] INTEGER, [ExpirationDate] INTEGER, [CustomerIDUse] INTEGER, [Amount] decimal, [Balance] decimal, [DeliveredDate] INTEGER NOT NULL DEFAULT 0);";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "CREATE TABLE [tb_OrderPayment] ([OrderPaymentID] INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1), [OrderID] INTEGER, [PaymentID] INTEGER, [CardID] INTEGER, [Amount] decimal );";
                            cmd.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    tr.Dispose();
                    cmd.Dispose();
                }
                catch (SqlException ex)
                {
                    if (tr != null)
                    {
                        try
                        {
                            tr.Rollback();
                        }
                        catch (ObjectDisposedException ex2)
                        {
                            _rs = ex2.Message;
                        }
                        finally
                        {
                            tr.Dispose();
                        }
                    }
                    _rs = ex.Message;
                }
            }
            else //SQLite
            {
                SQLiteConnection sqliteConn = ConnectionDB.getSQLiteConnection();
                SQLiteTransaction tr = null;
                SQLiteCommand cmd = null;
                try
                {
                    using (tr = sqliteConn.BeginTransaction())
                    {
                        using (cmd = sqliteConn.CreateCommand())
                        {
                            cmd.Transaction = tr;
                            cmd.CommandText = "CREATE TABLE [tb_Currency]([SettingID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, [Currency] nvarchar(200) NULL, [TaxRate] float NULL, [Active] int NULL, [Version] int NULL);";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "insert into tb_Currency select * from tb_Setting";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "drop table if exists [tb_Setting]";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "CREATE TABLE [tb_Setting]([SettingKey] nvarchar(255) NOT NULL PRIMARY KEY, [DefaultValue] TEXT NOT NULL, [SettingValue] TEXT NOT NULL); ";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = _strInsert.ToString();
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "CREATE TABLE [tb_GiftCard]([GiftCardID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [Barcode] nvarchar(255) NOT NULL, [Serial] nvarchar(255) NOT NULL, [CreateDate] INTEGER, [ExpirationDate] INTEGER, [CustomerIDUse] INTEGER, [Amount] DOUBLE, [Balance] DOUBLE, [DeliveredDate] INTEGER NOT NULL DEFAULT 0, UNIQUE([Barcode] ASC));";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "CREATE TABLE [tb_OrderPayment] ([OrderPaymentID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, [OrderID] INTEGER, [PaymentID] INTEGER, [CardID] INTEGER, [Amount] DOUBLE );";
                            cmd.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    tr.Dispose();
                    cmd.Dispose();
                }
                catch (SQLiteException ex)
                {
                    if (tr != null)
                    {
                        try
                        {
                            tr.Rollback();
                        }
                        catch (ObjectDisposedException ex2)
                        {
                            _rs = ex2.Message;
                        }
                        finally
                        {
                            tr.Dispose();
                        }
                    }
                    _rs = ex.Message;
                }
            }
            return _rs;
        }
    }
    public enum dateFormat
    {
        slashMdyyyy, slashMMddyyyy, slashyyyyMMdd, commadMMMyyyy, commaMMMdyyyy, slashddMMMyyyy, commaddddMMMMdyyyy, commaMMMMdyyyy, commadddddMMMMyyyy, commadMMMMyyyy
    }
    public enum timeFormat
    {
        None, hhmmtt, hmmtt, Hmm, HHmm, HHmmss, hmmsstt, hhmmsstt, Hmmss
    }
}
