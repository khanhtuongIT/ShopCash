using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterDAL;
using CashierRegisterEntity;
using System.Data;

namespace CashierRegisterBUS
{
    public class BUS_tb_OrderDetail
    {
        SQL_tb_OrderDetail sql_tb_od = new SQL_tb_OrderDetail();

        //insert OrderDetail
        public int InsertOrderDetail(EC_tb_OrderDetail ec_tb_od, bool data_type)
        {
            return sql_tb_od.InsertOrderDetail(ec_tb_od, data_type);
        }

        //update OrderDetail
        public int UpdateOrderDetail(EC_tb_OrderDetail ec_tb_od, bool data_type)
        {
            return sql_tb_od.UpdateOrderDetail(ec_tb_od, data_type);
        }

        //delete OrderDetail
        public int DeleteOrderDetail(EC_tb_OrderDetail ec_tb_od)
        {
            return sql_tb_od.DeleteOrderDetail(ec_tb_od);
        }

        //delete all OrderDetail
        public int DeleteAllOrderDetail(string condition)
        {
            return sql_tb_od.DeleteAllOrderDetail(condition);
        }

        //get OrderDetail
        public DataTable GetOrderDetail(string con)
        {
            return sql_tb_od.getOrderDetail(con);
        }

        //GetQtyProductOutput
        public int GetQtyProductOutput(string con)
        {
            int qty_productoutput = 0;
            qty_productoutput = sql_tb_od.GetQtyProductOutput(con);
            return qty_productoutput;
        }

        //GetQuatityOrderDetail
        public DataTable GetQuatityOrderDetail(string condition)
        {
            return sql_tb_od.GetQuatityOrderDetail(condition);
        }

        //GetTotalRevenueAll
        public DataTable GetTotalRevenueAll(string condition, bool sql_database_type)
        {
            DataTable tb_revenue_all = new DataTable();
            tb_revenue_all = sql_tb_od.GetTotalRevenueAll(condition, sql_database_type);
            return tb_revenue_all;
        }

        //GetTotalCapitalAll
        public DataTable GetTotalCapitalAll(string condition, bool sql_database_type)
        {
            DataTable tb_capital_all = new DataTable();
            tb_capital_all = sql_tb_od.GetTotalCapitalAll(condition, sql_database_type);
            return tb_capital_all;
        }

        //GetTotalRevenueYear
        public DataTable GetTotalRevenueYear(string _str_year, string condition, bool sql_database_type)
        {
            return sql_tb_od.GetTotalRevenueYear(_str_year, condition, sql_database_type);
        }

        //GetTotalCapitalYear
        public DataTable GetTotalCapitalYear(string _str_year, string condition, bool sql_database_type)
        {
            return sql_tb_od.GetTotalCapitalYear(_str_year, condition, sql_database_type);
        }

        //CheckExist
        public string CheckExist(string condition)
        {
            return sql_tb_od.CheckExist(condition);
        }

        //GetMaxID
        public int GetMaxID(string con)
        {
            return sql_tb_od.GetMaxID(con);
        }
    }
}
