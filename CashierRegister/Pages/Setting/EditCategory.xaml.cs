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

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for EditCategory.xaml
    /// </summary>
    public partial class EditCategory : ModernDialog
    {
        //using for category
        private BUS_tb_Category bus_tb_category = new BUS_tb_Category();

        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();

        //delegate
        public delegate void lbCategory_EditDelete_Delegate(bool edit_flag);
        public event lbCategory_EditDelete_Delegate lbcategory_edit_delegate;

        public EditCategory()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            txbCategoryName.Focus();
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txbCategoryName.Text = StaticClass.GeneralClass.categoryname_general;
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            AcceptEdit();
        }

        //txbCategoryName_KeyDown
        private void txbCategoryName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
                AcceptEdit();
        }

        //AcceptAdd
        private void AcceptEdit()
        {
            try
            {
                EC_tb_Category ec_tb_category = new EC_tb_Category();

                if (txbCategoryName.Text == "")
                {
                    tblNotification.Text = FindResource("category_name_empty").ToString();
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
                    ec_tb_category.CategoryName = StaticClass.GeneralClass.HandlingSpecialCharacter(txbCategoryName.Text.Trim());
                    ec_tb_category.CategoryID = StaticClass.GeneralClass.categoryid_general;

                    if (bus_tb_category.UpdateCategory(ec_tb_category, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {
                        if (lbcategory_edit_delegate != null)
                        {
                            StaticClass.GeneralClass.flag_add_edit_delete_category_general = true;
                            lbcategory_edit_delegate(true);
                        }
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }
    }
}
