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
using System.Data;
using System.Data.SqlClient;
using CashierRegisterDAL;


namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for RestoreLocal_Confirm.xaml
    /// </summary>
    public partial class RestoreLocal_Confirm : ModernDialog
    {
        //current directory
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        public string folder_restore = "";
        private string _folder_restore = "";
        private string __folder_restore = "";

        private Thread thread_restore = null;
        private System.IO.FileInfo file_info = null;
        private bool flag = false;
        private string current_time = "";
        private string _current_time = "";
        private string __current_time = "";
        private string current_folder_undo = "";
        private string sqltype = "";
        private string server_name = "";
        private int authentication = 0;
        private string user_name = "";
        private string password = "";

        //RestoreLocal_Confirm
        public RestoreLocal_Confirm()
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

            if (thread_restore != null && thread_restore.ThreadState == ThreadState.Running)
            {
                new Thread(() =>
                {
                    thread_restore.Abort();
                }).Start();
            }

            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (thread_restore != null && thread_restore.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_restore = new Thread(() =>
                    {
                        try
                        {
                            if (folder_restore == "")
                            {
                                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text =  FindResource("cannot_find_database").ToString(); }));
                                return;
                            }
                            else
                            {
                                current_time = System.DateTime.Now.ToString();
                                flag = false;

                                this.muiBtnOK.Dispatcher.Invoke((Action)(() => { muiBtnOK.Visibility = System.Windows.Visibility.Hidden; }));
                                this.mpr.Dispatcher.Invoke((Action)(() => 
                                {
                                    this.mpr.Visibility = System.Windows.Visibility.Visible;
                                    this.mpr.IsActive = true; 
                                }));
                                this.tblNotification.Dispatcher.Invoke((Action)(() => { tblNotification.Text = ""; }));
                                this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("please_wait").ToString(); }));

                                //if database type is sqlite
                                if (StaticClass.GeneralClass.flag_database_type_general == false)
                                {
                                    //close connect
                                    ConnectionDB.CloseConnect();

                                    //create folder undo database
                                    if (System.IO.Directory.Exists(current_directory + @"\DBRES_Local") == false)
                                        System.IO.Directory.CreateDirectory(current_directory + @"\DBRES_Local");

                                    //copy database to folder undo
                                    if (System.IO.Directory.Exists(current_directory + @"\Databases") == true)
                                    {
                                        if (System.IO.File.Exists(current_directory + @"\Databases\CheckOut.db") == true)
                                        {
                                            file_info = new System.IO.FileInfo(current_directory + @"\Databases\CheckOut.db");
                                            if (file_info.Length == 0)
                                                flag = true;
                                        }
                                        else
                                            return;
                                    }
                                    else
                                        return;
                                }

                                //if database is sqlserver
                                else
                                {
                                    //create folder undo database
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
                                        System.IO.Directory.CreateDirectory(current_directory + @"\DBRES_Local\" + __current_time);
                                        System.IO.File.Copy(current_directory + @"\Databases\CheckOut.db", current_directory + @"\DBRES_Local\" + __current_time + @"\CheckOut.db");
                                        System.IO.File.Delete(current_directory + @"\Databases\CheckOut.db");

                                        //copy backup database for databases folder
                                        _folder_restore = folder_restore.Replace("/", ".");
                                        __folder_restore = _folder_restore.Replace(":", "..");
                                        System.IO.File.Copy(current_directory + @"\DBBK_Local\" + __folder_restore + @"\CheckOut.db", current_directory + @"\Databases\CheckOut.db", true);

                                        //check CheckOut size
                                        file_info = new System.IO.FileInfo(current_directory + @"\Databases\CheckOut.db");
                                        if (file_info.Length == 0)
                                        {
                                            System.IO.File.Delete(current_directory + @"\Databases\CheckOut.db");
                                            System.IO.File.Copy(current_directory + @"\DBRES_Local\" + __current_time + @"\CheckOut.db", current_directory + @"\Databases\CheckOut.db");
                                        }
                                    }
                                    //if database type is sqlserver
                                    else
                                    {
                                        System.IO.Directory.CreateDirectory(current_directory + @"\DBRESSer_Local\" + __current_time);
                                        string _restoreFile = current_directory + @"\DBBKSer_Local\" + folder_restore.Replace("/", ".").Replace(":", "..") + @"\CheckOut.bak";
                                        if (System.IO.File.Exists(_restoreFile))
                                        {
                                            System.Diagnostics.Debug.WriteLine(_restoreFile);
                                            SqlConnection sqlConnect = ConnectionDB.getSQLConnection();
                                            var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}' WITH NOFORMAT, NOINIT,  NAME = N'data-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", ConnectionDB.getSqlServerDataBaseName(), current_directory + @"\DBRESSer_Local\" + __current_time + @"\" + ConnectionDB.getSqlServerDataBaseName() + ".bak");
                                            using (var command = new SqlCommand(query, sqlConnect))
                                            {
                                                command.ExecuteNonQuery();
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
                                        else
                                        {
                                            //export CheckOut current into auto backup folder
                                            if (StaticClass.GeneralClass.ExportSQL(current_directory + @"\DBRESSer_Local\" + __current_time + @"\CheckOut_.sql") == true)
                                            {
                                                //encrypt file
                                                StaticClass.GeneralClass.EncryptFileGD(current_directory + @"\DBRESSer_Local\" + __current_time + @"\CheckOut_.sql", current_directory + @"\DBRESSer_Local\" + __current_time + @"\CheckOut.sql", StaticClass.GeneralClass.key_register_general);

                                                if (System.IO.File.Exists(current_directory + @"\sqltype") == true)
                                                {
                                                    _folder_restore = folder_restore.Replace("/", ".");
                                                    __folder_restore = _folder_restore.Replace(":", "..");

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
                                                    System.IO.StreamReader stream_reader_insert = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\DBBKSer_Local\" + __folder_restore + @"\CheckOut.sql", StaticClass.GeneralClass.key_register_general);
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
                                                }
                                            }
                                            else
                                                System.IO.Directory.Delete(current_directory + @"\DBRESSer_Local\" + __current_time);
                                        }
                                    }
                                }

                                Thread.Sleep(500);
                                this.mpr.Dispatcher.Invoke((Action)(() => 
                                {
                                    this.mpr.Visibility = System.Windows.Visibility.Hidden;
                                    mpr.IsActive = false; 
                                }));
                                this.muiBtnOK.Dispatcher.Invoke((Action)(() => { muiBtnOK.Visibility = System.Windows.Visibility.Visible; }));
                                this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_restore").ToString(); }));
                                this.tblNotification.Dispatcher.Invoke((Action)(() => { tblNotification.Text = FindResource("restore_success").ToString(); })); ;

                                //open connect
                                if (StaticClass.GeneralClass.flag_database_type_general == false)
                                    ConnectionDB.OpenConnect();

                                if (flag == false)
                                {
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        this.Close();
                                        Pages.Setting.RestartUndoBackup page = new Pages.Setting.RestartUndoBackup();
                                        page.current_folder_undo = current_folder_undo;
                                        page.ShowDialog();
                                    }));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            //open connect
                            if (StaticClass.GeneralClass.flag_database_type_general == false)
                                ConnectionDB.OpenConnect();

                            this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_restore").ToString(); }));
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("have_not_access").ToString(); }));
                        }
                    });
                    thread_restore.Start();
                }
            }
            catch (Exception)
            {
                //open connect
                if (StaticClass.GeneralClass.flag_database_type_general == false)
                    ConnectionDB.OpenConnect();

                this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_restore").ToString(); }));
                this.tblNotification.Dispatcher.Invoke((Action)(() => { tblNotification.Text = FindResource("have_not_access").ToString(); }));
            }
        }
        
    }
}
