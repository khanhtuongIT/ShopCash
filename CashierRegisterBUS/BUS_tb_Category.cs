using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegisterDAL;
using CashierRegisterEntity;
using System.Data;

namespace CashierRegisterBUS
{
    public class BUS_tb_Category
    {
        SQL_tb_Category sql_tb_cat = new SQL_tb_Category();

        //insert Category
        public int InsertCategory(EC_tb_Category ec_tb_cat, bool database_type)
        {
            return sql_tb_cat.InsertCategory(ec_tb_cat, database_type);
        }

        //update Category
        public int UpdateCategory(EC_tb_Category ec_tb_cat, bool database_type)
        {
            return sql_tb_cat.UpdateCategory(ec_tb_cat, database_type);
        }

        //delete Category
        public int DeleteCategory(EC_tb_Category ec_tb_cat)
        {
            return sql_tb_cat.DeleteCategory(ec_tb_cat);
        }

        //delete all Category
        public int DeleteAllCategory()
        {
            return sql_tb_cat.DeleteAllCategory();
        }

        //get Category for form setting
        public DataTable GetCatagorySetting(string con)
        {
            DataTable dt = new DataTable();
            dt = sql_tb_cat.GetCategorySetting(con);
            return dt;
        }

        //get Category for form main
        public DataTable GetCategoryMain(string con)
        {
            DataTable dt = new DataTable();
            dt = sql_tb_cat.GetCategoryMain(con);
            return dt;
        }

        //get sum Category
        public int GetSumCategory(string con)
        {
            int sum_category = 0;
            sum_category = sql_tb_cat.GetSumCategory(con);
            return sum_category;
        }

        //GetMaxCategoryID
        public int GetMaxCategoryID(string con)
        {
            return sql_tb_cat.GetMaxCategoryID(con);
        }
    }
}
