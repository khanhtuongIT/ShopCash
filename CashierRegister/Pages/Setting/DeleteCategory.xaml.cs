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
using System.Data;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for DeleteCategory.xaml
    /// </summary>
    public partial class DeleteCategory : ModernDialog
    {
        //using for category
        private BUS_tb_Category bus_tb_category = new BUS_tb_Category();

        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();

        //using for order detail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //delegate
        public delegate void lbCategory_EditDelete_Delegate(bool edit_flag);
        public event lbCategory_EditDelete_Delegate lbcategory_delete_delegate;

        public DeleteCategory()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            muiBtnOK.Focus();
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            tblCategoryName.Text = StaticClass.GeneralClass.categoryname_general;
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //if (lbcategory_delete_delegate != null)
                //lbcategory_delete_delegate(false);

            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bus_tb_orderdetail.CheckExist(" Where CategoryID = " + StaticClass.GeneralClass.categoryid_general) != "")
                {
                    tblNotification.Text = FindResource("category_uses_existing").ToString();
                    return;
                }
                else
                {
                    EC_tb_Category ec_tb_category = new EC_tb_Category();

                    ec_tb_category.CategoryID = StaticClass.GeneralClass.categoryid_general;

                    if (bus_tb_category.DeleteCategory(ec_tb_category) == 1)
                    {
                        CashierRegisterEntity.EC_tb_Product ec_tb_product = new CashierRegisterEntity.EC_tb_Product();
                        ec_tb_product.CategoryID = StaticClass.GeneralClass.categoryid_general;

                        if (bus_tb_product.DeleteProductFromCategoryID(ec_tb_product) >= 0)
                        {
                            if (lbcategory_delete_delegate != null)
                            {
                                StaticClass.GeneralClass.flag_add_edit_delete_category_general = true;
                                lbcategory_delete_delegate(false);
                            }

                            this.Close();
                        }
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
