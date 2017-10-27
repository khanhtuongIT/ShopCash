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
using System.Threading;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;
using CashierRegister.Model;

namespace CashierRegister.Pages.Report
{
    /// <summary>
    /// Interaction logic for OrderDetail.xaml
    /// </summary>
    public partial class OrderDetail : ModernWindow
    {
        private bool flag_deleted = false;

        //using for order detai
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();
        private List<EC_tb_OrderDetail> list_ec_tb_orderdetail = new List<EC_tb_OrderDetail>();

        //delegate
        public delegate void btnViewDetail_Click_Delegate(bool flag_deleted_all);
        public event btnViewDetail_Click_Delegate btnviewdetail_delegate;

        //OrderDetail
        public OrderDetail()
        {
            InitializeComponent();
            dtgOrderDetail.ItemsSource = list_ec_tb_orderdetail;
        }
        private string getPaymentMethodForOrder(int orderId)
        {
            DataTable method = OrderPayment.getOrderPaymentName(orderId);
            string strMethod = "";
            foreach (DataRow dr in method.Rows)
            {
                strMethod += dr["Card"] + " = "+ StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(dr["Amount"].ToString())) + ";    ";
            }
            strMethod = (strMethod != "") ? "* " + App.Current.FindResource("payment")+": " + strMethod.Remove(strMethod.Length - 5, 5) : "* " + App.Current.FindResource("payment") + ": Cash";
            return strMethod;
        }
        //OrderDetail_Loaded
        private void OrderDetail_Loaded(object sender, RoutedEventArgs e)
        {
            chkCheckAll.IsChecked = false;
            GetOrderDetail(StaticClass.GeneralClass.orderdetailid_general);
            DataStatistic();
            txtPaymentName.Text = getPaymentMethodForOrder(Convert.ToInt16(StaticClass.GeneralClass.orderdetailid_general));
        }

        //DataStatistic
        private void DataStatistic()
        {
            int quantity_total = 0;
            decimal tax_total = 0;
            decimal discount_total = 0;
            decimal total_total = 0;

            if (list_ec_tb_orderdetail.Count > 0)
            {
                foreach (EC_tb_OrderDetail ec_tb_orderdetail in list_ec_tb_orderdetail)
                {
                    quantity_total += ec_tb_orderdetail.Qty;
                    tax_total += ec_tb_orderdetail.Tax;
                    //discount_total += ec_tb_orderdetail.Discount; 
                    discount_total += ec_tb_orderdetail.TotalDiscount;
                    total_total += ec_tb_orderdetail.Total;
                }
            }
            tblQty.Text = quantity_total.ToString();
            tblTax.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(tax_total);
            tblDiscount.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(discount_total + getTotalDiscountByOrderId(StaticClass.GeneralClass.orderdetailid_general));
            _tblTotal.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(total_total - getTotalDiscountByOrderId(StaticClass.GeneralClass.orderdetailid_general));
        }
        private decimal getTotalDiscountByOrderId(int _id)
        {
            decimal _decTotal = 0;
            decimal.TryParse(CashierRegisterDAL.ConnectionDB.GetOnlyRow("select TotalDiscount from tb_Order where OrderID = " + _id), out _decTotal);
            return _decTotal;
        }
        //GetOrderDetail
        DataTable dt_tb_orderdetail = new DataTable();
        private List<EC_tb_OrderDetail> GetOrderDetail(int orderid)
        {
            try
            {
                list_ec_tb_orderdetail.Clear();
                dtgOrderDetail.Items.Refresh();

                dt_tb_orderdetail.Clear();
                int no = 0;

                dt_tb_orderdetail = bus_tb_orderdetail.GetOrderDetail("WHERE OrderID = " + orderid);
                foreach (DataRow datarow in dt_tb_orderdetail.Rows)
                {
                    no++;
                    EC_tb_OrderDetail ec_tb_orderdetail = new EC_tb_OrderDetail();
                    ec_tb_orderdetail.No = no;
                    ec_tb_orderdetail.OrderID = Convert.ToInt32(datarow["OrderID"].ToString());
                    ec_tb_orderdetail.CategoryID = Convert.ToInt32(datarow["CategoryID"].ToString());
                    ec_tb_orderdetail.CategoryName = datarow["CategoryName"].ToString().Trim();
                    ec_tb_orderdetail.ProductID = Convert.ToInt32(datarow["ProductID"].ToString());
                    ec_tb_orderdetail.ProductName = datarow["ProductName"].ToString().Trim();

                    ec_tb_orderdetail.Cost = Convert.ToDecimal(datarow["Cost"].ToString());
                    ec_tb_orderdetail.StrCost = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Cost);

                    ec_tb_orderdetail.Price = Convert.ToDecimal(datarow["Price"].ToString());
                    ec_tb_orderdetail.StrPrice = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Price);

                    ec_tb_orderdetail.Qty = Convert.ToInt32(datarow["Qty"].ToString());

                    ec_tb_orderdetail.Tax = Convert.ToDecimal(datarow["Tax"].ToString());
                    ec_tb_orderdetail.StrTax = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Tax);

