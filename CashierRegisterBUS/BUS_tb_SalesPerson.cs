using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CashierRegisterDAL;
using CashierRegisterEntity;

namespace CashierRegisterBUS
{
    public class BUS_tb_SalesPerson
    {
        SQL_tb_SalesPerson sql_tb_sp = new SQL_tb_SalesPerson();

        //inser SalesPerson
        public int InsertSalesPerson(EC_tb_SalesPerson ec_tb_sp, bool data_type)
        {
            return sql_tb_sp.InsertSalesperson(ec_tb_sp, data_type);
        }

        //update SalesPerson
        public int UpdateSalesPerson(EC_tb_SalesPerson ec_tb_sp, bool data_type)
        {
            return sql_tb_sp.UpdateSalesperson(ec_tb_sp, data_type);
        }

        //update salesperson active
        public void UpdateSalespersonActive(int active)
        {
            sql_tb_sp.UpdateSalespersonActive(active);
        }

        //delete SalesPerson
        public int DeleteSalesPerson(EC_tb_SalesPerson ec_tb_sp)
        {
            return sql_tb_sp.DeleteSalesperson(ec_tb_sp);
        }

        //delete all SalesPerson
        public int DeleteAllSalesPerson()
        {
            return sql_tb_sp.DeleteAllSalesperson();
        }

        //get dataTable
        public DataTable GetSalesPerson(string con)
        {
            DataTable dt = new DataTable();
            dt = sql_tb_sp.GetSalesperson(con);
            return dt;
        }

        //GetMaxSalespersonID
        public int GetMaxSalespersonID(string con)
        {
            return sql_tb_sp.GetMaxSalespersonID(con);
        }
    }
}
