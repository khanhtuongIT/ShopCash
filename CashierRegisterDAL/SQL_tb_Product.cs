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
    public class SQL_tb_Product
    {
        //insert Product
        public int InsertProduct(EC_tb_Product ec_tb_pro, bool data_type)
        {
            string sql = "";

            if(!data_type)
                sql = "INSERT INTO [tb_Product]([BarcodeID],[ShortName],[LongName],[Cost],[Price],[InventoryCount],[CategoryID],[Tax],[PathImage],[Capture],[Active],[Country],[Size_Weight]) VALUES('" +
                              ec_tb_pro.BarcodeID + "','" + ec_tb_pro.ShortName + "','" + ec_tb_pro.LongName + "'," + ec_tb_pro.Cost.ToString(new CultureInfo("en-US")) + "," + ec_tb_pro.Price.ToString(new CultureInfo("en-US")) + "," + ec_tb_pro.InventoryCount + "," + ec_tb_pro.CategoryID + "," +
                              ec_tb_pro.Tax + ",'" + ec_tb_pro.PathImage + "'," + ec_tb_pro.Capture + "," + ec_tb_pro.Active + ",'" + ec_tb_pro.Country + "','" + ec_tb_pro.SizeWeight + "')";
            else
                sql = "INSERT INTO [tb_Product]([BarcodeID],[ShortName],[LongName],[Cost],[Price],[InventoryCount],[CategoryID],[Tax],[PathImage],[Capture],[Active],[Country],[Size_Weight]) VALUES(N'" +
                              ec_tb_pro.BarcodeID + "', N'" + ec_tb_pro.ShortName + "', N'" + ec_tb_pro.LongName + "', " + ec_tb_pro.Cost.ToString(new CultureInfo("en-US")) + ", " + ec_tb_pro.Price.ToString(new CultureInfo("en-US")) + ", " + ec_tb_pro.InventoryCount + ", " + ec_tb_pro.CategoryID + "," +
                              ec_tb_pro.Tax + ", N'" + ec_tb_pro.PathImage + "', " + ec_tb_pro.Capture + "," + ec_tb_pro.Active + ", N'" + ec_tb_pro.Country + "', N'" + ec_tb_pro.SizeWeight + "')";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update Product
        public int UpdateProduct(EC_tb_Product ec_tb_pro, bool data_type)
        {
            string sql = "";
            if(!data_type)
                sql = "UPDATE [tb_Product] SET [ShortName] = '" + ec_tb_pro.ShortName + "', [LongName] ='" + ec_tb_pro.LongName + "', [Cost] = " + ec_tb_pro.Cost.ToString(new CultureInfo("en-US")) + ", [Price] = " + ec_tb_pro.Price.ToString(new CultureInfo("en-US")) + ", [InventoryCount] = " + ec_tb_pro.InventoryCount +
                             ",[CategoryID] =" + ec_tb_pro.CategoryID + ",[Tax] =" + ec_tb_pro.Tax + ",[PathImage] ='" + ec_tb_pro.PathImage + "', [Capture]=" + ec_tb_pro.Capture +
                             ",[Active]=" + ec_tb_pro.Active + ",[BarcodeID]='" + ec_tb_pro.BarcodeID + "', [Country] = '" + ec_tb_pro.Country + "', [Size_Weight] = '" + ec_tb_pro.SizeWeight + "' WHERE ProductID = " + ec_tb_pro.ProductID;
            else
                sql = "UPDATE [tb_Product] SET [ShortName] = N'" + ec_tb_pro.ShortName + "', [LongName] = N'" + ec_tb_pro.LongName + "', [Cost] = " + ec_tb_pro.Cost.ToString(new CultureInfo("en-US")) + ", [Price] = " + ec_tb_pro.Price.ToString(new CultureInfo("en-US")) + ", [InventoryCount] = " + ec_tb_pro.InventoryCount +
                             ",[CategoryID] =" + ec_tb_pro.CategoryID + ", [Tax] = " + ec_tb_pro.Tax + ",[PathImage] = N'" + ec_tb_pro.PathImage + "', [Capture]= " + ec_tb_pro.Capture +
                             ", [Active]=" + ec_tb_pro.Active + ",[BarcodeID]= N'" + ec_tb_pro.BarcodeID + "', [Country] = N'" + ec_tb_pro.Country + "', [Size_Weight] = N'" + ec_tb_pro.SizeWeight + "' WHERE ProductID = " + ec_tb_pro.ProductID;

            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //update InventoryCount
        public int UpdateInventoryCount(EC_tb_Product ec_tb_product)
        {
            string sql = "Update [tb_Product] set [InventoryCount] = [InventoryCount] - " + ec_tb_product.InventoryCount + " where [ProductID] = " + ec_tb_product.ProductID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete product
        public int DeleteProduct(EC_tb_Product ec_tb_pro)
        {
            string sql = "DELETE FROM [tb_Product] WHERE ProductID=" + ec_tb_pro.ProductID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete all product
        public int DeleteAllProduct()
        {
            string sql = "DELETE FROM [tb_Product]";
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //delete product from CategoryID
        public int DeleteProductFromCategoryID(EC_tb_Product ec_tb_pro)
        {
            string sql = "DELETE FROM [tb_Product] WHERE [CategoryID]=" + ec_tb_pro.CategoryID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }

        //get Product
        public DataTable GetProduct(string con)
        {
            DataTable dt = new DataTable();
            dt = ConnectionDB.GetData("SELECT * FROM [tb_Product] " + con);
            return dt;
        }

        //find product is active
        public bool FindProductIsActive(int categoryid)
        {
            string sql = "SELECT [ProductID] from [tb_Product] where [CategoryID] = " + categoryid + " AND [Active] = 1";
            string result = ConnectionDB.GetOnlyRow(sql);
            if (result == "")
                return false;
            else
                return true;
        }

        //get sum Product
        public int GetTotalProduct(string con)
        {
            int sum_product = 0;
            string _sum_product = ConnectionDB.GetOnlyRow("select count([ProductID]) from [tb_Product] " + con);
            if (Int32.TryParse(_sum_product, out sum_product))
                return sum_product;
            else
                return 0;
        }

        //get product follow paging
        public DataTable GetProductFollowPaging(int be_limit, int af_limit, string con1, string con2, bool sql_database_type, string _oderby)
        {
            DataTable tb = new DataTable();
            string sql = "";
            if (sql_database_type == false)
                sql = "select *, (select sum(Qty) from tb_OrderDetail where tb_OrderDetail.ProductID = tb_Product.ProductID group by ProductID) as Total from [tb_Product] " + con1 + " " + _oderby + " limit " + be_limit + "," + af_limit;
            else
                //sql = "select top(" + af_limit + ") * from [tb_Product] where [ProductID] not in (select top(" + be_limit + ") [ProductID] from [tb_Product] " + con1 + ") " + con2 + " " + _oderby;
                sql = "select *, (select sum(Qty) from tb_OrderDetail where tb_OrderDetail.ProductID = tb_Product.ProductID group by ProductID) as Total from [tb_Product] " + con1 + " " + _oderby + " OFFSET " + be_limit + " ROWS FETCH NEXT " + af_limit + " ROWS ONLY";
            tb = ConnectionDB.GetData(sql);
            return tb;
        }

        //GetMaxProductID
        public int GetMaxProductID(string con)
        {
            int max_product_id = 0;
            Int32.TryParse(ConnectionDB.GetOnlyRow("select max(ProductID) from tb_Product"), out max_product_id);
            return max_product_id;
        }
    }
}
