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
    /// Interaction logic for DeleteProduct.xaml
    /// </summary>
    public partial class DeleteProduct : ModernDialog
    {
        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //delegate
        public delegate void muiBtnDelete_Click_Delegate(bool flag_delete);
        public event muiBtnDelete_Click_Delegate muibtndelete_delegate;

        //DeleteSalesperson
        public DeleteProduct()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            this.tblConfirm.Text = FindResource("really_want_delete").ToString();
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (muibtndelete_delegate != null)
            {
                //StaticClass.GeneralClass.list_ec_tb_product_general.Clear();
                //muibtndelete_delegate(true);
                this.Close();
            }
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (StaticClass.GeneralClass.list_ec_tb_product_general.Count == 0)
                tblNotification.Text = FindResource("select_least_product").ToString();
            else
            {
                try
                {
                    int result = 0;
                    for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_product_general.Count; i++)
                    {
                        if (bus_tb_orderdetail.GetOrderDetail("WHERE [ProductID]=" + StaticClass.GeneralClass.list_ec_tb_product_general[i].ProductID.ToString()).Rows.Count > 0)
                            tblNotification.Text = FindResource("product").ToString() + " " + StaticClass.GeneralClass.list_ec_tb_product_general[i].ShortName.ToString() + " " + FindResource("already_uses").ToString();
                        else
                        {
                            CashierRegisterEntity.EC_tb_Product ec_tb_product = new CashierRegisterEntity.EC_tb_Product();
                            ec_tb_product.ProductID = StaticClass.GeneralClass.list_ec_tb_product_general[i].ProductID;
                            if (bus_tb_product.DeleteProduct(ec_tb_product) == 1)
                            {
                                if (System.IO.File.Exists(StaticClass.GeneralClass.list_ec_tb_product_general[i].PathImage))
                                    System.IO.File.Delete(StaticClass.GeneralClass.list_ec_tb_product_general[i].PathImage);
                                result++;
                            }
                        }
                    }

                    if (result > 0)
                    {
                        if (muibtndelete_delegate != null)
                        {
                            StaticClass.GeneralClass.flag_add_edit_delete_product_general = true;
                            muibtndelete_delegate(true);
                            this.Close();
                        }
                    }
                    StaticClass.GeneralClass.list_ec_tb_product_general.Clear();
                }
                catch (Exception ex)
                {
                    tblNotification.Text = ex.Message;
                }
            }
        }
    }
}
