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
    public class BUS_tb_User
    {
        SQL_tb_User sql_tb_use = new SQL_tb_User();

        //inser User
        public int InsertUser(EC_tb_User ec_tb_use, bool data_type)
        {
            return sql_tb_use.InsertUser(ec_tb_use, data_type);
        }

        //update User
        public int UpdateUser(EC_tb_User ec_tb_use, bool data_type)
        {
            return sql_tb_use.UpdateUser(ec_tb_use, data_type);
        }

        //update User non password
        public int UpdateUserNonPassword(EC_tb_User ec_tb_use, bool data_type)
        {
            return sql_tb_use.UpdateUserNonPassword(ec_tb_use, data_type);
        }

        //update password user
        public int UpdatePasswordUser(EC_tb_User ec_tb_use, bool data_type)
        {
            return sql_tb_use.UpdatePasswordUser(ec_tb_use, data_type);
        }

        //delete User
        public int DeleteUser(EC_tb_User ec_tb_use)
        {
            return sql_tb_use.DeleteUser(ec_tb_use);
        }

        //delete all User
        public int DeleteAllUser()
        {
            return sql_tb_use.DeleteAllUser();
        }

        //get User
        public DataTable GetUser(string con)
        {
            return sql_tb_use.GetUser(con);
        }
    }
}
