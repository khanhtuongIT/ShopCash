using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterDAL;
using System.Data;

namespace CashierRegisterDAL
{
    public class SQL_tb_Statistic
    {
        //GetStatisticData
        public DataTable GetStatisticData(string _keyWord, string _date)
        {
            string sql = "select pr.[ProductID], pr.[ShortName], pr.[LongName], cat.[CategoryName], (case when [tb_temp].[Total] > 0 then [tb_temp].[Total] else 0 end) as SoldQuantity, pr.[InventoryCount] as QuantityAvailable from [tb_Product] as pr " +
                         "inner join [tb_Category] as cat on pr.[CategoryID] = cat.[CategoryID] " +
                         "left outer join (select odd.[ProductID], sum(odd.[Qty]) as Total from [tb_Order] as od inner join [tb_OrderDetail] as odd on od.[OrderID] = odd.[OrderID] where od.[OrderDate] like '%" + _date + "%' group by odd.[ProductID]) as tb_temp " +
                         "on pr.[ProductID] = [tb_temp].[ProductID] where (pr.[ShortName] like '%" + _keyWord + "%' or pr.[LongName] like '%" + _keyWord + "%') order by SoldQuantity desc;";
            System.Diagnostics.Debug.WriteLine(sql);
            return ConnectionDB.GetData(sql);
        }

        //GetStatisticData
        public DataTable GetStatisticData(string _keyWord, string _date, int _categoryID)
        {
            string sql = "select pr.[ProductID], pr.[ShortName], pr.[LongName], cat.[CategoryName], (case when [tb_temp].[Total] > 0 then [tb_temp].[Total] else 0 end) as SoldQuantity, pr.[InventoryCount] as QuantityAvailable from [tb_Product] as pr " +
                         "inner join [tb_Category] as cat on pr.[CategoryID] = cat.[CategoryID] " +
                         "left outer join (select odd.[ProductID], sum(odd.[Qty]) as Total from [tb_Order] as od inner join [tb_OrderDetail] as odd on od.[OrderID] = odd.[OrderID] where od.[OrderDate] like '%" + _date + "%' group by odd.[ProductID]) as tb_temp " +
                         "on pr.[ProductID] = [tb_temp].[ProductID] where (pr.[ShortName] like '%" + _keyWord + "%' or pr.[LongName] like '%" + _keyWord + "%') and pr.[CategoryID] = " + _categoryID + " order by SoldQuantity desc;";
            System.Diagnostics.Debug.WriteLine(sql);
            return ConnectionDB.GetData(sql);
        }
    }
}
