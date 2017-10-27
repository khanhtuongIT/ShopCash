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

using System.Threading;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Logging;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using CashierRegisterEntity;
using CashierRegisterBUS;
using CashierRegisterDAL;
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for BackupGDrive.xaml
    /// </summary>
    public partial class BackupGDrive : ModernDialog
    {
        //current directory
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        //this is full path to the file you want to upload
        //private const string upload_file_name = @"Databases\Checkout.db";
        private string upload_file_name = "";

        //using for date time
        private string current_time = "";
        private System.IO.FileInfo file_info = null;

        //this is a file type to upload
        private const string content_type = @"application/octet-stream";

        //using for user credential
        private bool clientid_folder_flag = false;

        //delegate
        public delegate void muiBtnBackup_Click_Delegate();
        public event muiBtnBackup_Click_Delegate muitbnbackup_delegate;

        //BackupGDrive
        public BackupGDrive()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { };
            muiBtnOK.Focus();
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //if database is sqlite
            if (StaticClass.GeneralClass.flag_database_type_general == false)
                ConnectionDB.OpenConnect();

            if (upload_thread != null && upload_thread.ThreadState == ThreadState.Running)
                new Thread(() => { upload_thread.Abort(); }).Start();

            if (upload_stream != null)
                upload_stream.Dispose();

            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            //if database type is sqlite
            if (StaticClass.GeneralClass.flag_database_type_general == false)
            {
                //close connect
                ConnectionDB.CloseConnect();

                upload_file_name = current_directory + @"\Databases\Checkout.db";
            }
            else
            {
                //check exist of temp folder contain database backup
                if (!System.IO.Directory.Exists(current_directory + @"\Database_Ser_Temp"))
                    System.IO.Directory.CreateDirectory(current_directory + @"\Database_Ser_Temp");

                //check temp database exist
                if (System.IO.File.Exists(current_directory + @"\Database_Ser_Temp\CheckOut.sql") == true)
                    System.IO.File.Delete(current_directory + @"\Database_Ser_Temp\CheckOut.sql");

                //export database to folder Database_Ser_Temp
                string _fileBackup = current_directory + @"\Database_Ser_Temp\CheckOut.bak";
                System.Data.SqlClient.SqlConnection sqlConnect = ConnectionDB.getSQLConnection();
                var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}' WITH NOFORMAT, NOINIT,  NAME = N'data-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", ConnectionDB.getSqlServerDataBaseName(), @_fileBackup);
                try
                {
                    using (var command = new System.Data.SqlClient.SqlCommand(query, sqlConnect))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    System.IO.File.Delete(current_directory + @"\Database_Ser_Temp\CheckOut.bak");
                    tblNotification.Text = FindResource("upload_failed").ToString();
                }
                file_info = new System.IO.FileInfo(_fileBackup);
                if (file_info.Length == 0)
                {
                    System.IO.File.Delete(current_directory + @"\Database_Ser_Temp\CheckOut.bak");
                    tblNotification.Text = FindResource("upload_failed").ToString();
                }
                else
                    upload_file_name = _fileBackup;
            }

            //UserCredential user_credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = client_id, ClientSecret = client_secret }, new[] { DriveService.Scope.Drive }, "user", CancellationToken.None).Result;
            UserCredential user_credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = GeneralClass.client_id, ClientSecret = GeneralClass.client_secret, }, new[] { DriveService.Scope.DriveFile }, "user", CancellationToken.None, new FileDataStore("TuanNguyen.GoogleDrive.Auth.Store")).Result;

            //create the driveservices
            var driveservice = new DriveService(new BaseClientService.Initializer() { HttpClientInitializer = user_credential, ApplicationName = "Backup database" });

            UploadDatabase(driveservice);
        }

        //UploadDatabase
        Thread upload_thread = null;
        System.IO.FileStream upload_stream = null;
        private void UploadDatabase(DriveService driveservice)
        {
            if (upload_thread != null && upload_thread.ThreadState == ThreadState.Running) { }
            else
            {
                try
                {
                    upload_thread = new Thread(() =>
                    {
                        try
                        {
                            upload_stream = new System.IO.FileStream(upload_file_name, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                            current_time = System.DateTime.Now.ToString();
                            string client_folderid_lc = "";
                            string client_folderid_gd = "";
                            this.muiBtnOK.Dispatcher.Invoke((Action)(() => { this.muiBtnOK.Visibility = System.Windows.Visibility.Hidden; }));
                            this.mpr.Dispatcher.Invoke((Action)(() => { this.mpr.IsActive = true; }));
                            this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = "Please wait..."; }));

                            //get all file from google drive
                            FileList file_list = driveservice.Files.List().Execute();

                            //get client folder info
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                using (System.IO.StreamReader streamreader = StaticClass.GeneralClass.DecryptFileGD("ClientFolderInfo", StaticClass.GeneralClass.key_register_general))
                                {
                                    if (streamreader != null)
                                    {
                                        //if database type is sqlite
                                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                                            client_folderid_lc = streamreader.ReadLine().Split(':')[1];

                                        //if database type is sqlserver
                                        else
                                        {
                                            streamreader.ReadLine();
                                            client_folderid_lc = streamreader.ReadLine().Split(':')[1];
                                        }
                                        streamreader.Close();
                                    }
                                }
                            }));


                            for (int i = 0; i < file_list.Items.Count; i++)
                            {
                                if ((file_list.Items[i].Id == client_folderid_lc) && (file_list.Items[i].ExplicitlyTrashed == false))
                                {
                                    clientid_folder_flag = true;
                                    client_folderid_gd = file_list.Items[i].Id;
                                }
                            }

                            //add new folder CashierRegister_Backup if not exist
                            if ((clientid_folder_flag == false) && client_folderid_gd == "")
                            {
                                File folder_client = new File();
                                
                                //if database type is sqlite
                                if (StaticClass.GeneralClass.flag_database_type_general == false)
                                    folder_client.Title = "CashierRegister_Backup";
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
                                    if (System.IO.File.Exists(current_directory + @"\ClientFolderInfo") == true)
                                    {
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
                                    }

                                    streamwriter.Close();
                                    StaticClass.GeneralClass.EncryptFileGD("ClientFolder", "ClientFolderInfo", StaticClass.GeneralClass.key_register_general);
                                    client_folderid_gd = response_folder.Id;
                                }
                            }
                            else
                                clientid_folder_flag = false;

                            //get client folder info
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                System.IO.StreamReader streamreader = streamreader = StaticClass.GeneralClass.DecryptFileGD("ClientFolderInfo", StaticClass.GeneralClass.key_register_general);

                                if (streamreader != null)
                                {
                                    //if database type is sqlite
                                    if (StaticClass.GeneralClass.flag_database_type_general == false)
                                        client_folderid_lc = streamreader.ReadLine().Split(':')[1];

                                    //if database type is sqlserver
                                    else
                                    {
                                        streamreader.ReadLine(); 
                                        client_folderid_lc = streamreader.ReadLine().Split(':')[1];
                                    }
                                    streamreader.Close();
                                }
                            }));

                            if (client_folderid_gd != "" && (client_folderid_gd == client_folderid_lc))
                            {
                                var file_name = upload_file_name;
                                if (file_name.LastIndexOf("\\") != -1)
                                {
                                    //if database type is sqlite
                                    if (StaticClass.GeneralClass.flag_database_type_general == false)
                                        file_name = "Backup_CashierRegister_" + current_time + ".db";
                                    else
                                        file_name = "Backup_CashierRegister_" + current_time + ".bak";
                                    file_name = file_name.Trim().Replace(" ", "_");
                                }

                                //var upload_stream = new System.IO.FileStream(upload_file_name, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);

                                File file_upload = new File();
                                file_upload.Title = file_name;

                                FilesResource.InsertMediaUpload request = driveservice.Files.Insert(new File { Title = file_name, Parents = new List<ParentReference>() { new ParentReference() { Id = client_folderid_gd, } } }, upload_stream, content_type);
                                request.Upload();
                                upload_stream.Dispose();

                                File response_file = request.ResponseBody;

                                this.tblNotification.Dispatcher.Invoke((Action)(() =>
                                {
                                    tblNotification.Text = FindResource("backup_success").ToString();
                                }));
                                if (muitbnbackup_delegate != null)
                                {
                                    this.mpr.Dispatcher.Invoke((Action)(() => { mpr.IsActive = false; }));
                                    this.muiBtnOK.Dispatcher.Invoke((Action)(() => { muiBtnOK.Visibility = System.Windows.Visibility.Visible; }));

                                    muitbnbackup_delegate();
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        this.Close();
                                    }));
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

                            this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("want_backup_database").ToString(); }));
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("have_not_access").ToString(); }));
                        }

                        catch (Exception)
                        {
                            //open connect
                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                                ConnectionDB.OpenConnect();

                            this.tblConfirm.Dispatcher.Invoke((Action)(() => { FindResource("want_backup_database").ToString(); }));
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("have_not_access").ToString(); }));
                        }
                    });
                    upload_thread.SetApartmentState(ApartmentState.STA);
                    upload_thread.Start();
                }
                catch (Exception ex)
                {
                    //open connect
                    if (StaticClass.GeneralClass.flag_database_type_general == false)
                        ConnectionDB.OpenConnect();

                    this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("want_backup_database").ToString(); }));
                    this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ex.Message; }));
                }
            }
        }

    }
}
