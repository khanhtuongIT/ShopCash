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
using System.Data;
using CashierRegisterBUS;
using CashierRegisterDAL;


namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for BackupLocal_Confirm.xaml
    /// </summary>
    public partial class BackupLocal_Confirm : ModernDialog
    {
        //current directory
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        //file info
        private System.IO.FileInfo file_info = null;

        //thread
        private Thread thread_backup = null;

        //using for date time
        private string current_time = "";
        private string _current_time = "";
        private string __current_time = "";

        //delegate
        public delegate void muiBtnBackup_Click_Delegate();
        public event muiBtnBackup_Click_Delegate muitbnbackup_delegate;

        //BackupLocal_Confirm
        public BackupLocal_Confirm()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { };
            muiBtnOK.Focus();
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //open connect
            if (StaticClass.GeneralClass.flag_database_type_general == false)
                ConnectionDB.OpenConnect();

            if (thread_backup != null && thread_backup.ThreadState == ThreadState.Running)
            {
                new Thread(() =>
                {
                    thread_backup.Abort();
                }).Start();
            }
            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (thread_backup != null && thread_backup.ThreadState == ThreadState.Running) { }
            else
            {
                thread_backup = new Thread(() =>
                {
                    try
                    {
                        this.muiBtnOK.Dispatcher.Invoke((Action)(() => { this.muiBtnOK.Visibility = System.Windows.Visibility.Hidden; }));
                        this.mpr.Dispatcher.Invoke((Action)(() => 
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Visible;
                            this.mpr.IsActive = true; 
                        }));
                        this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("please_wait").ToString(); }));
                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ""; }));

                        current_time = System.DateTime.Now.ToString();

                        //if database type is sqlite
                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                        {
                            //close connect
                            ConnectionDB.CloseConnect();

                            if (System.IO.Directory.Exists(current_directory + @"\DBBK_Local") == true)
                            {
                                if (current_time != "")
                                {
                                    _current_time = current_time.Replace("/", ".");
                                    __current_time = _current_time.Replace(":", "..");
                                    System.IO.Directory.CreateDirectory(current_directory + @"\DBBK_Local" + @"\" + __current_time);

                                    if (System.IO.File.Exists(current_directory + @"\Databases\CheckOut.db") == true)
                                    {
                                        file_info = new System.IO.FileInfo(current_directory + @"\Databases\CheckOut.db");
                                        if (file_info.Length > 0)
                                            System.IO.File.Copy(current_directory + @"\Databases\CheckOut.db", current_directory + @"\DBBK_Local" + @"\" + __current_time + @"\CheckOut.db");
                                    }
                                }

                                //check size of database
                                if (System.IO.File.Exists(current_directory + @"\DBBK_Local" + @"\" + __current_time + @"\CheckOut.db") == true)
                                {
                                    file_info = new System.IO.FileInfo(current_directory + @"\DBBK_Local" + @"\" + __current_time + @"\CheckOut.db");
                                    if (file_info.Length == 0)
                                        System.IO.Directory.Delete(current_directory + @"\DBBK_Local" + @"\" + __current_time, true);
                                }
                            }
                        }

                        //if database type is sqlserver
                        else
                        {
                            if (System.IO.Directory.Exists(current_directory + @"\DBBKSer_Local") == true)
                            {
                                if (current_time != "")
                                {
                                    _current_time = current_time.Replace("/", ".");
                                    __current_time = _current_time.Replace(":", "..");
                                    string _bkFolder = current_directory + @"\DBBKSer_Local\" + __current_time;
                                    System.IO.Directory.CreateDirectory(_bkFolder);
                                    string _fileBackup = _bkFolder + @"\" + ConnectionDB.getSqlServerDataBaseName() + ".bak";
                                    System.Data.SqlClient.SqlConnection sqlConnect = ConnectionDB.getSQLConnection();
                                    var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}' WITH NOFORMAT, NOINIT,  NAME = N'data-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", ConnectionDB.getSqlServerDataBaseName(), @_fileBackup);
                                    using (var command = new System.Data.SqlClient.SqlCommand(query, sqlConnect))
                                    {
                                        command.ExecuteNonQuery();
                                    }

                                    /*if (StaticClass.GeneralClass.ExportSQL(current_directory + @"\DBBKSer_Local" + @"\" + __current_time + @"\CheckOut_.sql") == true)
                                    {
                                        //encrypt file
                                        StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\DBBKSer_Local" + @"\" + __current_time + @"\CheckOut_.sql", current_directory + @"\DBBKSer_Local" + @"\" + __current_time + @"\CheckOut.sql", StaticClass.GeneralClass.key_register_general);

                                        file_info = new System.IO.FileInfo(current_directory + @"\DBBKSer_Local" + @"\" + __current_time + @"\CheckOut.sql");

                                        //delete database file backup if size is zero
                                        if (file_info.Length == 0)
                                            System.IO.Directory.Delete(current_directory + @"\DBBKSer_Local" + @"\" + __current_time + @"\CheckOut.sql", true);
                                    }*/
                                }
                            }
                        }
                        Thread.Sleep(500);
                        this.mpr.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Hidden;
                            this.mpr.IsActive = false;
                        }));
                        this.muiBtnOK.Dispatcher.Invoke((Action)(() => { this.muiBtnOK.Visibility = System.Windows.Visibility.Visible; }));
                        this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("want_backup_database").ToString(); }));
                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("backup_success").ToString(); }));
                        
                        //open connect
                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                            ConnectionDB.OpenConnect();

                        if (muitbnbackup_delegate != null)
                        {
                            muitbnbackup_delegate();
                            this.Dispatcher.Invoke((Action)(() => { this.Close(); }));
                        }
                    }
                    catch (Exception ex)
                    {
                        //open connect
                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                            ConnectionDB.OpenConnect();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("Close").ToString();
                            md.Content = ex.Message;
                            md.Title = FindResource("notification").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_backup.Start();
            }
        }

    }
}
