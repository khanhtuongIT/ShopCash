using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterEntity;
using System.Data;

namespace CashierRegisterDAL
{
    public class SQL_tb_SalesPerson
    {
        //insert SalesPerson
        public int InsertSalesperson(EC_tb_SalesPerson ec_tb_sp, bool data_type)
        {
            string sql = "";
            if (!data_type)
                sql = "INSERT INTO [tb_Salesperson]([Name], [Birthday], [Address], [Email], [Password], [Active]) VALUES('" + ec_tb_sp.Name + "', '" + ec_tb_sp.Birthday + "', '" + ec_tb_sp.Address + "', '" + ec_tb_sp.Email + "', '" + ec_tb_sp.Password + "', " + ec_tb_sp.Active + ")";
            else
                sql = "INSERT INTO [tb_Salesperson]([Name], [Birthday], [Address], [Email], [Password], [Active]) VALUES(N'" + ec_tb_sp.Name + "', N'" + ec_tb_sp.Birthday + "', N'" + ec_tb_sp.Address + "', N'" + ec_tb_sp.Email + "', N'" + ec_tb_sp.Password + "', " + ec_tb_sp.Active + ")";

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //upadate SalesPerson
        public int UpdateSalesperson(EC_tb_SalesPerson ec_tb_sp, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "UPDATE [tb_Salesperson] SET [Name] ='" + ec_tb_sp.Name + "', [Birthday]='" + ec_tb_sp.Birthday + "', [Address]='" + ec_tb_sp.Address + "', [Email]='" + ec_tb_sp.Email + "', [Password]='" + ec_tb_sp.Password + "', [Active]=" + ec_tb_sp.Active + " WHERE [SalespersonID]=" + ec_tb_sp.SalespersonID;
            else
                sql = "UPDATE [tb_Salesperson] SET [Name] = N'" + ec_tb_sp.Name + "', [Birthday] = N'" + ec_tb_sp.Birthday + "', [Address] = N'" + ec_tb_sp.Address + "', [Email]= N'" + ec_tb_sp.Email + "', [Password]= N'" + ec_tb_sp.Password + "', [Active] = " + ec_tb_sp.Active + " WHERE [SalespersonID] = " + ec_tb_sp.SalespersonID;

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update salesperson active
        public void UpdateSalespersonActive(int active)
        {
            ConnectionDB.ExecuteNonQuery("Update [tb_Salesperson] SET [Active]=0");
        }

        //delete SalesPerson
        public int DeleteSalesperson(EC_tb_SalesPerson ec_tb_sp)
        {
            string sql = "DELETE from [tb_SalesPerson] where [SalespersonID]=" + ec_tb_sp.SalespersonID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }


        //delete all SalesPerson
        public int DeleteAllSalesperson()
        {
            string sql = "DELETE from [tb_Salesperson]";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //get dataTable
        public DataTable GetSalesperson(string con)
        {
            DataTable dt = new DataTable();
            dt = ConnectionDB.GetData("SELECT * from [tb_Salesperson] " + con);
            return dt;
        }

        //GetMaxSalespersonID
        public int GetMaxSalespersonID(string con)
        {
            int max_salesperson_id = 0;
            Int32.TryParse(ConnectionDB.GetOnlyRow("select max(SalespersonID) from tb_Salesperson"), out max_salesperson_id);
            return max_salesperson_id;
        }
    }
}
