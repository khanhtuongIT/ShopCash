using CashierRegister.Helpers;
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
    class ImportCustomer_VM : ModelBase
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
        private string _tblName = string.Empty;
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
        private string _lstColumn = string.Empty;
        private const string _strColumnName = "FirstName,LastName,Address1,Address2,City,State,Zipcode,Phone,Email";
        private string _showError = string.Empty;
        public string showError
        {
            get { return _showError; }
            set
            {
                _showError = value;
                RaisePropertyChanged("showError");
            }
        }
        private Microsoft.Win32.SaveFileDialog _dlg = null;
        public ImportCustomer_VM()
        {
            CancelImportCmd = new RelayCommand(onCancelImportExec);
            BrowseCmd = new RelayCommand(onBrowseExec);
            OkImportCmd = new RelayCommand(onOkImportExec);
            GetSampleFileCmd = new RelayCommand(onGetSampleFileExec);
        }
        private void onGetSampleFileExec(object _param)
        {
            _dlg = new Microsoft.Win32.SaveFileDialog();
            _dlg.FileName = string.Format("file_sample_Customer{0:MMMddyyyy_HHmmssfff}", DateTime.Now); // Default file name
            _dlg.DefaultExt = ".zip"; // Default file extension
            _dlg.Filter = "Text documents (.zip)|*.zip";
            Nullable<bool> result = _dlg.ShowDialog();
            if (result == true)
            {
                Ionic.Zip.ZipFile _zipArc = new Ionic.Zip.ZipFile();
                using (StreamWriter stream_writer = new StreamWriter("tb_Customer.csv", false, System.Text.Encoding.UTF8))
                {
                    stream_writer.WriteLine(_strColumnName);
                    stream_writer.WriteLine("\"James\",\"Butt\",\"6649 N Blue Gum St\",\"\",\"New Orleans\",\"LA\",\"70116\",\"504-621-8927\",\"jbutt@gmail.com\"");
                    stream_writer.WriteLine("\"Josephine\",\"Darakjy\",\"4 B Blue Ridge Blvd\",\"\",\"Brighton\",\"MI\",\"48116\",\"810-292-9388\",\"josephine_darakjy@darakjy.org\"");
                    stream_writer.Close();
                }
                using (StreamWriter stream_writer = new StreamWriter("help.txt", false, System.Text.Encoding.UTF8))
                {
                    stream_writer.WriteLine("1.\tOpen sample file.");
                    stream_writer.WriteLine("\t\t1.1 Go to File > Open.");
                    stream_writer.WriteLine("\t\t\tIf you're using Excel 2007, click the Microsoft Office Button, and then click Open.");
                    stream_writer.WriteLine("\t\t1.2 Select CSV Files from the Open dialog box.");
                    stream_writer.WriteLine("\t\t1.3 Locate and double-click the csv file (tb_Customers.csv) that you want to open.");
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
                _zipArc.AddFile("tb_Customers.csv", string.Empty);
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
                showError = App.Current.FindResource("please_select_file").ToString() + " csv.";
                IsVisError = System.Windows.Visibility.Visible;
                return;
            }
            else if (_lstColumn != _strColumnName)
            {
                showError = App.Current.FindResource("content_file_invalid").ToString() + "\t\t";
                IsVisError = System.Windows.Visibility.Visible;
                return;
            }
            else
            {
                showError = string.Empty;
                IsVisError = System.Windows.Visibility.Collapsed;
            }
            FirstFloor.ModernUI.Windows.Controls.ModernDialog mdd = new FirstFloor.ModernUI.Windows.Controls.ModernDialog();
            mdd.Buttons = new System.Windows.Controls.Button[] { mdd.OkButton, mdd.CancelButton };
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
                    System.Windows.MessageBox.Show(App.Current.FindResource("err_import_tryagain").ToString(), "Error");
                    FileInfo fi = new System.IO.FileInfo(_fileImport);
                    File.Delete(fi.DirectoryName.ToString() + "\\schema.ini");
                }
            }
        }
        private string _ImportFromCSV(string _file, string _fileImport)
        {
            string _rs = "Success";
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
                                if (OptionsImport == TypeImport.Overwrite)
                                {
                                    cmd.CommandText = "TRUNCATE TABLE " + _tblName;
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.CommandText = _strImport.Replace("<N/>", "N"); ;
                                cmd.ExecuteNonQuery();
                            }
                            else _rs = App.Current.FindResource("content_file_invalid").ToString();
                        }
                        tr.Commit();
                    }
                    tr.Dispose();
                    cmd.Dispose();
                    return _rs;
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
                                if (OptionsImport == TypeImport.Overwrite)
                                {
                                    cmd.CommandText = "delete from " + _tblName + "; UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = '" + _tblName + "'; ";
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.CommandText = _strImport.Replace("<N/>","");
                                cmd.ExecuteNonQuery();
                            }
                            else _rs = App.Current.FindResource("content_file_invalid").ToString();
                        }
                        tr.Commit();
                    }
                    tr.Dispose();
                    cmd.Dispose();
                    return _rs;
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
                _tblName = "tb_Customer";
                while (dataReader.Read())
                {
                    string _strVal = string.Empty;
                    for (int _j = 0; _j < _ttField; _j++)
                    {
                        if(string.IsNullOrEmpty(dataReader[_j].ToString()) || IsNumeric(dataReader[_j]))
                            _strVal += (!string.IsNullOrEmpty(_strVal)) ? ",'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'" : "'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'";
                        else _strVal += (!string.IsNullOrEmpty(_strVal)) ? ",<N/>'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'" : "<N/>'" + dataReader[_j].ToString().Replace("'", "''").Replace(@"\u0022", "\"") + "'";
                    }
                    _insert = string.Format("insert into {0} ({1}) values ({2}); ", _tblName, string.Format("[{0}]", _lstColumn.Replace(",", "],[")), _strVal);
                    _sqlImport += _insert;
                }
            }
            conn.Close();
            System.Threading.Thread.Sleep(500);
            return _sqlImport;
        }
    }
    public enum TypeImport
    {
        Overwrite, Append
    }
}
