using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterEntity;
using System.Data;
using System.Globalization;


namespace CashierRegisterDAL
{
    public class SQL_tb_InputHistory
    {
        //InsertInputHistory
        public int InsertInputHistory(EC_tb_InputHistory ec_tb_inputhistory, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "INSERT INTO [tb_InputHistory]([ProductID], [ProductName], [InputDate], [UserID], [UserName], [Cost], [Price], [InventoryCount], [CategoryID], [CategoryName], [Tax], [Active], [Country], [Size_Weight]) VALUES(" +
                              ec_tb_inputhistory.ProductID + ", '" + ec_tb_inputhistory.ProductName + "', '" + ec_tb_inputhistory.InputDate + "', " + ec_tb_inputhistory.UserID + ", '" + ec_tb_inputhistory.UserName + "', " + ec_tb_inputhistory.Cost.ToString(new CultureInfo("en-US")) +
                              ", " + ec_tb_inputhistory.Price.ToString(new CultureInfo("en-US")) + ", " + ec_tb_inputhistory.InventoryCount + "," + ec_tb_inputhistory.CategoryID + ", '" + ec_tb_inputhistory.CategoryName + "', " + ec_tb_inputhistory.Tax + ", " + ec_tb_inputhistory.Active + ", '" + ec_tb_inputhistory.Country + "', '" + ec_tb_inputhistory.SizeWeight + "')";
            else
                sql = "INSERT INTO [tb_InputHistory]([ProductID], [ProductName], [InputDate], [UserID], [UserName], [Cost], [Price], [InventoryCount], [CategoryID], [CategoryName], [Tax], [Active], [Country], [Size_Weight]) VALUES(" +
                              ec_tb_inputhistory.ProductID + ", N'" + ec_tb_inputhistory.ProductName + "', N'" + ec_tb_inputhistory.InputDate + "', " + ec_tb_inputhistory.UserID + ", N'" + ec_tb_inputhistory.UserName + "', " + ec_tb_inputhistory.Cost.ToString(new CultureInfo("en-US")) +
                              ", " + ec_tb_inputhistory.Price.ToString(new CultureInfo("en-US")) + ", " + ec_tb_inputhistory.InventoryCount + "," + ec_tb_inputhistory.CategoryID + ", N'" + ec_tb_inputhistory.CategoryName + "', " + ec_tb_inputhistory.Tax + ", " + ec_tb_inputhistory.Active + ", N'" + ec_tb_inputhistory.Country + "', N'" + ec_tb_inputhistory.SizeWeight + "')";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //DeleteInputHistory
        public int DeleteInputHistory(EC_tb_InputHistory ec_tb_inputhistory)
        {
            string sql = "DELETE FROM  [tb_InputHistory] WHERE [InputID] = " + ec_tb_inputhistory.InputID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }


        //DeleteAllInputHistory
        public int DeleteAllInputHistory()
        {
            string sql = "DELETE FROM [tb_InputHistory]";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //GetInputHistory
        public DataTable GetInputHistory(string conn)
        {
            DataTable tb_inputhistory = new DataTable();
            tb_inputhistory = ConnectionDB.GetData("SELECT * FROM [tb_InputHistory] " + conn + " order by InPutID DESC");
            return tb_inputhistory;
        }

        //GetAllInputHistory
        public DataTable GetAllInputHistory(string conn)
        {
            DataTable tb_inputhistory = new DataTable();
            tb_inputhistory = ConnectionDB.GetData("select * from [tb_InputHistory] " + conn);
            return tb_inputhistory;
        }

        //GetMaxInputID
        public int GetMaxInputID(string con)
        {
            int max_input_id = 0;
            Int32.TryParse(ConnectionDB.GetOnlyRow("select max(InputID) from tb_InputHistory"), out max_input_id);
            return max_input_id;
        }
    }
}
