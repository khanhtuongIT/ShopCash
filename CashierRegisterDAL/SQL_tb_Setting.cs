using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CashierRegisterEntity;
using System.Globalization;

namespace CashierRegisterDAL
{
    public class SQL_tb_Setting
    {
        //insert Setting
        public int InsertSetting(EC_tb_Setting ec_tb_set, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "INSERT INTO [tb_Currency] ([Currency],[TaxRate],[Active],[Version]) VALUES('" + ec_tb_set.Currency + "'," + ec_tb_set.TaxRate.ToString(new CultureInfo("en-US")) + "," + ec_tb_set.Active + "," + ec_tb_set.Version + ")";
            else
                sql = "INSERT INTO [tb_Currency] ([Currency],[TaxRate],[Active],[Version]) VALUES( N'" + ec_tb_set.Currency + "', " + ec_tb_set.TaxRate.ToString(new CultureInfo("en-US")) + ", " + ec_tb_set.Active + ", " + ec_tb_set.Version + ")";

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update Setting
        public int UpdateSetting(EC_tb_Setting ec_tb_set, bool data_type)
        {
            string sql = "";
            if (!data_type)
                sql = "UPDATE [tb_Currency] SET [Currency] ='" + ec_tb_set.Currency + "', [TaxRate] = " + ec_tb_set.TaxRate.ToString(new CultureInfo("en-US")) + ", [Active]= " + ec_tb_set.Active + ", [Version] = " + ec_tb_set.Version +
                             " WHERE [SettingID] =" + ec_tb_set.SettingID;
            else
                sql = "UPDATE [tb_Currency] SET [Currency] = N'" + ec_tb_set.Currency + "', [TaxRate] = " + ec_tb_set.TaxRate.ToString(new CultureInfo("en-US")) + ", [Active] = " + ec_tb_set.Active + ", [Version] = " + ec_tb_set.Version +
                            " WHERE [SettingID] =" + ec_tb_set.SettingID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update Setting Active
        public int UpdateSettingActive()
        {
            string sql = "UPDATE [tb_Currency] SET [Active] = 0 WHERE [Active] = 1";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete Setting
        public int DeleteSetting(EC_tb_Setting ec_tb_set)
        {
            string sql = "DELETE FROM [tb_Currency] WHERE [SettingID] =" + ec_tb_set.SettingID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete all Setting
        public int DeleteAllSetting()
        {
            string sql = "DELETE FROM [tb_Currency]";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //get Setting
        public DataTable GetSetting(string con)
        {
            string sql = "SELECT * FROM [tb_Currency] " + con;
            return ConnectionDB.GetData(sql);
        }

        //GetMaxSettingID
        public int GetMaxSettingID(string con)
        {
            int max_setting_id = 0;
            Int32.TryParse(ConnectionDB.GetOnlyRow("select max(SettingID) from tb_Currency"), out max_setting_id);
            return max_setting_id;
        }
    }
}
