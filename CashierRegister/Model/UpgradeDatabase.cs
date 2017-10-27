using CashierRegisterDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegister.Model
{
    public class UpgradeDatabase
    {
        private static bool _sqlType = StaticClass.GeneralClass.flag_database_type_general;
        public static DataTable getAppSetting()
        {
            string sql = "select SettingKey, SettingValue from tb_Setting";
            return ConnectionDB.GetData(sql);
        }
        public static int updateAppSetting(System.Collections.Hashtable _hashTable)
        {
            string sql = "update tb_Setting set SettingValue = case";
            foreach (System.Collections.DictionaryEntry de in _hashTable)
            {
                sql += (!_sqlType) ? " when SettingKey = '" + de.Key + "' then '" + _hashTable[de.Key].ToString()+"'" : " when SettingKey = '" + de.Key + "' then N'" + _hashTable[de.Key].ToString() + "'" ;
            }
            sql += " ELSE SettingValue END";
            return ConnectionDB.ExecuteNonQuery(sql);
        }
    }
}
