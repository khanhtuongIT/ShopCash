using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterEntity;
using System.Data;

namespace CashierRegisterDAL
{
    public class SQL_tb_User
    {
        //insert User
        public int InsertUser(EC_tb_User ec_tb_use, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "INSERT INTO [tb_User] ([Name],[Email],[Address],[Password],[Question],[Answer]) VALUES('" + ec_tb_use.Name + "','" + ec_tb_use.Email + "','" + ec_tb_use.Address + "','" + ec_tb_use.Password + "','" + ec_tb_use.Question + "','" + ec_tb_use.Answer + "')";
            else
                sql = "INSERT INTO [tb_User] ([Name],[Email],[Address],[Password],[Question],[Answer]) VALUES(N'" + ec_tb_use.Name + "', N'" + ec_tb_use.Email + "', N'" + ec_tb_use.Address + "', N'" + ec_tb_use.Password + "', N'" + ec_tb_use.Question + "', N'" + ec_tb_use.Answer + "')";

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update User
        public int UpdateUser(EC_tb_User ec_tb_use, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "UPDATE [tb_User] SET [Name] ='" + ec_tb_use.Name + "', [Email] = '" + ec_tb_use.Email + "', [Address] = '" + ec_tb_use.Address + "', [Password] ='" + ec_tb_use.Password + "',[Question] ='" + ec_tb_use.Question + "',[Answer] ='" + ec_tb_use.Answer + "' WHERE [ID]=" + ec_tb_use.ID;
            else
                sql = "UPDATE [tb_User] SET [Name] = N'" + ec_tb_use.Name + "', [Email] = N'" + ec_tb_use.Email + "', [Address] = N'" + ec_tb_use.Address + "', [Password] = N'" + ec_tb_use.Password + "', [Question] = N'" + ec_tb_use.Question + "', [Answer] = N'" + ec_tb_use.Answer + "' WHERE [ID]=" + ec_tb_use.ID;

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update User non password
        public int UpdateUserNonPassword(EC_tb_User ec_tb_use, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "UPDATE [tb_User] SET [Name] ='" + ec_tb_use.Name + "', [Email] = '" + ec_tb_use.Email + "', [Address] = '" + ec_tb_use.Address + "', [Question] ='" + ec_tb_use.Question + "',[Answer] ='" + ec_tb_use.Answer + "' WHERE [ID]=" + ec_tb_use.ID;
            else
                sql = "UPDATE [tb_User] SET [Name] = N'" + ec_tb_use.Name + "', [Email] = N'" + ec_tb_use.Email + "', [Address] = N'" + ec_tb_use.Address + "', [Question] = N'" + ec_tb_use.Question + "', [Answer] = N'" + ec_tb_use.Answer + "' WHERE [ID] = " + ec_tb_use.ID;

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update password user
        public int UpdatePasswordUser(EC_tb_User ec_tb_use, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "UPDATE [tb_User] SET [Password] = '" + ec_tb_use.Password + "' WHERE [ID] = " + ec_tb_use.ID;
            else
                sql = "UPDATE [tb_User] SET [Password] = N'" + ec_tb_use.Password + "' WHERE [ID] = " + ec_tb_use.ID;

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete User
        public int DeleteUser(EC_tb_User ec_tb_use)
        {
            string sql = "DELETE FROM [tb_User] WHERE [ID]=" + ec_tb_use.ID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete all User
        public int DeleteAllUser()
        {
            string sql = "DELETE FROM [tb_User]";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //get User
        public DataTable GetUser(string con)
        {
            string sql = "SELECT * FROM [tb_User] " + con;
            return ConnectionDB.GetData(sql);
        }
    }
}
