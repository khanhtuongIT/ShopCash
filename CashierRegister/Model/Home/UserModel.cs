using CashierRegisterDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegister.Model.Home
{
    public class UserModel
    {
        private static bool _sqlType = StaticClass.GeneralClass.flag_database_type_general;
        public static DataTable getSalePersonLogin(string _userName, string _pass)
        {
            DataTable dt = ConnectionDB.GetData("SELECT [SalespersonID], [Birthday], [Address], [Email], [Active], [Default] from [tb_Salesperson] WHERE lower([Name]) = '" + _userName.ToLower() + "' AND [Password] = '" + _pass + "' and [Active] = 1");
            return dt;
        }
        public static DataTable getUserLogin(string _userName)
        {
            DataTable dt = ConnectionDB.GetData("SELECT * from [tb_User] WHERE lower([Name]) = '" + _userName.ToLower() + "'");
            return dt;
        }
    }
}
