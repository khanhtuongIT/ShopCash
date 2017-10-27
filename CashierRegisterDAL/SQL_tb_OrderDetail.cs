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
    public class SQL_tb_OrderDetail
    {
        //insert OrderDetail
        public int InsertOrderDetail(EC_tb_OrderDetail ec_tb_od, bool data_type)
        {
            string sql = "";
            if (!data_type)
                sql = "INSERT INTO [tb_OrderDetail]([CategoryID],[CategoryName],[ProductID],[ProductName],[Cost],[Price],[Qty],[Tax],[DiscountType],[Discount],[TotalDiscount],[Total],[OrderID]) VALUES(" +
                              ec_tb_od.CategoryID + ",'" + ec_tb_od.CategoryName + "'," + ec_tb_od.ProductID + ",'" + ec_tb_od.ProductName + "'," + ec_tb_od.Cost.ToString(new CultureInfo("en-US")) + ", " + ec_tb_od.Price.ToString(new CultureInfo("en-US")) + "," + ec_tb_od.Qty + "," + ec_tb_od.Tax.ToString(new CultureInfo("en-US")) +
                              "," + ec_tb_od.DiscountType + "," + ec_tb_od.Discount.ToString(new CultureInfo("en-US")) + "," + ec_tb_od.TotalDiscount.ToString(new CultureInfo("en-US")) + "," + ec_tb_od.Total.ToString(new CultureInfo("en-US")) + "," + ec_tb_od.OrderID + ")";
            else
                sql = "INSERT INTO [tb_OrderDetail]([CategoryID],[CategoryName],[ProductID],[ProductName],[Cost],[Price],[Qty],[Tax],[DiscountType],[Discount],[TotalDiscount],[Total],[OrderID]) VALUES(" +
                              ec_tb_od.CategoryID + ", N'" + ec_tb_od.CategoryName + "', " + ec_tb_od.ProductID + ", N'" + ec_tb_od.ProductName + "', " + ec_tb_od.Cost.ToString(new CultureInfo("en-US")) + ", " + ec_tb_od.Price.ToString(new CultureInfo("en-US")) + "," + ec_tb_od.Qty + "," + ec_tb_od.Tax.ToString(new CultureInfo("en-US")) +
                              "," + ec_tb_od.DiscountType + "," + ec_tb_od.Discount.ToString(new CultureInfo("en-US")) + "," + ec_tb_od.TotalDiscount.ToString(new CultureInfo("en-US")) + "," + ec_tb_od.Total.ToString(new CultureInfo("en-US")) + "," + ec_tb_od.OrderID + ")";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update OrderDetail
        public int UpdateOrderDetail(EC_tb_OrderDetail ec_tb_od, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "UPDATE [tb_OrderDetail] SET [CategoryID] = " + ec_tb_od.CategoryID + ",[CategoryName]='" + ec_tb_od.CategoryName + "',[ProductID] = " + ec_tb_od.ProductID + ",[ProductName]='" + ec_tb_od.ProductName + "', [Cost] = " + ec_tb_od.Cost.ToString(new CultureInfo("en-US")) + ", [Price] = " + ec_tb_od.Price.ToString(new CultureInfo("en-US")) +
                              ",[Qty] =" + ec_tb_od.Qty + ",[Tax] =" + ec_tb_od.Tax.ToString(new CultureInfo("en-US")) + ",[Discount] =" + ec_tb_od.Discount.ToString(new CultureInfo("en-US")) + ",[TotalDiscount]=" + ec_tb_od.TotalDiscount.ToString(new CultureInfo("en-US")) + ",[Total] =" + ec_tb_od.Total.ToString(new CultureInfo("en-US")) + ",[OrderID] =" + ec_tb_od.OrderID + " WHERE [ID]=" + ec_tb_od.ID;
            else
                sql = "UPDATE [tb_OrderDetail] SET [CategoryID] = " + ec_tb_od.CategoryID + ",[CategoryName]= N'" + ec_tb_od.CategoryName + "',[ProductID] = " + ec_tb_od.ProductID + ",[ProductName]= N'" + ec_tb_od.ProductName + "', [Cost] = " + ec_tb_od.Cost.ToString(new CultureInfo("en-US")) + ", [Price] = " + ec_tb_od.Price.ToString(new CultureInfo("en-US")) +
                              ",[Qty] =" + ec_tb_od.Qty + ",[Tax] =" + ec_tb_od.Tax.ToString(new CultureInfo("en-US")) + ",[Discount] =" + ec_tb_od.Discount.ToString(new CultureInfo("en-US")) + ",[TotalDiscount]=" + ec_tb_od.TotalDiscount.ToString(new CultureInfo("en-US")) + ",[Total] =" + ec_tb_od.Total.ToString(new CultureInfo("en-US")) + ",[OrderID] =" + ec_tb_od.OrderID + " WHERE [ID]=" + ec_tb_od.ID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete OrderDetail
        public int DeleteOrderDetail(EC_tb_OrderDetail ec_tb_od)
        {
            string sql = "DELETE FROM [tb_OrderDetail] WHERE [OrderID]=" + ec_tb_od.OrderID + " AND [ProductID] = " + ec_tb_od.ProductID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete all OrderDetail
        public int DeleteAllOrderDetail(string condition)
        {
            string sql = "DELETE FROM [tb_OrderDetail] " + condition;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //get OrderDetail
        public DataTable getOrderDetail(string con)
        {
            string sql = "SELECT * FROM tb_OrderDetail " + con;
            //System.Diagnostics.Debug.WriteLine(sql);
            return ConnectionDB.GetData(sql);
        }

        //GetQtyProductOutput
        public int GetQtyProductOutput(string con)
        {
            string qty_productoutput = "";
            string sql = "SELECT SUM(Qty) FROM [tb_OrderDetail] " + con;
            qty_productoutput = ConnectionDB.GetOnlyRow(sql);
            if (qty_productoutput == "")
                return 0;
            else
                return Convert.ToInt32(qty_productoutput);
        }

        //GetQuatityOrderDetail
        public DataTable GetQuatityOrderDetail(string condition)
        {
            string sql = "select [tb_OrderDetail].[ProductID], sum([tb_OrderDetail].[Qty]) as Qty from [tb_OrderDetail] JOIN [tb_Product] ON [tb_Product].[ProductID] = [tb_OrderDetail].[ProductID] WHERE [tb_Product].[Active] = 1 " + condition + " group by [tb_Product].[ProductID]";
            return ConnectionDB.GetData(sql);
        }

        //GetTotalRevenueAll
        public DataTable GetTotalRevenueAll(string condition, bool sql_database_type)
        {
            string sql = "";
            DataTable tb_revenue_all = new DataTable();
            if (sql_database_type == false)
                sql = "select strftime('%Y', [tb_Order].[OrderDate]) as RevenueAll, sum([tb_Order].[TotalAmount]) as TotalRevenueAll from [tb_Order] " + condition + " group by RevenueAll";
            else
                sql = "select year([tb_Order].[OrderDate]) as RevenueAll, sum([tb_Order].[TotalAmount]) as TotalRevenueAll from [tb_Order] " + condition + " group by year([tb_Order].[OrderDate])";
            tb_revenue_all = ConnectionDB.GetData(sql);
            return tb_revenue_all;
        }

        //GetTotalCapitalAll
        public DataTable GetTotalCapitalAll(string condition, bool sql_database_type)
        {
            string sql = "";
            DataTable tb_capital_all = new DataTable();
            if (sql_database_type == false)
                sql = "select strftime('%Y', [tb_Order].[OrderDate]) as CapitalAll, sum([tb_OrderDetail].[Cost] * [tb_OrderDetail].[Qty]) as TotalCapitalAll from [tb_Order] join [tb_OrderDetail] on [tb_Order].[OrderID] = [tb_OrderDetail].[OrderID] " + condition + " group by CapitalAll";
            else
                sql = "select year([tb_Order].[OrderDate]) as CapitalAll, sum([tb_OrderDetail].[Cost] * [tb_OrderDetail].[Qty]) as TotalCapitalAll from [tb_Order] join [tb_OrderDetail] on [tb_Order].[OrderID] = [tb_OrderDetail].[OrderID] " + condition + " group by year([tb_Order].[OrderDate])";

            tb_capital_all = ConnectionDB.GetData(sql);
            return tb_capital_all;
        }

        //GetTotalRevenueYear
        public DataTable GetTotalRevenueYear(string _str_year, string condition, bool sql_database_type)
        {
            string sql = "";
            if (sql_database_type == false)
                sql = "select strftime('%m', [tb_Order].[OrderDate]) as RevenueMonth, sum([tb_Order].[TotalAmount]) as TotalRevenueMonth from [tb_Order] where strftime('%Y', [tb_Order].[OrderDate]) = '" + _str_year + "' " + condition + " group by RevenueMonth";
            else
                sql = "select month([tb_Order].[OrderDate]) as RevenueMonth, sum([tb_Order].[TotalAmount]) as TotalRevenueMonth from [tb_Order] where year([tb_Order].[OrderDate]) = '" + _str_year + "' " + condition + " group by month([tb_Order].[OrderDate])";

            return ConnectionDB.GetData(sql);
        }

        //GetTotalCapitalYear
        public DataTable GetTotalCapitalYear(string _str_year, string condition, bool sql_database_type)
        {
            string sql = "";
            if (sql_database_type == false)
                sql = "select strftime('%m', [tb_Order].[OrderDate]) as CapitalMonth, sum([tb_OrderDetail].[Qty] * [tb_OrderDetail].[Cost]) as TotalCapitalMoth from [tb_Order] join [tb_OrderDetail] on [tb_Order].[OrderID] = [tb_OrderDetail].[OrderID] where strftime('%Y', [tb_Order].[OrderDate]) = '" + _str_year + "' " + condition + " group by CapitalMonth";
            else
                sql = "select month([tb_Order].[OrderDate]) as CapitalMonth, sum([tb_OrderDetail].[Qty] * [tb_OrderDetail].[Cost]) as TotalCapitalMoth from [tb_Order] join [tb_OrderDetail] on [tb_Order].[OrderID] = [tb_OrderDetail].[OrderID] where year([tb_Order].[OrderDate]) = '" + _str_year + "' " + condition + " group by month([tb_Order].[OrderDate])";

            return ConnectionDB.GetData(sql);
        }

        //CheckExist
        public string CheckExist(string condition)
        {
            string sql = "select [ProductID] from [tb_OrderDetail] " + condition;
            return ConnectionDB.GetOnlyRow(sql);
        }

        //GetMaxID
        public int GetMaxID(string con)
        {
            int max_id = 0;
            Int32.TryParse(ConnectionDB.GetOnlyRow("select max(ID) from tb_OrderDetail"), out max_id);
            return max_id;
        }
    }
}
