using System;
using Microsoft.Win32;
using System.Security.Cryptography;
using FirstFloor.ModernUI.Windows.Controls;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CashierRegisterEntity;
using System.Data;
using CashierRegisterBUS;
using System.Globalization;

namespace CashierRegister.StaticClass
{
    public static class GeneralClass
    {
        //activation func
        public static bool isPreActiveVersion = true;
        public static bool isFullVersion = false;
        public static bool register_request = false;
        public static bool help_request = false;

        //database func
        public static bool flag_database_type_general = false;

        //language func
        public static System.Windows.ResourceDictionary dict_language_current = new System.Windows.ResourceDictionary();

        //help func
        public static string current_page_active = "";

        //accentcolor func
        public static bool mousedown_logodata = false;
        public static bool accent_color_change = false;

        //size main func
        public static double height_taskbar_general = 0;
        public static double height_screen_working_general = 0;
        public static double width_screen_general = 0;
        public static bool flag_change_out_of_stock = false;

        //barcode func
        public static bool flag_barcode = false;
        public static string pathGiftCardsImg = "images/GiftCards";

        //login func
        public enum UserPermission : int { admin = 1, inventory = 2, report = 3, user = 4, seller = 5 };
        public static int user_permission = (int)UserPermission.user;
        public static int id_user_general = 0;
        public static string name_user_general = "";
        public static string password_user_general = "";

        //pay money func
        public static decimal subtotal_general = 0;

        public static int discounttype_general = 0;
        public static decimal discount_general = 0;

        public static int discountdetailtype_general = 0;
        public static decimal discountdetail_general = 0;

        public static decimal totaltaxrate_general = 0;
        public static decimal totaldiscount_general = 0;

        public static bool flag_paycash = false;
        public static bool flag_payother = false;

        //setting func
        public static bool flag_add_edit_setting_general = false;
        public static int settingid_setting_general = 0;
        public static string currency_setting_general = "";
        public static decimal taxrate_setting_general = 0;
        public static int active_setting_general = 0;
        public static int version_setting_general = 0;
        public static List<EC_tb_Setting> list_ec_tb_setting_general = new List<EC_tb_Setting>();
        public static List<int> lstCash = new List<int>();
        public static System.Collections.Hashtable app_settings = new System.Collections.Hashtable();
        public static System.Collections.Hashtable dateFromatSettings = new System.Collections.Hashtable();
        public static System.Collections.Hashtable timeFromatSettings = new System.Collections.Hashtable();

        //product func
        public static bool flag_add_edit_delete_product_general = false;
        public static int productid_general = 0;
        public static string productpathimage_general = "";
        public static List<EC_tb_Product> list_ec_tb_product_general = new List<EC_tb_Product>();

        //category func
        public static bool flag_add_edit_delete_category_general = false;
        public static int categoryid_general = 0;
        public static string categoryname_general = "";

        //salesperson func
        public static bool flag_add_edit_delete_salesperson_general = false;
        public static List<EC_tb_SalesPerson> list_ec_tb_salesperson_general = new List<EC_tb_SalesPerson>();
        public static int salespersonid_login_general = 0;
        public static string salespersonname_login_general = "None";
        public static string salespersonpassword_general = "";
        public static bool flag_salespersonlogin_general = false;
        public static bool flag_added_customer_general = false;

        //customer func
        public static bool flag_add_edit_delete_customer_general = false;
        public static int customerid_general = 0;
        public static string customername_general = "None";
        public static string customeremail_general = "None";
        public static string customerphone_general = "None";
        public static List<EC_tb_Customer> list_ec_tb_customer_general = new List<EC_tb_Customer>();
        public static List<Int32> customerGiftCard = new List<Int32>();
        public static List<decimal> customerGiftValue = new List<decimal>();

        //order func
        public static List<EC_tb_Order> list_ec_tb_order_general = new List<EC_tb_Order>();
        public static string condition_report_order = "";
        public static bool flag_check_delete_order_orderdetail = false;

        //orderdetail func
        public static int dtgorderdetail_selectedindex_general = 0;
        public static int dtgorderdetail_productid_general = 0;
        public static int orderdetailid_general = 0;
        public static int orderdetailqty_general = 0;
        public static List<EC_tb_OrderDetail> list_ec_tb_orderdetail_general = new List<EC_tb_OrderDetail>();

