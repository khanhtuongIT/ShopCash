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
using CashierRegisterBUS;
using CashierRegisterEntity;
using System.Data;
using System.Threading;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for PayOther.xaml
    /// </summary>
    public partial class PayOther : ModernDialog
    {
        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();

        //using for payment
        private BUS_tb_Payment bus_tb_payment = new BUS_tb_Payment();
        private List<EC_tb_Payment> list_ec_payment = new List<EC_tb_Payment>();

        //using for order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //using for orderdetail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //delegate
        public delegate void btnPayOther_Click_Delegate(bool flag_paycash);
        public event btnPayOther_Click_Delegate btnpayother_delegate;

        //using for invoice
        private decimal total = 0;

        //thread
        private Thread thread_saveinvoice = null;
        private Thread thread_savesendemail = null;

        //PayOther
        public PayOther()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};

            //create pay other
            GetPayment();
        }

        //GetProductQuantity
        private int GetProductQuantity(List<EC_tb_OrderDetail> list_ec_tb_orderdetail)
        {
            int quantity = 0;
            try
            {
                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count; i++)
                {
                    quantity += StaticClass.GeneralClass.list_ec_tb_orderdetail_general[i].Qty;
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
            return quantity;
        }

        //btnSaveInvoice_Click
        private void btnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            ModernDialog mdd = new ModernDialog();
            mdd.Buttons = new Button[] { mdd.OkButton, mdd.CancelButton, };
            mdd.OkButton.TabIndex = 0;
            mdd.OkButton.Content = FindResource("ok").ToString();
            mdd.CancelButton.TabIndex = 1;
            mdd.CancelButton.Content = FindResource("cancel").ToString();
            mdd.TabIndex = -1;
            mdd.Height = 200;
            mdd.Title = FindResource("save_invoice").ToString();
            mdd.Content = FindResource("really_want_save_invoice").ToString();
            mdd.OkButton.Focus();
            mdd.ShowDialog();

            if (mdd.MessageBoxResult == System.Windows.MessageBoxResult.OK)
            {
                if (thread_saveinvoice != null && thread_saveinvoice.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_saveinvoice = new Thread(() =>
                    {
                        try
                        {
                            this.btnSaveInvoice.Dispatcher.Invoke((Action)(() => { this.btnSaveInvoice.Visibility = System.Windows.Visibility.Hidden; }));
                            this.btnSaveSendEmail.Dispatcher.Invoke((Action)(() => { this.btnSaveSendEmail.IsEnabled = false; }));
                            this.btnClose.Dispatcher.Invoke((Action)(() => { this.btnClose.IsEnabled = false; }));

                            this.mprSaveInvoice.Dispatcher.Invoke((Action)(() =>
                            {
                                this.mprSaveInvoice.Visibility = System.Windows.Visibility.Visible;
                                this.mprSaveInvoice.IsActive = true;
                            }));

                            if (PayOrder() == true)
                            {
                                StaticClass.GeneralClass.flag_paycash = true;

                                btnpayother_delegate(true);
                                this.Dispatcher.Invoke((Action)(() => { this.Close(); }));
                            }
                            else
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    ModernDialog md = new ModernDialog();
                                    md.CloseButton.Content = FindResource("close").ToString();
                                    md.Content = FindResource("error_insert").ToString();
                                    md.Title = FindResource("notification").ToString();
                                    md.ShowDialog();
                                }));
                            }

                            this.mprSaveInvoice.Dispatcher.Invoke((Action)(() =>
                            {
                                this.mprSaveInvoice.Visibility = System.Windows.Visibility.Hidden;
                                this.mprSaveInvoice.IsActive = false;

                                this.btnSaveInvoice.Visibility = System.Windows.Visibility.Visible;
                                this.btnSaveSendEmail.IsEnabled = true;
                                this.btnClose.IsEnabled = true;
                            }));
                        }
                        catch (Exception ex)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                ModernDialog md = new ModernDialog();
                                md.CloseButton.Content = FindResource("close").ToString();
                                md.Content = ex.Message;
                                md.Title = FindResource("notification").ToString();
                                md.ShowDialog();
                            }));
                        }
                    });
                    thread_saveinvoice.Start();
                }
            }
        }

        //GetPayment
        private void GetPayment()
        {
            list_ec_payment.Clear();
            int no = 0;
            foreach (DataRow dr in bus_tb_payment.GetPayment("").Rows)
            {
                if(Convert.ToInt32(dr["PaymentID"].ToString()) != 1)
                {
                    EC_tb_Payment ec_payment = new EC_tb_Payment();
                    ec_payment.No = ++no;
                    ec_payment.PaymentID = Convert.ToInt32(dr["PaymentID"].ToString());
                    ec_payment.Card = dr["Card"].ToString();
                    ec_payment.RdoCard = new RadioButton();
                    if (no == 1)
                        ec_payment.RdoCard.IsChecked = true;
                    stpCard.Children.Add(ec_payment.RdoCard);
                    ec_payment.SepCard = new Separator();
                    stpCard.Children.Add(ec_payment.SepCard);
                    list_ec_payment.Add(ec_payment);
                }
            }
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            total = StaticClass.GeneralClass.subtotal_general + StaticClass.GeneralClass.totaltaxrate_general - StaticClass.GeneralClass.totaldiscount_general;
            btnSaveInvoice.Focus();
        }

        //btnSaveSendEmail_Click
        private void btnSaveSendEmail_Click(object sender, RoutedEventArgs e)
        {
            ModernDialog mdd = new ModernDialog();
            mdd.Buttons = new Button[] { mdd.OkButton, mdd.CancelButton, };
            mdd.OkButton.TabIndex = 0;
            mdd.OkButton.Content = FindResource("ok").ToString();
            mdd.CancelButton.TabIndex = 1;
            mdd.CancelButton.Content = FindResource("cancel").ToString();
            mdd.TabIndex = -1;
            mdd.Height = 200;
            mdd.Title = FindResource("save_invoice").ToString();
            mdd.Content = FindResource("really_want_save_invoice").ToString();
            mdd.OkButton.Focus();
            mdd.ShowDialog();

            if (mdd.MessageBoxResult == System.Windows.MessageBoxResult.OK)
            {
                if (thread_savesendemail != null && thread_savesendemail.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_savesendemail = new Thread(() =>
                    {
                        try
                        {
                            this.btnSaveSendEmail.Dispatcher.Invoke((Action)(() => { this.btnSaveSendEmail.Visibility = System.Windows.Visibility.Hidden; }));
                            this.btnSaveInvoice.Dispatcher.Invoke((Action)(() => { this.btnSaveInvoice.IsEnabled = false; }));
                            this.btnClose.Dispatcher.Invoke((Action)(() => { this.btnClose.IsEnabled = false; }));

                            this.mprSaveSendEmail.Dispatcher.Invoke((Action)(() =>
                            {
                                this.mprSaveSendEmail.Visibility = System.Windows.Visibility.Visible;
                                this.mprSaveSendEmail.IsActive = true;
                            }));

                            if (PayOrder() == true)
                            {
                                StaticClass.GeneralClass.flag_paycash = true;
                                btnpayother_delegate(true);

                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    SendEmail page = new SendEmail();
                                    page.ShowInTaskbar = false;
                                    var m = Application.Current.MainWindow;
                                    page.Owner = m;
                                    if (m.RenderSize.Width > StaticClass.GeneralClass.width_screen_general)
                                        page.Width = StaticClass.GeneralClass.width_screen_general * 90 / 100;

                                    if (m.RenderSize.Height > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Bottom)
                                        page.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Bottom;

                                    else
                                    {
                                        page.Height = m.RenderSize.Height;
                                        page.Width = m.RenderSize.Width * 90 / 100;
                                    }

                                    page.orderid = bus_tb_order.GetMaxOrderID("");
                                    page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                    page.ShowDialog();
                                }));

                                this.Dispatcher.Invoke((Action)(() => { this.Close(); }));
                            }
                            else
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    ModernDialog md = new ModernDialog();
                                    md.CloseButton.Content = FindResource("close").ToString();
                                    md.Content = FindResource("error_insert").ToString();
                                    md.Title = FindResource("notification").ToString();
                                    md.ShowDialog();
                                }));
                            }

                            this.mprSaveSendEmail.Dispatcher.Invoke((Action)(() =>
                            {
                                this.mprSaveSendEmail.Visibility = System.Windows.Visibility.Hidden;
                                this.mprSaveSendEmail.IsActive = false;
                            }));
                            this.btnSaveSendEmail.Dispatcher.Invoke((Action)(() => { this.btnSaveInvoice.Visibility = System.Windows.Visibility.Visible; }));
                            this.btnSaveInvoice.Dispatcher.Invoke((Action)(() => { this.btnSaveInvoice.IsEnabled = true; }));
                            this.btnClose.Dispatcher.Invoke((Action)(() => { this.btnClose.IsEnabled = true; }));
                        }
                        catch (Exception ex)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                ModernDialog md = new ModernDialog();
                                md.CloseButton.Content = FindResource("close").ToString();
                                md.Content = ex.Message;
                                md.Title = FindResource("notification").ToString();
                                md.ShowDialog();
                            }));
                        }
                    });
                    thread_savesendemail.Start();
                }
            }
        }

        //PayOrder
        private bool PayOrder()
        {
            int quantity = StaticClass.GeneralClass.list_ec_tb_orderdetail_general.Count;
            bool result = false;

            if (quantity > 0)
            {
                //create Order
                EC_tb_Order ec_tb_order = new EC_tb_Order();
                ec_tb_order.CustomerID = StaticClass.GeneralClass.customerid_general;
                ec_tb_order.CustomerName = StaticClass.GeneralClass.customername_general;
                ec_tb_order.Quatity = quantity;
                ec_tb_order.OrderDate = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                ec_tb_order.SalesPersonID = StaticClass.GeneralClass.salespersonid_login_general;
                ec_tb_order.SalesPersonName = StaticClass.GeneralClass.salespersonname_login_general;

                this.Dispatcher.Invoke((Action)(() =>
                {
                    ec_tb_order.PaymentID = list_ec_payment.Find(x => x.RdoCard.IsChecked.Value == true).PaymentID;
                    ec_tb_order.PaymentName = list_ec_payment.Find(x => x.RdoCard.IsChecked.Value == true).Card;
                }));
                ec_tb_order.DiscountType = StaticClass.GeneralClass.discounttype_general;
                ec_tb_order.Discount = StaticClass.GeneralClass.discount_general;
                ec_tb_order.TotalDiscount = StaticClass.GeneralClass.totaldiscount_general;
                ec_tb_order.TotalTax = StaticClass.GeneralClass.totaltaxrate_general;
                ec_tb_order.TotalAmount = total;

                if (bus_tb_order.InsertOrder(ec_tb_order, StaticClass.GeneralClass.flag_database_type_general) == 1)
                {
                    int num_orderdetail_inserted = 0;

                    foreach (EC_tb_OrderDetail ec_tb_orderdetail in StaticClass.GeneralClass.list_ec_tb_orderdetail_general)
                    {
                        ec_tb_order.OrderID = bus_tb_order.GetMaxOrderID("");
                        ec_tb_orderdetail.OrderID = ec_tb_order.OrderID;

                        if (bus_tb_orderdetail.InsertOrderDetail(ec_tb_orderdetail, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        {
                            num_orderdetail_inserted++;

                            //update InventoryCount for Product
                            EC_tb_Product ec_tb_product = new EC_tb_Product();
                            ec_tb_product.ProductID = ec_tb_orderdetail.ProductID;
                            ec_tb_product.InventoryCount = ec_tb_orderdetail.Qty;
                            bus_tb_product.UpdateInventoryCount(ec_tb_product);
                        }
                    }
                    //if insert orderdetai success
                    if (num_orderdetail_inserted > 0)
                        result = true;

                    //if insert orderdetail failed
                    else
                    {
                        if (bus_tb_order.DeleteOrder(ec_tb_order) == 1)
                            result = false;
                    }
                }
            }
            return result;
        }

        //btnClose_Click
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
