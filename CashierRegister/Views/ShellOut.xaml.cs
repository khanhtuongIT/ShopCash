using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using CashierRegister.Pages.Home;
using CashierRegister.StaticClass;
using CashierRegister.ViewModel;
using CashierRegister.Model;
using System.Globalization;
using CashierRegister.Helpers;

namespace CashierRegister.Views
{
    /// <summary>
    /// Interaction logic for ShellOut.xaml
    /// </summary>
    public partial class ShellOut : ModernDialog
    {
        private List<PayMoney> listPayTemplate = null;
        private bool flagStandardCal = true;

        private Thread thread_saveinvoice = null;
        private Thread thread_savesendemail = null;

        public delegate void btnPayCash_Click_Delegate(bool flag_paycash);
        public event btnPayCash_Click_Delegate btnpaycash_delegate;

        private BUS_tb_Payment bus_tb_payment = new BUS_tb_Payment();
        //private int paymentid = 0;

        //private string paymentname = "";
        public int orderid = 0;

        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();

        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //using for orderdetail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //using for invoice
        private decimal total = 0;
        //private decimal paycash = 0;
        public ShellOut()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
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
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += OnFocusRequested;
        }
        //mnuiStandard_Click
        private void mnuiStandard_Click(object sender, RoutedEventArgs e)
        {
            tblCalType.Text = FindResource("standard").ToString().ToUpper();
            column1.Width = new GridLength(0, GridUnitType.Star);
            column2.Width = new GridLength(1, GridUnitType.Star);
            //grdMain.Width = 420;
            grdPayTemplate.Visibility = Visibility.Hidden;
            grdPayNum.Margin = new Thickness(25, 0, 25, 0);
            tblstandardIcon.Visibility = Visibility.Visible;
            tblExtendIcon.Visibility = Visibility.Hidden;
            StaticClass.GeneralClass.app_settings["CalStandard"] = 1;
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
        }

