using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterDAL;
using System.Data;
using CashierRegisterEntity;

namespace CashierRegisterDAL
{
    public class SQL_tb_Payment
    {
        //insert Payment
        public int InsertPayment(EC_tb_Payment ec_tb_pay, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "INSERT INTO [tb_Payment] ([Card]) VALUES('" + ec_tb_pay.Card + "');";
            else
                sql = "INSERT INTO [tb_Payment] ([Card]) VALUES(N'" + ec_tb_pay.Card + "');";

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update Payment
        public int UpdatePayment(EC_tb_Payment ec_tb_pay, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "UPDATE [tb_Payment] SET [Card]= '" + ec_tb_pay.Card + "' WHERE [PaymentID]=" + ec_tb_pay.PaymentID + ";";
            else
                sql = "UPDATE [tb_Payment] SET [Card]= N'" + ec_tb_pay.Card + "' WHERE [PaymentID]=" + ec_tb_pay.PaymentID + ";";

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete Payment
        public int DeletePayment(EC_tb_Payment ec_tb_pay)
        {
            string sql = "DELETE FROM [tb_Payment] WHERE [PaymentID]=" + ec_tb_pay.PaymentID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete all Payment
        public int DeleteAllPayment()
        {
            string sql = "DELETE FROM [tb_Payment]";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //get Payment
        public DataTable GetPayment(string con)
        {
            string sql = "select * from tb_Payment " + con;
            return ConnectionDB.GetData(sql);
        }

        //GetMaxPaymentID
        public int GetMaxPaymentID(string con)
        {
            int max_payment_id = 0;
            Int32.TryParse(ConnectionDB.GetOnlyRow("select max(PaymentID) from tb_Payment"), out max_payment_id);
            return max_payment_id;
        }
    }
}
