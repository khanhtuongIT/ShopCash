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
    public class BUS_tb_Customer
    {
        SQL_tb_Customer sql_tb_cus = new SQL_tb_Customer();

        //insert Customer
        public int InsertCustomer(EC_tb_Customer ec_tb_cus, bool data_type)
        {
            return sql_tb_cus.InsertCustomer(ec_tb_cus, data_type);
        }

        //update Customer
        public int UpdateCustomer(EC_tb_Customer ec_tb_cus, bool data_type)
        {
            return sql_tb_cus.UpdateCustomer(ec_tb_cus, data_type);
        }

        //delete Customer
        public int DeleteCustomer(EC_tb_Customer ec_tb_cus)
        {
            return sql_tb_cus.DeleteCustomer(ec_tb_cus);
        }

        //delete all Customer
        public int DeleteAllCustomer()
        {
            return sql_tb_cus.DeleteAllCustomer();
        }

        //get Customer
        public DataTable GetCustomer(string con)
        {
            DataTable dt = new DataTable();
            dt = sql_tb_cus.GetCustomer(con);
            return dt;
        }

        //get sum Customer
        public int GetSumCustomer(string con)
        {
            return sql_tb_cus.GetSumCustomer(con);
        }

        //get customer follow paging
        public DataTable GetCustomerFollowPaging(int be_limit, int af_limit, string con1, string con2, bool sql_database_type)
        {
            DataTable tb = new DataTable();
            tb = sql_tb_cus.GetCustomerFollowPaging(be_limit, af_limit, con1, con2, sql_database_type);
            return tb;
        }

        //GetMaxCustomerID
        public int GetMaxCustomerID(string con)
        {
            return sql_tb_cus.GetMaxCustomerID(con);
        }
    }
}
