using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterEntity;
using CashierRegisterDAL;
using System.Data;

namespace CashierRegisterBUS
{
    public class BUS_tb_Order
    {
        SQL_tb_Order sql_tb_ord = new SQL_tb_Order();

        //insert Order
        public int InsertOrder(EC_tb_Order ec_tb_ord, bool data_type)
        {
            return sql_tb_ord.InsertOrder(ec_tb_ord, data_type);
        }

        //update Order
        public int UpdateOrder(EC_tb_Order ec_tb_ord, bool data_type)
        {
            return sql_tb_ord.UpdateOrder(ec_tb_ord, data_type);
        }

        //delete Order
        public int DeleteOrder(EC_tb_Order ec_tb_ord)
        {
            return sql_tb_ord.DeleteOrder(ec_tb_ord);
        }

        //delete all Order
        public int DeleteAllOrder()
        {
            return sql_tb_ord.DeleteAllOrder();
        }

        //get Order
        public DataTable GetOrder(string con)
        {
            return sql_tb_ord.GetOrder(con);
        }

        //GetOrderPrint
        public DataTable GetOrderPrint(string sql)
        {
            return sql_tb_ord.GetOrderPrint(sql);
        }

        //get sum order
        public int GetSumOrder(string con)
        {
            int sum_order = 0;
            sum_order = sql_tb_ord.GetSumOrder(con);
            return sum_order;
        }

        //get order follow paging
        public DataTable GetOrderFollowPaging(int be_limit, int af_limit, string con1, string con2, bool sql_database_type)
        {
            DataTable tb = new DataTable();
            tb = sql_tb_ord.GetOrderFollowPaging(be_limit, af_limit, con1, con2, sql_database_type);
            return tb;
        }

        //get year from tb_Order
        DataTable tb_year = new DataTable();
        public DataTable GetYearFromOrder(string condition, bool sql_database_type)
        {
            tb_year = sql_tb_ord.GetYearFromOrder(condition, sql_database_type);
            return tb_year;
        }

        //GetMaxOrderID
        public int GetMaxOrderID(string con)
        {
            return sql_tb_ord.GetMaxOrderID(con);
        }
    }
}
