using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CashierRegisterEntity;
using CashierRegisterDAL;

namespace CashierRegisterBUS
{
    public class BUS_tb_Setting
    {
        SQL_tb_Setting sql_tb_set = new SQL_tb_Setting();

        //insert Setting
        public int InsertSetting(EC_tb_Setting ec_tb_set, bool data_type)
        {
            return sql_tb_set.InsertSetting(ec_tb_set, data_type);
        }

        //update Setting
        public int UpdateSetting(EC_tb_Setting ec_tb_set, bool data_type)
        {
            return sql_tb_set.UpdateSetting(ec_tb_set, data_type);
        }

        //update Setting Active
        public int UpdateSettingActive()
        {
            return sql_tb_set.UpdateSettingActive();
        }

        //delete Setting
        public int DeleteSetting(EC_tb_Setting ec_tb_set)
        {
            return sql_tb_set.DeleteSetting(ec_tb_set);
        }

        //delete all Setting
        public int DeleteAllSetting()
        {
            return sql_tb_set.DeleteAllSetting();
        }


        //get Setting
        public DataTable GetSetting(string con)
        {
            return sql_tb_set.GetSetting(con);
        }

        //GetMaxSettingID
        public int GetMaxSettingID(string con)
        {
            return sql_tb_set.GetMaxSettingID(con);
        }
    }
}
