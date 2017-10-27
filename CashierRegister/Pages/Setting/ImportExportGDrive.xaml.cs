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
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;
using System.Data.SqlClient;
using CashierRegisterDAL;

using System.Threading;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Logging;
using Google.Apis.Upload;
using Google.Apis.Download;
using Google.Apis.Drive;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Util.Store;
using CashierRegister.StaticClass;


namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for ImportExportGDrive.xaml
    /// </summary>
    public partial class ImportExportGDrive : UserControl
    {
        //using for database
        private List<EC_tb_Database> list_tb_database = new List<EC_tb_Database>();
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        private BitmapImage bitmapimage_restore = new BitmapImage(new Uri(@"pack://application:,,,/Resources/restore.png", UriKind.Absolute));
        private BitmapImage bitmapimage_delete = new BitmapImage(new Uri(@"pack://application:,,,/Resources/delete.png", UriKind.Absolute));
        private BitmapImage bitmapimage_undo = new BitmapImage(new Uri(@"pack://application:,,,/Resources/database_undo.png", UriKind.Absolute));
        private Thread thread_getdatabase = null;

        private string current_folder_undo = "";
        private bool flag_check_loaded = false;
        private string server= "";
        private string id = "";
        private string password = "";
        private List<string> _lstExt = new List<string>();

        //ImportExportGDrive
        public ImportExportGDrive()
        {
            InitializeComponent();
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                current_folder_undo = "";
                flag_check_loaded = true;
                GetDatabase();
            }
        }

        //GetDatabase
        private void GetDatabase()
        {
            try
            {
                if (thread_getdatabase != null && thread_getdatabase.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_getdatabase = new Thread(() =>
                    {
                        try
                        {
                            this.mpr.Dispatcher.Invoke((Action)(() => 
                            {
                                mpr.IsActive = true;
                                dtgDatabase.ItemsSource = null;
                            }));
                            list_tb_database.Clear();

                            //UserCredential user_credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = client_id, ClientSecret = client_secret, }, new[] { DriveService.Scope.Drive }, "user", CancellationToken.None, new FileDataStore("TuanNguyen.GoogleDrive.Auth.Store")).Result;
                            UserCredential user_credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = GeneralClass.client_id, ClientSecret = GeneralClass.client_secret, }, new[] { DriveService.Scope.DriveFile }, "user", CancellationToken.None, new FileDataStore("TuanNguyen.GoogleDrive.Auth.Store")).Result;

                            //create the drive service
                            var driveservice = new DriveService(new BaseClientService.Initializer() { HttpClientInitializer = user_credential, ApplicationName = "Get database backup" });

                            this.Dispatcher.Invoke((Action)(() => { this.muiBtnBackup.Visibility = System.Windows.Visibility.Visible; }));

                            //get file from gdrive
                            string str_condition = "Backup_CashierRegister_";

                            try
                            {
                                FileList file_list = driveservice.Files.List().Execute();

                                string client_folderid_lc = "";
                                string client_folderid_gd = "";
                                string client_foldername_gd = "";

                                //if database type is sqlite
                                if (StaticClass.GeneralClass.flag_database_type_general == false)
                                    client_foldername_gd = "CashierRegister_Backup";
                                else
                                    client_foldername_gd = "CashierRegister_Ser_Backup";

                                //if ClientFolderInfo file isn't existed
                                if (!System.IO.File.Exists("ClientFolderInfo"))
                                {
                                    bool flag_exist_client_folderid = false;

                                    for (int i = 0; i < file_list.Items.Count; i++)
                                    {
                                        //if local ClientFolderInfo  file is't exist and CashierRegister_Backup is existed
                                        if ((file_list.Items[i].Title == client_foldername_gd) && (file_list.Items[i].MimeType == "application/vnd.google-apps.folder") && (file_list.Items[i].ExplicitlyTrashed == false))
                                        {
                                            //create ClientFolderInfo file
                                            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter("ClientFolder");

                                            //if database type is sqlite
                                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                                            {
                                                streamwriter.WriteLine("FolderID:" + file_list.Items[i].Id.ToString());
                                                streamwriter.WriteLine("FolderID:");
                                            }
                                            //if database tupe is sqlserver
                                            else
                                            {
                                                streamwriter.WriteLine("FolderID:");
                                                streamwriter.WriteLine("FolderID:" + file_list.Items[i].Id.ToString());
                                            }

                                            streamwriter.Close();
                                            StaticClass.GeneralClass.EncryptFileGD("ClientFolder", "ClientFolderInfo", StaticClass.GeneralClass.key_register_general);
                                            client_folderid_lc = file_list.Items[i].Id;
                                            client_folderid_gd = file_list.Items[i].Id;
                                            flag_exist_client_folderid = true;
                                            break;
                                        }
                                    }

                                    //if local ClientFolderInfo file isn't existed and drive ClientFolderInfo isn't existed
                                    if (flag_exist_client_folderid == false)
                                    {
                                        File folder_client = new File();

                                        //if database type is sqlite
                                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                                            folder_client.Title = "CashierRegister_Backup";
                                        //if database type is sqlserver
                                        else
                                            folder_client.Title = "CashierRegister_Ser_Backup";

                                        folder_client.Description = "This folder using for backup database";
                                        folder_client.MimeType = "application/vnd.google-apps.folder";
                                        //insert folder
                                        File response_folder = driveservice.Files.Insert(folder_client).Execute();
                                        if (response_folder != null)
                                        {
                                            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter("ClientFolder");

                                            //if database type is sqlite
                                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                                            {
                                                streamwriter.WriteLine("FolderID:" + response_folder.Id.ToString());
                                                streamwriter.WriteLine("FolderID:");
                                            }

                                            //if database tupe is sqlserver
                                            else
                                            {
                                                streamwriter.WriteLine("FolderID:");
                                                streamwriter.WriteLine("FolderID:" + response_folder.Id.ToString());
                                            }

                                            streamwriter.Close();
                                            StaticClass.GeneralClass.EncryptFileGD("ClientFolder", "ClientFolderInfo", StaticClass.GeneralClass.key_register_general);
                                            client_folderid_gd = response_folder.Id;
                                            client_folderid_lc = response_folder.Id;
                                        }
                                    }
                                }

                                //if local ClientFolderInfo file is existed
                                else
                                {
                                    bool flag_exist_client_folderid = false;

                                    System.IO.StreamReader streamreader_folder = StaticClass.GeneralClass.DecryptFileGD("ClientFolderInfo", StaticClass.GeneralClass.key_register_general);
                                    if (streamreader_folder != null)
                                    {
                                        //if database type is sqlite
                                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                                            client_folderid_lc = streamreader_folder.ReadLine().Split(':')[1];
                                        //if database type is sqlserver
                                        else
                                        {
                                            streamreader_folder.ReadLine();
                                            client_folderid_lc = streamreader_folder.ReadLine().Split(':')[1];
                                        }

                                        streamreader_folder.Close();
                                    }

                                    //if client_folderid_lc isn't null
                                    if (!String.IsNullOrWhiteSpace(client_folderid_lc))
                                    {
                                        for (int i = 0; i < file_list.Items.Count; i++)
                                        {
                                            //if local ClientFolderInfo file is exist and drive ClientFolderInfo is existed
                                            if ((file_list.Items[i].MimeType == "application/vnd.google-apps.folder") && (file_list.Items[i].ExplicitlyTrashed == false) && (file_list.Items[i].Id == client_folderid_lc))
                                            {
                                                client_folderid_gd = file_list.Items[i].Id;
                                                flag_exist_client_folderid = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < file_list.Items.Count; i++)
                                        {
                                            if ((file_list.Items[i].Title == client_foldername_gd) && (file_list.Items[i].MimeType == "application/vnd.google-apps.folder") && (file_list.Items[i].ExplicitlyTrashed == false))
                                            {
                                                client_folderid_gd = file_list.Items[i].Id;
                                                flag_exist_client_folderid = true;

                                                System.IO.StreamWriter streamwriter = new System.IO.StreamWriter("ClientFolder");

                                                //get FolderID
                                                System.IO.StreamReader stream_reader_temp = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\ClientFolderInfo", StaticClass.GeneralClass.key_register_general);

                                                //if database type is sqlite
                                                if (StaticClass.GeneralClass.flag_database_type_general == false)
                                                {
                                                    streamwriter.WriteLine("FolderID:" + file_list.Items[i].Id);
                                                    stream_reader_temp.ReadLine();
                                                    streamwriter.WriteLine("FolderID:" + stream_reader_temp.ReadLine().Split(':')[1].ToString());
                                                }

                                                //if database tupe is sqlserver
                                                else
                                                {
                                                    streamwriter.WriteLine("FolderID:" + stream_reader_temp.ReadLine().Split(':')[1].ToString());
                                                    streamwriter.WriteLine("FolderID:" + file_list.Items[i].Id);
                                                }

                                                //close stream_reader_temp
                                                if (stream_reader_temp != null)
                                                    stream_reader_temp.Close();

                                                streamwriter.Close();
                                                StaticClass.GeneralClass.EncryptFileGD("ClientFolder", "ClientFolderInfo", StaticClass.GeneralClass.key_register_general);
                                                client_folderid_gd = file_list.Items[i].Id;
                                                client_folderid_lc = file_list.Items[i].Id;

                                                break;
                                            }
                                        }

                                    }

                                    //if local ClientFolderInfo file is existed and drive ClientFolderInfo isn't existed
                                    if (flag_exist_client_folderid == false)
                                    {
                                        File folder_client = new File();

                                        //if database type is sqlite
                                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                                            folder_client.Title = "CashierRegister_Backup";

                                        //if database type is sqlserver
                                        else
                                            folder_client.Title = "CashierRegister_Ser_Backup";

                                        folder_client.Description = "This folder using for backup database";
                                        folder_client.MimeType = "application/vnd.google-apps.folder";
                                        //insert folder
                                        File response_folder = driveservice.Files.Insert(folder_client).Execute();
                                        if (response_folder != null)
                                        {
                                            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter("ClientFolder");

                                            //get FolderID
                                            System.IO.StreamReader stream_reader_temp = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\ClientFolderInfo", StaticClass.GeneralClass.key_register_general);

                                            //if database type is sqlite
                                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                                            {
                                                streamwriter.WriteLine("FolderID:" + response_folder.Id.ToString());
                                                stream_reader_temp.ReadLine();
                                                streamwriter.WriteLine("FolderID:" + stream_reader_temp.ReadLine().Split(':')[1].ToString());
                                            }

                                            //if database tupe is sqlserver
                                            else
                                            {
                                                streamwriter.WriteLine("FolderID:" + stream_reader_temp.ReadLine().Split(':')[1].ToString());
                                                streamwriter.WriteLine("FolderID:" + response_folder.Id.ToString());
                                            }

                                            //close stream_reader_temp
                                            if (stream_reader_temp != null)
                                                stream_reader_temp.Close();

                                            streamwriter.Close();
                                            StaticClass.GeneralClass.EncryptFileGD("ClientFolder", "ClientFolderInfo", StaticClass.GeneralClass.key_register_general);
                                            client_folderid_gd = response_folder.Id;
                                            client_folderid_lc = response_folder.Id;
                                        }
                                    }
                                }

                                int no = 0;
                                for (int i = 0; i < file_list.Items.Count; i++)
                                {
                                    
                                    if (file_list.Items[i].Title.Length > 23)
                                    {
                                        if (file_list.Items[i].Parents.Count > 0)
                                        {
                                            if ((file_list.Items[i].Parents[0].Id == client_folderid_lc) && (file_list.Items[i].MimeType == "application/octet-stream") && (file_list.Items[i].Title.Substring(0, 23) == str_condition) && (file_list.Items[i].ExplicitlyTrashed == false))
                                            {
                                                EC_tb_Database ec_tb_database = new EC_tb_Database();
                                                ec_tb_database.Id = ++no;
                                                ec_tb_database.IdDatabase = file_list.Items[i].Id.ToString();
                                                ec_tb_database.DownloadUrl = file_list.Items[i].DownloadUrl;
                                                ec_tb_database.BackupDate = file_list.Items[i].CreatedDate.ToString();
                                                ec_tb_database.FileSize = (file_list.Items[i].FileSize / 1000).ToString() + "KB";
                                                ec_tb_database.BitmapImage_Restore = bitmapimage_restore;
                                                ec_tb_database.BitmapImage_Delete = bitmapimage_delete;
                                                _lstExt.Add(file_list.Items[i].FileExtension.ToString());
                                                list_tb_database.Add(ec_tb_database);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (System.Net.Http.HttpRequestException)
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    ModernDialog md = new ModernDialog();
                                    md.Title = FindResource("notification").ToString();
                                    md.Content = FindResource("internet_problem").ToString();
                                    md.CloseButton.Content = FindResource("close").ToString();
                                    md.ShowDialog();
                                }));
                            }

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                tblTotal.Text = FindResource("checkout").ToString() + "(" + list_tb_database.Count.ToString() + ")";
                                dtgDatabase.ItemsSource = list_tb_database;
                                mpr.IsActive = false;
                                dtgDatabase.Visibility = System.Windows.Visibility.Visible;
                            }));

                            //get email address
                            string email_address = driveservice.About.Get().Execute().User.EmailAddress.ToString();
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                this.tblEmailAddress.Visibility = System.Windows.Visibility.Visible;
                                this.tblEmailAddress.Text = driveservice.About.Get().Execute().User.EmailAddress;

                                //set login logout
                                this.tblLogout.Visibility = System.Windows.Visibility.Visible;
                                this.tblLogin.Visibility = System.Windows.Visibility.Hidden;
                            }));
                        }
                        catch (AggregateException)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                ModernDialog md = new ModernDialog();
                                md.Title = FindResource("notification").ToString();
                                md.Content = FindResource("have_not_access").ToString();
                                md.ShowDialog();
                            }));
                        }
                    });
                    thread_getdatabase.Start();
                }
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ModernDialog md = new ModernDialog();
                    FindResource("notification").ToString();
                    md.Content = ex.Message;
                    md.ShowDialog();
                }));
            }
        }

        //btnRestore_Click
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDatabase.SelectedIndex > -1)
            {
                int index_selected = dtgDatabase.SelectedIndex;
                StaticClass.GeneralClass.id_database_general = list_tb_database[index_selected].IdDatabase;
                StaticClass.GeneralClass.download_url_general = list_tb_database[index_selected].DownloadUrl;

                RestoreGDrive page = new RestoreGDrive();
                page.extDownload = _lstExt[index_selected];
                page.ShowDialog();
            }
        }

        //btnDelete_Click
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDatabase.SelectedIndex > -1)
            {
                int index_selected = dtgDatabase.SelectedIndex;
                StaticClass.GeneralClass.id_database_general = list_tb_database[index_selected].IdDatabase;
                StaticClass.GeneralClass.download_url_general = list_tb_database[index_selected].DownloadUrl;

                DeleteGDrive page = new DeleteGDrive();
                page.btndelete_delegate += btnDelete_Click_Delegate;
                page.ShowDialog();
            }
        }

        //btnDelete_Click_Delegate
        private void btnDelete_Click_Delegate()
        {
            this.dtgDatabase.Dispatcher.Invoke((Action)(() => { GetDatabase(); }));
        }

        //muiBtnBackup_Click
        private void muiBtnBackup_Click(object sender, RoutedEventArgs e)
        {
            BackupGDrive page = new BackupGDrive();
            page.muitbnbackup_delegate += muiBtnBackup_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnBackup_Click_Delegate
        private void muiBtnBackup_Click_Delegate()
        {
            this.dtgDatabase.Dispatcher.Invoke((Action)(() =>
            {
                GetDatabase();
            }));
        }

        //muiBtngetCurrent_Click
        private void muiBtnGetCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (list_tb_database.Count > 0)
            {
                for (int i = 0; i < list_tb_database.Count; i++)
                {
                    if (list_tb_database[i].Active == 1)
                    {
                        dtgDatabase.ScrollIntoView(dtgDatabase.Items.GetItemAt(i));
                        dtgDatabase.SelectedIndex = i;
                    }
                }
            }
        }

        //hplLogout_Click
        private void hplLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ModernDialog md = new ModernDialog();
                md.Buttons = new Button[] { md.YesButton, md.CloseButton, };
                md.Title = FindResource("notification").ToString();
                md.Content = FindResource("are_you_sure_logout").ToString();
                md.YesButton.Content = FindResource("yes").ToString();
                md.CloseButton.Content = FindResource("no").ToString();
                md.YesButton.Focus();
                bool result = md.ShowDialog().Value;

                if (result == true)
                {
                    FileDataStore filedatastore_store = new FileDataStore("TuanNguyen.GoogleDrive.Auth.Store");
                    filedatastore_store.ClearAsync();

                    //set email textblock
                    this.tblEmailAddress.Text = "";
                    this.tblEmailAddress.Visibility = System.Windows.Visibility.Hidden;

                    //set login logout
                    this.tblLogout.Visibility = System.Windows.Visibility.Hidden;
                    this.tblLogin.Visibility = System.Windows.Visibility.Visible;

                    //set null
                    list_tb_database.Clear();
                    dtgDatabase.ItemsSource = null;
                    this.tblTotal.Text = FindResource("checkout_zero").ToString();
                    this.muiBtnBackup.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                ModernDialog md = new ModernDialog();
                md.Title = FindResource("notification").ToString();
                md.Content = ex.Message;
                md.ShowDialog();
            }
        }

        //hplLogin_Click
        private void hplLogin_Click(object sender, RoutedEventArgs e)
        {
            GetDatabase();
        }

        //muiBtnUndo_Click
        private void muiBtnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (current_folder_undo != "")
            {
                ModernDialog md = new ModernDialog();
                md.Buttons = new Button[] { md.YesButton, md.CloseButton, };
                md.Title = FindResource("notification").ToString();
                md.Content = FindResource("are_you_sure").ToString();
                md.YesButton.Content = FindResource("yes").ToString();
                md.CloseButton.Content = FindResource("no").ToString();
                md.YesButton.Focus();
                bool result = md.ShowDialog().Value;

                if (result == true)
                {
                    //if database type is sqlite
                    if (StaticClass.GeneralClass.flag_database_type_general == false)
                    {
                        //close connect
                        ConnectionDB.CloseConnect();

                        if (System.IO.File.Exists(current_directory + @"\DBRES_Local\" + current_folder_undo + @"\CheckOut.db") == true)
                        {
                            System.IO.File.Copy(current_directory + @"\DBRES_Local\" + current_folder_undo + @"\CheckOut.db", current_directory + @"\Databases\CheckOut.db", true);

                            //restart app
                            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                        }

                        //open connect
                        ConnectionDB.OpenConnect();
                    }

                    //if database type is sql server
                    else
                    {
                        //System.IO.Directory.CreateDirectory(current_directory + @"\DBRESSer_Local\" + __current_time);
                        string _restoreFile = current_directory + @"\DBRESSer_Local\" + current_folder_undo.Replace("/", ".").Replace(":", "..") + @"\CheckOut.bak";
                        if (System.IO.File.Exists(_restoreFile))
                        {
                            System.Diagnostics.Debug.WriteLine(_restoreFile);
                            SqlConnection sqlConnect = ConnectionDB.getSQLConnection();
                            var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}' WITH NOFORMAT, NOINIT,  NAME = N'data-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", ConnectionDB.getSqlServerDataBaseName(), current_directory + @"\DBRESSer_Local\" + current_folder_undo.Replace("/", ".").Replace(":", "..") + @"\" + ConnectionDB.getSqlServerDataBaseName() + ".bak");
                            using (var command = new SqlCommand(query, sqlConnect))
                            {
                                //command.ExecuteNonQuery();
                                command.CommandText = "Use Master";
                                command.ExecuteNonQuery();
                                command.CommandText = "ALTER DATABASE " + ConnectionDB.getSqlServerDataBaseName() + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                                command.ExecuteNonQuery();
                                command.CommandText = "RESTORE DATABASE " + ConnectionDB.getSqlServerDataBaseName() + " FROM DISK = '" + @_restoreFile + "' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10; ";
                                command.ExecuteNonQuery();
                                command.CommandText = "Use Master";
                                command.ExecuteNonQuery();
                                command.CommandText = "ALTER DATABASE " + ConnectionDB.getSqlServerDataBaseName() + " SET MULTI_USER;";
                                command.ExecuteNonQuery();
                                command.CommandText = "Use " + ConnectionDB.getSqlServerDataBaseName();
                                command.ExecuteNonQuery();
                            }
                        }
                        else if (System.IO.File.Exists(current_directory + @"\DBRESSer_Local\" + current_folder_undo + @"\CheckOut.sql") == true)
                        {
                            //get connection info
                            System.IO.StreamReader stream_reader = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                            stream_reader.ReadLine();
                            server = stream_reader.ReadLine().Split(':')[1].ToString();
                            id = stream_reader.ReadLine().Split(':')[1].ToString();
                            password = stream_reader.ReadLine().Split(':')[1].ToString();
                            stream_reader.Close();

                            //connection to server
                            string connection_string = "server = " + server + "; user id = " + id + "; password = " + password + "; integrated security = true";
                            SqlConnection sql_connection = new SqlConnection();
                            sql_connection.ConnectionString = connection_string;
                            sql_connection.Open();

                            //insert data
                            System.IO.StreamReader stream_reader_insert = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\DBRESSer_Local\" + current_folder_undo + @"\CheckOut.sql", StaticClass.GeneralClass.key_register_general);
                            string _line;
                            StringBuilder _strImport = new StringBuilder();
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

                            //restart app
                            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                        }
                    }
                }
            }
        }
    }
}
