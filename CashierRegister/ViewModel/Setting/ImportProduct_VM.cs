using CashierRegister.Helpers;
using CashierRegisterBUS;
using CashierRegisterEntity;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegister.ViewModel.Setting
{
    class ImportProduct_VM : ModelBase
    {
        public RelayCommand OkImportCmd { get; private set; }
        public RelayCommand CancelImportCmd { get; private set; }
        public RelayCommand BrowseCmd { get; private set; }
        public RelayCommand GetSampleFileCmd { get; private set; }
        private TypeImport _typeImport = TypeImport.Append;
        public TypeImport OptionsImport
        {
            get { return _typeImport; }
            set
            {
                _typeImport = value;
                RaisePropertyChanged("OptionsImport");
            }
        }
        private string _fileName = string.Empty;
        public string FileNameImport
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged("FileNameImport");
            }
        }
        private const string _tblName = "tb_Product";
        private Boolean _isShowProgress = false;
        public Boolean IsShowProgress
        {
            get { return _isShowProgress; }
            set
            {
                _isShowProgress = value;
                RaisePropertyChanged("IsShowProgress");
            }
        }
        private System.Windows.Visibility _isVisibleButton = System.Windows.Visibility.Visible;
        public System.Windows.Visibility IsVisibleButton
        {
            get { return _isVisibleButton; }
            set
            {
                _isVisibleButton = value;
                RaisePropertyChanged("IsVisibleButton");
            }
        }
        private System.Windows.Visibility _isVisError = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility IsVisError
        {
            get { return _isVisError; }
            set
            {
                _isVisError = value;
                RaisePropertyChanged("IsVisError");
            }
        }
        private BUS_tb_Category bus_tb_category = new BUS_tb_Category();
        private string _lstColumn = string.Empty;
        private const string _strColumnName = "Category,ShortName,LongName,Cost,Price,Taxable,Qty,Barcode";
        private static string _bkFolderNameSQL = string.Format(@"{0}\DBRESSer_Local\{1:M.dd.yyyy HH..mm..ss tt}", Directory.GetCurrentDirectory(), DateTime.Now);
        private static string _bkFolderNameSQLite = string.Format(@"{0}\DBRES_Local\{1:M.dd.yyyy HH..mm..ss tt}", Directory.GetCurrentDirectory(), DateTime.Now);
        private List<string> _lstCatName = new List<string>();
        private string _getColumnName = string.Empty;
        private string _showError = string.Empty;
        public string ShowError
        {
            get { return _showError; }
            set
            {
                _showError = value;
                RaisePropertyChanged("ShowError");
            }
        }
        private Microsoft.Win32.SaveFileDialog _dlg = null;
        public ImportProduct_VM()
        {
            CancelImportCmd = new RelayCommand(onCancelImportExec);
            BrowseCmd = new RelayCommand(onBrowseExec);
            OkImportCmd = new RelayCommand(onOkImportExec);
            GetSampleFileCmd = new RelayCommand(onGetSampleFileExec);
        }
        private void onGetSampleFileExec(object _param)
        {
            _dlg = new Microsoft.Win32.SaveFileDialog();
            _dlg.FileName = string.Format("file_sample_Product{0:MMMddyyyy_HHmmssfff}", DateTime.Now); // Default file name
            _dlg.DefaultExt = ".zip"; // Default file extension
            _dlg.Filter = "Text documents (.zip)|*.zip";
            Nullable<bool> result = _dlg.ShowDialog();
            if (result == true)
            {
                Ionic.Zip.ZipFile _zipArc = new Ionic.Zip.ZipFile();
                using (StreamWriter stream_writer = new StreamWriter("tb_Product.csv", false, System.Text.Encoding.UTF8))
                {
                    stream_writer.WriteLine(_strColumnName);
                    stream_writer.WriteLine("\"Books\",\"2-Seater bench\",\"Strathwood Gibranta All-Weather Hardwood 2-Seater Bench\",0.00,129.99,1,9,\"\"");
                    stream_writer.WriteLine("\"Clothing\",\"Baby Dog\",\"Beach Baby Dog Clothing\",0.00,8.49,1,0,\"\"");
                    stream_writer.Close();
                }
                using (StreamWriter stream_writer = new StreamWriter("help.txt", false, System.Text.Encoding.UTF8))
                {
                    stream_writer.WriteLine("1.\tOpen sample file.");
                    stream_writer.WriteLine("\t\t1.1 Go to File > Open.");
                    stream_writer.WriteLine("\t\t\tIf you're using Excel 2007, click the Microsoft Office Button, and then click Open.");
                    stream_writer.WriteLine("\t\t1.2 Select CSV Files from the Open dialog box.");
                    stream_writer.WriteLine("\t\t1.3 Locate and double-click the csv file (tb_Product.csv) that you want to open.");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("2.\tInput data that you want");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("3.\tSave file");
                    stream_writer.WriteLine("\t\t3.1 In your Excel workbook, switch to the File tab, and then click Save As. Alternatively, you can press F12 to open the same Save As dialog.");
                    stream_writer.WriteLine("\t\t3.2 In the Save as type box, choose to save your Excel file as CSV (Comma delimited).");
                    stream_writer.WriteLine("\t\t3.3 Choose the destination folder where you want to save your Excel file in the CSV format, and then click Save.");
                    stream_writer.WriteLine("\t\t3.4 The first dialog reminds you that only the active Excel spreadsheet will be saved to the CSV file format. If this is what you are looking for, click OK.");
                    stream_writer.WriteLine("\t\t3.5 Clicking OK in the first dialog will display a second message informing you that your worksheet may contain features unsupported by the CSV encoding. This is Okay, so simply click Yes.");
                    stream_writer.Close();
                }
                _zipArc.AddFile("tb_Product.csv", string.Empty);
                _zipArc.AddFile("help.txt", string.Empty);
                _zipArc.Save(_dlg.FileName);
                _zipArc.Dispose();
            }
        }
        private void onCancelImportExec(object _param)
        {
            OnRequestClose();
        }
        private void onBrowseExec(object _param)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog _dlg = new Microsoft.Win32.OpenFileDialog();
                _dlg.Multiselect = false;
                _dlg.Filter = "Text documents CSV|*.csv";
                Nullable<bool> result = _dlg.ShowDialog();
                if (result == true)
                {
                    FileNameImport = _dlg.FileName;
                    using (StreamReader reader = new StreamReader(FileNameImport))
                    {
                        _lstColumn = reader.ReadLine().Trim();
                    }
                }
            }
            catch
            {
                FirstFloor.ModernUI.Windows.Controls.ModernDialog mdd = new FirstFloor.ModernUI.Windows.Controls.ModernDialog();
                mdd.Buttons = new System.Windows.Controls.Button[] { mdd.CloseButton };
                mdd.CloseButton.TabIndex = 0;
                mdd.CloseButton.Content = App.Current.FindResource("close").ToString();
                mdd.Height = 200;
                mdd.Title = App.Current.FindResource("notification").ToString();
                mdd.Content = App.Current.FindResource("file_err_open").ToString();
                mdd.CloseButton.Focus();
                mdd.ShowDialog();
            }
        }
        private async void onOkImportExec(object _param)
        {
            if (string.IsNullOrEmpty(FileNameImport))
            {
                ShowError = App.Current.FindResource("sel_2_import").ToString();
                IsVisError = System.Windows.Visibility.Visible;
                return;
            }
            else if (_lstColumn != _strColumnName)
            {
                ShowError = App.Current.FindResource("content_file_invalid").ToString();
                IsVisError = System.Windows.Visibility.Visible;
                return;
            }
            else
            {
                ShowError = string.Empty;
                IsVisError = System.Windows.Visibility.Collapsed;
            }
            FirstFloor.ModernUI.Windows.Controls.ModernDialog mdd = new FirstFloor.ModernUI.Windows.Controls.ModernDialog();
            mdd.Buttons = new System.Windows.Controls.Button[] { mdd.OkButton, mdd.CancelButton, };
            mdd.OkButton.TabIndex = 0;
            mdd.OkButton.Content = App.Current.FindResource("ok").ToString();
            mdd.CancelButton.TabIndex = 1;
            mdd.CancelButton.Content = App.Current.FindResource("cancel").ToString();
            mdd.TabIndex = -1;
            mdd.Height = 200;
            mdd.Title = App.Current.FindResource("notification").ToString();
            mdd.Content = App.Current.FindResource("confirm_import").ToString();
            mdd.OkButton.Focus();
            mdd.ShowDialog();
            if (mdd.MessageBoxResult == System.Windows.MessageBoxResult.OK)
            {
                IsShowProgress = true;
                IsVisibleButton = System.Windows.Visibility.Collapsed;
                string _fileImport = FileNameImport;
                _getColumnName = getListColumnName(_tblName).Replace(", ", ",").Substring(12);
                string _isBackup = (StaticClass.GeneralClass.flag_database_type_general) ? _AutoBackupRestoreSqlServer(BackupRestore.backup, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName()) : _AutoBackupRestoreSqlite(BackupRestore.backup);
                if ("Success" == _isBackup)
                    {
                    var slowTask = Task<string>.Factory.StartNew(() => this._ImportFromCSV(Path.GetFileNameWithoutExtension(_fileImport), _fileImport));
                    await slowTask;
                    IsShowProgress = false;
                    IsVisibleButton = System.Windows.Visibility.Visible;
                    if (slowTask.Result.ToString() == "Success")
                    {
                        string _strText = App.Current.FindResource("import_success").ToString().Replace("$$", App.Current.FindResource("cash_register").ToString());
                        System.Windows.MessageBox.Show(_strText, "Successfull");
                        FileInfo fi = new System.IO.FileInfo(_fileImport);
                        File.Delete(fi.DirectoryName.ToString() + "\\schema.ini");
                        StaticClass.GeneralClass.app_settings["appIsRestart"] = true;
                        Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                        System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                        System.Windows.Application.Current.Shutdown();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(App.Current.FindResource("cannot_import").ToString() + " \t\t\t\t", "Error");
                        string _rs = (StaticClass.GeneralClass.flag_database_type_general) ? _AutoBackupRestoreSqlServer(BackupRestore.restore, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName()) : _AutoBackupRestoreSqlite(BackupRestore.restore);
                        if (_rs != "Success")
                        {
                            System.Windows.MessageBox.Show(App.Current.FindResource("import_err_restore_err").ToString(), "Error");
                        }
                        FileInfo fi = new System.IO.FileInfo(_fileImport);
                        File.Delete(fi.DirectoryName.ToString() + "\\schema.ini");
                    }
                }
                else
                {
                    IsShowProgress = false;
                    IsVisibleButton = System.Windows.Visibility.Visible;
                    System.Windows.MessageBox.Show(App.Current.FindResource("error_before_import").ToString(), "Error");
                }
            }
        }
        private string _ImportFromCSV(string _file, string _fileImport)
        {
            string _strImport = _BuildStringImportFormCSV(_fileImport);
            if (string.IsNullOrEmpty(_strImport))
            {
                return "Success";
            }
            foreach (string _str in _lstCatName)
            {
                long _catID = getCatIdFromCatName(_str);
                _strImport = _strImport.Replace("'<" + _str + "/>'", _catID.ToString());
            }
            if (StaticClass.GeneralClass.flag_database_type_general) //SQL Server
            {
                SqlConnection sqlConnect = CashierRegisterDAL.ConnectionDB.getSQLConnection();
                SqlTransaction tr = null;
                SqlCommand cmd = null;
                try
                {
                    using (tr = sqlConnect.BeginTransaction())
                    {
                        using (cmd = sqlConnect.CreateCommand())
                        {
                            if (!string.IsNullOrEmpty(_strImport))
                            {
                                cmd.Transaction = tr;
                                if (OptionsImport == TypeImport.Overwrite)
                                {
                                    cmd.CommandText = "TRUNCATE TABLE " + _tblName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.CommandText = _strImport.Replace("<N/>", "N");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tr.Commit();
                    }
                    tr.Dispose();
                    cmd.Dispose();
                    return "Success";
                }
                catch (SQLiteException ex)
                {
                    if (tr != null)
                    {
                        try
                        {
                            tr.Rollback();
                            return App.Current.FindResource("cannot_import").ToString();
                        }
                        catch (ObjectDisposedException ex2)
                        {
                            return ex2.Message;
                        }
                        finally
                        {
                            tr.Dispose();
                        }
                    }
                    return ex.Message;
                }
            }
            else
            {
                SQLiteConnection sqconn = CashierRegisterDAL.ConnectionDB.getSQLiteConnection();
                SQLiteTransaction tr = null;
                SQLiteCommand cmd = null;
                try
                {
                    using (tr = sqconn.BeginTransaction())
                    {
                        using (cmd = sqconn.CreateCommand())
                        {
                            if (!string.IsNullOrEmpty(_strImport))
                            {
                                cmd.Transaction = tr;
                                if (OptionsImport == TypeImport.Overwrite)
                                {
                                    cmd.CommandText = "delete from " + _tblName + "; UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = '"+ _tblName + "'; ";
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.CommandText = _strImport.Replace("<N/>", "");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tr.Commit();
                    }
                    tr.Dispose();
                    cmd.Dispose();
                    return "Success";
                }
                catch (SQLiteException ex)
                {
                    if (tr != null)
                    {
                        try
                        {
                            tr.Rollback();
                            return App.Current.FindResource("cannot_import").ToString();
                        }
                        catch (ObjectDisposedException ex2)
                        {
                            return ex2.Message + " 1";
                        }
                        finally
                        {
                            tr.Dispose();
                        }
                    }
                    return ex.Message + " 2";
                }
            }
        }
        private string _BuildStringImportFormCSV(string _pathFileImport)
        {
            string _insert = string.Empty;
            writeSchema(_pathFileImport);
            FileInfo fi = new FileInfo(_pathFileImport);
            string strConnString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + fi.DirectoryName.ToString() + ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
            string sql_select = "select * from [" + fi.Name.ToString() + "]";
            string _sqlImport = string.Empty;
            OdbcConnection conn;
            conn = new OdbcConnection(strConnString.Trim());
            conn.Open();
            OdbcCommand commandSourceData = new OdbcCommand(sql_select, conn);
            OdbcDataReader dataReader = commandSourceData.ExecuteReader();
            int _ttField = dataReader.FieldCount;
            if (_lstColumn == _strColumnName)
            {
                while (dataReader.Read())
                {
                    _lstCatName.Add(dataReader[0].ToString());
                    string _short = (string.IsNullOrEmpty(dataReader[1].ToString())) ? "''" : "<N/>'" + dataReader[1].ToString().Replace(@"\u0022", "\"").Replace("'", "''") + "'";
                    string _long = (string.IsNullOrEmpty(dataReader[2].ToString())) ? "''" : "<N/>'" + dataReader[2].ToString().Replace(@"\u0022", "\"").Replace("'", "''") + "'";
                    string _cost = "'" + dataReader[3].ToString() + "'";
                    string _price = "'" + dataReader[4].ToString() + "'";
                    string _tax = "'" + dataReader[5].ToString() + "'";
                    string _qty = "'" + dataReader[6].ToString() + "'";
                    string _code = "'" + dataReader[7].ToString() + "'";

                    string _strVal = string.Format("{0},{1},{2},{3},{4},{5},'<{6}/>',{7},{8},{9},{10},{11},{12}", _code, _short, _long, _cost, _price, _qty, dataReader[0].ToString(), _tax, "''", "'-1'", "'1'", "''", "''");
                    _insert = string.Format("insert into {0} ({1}) values ({2}); ", _tblName, _getColumnName, _strVal);
                    _sqlImport += _insert;
                }
            }
            conn.Close();
            System.Threading.Thread.Sleep(500);
            return _sqlImport;
        }
        private string _AutoBackupRestoreSqlServer(BackupRestore _opts, string _DBName)
        {
            string _info = "Success";
            string _fileBackup = string.Format(@"{0}\{1}.bak", _bkFolderNameSQL, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName());
            if (_opts == BackupRestore.backup)
            {
                if (!Directory.Exists(_bkFolderNameSQL))
                {
                    Directory.CreateDirectory(_bkFolderNameSQL);
                }
                SqlConnection sqlConnect = CashierRegisterDAL.ConnectionDB.getSQLConnection();
                var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}' WITH NOFORMAT, NOINIT,  NAME = N'data-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", _DBName, @_fileBackup);
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
            else if (_opts == BackupRestore.restore)
            {
                if (File.Exists(_fileBackup))
                {
                    CashierRegisterDAL.ConnectionDB.OpenConnect();
                    SqlConnection sqlConnect = CashierRegisterDAL.ConnectionDB.getSQLConnection();
                    using (var command = sqlConnect.CreateCommand())
                        try
                        {
                            command.CommandText = "Use Master";
                            command.ExecuteNonQuery();
                            command.CommandText = "ALTER DATABASE " + _DBName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                            command.ExecuteNonQuery();
                            command.CommandText = "RESTORE DATABASE " + _DBName + " FROM DISK = '" + @_fileBackup + "' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10; ";
                            command.ExecuteNonQuery();
                            command.CommandText = "Use Master";
                            command.ExecuteNonQuery();
                            command.CommandText = "ALTER DATABASE " + _DBName + " SET MULTI_USER;";
                            command.ExecuteNonQuery();
                            command.CommandText = "Use " + _DBName;
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            _info = ex.Message;
                        }
                    CashierRegisterDAL.ConnectionDB.CloseConnect();
                }
                else _info = _fileBackup + " " + App.Current.FindResource("is_not_exists").ToString();
                //File.Delete(_path);
            }
            return _info;
        }
        private string _AutoBackupRestoreSqlite(BackupRestore _opts)
        {
            string _info = "Success";
            string _fileBackup = string.Format(@"{0}\{1}.db", _bkFolderNameSQLite, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName());
            if (_opts == BackupRestore.backup)
            {
                if (!Directory.Exists(_bkFolderNameSQLite))
                {
                    Directory.CreateDirectory(_bkFolderNameSQLite);
                }
                try
                {
                    CashierRegisterDAL.ConnectionDB.CloseConnect();
                    File.Copy(Directory.GetCurrentDirectory() + @"\Databases\CheckOut.db", _fileBackup, true);
                    CashierRegisterDAL.ConnectionDB.OpenConnect();
                }
                catch (Exception ex)
                {
                    _info = ex.Message;
                }
            }
            else if (_opts == BackupRestore.restore)
            {
                if (File.Exists(_fileBackup))
                {
                    try
                    {
                        CashierRegisterDAL.ConnectionDB.CloseConnect();
                        File.Copy(_fileBackup, Directory.GetCurrentDirectory() + @"\Databases\CheckOut.db", true);
                        CashierRegisterDAL.ConnectionDB.OpenConnect();
                    }
                    catch (Exception ex)
                    {
                        _info = ex.Message;
                    }
                }
                else _info = _fileBackup + " " + App.Current.FindResource("is_not_exists").ToString();
            }
            return _info;
        }
        private string getListColumnName(string _tblName_)
        {
            string _rs = string.Empty;
            if (string.IsNullOrEmpty(_tblName_)) return _rs;
            System.Data.DataTable _cols = Model.Setting.TableModel.getColumnsByTable(_tblName_);
            if (_cols.Rows.Count > 0)
            {
                foreach (System.Data.DataRow _dr in _cols.Rows)
                {
                    _rs += "[" + _dr["name"].ToString() + "], ";
                }
            }
            if (!string.IsNullOrEmpty(_rs))
                _rs = _rs.Remove(_rs.Length - 2, 2);
            return _rs;
        }
        private bool _stringInList(string _str, List<string> _lst)
        {
            bool rs = false;
            foreach (string _string in _lst)
            {
                string _newString = _string.Replace("[", "").Replace("]", "");
                if (String.Compare(_newString.Replace("\"", ""), _str, true) == 0)
                {
                    rs = true;
                    break;
                }
            }
            return rs;
        }
        private long getCatIdFromCatName(string _catName)   //if not exists then insert and return CatgoryID
        {
            int _rs = 0;
            if (!string.IsNullOrEmpty(_catName))
            {
                int _tmp = Model.Setting.TableModel.getCatIdByName(_catName);
                if (_tmp <= 0)
                {
                    EC_tb_Category ec_tb_category = new EC_tb_Category();
                    ec_tb_category.CategoryName = _catName;
                    if (bus_tb_category.InsertCategory(ec_tb_category, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {
                        _rs = Model.Setting.TableModel.getCatIdByName(_catName);
                    }
                }
                else _rs = _tmp;
            }
            return _rs;
        }
    }
}
