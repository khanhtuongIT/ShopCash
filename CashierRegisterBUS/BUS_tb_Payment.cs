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
    public class BUS_tb_Payment
    {
        SQL_tb_Payment sql_tb_pay = new SQL_tb_Payment();

        //insert Payment
        public int InsertPayment(EC_tb_Payment ec_tb_pay, bool data_type)
        {
            return sql_tb_pay.InsertPayment(ec_tb_pay, data_type);
        }

        //update Payment
        public int UpdatePayment(EC_tb_Payment ec_tb_pay, bool data_type)
        {
            return sql_tb_pay.UpdatePayment(ec_tb_pay, data_type);
        }

        //delete Payment
        public int DeletePayment(EC_tb_Payment ec_tb_pay)
        {
            return sql_tb_pay.DeletePayment(ec_tb_pay);
        }

        //delete all Payment
        public int DeleteAllPayment()
        {
            return sql_tb_pay.DeleteAllPayment();
        }

        //get Payment
        public DataTable GetPayment(string con)
        {
            return sql_tb_pay.GetPayment(con);
        }

        //GetMaxPaymentID
        public int GetMaxPaymentID(string con)
        {
            return sql_tb_pay.GetMaxPaymentID(con);
        }
    }
}
