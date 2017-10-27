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
using System.Data.SqlClient;
using CashierRegisterDAL;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for RestartUndoBackup.xaml
    /// </summary>
    public partial class RestartUndoBackup : ModernDialog
    {
        private Thread thread_undo = null;
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        public string current_folder_undo = "";
        private string sqltype = "";
        private string server_name = "";
        private int authentication = 0;
        private string user_name = "";
        private string password = "";

        //RestartUndoBackup
        public RestartUndoBackup()
        {
            new Thread(() =>
            {
                System.IO.StreamReader stream_reader = StaticClass.GeneralClass.DecryptFileGD(current_directory + @"\sqltype", StaticClass.GeneralClass.key_register_general);
                sqltype = stream_reader.ReadLine().Split(':')[1].ToString();
                server_name = stream_reader.ReadLine().Split(':')[1].ToString();
                Int32.TryParse(stream_reader.ReadLine().Split(':')[1].ToString(), out authentication);
                user_name = stream_reader.ReadLine().Split(':')[1].ToString();
                password = stream_reader.ReadLine().Split(':')[1].ToString();
                stream_reader.Close();
            }).Start();

            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
        }

        //btnRestart_Click
        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            StaticClass.GeneralClass.app_settings["appIsRestart"] = "True";
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        //btnUndo_Click
        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (thread_undo != null && thread_undo.ThreadState == ThreadState.Running) { }
            else
            {
                thread_undo = new Thread(() =>
                {
                    this.btnRestart.Dispatcher.Invoke((Action)(() => { this.btnRestart.IsEnabled = false; }));
                    this.btnUndo.Dispatcher.Invoke((Action)(() => { this.btnUndo.Visibility = System.Windows.Visibility.Collapsed; }));
                    this.mpr.Dispatcher.Invoke((Action)(() =>
                    {
                        this.mpr.Visibility = System.Windows.Visibility.Visible;
                        this.mpr.IsActive = true;
                    }));

                    try
                    {
                        //if database type is sqlite
                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                        {
                            ConnectionDB.CloseConnect();

                            if (System.IO.File.Exists(current_directory + @"\DBRES_Local\" + current_folder_undo + @"\CheckOut.db") == true)
                            {
                                //copy database from current folder undo
                                System.IO.File.Copy(current_directory + @"\DBRES_Local\" + current_folder_undo + @"\CheckOut.db", current_directory + @"\Databases\CheckOut.db", true);

                                //delete database from current folder undo
                                System.IO.Directory.Delete(current_directory + @"\DBRES_Local\" + current_folder_undo, true);
                            }

                            //open connect
                            ConnectionDB.OpenConnect();
                        }

                        //if database type is sql server
                        else
                        {
                            string _fileRestore = current_directory + @"\DBRESSer_Local\" + current_folder_undo + @"\CheckOut.bak";
                            if (System.IO.File.Exists(_fileRestore))
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
                            else if (System.IO.File.Exists(current_directory + @"\DBRESSer_Local\" + current_folder_undo + @"\CheckOut.sql") == true)
                            {
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

                                //insert data from current folder undo
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
                                    catch
                                    {
                                        if (tr != null)
                                        {
                                            try
                                            {
                                                tr.Rollback();
                                            }
                                            catch
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

                                //delete data from current folder undo
                                System.IO.Directory.Delete(current_directory + @"\DBRESSer_Local\" + current_folder_undo, true);
                            }
                        }
                        this.Dispatcher.Invoke((Action)(() => { this.Close(); }));
                    }
                    catch (Exception ex)
                    {
                        this.btnRestart.Dispatcher.Invoke((Action)(() => { this.btnRestart.IsEnabled = true; }));
                        this.btnUndo.Dispatcher.Invoke((Action)(() => { this.btnUndo.Visibility = System.Windows.Visibility.Visible; }));
                        this.mpr.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Collapsed;
                            this.mpr.IsActive = false;
                        }));
                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ex.Message; }));
                    }
                });
                thread_undo.Start();
            }
           
        }
    }
}
