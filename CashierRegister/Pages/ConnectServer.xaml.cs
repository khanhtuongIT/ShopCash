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
using System.Data.SqlClient;
using CashierRegisterDAL;
using System.Threading;

namespace CashierRegister.Pages
{
    /// <summary>
    /// Interaction logic for ConnectServer.xaml
    /// </summary>
    public partial class ConnectServer : ModernWindow
    {
        //connect database
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        public string sqltype = "";
        public string server_name = "";
        public int authentication = 0;
        public string user_name = "";
        public string password = "";
        private Thread thread_connect = null;
        private Thread thread_loaded = null;
        private bool flag_check_connected = false;

        //ConnectServer
        public ConnectServer()
        {
            InitializeComponent();
        }

        //ModernWindow_Loaded
        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (thread_loaded != null && thread_loaded.ThreadState == ThreadState.Running) { }
            else
            {
                thread_loaded = new Thread(() =>
                {
                    try
                    {
                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            this.pgbSQLServer.IsIndeterminate = true;

                            server_name = this.txbServerName.Text.Trim().ToString();
                            this.txbServerName.IsEnabled = false;

                            authentication = this.cboAuthentication.SelectedIndex;
                            this.cboAuthentication.IsEnabled = false;

                            user_name = this.txbUserName.Text.Trim().ToString();
                            this.txbUserName.IsEnabled = false;

                            password = pwbPassword.Password.Trim().ToString();
                            this.pwbPassword.IsEnabled = false;

                            this.btnConnect.IsEnabled = false;
                            this.btnCancel.IsEnabled = true;
                            this.tblUsingSQLite.Visibility = System.Windows.Visibility.Hidden;
                        }));
                        
                        //check connect
                        string result = CheckConnect();
                        if (result == "successfully")
                        {
                            flag_check_connected = true;
                            Thread.Sleep(500);
                            this.Dispatcher.Invoke((Action)(() => { this.Close(); }));
                        }
                        else
                        {
                            flag_check_connected = false;
                            this.Dispatcher.Invoke((Action)(() => 
                            {
                                tblNotificationSQLServer.Text = FindResource("connect_failed").ToString();
                                this.pgbSQLServer.IsIndeterminate = false;
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
                                this.btnConnect.IsEnabled = true;
                                this.btnCancel.IsEnabled = false;
                                this.tblUsingSQLite.Visibility = System.Windows.Visibility.Visible;
                            }));
                        }
                    }
                    catch (Exception)
                    {
                        flag_check_connected = false;
                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            tblNotificationSQLServer.Text = FindResource("connect_failed").ToString();
                            this.pgbSQLServer.IsIndeterminate = false;
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
                            this.btnConnect.IsEnabled = true;
                            this.btnCancel.IsEnabled = false;
                            this.tblUsingSQLite.Visibility = System.Windows.Visibility.Visible;
                        }));
                    }
                });
                thread_loaded.Start();
            }
        }

        //btnConnect_Click
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            //check empty
            if (CheckEmpty() == true)
            {
                this.btnConnect.IsEnabled = true;
                return;
            }

            if (thread_connect != null && thread_connect.ThreadState == ThreadState.Running) { }
            else
            {
                thread_connect = new Thread(() =>
                {
                    try
                    {
                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            this.pgbSQLServer.IsIndeterminate = true;
                            this.btnConnect.IsEnabled = false;
                            this.hplUsingSQLite.IsEnabled = false;
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
                            this.btnCancel.IsEnabled = true;
                            this.hplUsingSQLite.IsEnabled = false;
                        }));

                        //check connect
                        string result = CheckConnect();
                        if (result == "successfully")
                        {
                            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(current_directory + @"\_sqltype");
                            streamwriter.WriteLine("sqltype:sqlserver");
                            streamwriter.WriteLine("server:" + server_name);
                            streamwriter.WriteLine("authentication:" + authentication.ToString());
                            streamwriter.WriteLine("id:" + user_name);
                            streamwriter.WriteLine("password:" + password);
                            streamwriter.Close();
                            StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\_sqltype", current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                            using (System.IO.StreamWriter file = System.IO.File.CreateText(current_directory+@"\appSettings.json"))
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
                        else
                        {
                            flag_check_connected = false;
                            this.Dispatcher.Invoke((Action)(() => 
                            {
                                tblNotificationSQLServer.Text = FindResource("connect_failed").ToString();
                                this.pgbSQLServer.IsIndeterminate = false;
                                this.txbServerName.IsEnabled = true;
                                this.cboAuthentication.IsEnabled = true;
                            }));

                            if (authentication == 1)
                            {
                                this.txbUserName.Dispatcher.Invoke((Action)(() => { this.txbUserName.IsEnabled = true; }));
                                this.pwbPassword.Dispatcher.Invoke((Action)(() => { this.pwbPassword.IsEnabled = true; }));
                            }

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                this.btnConnect.IsEnabled = true;
                                this.btnCancel.IsEnabled = false;
                                this.tblUsingSQLite.Visibility = System.Windows.Visibility.Visible;
                                this.hplUsingSQLite.IsEnabled = true;
                            }));
                        }
                    }
                    catch (Exception)
                    {
                        flag_check_connected = false;

                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            tblNotificationSQLServer.Text = FindResource("connect_failed").ToString();
                            this.pgbSQLServer.IsIndeterminate = false;
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
                            this.btnConnect.IsEnabled = true;
                            this.btnCancel.IsEnabled = false;
                            this.tblUsingSQLite.Visibility = System.Windows.Visibility.Visible;
                            this.hplUsingSQLite.IsEnabled = true;
                        }));
                    }
                });
                thread_connect.Start();
            }
        }

        //CheckConnect
        private string CheckConnect()
        {
            if (server_name != "")
            {
                string str_connect_server = ConnectionDB.CheckConnectionServer(server_name, user_name, password, authentication);
                if (str_connect_server == "successfully")
                {
                    //check database exist
                    if (ConnectionDB.CheckDatabaseExists(server_name, user_name, password, authentication))
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

        //tblCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (thread_loaded != null && thread_loaded.ThreadState == ThreadState.Running)
            {
                new Thread(() =>
                {
                    thread_loaded.Abort();
                    this.Dispatcher.Invoke((Action)(() => 
                    {
                        this.pgbSQLServer.IsIndeterminate = false;
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
                        this.btnConnect.IsEnabled = true;
                        this.btnCancel.IsEnabled = false;
                        this.tblUsingSQLite.Visibility = System.Windows.Visibility.Visible;
                    }));
                }).Start();
            }

            if (thread_connect != null && thread_connect.ThreadState == ThreadState.Running)
            {
                new Thread(() =>
                {
                    thread_connect.Abort();
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.pgbSQLServer.IsIndeterminate = false;
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
                        this.btnConnect.IsEnabled = true;
                        this.btnCancel.IsEnabled = false;
                        this.tblUsingSQLite.Visibility = System.Windows.Visibility.Visible;
                        this.hplUsingSQLite.IsEnabled = true;
                    }));
                }).Start();
            }
        }

        //hplUsingSQLite_Click
        private void hplUsingSQLite_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                try
                {
                    this.Dispatcher.Invoke((Action)(() => 
                    {
                        this.pgbSQLServer.IsIndeterminate = false;
                        this.tblNotificationSQLServer.Text = "";
                    }));

                    //abort thread_connect if it is running
                    if (thread_connect != null && thread_connect.ThreadState == ThreadState.Running)
                        thread_connect.Abort();

                    this.Dispatcher.Invoke((Action)(() => 
                    {
                        this.tblUsingSQLite.Visibility = System.Windows.Visibility.Hidden;
                        this.spSQLServer.Visibility = System.Windows.Visibility.Hidden;
                        this.spSQLite.Visibility = System.Windows.Visibility.Visible;
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
                        md.Content = ex.Message;
                        md.ShowDialog();
                    }));
                }
            }).Start();
        }

        //ModernWindow_Closed
        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            if (flag_check_connected == false)
                Environment.Exit(1);
        }

        //btnYes_Click
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                this.pgbSQLite.Dispatcher.Invoke((Action)(() => { this.pgbSQLite.IsIndeterminate = true; }));

                this.Dispatcher.Invoke((Action)(() =>
                {
                    System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(current_directory + @"\_sqltype");
                    streamwriter.WriteLine("sqltype:sqlite");
                    streamwriter.WriteLine("server:" + server_name);
                    streamwriter.WriteLine("authentication:" + authentication.ToString());
                    streamwriter.WriteLine("id:" + user_name);
                    streamwriter.WriteLine("password:" + password);
                    streamwriter.Close();
                    StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\_sqltype", current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                }));
                using (System.IO.StreamWriter file = System.IO.File.CreateText(current_directory + @"\appSettings.json"))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.Serialize(file, StaticClass.GeneralClass.app_settings);
                }
                Thread.Sleep(500);
                this.pgbSQLite.Dispatcher.Invoke((Action)(() => { this.pgbSQLite.IsIndeterminate = false; }));

                this.Dispatcher.Invoke((Action)(() =>
                {
                    StaticClass.GeneralClass.app_settings["appIsRestart"] = "True";
                    Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }));
            }).Start();
        }

        //tblNo_Click
        private void tblNo_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                this.Dispatcher.Invoke((Action)(() => 
                {
                    this.spSQLite.Visibility = System.Windows.Visibility.Hidden;
                    this.spSQLServer.Visibility = System.Windows.Visibility.Visible;
                    this.tblUsingSQLite.Visibility = System.Windows.Visibility.Visible;
                }));
            }).Start();
        }

        //cboAuthentication_SelectionChanged
        private void cboAuthentication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboAuthentication.SelectedIndex == 0)
            {
                txbUserName.IsEnabled = false;
                pwbPassword.IsEnabled = false;
                txbUserName.Text = "";
                pwbPassword.Password = "";
            }
            else
            {
                txbUserName.IsEnabled = true;
                pwbPassword.IsEnabled = true;
            }
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

    }
}
