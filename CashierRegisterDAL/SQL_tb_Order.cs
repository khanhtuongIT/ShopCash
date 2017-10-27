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
    public class SQL_tb_Order
    {
        //insert Order
        public int InsertOrder(EC_tb_Order ec_tb_ord, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "INSERT INTO [tb_Order]([CustomerID],[CustomerName],[Quantity],[OrderDate],[SalespersonID],[SalespersonName],[PaymentID],[PaymentName],[DiscountType],[Discount],[TotalDiscount],[TotalTax],[TotalAmount]) VALUES(" +
                              ec_tb_ord.CustomerID + ",'" + ec_tb_ord.CustomerName + "'," + ec_tb_ord.Quatity + ",'" + ec_tb_ord.OrderDate + "'," + ec_tb_ord.SalesPersonID + ",'" + ec_tb_ord.SalesPersonName + "'," +
                              ec_tb_ord.PaymentID + ",'" + ec_tb_ord.PaymentName + "'," + ec_tb_ord.DiscountType + "," + ec_tb_ord.Discount.ToString(new CultureInfo("en-US")) + "," + ec_tb_ord.TotalDiscount.ToString(new CultureInfo("en-US")) + "," + ec_tb_ord.TotalTax.ToString(new CultureInfo("en-US")) + "," + ec_tb_ord.TotalAmount.ToString(new CultureInfo("en-US")) + ")";
            else
                sql = "INSERT INTO [tb_Order]([CustomerID],[CustomerName],[Quantity],[OrderDate],[SalespersonID],[SalespersonName],[PaymentID],[PaymentName],[DiscountType],[Discount],[TotalDiscount],[TotalTax],[TotalAmount]) VALUES(" +
                              ec_tb_ord.CustomerID + ", N'" + ec_tb_ord.CustomerName + "', " + ec_tb_ord.Quatity + ", N'" + ec_tb_ord.OrderDate + "', " + ec_tb_ord.SalesPersonID + ", N'" + ec_tb_ord.SalesPersonName + "', " +
                              ec_tb_ord.PaymentID + ", N'" + ec_tb_ord.PaymentName + "', " + ec_tb_ord.DiscountType + ", " + ec_tb_ord.Discount.ToString(new CultureInfo("en-US")) + ", " + ec_tb_ord.TotalDiscount.ToString(new CultureInfo("en-US")) + ", " + ec_tb_ord.TotalTax.ToString(new CultureInfo("en-US")) + "," + ec_tb_ord.TotalAmount.ToString(new CultureInfo("en-US")) + ")";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //updater Order
        public int UpdateOrder(EC_tb_Order ec_tb_ord, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "UPDATE [tb_Order] SET [CustomerID] = " + ec_tb_ord.CustomerID + ", [CustomerName] = '" + ec_tb_ord.CustomerName + "',[Quantity] =" + ec_tb_ord.Quatity + ",[OrderDate] ='" + ec_tb_ord.OrderDate +
                            "', [SalespersonID] = " + ec_tb_ord.SalesPersonID + ", [SalespersonName] ='" + ec_tb_ord.SalesPersonName + "', [PaymentID] = " + ec_tb_ord.PaymentID + ", [PaymentName] = '" + ec_tb_ord.PaymentName +
                            "',[DiscountType]=" + ec_tb_ord.DiscountType + ",[Discount] = " + ec_tb_ord.Discount.ToString(new CultureInfo("en-US")) + ",[TotalDiscount]=" + ec_tb_ord.TotalDiscount.ToString(new CultureInfo("en-US")) + ",[TotalTax] = " + ec_tb_ord.TotalTax.ToString(new CultureInfo("en-US")) + ",[TotalAmount] = " + ec_tb_ord.TotalAmount.ToString(new CultureInfo("en-US")) + " WHERE [OrderID] = " + ec_tb_ord.OrderID;
            else
                sql = "UPDATE [tb_Order] SET [CustomerID] = " + ec_tb_ord.CustomerID + ", [CustomerName] = N'" + ec_tb_ord.CustomerName + "', [Quantity] = " + ec_tb_ord.Quatity + ", [OrderDate] = N'" + ec_tb_ord.OrderDate +
                            "', [SalespersonID] = " + ec_tb_ord.SalesPersonID + ", [SalespersonName] = N'" + ec_tb_ord.SalesPersonName + "', [PaymentID] = " + ec_tb_ord.PaymentID + ", [PaymentName] = N'" + ec_tb_ord.PaymentName +
                            "', [DiscountType] = " + ec_tb_ord.DiscountType + ", [Discount] = " + ec_tb_ord.Discount.ToString(new CultureInfo("en-US")) + ", [TotalDiscount] = " + ec_tb_ord.TotalDiscount.ToString(new CultureInfo("en-US")) + ", [TotalTax] = " + ec_tb_ord.TotalTax.ToString(new CultureInfo("en-US")) + ",[TotalAmount] = " + ec_tb_ord.TotalAmount.ToString(new CultureInfo("en-US")) + " WHERE [OrderID] = " + ec_tb_ord.OrderID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete Order
        public int DeleteOrder(EC_tb_Order ec_tb_ord)
        {
            string sql = "DELETE FROM [tb_Order] WHERE [OrderID]=" + ec_tb_ord.OrderID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete all Order
        public int DeleteAllOrder()
        {
            string sql = "DELETE FROM [tb_Order]";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //get Order
        public DataTable GetOrder(string con)
        {
            return ConnectionDB.GetData(" SELECT * FROM [tb_Order] " + con);
        }

        //GetOrderPrint
        public DataTable GetOrderPrint(string sql)
        {
            return ConnectionDB.GetData(sql);
        }

        //get sum order
        public int GetSumOrder(string con)
        {
            int sum_order = 0;
            string _sum_order = ConnectionDB.GetOnlyRow("select count([OrderID]) from [tb_Order] " + con);
            if (int.TryParse(_sum_order, out sum_order) == true)
                return sum_order;
            else
                return 0;
        }

        //get order follow paging
        public DataTable GetOrderFollowPaging(int be_limit, int af_limit, string con1, string con2, bool sql_database_type)
        {
            string sql = "";
            DataTable tb = new DataTable();
            if (sql_database_type == false)
                sql = "select * from [tb_Order] " + con1 + " limit " + be_limit + ", " + af_limit;
            else
                sql = "select top(" + af_limit + ") * from [tb_Order] where [OrderID] not in (select top(" + be_limit + ") [OrderID] from [tb_Order] " + con1 + ") " + con2;
            tb = ConnectionDB.GetData(sql);
            return tb;
        }
      
        //get year from tb_Order
        DataTable tb_year = new DataTable();
        public DataTable GetYearFromOrder(string condition, bool sql_database_type) 
        {
            string sql = "";
            if (sql_database_type == false)
                sql = "select distinct strftime('%Y', [tb_Order].[OrderDate]) as Year from [tb_Order] " + condition;
            else
                sql = "select distinct year([tb_Order].[OrderDate]) as Year from [tb_Order] " + condition;

            tb_year = ConnectionDB.GetData(sql);
            return tb_year;
        }

        //GetMaxOrderID
        public int GetMaxOrderID(string con)
        {
            int max_order_id = 0;
            Int32.TryParse(ConnectionDB.GetOnlyRow("select max(OrderID) from tb_Order"), out max_order_id);
            return max_order_id;
        }
    }
}
