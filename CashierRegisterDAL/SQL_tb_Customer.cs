using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterEntity;
using System.Data;

namespace CashierRegisterDAL
{
    public class SQL_tb_Customer
    {
        //insert Customer
        public int InsertCustomer(EC_tb_Customer ec_tb_cus, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "INSERT INTO [tb_Customer] ([FirstName],[LastName],[Address1],[Address2],[City],[State],[Zipcode],[Phone],[Email]) VALUES('" +
                             ec_tb_cus.FirstName + "','" + ec_tb_cus.LastName + "','" + ec_tb_cus.Address1 + "','" + ec_tb_cus.Address2 + "','" + ec_tb_cus.City +
                             "','" + ec_tb_cus.State + "','" + ec_tb_cus.Zipcode + "','" + ec_tb_cus.Phone + "','" + ec_tb_cus.Email + "')";
            else
                sql = "INSERT INTO [tb_Customer] ([FirstName],[LastName],[Address1],[Address2],[City],[State],[Zipcode],[Phone],[Email]) VALUES(N'" +
                             ec_tb_cus.FirstName + "', N'" + ec_tb_cus.LastName + "', N'" + ec_tb_cus.Address1 + "', N'" + ec_tb_cus.Address2 + "', N'" + ec_tb_cus.City +
                             "', N'" + ec_tb_cus.State + "', N'" + ec_tb_cus.Zipcode + "', N'" + ec_tb_cus.Phone + "', N'" + ec_tb_cus.Email + "')";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update Customer
        public int UpdateCustomer(EC_tb_Customer ec_tb_cus, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "UPDATE [tb_Customer] SET [FirstName] = '" + ec_tb_cus.FirstName + "',[LastName] ='" + ec_tb_cus.LastName +
                        "',[Address1] ='" + ec_tb_cus.Address1 + "',[Address2] ='" + ec_tb_cus.Address2 + "',[City] ='" + ec_tb_cus.City +
                        "',[State] ='" + ec_tb_cus.State + "',[Zipcode] ='" + ec_tb_cus.Zipcode + "',[Phone] ='" + ec_tb_cus.Phone +
                        "',[Email] ='" + ec_tb_cus.Email + "' WHERE CustomerID=" + ec_tb_cus.CustomerID;
            else
                sql = "UPDATE [tb_Customer] SET [FirstName] = N'" + ec_tb_cus.FirstName + "', [LastName] = N'" + ec_tb_cus.LastName +
                       "',[Address1] = N'" + ec_tb_cus.Address1 + "', [Address2] = N'" + ec_tb_cus.Address2 + "', [City] = N'" + ec_tb_cus.City +
                       "', [State] = N'" + ec_tb_cus.State + "',[Zipcode] = N'" + ec_tb_cus.Zipcode + "', [Phone] = N'" + ec_tb_cus.Phone +
                       "', [Email] = N'" + ec_tb_cus.Email + "' WHERE CustomerID=" + ec_tb_cus.CustomerID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete Customer
        public int DeleteCustomer(EC_tb_Customer ec_tb_cus)
        {
            string sql = "delete from tb_Customer where CustomerID=" + ec_tb_cus.CustomerID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }


        //delete all Customer
        public int DeleteAllCustomer()
        {
            string sql = "delete from tb_Customer";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //get Customer
        public DataTable GetCustomer(string con)
        {
            DataTable dt = new DataTable();
            dt = ConnectionDB.GetData("select * from tb_Customer " + con);
            return dt;
        }

        //get sum Customer
        public int GetSumCustomer(string con)
        {
            int sum_customer = 0;
            string _sum_customer = ConnectionDB.GetOnlyRow("select count([CustomerID]) from [tb_Customer] " + con);
            if (int.TryParse(_sum_customer, out sum_customer) == true)
                return sum_customer;
            else
                return 0;
        }

        //get customer follow paging
        public DataTable GetCustomerFollowPaging(int be_limit, int af_limit, string con1, string con2, bool sql_database_type)
        {
            string sql = "";
            DataTable tb = new DataTable();
            if (sql_database_type == false)
                sql = "select * from [tb_Customer] " + con1 + " limit " + be_limit + ", " + af_limit;
            else
                sql = "select top(" + af_limit + ") * from [tb_Customer] where [CustomerID] not in (select top(" + be_limit + ") [CustomerID] from [tb_Customer] " + con1 + ") " + con2;

            tb = ConnectionDB.GetData(sql);
            return tb;
        }

        //GetMaxCustomerID
        public int GetMaxCustomerID(string con)
        {
            int max_customer_id = 0;
            Int32.TryParse(ConnectionDB.GetOnlyRow("select max(CustomerID) from tb_Customer"), out max_customer_id);
            return max_customer_id;
        }
    }
}
