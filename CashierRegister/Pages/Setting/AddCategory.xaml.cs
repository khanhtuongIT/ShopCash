using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.IO;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.Odbc;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : ModernDialog
    {
        //using for category
        private BUS_tb_Category bus_tb_category = new BUS_tb_Category();

        //delegate
        public delegate void btnAdd_Click_Delegate();
        public event btnAdd_Click_Delegate addcategory_delegate;
        public AddCategory()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { };

            txbCategoryName.Focus();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txbCategoryName.Text == "")
            {
                tblNotification.Text = FindResource("have_not_enter_category_name").ToString();
                txbCategoryName.Focus();
                return;
            }

            if (bus_tb_category.GetCatagorySetting("where Upper(CategoryName)='" + txbCategoryName.Text.ToString().ToUpper() + "'").Rows.Count > 0)
            {
                tblNotification.Text = FindResource("category_name_already").ToString();
                txbCategoryName.Focus();
                return;
            }
            else
            {
                try
                {
                    EC_tb_Category ec_tb_category = new EC_tb_Category();
                    //ec_tb_category.CategoryID = bus_tb_category.GetMaxCategoryID("") + 1;
                    ec_tb_category.CategoryName = txbCategoryName.Text;

                    if (bus_tb_category.InsertCategory(ec_tb_category, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {
                        if (addcategory_delegate != null)
                        {
                            StaticClass.GeneralClass.flag_add_edit_delete_category_general = true;
                            addcategory_delegate();
                        }
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    tblNotification.Text = ex.Message;
                }
            }
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //txbCategoryName_KeyDown
        private void txbCategoryName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                if (txbCategoryName.Text == "")
                {
                    tblNotification.Text = FindResource("have_not_enter_category_name").ToString();
                    txbCategoryName.Focus();
                    return;
                }

                if (bus_tb_category.GetCatagorySetting("where Upper(CategoryName)='" + txbCategoryName.Text.ToString().ToUpper() + "'").Rows.Count > 0)
                {
                    tblNotification.Text = FindResource("category_name_already").ToString();
                    txbCategoryName.Focus();
                    return;
                }
                else
                {
                    try
                    {
                        EC_tb_Category ec_tb_category = new EC_tb_Category();
                        //ec_tb_category.CategoryID = bus_tb_category.GetMaxCategoryID("") + 1;
                        ec_tb_category.CategoryName = txbCategoryName.Text;

                        if (bus_tb_category.InsertCategory(ec_tb_category, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        {
                            if (addcategory_delegate != null)
                            {
                                StaticClass.GeneralClass.flag_add_edit_delete_category_general = true;
                                addcategory_delegate();
                            }
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        tblNotification.Text = ex.Message;
                    }
                }
            }
        }
    }
}
