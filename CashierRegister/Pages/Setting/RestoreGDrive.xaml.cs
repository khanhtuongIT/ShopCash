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
using System.Data.SqlClient;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Upload;
using Google.Apis.Services;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Logging;
using Google.Apis.Util.Store;
using System.Data;
using CashierRegisterDAL;
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for RestoreGDrive.xaml
    /// </summary>
    public partial class RestoreGDrive : ModernDialog
    {
        //current directory
        private string current_directory = "";

        //check click restore
        private bool flag_check_click = false;

        //this is full path to the file you want to download
        //private const string download_file_name = @"Databses\CheckOut.db";
        private string download_file_name = "";

        //this is a download directory
        //private const string download_directory_name = "Databases";
        private string download_directory_name = "";

        //this is a file type to download
        private const string content_type = @"application/octet-stream";

        private string current_time = "";
        private string _current_time = "";
        private string __current_time = "";
        private string current_folder_undo = "";
        private string sqltype = "";
        private string server_name = "";
        private int authentication = 0;
        private string user_name = "";
        private string password = "";
        private System.IO.FileInfo file_info = null;
        private System.Text.StringBuilder _strImport = new System.Text.StringBuilder();
        public string extDownload = string.Empty;

        //DeleteSalesperson
        public RestoreGDrive()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { };
            current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //open connect
            if (StaticClass.GeneralClass.flag_database_type_general == false)
                ConnectionDB.OpenConnect();

            if (thread_restore != null && thread_restore.ThreadState == ThreadState.Running)
            {
                new Thread(() =>
                {
                    thread_restore.Abort();
                }).Start();

                if (filestream != null)
                    filestream.Dispose();

                long lenght = new System.IO.FileInfo(current_directory + @"\Databases\CheckOut.db").Length;
                if (lenght == 0)
                {
                    try
                    {
                        if (System.IO.File.Exists(current_directory + @"\DBRES_Local\" + current_folder_undo + @"\CheckOut.db") == true)
                            //copy database file
                            System.IO.File.Copy(current_directory + @"\DBRES_Local\" + current_folder_undo + @"\CheckOut.db", current_directory + @"\Databases\CheckOut.db", true);
                    }
                    catch (Exception) { }
                }
            }
            System.Diagnostics.Debug.WriteLine(extDownload);
            this.Close();
        }

        //muiBtnOK_Click
        private Thread thread_restore = null;

        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flag_check_click = true;
                if (thread_restore != null && thread_restore.ThreadState == ThreadState.Running)
                    return;
                else
                {
                    thread_restore = new Thread(() =>
                    {
                        try
                        {
                            this.muiBtnCancel.Dispatcher.Invoke((Action)(() => { this.muiBtnCancel.IsEnabled = false; }));
                            this.muiBtnOK.Dispatcher.Invoke((Action)(() => { muiBtnOK.Visibility = System.Windows.Visibility.Hidden; }));
                            this.mpr.Dispatcher.Invoke((Action)(() => { mpr.IsActive = true; }));
                            this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("please_wait").ToString(); }));

                            //UserCredential user_credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = client_id, ClientSecret = client_secret }, new[] { DriveService.Scope.Drive }, "user", CancellationToken.None).Result;
                            UserCredential user_credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = GeneralClass.client_id, ClientSecret = GeneralClass.client_secret, }, new[] { DriveService.Scope.DriveFile }, "user", CancellationToken.None, new FileDataStore("TuanNguyen.GoogleDrive.Auth.Store")).Result;

                            //create the drive service
                            var driveservice = new DriveService(new BaseClientService.Initializer() { HttpClientInitializer = user_credential, ApplicationName = "Restore database" });

                            string current_time = System.DateTime.Now.ToString();
                            bool flag = false;

                            //create folder undo database
                            //if database type is sqlite
                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                            {
                                //close connect
                                ConnectionDB.CloseConnect();

                                if (System.IO.Directory.Exists(current_directory + @"\DBRES_Local") == false)
                                    System.IO.Directory.CreateDirectory(current_directory + @"\DBRES_Local");

                                //copy database to folder undo
                                if (System.IO.Directory.Exists(current_directory + @"\Databases") == true)
                                {
                                    if (System.IO.File.Exists(current_directory + @"\Databases\CheckOut.db") == true)
                                    {
                                        System.IO.FileInfo file_info = new System.IO.FileInfo(current_directory + @"\Databases\CheckOut.db");
                                        if (file_info.Length == 0)
                                            flag = true;
                                    }
                                    else
                                        return;
                                }
                                else
                                    return;
                            }
                            //if database type is sql server
                            else
                            {
                                if (System.IO.Directory.Exists(current_directory + @"\DBRESSer_Local") == false)
                                    System.IO.Directory.CreateDirectory(current_directory + @"\DBRESSer_Local");
                            }

                            //database size greater than zero
                            if (flag == false)
                            {
                                //copy database to backup folder
                                _current_time = current_time.Replace("/", ".");
                                __current_time = _current_time.Replace(":", "..");
                                current_folder_undo = __current_time;

                                //if database type is sqlite
                                if (StaticClass.GeneralClass.flag_database_type_general == false)
                                {
                                    System.IO.Directory.CreateDirectory(current_directory + @"\DBRES_Local\" + current_folder_undo);
                                    System.IO.File.Copy(current_directory + @"\Databases\CheckOut.db", current_directory + @"\DBRES_Local\" + current_folder_undo + @"\CheckOut.db");
                                    System.IO.File.Delete(current_directory + @"\Databases\CheckOut.db");
                                }
                                else
                                {
                                    System.IO.Directory.CreateDirectory(current_directory + @"\DBRESSer_Local\" + current_folder_undo);
                                    string _fileBackup = current_directory + @"\DBRESSer_Local\" + current_folder_undo + @"\CheckOut.bak";
                                    SqlConnection sqlConnect = ConnectionDB.getSQLConnection();
                                    var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}' WITH NOFORMAT, NOINIT,  NAME = N'data-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", ConnectionDB.getSqlServerDataBaseName(), @_fileBackup);
                                    try
                                    {
                                        using (var command = new SqlCommand(query, sqlConnect))
                                        {
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                    catch{}
                                }

                                //if restore database success
                                if (RestoreDatabase(driveservice, StaticClass.GeneralClass.download_url_general) == true)
                                {
                                    //if database type is sqlite
                                    if (StaticClass.GeneralClass.flag_database_type_general == false)
                                    {
                                        //undo database file if file size is 0
                                        System.IO.FileInfo fileinfo_checkout = new System.IO.FileInfo(current_directory + @"\Databases\CheckOut.db");
                                        if (fileinfo_checkout.Length == 0)
                                            //copy database file
                                            System.IO.File.Copy(current_directory + @"\DBRES_Local\" + current_folder_undo + @"\CheckOut.db", current_directory + @"\Databases\CheckOut.db", true);
                                    }
                                    //if database type sqlserver
                                    else
                                    {
                                        string _fileRestore = current_directory + @"\DBRESSer_Local\Database_Ser_Temp\CheckOut.bak";
                                        if (!string.IsNullOrEmpty(extDownload) && extDownload == "bak" && System.IO.File.Exists(_fileRestore))
                                        {
                                            SqlConnection sqlConnect = ConnectionDB.getSQLConnection();
                                            using (var command = sqlConnect.CreateCommand())
                                            {
                                                try
                                                {
                                                    command.CommandText = "Use Master";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "ALTER DATABASE " + ConnectionDB.getSqlServerDataBaseName() + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "RESTORE DATABASE " + ConnectionDB.getSqlServerDataBaseName() + " FROM DISK = '" + _fileRestore + "' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10; ";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "Use Master";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "ALTER DATABASE " + ConnectionDB.getSqlServerDataBaseName() + " SET MULTI_USER;";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "Use " + ConnectionDB.getSqlServerDataBaseName();
                                                    command.ExecuteNonQuery();
                                                }
                                                catch { }
                                            }
                                        }
                                        else if (System.IO.File.Exists(current_directory + @"\sqltype") == true)
                                        {
                                            //get connection info
                                            System.IO.StreamReader stream_reader = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                                            sqltype = stream_reader.ReadLine().Split(':')[1].ToString();
                                            server_name = stream_reader.ReadLine().Split(':')[1].ToString();
                                            Int32.TryParse(stream_reader.ReadLine().Split(':')[1].ToString(), out authentication);
                                            user_name = stream_reader.ReadLine().Split(':')[1].ToString();
                                            password = stream_reader.ReadLine().Split(':')[1].ToString();
                                            stream_reader.Close();

                                            //connection to server
                                            string connection_string = "";

                                            //if authentication is windows
                                            if (authentication == 0)
                                                connection_string = "server = " + server_name + "; Trusted_Connection = true;";

                                            //if authentication is sql server
                                            else
                                                connection_string = "server = " + server_name + "; user id = " + user_name + "; password = " + password + "; integrated security = true;";

                                            SqlConnection sql_connection = new SqlConnection();
                                            sql_connection.ConnectionString = connection_string;
                                            sql_connection.Open();

                                            //insert data

                                            System.IO.StreamReader stream_reader_insert = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\Database_Ser_Temp\CheckOut.db", StaticClass.GeneralClass.key_register_general);
                                            string _line;
                                            while ((_line = stream_reader_insert.ReadLine()) != null)
                                            {
                                                if (!string.IsNullOrEmpty(_line) && _line.Substring(0, 3) != "-- ")
                                                {
                                                    if (_line.Contains("INSERT INTO"))
                                                    {
                                                        string _strTemp = _line.Substring(0, _line.IndexOf("("));
                                                        _strTemp = _strTemp.Substring("insert into ".Length).Trim().Replace("[", "").Replace("]", "");
                                                        _line = _line.Replace("')", @"\u0066").Replace("''", @"\u0055").Replace("','", @"\u0022").Replace("', '", @"\u0099").Replace(",'", @"\u0033").Replace(", '", @"\u0077").Replace("',", @"\u0044").Replace("' ,", @"\u0088").Replace("'", "''");
                                                        _line = _line.Replace(@"\u0066", "')").Replace(@"\u0055", "''").Replace(@"\u0022", "',N'").Replace(@"\u0099", "', N'").Replace(@"\u0033", ",N'").Replace(@"\u0077", ", N'").Replace(@"\u0044", "',").Replace(@"\u0088", "' ,");
                                                        _line = "if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _strTemp + "', 'U') and is_identity = 1) begin SET IDENTITY_INSERT " + _strTemp + " ON; " + _line + " SET IDENTITY_INSERT " + _strTemp + " OFF; end else " + _line;
                                                    }
                                                    _strImport.AppendLine(_line);
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(_strImport.ToString()))
                                            {
                                                SqlTransaction tr = null;
                                                SqlCommand sql_command = null;
                                                try
                                                {
                                                    using (tr = sql_connection.BeginTransaction())
                                                    {
                                                        using (sql_command = sql_connection.CreateCommand())
                                                        {
                                                            sql_command.Transaction = tr;
                                                            sql_command.CommandText = _strImport.ToString();
                                                            sql_command.ExecuteNonQuery();
                                                        }
                                                        tr.Commit();
                                                    }
                                                    tr.Dispose();
                                                    sql_command.Dispose();
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
                                                        }
                                                        finally
                                                        {
                                                            tr.Dispose();
                                                        }
                                                    }
                                                }
                                            }
                                            sql_connection.Close();
                                        }
                                    }

                                    this.Dispatcher.Invoke((Action)(() => 
                                    { 
                                        this.Close();
                                        Pages.Setting.RestartUndoBackup page = new Pages.Setting.RestartUndoBackup();
                                        page.current_folder_undo = current_folder_undo;
                                        page.ShowDialog();
                                    }));
                                }

                                //if restore database failed
                                else
                                {
                                    //if database type is sqlite
                                    if (StaticClass.GeneralClass.flag_database_type_general == false)
                                    {
                                        //undo database file if file size is 0
                                        System.IO.FileInfo fileinfo_checkout = new System.IO.FileInfo(current_directory + @"\Databases\CheckOut.db");
                                        if (fileinfo_checkout.Length == 0)
                                            //copy database file
                                            System.IO.File.Copy(current_directory + @"\DBRES_Local\" + current_folder_undo + @"\CheckOut.db", current_directory + @"\Databases\CheckOut.db", true);
                                    }
                                    else
                                    {
                                        string _fileRestore = current_directory + @"\DBRESSer_Local\" + current_folder_undo + @"\CheckOut.bak";
                                        if (!string.IsNullOrEmpty(extDownload) && extDownload == "bak" && System.IO.File.Exists(_fileRestore))
                                        {
                                            SqlConnection sqlConnect = ConnectionDB.getSQLConnection();
                                            using (var command = sqlConnect.CreateCommand())
                                            {
                                                try
                                                {
                                                    command.CommandText = "Use Master";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "ALTER DATABASE " + ConnectionDB.getSqlServerDataBaseName() + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "RESTORE DATABASE " + ConnectionDB.getSqlServerDataBaseName() + " FROM DISK = '" + _fileRestore + "' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10; ";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "Use Master";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "ALTER DATABASE " + ConnectionDB.getSqlServerDataBaseName() + " SET MULTI_USER;";
                                                    command.ExecuteNonQuery();
                                                    command.CommandText = "Use " + ConnectionDB.getSqlServerDataBaseName();
                                                    command.ExecuteNonQuery();
                                                }
                                                catch { }
                                            }
                                        }
                                        else if (System.IO.File.Exists(current_directory + @"\sqltype") == true)
                                        {
                                            //get connection info
                                            System.IO.StreamReader stream_reader = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                                            sqltype = stream_reader.ReadLine().Split(':')[1].ToString();
                                            Int32.TryParse(stream_reader.ReadLine().Split(':')[1].ToString(), out authentication);
                                            server_name = stream_reader.ReadLine().Split(':')[1].ToString();
                                            user_name = stream_reader.ReadLine().Split(':')[1].ToString();
                                            password = stream_reader.ReadLine().Split(':')[1].ToString();
                                            stream_reader.Close();

                                            //connection to server
                                            string connection_string = "";

                                            //if authentication is windows
                                            if (authentication == 0)
                                                connection_string = "server = " + server_name + "; Trusted_Connection = true;";

                                            //if authentication is sql server
                                            else
                                                connection_string = "server = " + server_name + "; user id = " + user_name + "; password = " + password + "; integrated security = true;";

                                            SqlConnection sql_connection = new SqlConnection();
                                            sql_connection.ConnectionString = connection_string;
                                            sql_connection.Open();

                                            //insert data
                                            System.IO.StreamReader stream_reader_insert = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\DBRESSer_Local\" + current_folder_undo + @"\CheckOut.sql", StaticClass.GeneralClass.key_register_general);
                                            string _line;
                                            while ((_line = stream_reader_insert.ReadLine()) != null)
                                            {
                                                if (!string.IsNullOrEmpty(_line) && _line.Substring(0, 3)!="-- ")
                                                {
                                                    if (_line.StartsWith("INSERT INTO") || _line.StartsWith("insert into"))
                                                    {
                                                        string _strTemp = _line.Substring(0, _line.IndexOf("("));
                                                        _strTemp = _strTemp.Substring("insert into ".Length).Trim().Replace("[", "").Replace("]", "");
                                                        _line = _line.Replace("')", @"\u0066").Replace("''", @"\u0055").Replace("','", @"\u0022").Replace("', '", @"\u0099").Replace(",'", @"\u0033").Replace(", '", @"\u0077").Replace("',", @"\u0044").Replace("' ,", @"\u0088").Replace("'", "''");
                                                        _line = _line.Replace(@"\u0066", "')").Replace(@"\u0055", "''").Replace(@"\u0022", "',N'").Replace(@"\u0099", "', N'").Replace(@"\u0033", ",N'").Replace(@"\u0077", ", N'").Replace(@"\u0044", "',").Replace(@"\u0088", "' ,");
                                                        _line = "if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _strTemp + "', 'U') and is_identity = 1) begin SET IDENTITY_INSERT " + _strTemp + " ON; " + _line + " SET IDENTITY_INSERT " + _strTemp + " OFF; end else " + _line;
                                                    }
                                                    _strImport.AppendLine(_line);
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(_strImport.ToString()))
                                            {
                                                SqlTransaction tr = null;
                                                SqlCommand sql_command = null;
                                                try
                                                {
                                                    using (tr = sql_connection.BeginTransaction())
                                                    {
                                                        using (sql_command = sql_connection.CreateCommand())
                                                        {
                                                            sql_command.Transaction = tr;
                                                            sql_command.CommandText = _strImport.ToString();
                                                            sql_command.ExecuteNonQuery();
                                                        }
                                                        tr.Commit();
                                                    }
                                                    tr.Dispose();
                                                    sql_command.Dispose();
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
                                                        }
                                                        finally
                                                        {
                                                            tr.Dispose();
                                                        }
                                                    }
                                                }
                                            }
                                            sql_connection.Close();
                                        }
                                    }
                                }
                            }

                            //open connect
                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                                ConnectionDB.OpenConnect();
                        }
                        catch (AggregateException)
                        {
                            //open connect
                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                                ConnectionDB.OpenConnect();

                            this.muiBtnCancel.Dispatcher.Invoke((Action)(() => { this.muiBtnCancel.IsEnabled = true; }));
                            this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_restore").ToString(); }));
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { tblNotification.Text = FindResource("have_not_access").ToString(); }));
                        }

                        catch (Exception)
                        {
                            //open connect
                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                                ConnectionDB.OpenConnect();

                            this.muiBtnCancel.Dispatcher.Invoke((Action)(() => { this.muiBtnCancel.IsEnabled = true; }));
                            this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_restore").ToString(); }));
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("have_not_access").ToString(); }));
                        }
                    });
                    thread_restore.Start();
                }
            }
            catch (Exception ex)
            {
                //open connect
                if (StaticClass.GeneralClass.flag_database_type_general == false)
                    ConnectionDB.OpenConnect();

                this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_restore").ToString(); }));
                this.tblNotification.Dispatcher.Invoke((Action)(() => { tblNotification.Text = ex.Message; }));
            }
        }

        //Restore database
        private System.IO.FileStream filestream = null;
        private bool RestoreDatabase(DriveService driveservice, string downloadurl)
        {
            bool flag = false;
            var downloader = new MediaDownloader(driveservice);

            int last_dot;
            var file_name = "";

            //if database type is sqlite
            if (StaticClass.GeneralClass.flag_database_type_general == false)
            {
                download_file_name = @"Databses\CheckOut.db";
                last_dot = download_file_name.LastIndexOf(".");
                download_directory_name = "Databases";
                file_name = download_directory_name + @"\CheckOut" + (last_dot != -1 ? "." + download_file_name.Substring(last_dot + 1) : "");
            }
            //if database type is sqlserver
            else
            {
                //check exist of folder temp contain database backup
                if (System.IO.Directory.Exists(current_directory + @"\Database_Ser_Temp") == false)
                    System.IO.Directory.CreateDirectory(current_directory + @"\Database_Ser_Temp");

                //check database exist
                if (System.IO.File.Exists(current_directory + @"\Database_Ser_Temp\CheckOut.sql") == true)
                    System.IO.File.Delete(current_directory + @"\Database_Ser_Temp\CheckOut.sql");
                if(!string.IsNullOrEmpty(extDownload) && extDownload=="bak")
                    download_file_name = @"Databses\CheckOut.bak";
                else download_file_name = @"Databses\CheckOut.db";
                last_dot = download_file_name.LastIndexOf(".");
                download_directory_name = "Database_Ser_Temp";
                file_name = download_directory_name + @"\CheckOut" + (last_dot != -1 ? "." + download_file_name.Substring(last_dot + 1) : "");
            }

            var full_path = System.IO.Path.Combine(file_name);

            filestream = new System.IO.FileStream(full_path, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);

            if (downloader.Download(downloadurl, filestream) != null)
                flag = true;
            else
                flag = false;

            filestream.Dispose();
            return flag;
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            muiBtnOK.Focus();
            if (filestream != null)
                filestream.Dispose();
        }
    }
}
