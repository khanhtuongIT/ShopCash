using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterDAL;
using System.Data;

namespace CashierRegisterBUS
{
    public class Bus_tb_Statistic
    {
        private SQL_tb_Statistic sql_tb_statistic = new SQL_tb_Statistic();

        //GetStatisticData
        public DataTable GetStatisticData(string _keyWord, string _date)
        {
            return sql_tb_statistic.GetStatisticData(_keyWord, _date);
        }

        //GetStatisticData
        public DataTable GetStatisticData(string _keyWord, string _date, int _categoryID)
        {
            return sql_tb_statistic.GetStatisticData(_keyWord, _date, _categoryID);
        }

    }
}