        public static double locationx_muibtnCustomer;
        public static double locationy_muibtnCustomer;

        //backup database func
        public static int id_database_backup_general;
        public static string id_database_general = "";
        public static string download_url_general = "";
        public static string client_id = "503107092807-4jieh8srj3ul3d08ivgamdtfh4ieasuf.apps.googleusercontent.com";
        public static string client_secret = "4r4jINB--kGudGPw-kcQH6va";

        //ExportSQL func
        public static bool ExportSQL(string backup_position)
        {
            bool flag = false;
            try
            {
                //using for table user
                BUS_tb_User bus_tb_user = new BUS_tb_User();

                //using for table setting
                BUS_tb_Setting bus_tb_setting = new BUS_tb_Setting();

                //using for table category
                BUS_tb_Category bus_tb_category = new BUS_tb_Category();

                //using for table customer
                BUS_tb_Customer bus_tb_customer = new BUS_tb_Customer();

                //using for table inputhistory
                BUS_tb_InputHistory bus_tb_inputdistory = new BUS_tb_InputHistory();

                //using for table order
                BUS_tb_Order bus_tb_order = new BUS_tb_Order();

                //using for table orderdetail
                BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

                //using for table payment
                BUS_tb_Payment bus_tb_payment = new BUS_tb_Payment();

                //using for table product
                BUS_tb_Product bus_tb_product = new BUS_tb_Product();

                //using for table salesperson
                BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();

                using (System.IO.StreamWriter stream_writer = new System.IO.StreamWriter(backup_position))
                {
                    stream_writer.WriteLine("use CheckOut;");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");

                    //export table user
                    stream_writer.WriteLine("-- table dbo.tb_User");
                    //reset auto increment
                    //stream_writer.WriteLine("DBCC CHECKIDENT(tb_User, RESEED, 0);");
                    //delete data from table user
                    stream_writer.WriteLine("delete from [tb_User];");
                    stream_writer.WriteLine("");
                    DataTable tb_user = new DataTable();
                    tb_user = bus_tb_user.GetUser("");
                    foreach (DataRow dr in tb_user.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_User] ([ID],[Name],[Email],[Address],[Password],[Question],[Answer]) VALUES (" + dr["ID"].ToString() + ",'" + dr["Name"].ToString() + "','" + dr["Email"].ToString() + "','" + dr["Address"].ToString() + "','" + dr["Password"].ToString() + "','" + dr["Question"].ToString() + "','" + dr["Answer"].ToString() + "');");
                    }

                    //export table setting
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("-- table dbo.tb_Setting");
                    //delete data from table Setting
                    stream_writer.WriteLine("delete from [tb_Setting];");
                    stream_writer.WriteLine("");
                    DataTable tb_setting = new DataTable();
                    tb_setting = bus_tb_setting.GetSetting("");
                    foreach (DataRow dr in tb_setting.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_Setting] ([SettingID],[Currency],[TaxRate],[Active],[Version]) VALUES (" + dr["SettingID"].ToString() + ",'" + dr["Currency"].ToString() + "'," + dr["TaxRate"].ToString() + "," + dr["Active"].ToString() + "," + dr["Version"].ToString() + ");");
                    }

                    //export table category
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("-- table dbo.tb_Category");
                    //delete data from table category
                    stream_writer.WriteLine("delete from [tb_Category];");
                    stream_writer.WriteLine("");
                    DataTable tb_category = new DataTable();
                    tb_category = bus_tb_category.GetCatagorySetting("");
                    foreach (DataRow dr in tb_category.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_Category] ([CategoryID],[CategoryName]) VALUES (" + dr["CategoryID"].ToString() + ",'" + dr["CategoryName"].ToString() + "');");
                    }

                    //export table customer
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("-- table dbo.tb_Customer");
                    //delete data from table customer
                    stream_writer.WriteLine("delete from [tb_Customer];");
                    stream_writer.WriteLine("");
                    DataTable tb_customer = new DataTable();
                    tb_customer = bus_tb_customer.GetCustomer("");
                    foreach (DataRow dr in tb_customer.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_Customer] ([CustomerID],[FirstName],[LastName],[Address1],[Address2],[City],[State],[Zipcode],[Phone],[Email]) VALUES (" + dr["CustomerID"].ToString() + ",'" + dr["FirstName"].ToString() + "','" + dr["LastName"].ToString() + "','" + dr["Address1"].ToString() + "','" + dr["Address2"].ToString() + "','" + dr["City"].ToString() + "','" + dr["State"].ToString() + "','" + dr["Zipcode"].ToString() + "','" + dr["Phone"].ToString() + "','" + dr["Email"].ToString() + "');");
                    }