        //mnuiExtend_Click
        private void mnuiExtend_Click(object sender, RoutedEventArgs e)
        {
            tblCalType.Text = FindResource("extend").ToString().ToUpper();
            column1.Width = new GridLength(2, GridUnitType.Star);
            column2.Width = new GridLength(3, GridUnitType.Star);
            //grdMain.Width = 700;
            grdPayTemplate.Visibility = Visibility.Visible;
            grdPayNum.Margin = new Thickness(50, 0, 0, 0);
            tblstandardIcon.Visibility = Visibility.Hidden;
            tblExtendIcon.Visibility = Visibility.Visible;
            StaticClass.GeneralClass.app_settings["CalStandard"] = 2;
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
        }
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
        private void btnMoney_Click(object sender, RoutedEventArgs e)
        {
            if (lstShellOut.SelectedIndex > -1) {
                ShellOutModel item = (ShellOutModel)lstShellOut.SelectedItem;
                string _strId = item.PaymentId.ToString();
                if (_strId.Length >= 3)
                {
                    _strId = _strId.Remove(2);
                }
                if (Convert.ToInt32(_strId) == 11)
                {
                    return;
                }
                ShellOutModel _default = (ShellOutModel)lstShellOut.Items[0];
                string _strval = ((PayMoney)(sender as Button).DataContext).Money.ToString();
                decimal _tempVal = CalculateTotalBalance(Convert.ToInt16(item.PaymentId.ToString()), Convert.ToDecimal(_strval));
                decimal tmpCompare = (_default.PaymentId == item.PaymentId) ? Convert.ToDecimal(_strval) : StaticClass.GeneralClass.ConverStringToDecimal(_default.PaymentBalance);
                if (_tempVal > tmpCompare)
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.FindResource("close").ToString();
                    md.Content = App.Current.FindResource("payment_balance_less_cash_balance").ToString();
                    md.Title = App.Current.FindResource("notification").ToString();
                    md.ShowDialog();
                }
                else item.PaymentBalance = StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(_strval));
            }
            isEnableInvoice();
        }
        private decimal CalculateTotalBalance(int _id, decimal _val)
        {
            decimal _balane = 0;
            foreach (var _selectedShell in lstShellOut.Items)
            {
                ShellOutModel _shellOut = (ShellOutModel)_selectedShell;
                if(_val < 0)
                {
                    _balane += (_shellOut.PaymentId == _id) ? 0 : StaticClass.GeneralClass.ConverStringToDecimal(_shellOut.PaymentBalance);
                }
                else
                {
                    _balane += (_shellOut.PaymentId == _id) ? _val : StaticClass.GeneralClass.ConverStringToDecimal(_shellOut.PaymentBalance);
                }
            }
            decimal lastBalance = _balane - total;
            return StaticClass.GeneralClass.ConverStringToDecimal(StaticClass.GeneralClass.GetNumFormatDisplay(lastBalance));
        }
        private void btnNum_Click(object sender, RoutedEventArgs e)
        {
            ShellOutModel _shell = (ShellOutModel)lstShellOut.SelectedItem;
            string _strId = _shell.PaymentId.ToString();
            if (_strId.Length >= 3)
            {
                _strId = _strId.Remove(2);
            }
            if (Convert.ToInt32(_strId) == 11)
            {
                return;
            }
            ShellOutModel _default = (ShellOutModel)lstShellOut.Items[0];
            string _strVal = _shell.PaymentBalance + (sender as Button).Uid;
            string _strCompare = Convert.ToString(StaticClass.GeneralClass.ConverStringToDecimal(_strVal));
            string strVal = _shell.PaymentBalance;
            if (Convert.ToDouble(_strCompare) < 999999999.99)
            {
                string _strSeparator = (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0") ? "." : ",";
                int ind = strVal.IndexOf(_strSeparator);
                decimal _totalBalance = StaticClass.GeneralClass.ConverStringToDecimal(tblCashBalance.Text);
                if (_shell.PaymentId == 1)
                {
                    if (strVal != "0")
                    {
                        if (ind > 0)
                        {
                            string _strNumberDecimalSeparator = strVal.Substring(ind + 1);
                            if (_strNumberDecimalSeparator.Length < 2)
                            {
                                strVal += (sender as Button).Uid;
                            }
                        }
                        else strVal += (sender as Button).Uid;
                    }
                    else if ((sender as Button).Uid != "0")
                    {
                        strVal = (sender as Button).Uid;
                    }

                    int _ind = strVal.IndexOf(_strSeparator);
                    if (_ind > 0)
                    {
                        string _strNumberDecimalSeparator = strVal.Substring(_ind + 1);
                        _shell.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(StaticClass.GeneralClass.ConverStringToDecimal(strVal), _strNumberDecimalSeparator.Length);
                    }
                    else _shell.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(StaticClass.GeneralClass.ConverStringToDecimal(strVal), 0);
                    isEnableInvoice();
                }
                else if (CalculateTotalBalance(_shell.PaymentId, StaticClass.GeneralClass.ConverStringToDecimal(_strVal)) <= StaticClass.GeneralClass.ConverStringToDecimal(_default.PaymentBalance))
                {
                    if (strVal != "0")
                    {
                        if (ind > 0)
                        {
                            string _strNumberDecimalSeparator = strVal.Substring(ind + 1);
                            if (_strNumberDecimalSeparator.Length < 2)
                            {
                                strVal += (sender as Button).Uid;
                            }
                        }
                        else strVal += (sender as Button).Uid;
                    }
                    else if ((sender as Button).Uid != "0")
                    {
                        strVal = (sender as Button).Uid;
                    }

                    int _ind = strVal.IndexOf(_strSeparator);
                    if (_ind > 0)
                    {
                        string _strNumberDecimalSeparator = strVal.Substring(_ind + 1);
                        _shell.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(StaticClass.GeneralClass.ConverStringToDecimal(strVal), _strNumberDecimalSeparator.Length);
                    }
                    else _shell.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(StaticClass.GeneralClass.ConverStringToDecimal(strVal), 0);
                    isEnableInvoice();
                }
                else
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.FindResource("close").ToString();
                    md.Content = App.Current.FindResource("payment_balance_less_cash_balance").ToString();
                    md.Title = App.Current.FindResource("notification").ToString();
                    md.ShowDialog();
                }
            }
        }
        private void btnpoint_Click(object sender, RoutedEventArgs e)
        {
            ShellOutModel _shell = (ShellOutModel)lstShellOut.SelectedItem;
            var strPoint = (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0") ? "." : ",";
            if (!_shell.PaymentBalance.Trim().Contains(strPoint))
                _shell.PaymentBalance += strPoint;
            isEnableInvoice();
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ShellOutModel _shell = (ShellOutModel)lstShellOut.SelectedItem;
            _shell.PaymentBalance = "0";
            buttonAddRemovePayment_Click(null, null);
            isEnableInvoice();
        }
        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            ShellOutModel _shell = (ShellOutModel)lstShellOut.SelectedItem;
            string _strId = _shell.PaymentId.ToString();
            if (_strId.Length >= 3)
            {
                _strId = _strId.Remove(2);
            }
            if (Convert.ToInt32(_strId) == 11) return;
            if(_shell.PaymentBalance.Trim().Length == 1)
            {
                _shell.PaymentBalance = "0";
            }
            else
            {
                string _strSeparator = (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0") ? "." : ",";
                string _tmpVal = _shell.PaymentBalance.Remove(_shell.PaymentBalance.Length - 1, 1);
                decimal _tmpDec = StaticClass.GeneralClass.ConverStringToDecimal(_tmpVal);
                _tmpVal = Convert.ToString(_tmpDec);
                int _ind = _tmpVal.IndexOf(_strSeparator);
                if (_ind > 0)
                {
                    string _strNumberDecimalSeparator = _tmpVal.Substring(_ind + 1);
                    _shell.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(_tmpDec, _strNumberDecimalSeparator.Length);
                }
                else _shell.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(_tmpDec, 0);
            }
            buttonAddRemovePayment_Click(null, null);
            isEnableInvoice();
        }
        private void btnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            string _strBalance = (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0") ? tblCashBalance.Text.Replace(", ", "") : tblCashBalance.Text.Replace(".", "");
            if (Convert.ToDecimal(_strBalance.Trim(), StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR")) < 0)
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
            string _strBalane = (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0") ? tblCashBalance.Text.Replace(", ", "") : tblCashBalance.Text.Replace(".", "");
            if (Convert.ToDecimal(_strBalane.Trim(), StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0" ? new CultureInfo("en-US") : new CultureInfo("fr-FR")) < 0)
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
                ec_tb_order.OrderDate = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                ec_tb_order.SalesPersonID = StaticClass.GeneralClass.salespersonid_login_general;
                ec_tb_order.SalesPersonName = StaticClass.GeneralClass.salespersonname_login_general;
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ShellOutModel shellPayMent = (ShellOutModel)lstShellOut.Items[0];
                    ec_tb_order.PaymentID = Convert.ToInt16(shellPayMent.PaymentId); //paymentid;
                    ec_tb_order.PaymentName = shellPayMent.PaymentName; //paymentname;
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
                            ShellOutViewModel.eventSavePaymentOther(ec_tb_order.OrderID, null);
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
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            //lstShellOut.SelectedIndex = 0;
            total = StaticClass.GeneralClass.subtotal_general + StaticClass.GeneralClass.totaltaxrate_general - StaticClass.GeneralClass.totaldiscount_general;
            tblTotal.Text = StaticClass.GeneralClass.GetNumFormatDisplay(total);
            tblCashBalance.Text = StaticClass.GeneralClass.GetNumFormatDisplay(-total);
            tblCashBalanceUnit.Text = StaticClass.GeneralClass.currency_setting_general;
            tblTotalUnit.Text = StaticClass.GeneralClass.currency_setting_general;
            contentTotal.Content = StaticClass.GeneralClass.GetNumFormatDisplay(total);
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            decimal cashblane = 0;
            for (int i = 0; i < lstShellOut.Items.Count; i++)
            {
                ShellOutModel msShellout = (ShellOutModel)lstShellOut.Items[i];
                cashblane = cashblane + Convert.ToDecimal(msShellout.PaymentBalance);
            }
            cashblane = cashblane - Convert.ToDecimal(tblTotal.Text);
            tblCashBalance.Text = StaticClass.GeneralClass.GetNumFormatDisplay(cashblane);
            if (cashblane >= 0)
            {
                btnSaveInvoice.IsEnabled = true;
                btnSaveSendEmail.IsEnabled = true;
            }
            return;
        }

        private void muiBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnSaveInvoice.IsEnabled = false;
            btnSaveSendEmail.IsEnabled = false;
        }

        private void btRemovePayment_Click(object sender, RoutedEventArgs e)
        {
            btnSaveInvoice.IsEnabled = false;
            btnSaveSendEmail.IsEnabled = false;
        }
        private void buttonAddRemovePayment_Click(object sender, RoutedEventArgs e)
        {
            btnSaveInvoice.IsEnabled = false;
            btnSaveSendEmail.IsEnabled = false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            StaticClass.GeneralClass.lstCash.Clear();
            StaticClass.GeneralClass.customerGiftCard.Clear();
            StaticClass.GeneralClass.customerGiftValue.Clear();
            this.Close();
        }
        private void isEnableInvoice()
        {
            decimal _balane = 0;
            foreach (var _selectedShell in lstShellOut.Items)
            {
                ShellOutModel _shellOut = (ShellOutModel)_selectedShell;
                _balane += StaticClass.GeneralClass.ConverStringToDecimal(_shellOut.PaymentBalance);
            }
            decimal lastBalance = StaticClass.GeneralClass.ConverStringToDecimal(StaticClass.GeneralClass.GetNumFormatDisplay(_balane - total));
            tblCashBalance.Text = StaticClass.GeneralClass.GetNumFormatDisplay(lastBalance);
            if (_balane >= 0 && lastBalance >= 0)
            {
                btnSaveInvoice.IsEnabled = true;
                btnSaveSendEmail.IsEnabled = true;
                mainButtonAdd.Visibility = Visibility.Hidden;
                /*
                 * muiBtnAdd.Visibility = Visibility.Hidden;
                 * btRemovePayment.Visibility = Visibility.Hidden;
                 */
                btnSaveInvoice.Focus();
            }
            else
            {
                buttonAddRemovePayment_Click(null, null);
                mainButtonAdd.Visibility = Visibility.Visible;
                /*
                 * muiBtnAdd.Visibility = Visibility.Hidden;
                 * btRemovePayment.Visibility = Visibility.Visible;
                 */
                btnClose.Focus();
            }
        }
        //ModernDialog_KeyDown
        private void ModernDialog_KeyDown(object sender, KeyEventArgs e)
        {
            ShellOutModel msShellout = (ShellOutModel)lstShellOut.Items[lstShellOut.SelectedIndex];
            string _strId = msShellout.PaymentId.ToString();
            if (_strId.Length >= 3)
            {
                _strId = _strId.Remove(2);
            }
            if (Convert.ToInt32(_strId) == 11)
            {
                return;
            }
            ShellOutModel _default = (ShellOutModel)lstShellOut.Items[0];
            string _strSeparator = (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0") ? "." : ",";
            string strVal = msShellout.PaymentBalance.ToString();
            string strkey = GetCharFromKey(e.Key).ToString();
            if (GetCharFromKey(e.Key).ToString() == _strSeparator)
            {
                if (!msShellout.PaymentBalance.Trim().Contains(strkey))
                    strVal += GetCharFromKey(e.Key).ToString();
                msShellout.PaymentBalance = strVal;
                isEnableInvoice();
            }
            try
            {
                int num;
                if (int.TryParse(GetCharFromKey(e.Key).ToString(), out num))
                {
                    string _strCompare = Convert.ToString(StaticClass.GeneralClass.ConverStringToDecimal(strVal)) + strkey;
                    if (Convert.ToDouble(_strCompare) < 999999999.99)
                    {
                        if (msShellout.PaymentId == 1)
                        {
                            if (StaticClass.GeneralClass.ConverStringToDecimal(strVal)==0)
                            {
                                strVal = GetCharFromKey(e.Key).ToString();
                            }
                            else
                            {
                                int ind = strVal.IndexOf(_strSeparator);
                                if (ind > 0)
                                {
                                    string _strNumberDecimalSeparator = strVal.Substring(ind + 1);
                                    if (_strNumberDecimalSeparator.Length < 2)
                                    {
                                        strVal += GetCharFromKey(e.Key).ToString();
                                    }
                                }
                                else strVal += GetCharFromKey(e.Key).ToString();
                            }
                            int _ind = strVal.IndexOf(_strSeparator);
                            if (_ind > 0)
                            {
                                string _strNumberDecimalSeparator = strVal.Substring(_ind + 1);
                                msShellout.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(StaticClass.GeneralClass.ConverStringToDecimal(strVal), _strNumberDecimalSeparator.Length);
                            }
                            else msShellout.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(StaticClass.GeneralClass.ConverStringToDecimal(strVal), 0);
                            isEnableInvoice();
                        }
                        else if (CalculateTotalBalance(msShellout.PaymentId, StaticClass.GeneralClass.ConverStringToDecimal(strVal + strkey)) <= StaticClass.GeneralClass.ConverStringToDecimal(_default.PaymentBalance))
                        {
                            if (StaticClass.GeneralClass.ConverStringToDecimal(strVal)==0)
                            {
                                strVal = GetCharFromKey(e.Key).ToString();
                            }
                            else
                            {
                                int ind = strVal.IndexOf(_strSeparator);
                                if (ind > 0)
                                {
                                    string _strNumberDecimalSeparator = strVal.Substring(ind + 1);
                                    if (_strNumberDecimalSeparator.Length < 2)
                                    {
                                        strVal += GetCharFromKey(e.Key).ToString();
                                    }
                                }
                                else strVal += GetCharFromKey(e.Key).ToString();
                            }
                            int _ind = strVal.IndexOf(_strSeparator);
                            if (_ind > 0)
                            {
                                string _strNumberDecimalSeparator = strVal.Substring(_ind + 1);
                                msShellout.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(StaticClass.GeneralClass.ConverStringToDecimal(strVal), _strNumberDecimalSeparator.Length);
                            }
                            else msShellout.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(StaticClass.GeneralClass.ConverStringToDecimal(strVal), 0);
                            isEnableInvoice();
                        }
                        else
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.FindResource("close").ToString();
                            md.Content = App.Current.FindResource("payment_balance_less_cash_balance").ToString();
                            md.Title = App.Current.FindResource("notification").ToString();
                            md.ShowDialog();
                        }
                    }
                }
                //enter key press
                if (e.Key.ToString() == "Return")
                {
                    //btnEnter_Click(null, null);
                }

                //back key press
                if (e.Key.ToString() == "Back")
                {
                    string _strVal = msShellout.PaymentBalance.ToString();
                    if (_strVal.Length == 1)
                    {
                        msShellout.PaymentBalance = "0";
                    }
                    else
                    {
                        _strVal = _strVal.Remove(_strVal.Length - 1, 1);
                        decimal _tmpDec = StaticClass.GeneralClass.ConverStringToDecimal(_strVal);
                        _strVal = Convert.ToString(_tmpDec);
                        int _ind = _strVal.IndexOf(_strSeparator);
                        if (_ind > 0)
                        {
                            string _strNumberDecimalSeparator = _strVal.Substring(_ind + 1);
                            msShellout.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(_tmpDec, _strNumberDecimalSeparator.Length);
                        }
                        else msShellout.PaymentBalance = StaticClass.GeneralClass.FormatNumberDisplay(_tmpDec, 0);
                    }
                    isEnableInvoice();
                }

            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
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
        private void OnFocusRequested(object sender, FocusRequestedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ButtonInvoice":
                    btnSaveInvoice.Focus();
                    break;
                case "ButtonEmailinvoice":
                    btnSaveSendEmail.Focus();
                    break;
                default:
                    btnClose.Focus();
                    break;
            }
        }
    }
}
