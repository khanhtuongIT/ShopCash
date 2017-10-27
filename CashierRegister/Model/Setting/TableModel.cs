using CashierRegister.Helpers;
using CashierRegisterDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegister.Model.Setting
{
    public class TableModel : ModelBase
    {
        private static bool _sqlType = StaticClass.GeneralClass.flag_database_type_general;

        private int _TableId;
        public int TableId
        {
            get { return _TableId; }
            set
            {
                _TableId = value;
                RaisePropertyChanged("TableId");
            }
        }
        private string _TableName = string.Empty;
        public string TableName
        {
            get { return _TableName; }
            set
            {
                _TableName = value;
                RaisePropertyChanged("TableName");
            }
        }
        private string _TableColumn = string.Empty;
        public string TableColumn
        {
            get { return _TableColumn; }
            set
            {
                _TableColumn = value;
                RaisePropertyChanged("TableColumn");
            }
        }
        private string _TableStructure = string.Empty;
        public string TableStructure
        {
            get { return _TableStructure; }
            set
            {
                _TableStructure = value;
                RaisePropertyChanged("TableStructure");
            }
        }
        private bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }
        private string _showTblName = string.Empty;
        public string ShowTableName
        {
            get { return _showTblName; }
            set
            {
                _showTblName = value;
                RaisePropertyChanged("ShowTableName");
            }
        }
        public static DataTable getListTables()
        {
            string sql = string.Empty;
            if (!_sqlType)
                sql = "SELECT tbl_name as TblName, sql as StrCreated FROM sqlite_master where type='table' and name like 'tb_%'";
            else sql = "SELECT TABLE_NAME as TblName, TABLE_TYPE as StrCreated FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = N'BASE TABLE' and TABLE_NAME <> 'sysdiagrams'";
            return ConnectionDB.GetData(sql);
        }
        public static DataTable getColumnsByTable(string _tblName)
        {
            string sql = string.Empty;
            if (!_sqlType)
                sql = "PRAGMA table_info("+ _tblName + ")";
            else sql = "select COLUMN_NAME as name, DATA_TYPE as type from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + _tblName + "'";
            return ConnectionDB.GetData(sql);
        }
        public static DataTable getAllDataTable(string _tblName)
        {
            string sql = "SELECT * FROM " + _tblName;
            return ConnectionDB.GetData(sql);
        }
        public static DataTable getListTablesRecover(string _prefix)
        {
            string sql = string.Empty;
            if (!_sqlType)
                sql = "SELECT tbl_name as TblName, sql as StrCreated FROM sqlite_master where type='table' and name like '" + _prefix + "%'";
            else sql = "SELECT TABLE_NAME as TblName, TABLE_TYPE as StrCreated FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = N'BASE TABLE' and TABLE_NAME like '" + _prefix + "%'";
            System.Diagnostics.Debug.WriteLine("SQL = "+sql);
            return ConnectionDB.GetData(sql);
        }
        public static DataTable getNewTableList(List<string> _lstOldTbl)
        {
            if (_lstOldTbl.Count == 0) return null;
            string sql = string.Empty;
            if (!_sqlType)
                sql = "SELECT tbl_name as TblName, sql as StrCreated FROM sqlite_master where type='table' and name like 'tb_%' and TABLE_NAME in ('" + string.Join(",", _lstOldTbl.ToArray()).Replace(",","','") + "')";
            else sql = "SELECT TABLE_NAME as TblName, TABLE_TYPE as StrCreated FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = N'BASE TABLE' and TABLE_NAME <> 'sysdiagrams' and TABLE_NAME not in ('" + string.Join(",", _lstOldTbl.ToArray()).Replace(",", "','") + "')";
            System.Diagnostics.Debug.WriteLine(sql);
            return ConnectionDB.GetData(sql);
        }
        public static DataTable getDataProductToExport()
        {
            string sql = "SELECT CategoryName,ShortName,LongName,Cost,Price,Tax,InventoryCount,BarcodeID FROM tb_Product as p inner join tb_Category as c on p.CategoryID = c.CategoryID";
            return ConnectionDB.GetData(sql);
        }
        public static int getCatIdByName(string _name)  //Using for import data
        {
            int _catId = 0;
            if (!_sqlType)
                Int32.TryParse(ConnectionDB.GetOnlyRow("select CategoryID from tb_Category where lower(CategoryName) = '" + _name.ToLower() + "' limit 1"), out _catId);
            else Int32.TryParse(ConnectionDB.GetOnlyRow("select top 1 CategoryID from tb_Category where lower(CategoryName) = '" + _name.ToLower() + "'"), out _catId);
            return _catId;
        }
        public static int updatePasswordForUserPassIsNull(string _pass)
        {
            string _sql = string.Format("update tb_User set Password = '{0}' where Password IS NULL or Password=''", _pass);
            return ConnectionDB.ExecuteNonQuery(_sql);
        }
    }
}
