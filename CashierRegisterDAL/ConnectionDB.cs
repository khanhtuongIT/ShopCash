using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace CashierRegisterDAL
{
    public static class ConnectionDB
    {
        private static string sqltype;
        private static string server = "";
        private static int authentication = 0;
        private static string database_name = "CheckOut";
        private static string key_decryp = "C@$$&O??";
        private static string id = "";
        private static string password = "";
        private static string _current_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        //create connect
        private static SQLiteConnection sqlite_con;
        private static SqlConnection sql_con;
        private static string _SQLConnect = string.Empty;
        public static SQLiteConnection getSQLiteConnection()
        {
            return sqlite_con;
        }
        public static SqlConnection getSQLConnection()
        {
            return sql_con;
        }
        //ConnectionDBInitialize
        public static void ConnectionDBInitialize()
        {
            try
            {
                if (!System.IO.File.Exists(_current_directory + @"\sqltype"))
                {
                    System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(_current_directory + @"\_sqltype");
                    streamwriter.WriteLine("sqltype:sqlite");
                    streamwriter.WriteLine("server:" + server);
                    streamwriter.WriteLine("authentication:" + authentication);
                    streamwriter.WriteLine("id:" + id);
                    streamwriter.WriteLine("password:" + password);
                    streamwriter.Close();
                    EncryptFile(_current_directory + @"\_sqltype", _current_directory + @"\sqltype", key_decryp);
                }

                System.IO.StreamReader streamreader = DecryptFile("sqltype", key_decryp);
                sqltype = streamreader.ReadLine().Split(':')[1].ToString();
                server = streamreader.ReadLine().Split(':')[1].ToString();
                Int32.TryParse(streamreader.ReadLine().Split(':')[1].ToString(), out authentication);
                id = streamreader.ReadLine().Split(':')[1].ToString();
                password = streamreader.ReadLine().Split(':')[1].ToString();
                streamreader.Close();

                //check exist sqlite database
                if (!System.IO.File.Exists(_current_directory + @"\Databases\CheckOut.db"))
                    CreateSqliteDatabase();
                else
                {
                    System.IO.FileInfo file_info = new System.IO.FileInfo(_current_directory + @"\Databases\CheckOut.db");
                    if (file_info.Length == 0)
                        CreateSqliteDatabase();
                }

                //open connection
                OpenConnect();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Connect error 001: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string getSqlServerDataBaseName()
        {
            return database_name;
        }
        public static string getKeyDecrypt()
        {
            return key_decryp;
        }
        //CreateSqliteDatabase
        public static bool CreateSqliteDatabase()
        {
            try
            {
                if (!System.IO.Directory.Exists(_current_directory + @"\Databases"))
                    System.IO.Directory.CreateDirectory(_current_directory + @"\Databases");
                SQLiteConnection.CreateFile(_current_directory + @"\Databases\CheckOut.db");
                SQLiteConnection sqlite_connection = new SQLiteConnection(@"data source=Databases\CheckOut.db; version=3;");
                sqlite_connection.Open();
                sqlite_connection.ChangePassword(key_decryp);
                System.IO.StreamReader stream_reader = DecryptFile(_current_directory + @"\script1", key_decryp);
                string sql = stream_reader.ReadToEnd();
                stream_reader.Close();

                SQLiteCommand sqlite_command = new SQLiteCommand(sql, sqlite_connection);
                sqlite_command.ExecuteNonQuery();
                sqlite_connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Connect error 0011: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //check connect database
        public static string CheckConnectionDatabase(string _sqltype, string _server, string _id, string _password, int _authentication)
        {
            string result = "successfully";
            try
            {
                //Database type is sqlite
                if (_sqltype == "sqlite")
                {
                    SQLiteConnection sqlite_connection = new SQLiteConnection(@"Data source=Databases\\CheckOut.db; version=3; Password='" + key_decryp + "';");
                    sqlite_connection.Open();
                    sqlite_connection.Close();
                }

                //Database type is sqlserver
                else
                {
                    string str_connection_string = "";
                    if (_authentication == 0)
                        str_connection_string = "server = " + _server + "; database = " + database_name + "; Trusted_Connection = true;";
                    else
                        str_connection_string = "server = " + _server + "; database = " + database_name + "; user id = " + _id + "; password = " + _password + "; integrated security = false;";
                    _SQLConnect = str_connection_string;
                    SqlConnection sql_connection = new SqlConnection();
                    sql_connection.ConnectionString = str_connection_string;
                    sql_connection.Open();
                    sql_connection.Close();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                System.Windows.Forms.MessageBox.Show("Connect error 002: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }
        [System.Runtime.InteropServices.DllImport("sqlite3.dll")]
        public static extern int sqlite3_limit(IntPtr dbHandle, int id, int newVal);
        public static void SetLimit(SQLiteConnection conn, int newLimit)
        {
            try
            {
                object sqlVar = conn.GetType().GetProperty("_sql", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(conn);
                IntPtr dbHandle = (IntPtr)sqlVar.GetType().GetProperty("handle", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(sqlVar);
                sqlite3_limit(dbHandle, 4, newLimit);
                System.Diagnostics.Debug.WriteLine("===>>" + newLimit);
            }
            catch(NullReferenceException ex)
            {
                System.Diagnostics.Debug.WriteLine("===>>" + ex.Message);
            }
        }
        //open connect
        public static void OpenConnect()
        {
            try
            {
                //database type is sqlite
                if (sqltype == "sqlite")
                {
                    if (ConnectionDB.sqlite_con == null)
                    {
                        ConnectionDB.sqlite_con = new SQLiteConnection(@"Data source=Databases\\CheckOut.db; version=3; Password='" + key_decryp + "';");
                    }
                        
                    if (ConnectionDB.sqlite_con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectionDB.sqlite_con.Open();
                        //ConnectionDB.sqlite_con.ChangePassword("");
                        //ConnectionDB.sqlite_con.ChangePassword(key_decryp);
                        if (GetOnlyRow("SELECT name FROM sqlite_master WHERE type='table' AND name='tb_Temp';") == "")
                        {
                            System.IO.StreamReader stream_reader = DecryptFile(_current_directory + @"\script2", key_decryp);
                            string sql = stream_reader.ReadToEnd();
                            stream_reader.Close();

                            SQLiteCommand sqlite_command = new SQLiteCommand(sql, sqlite_con);
                            sqlite_command.ExecuteNonQuery();
                        }
                    }
                }

                //database type is sqlserver
                else
                {
                    if (ConnectionDB.sql_con == null)
                    {
                        string connection_string = "";
                        
                        //if authentication is windows
                        if (authentication == 0)
                            connection_string = "server = " + server + "; database = " + database_name + "; Trusted_Connection = true;";
                        //if authentication is sql server
                        else
                            connection_string = "server = " + server + "; database = " + database_name + "; user id = " + id + "; password = " + password + "; integrated security = false;";
                        ConnectionDB.sql_con = new SqlConnection();
                        ConnectionDB.sql_con.ConnectionString = connection_string;
                        _SQLConnect = connection_string;
                    }

                    if (ConnectionDB.sql_con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectionDB.sql_con.Open();

                        if(GetOnlyRow("SELECT * FROM CheckOut.INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tb_Temp';") == "")
                        {
                            System.IO.StreamReader stream_reader = DecryptFile(_current_directory + @"\script4", key_decryp);
                            string sql = stream_reader.ReadToEnd();
                            stream_reader.Close();

                            SqlCommand sql_command = new SqlCommand(sql, sql_con);
                            sql_command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Connect error 003: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //close connect
        public static void CloseConnect()
        {
            try
            {
                //database type is sqlite
                if (sqltype == "sqlite")
                {
                    if (ConnectionDB.sqlite_con != null)
                        if (ConnectionDB.sqlite_con.State == System.Data.ConnectionState.Open)
                            ConnectionDB.sqlite_con.Close();
                }

                //database type is sqlserver
                else
                {
                    if (ConnectionDB.sql_con != null)
                        if (ConnectionDB.sql_con.State == System.Data.ConnectionState.Open)
                            ConnectionDB.sql_con.Close();
                    _SQLConnect = string.Empty;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Connect error 004: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //execute non query
        public static int ExecuteNonQuery(string sql)
        {
            int result = 0;
            try
            {
                //database type is sqlite
                if (sqltype == "sqlite")
                {
                    SQLiteCommand sqlite_cm = new SQLiteCommand(sql, sqlite_con);
                    result = sqlite_cm.ExecuteNonQuery();
                }

                //database type is sqlserver
                else
                {
                    SqlCommand sql_cm = new SqlCommand(sql, sql_con);
                    result = sql_cm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Connect error 005: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        //get data
        private static SqlDataAdapter sql_da = new SqlDataAdapter();
        public static DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                //database type is sqlite
                if (sqltype == "sqlite")
                {
                    using (SQLiteDataAdapter sqlite_da = new SQLiteDataAdapter(sql, sqlite_con))
                    {
                        sqlite_da.Fill(dt);
                    }
                }
                //database type is sqlserver
                else
                {
                    using (SqlCommand sql_com = new SqlCommand(sql, sql_con))
                    {
                        sql_da.SelectCommand = sql_com;
                        sql_da.Fill(dt);
                    }
                }
            }
            catch (Exception)
            {
                //System.Windows.Forms.MessageBox.Show("Connect error 006: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        //get only row
        public static string GetOnlyRow(string sql)
        {
            string or = "";
            try
            {
                //database type is sqlite
                if (sqltype == "sqlite")
                {
                    using (SQLiteCommand sqlite_cm = new SQLiteCommand(sql, sqlite_con))
                    {
                        using (SQLiteDataReader sqlite_dr = sqlite_cm.ExecuteReader())
                        {
                            while (sqlite_dr.Read())
                            {
                                or = sqlite_dr[0].ToString();
                            }
                        }
                    }
                }

                //database type is sqlserver
                else
                {
                    using (SqlCommand sql_cm = new SqlCommand(sql, sql_con))
                    {
                        using (SqlDataReader sql_dr = sql_cm.ExecuteReader())
                        {
                            while (sql_dr.Read())
                            {
                                or = sql_dr[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //System.Windows.Forms.MessageBox.Show("Connect error 007: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
            return or;
        }

        //EncryptFile
        public static void EncryptFile(string inputfilename, string outputfilename, string key)
        {
            try
            {
                System.IO.FileStream filestream_input = new System.IO.FileStream(inputfilename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.FileStream filestream_encrypted = new System.IO.FileStream(outputfilename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                System.Security.Cryptography.DESCryptoServiceProvider desCtoSP = new System.Security.Cryptography.DESCryptoServiceProvider();
                desCtoSP.Key = ASCIIEncoding.ASCII.GetBytes(key);
                desCtoSP.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.Security.Cryptography.ICryptoTransform ICtoTf = desCtoSP.CreateEncryptor();
                System.Security.Cryptography.CryptoStream CtoS = new System.Security.Cryptography.CryptoStream(filestream_encrypted, ICtoTf, System.Security.Cryptography.CryptoStreamMode.Write);
                byte[] bytearrayinput = new byte[filestream_input.Length];
                filestream_input.Read(bytearrayinput, 0, bytearrayinput.Length);
                CtoS.Write(bytearrayinput, 0, bytearrayinput.Length);
                CtoS.Close();
                filestream_input.Close();
                System.IO.File.Delete(inputfilename);
                filestream_encrypted.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Connect error 008: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //DecryptFile
        public static System.IO.StreamReader DecryptFile(string inputfilename, string key)
        {
            System.IO.StreamReader streamreader = null;
            try
            {
                System.Security.Cryptography.DESCryptoServiceProvider DESCtoSP = new System.Security.Cryptography.DESCryptoServiceProvider();
                DESCtoSP.Key = ASCIIEncoding.ASCII.GetBytes(key);
                DESCtoSP.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.FileStream filestream = new System.IO.FileStream(inputfilename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.Security.Cryptography.ICryptoTransform ICtoTf = DESCtoSP.CreateDecryptor();
                System.Security.Cryptography.CryptoStream cryptostream = new System.Security.Cryptography.CryptoStream(filestream, ICtoTf, System.Security.Cryptography.CryptoStreamMode.Read);
                streamreader = new System.IO.StreamReader(cryptostream);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Windows.Forms.MessageBox.Show("Connect error 009: " + ex.Message, "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return streamreader;
        }

        //CheckDatabaseExists
        public static bool CheckDatabaseExists(string _server, string _id, string _password, int _authentication)
        {
            int database_id = 0;

            try
            {
                string connection_string = "";
                string sql = string.Format("select database_id from sys.databases where name = '{0}'", database_name);
                
                //if authentication is windows
                if(_authentication ==0)
                    connection_string = "server = " + _server + "; Trusted_Connection = true;";
                //if authencation is sql server
                else
                    connection_string = "server = " + _server + "; user id = " + _id + "; password = " + _password + "; integrated security = false;";

                SqlConnection sqlcon_check = new SqlConnection();
                sqlcon_check.ConnectionString = connection_string;
                _SQLConnect = connection_string;

                sqlcon_check.Open();

                using (sqlcon_check)
                {
                    using (SqlCommand sql_com = new SqlCommand(sql, sqlcon_check))
                    {
                        SqlDataReader sql_read = sql_com.ExecuteReader();
                        if (sql_read.HasRows)
                        {
                            while (sql_read.Read())
                            {
                                database_id = sql_read.GetInt32(0);
                                break;
                            }
                        }
                    }
                }

                sqlcon_check.Close();

                if (database_id > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //check connectiong server
        public static string CheckConnectionServer(string _server, string _id, string _password, int _authentication)
        {
            try
            {
                string connection_string = "";

                //if authentication is windows
                if (_authentication == 0)
                    connection_string = "server = " + _server + "; Trusted_Connection = true;";

                //if authentication is sql server
                else
                    connection_string = "server = " + _server + "; user id = " + _id + "; password = " + _password + "; integrated security = false;";

                SqlConnection sql_connection = new SqlConnection();
                sql_connection.ConnectionString = connection_string;
                sql_connection.Open();
                sql_connection.Close();
                _SQLConnect = connection_string;

                return "successfully";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections.";
            }
        }
    }
}
