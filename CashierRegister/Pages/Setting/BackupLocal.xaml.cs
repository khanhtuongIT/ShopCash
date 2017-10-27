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
using System.Threading;
using System.Data.SqlClient;
using CashierRegisterDAL;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for BackupLocal.xaml
    /// </summary>
    public partial class BackupLocal : UserControl
    {
        //using for database
        private List<EC_tb_Database> list_tb_database = new List<EC_tb_Database>();
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        private string[] directory = null;
        private System.IO.FileInfo file_info = null;

        //GetDatabase
        private BitmapImage bitmapimage_restore;
        private BitmapImage bitmapimage_delete;
        private BitmapImage bitmapimage_undo;
        private Thread thread_getdatabase = null;


        //BackupLocal
        public BackupLocal()
        {
            InitializeComponent();

            bitmapimage_restore = new BitmapImage(new Uri(@"pack://application:,,,/Resources/restore.png", UriKind.Absolute));
            bitmapimage_delete = new BitmapImage(new Uri(@"pack://application:,,,/Resources/delete.png", UriKind.Absolute));
            bitmapimage_undo = new BitmapImage(new Uri(@"pack://application:,,,/Resources/database_undo.png", UriKind.Absolute));

            if (StaticClass.GeneralClass.flag_database_type_general == false)
            {
                //create for sqlite if backup folder when not exist
                if (!System.IO.Directory.Exists(current_directory + @"\DBBK_Local"))
                    System.IO.Directory.CreateDirectory(current_directory + @"\DBBK_Local");
            }
            else
            {
                //create for sqlserver if backup folder when not exist
                if (!System.IO.Directory.Exists(current_directory + @"\DBBKSer_Local"))
                    System.IO.Directory.CreateDirectory(current_directory + @"\DBBKSer_Local");
            }
        }

        //UserControl_Loaded
        private bool flag_check_loaded = false;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
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
                            this.dtgDatabase.Dispatcher.Invoke((Action)(() =>
                            {
                                dtgDatabase.Visibility = System.Windows.Visibility.Hidden;
                                dtgDatabase.ItemsSource = null;
                            }));

                            this.mpr.Dispatcher.Invoke((Action)(() => 
                            {
                                this.mpr.Visibility = System.Windows.Visibility.Visible;
                                this.mpr.IsActive = true; 
                            }));
                            
                            list_tb_database.Clear();

                            //create folder backup
                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                            {
                                if (!System.IO.Directory.Exists(current_directory + @"\DBBK_Local"))
                                    System.IO.Directory.CreateDirectory(current_directory + @"\DBBK_Local");
                                directory = System.IO.Directory.GetDirectories(current_directory + @"\DBBK_Local");
                            }
                            else
                            {
                                if (!System.IO.Directory.Exists(current_directory + @"\DBBKSer_Local"))
                                    System.IO.Directory.CreateDirectory(current_directory + @"\DBBKSer_Local");
                                directory = System.IO.Directory.GetDirectories(current_directory + @"\DBBKSer_Local");
                            }

                            string directory_name = "";
                            string _directory_name = "";
                            string __directory_name = "";

                            int id = 0;
                            bool flag = false;
                            for (int i = 0; i < directory.Length; i++)
                            {
                                flag = false;
                                id++;
                                directory_name = System.IO.Path.GetFileName(directory[i]);
                                _directory_name = directory_name.Replace("..", ":");
                                __directory_name = _directory_name.Replace(".", "/");

                                //using for  sqlite
                                if (StaticClass.GeneralClass.flag_database_type_general == false)
                                {
                                    //check exist for database in folder
                                    if (System.IO.File.Exists(directory[i] + @"\CheckOut.db") == true)
                                    {
                                        file_info = new System.IO.FileInfo(directory[i] + @"\CheckOut.db");
                                        if (file_info.Length == 0)
                                        {
                                            System.IO.Directory.Delete(directory[i], true);
                                            flag = true;
                                        }
                                    }
                                    else
                                    {
                                        System.IO.Directory.Delete(directory[i], true);
                                        flag = true;
                                    }
                                }
                                //using for sqlserver
                                else
                                {
                                    //check exist for database in folder
                                    if (System.IO.File.Exists(directory[i] + @"\CheckOut.sql") == true)
                                    {
                                        file_info = new System.IO.FileInfo(directory[i] + @"\CheckOut.sql");
                                        if (file_info.Length == 0)
                                        {
                                            System.IO.Directory.Delete(directory[i], true);
                                            flag = true;
                                        }
                                    }
                                    else if (System.IO.File.Exists(directory[i] + @"\CheckOut.bak") == true)
                                    {
                                        file_info = new System.IO.FileInfo(directory[i] + @"\CheckOut.bak");
                                        if (file_info.Length == 0)
                                        {
                                            System.IO.Directory.Delete(directory[i], true);
                                            flag = true;
                                        }
                                    }
                                    else
                                    {
                                        System.IO.Directory.Delete(directory[i], true);
                                        flag = true;
                                    }
                                }

                                if (flag == false)
                                {
                                    EC_tb_Database ec_tb_database = new EC_tb_Database();
                                    ec_tb_database.Id = id;
                                    ec_tb_database.BackupDate = __directory_name;

                                    //get file info 
                                    //if database type is sqlite
                                    if (StaticClass.GeneralClass.flag_database_type_general == false)
                                        file_info = new System.IO.FileInfo(directory[i] + @"\CheckOut.db");
                                    else
                                    {
                                        if (System.IO.File.Exists(directory[i] + @"\CheckOut.bak"))
                                            file_info = new System.IO.FileInfo(directory[i] + @"\CheckOut.bak");
                                        else file_info = new System.IO.FileInfo(directory[i] + @"\CheckOut.sql");
                                    }
                                    ec_tb_database.FileSize = (file_info.Length / 1000).ToString() + "KB";
                                    ec_tb_database.BitmapImage_Delete = bitmapimage_delete;
                                    ec_tb_database.BitmapImage_Restore = bitmapimage_restore;

                                    list_tb_database.Add(ec_tb_database);
                                }
                            }

                            Thread.Sleep(500);
                            this.mpr.Dispatcher.Invoke((Action)(() =>
                            {
                                this.mpr.Visibility = System.Windows.Visibility.Hidden;
                                this.mpr.IsActive = false;
                            }));

                            this.dtgDatabase.Dispatcher.Invoke((Action)(() =>
                            {
                                dtgDatabase.Visibility = System.Windows.Visibility.Visible;
                                dtgDatabase.ItemsSource = list_tb_database;
                            }));
                            this.tblTotal.Dispatcher.Invoke((Action)(() => { this.tblTotal.Text = FindResource("total").ToString() + "(" + list_tb_database.Count.ToString() + ")"; }));
                        }
                        catch (Exception)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                ModernDialog md = new ModernDialog();
                                md.Title = FindResource("notification").ToString();
                                md.Content = FindResource("have_not_access").ToString();
                                md.CloseButton.Content = FindResource("close").ToString();
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
                    md.Title = FindResource("notification").ToString();
                    md.Content = ex.Message;
                    md.CloseButton.Content = FindResource("close").ToString();
                    md.ShowDialog();
                }));
            }
        }

        //btnRestore_Click
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDatabase.SelectedIndex > -1)
            {
                RestoreLocal_Confirm page = new RestoreLocal_Confirm();
                page.folder_restore = list_tb_database[dtgDatabase.SelectedIndex].BackupDate;
                page.ShowDialog();
            }
        }

        //btnDelete_Click
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDatabase.SelectedIndex > -1)
            {
                DeleteLocal_Confirm page = new DeleteLocal_Confirm();
                page.folder_delete = list_tb_database[dtgDatabase.SelectedIndex].BackupDate;
                page.btndelete_delegate += btnDelete_Click_Delegate;
                page.ShowDialog();
            }
        }

        //btnDelete_Click_Delegate
        private void btnDelete_Click_Delegate()
        {
            GetDatabase();
        }

        //muiBtnBackup_Click
        private void muiBtnBackup_Click(object sender, RoutedEventArgs e)
        {
            BackupLocal_Confirm page = new BackupLocal_Confirm();
            page.muitbnbackup_delegate += muiBtnBackup_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnBackup_Click_Delegate
        private void muiBtnBackup_Click_Delegate()
        {
            GetDatabase();
        }

    }
}