                    ec_tb_orderdetail.DiscountType = Convert.ToInt32(datarow["DiscountType"].ToString());
                    if (ec_tb_orderdetail.DiscountType == 1)
                    {
                        ec_tb_orderdetail.DisAmount = "";
                        ec_tb_orderdetail.DisPercent = "%";
                    }
                    else
                    {
                        ec_tb_orderdetail.DisAmount = StaticClass.GeneralClass.currency_setting_general;
                        ec_tb_orderdetail.DisPercent = "";
                    }

                    ec_tb_orderdetail.Discount = Convert.ToDecimal(datarow["Discount"].ToString());
                    ec_tb_orderdetail.StrDiscount = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Discount);

                    ec_tb_orderdetail.TotalDiscount = Convert.ToDecimal(string.IsNullOrEmpty(datarow["TotalDiscount"].ToString()) ? "0" : datarow["TotalDiscount"].ToString());
                    ec_tb_orderdetail.StrTotalDiscount = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.TotalDiscount);

                    ec_tb_orderdetail.Total = Convert.ToDecimal(datarow["Total"].ToString())+ Convert.ToDecimal(datarow["Tax"].ToString());
                    ec_tb_orderdetail.StrTotal = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Total);

                    ec_tb_orderdetail.CheckDel = false;
                    ec_tb_orderdetail.Currency = StaticClass.GeneralClass.currency_setting_general;

                    list_ec_tb_orderdetail.Add(ec_tb_orderdetail);
                }

                tblTotal.Text = FindResource("total").ToString() + "(" + list_ec_tb_orderdetail.Count.ToString() + ")";
                dtgOrderDetail.Items.Refresh();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
            return list_ec_tb_orderdetail;
        }


        //muiBtnPrint_Click
        private Thread order_thread = null;
        private void muiBtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (order_thread != null && order_thread.ThreadState == ThreadState.Running)
                {
                    order_thread.Abort();
                    return;
                }
                else
                {
                    order_thread = new Thread(() =>
                    {
                        this.muiBtnPrint.Dispatcher.Invoke((Action)(() => { muiBtnPrint.Visibility = System.Windows.Visibility.Hidden; }));
                        this.mprPrint.Dispatcher.Invoke((Action)(() => { mprPrint.IsActive = true; }));

                        ReportDetailPrint page = new ReportDetailPrint();
                        page.titleParameter = FindResource("list_of_order").ToString();
                        page.idParameter = FindResource("id").ToString();
                        page.categoryNameParameter = FindResource("category_name").ToString();
                        page.productIDParameter = FindResource("product_id").ToString();
                        page.productNameParameter = FindResource("product_name").ToString();
                        page.priceParameter = FindResource("price").ToString();
                        page.qtyParameter = FindResource("qty").ToString();
                        page.taxParameter = FindResource("tax").ToString();
                        page.discountTypeParameter = FindResource("discount_type").ToString();
                        page.discountParameter = FindResource("discount").ToString();
                        page.totalParameter = FindResource("total").ToString();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            var m = Application.Current.MainWindow;
                            Point m_point_to_screen = StaticClass.GeneralClass.ElementPointToScreenPoint(m as UIElement, new Point(0, 0));

                            if (m.RenderSize.Width > StaticClass.GeneralClass.width_screen_general)
                            {
                                page.point_x = (int)(m_point_to_screen.X + StaticClass.GeneralClass.width_screen_general / 2 - page.Width / 2);
                                page.Height = (int)m.RenderSize.Height;
                            }

                            if (m.RenderSize.Height > StaticClass.GeneralClass.height_screen_working_general)
                            {
                                page.point_y = 0;
                                page.Height = (int)StaticClass.GeneralClass.height_screen_working_general;
                            }

                            else
                            {
                                page.point_x = (int)(m_point_to_screen.X + (m.RenderSize.Width / 2 - page.Width / 2));
                                page.point_y = (int)m_point_to_screen.Y;
                                page.Height = (int)m.RenderSize.Height;
                            }
                            page.ShowInTaskbar = false;
                        }));

                        Thread.Sleep(500);
                        this.mprPrint.Dispatcher.Invoke((Action)(() => { mprPrint.IsActive = false; }));
                        this.muiBtnPrint.Dispatcher.Invoke((Action)(() => { muiBtnPrint.Visibility = System.Windows.Visibility.Visible; }));

                        page.ShowDialog();
                    });
                    order_thread.SetApartmentState(ApartmentState.STA);
                    order_thread.Start();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //chkCheckAll_Checked
        private void chkCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            dtgOrderDetail.SelectedIndex = -1;

            if (dtgOrderDetail.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_orderdetail.Count; i++)
                {
                    list_ec_tb_orderdetail[i].CheckDel = true;
                }

                StaticClass.GeneralClass.list_ec_tb_orderdetail_general.AddRange(list_ec_tb_orderdetail);
                dtgOrderDetail.Items.Refresh();
            }
        }

        //chkCheckAll_Unchecked
        private void chkCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            dtgOrderDetail.SelectedIndex = -1;

            if (dtgOrderDetail.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_orderdetail.Count; i++)
                {
                    list_ec_tb_orderdetail[i].CheckDel = false;
                }
                StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Clear();
                dtgOrderDetail.Items.Refresh();
            }
        }

        //chkDel_Checked
        private void chkDel_Checked(object sender, RoutedEventArgs e)
        {
            if (dtgOrderDetail.SelectedIndex > -1)
            {
                list_ec_tb_orderdetail[dtgOrderDetail.SelectedIndex].CheckDel = true;
                StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Add(list_ec_tb_orderdetail[dtgOrderDetail.SelectedIndex]);
            }
        }

        //chkDel_Unchecked
        private void chkDel_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dtgOrderDetail.SelectedIndex > -1)
            {
                list_ec_tb_orderdetail[dtgOrderDetail.SelectedIndex].CheckDel = false;
                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                {
                    if (list_ec_tb_orderdetail[dtgOrderDetail.SelectedIndex].ProductID == StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].ProductID)
                        StaticClass.GeneralClass.list_ec_tb_orderdetail_general.RemoveAt(i);
                }
            }
        }

        //muiBtnDelete_Click
        private void muiBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteOrderDetail page = new DeleteOrderDetail();
            page.btndelete_delegate += muiBtnDelete_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnDelete_Click_Delegate
        private void muiBtnDelete_Click_Delegate(bool _flag_deleted)
        {
            flag_deleted = _flag_deleted;
            GetOrderDetail(StaticClass.GeneralClass.orderdetailid_general);
            DataStatistic();
        }

        //OrderDetail_Closed
        private void OrderDetail_Closed(object sender, EventArgs e)
        {
            if (btnviewdetail_delegate != null)
            {
                if (list_ec_tb_orderdetail.Count == 0 || flag_deleted == true)
                {
                    btnviewdetail_delegate(true);
                    this.Close();
                }
            }
            else
                this.Close();
        }
    }
}
