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
    public class BUS_tb_InputHistory
    {
        SQL_tb_InputHistory sql_tb_inputhistory = new SQL_tb_InputHistory();

        //InsertInputHistory
        public int InsertInputHistory(EC_tb_InputHistory ec_tb_inputhistory, bool data_type)
        {
            return sql_tb_inputhistory.InsertInputHistory(ec_tb_inputhistory, data_type);
        }

        //DeleteInputHistory
        public int DeleteInputHistory(EC_tb_InputHistory ec_tb_inputhistory)
        {
            return sql_tb_inputhistory.DeleteInputHistory(ec_tb_inputhistory);
        }

        //DeleteAllInputHistory
        public int DeleteAllInputHistory()
        {
            return sql_tb_inputhistory.DeleteAllInputHistory();
        }

        //GetInputHistory
        public DataTable GetInputHistory(string conn)
        {
            return sql_tb_inputhistory.GetInputHistory(conn);
        }

        //GetAllInputHistory
        public DataTable GetAllInputHistory(string conn)
        {
            return sql_tb_inputhistory.GetAllInputHistory(conn);
        }

        //MaxInputID
        public int GetMaxInputID(string con)
        {
            return sql_tb_inputhistory.GetMaxInputID(con);
        }
    }
}