                    //export table inputhistory
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("-- table dbo.tb_InputHistory");
                    //delete data from table inputhistory
                    stream_writer.WriteLine("delete from [tb_InputHistory];");
                    stream_writer.WriteLine("");
                    DataTable tb_inputhistory = new DataTable();
                    tb_inputhistory = bus_tb_inputdistory.GetAllInputHistory("");
                    foreach (DataRow dr in tb_inputhistory.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_InputHistory] ([InputID],[ProductID],[ProductName],[InputDate],[UserID],[UserName],[Cost],[Price],[InventoryCount],[CategoryID],[CategoryName],[Tax],[Active],[Country],[Size_Weight]) VALUES (" + dr["InputID"].ToString() + "," + dr["ProductID"].ToString() + ",'" + dr["ProductName"].ToString() + "','" + dr["InputDate"].ToString() + "'," + dr["UserID"].ToString() + ",'" + dr["UserName"].ToString() + "'," + dr["Cost"].ToString() + "," + dr["Price"].ToString() + "," + dr["InventoryCount"].ToString() + "," + dr["CategoryID"].ToString() + ",'" + dr["CategoryName"].ToString() + "'," + dr["Tax"].ToString() + "," + dr["Active"].ToString() + ",'" + dr["Country"].ToString() + "','" + dr["Size_Weight"].ToString() + "');");
                    }

                    //export table order 
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("-- table dbo.tb_Order");
                    //delete data from table order
                    stream_writer.WriteLine("delete from [tb_Order];");
                    stream_writer.WriteLine("");
                    DataTable tb_order = new DataTable();
                    tb_order = bus_tb_order.GetOrder("");
                    foreach (DataRow dr in tb_order.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_Order] ([OrderID],[CustomerID],[CustomerName],[Quantity],[OrderDate],[SalespersonID],[SalespersonName],[PaymentID],[PaymentName],[DiscountType],[Discount],[TotalDiscount],[TotalTax],[TotalAmount]) VALUES (" + dr["OrderID"].ToString() + "," + dr["CustomerID"].ToString() + ",'" + dr["CustomerName"].ToString() + "', " + dr["Quantity"].ToString() + ",'" + dr["OrderDate"].ToString() + "', " + dr["SalespersonID"].ToString() + ", '" + dr["SalespersonName"].ToString() + "', " + dr["PaymentID"].ToString() + ",'" + dr["PaymentName"].ToString() + "', " + dr["DiscountType"].ToString() + "," + dr["Discount"].ToString() + "," + dr["TotalDiscount"].ToString() + "," + dr["TotalTax"].ToString() + "," + dr["TotalAmount"].ToString() + ");");
                    }

                    //export table orderdetail
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("-- table dbo.tb_OrderDetail");
                    //delete data from table orderdetail
                    stream_writer.WriteLine("delete from [tb_OrderDetail];");
                    stream_writer.WriteLine("");
                    DataTable tb_orderdetail = new DataTable();
                    tb_orderdetail = bus_tb_orderdetail.GetOrderDetail("");
                    foreach (DataRow dr in tb_orderdetail.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_OrderDetail] ([ID],[CategoryID],[CategoryName],[ProductID],[ProductName],[Cost],[Price],[Qty],[Tax],[DiscountType],[Discount],[TotalDiscount],[Total],[OrderID]) VALUES (" + dr["ID"].ToString() + "," + dr["CategoryID"].ToString() + ",'" + dr["CategoryName"].ToString() + "'," + dr["ProductID"].ToString() + ", '" + dr["ProductName"].ToString() + "'," + dr["Cost"].ToString() + "," + dr["Price"].ToString() + "," + dr["Qty"].ToString() + "," + dr["Tax"].ToString() + "," + dr["DiscountType"].ToString() + "," + dr["Discount"].ToString() + "," + dr["TotalDiscount"].ToString() + "," + dr["Total"].ToString() + "," + dr["OrderID"].ToString() + ");");
                    }

                    //export table payment
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("-- table dbo.tb_Payment");
                    //delete data from table payment
                    stream_writer.WriteLine("delete from [tb_Payment];");
                    stream_writer.WriteLine("");
                    DataTable tb_payment = new DataTable();
                    tb_payment = bus_tb_payment.GetPayment("");
                    foreach (DataRow dr in tb_payment.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_Payment] ([PaymentID],[Card]) VALUES (" + dr["PaymentID"].ToString() + ",'" + dr["Card"].ToString() + "');");
                    }

                    //export table product str
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("-- table dbo.tb_Product");
                    //delete data from table product
                    stream_writer.WriteLine("delete from [tb_Product];");
                    stream_writer.WriteLine("");
                    DataTable tb_product = new DataTable();
                    tb_product = bus_tb_product.GetProduct("");
                    foreach (DataRow dr in tb_product.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_Product] ([ProductID],[BarcodeID],[ShortName],[LongName],[Cost],[Price],[InventoryCount],[CategoryID],[Tax],[PathImage],[Capture],[Active],[Country],[Size_Weight]) VALUES (" + dr["ProductID"].ToString() + ",'" + dr["BarcodeID"].ToString() + "','" + HandlingSpecialCharacter(dr["ShortName"].ToString()) + "','" + HandlingSpecialCharacter(dr["LongName"].ToString()) + "'," + dr["Cost"].ToString() + "," + dr["Price"].ToString() + "," + dr["InventoryCount"].ToString() + "," + dr["CategoryID"].ToString() + "," + dr["Tax"].ToString() + ",'" + dr["PathImage"].ToString() + "'," + dr["Capture"].ToString() + "," + dr["Active"].ToString() + ",'" + dr["Country"].ToString() + "','" + dr["Size_Weight"].ToString() + "');");
                    }

                    //export table salesperson
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("");
                    stream_writer.WriteLine("-- table dbo.tb_Salesperson");
                    //delete data from table salesperson
                    stream_writer.WriteLine("delete from [tb_Salesperson];");
                    stream_writer.WriteLine("");
                    DataTable tb_salesperson = new DataTable();
                    tb_salesperson = bus_tb_salesperson.GetSalesPerson("");
                    foreach (DataRow dr in tb_salesperson.Rows)
                    {
                        stream_writer.WriteLine("INSERT INTO [tb_Salesperson] ([SalespersonID],[Name],[Birthday],[Address],[Email],[Password],[Active],[Default]) VALUES (" + dr["SalespersonID"].ToString() + ",'" + dr["Name"].ToString() + "', '" + dr["Birthday"].ToString() + "', '" + dr["Address"].ToString() + "', '" + dr["Email"].ToString() + "', '" + dr["Password"].ToString() + "', " + dr["Active"].ToString() + "," + dr["Default"].ToString() + ");");
                    }
                    stream_writer.Close();
                    flag = true;
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        //location func
        public static System.Windows.Point ElementPointToScreenPoint(System.Windows.UIElement element, System.Windows.Point pointoscreen)
        {
            return element.PointToScreen(pointoscreen);
        }

        //GetNumFormat func
        public static string GetNumFormatDisplay(decimal _number)
        {
            #region
            //string value = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", num);
            //if (Properties.Settings.Default.decimalSeparator == 1)
            //    value = value.Replace(".", "*").Replace(",", ".").Replace("*", ",");
            #endregion

            NumberFormatInfo nfi = null;
            if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0")
                nfi = new CultureInfo("en-US", false).NumberFormat;
            else
            {
                nfi = new CultureInfo("fr-FR", false).NumberFormat;
                nfi.NumberGroupSeparator = ".";
            }
            nfi.NumberDecimalDigits = 2;
            return _number.ToString("N", nfi);
        }

        //GetNumFormat func
        public static string GetNumFormatEdit(decimal _number)
        {
            NumberFormatInfo nfi = null;
            if(StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0")
                nfi = new CultureInfo("en-US", false).NumberFormat;
            else
            {
                nfi = new CultureInfo("fr-FR").NumberFormat;
            }
            nfi.NumberGroupSeparator = "";
            nfi.NumberDecimalDigits = 2;

            #region
            //string value = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.00}", num);
            //if (Properties.Settings.Default.decimalSeparator == 1)
            //    value = value.Replace(".", ",");
            #endregion
            return _number.ToString("N", nfi);
        }

        public static int software_version = 1;
        public static bool flag_registered_general = false;
        public static string youremail_registered_general = "";
        public static string key_register_general = "C@$$&O??";
        public static string password_db_general = "Ch3ck0nt##$$!!";

        //using in regedit
        public static string userroot_register_general = "HKEY_CURRENT_USER";
        public static string subkey_register_general = "RegistryCash";
        public static string keyname_register_general = userroot_register_general + @"\" + subkey_register_general;
        public static string valuename_register_general = "ActiveStatus";

        public static string HandlingSpecialCharacter(string _string)
        {
            string str = "";
            int str_length = _string.Length;
            if (str_length > 0)
            {
                for (int i = 0; i < str_length; i++)
                {
                    if (_string.Substring(i, 1) == "'")
                        str += "''";
                    else
                        str += _string.Substring(i, 1);
                }
            }
            return str;
        }

        //hash a string to md5 func
        public static string MD5Hash(string _string)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //compute hash from the byte of string
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_string));
            //get hash result from compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //chang it into 2 hexadecimal digits for each byet
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        //Encrypt and Decrypt file func
        public static void EncryptFileGD(string inputfilename, string outputfilename, string key)
        {
            try
            {
                FileStream filestream_input = new FileStream(inputfilename, FileMode.Open, FileAccess.Read);
                FileStream filestream_encrypted = new FileStream(outputfilename, FileMode.Create, FileAccess.Write);
                DESCryptoServiceProvider desCtoSP = new DESCryptoServiceProvider();
                desCtoSP.Key = ASCIIEncoding.ASCII.GetBytes(key);
                desCtoSP.IV = ASCIIEncoding.ASCII.GetBytes(key);
                ICryptoTransform ICtoTf = desCtoSP.CreateEncryptor();
                CryptoStream CtoS = new CryptoStream(filestream_encrypted, ICtoTf, CryptoStreamMode.Write);
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
                System.Windows.Forms.MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static StreamReader DecryptFileGD(string inputfilename, string key)
        {
            StreamReader streamreader = null;
            try
            {
                DESCryptoServiceProvider DESCtoSP = new DESCryptoServiceProvider();
                DESCtoSP.Key = ASCIIEncoding.ASCII.GetBytes(key);
                DESCtoSP.IV = ASCIIEncoding.ASCII.GetBytes(key);
                FileStream filestream = new FileStream(inputfilename, FileMode.Open, FileAccess.Read);
                ICryptoTransform ICtoTf = DESCtoSP.CreateDecryptor();
                CryptoStream cryptostream = new CryptoStream(filestream, ICtoTf, CryptoStreamMode.Read);
                streamreader = new StreamReader(cryptostream, Encoding.UTF8);
            }
            catch (FileNotFoundException) { }
            return streamreader;
        }

        //save logs login/logout func
        public static void SaveLogs(string user, string action, DateTime time)
        {
            try
            {
                string str = "";
                if (File.Exists("Logs"))
                {
                    var stream = DecryptFileGD("Logs", key_register_general);
                    str = stream.ReadToEnd();
                    stream.Close();
                    File.Delete("Logs");
                }

                StreamWriter stream_writer = new StreamWriter("Log", true);
                stream_writer.WriteLine(str);
                stream_writer.WriteLine(user + "==========> " + action + "==========> " + time.ToString());
                stream_writer.Flush();
                stream_writer.Close();
                EncryptFileGD("Log", "Logs", key_register_general);
            }
            catch(Exception)
            {
                if (File.Exists("Log"))
                    File.Delete("Log");

                if (File.Exists("Logs"))
                    File.Delete("Logs");
            }
        }
        public static decimal ConverStringToDecimal(string _strval)
        {
            if (string.IsNullOrEmpty(_strval)) return 0;
            if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0")
            {
                _strval = _strval.Replace(",", "");
            }
            else
            {
                _strval = _strval.Replace(".", "");
                _strval = _strval.Replace(",", ".");
            }
            return Convert.ToDecimal(_strval);
        }
        public static string FormatNumberDisplay(decimal _number, int _length)
        {
            #region
            //string value = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", num);
            //if (Properties.Settings.Default.decimalSeparator == 1)
            //    value = value.Replace(".", "*").Replace(",", ".").Replace("*", ",");
            #endregion

            NumberFormatInfo nfi = null;
            if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0")
                nfi = new CultureInfo("en-US", false).NumberFormat;
            else
            {
                nfi = new CultureInfo("fr-FR", false).NumberFormat;
                nfi.NumberGroupSeparator = ".";
            }
            nfi.NumberDecimalDigits = _length;
            return _number.ToString("N", nfi);
        }
        /*
        public static Boolean BackupSQLServer(System.Data.SqlClient.SqlConnection _sqlConnect, string _databaseName, string _pathToFile)
        {
            bool _rs = true;
            if (_sqlConnect == null || string.IsNullOrEmpty(_databaseName) || string.IsNullOrEmpty(_pathToFile))
            {
                return false;
            }
            var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}' WITH NOFORMAT, NOINIT,  NAME = N'data-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", _databaseName, @_pathToFile);
            try
            {
                using (var command = new System.Data.SqlClient.SqlCommand(query, _sqlConnect))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                _rs = false;
            }
            return _rs;
        }
        public static Boolean RestoreSQLServer(System.Data.SqlClient.SqlConnection _sqlConnect, string _databaseName, string _pathFromFile)
        {
            bool _rs = true;
            if(_sqlConnect==null || string.IsNullOrEmpty(_databaseName) || string.IsNullOrEmpty(_pathFromFile))
            {
                return false;
            }
            using (var command = _sqlConnect.CreateCommand())
            {
                try
                {
                    command.CommandText = "Use Master";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER DATABASE " + _databaseName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                    command.ExecuteNonQuery();
                    command.CommandText = "RESTORE DATABASE " + _databaseName + " FROM DISK = '" + _pathFromFile + "' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10; ";
                    command.ExecuteNonQuery();
                    command.CommandText = "Use Master";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER DATABASE " + _databaseName + " SET MULTI_USER;";
                    command.ExecuteNonQuery();
                    command.CommandText = "Use " + _databaseName;
                    command.ExecuteNonQuery();
                }
                catch
                {
                    _rs = false;
                }
            }
            return _rs;
        }
        public static Boolean BackupSQLite(System.Data.SQLite.SQLiteConnection sqlConnect, string pathDestination)
        {
            bool _rs = true;
            if (sqlConnect == null || string.IsNullOrEmpty(pathDestination))
            {
                return false;
            }
            using (sqlConnect)
            {
                sqlConnect.Open();
                using (var _destination = new System.Data.SQLite.SQLiteConnection(@"Data Source=" + pathDestination + @"; Version=3;"))
                {
                    try
                    {
                        _destination.Open();
                        sqlConnect.BackupDatabase(_destination, "main", "main", -1, null, 0);
                        _destination.Close();
                    }
                    catch
                    {
                        _rs = false;
                    }
                }
                sqlConnect.Close();
            }
            return _rs;
        }
        public static Boolean RestoreSQLite(string pathSource, string pathDestination)
        {
            bool _rs = true;
            if (string.IsNullOrEmpty(pathSource) || string.IsNullOrEmpty(pathDestination))
            {
                return false;
            }
            try
            {
                CashierRegisterDAL.ConnectionDB.CloseConnect();
                File.Copy(pathSource, pathDestination, true);
                CashierRegisterDAL.ConnectionDB.OpenConnect();
            }
            catch
            {
                _rs = false;
            }
            return _rs;
        }
        public static Boolean RestoreSQLFromSQLFile(System.IO.StreamReader StreamReader, System.Data.SqlClient.SqlConnection _sqlConnect)
        {
            bool _rs = true;
            string _line;
            System.Text.StringBuilder _strImport = new System.Text.StringBuilder();
            while ((_line = StreamReader.ReadLine()) != null)
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
                System.Data.SqlClient.SqlTransaction tr = null;
                System.Data.SqlClient.SqlCommand sql_command = null;
                try
                {
                    using (tr = _sqlConnect.BeginTransaction())
                    {
                        using (sql_command = _sqlConnect.CreateCommand())
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
                    _rs = false;
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
            return _rs;
        }*/
    }

}
