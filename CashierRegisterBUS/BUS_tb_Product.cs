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
    public class BUS_tb_Product
    {
        SQL_tb_Product sql_tb_pro = new SQL_tb_Product();

        //insert Product
        public int InsertProduct(EC_tb_Product ec_tb_pro, bool data_type)
        {
            return sql_tb_pro.InsertProduct(ec_tb_pro, data_type);
        }

        //update Product
        public int UpdateProduct(EC_tb_Product ec_tb_pro, bool data_type)
        {
            return sql_tb_pro.UpdateProduct(ec_tb_pro, data_type);
        }

        //update InventoryCount
        public int UpdateInventoryCount(EC_tb_Product ec_tb_product)
        {
            return sql_tb_pro.UpdateInventoryCount(ec_tb_product);
        }

        //delete Product
        public int DeleteProduct(EC_tb_Product ec_tb_pro)
        {
            return sql_tb_pro.DeleteProduct(ec_tb_pro);
        }

        //delete all Product
        public int DeleteAllProduct()
        {
            return sql_tb_pro.DeleteAllProduct();
        }

        //delete product from CategoryID
        public int DeleteProductFromCategoryID(EC_tb_Product ec_tb_pro)
        {
            return sql_tb_pro.DeleteProductFromCategoryID(ec_tb_pro);
        }

        //get Product
        public DataTable GetProduct(string con)
        {
            DataTable dt = new DataTable();
            dt = sql_tb_pro.GetProduct(con);
            return dt;
        }

        //find product is active
        public bool FindProductIsActive(int categoryid)
        {
            return sql_tb_pro.FindProductIsActive(categoryid);
        }

        //get sum Product
        public int GetSumProduct(string con)
        {
            int sum_product = 0;
            sum_product = sql_tb_pro.GetTotalProduct(con);
            return sum_product;
        }

        //get product follow paging
        public DataTable GetProductFollowPaging(int be_limit, int af_limit, string con1, string con2, bool sql_database_type, string _oderby)
        {
            DataTable tb = new DataTable();
            tb = sql_tb_pro.GetProductFollowPaging(be_limit, af_limit, con1, con2, sql_database_type, _oderby);
            return tb;
        }

        //getMaxProductID
        public int GetMaxProductID(string con)
        {
            return sql_tb_pro.GetMaxProductID(con);
        }

        //GetTotalProduct
        public int GetTotalProduct(string con)
        {
            return sql_tb_pro.GetTotalProduct(con);
        }
    }
}
