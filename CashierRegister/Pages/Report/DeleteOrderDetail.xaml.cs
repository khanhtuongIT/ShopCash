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

namespace CashierRegister.Pages.Report
{
    /// <summary>
    /// Interaction logic for DeleteOrderDetail.xaml
    /// </summary>
    public partial class DeleteOrderDetail : ModernDialog
    {
        //using for orderdetail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //using for order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //delegate
        public delegate void muiBtnDelete_Click_Delegate(bool flag_deleted);
        public event muiBtnDelete_Click_Delegate btndelete_delegate;

        //DeleteOrderDetail
        public DeleteOrderDetail()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Clear();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count == 0)
            {
                tblNotification.Text = FindResource("select_least_order_detail").ToString();
                return;
            }
            else
            {
                try
                {
                    int orderdetail_qty_root = bus_tb_orderdetail.GetOrderDetail("WHERE [OrderID] = " + StaticClass.GeneralClass.orderdetailid_general).Rows.Count;
                    int result = 0;

                    for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                    {
                        EC_tb_OrderDetail ec_tb_orderdetail = new EC_tb_OrderDetail();
                        ec_tb_orderdetail.OrderID = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].OrderID;
                        ec_tb_orderdetail.ProductID = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID;
                        //ec_tb_orderdetail.InputID = StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].InputID;

                        if (bus_tb_orderdetail.DeleteOrderDetail(ec_tb_orderdetail) == 1)
                            result++;
                    }

                    if (result == orderdetail_qty_root)
                    {
                        EC_tb_Order ec_tb_order = new EC_tb_Order();
                        ec_tb_order.OrderID = StaticClass.GeneralClass.orderdetailid_general;
                        if (bus_tb_order.DeleteOrder(ec_tb_order) == 1){}
                        else
                        {
                            tblNotification.Text = FindResource("cannot_delete_order").ToString();
                            return;
                        }
                    }

                    
                    //update table order
                    if ((result > 0) && (result < orderdetail_qty_root))
                    {
                        DataTable dt_tb_order = bus_tb_order.GetOrder("WHERE [OrderID] = " + StaticClass.GeneralClass.orderdetailid_general);
                        DataTable dt_tb_orderdetail = bus_tb_orderdetail.GetOrderDetail("WHERE [OrderID] = " + StaticClass.GeneralClass.orderdetailid_general);
                        int quantity = 0;
                        decimal total_tax = 0;
                        decimal total = 0;
                        decimal total_discount = 0;
                        decimal total_amount = 0;

                        foreach (DataRow datarow in dt_tb_orderdetail.Rows)
                        {
                            quantity += Convert.ToInt32(datarow["Qty"].ToString());
                            total_tax += Convert.ToDecimal(datarow["Tax"].ToString());
                            total += Convert.ToDecimal(datarow["Total"].ToString());
                        }

                        if(Convert.ToInt32(dt_tb_order.Rows[0]["DiscountType"].ToString()) == 1)
                            total_discount = Convert.ToDecimal(dt_tb_order.Rows[0]["Discount"].ToString()) * total / 100;
                        else
                            total_discount = Convert.ToDecimal(dt_tb_order.Rows[0]["Discount"].ToString());

                        total_amount = total - total_discount + total_tax;

                        EC_tb_Order ec_tb_order = new EC_tb_Order();
                        ec_tb_order.OrderID = Convert.ToInt32(dt_tb_order.Rows[0]["OrderID"].ToString());
                        ec_tb_order.CustomerID = Convert.ToInt32(dt_tb_order.Rows[0]["CustomerID"].ToString());
                        ec_tb_order.CustomerName = dt_tb_order.Rows[0]["CustomerName"].ToString();
                        ec_tb_order.Quatity = quantity;
                        ec_tb_order.OrderDate = dt_tb_order.Rows[0]["OrderDate"].ToString();
                        ec_tb_order.SalesPersonID = Convert.ToInt32(dt_tb_order.Rows[0]["SalespersonID"].ToString());
                        ec_tb_order.SalesPersonName = dt_tb_order.Rows[0]["SalespersonName"].ToString();
                        ec_tb_order.PaymentID = Convert.ToInt32(dt_tb_order.Rows[0]["PaymentID"].ToString());
                        ec_tb_order.PaymentName = dt_tb_order.Rows[0]["PaymentName"].ToString();
                        ec_tb_order.DiscountType = Convert.ToInt32(dt_tb_order.Rows[0]["DiscountType"].ToString());
                        ec_tb_order.Discount = Convert.ToDecimal(dt_tb_order.Rows[0]["Discount"].ToString());
                        ec_tb_order.TotalDiscount = total_discount;
                        ec_tb_order.TotalTax = total_tax;
                        ec_tb_order.TotalAmount = total_amount;

                        bus_tb_order.UpdateOrder(ec_tb_order, StaticClass.GeneralClass.flag_database_type_general);
                    }

                    if (btndelete_delegate != null)
                    {
                        StaticClass.GeneralClass.flag_check_delete_order_orderdetail = true;

                        StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Clear();
                        btndelete_delegate(true);
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
