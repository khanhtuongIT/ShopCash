using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterEntity;
using System.Data;

namespace CashierRegisterDAL
{
    public class SQL_tb_Category
    {
        //insert Category  precede
        public int InsertCategory(EC_tb_Category ec_tb_cat, bool database_type)
        {
            if(!database_type)
                return ConnectionDB.ExecuteNonQuery("INSERT INTO tb_Category([CategoryName]) VALUES('" + ec_tb_cat.CategoryName + "')");
            else
                return ConnectionDB.ExecuteNonQuery("INSERT INTO tb_Category([CategoryName]) VALUES(N'" + ec_tb_cat.CategoryName + "')");
        }

        //update Category
        public int UpdateCategory(EC_tb_Category ec_tb_cat, bool database_type)
        {
            if(!database_type)
                return ConnectionDB.ExecuteNonQuery("update tb_Category set [CategoryName]='" + ec_tb_cat.CategoryName + "' where CategoryID=" + ec_tb_cat.CategoryID + "");
            else
                return ConnectionDB.ExecuteNonQuery("update tb_Category set [CategoryName]=N'" + ec_tb_cat.CategoryName + "' where CategoryID=" + ec_tb_cat.CategoryID + "");
        }

        //delete Category
        public int DeleteCategory(EC_tb_Category ec_tb_cat)
        {
            return ConnectionDB.ExecuteNonQuery("delete from tb_Category where [CategoryID]=" + ec_tb_cat.CategoryID);
        }

        //delete all Category
        public int DeleteAllCategory()
        {
            return ConnectionDB.ExecuteNonQuery("delete from tb_Category");
        }

        //get maxCategoryID
        public int GetMaxCategoryID()
        {
            int maxcat;
            maxcat = Convert.ToInt32(ConnectionDB.GetOnlyRow("select max(CategoryID) from tb_Category"));
            return maxcat;
        }

        //get Category for form setting
        public DataTable GetCategorySetting(string cond)
        {
            DataTable dt = new DataTable();
            dt = ConnectionDB.GetData("select * from tb_Category " + cond);
            return dt;
        }

        //get Category for form main
        public DataTable GetCategoryMain(string cond)
        {
            DataTable dt = new DataTable();
            dt = ConnectionDB.GetData("select distinct tb_Category.CategoryID, tb_Category.CategoryName from tb_Category inner join tb_Product on tb_Category.CategoryID = tb_Product.CategoryID " + cond);
            return dt;
        }

        //get sum Category
        public int GetSumCategory(string con)
        {
            int sum_category = 0;
            string _sum_category = ConnectionDB.GetOnlyRow("select count([CategoryID]) from [tb_Category] " + con);
            sum_category = Convert.ToInt32(_sum_category);
            return sum_category;
        }

        ////get category follow paging
        //public DataTable GetCategoryFollowPaging(int be_limit, int af_limit, string con1, string con2)
        //{
        //    DataTable tb = new DataTable();
        //    string sql = "select * from [tb_Category] where [CategoryID] not in (select [CategoryID] from [tb_Category] " + con1 + " limit " + be_limit + ") " + con2 + " limit " + af_limit;
        //    tb = ConnectionDB.GetData(sql);
        //    return tb;
        //}

        //GetMaxCategoryID
        public int GetMaxCategoryID(string con)
        {
            int max_category_id = 0;
            Int32.TryParse(ConnectionDB.GetOnlyRow("select max(CategoryID) from tb_Category"), out max_category_id);
            return max_category_id;
        }
    }
}
