using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;
using System.Threading;
using CashierRegister.StaticClass;
using System.Globalization;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for PayCash.xaml
    /// </summary>
    public partial class PayCash : ModernDialog
    {
        private List<PayMoney> listPayTemplate = null;
        private bool flagStandardCal = true;

        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();

        //using for payment
        private BUS_tb_Payment bus_tb_payment = new BUS_tb_Payment();
        private int paymentid = 0;
        private string paymentname = "";
        public int orderid = 0;

        //using for order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //using for orderdetail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //using for invoice
        private decimal total = 0;
        private decimal paycash = 0;

        //thread
        private Thread thread_saveinvoice = null;
        private Thread thread_savesendemail = null;

        //delegate
        public delegate void btnPayCash_Click_Delegate(bool flag_paycash);
        public event btnPayCash_Click_Delegate btnpaycash_delegate;

        //PayCash
        public PayCash()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { };

            //setting decimal separator
            if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "1")
                btnpoint.Content = ",";

            //setting standard calculator
            if (StaticClass.GeneralClass.app_settings["CalStandard"].ToString() == "1")
                mnuiStandard_Click(null, null);
            else
                mnuiExtend_Click(null, null);

            //create pay template
            new Thread(() =>
            {
                this.Dispatcher.Invoke((Action)(() => { CreatePayTemplate(); }));
            }).Start();
        }

        //CreatePayTemplate
        private void CreatePayTemplate()
        {
            listPayTemplate = new List<PayMoney>()
            {
                new PayMoney(GeneralClass.currency_setting_general, 10, 0, 0, grdPayTemplate, new Thickness(0, 0, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 30, 1, 0, grdPayTemplate, new Thickness(0, 1, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 50, 2, 0, grdPayTemplate, new Thickness(0, 1, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 70, 3, 0, grdPayTemplate, new Thickness(0, 1, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 90, 4, 0, grdPayTemplate, new Thickness(0, 1, 1, 0), new RoutedEventHandler(btnMoney_Click)),

                new PayMoney(GeneralClass.currency_setting_general, 15, 0, 1, grdPayTemplate, new Thickness(1, 0, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 35, 1, 1, grdPayTemplate, new Thickness(1, 1, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 55, 2, 1, grdPayTemplate, new Thickness(1, 1, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 75, 3, 1, grdPayTemplate, new Thickness(1, 1, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 95, 4, 1, grdPayTemplate, new Thickness(1, 1, 1, 0), new RoutedEventHandler(btnMoney_Click)),

                new PayMoney(GeneralClass.currency_setting_general, 20, 0, 2, grdPayTemplate, new Thickness(1, 0, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 40, 1, 2, grdPayTemplate, new Thickness(1, 1, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 60, 2, 2, grdPayTemplate, new Thickness(1, 1, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 80, 3, 2, grdPayTemplate, new Thickness(1, 1, 1, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 100, 4, 2, grdPayTemplate, new Thickness(1, 1, 0, 0), new RoutedEventHandler(btnMoney_Click)),

                new PayMoney(GeneralClass.currency_setting_general, 25, 0, 3, grdPayTemplate, new Thickness(1, 0, 0, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 45, 1, 3, grdPayTemplate, new Thickness(1, 1, 0, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 65, 2, 3, grdPayTemplate, new Thickness(1, 1, 0, 1), new RoutedEventHandler(btnMoney_Click)),
                new PayMoney(GeneralClass.currency_setting_general, 85, 3, 3, grdPayTemplate, new Thickness(1, 1, 0, 1), new RoutedEventHandler(btnMoney_Click)),
            };
        }

        //btnMoney_Click
        private void btnMoney_Click(object sender, RoutedEventArgs e)
        {
            flagStandardCal = false;
            tblPayCash.Text = ((PayMoney)(sender as Button).DataContext).Money.ToString();
            tblCashBalance.Text = StaticClass.GeneralClass.GetNumFormatEdit(0);
            btnEnter.IsEnabled = true;
            btnSaveInvoice.IsEnabled = false;
            btnSaveSendEmail.IsEnabled = false;
            btntemp.Focus();
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //GetPayment
                GetPayment(1);

                //disable btnSaveInvoice
                btnEnter.IsEnabled = false;
                btnSaveInvoice.IsEnabled = false;
                btnSaveSendEmail.IsEnabled = false;

                total = StaticClass.GeneralClass.subtotal_general + StaticClass.GeneralClass.totaltaxrate_general - StaticClass.GeneralClass.totaldiscount_general;
                tblTotalUnit.Text = StaticClass.GeneralClass.currency_setting_general;
                tblTotal.Text = StaticClass.GeneralClass.GetNumFormatDisplay(total);

                tblPayCashUnit.Text = StaticClass.GeneralClass.currency_setting_general;
                tblPayCash.Text = "0";

                tblCashBalanceUnit.Text = StaticClass.GeneralClass.currency_setting_general;
                tblCashBalance.Text = StaticClass.GeneralClass.GetNumFormatEdit(0);
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //btnBackspace_Click
        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            BackspacePress();
        }

        //btnClear_Click
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tblPayCash.Text = "0";
            tblCashBalance.Text = StaticClass.GeneralClass.GetNumFormatEdit(0);
            btnEnter.IsEnabled = false;
            btnSaveInvoice.IsEnabled = false;
            btnSaveSendEmail.IsEnabled = false;
        }

        //btnNum_Click
        private void btnNum_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Uid)
            {
                case "0":
                    Paycash("0", true);
                    break;
                case "1":
                    Paycash("1", true);
                    break;
                case "2":
                    Paycash("2", true);
                    break;
                case "3":
                    Paycash("3", true);
                    break;
                case "4":
                    Paycash("4", true);
                    break;
                case "5":
                    Paycash("5", true);
                    break;
                case "6":
                    Paycash("6", true);
                    break;
                case "7":
                    Paycash("7", true);
                    break;
                case "8":
                    Paycash("8", true);
                    break;
                case "9":
                    Paycash("9", true);
                    break;
            }
        }

        //btnpoint_Click
        private void btnpoint_Click(object sender, RoutedEventArgs e)
        {
            if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0")
                Paycash(".", true);
            else
                Paycash(",", true);
        }

        //btnEnter_Click
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            EnterPress();
            btnSaveInvoice.Focus();
            btnEnter.IsEnabled = false;
        }

        //btntemp_Click
        private void btntemp_Click(object sender, RoutedEventArgs e)
        {
            EnterPress();
            btnSaveInvoice.Focus();
        }

        //btnClose_Click
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Paycash
        private void Paycash(string num, bool isenable)
        {
            try
            {
                if (!flagStandardCal)
                {
                    flagStandardCal = true;
                    tblPayCash.Text = "0";
                }

                btnEnter.IsEnabled = isenable;
                btnSaveInvoice.IsEnabled = false;
                btnSaveSendEmail.IsEnabled = false;
                this.tblCashBalance.Text = "00.00";

                if (num == "." || num == ",")
                {
                    if (tblPayCash.Text.Trim().Contains(".") || tblPayCash.Text.Trim().Contains(","))
                        return;
                }

                btntemp.Focus();

                string strTemp = "";
                strTemp = tblPayCash.Text.Trim();

                if (strTemp.Length == 1)
                {
                    if (strTemp == "0")
                    {
                        if (num == "0")
                            tblPayCash.Text = "0";
                        if (num == "." || num == ",")
                            tblPayCash.Text += num;
                        else
                            tblPayCash.Text = num;
                    }
                    else
                        tblPayCash.Text += num;
                }
                else
                {
                    string checkPoint = "";

                    checkPoint = strTemp.Substring(strTemp.Length - 1, 1);

                    if ((checkPoint == "." && num == ".") || (checkPoint == "," && num == ","))
                        tblPayCash.Text = strTemp;
                    else
                        tblPayCash.Text += num;
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //ModernDialog_KeyDown
        private void ModernDialog_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int num;
                if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0")
                {
                    if (int.TryParse(GetCharFromKey(e.Key).ToString(), out num) || GetCharFromKey(e.Key).ToString() == ".")
                        Paycash(GetCharFromKey(e.Key).ToString(), true);
                }
                else
                {
                    if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "1")
                    {
                        if (int.TryParse(GetCharFromKey(e.Key).ToString(), out num) || GetCharFromKey(e.Key).ToString() == ",")
                            Paycash(GetCharFromKey(e.Key).ToString(), true);
                    }
                }

                //enter key press
                if (e.Key.ToString() == "Return")
                {
                    if (EnterPress())
                    {
                        btnSaveInvoice.Focus();
                        btnSaveInvoice.IsEnabled = true;
                        btnSaveSendEmail.IsEnabled = true;
                    }
                }

                //back key press
                if (e.Key.ToString() == "Back")
                    BackspacePress();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //BackspacePress
        private void BackspacePress()
        {
            string paycash_temp = tblPayCash.Text.Trim().ToString();

            if (paycash_temp.Length > 1)
            {
                tblPayCash.Text = paycash_temp.Remove(paycash_temp.Length - 1, 1);
                tblCashBalance.Text = StaticClass.GeneralClass.GetNumFormatEdit(0);
            }
            else
            {
                tblPayCash.Text = "0";
                tblCashBalance.Text = StaticClass.GeneralClass.GetNumFormatEdit(0);
            }

            btnSaveInvoice.IsEnabled = false;
            btnSaveSendEmail.IsEnabled = false;
        }

        //enter press
        private bool EnterPress()
        {
            try
            {
                paycash = Convert.ToDecimal(tblPayCash.Text.Trim(), StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("fr-FR"));

                if (paycash > 0)
                {
                    //enable save invoice
                    btnSaveInvoice.IsEnabled = true;
                    btnSaveSendEmail.IsEnabled = true;

                    if (total > paycash)
                        tblCashBalance.Text = "-" + StaticClass.GeneralClass.GetNumFormatDisplay(total - paycash);
                    else
                        tblCashBalance.Text = StaticClass.GeneralClass.GetNumFormatDisplay(paycash - total);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
            return false;
        }

        //btnSaveInvoice_Click
        private void btnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDecimal(tblCashBalance.Text.Trim(), StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR")) < 0)
            {
                ModernDialog md = new ModernDialog();
                md.CloseButton.Content = FindResource("close").ToString();
                md.Title = FindResource("notification").ToString();
                md.Content = FindResource("paycash_greater_total").ToString();
                md.CloseButton.Focus();
                md.ShowDialog();
                return;
            }
            else
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
                                this.mprSaveInvoice.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.btnSaveInvoice.Visibility = System.Windows.Visibility.Hidden;
                                    this.btnSaveSendEmail.IsEnabled = false;
                                    this.btnClose.IsEnabled = false;
                                    this.mprSaveInvoice.Visibility = System.Windows.Visibility.Visible;
                                    this.mprSaveInvoice.IsActive = true;
                                }));

                                if (PayOrder() == true)
                                {
                                    StaticClass.GeneralClass.flag_paycash = true;

                                    btnpaycash_delegate(true);
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
                                    md.CloseButton.FindResource("close").ToString();
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
        }

        //btnSaveSendEmail_Click
        private void btnSaveSendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDecimal(tblCashBalance.Text.Trim(), StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR")) < 0)
            {
                ModernDialog md = new ModernDialog();
                md.CloseButton.Content = FindResource("close").ToString();
                md.Title = FindResource("notification").ToString();
                md.Content = FindResource("paycash_greater_total").ToString();
                md.CloseButton.Focus();
                md.ShowDialog();
                return;
            }
            else
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

                                    btnpaycash_delegate(true);

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
                                    md.Content = "Error: " + ex.Message;
                                    md.Title = FindResource("notification").ToString();
                                    md.ShowDialog();
                                }));
                            }
                        });
                        thread_savesendemail.Start();
                    }
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
                ec_tb_order.PaymentID = paymentid;
                ec_tb_order.PaymentName = paymentname;
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
                        //ec_tb_orderdetail.ID = bus_tb_orderdetail.GetMaxID("") + 1;
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

        //GetPayment
        private void GetPayment(int _paymentid)
        {
            DataTable dt_payment = bus_tb_payment.GetPayment("WHERE PaymentID = " + _paymentid);
            if (dt_payment.Rows.Count == 1)
            {
                paymentid = Convert.ToInt32(dt_payment.Rows[0]["PaymentID"].ToString());
                paymentname = dt_payment.Rows[0]["Card"].ToString();
            }
        }

        #region get char from key
        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [System.Runtime.InteropServices.Out, System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }
        #endregion

        //mnuiStandard_Click
        private void mnuiStandard_Click(object sender, RoutedEventArgs e)
        {
            tblCalType.Text = FindResource("standard").ToString().ToUpper();
            column1.Width = new GridLength(0, GridUnitType.Star);
            column2.Width = new GridLength(1, GridUnitType.Star);
            grdMain.Width = 420;
            grdPayNum.Margin = new Thickness(25, 0, 25, 0);
            tblstandardIcon.Visibility = Visibility.Visible;
            tblExtendIcon.Visibility = Visibility.Hidden;
            StaticClass.GeneralClass.app_settings["CalStandard"] = "1";
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
        }

        //mnuiExtend_Click
        private void mnuiExtend_Click(object sender, RoutedEventArgs e)
        {
            tblCalType.Text = FindResource("extend").ToString().ToUpper();
            column1.Width = new GridLength(2, GridUnitType.Star);
            column2.Width = new GridLength(3, GridUnitType.Star);
            grdMain.Width = 700;
            grdPayNum.Margin = new Thickness(50, 0, 0, 0);
            tblstandardIcon.Visibility = Visibility.Hidden;
            tblExtendIcon.Visibility = Visibility.Visible;
            StaticClass.GeneralClass.app_settings["CalStandard"] = "2";
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
        }
        private void btAddPayment_Click(object sender, RoutedEventArgs e)
        {
            Views.AddPaymentToPayCash page = new Views.AddPaymentToPayCash();
            page.btn_addPayment_delegate += btnAddPayment_Click_Delegate;
            page.ShowDialog();
        }
        private void btnAddPayment_Click_Delegate()
        {

        }
    }
}
