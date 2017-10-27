using CashierRegister.Helpers;
using CashierRegister.Model.Setting;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using Ionic.Zip;
using System.Data.Odbc;

namespace CashierRegister.ViewModel.Setting
{
    public class ImExportDataBase_VM : ModelBase
    {
        private bool _isLoaded = false;
        public RelayCommand formLoadCmd { get; private set; }
        public ObservableCollection<TableModel> Tables { get; set; }
        public RelayCommand exportCmd { get; private set; }
        private StaticClass.ExcelUtilityClass excel_utility = new StaticClass.ExcelUtilityClass();
        private DataTable datatable_export = new DataTable();
        private int num_export = 0;
        private string _pathExport = Directory.GetCurrentDirectory() + @"\images";
        private static string _SqliteBackupPath = Directory.GetCurrentDirectory() + @"\" + CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName() + ".db";
        private static string _SQLServerBackupPath = Directory.GetCurrentDirectory() + @"\" + CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName() + ".bak";
        public RelayCommand openFileCmd { get; private set; }
        public RelayCommand importCmd { get; private set; }
        private List<string> _oldTable = new List<string>();// store current tables
        private List<string> _newTable = new List<string>();// store new tables from file import
        private List<string> _bkTable = new List<string>();// store current tables after rename
        private static string _bk = "bk_";
        private System.Text.StringBuilder _strCopy = new System.Text.StringBuilder();
        private System.Text.StringBuilder _strDrop = new System.Text.StringBuilder();
        private System.Text.StringBuilder _strRename = new System.Text.StringBuilder();
        private System.Text.StringBuilder _bkAlter = new System.Text.StringBuilder();
        private System.Text.StringBuilder _strImport = new System.Text.StringBuilder();
        private List<string> _lstInsert = new List<string>();
        private List<string> _lstMove = new List<string>();
        private bool _isImportRuning = false;
        private Pages.Setting.Waiting _connecting;
        private System.Windows.Threading.DispatcherTimer _myTimer_ = null;
        private string _extFileName_ = string.Empty;
        private int _importStatus = 0;
        private System.Threading.Thread _threadConnect = null;
        private ImportFileType _opts = ImportFileType.none;
        private string _fileTypeOfFileImport = string.Empty;
        private string _nameOfFileImportWithoutExt = string.Empty;
        private bool _isExportRuning = false;
        private int _exportStatus = 0;
        private Microsoft.Win32.SaveFileDialog dlg = null;
        private const string _keyZip = "desktop-WV35G5S26AVK9";
        private static string _bkFolderNameSQL = string.Format(@"{0}\DBRESSer_Local\{1:M.dd.yyyy HH..mm..ss tt}", Directory.GetCurrentDirectory(), DateTime.Now);
        private static string _bkFolderNameSQLite = string.Format(@"{0}\DBRES_Local\{1:M.dd.yyyy HH..mm..ss tt}", Directory.GetCurrentDirectory(), DateTime.Now);
        public RelayCommand txtFileTextCmd { get; private set; }
        private static System.Collections.Hashtable _hashBackupSettings = null; 
        public ImExportDataBase_VM()
        {
            Tables = new ObservableCollection<TableModel>();
            formLoadCmd = new RelayCommand(onFormLoadExec);
            exportCmd = new RelayCommand(onExportExec);
            openFileCmd = new RelayCommand(onOpenFileExec);
            importCmd = new RelayCommand(onImportExec);
            txtFileTextCmd = new RelayCommand(onTxtFileTextExec);
        }
        private void onTxtFileTextExec(object _param)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "Text documents ZIP|*.zip|SQL|*.sql|SQLite|*.sqlite; *.db|CSV|*.csv";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                FileNameImport = dlg.FileName;
            }
        }
        private void onOpenFileExec(object _param)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "Text documents ZIP|*.zip|SQL|*.sql|SQLite|*.sqlite; *.db|CSV|*.csv";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                FileNameImport = dlg.FileName;
            }
        }
        private void _eventTimerImport(object sender, EventArgs e)
        {
            if (_isImportRuning)
            {
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                _isImportRuning = false;
                _importStatus = 0;
                _connecting.Close();
                _threadConnect.Abort();
            }
            else
            {
                if (_importStatus == 0)
                {
                    _importStatus = 1;
                    if (_opts == ImportFileType.csv)
                        _runImportFromCSV(this._nameOfFileImportWithoutExt, _extFileName_);
                    else if (_opts == ImportFileType.db || _opts == ImportFileType.sqlite)
                        _runImportFromSQLiteFile(_extFileName_);
                    else if (_opts == ImportFileType.sql)
                        _runImportFromSQLFile(_extFileName_);
                    else if (_opts == ImportFileType.zip)
                        _runImportFormZipFile(_extFileName_);
                }
            }
        }
        private async void _runImportFromSQLFile(string _pathFile)
        {
            var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._ImportFromSQLFile(_pathFile));
            await slowTask;
            _isImportRuning = true;
            if (slowTask.Result.ToString() == "Success")
            {
                if (StaticClass.GeneralClass.flag_database_type_general)
                    File.Delete(_SQLServerBackupPath);
                else File.Delete(_SqliteBackupPath);
                StaticClass.GeneralClass.app_settings["appIsRestart"] = "True";
                if (_hashBackupSettings != null)
                {
                    StaticClass.GeneralClass.app_settings["storeName"] = _hashBackupSettings["SHOP_NAME"].ToString();
                    StaticClass.GeneralClass.app_settings["storeAddress"] = _hashBackupSettings["SHOP_ADDRESS"].ToString();
                    StaticClass.GeneralClass.app_settings["storePhone"] = _hashBackupSettings["SHOP_PHONE"].ToString();
                    StaticClass.GeneralClass.app_settings["shopWebsite"] = _hashBackupSettings["SHOP_WEBSITE"].ToString();
                    StaticClass.GeneralClass.app_settings["receiptHeader"] = _hashBackupSettings["RECEIPT_HEADER"].ToString();
                    StaticClass.GeneralClass.app_settings["receiptFooter"] = _hashBackupSettings["RECEIPT_FOOTER"].ToString();
                }
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                string _strText = App.Current.FindResource("import_success").ToString().Replace("$$", App.Current.FindResource("cash_register").ToString());
                this._showNotify(_strText, "ok");
                System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                //Application.Restart();
                System.Windows.Application.Current.Shutdown();
            }
            else if (slowTask.Result.ToString() == "ErrorAutoBackup")
            {
                this._showNotify(App.Current.FindResource("error_before_import").ToString(), "close");
            }
            else
            {
                string rs = (StaticClass.GeneralClass.flag_database_type_general) ? _AutoBackupRestoreSqlServer(BackupRestore.restore, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName(), _SQLServerBackupPath) : _AutoBackupRestoreSqlite(BackupRestore.restore, _SqliteBackupPath);
                if (rs == "Success")
                {
                    if (StaticClass.GeneralClass.flag_database_type_general)
                        File.Delete(_SQLServerBackupPath);
                    else File.Delete(_SqliteBackupPath);
                    this._showNotify(App.Current.FindResource("import_err_restore_succ").ToString(), "close");
                }
                else this._showNotify(App.Current.FindResource("import_err_restore_err").ToString(), "close");
            }
        }
        private async void _runImportFromCSV(string _fName, string _pathFileImport)
        {
            var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._ImportFromCSV(_fName, _pathFileImport));
            await slowTask;
            _isImportRuning = true;
            if (slowTask.Result.ToString() == "Success")
            {
                if (StaticClass.GeneralClass.flag_database_type_general)
                    File.Delete(_SQLServerBackupPath);
                else File.Delete(_SqliteBackupPath);
                StaticClass.GeneralClass.app_settings["appIsRestart"] = "True";
                if (_hashBackupSettings != null)
                {
                    StaticClass.GeneralClass.app_settings["storeName"] = _hashBackupSettings["SHOP_NAME"].ToString();
                    StaticClass.GeneralClass.app_settings["storeAddress"] = _hashBackupSettings["SHOP_ADDRESS"].ToString();
                    StaticClass.GeneralClass.app_settings["storePhone"] = _hashBackupSettings["SHOP_PHONE"].ToString();
                    StaticClass.GeneralClass.app_settings["shopWebsite"] = _hashBackupSettings["SHOP_WEBSITE"].ToString();
                    StaticClass.GeneralClass.app_settings["receiptHeader"] = _hashBackupSettings["RECEIPT_HEADER"].ToString();
                    StaticClass.GeneralClass.app_settings["receiptFooter"] = _hashBackupSettings["RECEIPT_FOOTER"].ToString();
                }
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                this._showNotify("Import successfully !" + App.Current.FindResource("cash_register").ToString() + " will now restart.", "ok");
                System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                //Application.Restart();
                System.Windows.Application.Current.Shutdown();
            }
            else if (slowTask.Result.ToString() == "ErrorAutoBackup")
            {
                this._showNotify("Can not auto backup before import.", "close");
            }
            else
            {
                string _rs = _AutoBackupRestoreSqlServer(BackupRestore.restore, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName(), _SQLServerBackupPath);
                if (_rs == "Success")
                {
                    if (StaticClass.GeneralClass.flag_database_type_general)
                        File.Delete(_SQLServerBackupPath);
                    else File.Delete(_SqliteBackupPath);
                    this._showNotify("Can not import. And restored your database successfully.", "close");
                }
                else this._showNotify("Can not import as can not restore your database.", "close");
            }
        }
        private async void _runImportFromSQLiteFile(string _pathFileImport)
        {
            var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._ImportFromSQLiteFile(_pathFileImport));
            await slowTask;
            _isImportRuning = true;
            if (slowTask.Result.ToString() == "Success")
            {
                if (StaticClass.GeneralClass.flag_database_type_general)
                    File.Delete(_SQLServerBackupPath);
                else File.Delete(_SqliteBackupPath);
                StaticClass.GeneralClass.app_settings["appIsRestart"] = "True";
                if (_hashBackupSettings != null)
                {
                    StaticClass.GeneralClass.app_settings["storeName"] = _hashBackupSettings["SHOP_NAME"].ToString();
                    StaticClass.GeneralClass.app_settings["storeAddress"] = _hashBackupSettings["SHOP_ADDRESS"].ToString();
                    StaticClass.GeneralClass.app_settings["storePhone"] = _hashBackupSettings["SHOP_PHONE"].ToString();
                    StaticClass.GeneralClass.app_settings["shopWebsite"] = _hashBackupSettings["SHOP_WEBSITE"].ToString();
                    StaticClass.GeneralClass.app_settings["receiptHeader"] = _hashBackupSettings["RECEIPT_HEADER"].ToString();
                    StaticClass.GeneralClass.app_settings["receiptFooter"] = _hashBackupSettings["RECEIPT_FOOTER"].ToString();
                }
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                string _strText = App.Current.FindResource("import_success").ToString().Replace("$$", App.Current.FindResource("cash_register").ToString());
                this._showNotify(_strText, "ok");
                System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                //Application.Restart();
                System.Windows.Application.Current.Shutdown();
            }
            else if (slowTask.Result.ToString() == "ErrorAutoBackup")
            {
                this._showNotify(App.Current.FindResource("error_before_import").ToString(), "close");
            }
            else
            {
                string rs = (StaticClass.GeneralClass.flag_database_type_general) ? _AutoBackupRestoreSqlServer(BackupRestore.restore, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName(), _SQLServerBackupPath) : _AutoBackupRestoreSqlite(BackupRestore.restore, _SqliteBackupPath);
                if (rs == "Success")
                {
                    if (StaticClass.GeneralClass.flag_database_type_general)
                        File.Delete(_SQLServerBackupPath);
                    else File.Delete(_SqliteBackupPath);
                    this._showNotify(App.Current.FindResource("import_err_restore_succ").ToString(), "close");
                }
                else this._showNotify(App.Current.FindResource("import_err_restore_err").ToString(), "close");
            }
        }
        private async void _runImportFormZipFile(string _pathFileImport)
        {
            string _baseDirectory = Directory.GetCurrentDirectory() + @"\images\output";
            if (!Directory.Exists(_baseDirectory))
                Directory.CreateDirectory(_baseDirectory);
            string _status = string.Empty;
            using (ZipFile archive = new ZipFile(_pathFileImport))
            {
                if (DataFromIOS)
                {
                    archive.Password = "WV35G5S26AVK9";
                    archive.Encryption = EncryptionAlgorithm.PkzipWeak; // the default: you might need to select the proper value here
                }
                else
                {
                    archive.Password = _keyZip;
                    archive.Encryption = EncryptionAlgorithm.PkzipWeak;
                }
                try
                {
                    archive.ExtractAll(_baseDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
                catch (BadPasswordException zex)
                {
                    _status = zex.Message;
                    _isImportRuning = true;
                    this._showNotify(App.Current.FindResource("err_extract").ToString(), "close");
                }
            }
            if (string.IsNullOrEmpty(_status))
            {
                var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._ImportFormZipFile(_baseDirectory));
                await slowTask;
                _isImportRuning = true;
                if (slowTask.Result.ToString() == "Success")
                {
                    StaticClass.GeneralClass.app_settings["appIsRestart"] = "True";
                    if (_hashBackupSettings != null)
                    {
                        StaticClass.GeneralClass.app_settings["storeName"] = _hashBackupSettings["SHOP_NAME"].ToString();
                        StaticClass.GeneralClass.app_settings["storeAddress"] = _hashBackupSettings["SHOP_ADDRESS"].ToString();
                        StaticClass.GeneralClass.app_settings["storePhone"] = _hashBackupSettings["SHOP_PHONE"].ToString();
                        StaticClass.GeneralClass.app_settings["shopWebsite"] = _hashBackupSettings["SHOP_WEBSITE"].ToString();
                        StaticClass.GeneralClass.app_settings["receiptHeader"] = _hashBackupSettings["RECEIPT_HEADER"].ToString();
                        StaticClass.GeneralClass.app_settings["receiptFooter"] = _hashBackupSettings["RECEIPT_FOOTER"].ToString();
                    }
                    Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                    string _strText = App.Current.FindResource("import_success").ToString().Replace("$$", App.Current.FindResource("cash_register").ToString());
                    this._showNotify(_strText, "ok");
                    this._ImportImageToApp(_baseDirectory, Directory.GetCurrentDirectory() + @"\images");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Directory.Delete(_baseDirectory, true);
                    if (StaticClass.GeneralClass.flag_database_type_general)
                        File.Delete(_SQLServerBackupPath);
                    else File.Delete(_SqliteBackupPath);
                    System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                    //Application.Restart();
                    System.Windows.Application.Current.Shutdown();
                }
                else if (slowTask.Result.ToString() == "ErrorAutoBackup")
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Directory.Delete(_baseDirectory, true);
                    this._showNotify(App.Current.FindResource("error_before_import").ToString(), "close");
                }
                else
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Directory.Delete(_baseDirectory, true);
                    string rs = (StaticClass.GeneralClass.flag_database_type_general) ? _AutoBackupRestoreSqlServer(BackupRestore.restore, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName(), _SQLServerBackupPath) : _AutoBackupRestoreSqlite(BackupRestore.restore, _SqliteBackupPath);
                    if (rs == "Success")
                    {
                        if (StaticClass.GeneralClass.flag_database_type_general)
                            File.Delete(_SQLServerBackupPath);
                        else File.Delete(_SqliteBackupPath);
                        this._showNotify(App.Current.FindResource("import_err_restore_succ").ToString(), "close");
                    }
                    else this._showNotify(App.Current.FindResource("import_err_restore_err").ToString(), "close");
                }
            }
        }
        private void onImportExec(object _param)
        {
            string _fileImport = FileNameImport;
            _extFileName_ = FileNameImport;
            if (string.IsNullOrEmpty(_fileImport))
            {
                string _message = App.Current.FindResource("please_select_file").ToString();
                this._showNotify(_message, "ok");
            }
            else
            {
                this._fileTypeOfFileImport = Path.GetExtension(_fileImport).Remove(0, 1);
                this._nameOfFileImportWithoutExt = Path.GetFileNameWithoutExtension(_fileImport);
                string _ext = this._fileTypeOfFileImport;
                if (_ext.ToLower() == ImportFileType.csv.ToString())
                {
                    _opts = ImportFileType.csv;
                }
                else if (_ext.ToLower() == ImportFileType.sqlite.ToString() || _ext.ToLower() == ImportFileType.db.ToString())
                {
                    _opts = ImportFileType.sqlite;
                }
                else if (_ext.ToLower() == ImportFileType.sql.ToString())
                {
                    _opts = ImportFileType.sql;
                }
                else if (_ext.ToLower() == ImportFileType.zip.ToString())
                {
                    _opts = ImportFileType.zip;
                }
                if (_opts == ImportFileType.none)
                {
                    this._showNotify(App.Current.FindResource("unknown_file").ToString(), "close");
                }
                else
                {
                    ModernDialog mdd = new ModernDialog();
                    mdd.Buttons = new System.Windows.Controls.Button[] { mdd.OkButton, mdd.CancelButton };
                    mdd.OkButton.TabIndex = 0;
                    mdd.OkButton.Content = App.Current.FindResource("ok").ToString();
                    mdd.CancelButton.TabIndex = 1;
                    mdd.CancelButton.Content = App.Current.FindResource("cancel").ToString();
                    mdd.TabIndex = -1;
                    mdd.Height = 200;
                    mdd.Title = App.Current.FindResource("notification").ToString();
                    mdd.Content = App.Current.FindResource("replace_cur_data").ToString();
                    mdd.OkButton.Focus();
                    mdd.ShowDialog();
                    if (mdd.MessageBoxResult == System.Windows.MessageBoxResult.OK)
                    {
                        _myTimer_ = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
                        _connecting = new Pages.Setting.Waiting();
                        _connecting.StartPosition = FormStartPosition.CenterScreen;
                        _connecting.ShowInTaskbar = false;
                        _threadConnect = new System.Threading.Thread(() =>
                        {
                            _myTimer_.Tick += _eventTimerImport;
                            _myTimer_.Start();
                        });
                        _threadConnect.SetApartmentState(System.Threading.ApartmentState.STA);
                        _threadConnect.Start();
                        _threadConnect.Join();
                        _connecting.ShowDialog();
                    }
                }
            }
        }
        private void _ImportImageToApp(string _fromFolder, string _toFolder)
        {
            string[] _fileDel = Directory.GetFiles(_toFolder);
            if (IsDeleteImage)
            {
                foreach (string _filename in _fileDel)
                {
                    File.Delete(_filename);
                }
            }
            string[] _fileIns = Directory.GetFiles(_fromFolder);
            foreach(string _filename in _fileIns)
            {
                string _ext = Path.GetExtension(_filename).Remove(0, 1).ToLower();
                if (_ext != "sql" && _ext != "sqlite" && _ext != "db" && _ext != "csv" && _ext != "ini")
                {
                    string _file = Path.GetFileName(_filename);
                    File.Copy(_filename, _toFolder + @"\" + _file, true);
                }
            }
        }
        private string _ImportFormZipFile(string _folderExtract)
        {
            string _info = "Success";
            string[] _fileSQL = Directory.GetFiles(_folderExtract, "*.sql");
            string[] _fileDB = Directory.GetFiles(_folderExtract, "*.db");
            string[] _fileCSV = Directory.GetFiles(_folderExtract, "*.csv");
            if (_fileSQL.Length > 0)
            {
                string _ext = Path.GetExtension(_fileSQL[0]).Remove(0, 1);
                if(_ext.ToLower()=="sql")
                    _info = _ImportFromSQLFile(_fileSQL[0]);
                else if (_ext.ToLower() == "sqlite")
                {
                    _info = _ImportFromSQLiteFile(_fileSQL[0]);
                }
                    
            }
            else if(_fileDB.Length > 0)
            {
                _info = _ImportFromSQLiteFile(_fileDB[0]);
            }
            else if(_fileCSV.Length > 0)
            {
                string _rs = (StaticClass.GeneralClass.flag_database_type_general) ? _AutoBackupRestoreSqlServer(BackupRestore.backup, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName(), _SQLServerBackupPath) : _AutoBackupRestoreSqlite(BackupRestore.backup, _SqliteBackupPath);
                if (_rs == "Success")
                {
                    foreach (string fileName in _fileCSV)
                    {
                        string _file = Path.GetFileNameWithoutExtension(fileName);
                        _info = _ImportFromCSV(_file, fileName);
                        if (_info != "Success")
                            break;
                    }
                }
                else _info = _rs;
            }
            return _info;
        }
        private void _BuildQueryFromSQLiteFileToInsertIntoSqlite(string _fileImport)
        {
            foreach (TableModel _tbl in Tables)
            {
                this._oldTable.Add(_tbl.TableName);
            }
            try
            {
                string _strCnn = (DataFromIOS) ? @"Data Source=" + _fileImport + @"; Version=3;" : @"Data Source=" + _fileImport + @"; Version=3; Password='" + CashierRegisterDAL.ConnectionDB.getKeyDecrypt() + "'" ;
                using (var destination = new SQLiteConnection(_strCnn))
                {
                    destination.Open();
                    string sql = "SELECT tbl_name as TblName, sql as StrCreated FROM sqlite_master where type='table' and name like 'tb_%'";
                    SQLiteCommand sqlite_cm = new SQLiteCommand(sql, destination);
                    SQLiteDataReader sqlite_dr = sqlite_cm.ExecuteReader();
                    if (DataFromIOS)
                    {
                        while (sqlite_dr.Read())
                        {
                            if (!this._stringInList(sqlite_dr[0].ToString(), this._oldTable))
                            {
                                this._strCopy.AppendLine(sqlite_dr[1].ToString() + "; ");
                                string sqlCmd = "select * from " + sqlite_dr[0].ToString();
                                SQLiteCommand cmd = new SQLiteCommand(sqlCmd, destination);
                                SQLiteDataReader rs = cmd.ExecuteReader();
                                string valor = "";
                                string Valores = "";
                                string Campos = "";
                                string Campo = "";
                                while (rs.Read())
                                {
                                    string _SqlInsert = string.Empty;
                                    _SqlInsert = "INSERT INTO " + sqlite_dr[0].ToString();
                                    Campos = "";
                                    Valores = "";
                                    for (int i = 0; i < rs.FieldCount; i++)
                                    {
                                        Campo = "" + rs.GetName(i);
                                        valor = "" + rs.GetValue(i);
                                        if (Valores != "")
                                        {
                                            Valores = Valores + ',';
                                            Campos = Campos + ',';
                                        }
                                        if (string.IsNullOrEmpty(valor))
                                            Valores = Valores + @"null";
                                        else if (IsNumeric(valor))
                                            Valores = Valores + "'" + valor + "'";
                                        else Valores = Valores + "'" + valor.Replace("'", "''") + "'";
                                        Campos = Campos + "[" + Campo + "]";
                                    }
                                    _SqlInsert = _SqlInsert + "(" + Campos + ") Values (" + Valores + "); ";
                                    this._strImport.AppendLine(_SqlInsert);
                                }
                            }
                            else
                            {
                                if(sqlite_dr[0].ToString() != "tb_Setting")
                                    this._strDrop.AppendLine("delete from " + sqlite_dr[0].ToString() + "; UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = '" + sqlite_dr[0].ToString() + "'; ");
                                string sqlCmd = string.Empty;
                                switch (sqlite_dr[0].ToString())
                                {
                                    case "tb_Category":
                                        sqlCmd = "select CategoryID, CategoryName from tb_Category";
                                        break;
                                    case "tb_Customer":
                                        sqlCmd = "select CustomerID, FirstName, LastName, Address1, Address2, City, State, Zipcode, PhoneNumber, Email from tb_Customer";
                                        break;
                                    case "tb_GiftCard":
                                        sqlCmd = "select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, 0 from tb_GiftCard";
                                        break;
                                    case "tb_Order":
                                        sqlCmd = "select OrderID, o.CustomerID, FirstName || ' ' || LastName, Quantity, CAST(datetime(DateT, 'unixepoch', 'localtime') as nvarchar), o.SalesPersonID, u.Name, o.PaymentID, pm.Name, IsRateDiscountOrder, Discount, 0, Tax, Total from tb_Order as o left join tb_Customer as c on c.CustomerID = o.CustomerID left join tb_User as u on u.UserID = o.SalesPersonID left join tb_Payment as pm on pm.PaymentID = o.PaymentID";
                                        break;
                                    case "tb_OrderDetail":
                                        sqlCmd = "select ID, 0, NULL, od.ProductID, ShortName || ' ' || LongName, od.Cost, od.Price, Quantity, od.Tax, -1, Discount, DiscountMain, (od.Price-Discount), od.OrderID from tb_OrderDetail as od left join tb_Product as p on p.ProductID = od.ProductID";
                                        break;
                                    case "tb_Payment":
                                        sqlCmd = "select PaymentID, Name from tb_Payment";
                                        break;
                                    case "tb_Product":
                                        sqlCmd = "select ProductID, Barcode, ShortName, LongName, Cost, Price, Qty, CategoryID, Taxable, " + @"'\images\product_' || ProductID || '.png', 0, Active, NULL, NULL from tb_Product";
                                        break;
                                    case "tb_Salesperson":
                                        sqlCmd = "select UserID, Name, NULL, NULL, NULL, Password, IsActive, 0 from tb_User where Type = 'Salesperson'";
                                        break;
                                    case "tb_Currency":
                                        sqlCmd = "select PaymentID, Symbol, CASE WHEN PaymentID = 33 THEN 4 WHEN PaymentID = 30 THEN 6 WHEN PaymentID in (4,8,10,12,14,18,20,22,24,26,31,35,37,39,41,43,45,47,49,51,53,55,57,59) THEN 5 ELSE 10 END, CASE WHEN Symbol = '$' THEN 1 ELSE 0 END, 1 from tb_Currency";
                                        break;
                                    case "tb_Setting":
                                        _hashBackupSettings = new System.Collections.Hashtable();
                                        SQLiteCommand _cmd = new SQLiteCommand("select SettingKey, SettingValue from tb_Setting where SettingKey in('SHOP_NAME','SHOP_ADDRESS','SHOP_PHONE','SHOP_WEBSITE','RECEIPT_HEADER','RECEIPT_FOOTER')", destination);
                                        SQLiteDataReader _rs = _cmd.ExecuteReader();
                                        while (_rs.Read())
                                        {
                                            _hashBackupSettings.Add(_rs["SettingKey"].ToString(), _rs["SettingValue"].ToString());
                                        }
                                        break;
                                    case "tb_User":
                                        sqlCmd = "select UserID, Name, NULL, NULL, Password, NULL, NULL from tb_User where Type <> 'Salesperson'";
                                        break;
                                    default:
                                        sqlCmd = "select * from " + sqlite_dr[0].ToString();
                                        break;
                                }
                                if (!string.IsNullOrEmpty(sqlCmd))
                                {
                                    SQLiteCommand cmd = new SQLiteCommand(sqlCmd, destination);
                                    SQLiteDataReader rs = cmd.ExecuteReader();
                                    string valor = "";
                                    string Valores = "";
                                    while (rs.Read())
                                    {
                                        string _SqlInsert = string.Empty;
                                        _SqlInsert = "INSERT INTO " + sqlite_dr[0].ToString();
                                        Valores = "";
                                        for (int i = 0; i < rs.FieldCount; i++)
                                        {
                                            valor = "" + rs.GetValue(i);
                                            if (Valores != "")
                                            {
                                                Valores = Valores + ',';
                                            }
                                            if (string.IsNullOrEmpty(valor))
                                                Valores = Valores + @"null";
                                            else if (IsNumeric(valor))
                                                Valores = Valores + "'" + valor + "'";
                                            else Valores = Valores + "'" + valor.Replace("'", "''") + "'";
                                        }
                                        _SqlInsert = _SqlInsert + "(" + this.getListColumnName(sqlite_dr[0].ToString()) + ") Values (" + Valores + "); ";
                                        this._strImport.AppendLine(_SqlInsert);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        while (sqlite_dr.Read())
                        {
                            this._strDrop.AppendLine("delete from " + sqlite_dr[0].ToString() + "; UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = '" + sqlite_dr[0].ToString() + "'; ");
                            string sqlCmd = "select *  from " + sqlite_dr[0].ToString();
                            SQLiteCommand cmd = new SQLiteCommand(sqlCmd, destination);
                            SQLiteDataReader rs = cmd.ExecuteReader();
                            string valor = "";
                            string Valores = "";
                            string Campos = "";
                            string Campo = "";
                            while (rs.Read())
                            {
                                string _SqlInsert = string.Empty;
                                _SqlInsert = "INSERT INTO " + sqlite_dr[0].ToString();
                                Campos = "";
                                Valores = "";
                                for (int i = 0; i < rs.FieldCount; i++)
                                {
                                    Campo = "" + rs.GetName(i);
                                    valor = "" + rs.GetValue(i);

                                    if (Valores != "")
                                    {
                                        Valores = Valores + ',';
                                        Campos = Campos + ',';
                                    }
                                    if (string.IsNullOrEmpty(valor))
                                        Valores = Valores + @"null";
                                    else if (IsNumeric(valor))
                                        Valores = Valores + "'" + valor + "'";
                                    else Valores = Valores + "'" + valor.Replace("'", "''") + "'";
                                    Campos = Campos + "[" + Campo + "]";
                                }
                                _SqlInsert = _SqlInsert + "(" + Campos + ") Values (" + Valores + "); ";
                                this._strImport.AppendLine(_SqlInsert);
                            }
                            if (!this._stringInList(sqlite_dr[0].ToString(), this._oldTable))
                            {
                                this._strCopy.AppendLine(sqlite_dr[1].ToString()+"; ");
                            }
                        }
                    }
                    destination.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        private void _BuildQueryFromSQLiteFileToInsertIntoSQL(string _fileImport)
        {
            foreach (TableModel _tbl in Tables)
            {
                this._oldTable.Add(_tbl.TableName);
            }
            try
            {
                string _strCnn = (DataFromIOS) ? @"Data Source=" + _fileImport + @"; Version=3;" : @"Data Source=" + _fileImport + @"; Version=3; Password='" + CashierRegisterDAL.ConnectionDB.getKeyDecrypt() + "'";
                //SQLiteConnection destination = new SQLiteConnection(_strCnn);
                using (var destination = new SQLiteConnection(_strCnn))
                {
                    destination.Open();
                    string sql = "SELECT tbl_name as TblName, sql as StrCreated FROM sqlite_master where type='table' and name like 'tb_%'";
                    SQLiteCommand sqlite_cm = new SQLiteCommand(sql, destination);
                    SQLiteDataReader sqlite_dr = sqlite_cm.ExecuteReader();
                    if (DataFromIOS)
                    {
                        while (sqlite_dr.Read())
                        {
                            if (!this._stringInList(sqlite_dr[0].ToString(), this._oldTable))
                            {
                                string _sql_ = sqlite_dr[1].ToString().Replace("COLLATE NOCASE", "");
                                _sql_ = _sql_.Replace("AUTOINCREMENT", "IDENTITY(1,1)");
                                _sql_ = _sql_.Replace("DOUBLE", "decimal");
                                _sql_ = _sql_.Replace("`", "\"");
                                _sql_ = _sql_.Replace("BOOL", "BIT");
                                _sql_ = _sql_.Replace("TEXT UNIQUE", "nvarchar(255) UNIQUE");
                                _sql_ = _sql_.Replace("'", "\"");
                                if (sqlite_dr[0].ToString() == "tb_Currency")
                                {
                                    _sql_ = _sql_.Replace("VARCHAR NOT NULL", "VARCHAR(10) NOT NULL");
                                }
                                if (sqlite_dr[0].ToString() == "tb_GiftCard")
                                {
                                    _sql_ = _sql_.Replace("\"Barcode\" TEXT", "\"Barcode\" nvarchar(255) UNIQUE").Replace("UNIQUE (\"Barcode\" ASC)", "");
                                }
                                this._strCopy.AppendLine(_sql_ + "; ");

                                string sqlCmd = "select * from " + sqlite_dr[0].ToString();
                                SQLiteCommand cmd = new SQLiteCommand(sqlCmd, destination);
                                SQLiteDataReader rs = cmd.ExecuteReader();
                                string valor = "";
                                string Valores = "";
                                string Campos = "";
                                string Campo = "";
                                while (rs.Read())
                                {
                                    string _SqlInsert = string.Empty;
                                    _SqlInsert = "INSERT INTO " + sqlite_dr[0].ToString();
                                    Campos = "";
                                    Valores = "";
                                    for (int i = 0; i < rs.FieldCount; i++)
                                    {
                                        Campo = "" + rs.GetName(i);
                                        valor = "" + rs.GetValue(i);
                                        if (Valores != "")
                                        {
                                            Valores = Valores + ',';
                                            Campos = Campos + ',';
                                        }
                                        if(string.IsNullOrEmpty(valor))
                                            Valores = Valores + @"null";
                                        else if(IsNumeric(valor))
                                            Valores = Valores + "'" + valor + "'";
                                        else Valores = Valores + "N'" + valor.Replace("'", "''") + "'";
                                        Campos = Campos + "[" + Campo + "]";
                                    }
                                    _SqlInsert = _SqlInsert + "(" + Campos + ") Values (" + Valores + "); ";
                                    _SqlInsert = "if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + sqlite_dr[0].ToString() + "', 'U') and is_identity = 1) begin SET IDENTITY_INSERT " + sqlite_dr[0].ToString() + " ON; " + _SqlInsert + " SET IDENTITY_INSERT " + sqlite_dr[0].ToString() + " OFF; end else " + _SqlInsert;
                                    this._strImport.AppendLine(_SqlInsert);
                                }
                            }
                            else
                            {
                                if(sqlite_dr[0].ToString()!="tb_Setting")
                                    this._strDrop.AppendLine("TRUNCATE TABLE  " + sqlite_dr[0].ToString() + "; ");
                                string sqlCmd = string.Empty;
                                switch (sqlite_dr[0].ToString())
                                {
                                    case "tb_Category":
                                        sqlCmd = "select CategoryID, CategoryName from tb_Category";
                                        break;
                                    case "tb_Customer":
                                        sqlCmd = "select CustomerID, FirstName, LastName, Address1, Address2, City, State, Zipcode, PhoneNumber, Email from tb_Customer";
                                        break;
                                    case "tb_GiftCard":
                                        sqlCmd = "select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, 0 from tb_GiftCard";
                                        break;
                                    case "tb_Order":
                                        sqlCmd = "select OrderID, o.CustomerID, FirstName || ' ' || LastName, Quantity, CAST(datetime(DateT, 'unixepoch', 'localtime') as nvarchar), o.SalesPersonID, u.Name, o.PaymentID, pm.Name, IsRateDiscountOrder, Discount, 0, Tax, Total from tb_Order as o left join tb_Customer as c on c.CustomerID = o.CustomerID left join tb_User as u on u.UserID = o.SalesPersonID left join tb_Payment as pm on pm.PaymentID = o.PaymentID";
                                        break;
                                    case "tb_OrderDetail":
                                        sqlCmd = "select ID, 0, NULL, od.ProductID, ShortName || ' ' || LongName, od.Cost, od.Price, Quantity, od.Tax, -1, Discount, DiscountMain, (od.Price-Discount), od.OrderID from tb_OrderDetail as od left join tb_Product as p on p.ProductID = od.ProductID";
                                        break;
                                    case "tb_Payment":
                                        sqlCmd = "select PaymentID, Name from tb_Payment";
                                        break;
                                    case "tb_Product":
                                        sqlCmd = "select ProductID, Barcode, ShortName, LongName, Cost, Price, Qty, CategoryID, Taxable, " + @"'\images\product_' || ProductID || '.png', 0, Active, NULL, NULL from tb_Product";
                                        break;
                                    case "tb_Salesperson":
                                        sqlCmd = "select UserID, Name, NULL, NULL, NULL, Password, IsActive, 0 from tb_User where Type = 'Salesperson'";
                                        break;
                                    case "tb_Currency":
                                        sqlCmd = "select PaymentID, Symbol, CASE WHEN PaymentID = 33 THEN 4 WHEN PaymentID = 30 THEN 6 WHEN PaymentID in (4,8,10,12,14,18,20,22,24,26,31,35,37,39,41,43,45,47,49,51,53,55,57,59) THEN 5 ELSE 10 END, CASE WHEN Symbol = '$' THEN 1 ELSE 0 END, 1 from tb_Currency";
                                        break;
                                    case "tb_Setting":
                                        _hashBackupSettings = new System.Collections.Hashtable();
                                        SQLiteCommand _cmd = new SQLiteCommand("select SettingKey, SettingValue from tb_Setting where SettingKey in('SHOP_NAME','SHOP_ADDRESS','SHOP_PHONE','SHOP_WEBSITE','RECEIPT_HEADER','RECEIPT_FOOTER')", destination);
                                        SQLiteDataReader _rs = _cmd.ExecuteReader();
                                        while (_rs.Read())
                                        {
                                            _hashBackupSettings.Add(_rs["SettingKey"].ToString(), _rs["SettingValue"].ToString());
                                        }
                                        break;
                                    case "tb_User":
                                        sqlCmd = "select UserID, Name, NULL, NULL, Password, NULL, NULL from tb_User where Type <> 'Salesperson'";
                                        break;
                                    default:
                                        sqlCmd = "select * from " + sqlite_dr[0].ToString();
                                        break;
                                }
                                if (!string.IsNullOrEmpty(sqlCmd))
                                {
                                    SQLiteCommand cmd = new SQLiteCommand(sqlCmd, destination);
                                    SQLiteDataReader rs = cmd.ExecuteReader();
                                    string valor = "";
                                    string Valores = "";
                                    while (rs.Read())
                                    {
                                        string _SqlInsert = string.Empty;
                                        _SqlInsert = "INSERT INTO " + sqlite_dr[0].ToString();
                                        Valores = "";
                                        for (int i = 0; i < rs.FieldCount; i++)
                                        {
                                            valor = "" + rs.GetValue(i);
                                            if (Valores != "")
                                            {
                                                Valores = Valores + ',';
                                            }
                                            if (string.IsNullOrEmpty(valor))
                                                Valores = Valores + @"null";
                                            else if (IsNumeric(valor))
                                                Valores = Valores + "'" + valor + "'";
                                            else Valores = Valores + "N'" + valor.Replace("'", "''") + "'";
                                        }
                                        _SqlInsert = _SqlInsert + "(" + this.getListColumnName(sqlite_dr[0].ToString()) + ") Values (" + Valores + "); ";
                                        _SqlInsert = "if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + sqlite_dr[0].ToString() + "', 'U') and is_identity = 1) begin SET IDENTITY_INSERT " + sqlite_dr[0].ToString() + " ON; " + _SqlInsert + " SET IDENTITY_INSERT " + sqlite_dr[0].ToString() + " OFF; end else " + _SqlInsert;
                                        this._strImport.AppendLine(_SqlInsert);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        while (sqlite_dr.Read())
                        {
                            this._strDrop.AppendLine("TRUNCATE TABLE " + sqlite_dr[0].ToString() + "; ");
                            string sqlCmd = "select * from " + sqlite_dr[0].ToString();
                            SQLiteCommand cmd = new SQLiteCommand(sqlCmd, destination);
                            SQLiteDataReader rs = cmd.ExecuteReader();
                            string valor = "";
                            string Valores = "";
                            while (rs.Read())
                            {
                                string _SqlInsert = string.Empty;
                                _SqlInsert = "INSERT INTO " + sqlite_dr[0].ToString();
                                Valores = "";
                                for (int i = 0; i < rs.FieldCount; i++)
                                {
                                    valor = "" + rs.GetValue(i);
                                    if (Valores != "")
                                    {
                                        Valores = Valores + ',';
                                    }
                                    if (string.IsNullOrEmpty(valor))
                                        Valores = Valores + @"null";
                                    else if (IsNumeric(valor))
                                        Valores = Valores + "'" + valor + "'";
                                    else Valores = Valores + "N'" + valor.Replace("'", "''") + "'";
                                }
                                _SqlInsert = _SqlInsert + "(" + this.getListColumnName(sqlite_dr[0].ToString()) + ") Values (" + Valores + "); ";
                                _SqlInsert = "if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + sqlite_dr[0].ToString() + "', 'U') and is_identity = 1) begin SET IDENTITY_INSERT " + sqlite_dr[0].ToString() + " ON; " + _SqlInsert + " SET IDENTITY_INSERT " + sqlite_dr[0].ToString() + " OFF; end else " + _SqlInsert;
                                this._strImport.AppendLine(_SqlInsert);
                            }
                            if (!this._stringInList(sqlite_dr[0].ToString(), this._oldTable))
                            {
                                string _sql_ = sqlite_dr[1].ToString().Replace("COLLATE NOCASE", "");
                                _sql_ = _sql_.Replace("AUTOINCREMENT", "IDENTITY(1,1)");
                                _sql_ = _sql_.Replace("DOUBLE", "decimal");
                                _sql_ = _sql_.Replace("`", "\"").Replace("'", "\"");
                                _sql_ = _sql_.Replace("BOOL", "BIT");
                                _sql_ = _sql_.Replace("TEXT UNIQUE", "nvarchar(255) UNIQUE");
                                if(sqlite_dr[0].ToString()== "tb_Currency")
                                {
                                    _sql_ = _sql_.Replace("VARCHAR NOT NULL", "VARCHAR(10) NOT NULL");
                                }
                                if (sqlite_dr[0].ToString() == "tb_GiftCard")
                                {
                                    _sql_ = _sql_.Replace("\"Barcode\" TEXT", "\"Barcode\" nvarchar(255) UNIQUE").Replace("UNIQUE (\"Barcode\" ASC)", "");
                                }
                                this._strCopy.AppendLine(_sql_ + "; ");
                            }
                        }
                    }
                    destination.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        private string _ImportFromSQLiteFile(string _fileImport)
        {
            string _info = "Success";
            if (StaticClass.GeneralClass.flag_database_type_general) //SQL Server
            {
                _BuildQueryFromSQLiteFileToInsertIntoSQL(_fileImport);
                SqlConnection sqlConnect = CashierRegisterDAL.ConnectionDB.getSQLConnection();
                SqlTransaction tr = null;
                SqlCommand cmd = null;
                try
                {
                    string _resultBK = _AutoBackupRestoreSqlServer(BackupRestore.backup, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName(), _SQLServerBackupPath);
                    if (_resultBK == "Success")
                    {
                        using (tr = sqlConnect.BeginTransaction())
                        {
                            using (cmd = sqlConnect.CreateCommand())
                            {
                                cmd.Transaction = tr;
                                if (!string.IsNullOrEmpty(this._strCopy.ToString()))
                                {
                                    //System.Diagnostics.Debug.WriteLine(this._strCopy.ToString());
                                    cmd.CommandText = this._strCopy.ToString();
                                    cmd.ExecuteNonQuery();
                                }
                                if (!string.IsNullOrEmpty(this._strDrop.ToString()))
                                {
                                    cmd.CommandText = this._strDrop.ToString();
                                    cmd.ExecuteNonQuery();
                                }
                                //System.Diagnostics.Debug.WriteLine(this._strImport.ToString());
                                cmd.CommandText = this._strImport.ToString();
                                cmd.ExecuteNonQuery();
                            }
                            tr.Commit();
                        }
                        tr.Dispose();
                        cmd.Dispose();
                    }
                    else _info = "ErrorAutoBackup";
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
                            _info = ex2.Message;
                        }
                        finally
                        {
                            tr.Dispose();
                        }
                    }
                    _info = ex.Message;
                }
            }
            else
            {
                _BuildQueryFromSQLiteFileToInsertIntoSqlite(_fileImport);
                string _resultBK = _AutoBackupRestoreSqlite(BackupRestore.backup, _SqliteBackupPath);
                if (_resultBK == "Success")
                {
                    SQLiteConnection sqliteConn = CashierRegisterDAL.ConnectionDB.getSQLiteConnection();
                    SQLiteTransaction tr = null;
                    SQLiteCommand cmd = null;
                    try
                    {
                        using (tr = sqliteConn.BeginTransaction())
                        {
                            using (cmd = sqliteConn.CreateCommand())
                            {
                                cmd.Transaction = tr;

                                if (!string.IsNullOrEmpty(this._strCopy.ToString()))
                                {
                                    cmd.CommandText = this._strCopy.ToString();
                                    cmd.ExecuteNonQuery();
                                }
                                if (!string.IsNullOrEmpty(this._strDrop.ToString()))
                                {
                                    cmd.CommandText = this._strDrop.ToString();
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.CommandText = this._strImport.ToString();
                                cmd.ExecuteNonQuery();
                            }
                            tr.Commit();
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        _info = ex.Message;
                        if (tr != null)
                        {
                            try
                            {
                                tr.Rollback();
                            }
                            catch (ObjectDisposedException ex2)
                            {
                                _info = ex2.Message;
                            }
                            finally
                            {
                                tr.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        tr.Dispose();
                        cmd.Dispose();
                    }
                }
                else _info = "ErrorAutoBackup";
            }
            return _info;
        }
        private string _ImportFromSQLFile(string _fileImport)
        {
            if (StaticClass.GeneralClass.flag_database_type_general) //SQL Server
            {
                SqlConnection sqlConnect = CashierRegisterDAL.ConnectionDB.getSQLConnection();
                SqlTransaction tr = null;
                SqlCommand cmd = null;
                string _info = "Success";
                try
                {
                    string _resultBK = _AutoBackupRestoreSqlServer(BackupRestore.backup, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName(), _SQLServerBackupPath);
                    if (_resultBK == "Success")
                    {
                        using (tr = sqlConnect.BeginTransaction())
                        {
                            using (cmd = sqlConnect.CreateCommand())
                            {
                                cmd.Transaction = tr;
                                this._BuildStringForCopyFromSQL2SqlServer(_fileImport);
                                cmd.CommandText = _bkAlter.ToString() + _strImport.ToString() + _strCopy.ToString();
                                cmd.ExecuteNonQuery();
                                if (_lstInsert.Count > 0)
                                {
                                    foreach(string _val in _lstInsert)
                                    {
                                        cmd.CommandText = _val;
                                        try
                                        {
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch(SqlException SqlEx)
                                        {
                                            _info = SqlEx.Message;
                                            break;
                                        }
                                    }
                                }
                            }
                            tr.Commit();
                        }
                        tr.Dispose();
                        cmd.Dispose();
                        if (_info == "Success" && this._lstMove.Count > 0)
                        {
                            CashierRegisterDAL.ConnectionDB.CloseConnect();
                            foreach (string _str in this._lstMove)
                            {
                                CashierRegisterDAL.ConnectionDB.OpenConnect();
                                SqlConnection tryConnect = CashierRegisterDAL.ConnectionDB.getSQLConnection();
                                string _tmpName = _str.Substring(0, _str.IndexOf("$")); ;
                                string _temp = _str.Substring(_tmpName.Length+1);
                                using (cmd = tryConnect.CreateCommand())
                                {
                                    cmd.CommandText = _temp.Replace("(<listcolumn>)", "(" + this.getListColumnName(_tmpName) + ")");
                                    try
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (SqlException ex)
                                    {
                                        _info = ex.Message;
                                    }
                                }
                                CashierRegisterDAL.ConnectionDB.CloseConnect();
                                if (_info != "Success") break;
                            }
                            if (_info == "Success")
                            {
                                try
                                {
                                    CashierRegisterDAL.ConnectionDB.OpenConnect();
                                    SqlConnection tryConnect = CashierRegisterDAL.ConnectionDB.getSQLConnection();
                                    using (tr = tryConnect.BeginTransaction())
                                    {
                                        using (cmd = tryConnect.CreateCommand())
                                        {
                                            cmd.Transaction = tr;
                                            cmd.CommandText = this._strDrop.ToString();
                                            cmd.ExecuteNonQuery();
                                            cmd.CommandText = this._strRename.ToString();
                                            cmd.ExecuteNonQuery();
                                        }
                                        tr.Commit();
                                    }
                                    tr.Dispose();
                                    cmd.Dispose();
                                    CashierRegisterDAL.ConnectionDB.CloseConnect();
                                }
                                catch (SqlException ex)
                                {
                                    _info = ex.Message;
                                }
                            }
                            CashierRegisterDAL.ConnectionDB.OpenConnect();
                        }
                        return _info;
                    }
                    else return "ErrorAutoBackup";
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
            else //sqlite
            {
                string _info = "Success";
                string _resultBK = _AutoBackupRestoreSqlite(BackupRestore.backup, _SqliteBackupPath);
                if (_resultBK == "Success")
                {
                    SQLiteConnection sqliteConn = CashierRegisterDAL.ConnectionDB.getSQLiteConnection();
                    SQLiteTransaction tr = null;
                    SQLiteCommand cmd = null;
                    CashierRegisterDAL.ConnectionDB.SetLimit(CashierRegisterDAL.ConnectionDB.getSQLiteConnection(), 1000000);
                    try
                    {
                        using (tr = sqliteConn.BeginTransaction())
                        {
                            using (cmd = sqliteConn.CreateCommand())
                            {
                                cmd.Transaction = tr;
                                this._BuildStringForCopyFromSQL2Sqlite(_fileImport);
                                cmd.CommandText = this._bkAlter.ToString();
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = this._strImport.ToString();
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = this._strCopy.ToString();
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = this._strDrop.ToString();
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = this._strRename.ToString();
                                cmd.ExecuteNonQuery();
                            }
                            tr.Commit();
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        _info = ex.Message;
                        if (tr != null)
                        {
                            try
                            {
                                tr.Rollback();
                            }
                            catch (ObjectDisposedException ex2)
                            {
                                _info = ex2.Message;
                            }
                            finally
                            {
                                tr.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        tr.Dispose();
                        cmd.Dispose();
                    }
                    return _info;
                }
                else return "ErrorAutoBackup";
            }
        }
        private bool _stringInList(string _str, List<string> _lst)
        {
            bool rs = false;
            foreach(string _string in _lst)
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
        private void _BuildStringForCopyFromSQL2Sqlite(string _sqlSource)
        {
            foreach (TableModel _tbl in Tables)
            {
                this._oldTable.Add(_tbl.TableName);
                this._bkTable.Add(_bk + _tbl.TableName);
                _bkAlter.AppendLine("alter table " + _tbl.TableName + " RENAME TO " + _bk + _tbl.TableName + "; ");
            }
            using (StreamReader reader = StaticClass.GeneralClass.DecryptFileGD(_sqlSource, CashierRegisterDAL.ConnectionDB.getKeyDecrypt()))// new StreamReader(_sqlSource, System.Text.Encoding.UTF8, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        line = line.Replace("IDENTITY(1,1)", "AUTOINCREMENT");
                        if (line.Contains("CREATE UNIQUE INDEX"))
                        {
                            line = line.Replace("CREATE UNIQUE INDEX", "CREATE UNIQUE INDEX IF NOT EXISTS");
                        }
                        _strImport.AppendLine(line.Replace("<N/>", ""));
                        if (line.Contains("CREATE TABLE "))
                        {
                            string _strTemp = line.Substring(0, line.IndexOf("("));
                            _newTable.Add(_strTemp.Substring("CREATE TABLE ".Length).Trim().Replace("`", "\""));
                        }
                    }
                }
                reader.Dispose();
            }
            if (DataFromIOS)
            {
                DataTable _dt = TableModel.getListTables();
                if (_dt.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dt.Rows)
                    {
                        if (this._stringInList(_dr["TblName"].ToString(), _oldTable))
                        {
                            _strRename.AppendLine("alter table " + _bk + _dr["TblName"].ToString() + " RENAME TO " + _dr["TblName"].ToString() + "; ");
                            if (_dr["TblName"].ToString() != "tb_Setting")
                            {
                                _strCopy.AppendLine("delete from " + _bk + _dr["TblName"].ToString() + "; UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = '" + _dr["TblName"].ToString() + "'; ");
                                _strDrop.AppendLine("drop table if exists " + _dr["TblName"].ToString() + "; ");
                            }
                            switch (_dr["TblName"].ToString())
                            {
                                case "tb_Category":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_Category select CategoryID, CategoryName from tb_Category;");
                                    break;
                                case "tb_Customer":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_Customer select CustomerID, FirstName, LastName, Address1, Address2, City, State, Zipcode, PhoneNumber, Email from tb_Customer;");
                                    break;
                                case "tb_GiftCard":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_GiftCard select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, 0 from tb_GiftCard;");
                                    break;
                                case "tb_Order":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_Order select OrderID, o.CustomerID, FirstName || ' ' || LastName, Quantity, CAST(datetime(DateT, 'unixepoch', 'localtime') as nvarchar), o.SalesPersonID, u.Name, o.PaymentID, pm.Name, IsRateDiscountOrder, Discount, 0, Tax, Total from tb_Order as o left join tb_Customer as c on c.CustomerID = o.CustomerID left join tb_User as u on u.UserID = o.SalesPersonID left join tb_Payment as pm on pm.PaymentID = o.PaymentID;");
                                    break;
                                case "tb_OrderDetail":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_OrderDetail select ID, 0, NULL, od.ProductID, ShortName || ' ' || LongName, Cost, Price, Quantity, Tax, -1, Discount, DiscountMain, (Price-Discount), od.OrderID from tb_OrderDetail as od left join tb_Product as p on p.ProductID = od.ProductID;");
                                    break;
                                case "tb_Payment":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_Payment select PaymentID, Name from tb_Payment;");
                                    break;
                                case "tb_Product":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_Product select ProductID, Barcode, ShortName, LongName, Cost, Price, Qty, CategoryID, Taxable, " + @"'\images\product_' || ProductID || '.png', 0, Active, NULL, NULL from tb_Product;");
                                    break;
                                case "tb_Salesperson":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_Salesperson select UserID, Name, NULL, NULL, NULL, Password, IsActive, 0 from tb_User where Type = 'Salesperson';");
                                    break;
                                case "tb_Currency":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_Currency select PaymentID, Symbol, CASE WHEN PaymentID = 33 THEN 4 WHEN PaymentID = 30 THEN 6 WHEN PaymentID in (4,8,10,12,14,18,20,22,24,26,31,35,37,39,41,43,45,47,49,51,53,55,57,59) THEN 5 ELSE 10 END, CASE WHEN Symbol = '$' THEN 1 ELSE 0 END, 1 from tb_Currency;");
                                    break;
                                case "tb_Setting":
                                    _hashBackupSettings = new System.Collections.Hashtable();
                                    SQLiteCommand _cmd = new SQLiteCommand("select SettingKey, SettingValue from tb_Setting where SettingKey in('SHOP_NAME','SHOP_ADDRESS','SHOP_PHONE','SHOP_WEBSITE','RECEIPT_HEADER','RECEIPT_FOOTER')", CashierRegisterDAL.ConnectionDB.getSQLiteConnection());
                                    SQLiteDataReader _rs = _cmd.ExecuteReader();
                                    while (_rs.Read())
                                    {
                                        _hashBackupSettings.Add(_rs["SettingKey"].ToString(), _rs["SettingValue"].ToString());
                                    }
                                    break;
                                case "tb_User":
                                    _strCopy.AppendLine("insert into " + _bk + "tb_User select UserID, Name, NULL, NULL, Password, NULL, NULL from tb_User where Type <> 'Salesperson';");
                                    break;
                                default:
                                    _strCopy.AppendLine("insert into " + _bk + _dr["TblName"].ToString() + " select * from " + _dr["TblName"].ToString() + ";");
                                    break;
                            }
                        }
                        else
                        {
                            _strRename.AppendLine("alter table " + _bk + _dr["TblName"].ToString() + " RENAME TO " + _dr["TblName"].ToString() + "; ");
                        }
                    }
                }
            }
            else
            {
                foreach (string _name in _oldTable)
                {
                    if (this._stringInList(_name, this._newTable))
                    {
                        this._strCopy.AppendLine("delete from " + _bk + _name + "; UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = '" + _bk + _name + "'; ");
                        this._strRename.AppendLine("alter table " + _bk + _name + " RENAME TO " + _name + "; ");
                        this._strDrop.AppendLine("drop table if exists " + _name + "; ");
                        this._strCopy.AppendLine("insert into " + _bk + _name + " select * from " + _name + ";");
                    }
                    else
                    {
                        this._strRename.AppendLine("alter table " + _bk + _name + " RENAME TO " + _name + "; ");
                    }
                }
            }
        }
        private void _BuildStringForCopyFromSQL2SqlServer(string _sqlSource)
        {
            foreach (TableModel _tbl in Tables)
            {
                this._oldTable.Add(_tbl.TableName);
                this._bkTable.Add(_bk + _tbl.TableName);
                _bkAlter.AppendLine("exec sp_rename '" + _tbl.TableName + "', '" + _bk + _tbl.TableName + "'; ");
            }
            using (StreamReader reader = StaticClass.GeneralClass.DecryptFileGD(_sqlSource, CashierRegisterDAL.ConnectionDB.getKeyDecrypt()))//new StreamReader(_sqlSource, System.Text.Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("insert into "))
                        {
                            string _strTemp = line.Substring(0, line.IndexOf("("));
                            _strTemp = _strTemp.Substring("insert into ".Length).Trim();
                            System.Text.StringBuilder _strInsert = new System.Text.StringBuilder();
                            _strInsert.AppendLine("if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _strTemp + "', 'U') and is_identity = 1)");
                            _strInsert.AppendLine("begin");
                            _strInsert.AppendLine("SET IDENTITY_INSERT " + _strTemp + " ON; ");
                            _strInsert.AppendLine(line.Replace("<N/>", "N"));
                            _strInsert.AppendLine("SET IDENTITY_INSERT " + _strTemp + " OFF;");
                            _strInsert.AppendLine("end");
                            _strInsert.AppendLine("else");
                            _strInsert.AppendLine(line.Replace("<N/>", "N"));
                            _lstInsert.Add(_strInsert.ToString());
                        }
                        else if (line.Contains("CREATE TABLE "))
                        {
                            string _strTemp = line.Substring(0, line.IndexOf("("));
                            string _tbName_ = _strTemp.Substring("CREATE TABLE ".Length).Trim();
                            _newTable.Add(_tbName_);
                            line = line.Replace("TEXT UNIQUE", "nvarchar UNIQUE");
                            _tbName_ = _tbName_.Replace("\"", "").Replace("[", "").Replace("]", "");
                            if (_tbName_ == "tb_Currency")
                            {
                                line = line.Replace("VARCHAR NOT NULL", "VARCHAR(10) NOT NULL");
                            }
                            _strImport.AppendLine(line.Replace("<N/>", "N").Replace("`", "\"").Replace("BOOL", "BIT"));
                        }
                        else _strImport.AppendLine(line.Replace("<N/>", "N"));
                    }
                }
                reader.Dispose();
            }
            if (DataFromIOS)
            {
                foreach (string _strName in _oldTable)
                {
                    if (this._stringInList(_strName, this._newTable))
                    {
                        _strRename.AppendLine("exec sp_rename '" + _bk + _strName + "', '" + _strName + "'; ");
                        if(_strName != "tb_Setting")
                        {
                            _strCopy.AppendLine("TRUNCATE TABLE " + _bk + _strName + ";");
                            _strDrop.AppendLine("If Exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = '" + _strName + "') drop table " + _strName + "; ");
                        }
                        System.Text.StringBuilder _strInsert = new System.Text.StringBuilder();
                        switch (_strName)
                        {
                            case "tb_Category":
                                _strInsert.AppendLine(_bk + "tb_Category$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_Category', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Category ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Category(<listcolumn>) select CategoryID, CategoryName from tb_Category;");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Category OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Category(<listcolumn>) select CategoryID, CategoryName from tb_Category;");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            case "tb_Customer":
                                _strInsert.AppendLine(_bk + "tb_Customer$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_Customer', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Customer ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Customer(<listcolumn>) select CustomerID, FirstName, LastName, Address1, Address2, City, State, Zipcode, PhoneNumber, Email from tb_Customer;");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Customer OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Customer(<listcolumn>) select CustomerID, FirstName, LastName, Address1, Address2, City, State, Zipcode, PhoneNumber, Email from tb_Customer;");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            case "tb_GiftCard":
                                _strInsert.AppendLine(_bk + "tb_GiftCard$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_GiftCard', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_GiftCard ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_GiftCard(<listcolumn>) select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, 0 from tb_GiftCard;");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_GiftCard OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_GiftCard(<listcolumn>) select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, 0 from tb_GiftCard;");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            case "tb_Order":
                                _strInsert.AppendLine(_bk + "tb_Order$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_Order', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Order ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Order(<listcolumn>) select OrderID, o.CustomerID, CONCAT(FirstName, ' ', LastName), Quantity, CAST(datetime(DateT, 'unixepoch', 'localtime') as nvarchar), o.SalesPersonID, u.Name, o.PaymentID, pm.Name, IsRateDiscountOrder, Discount, 0, Tax, Total from tb_Order as o left join tb_Customer as c on c.CustomerID = o.CustomerID left join tb_User as u on u.UserID = o.SalesPersonID left join tb_Payment as pm on pm.PaymentID = o.PaymentID;");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Order OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Order(<listcolumn>) select OrderID, o.CustomerID, CONCAT(FirstName, ' ', LastName), Quantity, CAST(datetime(DateT, 'unixepoch', 'localtime') as nvarchar), o.SalesPersonID, u.Name, o.PaymentID, pm.Name, IsRateDiscountOrder, Discount, 0, Tax, Total from tb_Order as o left join tb_Customer as c on c.CustomerID = o.CustomerID left join tb_User as u on u.UserID = o.SalesPersonID left join tb_Payment as pm on pm.PaymentID = o.PaymentID;");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            case "tb_OrderDetail":
                                _strInsert.AppendLine(_bk + "tb_OrderDetail$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_OrderDetail', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_OrderDetail ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_OrderDetail(<listcolumn>) select ID, 0, NULL, od.ProductID, CONCAT(ShortName, ' ', LongName), Cost, Price, Quantity, Tax, -1, Discount, DiscountMain, (Price-Discount), od.OrderID from tb_OrderDetail as od left join tb_Product as p on p.ProductID = od.ProductID;");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_OrderDetail OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_OrderDetail(<listcolumn>) select ID, 0, NULL, od.ProductID, CONCAT(ShortName, ' ', LongName), Cost, Price, Quantity, Tax, -1, Discount, DiscountMain, (Price-Discount), od.OrderID from tb_OrderDetail as od left join tb_Product as p on p.ProductID = od.ProductID;");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            case "tb_Payment":
                                _strInsert.AppendLine(_bk + "tb_Payment$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_Payment', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Payment ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Payment(<listcolumn>) select PaymentID, Name from tb_Payment;");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Payment OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Payment(<listcolumn>) select PaymentID, Name from tb_Payment;");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            case "tb_Product":
                                _strInsert.AppendLine(_bk + "tb_Product$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_Product', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Product ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Product(<listcolumn>) select ProductID, Barcode, ShortName, LongName, Cost, Price, Qty, CategoryID, Taxable, " + @"'\images\product_' || ProductID || '.png', 0, Active, NULL, NULL from tb_Product;");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Product OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Product(<listcolumn>) select ProductID, Barcode, ShortName, LongName, Cost, Price, Qty, CategoryID, Taxable, " + @"'\images\product_' || ProductID || '.png', 0, Active, NULL, NULL from tb_Product;");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            case "tb_Salesperson":
                                _strInsert.AppendLine(_bk + "tb_Salesperson$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_Salesperson', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Salesperson ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Salesperson(<listcolumn>) select UserID, Name, NULL, NULL, NULL, Password, IsActive, 0 from tb_User where Type = 'Salesperson';");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Salesperson OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Salesperson(<listcolumn>) select UserID, Name, NULL, NULL, NULL, Password, IsActive, 0 from tb_User where Type = 'Salesperson';");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            case "tb_Currency":
                                _strInsert.AppendLine(_bk + "tb_Currency$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_Currency', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Currency ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Currency(<listcolumn>) select PaymentID, Symbol, CASE WHEN PaymentID = 33 THEN 4 WHEN PaymentID = 30 THEN 6 WHEN PaymentID in (4,8,10,12,14,18,20,22,24,26,31,35,37,39,41,43,45,47,49,51,53,55,57,59) THEN 5 ELSE 10 END, CASE WHEN Symbol = '$' THEN 1 ELSE 0 END, 1 from tb_Currency;");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_Currency OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_Currency(<listcolumn>) select PaymentID, Symbol, CASE WHEN PaymentID = 33 THEN 4 WHEN PaymentID = 30 THEN 6 WHEN PaymentID in (4,8,10,12,14,18,20,22,24,26,31,35,37,39,41,43,45,47,49,51,53,55,57,59) THEN 5 ELSE 10 END, CASE WHEN Symbol = '$' THEN 1 ELSE 0 END, 1 from tb_Currency;");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            case "tb_Setting":
                                _hashBackupSettings = new System.Collections.Hashtable();
                                SqlCommand _cmd = new SqlCommand("select SettingKey, SettingValue from tb_Setting where SettingKey in('SHOP_NAME','SHOP_ADDRESS','SHOP_PHONE','SHOP_WEBSITE','RECEIPT_HEADER','RECEIPT_FOOTER')", CashierRegisterDAL.ConnectionDB.getSQLConnection());
                                SqlDataReader _rs = _cmd.ExecuteReader();
                                while (_rs.Read())
                                {
                                    _hashBackupSettings.Add(_rs["SettingKey"].ToString(), _rs["SettingValue"].ToString());
                                }
                                break;
                            case "tb_User":
                                _strInsert.AppendLine(_bk + "tb_User$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + "tb_User', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_User ON; ");
                                _strInsert.AppendLine("insert into " + _bk + "tb_User(<listcolumn>) select UserID, Name, NULL, NULL, Password, NULL, NULL from tb_User where Type <> 'Salesperson';");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + "tb_User OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + "tb_User(<listcolumn>) select UserID, Name, NULL, NULL, Password, NULL, NULL from tb_User where Type <> 'Salesperson';");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                            default:
                                _strInsert.AppendLine(_bk + _strName + "$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + _strName + "', 'U') and is_identity = 1)");
                                _strInsert.AppendLine("begin");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + _strName + " ON; ");
                                _strInsert.AppendLine("insert into " + _bk + _strName + "(<listcolumn>) select * from " + _strName + ";");
                                _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + _strName + " OFF;");
                                _strInsert.AppendLine("end");
                                _strInsert.AppendLine("else");
                                _strInsert.AppendLine("insert into " + _bk + _strName + "(<listcolumn>) select * from " + _strName + ";");
                                this._lstMove.Add(_strInsert.ToString());
                                break;
                        }
                    }
                    else
                    {
                        _strRename.AppendLine("exec sp_rename '" + _bk + _strName + "', '" + _strName + "'; ");
                    }
                }
            }
            else
            {
                foreach (string _name in _oldTable)
                {
                    if (this._stringInList(_name, this._newTable))
                    {
                        this._strCopy.AppendLine("TRUNCATE TABLE " + _bk + _name + "; ");
                        this._strRename.AppendLine("exec sp_rename '" + _bk + _name + "', '" + _name + "'; ");
                        System.Text.StringBuilder _strInsert = new System.Text.StringBuilder();
                        _strInsert.AppendLine(_bk + _name + "$if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _bk + _name + "', 'U') and is_identity = 1)");
                        _strInsert.AppendLine("begin");
                        _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + _name + " ON; ");
                        _strInsert.AppendLine("insert into " + _bk + _name + "(<listcolumn>) select * from " + _name + ";");
                        _strInsert.AppendLine("SET IDENTITY_INSERT " + _bk + _name + " OFF;");
                        _strInsert.AppendLine("end");
                        _strInsert.AppendLine("else");
                        _strInsert.AppendLine("insert into " + _bk + _name + "(<listcolumn>) select * from " + _name + ";");
                        this._lstMove.Add(_strInsert.ToString());
                        this._strDrop.AppendLine("If Exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = '" + _name + "') drop table " + _name + "; ");
                    }
                    else
                    {
                        _strRename.AppendLine("exec sp_rename '" + _bk + _name + "', '" + _name + "'; ");
                    }
                }
            }
        }
        private bool _isDataIOS = false;
        public bool DataFromIOS
        {
            get { return _isDataIOS; }
            set
            {
                _isDataIOS = value;
                RaisePropertyChanged("DataFromIOS");
            }
        }
        private string _ImportFromCSV(string _file, string _fileImport)
        {
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
                            string _strImport = _BuildStringImportFormCSV(_fileImport);
                            if (!string.IsNullOrEmpty(_strImport))
                            {
                                cmd.Transaction = tr;
                                cmd.CommandText = "TRUNCATE TABLE " + _file;
                                cmd.ExecuteNonQuery();
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
                            string _strImport = _BuildStringImportFormCSV(_fileImport);
                            if (!string.IsNullOrEmpty(_strImport))
                            {
                                cmd.Transaction = tr;
                                cmd.CommandText = "delete from " + _file + "; UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = '" + _file + "'; ";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = _strImport.Replace("<N/>", ""); ;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tr.Commit();
                    }
                    tr.Dispose();
                    cmd.Dispose();
                    return "Success";
                }
                catch(SQLiteException ex)
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
            string _lstColumn = string.Empty;
            string _insert = string.Empty;
            using (StreamReader reader = new StreamReader(_pathFileImport))
            {
                _lstColumn = reader.ReadLine().Trim();
            }
            writeSchema(_pathFileImport);
            FileInfo fi = new FileInfo(_pathFileImport);
            string strConnString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + fi.DirectoryName.ToString() + ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
            string sql_select = "select * from [" + fi.Name.ToString() + "]";
            string _tblName = string.Empty;
            string _sqlImport = string.Empty;
            OdbcConnection conn;
            conn = new OdbcConnection(strConnString.Trim());
            conn.Open();
            OdbcCommand commandSourceData = new OdbcCommand(sql_select, conn);
            OdbcDataReader dataReader = commandSourceData.ExecuteReader();
            int _ttField = dataReader.FieldCount;
            if (_lstColumn == "CategoryID,CategoryName")
            {
                _tblName = "tb_Category";
            }
            else if (_lstColumn == "PaymentID,Symbol")
            {
                _tblName = "tb_Currency";
            }
            else if (_lstColumn == "CustomerID,FirstName,LastName,Address1,Address2,City,State,Zipcode,Phone,Email")
            {
                _tblName = "tb_Customer";
            }
            else if (_lstColumn == "GiftCardID,Barcode,Serial,CreateDate,ExpirationDate,CustomerIDUse,Amount,Balance,DeliveredDate")
            {
                _tblName = "tb_GiftCard";
            }
            else if (_lstColumn == "InputID,ProductID,ProductName,InputDate,UserID,UserName,Cost,Price,InventoryCount,CategoryID,CategoryName,Tax,Active,Country,Size_Weight")
            {
                _tblName = "tb_InputHistory";
            }
            else if (_lstColumn == "LocationTrackerID,ShortName,Address")
            {
                _tblName = "tb_LocationTracker";
            }
            else if (_lstColumn == "ID,ProductID,Quantity,IsRateDiscount,Discount,DateNoSale,UserIDNoSale")
            {
                _tblName = "tb_NoSale";
            }
            else if (_lstColumn == "OrderID,CustomerID,CustomerName,Quantity,OrderDate,SalespersonID,SalespersonName,PaymentID,PaymentName,DiscountType,Discount,TotalDiscount,TotalTax,TotalAmount")
            {
                _tblName = "tb_Order";
            }
            else if (_lstColumn == "ID,CategoryID,CategoryName,ProductID,ProductName,Cost,Price,Qty,Tax,DiscountType,Discount,TotalDiscount,Total,OrderID")
            {
                _tblName = "tb_OrderDetail";
            }
            else if (_lstColumn == "OrderPaymentID,OrderID,PaymentID,CardID,Amount")
            {
                _tblName = "tb_OrderPayment";
            }
            else if (_lstColumn == "PaymentID,Card")
            {
                _tblName = "tb_Payment";
            }
            else if (_lstColumn == "ProductID,BarcodeID,ShortName,LongName,Cost,Price,InventoryCount,CategoryID,Tax,PathImage,Capture,Active,Country,Size_Weight")
            {
                _tblName = "tb_Product";
            }
            else if (_lstColumn == "PromoCodeID,Barcode,Serial,CreateDate,ExpirationDate,UsedDate,OrderIDUse,IsRate,Amount,MaxAmount,TypeID,ProductID")
            {
                _tblName = "tb_PromoCode";
            }
            else if (_lstColumn == "SalespersonID,Name,Birthday,Address,Email,Password,Active,Default")
            {
                _tblName = "tb_Salesperson";
            }
            else if (_lstColumn == "SettingID,Currency,TaxRate,Active,Version")
            {
                _tblName = "tb_Setting";
            }
            else if (_lstColumn == "ID,Name,Email,Address,Password,Question,Answer")
            {
                _tblName = "tb_User";
            }
            else if (_lstColumn == "ZReportID,OpenTime,OpenAmount,OpenUserID,CloseTime,CloseAmount,CloseUserID,Short,Transactions,NoSales,CashSales,CashReturns,Drops,Payouts,Payins,Sales,Discounts,Returns,Tax,NetSales,GiftCard,Cash,Credit")
            {
                _tblName = "tb_ZReport";
            }
            if (!string.IsNullOrEmpty(_tblName))
            {
                while (dataReader.Read())
                {
                    string _strVal = string.Empty;
                    for (int _j = 0; _j < _ttField; _j++)
                    {
                        if (string.IsNullOrEmpty(dataReader[_j].ToString()) || IsNumeric(dataReader[_j]))
                            _strVal += (!string.IsNullOrEmpty(_strVal)) ? ",'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'" : "'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'";
                        else _strVal += (!string.IsNullOrEmpty(_strVal)) ? ",<N/>'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'" : "<N/>'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'";
                        //_strVal += (!string.IsNullOrEmpty(_strVal)) ? ",'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'" : "'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'";
                    }
                    _insert = string.Format("insert into {0} ({1}) values ({2}); ", _tblName, string.Format("[{0}]", _lstColumn.Replace(",", "],[")), _strVal);
                    if (StaticClass.GeneralClass.flag_database_type_general)
                    {
                        _insert = "if exists (select column_id from sys.columns where object_id = OBJECT_ID('" + _tblName + "', 'U') and is_identity = 1) begin SET IDENTITY_INSERT " + _tblName + " ON; " + _insert + " SET IDENTITY_INSERT " + _tblName + " OFF; end else " + _insert;
                    }
                    _sqlImport += _insert;
                }
            }
            conn.Close();
            System.Threading.Thread.Sleep(500);
            return _sqlImport;
        }
        private void _eventTimerExport(object sender, EventArgs e)
        {
            if (_isExportRuning)
            {
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                _isExportRuning = false;
                _exportStatus = 0;
            }
            else
            {
                if (_exportStatus == 0)
                {
                    _exportStatus = 1;
                    ExportType _exportType = ExportTypeDB;
                    if (_exportType == ExportType.Images)
                        _onlyExportImage();
                    else if (_exportType == ExportType.Database)
                        _onlyExportDatabase();
                    else if (_exportType == ExportType.Both)
                        _exportImgAndDatabase();
                }
            }
        }
        private async void _onlyExportImage()
        {
            var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipImageProducts());
            await slowTask;
            _isExportRuning = true;
            if (slowTask.Result.ToString() == "Success")
            {
                _connecting.Close();
                this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
            }
            else
            {
                _connecting.Close();
                this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
            }
        }
        private async void _onlyExportDatabase()
        {
            DBFileFormat _dbType = DBFormat;
            if (_dbType == DBFileFormat.csv)
            {
                var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipCSV(FileOrFolder.file));
                await slowTask;
                _isExportRuning = true;
                if (slowTask.Result.ToString() == "Success")
                {
                    _connecting.Close();
                    this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                }
                else
                {
                    _connecting.Close();
                    this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
                }
            }
            else if (_dbType == DBFileFormat.db)
            {
                if (StaticClass.GeneralClass.flag_database_type_general)
                {
                    var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipSQLServer2SQLite(FileOrFolder.file));
                    await slowTask;
                    _isExportRuning = true;
                    if (slowTask.Result.ToString() == "Success")
                    {
                        _connecting.Close();
                        this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                    }
                    else
                    {
                        _connecting.Close();
                        this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
                    }
                }
                else
                {
                    var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipSQLite2SQLite(FileOrFolder.file));
                    await slowTask;
                    _isExportRuning = true;
                    if (slowTask.Result.ToString() == "Success")
                    {
                        _connecting.Close();
                        this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                    }
                    else
                    {
                        _connecting.Close();
                        this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
                    }
                }
            }
            else if (_dbType == DBFileFormat.sql)
            {
                if (StaticClass.GeneralClass.flag_database_type_general)
                {
                    var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipSQLServer2SQLServer(FileOrFolder.file));
                    await slowTask;
                    _isExportRuning = true;
                    if (slowTask.Result.ToString() == "Success")
                    {
                        _connecting.Close();
                        this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                    }
                    else
                    {
                        _connecting.Close();
                        this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
                    }
                }
                else
                {
                    var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipSQLite2SQLServer(FileOrFolder.file, _pathExport + @"\CheckOut.sql"));
                    await slowTask;
                    _isExportRuning = true;
                    if (slowTask.Result.ToString() == "Success")
                    {
                        _connecting.Close();
                        this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                    }
                    else
                    {
                        _connecting.Close();
                        this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
                    }
                }
            }
        }
        private async void _exportImgAndDatabase()
        {
            DBFileFormat _dbType = DBFormat;
            if (_dbType == DBFileFormat.csv)
            {
                var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipCSV(FileOrFolder.folder));
                await slowTask;
                _isExportRuning = true;
                if (slowTask.Result.ToString() == "Success")
                {
                    _connecting.Close();
                    this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                }
                else
                {
                    _connecting.Close();
                    this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
                }
            }
            else if (_dbType == DBFileFormat.sql)
            {
                if (StaticClass.GeneralClass.flag_database_type_general)
                {
                    var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipSQLServer2SQLServer(FileOrFolder.folder));
                    await slowTask;
                    _isExportRuning = true;
                    if (slowTask.Result.ToString() == "Success")
                    {
                        _connecting.Close();
                        this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                    }
                    else
                    {
                        _connecting.Close();
                        this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
                    }
                }
                else
                {
                    var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipSQLite2SQLServer(FileOrFolder.folder, _pathExport + @"\CheckOut.sql"));
                    await slowTask;
                    _isExportRuning = true;
                    if (slowTask.Result.ToString() == "Success")
                    {
                        _connecting.Close();
                        this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                    }
                    else
                    {
                        _connecting.Close();
                        this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
                    }
                }
            }
            else if (_dbType == DBFileFormat.db)
            {
                if (StaticClass.GeneralClass.flag_database_type_general)
                {
                    var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipSQLServer2SQLite(FileOrFolder.folder));
                    await slowTask;
                    _isExportRuning = true;
                    if (slowTask.Result.ToString() == "Success")
                    {
                        _connecting.Close();
                        this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                    }
                    else
                    {
                        _connecting.Close();
                        this._showNotify(slowTask.Result.ToString()+"\t\t\t\t", "close");
                    }
                }
                else
                {
                    var slowTask = System.Threading.Tasks.Task<string>.Factory.StartNew(() => this._IonicZipSQLite2SQLite(FileOrFolder.folder));
                    await slowTask;
                    _isExportRuning = true;
                    if (slowTask.Result.ToString() == "Success")
                    {
                        _connecting.Close();
                        this._showNotify(App.Current.FindResource("export_success_tab").ToString(), "close");
                    }
                    else
                    {
                        _connecting.Close();
                        this._showNotify(slowTask.Result.ToString()+ "\t\t\t\t", "close");
                    }
                }
            }
        }
        private void onExportExec(object _param)
        {
            bool _checked = false;
            foreach(TableModel _tbl in Tables)
            {
                if (_tbl.IsChecked)
                {
                    _checked = true;
                    break;
                }
            }
            if (!_checked && DBFormat==DBFileFormat.csv && ExportTypeDB != ExportType.Images)
            {
                this._showNotify(App.Current.FindResource("select_tbl_export").ToString(), "ok");
            }
            else
            {
                dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = string.Format("CashierRegister_{0:MMMddyyyy_HHmmssfff}", DateTime.Now); // Default file name
                dlg.DefaultExt = ".zip"; // Default file extension
                dlg.Filter = "Text documents (.zip)|*.zip"; // Filter files by extension
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    _myTimer_ = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
                    _connecting = new Pages.Setting.Waiting();
                    _connecting.StartPosition = FormStartPosition.CenterScreen;
                    _connecting.ShowInTaskbar = false;
                    _threadConnect = new System.Threading.Thread(() =>
                    {
                        _myTimer_.Tick += _eventTimerExport;
                        _myTimer_.Start();
                    });
                    _threadConnect.SetApartmentState(System.Threading.ApartmentState.STA);
                    _threadConnect.Start();
                    _threadConnect.Join();
                    _connecting.ShowDialog();
                    if (num_export > 0)
                    {
                        foreach (TableModel _tbl in Tables)
                        {
                            if (_tbl.IsChecked && File.Exists(_pathExport + @"\" + _tbl.TableName + @".csv"))
                            {
                                File.Delete(_pathExport + @"\" + _tbl.TableName + @".csv");
                            }
                        }
                    }
                    DBFileFormat _dbType_ = DBFormat;
                    if (_dbType_ != DBFileFormat.csv && File.Exists(_pathExport + @"\CheckOut." + _dbType_))
                    {
                        File.Delete(_pathExport + @"\CheckOut." + _dbType_);
                    }
                }
            }
        }
        private string _IonicZipCSV(FileOrFolder _opts)
        {
            ZipFile _zipArc = new ZipFile();
            _zipArc.Password = _keyZip;
            try
            {
                foreach (TableModel _tbl in Tables)
                {
                    if (_tbl.IsChecked)
                    {
                        string _strTblName = _pathExport + @"\" + _tbl.TableName + ".csv";
                        using (StreamWriter stream_writer = new StreamWriter(_strTblName))
                        {
                            string _lstColumn = this.getListColumnName(_tbl.TableName).Replace("[", "");
                            _lstColumn = _lstColumn.Replace("]", "");
                            stream_writer.WriteLine(_lstColumn.Replace(", ", ","));
                            DataTable _dt = TableModel.getAllDataTable(_tbl.TableName);
                            if (_dt.Rows.Count > 0)
                            {
                                foreach (DataRow _dr in _dt.Rows)
                                {
                                    string _rs = string.Empty;
                                    DataTable _dt_ = TableModel.getColumnsByTable(_tbl.TableName);
                                    foreach (DataRow _dr_ in _dt_.Rows)
                                    {
                                        string _strComp = _dr_["type"].ToString().ToLower();
                                        if (_strComp.Contains("text") || _strComp.Contains("ntext") || _strComp.Contains("varchar") || _strComp.Contains("nvarchar") || _strComp.Contains("char") || _strComp.Contains("nchar") || _strComp.Contains("xml"))
                                        {
                                            _rs += "\"" + @_dr[_dr_["name"].ToString()].ToString().Replace("\"", @"\u0022") + "\",";
                                        }
                                        else _rs += _dr[_dr_["name"].ToString()].ToString() + ",";
                                    }
                                    _rs = _rs.Remove(_rs.Length - 1, 1);
                                    stream_writer.WriteLine(_rs);
                                }
                            }
                            stream_writer.Close();
                        }
                        if (_opts == FileOrFolder.file)
                        {
                            _zipArc.AddFile(_strTblName, string.Empty);
                        }
                        num_export++;
                    }
                }
                if (_opts == FileOrFolder.folder)
                {
                    _zipArc.AddDirectory(_pathExport);
                }
                _zipArc.Save(dlg.FileName);
                _zipArc.Dispose();
                return "Success";
            }
            catch
            {
                _zipArc.Dispose();
                return "Error";
            }
        }
        private string _IonicZipSQLite2SQLServer(FileOrFolder _opts, string _sqlFileName)
        {
            ZipFile _zipArc = new ZipFile();
            _zipArc.Password = _keyZip;
            try
            {
                if (File.Exists(_pathExport + @"\CheckOut_.sql"))
                    File.Delete(_pathExport + @"\CheckOut_.sql");
                using (StreamWriter stream_writer = new StreamWriter(_pathExport + @"\CheckOut_.sql", false, System.Text.Encoding.UTF8))
                {
                    foreach (TableModel _tbl in Tables)
                    {
                        string _structure = _tbl.TableStructure.ToString();
                        _structure = _structure.Replace("\n", "");
                        _structure = _structure.Replace("\t", "");
                        _structure = _structure.Replace("DOUBLE", "decimal");
                        _structure = _structure.Replace("AUTOINCREMENT", "IDENTITY(1,1)");
                        _structure = _structure.Replace(" COLLATE NOCASE", "");
                        stream_writer.WriteLine(_structure + "; ");
                        string _strAdd = SQLiteGenerateStructure(_tbl.TableName);
                        if (!string.IsNullOrEmpty(_strAdd))
                            stream_writer.WriteLine(_strAdd + ";");
                    }
                    stream_writer.Close();
                }
                StaticClass.GeneralClass.EncryptFileGD(_pathExport + @"\CheckOut_.sql", _sqlFileName, CashierRegisterDAL.ConnectionDB.getKeyDecrypt());
                if (_opts == FileOrFolder.file)
                {
                    _zipArc.AddFile(_sqlFileName, string.Empty);
                }
                else if (_opts == FileOrFolder.folder)
                {
                    _zipArc.AddDirectory(_pathExport);
                }
                _zipArc.Save(dlg.FileName);
                _zipArc.Dispose();
                File.Delete(_pathExport + @"\CheckOut_.sql");
                return "Success";
            }
            catch
            {
                _zipArc.Dispose();
                return "Error";
            }
        }
        private string _IonicZipSQLite2SQLite(FileOrFolder _opts)
        {
            ZipFile _zipArc = new ZipFile();
            _zipArc.Password = _keyZip;
            try
            {
                if (_opts == FileOrFolder.file)
                {
                    CashierRegisterDAL.ConnectionDB.CloseConnect();
                    _zipArc.AddFile(Directory.GetCurrentDirectory() + @"\Databases\CheckOut.db", string.Empty);
                    CashierRegisterDAL.ConnectionDB.OpenConnect();
                }
                else if (_opts == FileOrFolder.folder)
                {
                    File.Copy(Directory.GetCurrentDirectory() + @"\Databases\CheckOut.db", _pathExport + @"\CheckOut.db");
                    _zipArc.AddDirectory(_pathExport);
                }
                _zipArc.Save(dlg.FileName);
                _zipArc.Dispose();
                return "Success";
            }
            catch
            {
                _zipArc.Dispose();
                return App.Current.FindResource("cannot_export_sqlite").ToString();
            }
        }
        private string _IonicZipSQLServer2SQLServer(FileOrFolder _opts)
        {
            ZipFile _zipArc = new ZipFile();
            _zipArc.Password = _keyZip;
            if (File.Exists(_pathExport + @"\CheckOut_.sql"))
                File.Delete(_pathExport + @"\CheckOut_.sql");
            try
            {
                Sql2Sqlite.SqlServerToSQLite.ConvertSqlServerToSQLDatabase(GetSqlServerConnectionString(), _pathExport + @"\CheckOut_.sql", null, false, false);
                StaticClass.GeneralClass.EncryptFileGD(_pathExport + @"\CheckOut_.sql", _pathExport + @"\CheckOut.sql", CashierRegisterDAL.ConnectionDB.getKeyDecrypt());
                if (_opts == FileOrFolder.file)
                {
                    _zipArc.AddFile(_pathExport + @"\CheckOut.sql", string.Empty);
                }
                else if (_opts == FileOrFolder.folder)
                {
                    _zipArc.AddDirectory(_pathExport);
                }
                _zipArc.Save(dlg.FileName);
                File.Delete(_pathExport + @"\CheckOut_.sql");
                _zipArc.Dispose();
                return "Success";
            }
            catch
            {
                _zipArc.Dispose();
                return "Error";
            }
        }
        private string _IonicZipSQLServer2SQLite(FileOrFolder _opts)
        {
            ZipFile _zipArc = new ZipFile();
            _zipArc.Password = _keyZip;
            try
            {
                Sql2Sqlite.SqlServerToSQLite.ConvertSqlServerToSQLiteDatabase(GetSqlServerConnectionString(), _pathExport + @"\CheckOut.db", CashierRegisterDAL.ConnectionDB.getKeyDecrypt(), false, false);
                if (_opts == FileOrFolder.file)
                {
                    _zipArc.AddFile(_pathExport + @"\CheckOut.db", string.Empty);
                }
                else if (_opts == FileOrFolder.folder)
                {
                    _zipArc.AddDirectory(_pathExport);
                }
                _zipArc.Save(dlg.FileName);
                _zipArc.Dispose();
                return "Success";
            }
            catch
            {
                _zipArc.Dispose();
                return App.Current.FindResource("failed_sql_sqlite").ToString();
            }
        }
        private string _IonicZipImageProducts()
        {
            ZipFile _zipArc = new ZipFile();
            _zipArc.Password = _keyZip;
            try
            {
                _zipArc.AddDirectory(_pathExport);
                _zipArc.Save(dlg.FileName);
                _zipArc.Dispose();
                return "Success";
            }
            catch
            {
                _zipArc.Dispose();
                return "Error";
            }
        }
        private void _showNotify(string _strNotify, string _lbButton)
        {
            ModernDialog md = new ModernDialog();
            if (_lbButton == "close")
            {
                //md.CloseButton.FindResource("close").ToString();
                md.Buttons = new System.Windows.Controls.Button[] { md.CloseButton };
                md.CloseButton.Content = App.Current.FindResource("close").ToString();
            }
            else
            {
                md.Buttons = new System.Windows.Controls.Button[] { md.OkButton };
                md.OkButton.Content = App.Current.FindResource("ok").ToString();
            }
            md.Content = _strNotify;
            md.Title = App.Current.FindResource("notification").ToString();
            md.ShowDialog();
        }
        private void onFormLoadExec(object _param)
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                DataTable _tbl = TableModel.getListTables();
                if (_tbl.Rows.Count > 0)
                {
                    int _i = 1;
                    foreach (DataRow _dr in _tbl.Rows)
                    {
                        Tables.Add(new TableModel { TableId = _i, ShowTableName = _dr["TblName"].ToString().Insert(2, @"_"), TableName = _dr["TblName"].ToString(), TableColumn = this.getListColumnName(_dr["TblName"].ToString()), TableStructure = _dr["StrCreated"].ToString(), IsChecked = true });
                        _i++;
                    }
                    DBFormat = (!StaticClass.GeneralClass.flag_database_type_general) ? DBFileFormat.db : DBFileFormat.sql;
                }
            }
            else _isLoaded = false;
        }
        private string getListColumnName(string _tblName)
        {
            string _rs = string.Empty;
            if (string.IsNullOrEmpty(_tblName)) return _rs;
            DataTable _cols = TableModel.getColumnsByTable(_tblName);
            if (_cols.Rows.Count > 0)
            {
                foreach(DataRow _dr in _cols.Rows)
                {
                    _rs += "["+_dr["name"].ToString() + "], ";
                }
            }
            if(!string.IsNullOrEmpty(_rs))
                _rs = _rs.Remove(_rs.Length - 2, 2);
            return _rs;
        }
        private DBFileFormat _dbFormat = DBFileFormat.csv;
        public DBFileFormat DBFormat
        {
            get { return _dbFormat; }
            set
            {
                _dbFormat = value;
                RaisePropertyChanged("DBFormat");
                if (_dbFormat == DBFileFormat.csv)
                {
                    VisibleListTable = System.Windows.Visibility.Visible;
                }
                else VisibleListTable = System.Windows.Visibility.Collapsed;
            }
        }
        private ExportType _expType = ExportType.Both;
        public ExportType ExportTypeDB
        {
            get { return _expType; }
            set
            {
                _expType = value;
                RaisePropertyChanged("ExportTypeDB");
            }
        }
        private string SQLiteGenerateStructure(string _tblName)
        {
            string _rs = string.Empty;
            DataTable _dt = TableModel.getAllDataTable(_tblName);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow _dr in _dt.Rows)
                {
                    _rs += "insert into " + _tblName + "(" + getListColumnName(_tblName) + ") values (";
                    DataTable _dt_ = TableModel.getColumnsByTable(_tblName);
                    foreach(DataRow _dr_ in _dt_.Rows)
                    {
                        string _strComp = _dr_["type"].ToString().ToLower();
                        if (string.IsNullOrEmpty(_dr[_dr_["name"].ToString()].ToString()) || _dr[_dr_["name"].ToString()].ToString() == "(null)" || _dr[_dr_["name"].ToString()].ToString() == "null")
                        {
                            _rs += @"null, ";
                        }
                        else
                        {
                            if (_strComp.Contains("text") || _strComp.Contains("ntext") || _strComp.Contains("varchar") || _strComp.Contains("nvarchar") || _strComp.Contains("char") || _strComp.Contains("nchar") || _strComp.Contains("xml"))
                            {
                                _rs += "<N/>'" + @_dr[_dr_["name"].ToString()].ToString().Replace("'", "''") + "', ";
                            }
                            else _rs += "'" + _dr[_dr_["name"].ToString()].ToString() + "', ";
                        }
                    }
                    _rs = _rs.Remove(_rs.Length - 2, 2) + "); ";
                }
                _rs = _rs.Remove(_rs.Length - 2, 2);
            }
            return _rs;
        }
        private string GetSqlServerConnectionString()
        {
            string sqltype = "";
            string server = "";
            int authentication = 0;
            string id = "";
            string password = "";
            string res = string.Empty;
            StreamReader stream_sqltype = StaticClass.GeneralClass.DecryptFileGD(Directory.GetCurrentDirectory() + @"\sqltype", StaticClass.GeneralClass.key_register_general);
            sqltype = stream_sqltype.ReadLine().Split(':')[1].ToString();
            server = stream_sqltype.ReadLine().Split(':')[1].ToString();
            Int32.TryParse(stream_sqltype.ReadLine().Split(':')[1].ToString(), out authentication);
            id = stream_sqltype.ReadLine().Split(':')[1].ToString();
            password = stream_sqltype.ReadLine().Split(':')[1].ToString();
            stream_sqltype.Close();
            if (authentication == 0)
                res = "server = " + server + "; database = "+ CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName() + "; Trusted_Connection = true;";
            else
                res = "server = " + server + "; database = " + CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName() + "; user id = " + id + "; password = " + password + "; integrated security = false;";
            return res;
        }
        private string _getPrimaryKey(string _tblName)
        {
            string _rs = string.Empty;
            DataTable _dt_ = TableModel.getColumnsByTable(_tblName);
            foreach(DataRow _dr_ in _dt_.Rows)
            {
                if (_dr_["pk"].ToString() == "1")
                {
                    _rs = _dr_["name"].ToString()+",";
                }
            }
            return _rs.Remove(_rs.Length-1,1);
        }
        private System.Windows.Visibility _visibleListTable = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility VisibleListTable
        {
            get { return _visibleListTable; }
            set
            {
                _visibleListTable = value;
                RaisePropertyChanged("VisibleListTable");
            }
        }
        private bool _isShowProgress = false;
        public bool IsShowProgress
        {
            get { return _isShowProgress; }
            set
            {
                _isShowProgress = value;
                RaisePropertyChanged("IsShowProgress");
            }
        }
        private string _strFileName = string.Empty;
        public string FileNameImport
        {
            get { return _strFileName; }
            set
            {
                _strFileName = value;
                RaisePropertyChanged("FileNameImport");
            }
        }
        private bool _isDelImg = false;
        public bool IsDeleteImage
        {
            get { return _isDelImg; }
            set
            {
                _isDelImg = value;
                RaisePropertyChanged("IsDeleteImage");
            }
        }
        private string _AutoBackupRestoreSqlServer(BackupRestore _opts, string _DBName, string _path)
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
        private string _AutoBackupRestoreSqlite(BackupRestore _opts, string _path)
        {
            string _info = "Success";
            string _fileBackup = string.Format(@"{0}\{1}.db", _bkFolderNameSQLite, CashierRegisterDAL.ConnectionDB.getSqlServerDataBaseName());
            if (_opts == BackupRestore.backup)
            {
                if (!Directory.Exists(_bkFolderNameSQLite))
                {
                    Directory.CreateDirectory(_bkFolderNameSQLite);
                }
                /*using (SQLiteConnection sqlConnect = CashierRegisterDAL.ConnectionDB.getSQLiteConnection())
                {
                    using (var destination = new SQLiteConnection(@"Data Source="+ _path + @"; Version=3;"))
                    {
                        try
                        {
                            destination.Open();
                            sqlConnect.BackupDatabase(destination, "main", "main", -1, null, 0);
                            destination.Close();
                        }
                        catch(SQLiteException ex)
                        {
                            _info = ex.Message;
                        }
                    }
                }*/
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
                        //File.Delete(_path);
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
    }
    public enum DBFileFormat
    {
        csv, sql, db
    }
    public enum ExportType
    {
        Database, Images, Both
    }
    public enum FileOrFolder
    {
        file, folder
    }
    public enum ImportFileType
    {
        sql, sqlite, db, zip, csv, none
    }
    
}
