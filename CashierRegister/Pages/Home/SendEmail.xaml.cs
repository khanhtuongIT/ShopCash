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
using System.Data;
using System.Threading;
using CashierRegisterBUS;
using CashierRegisterEntity;
using System.Net.Mail;
using CashierRegister.Model;
using CashierRegister.ViewModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for SendEmail.xaml
    /// </summary>
    public partial class SendEmail : ModernWindow
    {
        //using for order detail
        private List<EC_tb_OrderDetail> list_ec_tb_orderdetail = new List<EC_tb_OrderDetail>();
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();
        private BUS_tb_Customer bus_tb_customer = new BUS_tb_Customer();
        public int orderid = 0;
        private string email_test = "cashregistertest@gmail.com";
        private string password_test = "TestGmail";

        //using for order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();
        private List<EC_tb_Order> list_ec_tb_order = new List<EC_tb_Order>();
        private Thread thread_calculatetotalpages = null;
        private Thread thread_image_paging = null;
        private Thread thread_order = null;
        private Thread thread_orderdetail = null;
        private int count_rows = 0;
        private int total_pages = 0;
        private int page_size = 100;
        private int current_page = 1;
        private string condition = "";

        //delegate
        public delegate void btnSendEmail_Click_Delegate();
        public event btnSendEmail_Click_Delegate btnsend_delegate;
        private string _orderTime = string.Empty;
        private string _orderDate = string.Empty;

        //SendEmail
        public SendEmail()
        {
            InitializeComponent();
            _orderTime = (!string.IsNullOrEmpty(StaticClass.GeneralClass.app_settings["timeFormat"].ToString())) ? " " + StaticClass.GeneralClass.timeFromatSettings[StaticClass.GeneralClass.app_settings["timeFormat"]].ToString() : "";
            System.Windows.Media.Imaging.BitmapImage bitmapimage = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/save.png", UriKind.Absolute));
            imgFromSave.Source = bitmapimage;
            imgPasswordSave.Source = bitmapimage;
            System.Windows.Media.Color accent_color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(StaticClass.GeneralClass.app_settings["accentColor"].ToString());
            SolidColorBrush brush = new SolidColorBrush(accent_color);
            gs.Background = brush;

            if (thread_image_paging != null && thread_image_paging.ThreadState == ThreadState.Running) { }
            else
            {
                thread_image_paging = new Thread(() =>
                {
                    try
                    {
                        EC_Image_Paging image_paging = new EC_Image_Paging();

                        image_paging.Be_Previous_First = @"pack://application:,,,/Resources/be_previous_first.png";
                        image_paging.Af_Previous_First = @"pack://application:,,,/Resources/af_previous_first.png";

                        image_paging.Be_Previous = @"pack://application:,,,/Resources/be_previous.png";
                        image_paging.Af_Previous = @"pack://application:,,,/Resources/af_previous.png";

                        image_paging.Be_Center = @"pack://application:,,,/Resources/be_center.png";
                        image_paging.Af_Center = @"pack://application:,,,/Resources/af_center.png";

                        image_paging.Be_Next = @"pack://application:,,,/Resources/be_next.png";
                        image_paging.Af_Next = @"pack://application:,,,/Resources/af_next.png";

                        image_paging.Be_Next_End = @"pack://application:,,,/Resources/be_next_end.png";
                        image_paging.Af_Next_End = @"pack://application:,,,/Resources/af_next_end.png";

                        this.stpPaging.Dispatcher.Invoke((Action)(() => { this.stpPaging.DataContext = image_paging; }));

                        this.imgSearchOrder.Dispatcher.Invoke((Action)(() =>
                        {
                            System.Uri uri_imgage_search_order = new Uri(@"pack://application:,,,/Resources/search.png", UriKind.Absolute);
                            System.Windows.Media.Imaging.BitmapImage bitmapimage_search_order = new System.Windows.Media.Imaging.BitmapImage();
                            bitmapimage_search_order.BeginInit();
                            bitmapimage_search_order.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapimage_search_order.UriSource = uri_imgage_search_order;
                            this.imgSearchOrder.Source = bitmapimage_search_order;
                            bitmapimage_search_order.EndInit();
                            bitmapimage_search_order.Freeze();
                        }));

                        this.imgColspan.Dispatcher.Invoke((Action)(() =>
                        {
                            System.Uri uri_colspan_imgage_search_order = new Uri(@"pack://application:,,,/Resources/colspan.png", UriKind.Absolute);
                            System.Windows.Media.Imaging.BitmapImage bitmapimage_colspan_search_order = new System.Windows.Media.Imaging.BitmapImage();
                            bitmapimage_colspan_search_order.BeginInit();
                            bitmapimage_colspan_search_order.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapimage_colspan_search_order.UriSource = uri_colspan_imgage_search_order;
                            this.imgColspan.Source = bitmapimage_colspan_search_order;
                            bitmapimage_colspan_search_order.EndInit();
                            bitmapimage_colspan_search_order.Freeze();
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_image_paging.Start();
            }

        }

        private string getPaymentMethodForOrder(int orderId)
        {
            DataTable method = OrderPayment.getOrderPaymentName(orderId);
            string strMethod = "";
            foreach(DataRow dr in method.Rows){
                strMethod += dr["Card"]+" = "+ StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(dr["Amount"].ToString())) + ";    ";
            }
            strMethod = (strMethod!="")?strMethod.Remove(strMethod.Length - 5, 5):"Cash";
            return strMethod;
        }
        //SendEmail_Loaded
        private void SendEmail_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //set recipient
                new Thread(() =>
                {
                    Application.Current.Resources["font_size_val"] = StaticClass.GeneralClass.app_settings["fontSize"].ToString();
                    Application.Current.Resources["order_width_val"] = StaticClass.GeneralClass.app_settings["unitVal"].ToString();

                    //check email account
                    if (String.IsNullOrWhiteSpace(StaticClass.GeneralClass.app_settings["emailAcc"].ToString()) || (StaticClass.GeneralClass.app_settings["emailAcc"].ToString() == email_test && StaticClass.GeneralClass.app_settings["emailPass"].ToString() != password_test))
                    {
                        StaticClass.GeneralClass.app_settings["emailAcc"] = email_test;
                        StaticClass.GeneralClass.app_settings["emailPass"] = password_test;
                        Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
                    }

                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        txbTo.Text = StaticClass.GeneralClass.customeremail_general;
                        txbFrom.Text = StaticClass.GeneralClass.app_settings["emailAcc"].ToString();
                        this.pwbPasssword.Password = StaticClass.GeneralClass.app_settings["emailPass"].ToString();
                    }));

                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        tblStoreName.Text = StaticClass.GeneralClass.app_settings["storeName"].ToString();
                        tblStoreAddress.Text = StaticClass.GeneralClass.app_settings["storeAddress"].ToString();
                        tblStorePhone.Text = FindResource("phone").ToString() + ": " + StaticClass.GeneralClass.app_settings["storePhone"].ToString();
                        tblPayment.Text = "";
                    }));
                }).Start();

                LoadData("");
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        //LoadData
        private void LoadData(string str_con)
        {
            CalculateTotalPages(str_con);
            GetListOrder(condition);
            new Thread(() =>
            {
                //set status for paging image
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.imgPreviousFirst.IsEnabled = false;
                    this.imgPrevious.IsEnabled = false;
                }));

                if (total_pages <= 1)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.imgNext.IsEnabled = false;
                        this.imgNextEnd.IsEnabled = false;
                        this.imgCenter.IsEnabled = false;
                    }));
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.imgNext.IsEnabled = true;
                        this.imgNextEnd.IsEnabled = true;
                        this.imgCenter.IsEnabled = true;
                    }));
                }
            }).Start();
        }

        //CalculateTotalPages
        private void CalculateTotalPages(string str_con)
        {
            if (thread_calculatetotalpages != null && thread_calculatetotalpages.ThreadState == ThreadState.Running) { }
            else
            {
                thread_calculatetotalpages = new Thread(() =>
                {
                    //set current page
                    current_page = 1;

                    count_rows = bus_tb_order.GetSumOrder(" where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%'" );
                    this.total_pages = count_rows / page_size;
                    if (count_rows % page_size > 0)
                        this.total_pages += 1;

                    if (count_rows > page_size)
                    {
                        if (StaticClass.GeneralClass.flag_database_type_general)
                            condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' order by [OrderID] desc OFFSET 0 ROWS FETCH NEXT " + (count_rows - page_size) + " ROWS ONLY;";
                        else
                            condition = "where [OrderID] not in (select [OrderID] from [tb_Order] where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' limit " + (count_rows - page_size) + ") and ([OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%')";
                    }
                    else
                    {
                        condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%'";
                    }
                });
                thread_calculatetotalpages.Start();
                thread_calculatetotalpages.Join();
            }
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (send_thread != null && send_thread.ThreadState == ThreadState.Running)
                send_thread.Abort();

            if (btnsend_delegate != null)
                btnsend_delegate();

            this.Close();
        }

        //GetOrderDetail
        private DataTable dt_orderdetail = new DataTable();
        private bool flag_check = false;
        private void GetOrderDetail(int _orderid)
        {
            if (thread_orderdetail != null && thread_orderdetail.ThreadState == ThreadState.Running) { }
            else
            {
                thread_orderdetail = new Thread(() =>
                {
                    try
                    {
                        this.grOrderDatail.Dispatcher.Invoke((Action)(() => { this.grOrderDatail.Visibility = System.Windows.Visibility.Hidden; }));
                        this.dgOrderDetail.Dispatcher.Invoke((Action)(() => { this.dgOrderDetail.ItemsSource = null; }));
                        this.mprOrderDetail.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mprOrderDetail.Visibility = System.Windows.Visibility.Visible;
                            this.mprOrderDetail.IsActive = true;
                        }));

                        dt_orderdetail.Clear();
                        list_ec_tb_orderdetail.Clear();
                        dt_orderdetail = bus_tb_orderdetail.GetOrderDetail("WHERE [OrderID]=" + _orderid);
                        int _no = 0;
                        if (dt_orderdetail.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt_orderdetail.Rows)
                            {
                                EC_tb_OrderDetail ec_tb_orderdetail = new EC_tb_OrderDetail();
                                ec_tb_orderdetail.No = ++_no;
                                ec_tb_orderdetail.ID = Convert.ToInt32(dr["ID"].ToString());
                                ec_tb_orderdetail.CategoryID = Convert.ToInt32(dr["CategoryID"].ToString());
                                ec_tb_orderdetail.CategoryName = dr["CategoryName"].ToString();
                                ec_tb_orderdetail.ProductID = Convert.ToInt32(dr["ProductID"].ToString());
                                ec_tb_orderdetail.ProductName = dr["ProductName"].ToString();
                                ec_tb_orderdetail.Price = Convert.ToDecimal(dr["Price"].ToString());
                                ec_tb_orderdetail.StrPrice = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Price);
                                ec_tb_orderdetail.Qty = Convert.ToInt32(dr["Qty"].ToString());
                                ec_tb_orderdetail.Tax = Convert.ToDecimal(dr["Tax"].ToString());
                                ec_tb_orderdetail.DiscountType = Convert.ToInt32(dr["DiscountType"].ToString());
                                //ec_tb_orderdetail.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                                ec_tb_orderdetail.Discount = Convert.ToDecimal(string.IsNullOrEmpty(dr["TotalDiscount"].ToString()) ? "0" : dr["TotalDiscount"].ToString());
                                ec_tb_orderdetail.Total = Convert.ToDecimal(dr["Total"].ToString());
                                ec_tb_orderdetail.StrTotal = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Total);
                                ec_tb_orderdetail.OrderID = Convert.ToInt32(dr["OrderID"].ToString());
                                ec_tb_orderdetail.Currency = StaticClass.GeneralClass.currency_setting_general;

                                flag_check = false;
                                for (int i = 0; i < list_ec_tb_orderdetail.Count; i++)
                                {
                                    if (list_ec_tb_orderdetail[i].ProductID == ec_tb_orderdetail.ProductID)
                                    {
                                        flag_check = true;
                                        list_ec_tb_orderdetail[i].Qty += ec_tb_orderdetail.Qty;
                                        list_ec_tb_orderdetail[i].Total += ec_tb_orderdetail.Total;
                                        list_ec_tb_orderdetail[i].Subtotal = StaticClass.GeneralClass.GetNumFormatDisplay(list_ec_tb_orderdetail[i].Total);
                                        break;
                                    }
                                }

                                if (flag_check == false)
                                {
                                    ec_tb_orderdetail.Subtotal = StaticClass.GeneralClass.GetNumFormatDisplay(ec_tb_orderdetail.Total);
                                    list_ec_tb_orderdetail.Add(ec_tb_orderdetail);
                                }
                            }
                        }
                        this.dgOrderDetail.Dispatcher.Invoke((Action)(() => { this.dgOrderDetail.ItemsSource = list_ec_tb_orderdetail; }));

                        Thread.Sleep(500);
                        this.mprOrderDetail.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mprOrderDetail.Visibility = System.Windows.Visibility.Hidden;
                            this.mprOrderDetail.IsActive = false;
                        }));

                        this.grOrderDatail.Dispatcher.Invoke((Action)(() => { this.grOrderDatail.Visibility = System.Windows.Visibility.Visible; }));

                        TotalStatistic();
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_orderdetail.Start();
            }
        }

        //TotalStatistic
        private decimal subtotal = 0;
        private decimal totaltax = 0;
        private decimal totaldiscount = 0;
        private decimal total_amount = 0;
        private void TotalStatistic()
        {
            try
            {
                subtotal = 0;

                if (list_ec_tb_orderdetail.Count > 0)
                {
                    foreach (EC_tb_OrderDetail ec_tb_orderdetail in list_ec_tb_orderdetail)
                    {
                        subtotal += ec_tb_orderdetail.Total;
                    }
                }

                DataTable r_order = new DataTable(); ;

                if (list_ec_tb_orderdetail.Count > 0)
                {
                    r_order = bus_tb_order.GetOrder("WHERE [OrderID] = " + list_ec_tb_orderdetail[0].OrderID);
                    totaltax = Convert.ToDecimal(r_order.Rows[0]["TotalTax"].ToString());
                    totaldiscount = Convert.ToDecimal(string.IsNullOrEmpty(r_order.Rows[0]["TotalDiscount"].ToString()) ? "0" : r_order.Rows[0]["TotalDiscount"].ToString());
                    total_amount = Convert.ToDecimal(r_order.Rows[0]["TotalAmount"].ToString());
                }
                else
                {
                    subtotal = 0;
                    totaltax = 0;
                    totaldiscount = 0;
                    total_amount = 0;
                }

                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.tblSubtotal.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(subtotal);
                    this.tblTaxLeft.Text = FindResource("tax").ToString() + " (" + StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.taxrate_setting_general) + "%)";
                    this.tblTax.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(totaltax);
                    this.tblDiscount.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(totaldiscount);
                    this.tblTotal.Text = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(total_amount);
                }));
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.Content = FindResource("close").ToString();
                    md.Title = FindResource("notification").ToString();
                    md.Content = ex.Message;
                    md.ShowDialog();
                }));
            }
        }

        //SendEmail
        private Thread send_thread = null;
        private void _SendEmail(string mail_body, string smtp_host, string sender, string password, string recipent)
        {
            MailMessage mail_message = null;
            try
            {
                if (send_thread != null && send_thread.ThreadState == ThreadState.Running) { }
                else
                {
                    send_thread = new Thread(() =>
                    {
                        try
                        {
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ""; }));
                            this.muiBtnSend.Dispatcher.Invoke((Action)(() => { muiBtnSend.Visibility = System.Windows.Visibility.Hidden; }));
                            this.muiBtnCancel.Dispatcher.Invoke((Action)(() => { muiBtnCancel.Visibility = System.Windows.Visibility.Hidden; }));
                            this.mprSend.Dispatcher.Invoke((Action)(() => { mprSend.IsActive = true; }));

                            mail_message = new MailMessage(sender, recipent);
                            mail_message.IsBodyHtml = true;
                            mail_message.Body = mail_body;
                            this.Dispatcher.Invoke((Action)(() => { mail_message.Subject = txbSubject.Text.Trim().ToString(); }));

                            SmtpClient smtpclient = new SmtpClient();
                            smtpclient.Host = smtp_host;
                            smtpclient.Port = 587;
                            smtpclient.Timeout = 30000;
                            smtpclient.EnableSsl = true;
                            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtpclient.UseDefaultCredentials = false;

                            smtpclient.Credentials = new System.Net.NetworkCredential(sender, password);
                            smtpclient.Send(mail_message);

                            this.muiBtnSend.Dispatcher.Invoke((Action)(() => { muiBtnSend.Visibility = System.Windows.Visibility.Visible; }));
                            this.muiBtnCancel.Dispatcher.Invoke((Action)(() => { muiBtnCancel.Visibility = System.Windows.Visibility.Visible; }));
                            this.mprSend.Dispatcher.Invoke((Action)(() => { mprSend.IsActive = false; }));

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                EmailSent es = new EmailSent();
                                es.emailsent_delegate += EmailSent_Delegate;
                                es.ShowDialog();
                            }));
                            this.Dispatcher.Invoke((Action)(() => { tblNotification.Text = FindResource("message_sent").ToString(); }));

                            if (mail_message != null)
                                mail_message.Dispose();
                        }
                        catch (FormatException)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                muiBtnSend.Visibility = System.Windows.Visibility.Visible;
                                muiBtnCancel.Visibility = System.Windows.Visibility.Visible;
                                mprSend.IsActive = false;
                                tblNotification.Text = FindResource("email_malformed").ToString();
                            }));

                            if (mail_message != null)
                                mail_message.Dispose();

                            send_thread.Abort();
                        }

                        catch (SmtpException)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                muiBtnSend.Visibility = System.Windows.Visibility.Visible;
                                muiBtnCancel.Visibility = System.Windows.Visibility.Visible;
                                mprSend.IsActive = false;
                                //tblNotification.Text = ex.Message; //FindResource("password_incorrect").ToString();
                                AccessSecureApp acc_page = new AccessSecureApp();
                                acc_page.ShowInTaskbar = false;
                                acc_page.Owner = this;
                                acc_page.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                acc_page.ShowDialog();
                            }));

                            if (mail_message != null)
                                mail_message.Dispose();

                            send_thread.Abort();
                        }
                    });
                    send_thread.Start();
                }
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    muiBtnSend.Visibility = System.Windows.Visibility.Visible;
                    muiBtnCancel.Visibility = System.Windows.Visibility.Visible;
                    mprSend.IsActive = false;
                    tblNotification.Text = ex.Message;
                }));
                send_thread.Abort();
            }
        }

        //EmailSent_Delegate
        private void EmailSent_Delegate()
        {
            if (btnsend_delegate != null)
            {
                btnsend_delegate();
                this.Close();
            }
            else
            {
                var m = Application.Current.MainWindow;
                m.Activate();
                this.Close();
            }
        }

        //muiBtnSend_Click
        private string mailBody = "";
        private void muiBtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txbFrom.Text.Trim() == "")
            {
                tblNotification.Text = FindResource("from_null").ToString();
                this.txbFrom.Focus();
                return;
            }

            if (pwbPasssword.Password.Trim() == "")
            {
                tblNotification.Text = FindResource("password_null").ToString();
                this.pwbPasssword.Focus();
                return;
            }

            if (txbTo.Text.Trim() == "")
            {
                tblNotification.Text = FindResource("to_null").ToString();
                this.txbTo.Focus();
                return;
            }

            if (txbTo.Text.Trim().ToString() == "None")
            {
                tblNotification.Text = FindResource("no_recipient").ToString();
                this.txbTo.Focus();
                return;
            }
            else
            {
                mailBody = "";
                TextRange text_range = new TextRange(rtbMailBody.Document.ContentStart, rtbMailBody.Document.ContentEnd);
                mailBody += "<font>" + text_range.Text.Replace("\n", "<br />") + "</font><br />";
                mailBody += CreateHTML();

                _SendEmail(mailBody, "smtp.gmail.com", txbFrom.Text.Trim(), pwbPasssword.Password.Trim(), txbTo.Text.Trim().ToString());
            }
        }

        //imgFromSave_MouseDown
        private void imgFromSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StaticClass.GeneralClass.app_settings["emailAcc"] = txbFrom.Text.Trim().ToString();
                StaticClass.GeneralClass.app_settings["emailPass"] = pwbPasssword.Password.Trim().ToString();
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        //imgPasswordSave_MouseDown
        private void imgPasswordSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StaticClass.GeneralClass.app_settings["emailAcc"] = txbFrom.Text.Trim().ToString();
                StaticClass.GeneralClass.app_settings["emailPass"] = pwbPasssword.Password.Trim().ToString();
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        //GetListOrder
        private DataTable tb_order = new DataTable();
        private void GetListOrder(string con)
        {
            if (thread_order != null && thread_order.ThreadState == ThreadState.Running) { }
            else
            {
                thread_order = new Thread(() =>
                {
                    try
                    {
                        this.dtgOrder.Dispatcher.Invoke((Action)(() =>
                        {
                            this.dtgOrder.Visibility = System.Windows.Visibility.Collapsed;
                            this.dtgOrder.ItemsSource = null;
                            this.mprOrder.Visibility = System.Windows.Visibility.Visible;
                            this.mprOrder.IsActive = true;
                        }));

                        tb_order.Clear();
                        list_ec_tb_order.Clear();

                        tb_order = bus_tb_order.GetOrder(con);
                        foreach (DataRow datarow in tb_order.Rows)
                        {
                            _orderDate = string.Format(@"{0:" + StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString()+ _orderTime + "}", Convert.ToDateTime(datarow["OrderDate"].ToString()));
                            EC_tb_Order ec_tb_order = new EC_tb_Order();
                            ec_tb_order.OrderID = Convert.ToInt32(datarow["OrderID"].ToString());
                            ec_tb_order.CustomerID = Convert.ToInt32(datarow["CustomerID"].ToString());
                            ec_tb_order.CustomerName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow["CustomerName"].ToString());
                            ec_tb_order.Quatity = Convert.ToInt32(datarow["Quantity"].ToString());
                            ec_tb_order.OrderDate = _orderDate;
                            ec_tb_order.PaymentName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow["PaymentName"].ToString());
                            list_ec_tb_order.Add(ec_tb_order);
                        }

                        Comparison<EC_tb_Order> comparison = new Comparison<EC_tb_Order>(EC_tb_Order.CompareOrderID);
                        list_ec_tb_order.Sort(comparison);

                        this.dtgOrder.Dispatcher.Invoke((Action)(() => { this.dtgOrder.ItemsSource = list_ec_tb_order; }));

                        Thread.Sleep(500);

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mprOrder.IsActive = false;
                            this.mprOrder.Visibility = System.Windows.Visibility.Hidden;
                            this.dtgOrder.Visibility = System.Windows.Visibility.Visible;
                        }));

                        if (list_ec_tb_order.Count > 0)
                            this.dtgOrder.Dispatcher.Invoke((Action)(() => { dtgOrder.SelectedIndex = 0; }));
                        else
                        {
                            list_ec_tb_orderdetail.Clear();
                            this.dgOrderDetail.Dispatcher.Invoke((Action)(() => { dgOrderDetail.ItemsSource = null; }));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_order.Start();
            }
        }

        //dtgOrder_SelectionChanged
        int dtgorder_selected = -1;
        private void dtgOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                dtgorder_selected = -1;
                if (dtgOrder.SelectedIndex > -1)
                {
                    dtgorder_selected = dtgOrder.SelectedIndex;   
                    orderid = list_ec_tb_order[dtgorder_selected].OrderID;
                    if (list_ec_tb_order[dtgorder_selected].CustomerID == 0)
                    {
                        list_ec_tb_order[dtgorder_selected].CustomerName = "None";
                        list_ec_tb_order[dtgorder_selected].Phone = "None";
                    }
                    else
                    {
                        DataTable tb_customer = new DataTable();
                        tb_customer = bus_tb_customer.GetCustomer("where [CustomerID] = " + list_ec_tb_order[dtgorder_selected].CustomerID);
                        list_ec_tb_order[dtgorder_selected].CustomerName = list_ec_tb_order[dtgorder_selected].CustomerName;
                        list_ec_tb_order[dtgorder_selected].Phone = tb_customer.Rows[0]["Phone"].ToString();
                        this.txbTo.Text = tb_customer.Rows[0]["Email"].ToString();
                    }

                    this.tblCustomerName.Text = list_ec_tb_order[dtgorder_selected].CustomerName;
                    this.tblPhoneCustomer.Text = list_ec_tb_order[dtgorder_selected].Phone;
                    this.tblPayment.Text = getPaymentMethodForOrder(orderid); //list_ec_tb_order[dtgorder_selected].PaymentName;
                    this.tblDatetime.Text = list_ec_tb_order[dtgorder_selected].OrderDate;
                    GetOrderDetail(orderid);
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //ModernWindow_Closed
        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            if (btnsend_delegate != null)
            {
                btnsend_delegate();
                this.Close();
            }
        }

        //imgPreviousFirst_MouseDown
        private void imgPreviousFirst_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new Thread(() =>
            {
                string str_con = "";
                current_page = 1;

                this.Dispatcher.Invoke((Action)(() =>
                {
                    str_con = this.txbSearch.Text.Trim().ToString();
                    imgNext.IsEnabled = true;
                    imgNextEnd.IsEnabled = true;
                    imgCenter.IsEnabled = true;
                    imgPrevious.IsEnabled = false;
                    imgPreviousFirst.IsEnabled = false;
                }));
                if(StaticClass.GeneralClass.flag_database_type_general)
                    condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' order by [OrderID] desc OFFSET 0 ROWS FETCH NEXT " + (count_rows - page_size) + " ROWS ONLY;";
                else
                    condition = "where [OrderID] not in (select [OrderID] from [tb_Order] where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' limit " + (count_rows - page_size) + " ) and ([OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%')"; 
                GetListOrder(condition);
            }).Start();
        }

        //imgPrevious_MouseDown
        private void imgPrevious_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new Thread(() =>
            {
                string str_con = "";
                current_page--;

                this.Dispatcher.Invoke((Action)(() =>
                {
                    str_con = this.txbSearch.Text.Trim().ToString();
                    imgNext.IsEnabled = true;
                    imgNextEnd.IsEnabled = true;
                    imgCenter.IsEnabled = true;
                }));

                if (current_page == 1)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        imgPrevious.IsEnabled = false;
                        imgPreviousFirst.IsEnabled = false;
                    }));
                    if (StaticClass.GeneralClass.flag_database_type_general)
                        condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' order by [OrderID] desc OFFSET 0 ROWS FETCH NEXT " + (count_rows - page_size) + " ROWS ONLY;";
                    else
                        condition = "where [OrderID] not in (select [OrderID] from [tb_Order] where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' limit " + (count_rows - page_size) + " ) and ([OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%')"; 
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        imgPrevious.IsEnabled = true;
                        imgPreviousFirst.IsEnabled = true;
                    }));
                    if (StaticClass.GeneralClass.flag_database_type_general)
                        condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' order by [OrderID] desc OFFSET " + (count_rows - current_page * page_size) + " ROWS FETCH NEXT " + page_size + " ROWS ONLY;";
                    else
                        condition = "where [OrderID] not in (select [OrderID] from [tb_Order] where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' limit " + (count_rows - current_page * page_size) + ") and ([OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%') limit " + page_size;
                }

                GetListOrder(condition);
            }).Start();
        }

        //imgCenter_MouseDown
        private void imgCenter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new Thread(() =>
            {
                string str_con = "";
                current_page = total_pages / 2;

                this.Dispatcher.Invoke((Action)(() =>
                {
                    str_con = this.txbSearch.Text.Trim().ToString();
                    imgNext.IsEnabled = true;
                    imgNextEnd.IsEnabled = true;
                    imgCenter.IsEnabled = true;
                }));

                if (current_page == 1)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        imgPrevious.IsEnabled = false;
                        imgPreviousFirst.IsEnabled = false;
                    }));
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        imgPrevious.IsEnabled = true;
                        imgPreviousFirst.IsEnabled = true;
                    }));
                }
                if (StaticClass.GeneralClass.flag_database_type_general)
                    condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' order by [OrderID] desc OFFSET " + (count_rows - current_page * page_size) + " ROWS FETCH NEXT " + page_size + " ROWS ONLY;";
                else
                    condition = "where [OrderID] not in (select [OrderID] from [tb_Order] where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' limit " + (count_rows - current_page * page_size) + ") and ([OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%') limit " + page_size;
                GetListOrder(condition);
            }).Start();
        }

        //imgNext_MouseDown
        private void imgNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new Thread(() =>
            {
                current_page++;
                string str_con = "";
                this.txbSearch.Dispatcher.Invoke((Action)(() => { str_con = this.txbSearch.Text.Trim().ToString(); }));

                if (current_page == total_pages)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        imgNext.IsEnabled = false;
                        imgNextEnd.IsEnabled = false;
                    }));
                    if (StaticClass.GeneralClass.flag_database_type_general)
                        condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' order by [OrderID] desc OFFSET " + (count_rows - current_page * page_size) + " ROWS FETCH NEXT " + page_size + " ROWS ONLY;";
                    else
                        condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' limit " + (count_rows - (current_page - 1) * page_size);
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        imgNext.IsEnabled = true;
                        imgNextEnd.IsEnabled = true;
                    }));
                    if (StaticClass.GeneralClass.flag_database_type_general)
                        condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' order by [OrderID] desc OFFSET " + (count_rows - current_page * page_size) + " ROWS FETCH NEXT " + page_size + " ROWS ONLY;";
                    else
                        condition = "where [OrderID] not in (select [OrderID] from [tb_Order] where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' limit " + (count_rows - current_page * page_size) + ") and ([OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%') limit " + page_size;
                }

                this.Dispatcher.Invoke((Action)(() =>
                {
                    imgCenter.IsEnabled = true;
                    imgPrevious.IsEnabled = true;
                    imgPreviousFirst.IsEnabled = true;
                }));

                GetListOrder(condition);
            }).Start();
        }

        //imgNextEnd_MouseDown
        private void imgNextEnd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new Thread(() =>
            {
                string str_con = "";
                current_page = total_pages;

                this.Dispatcher.Invoke((Action)(() =>
                {
                    str_con = this.txbSearch.Text.Trim().ToString();
                    imgNext.IsEnabled = false;
                    imgNextEnd.IsEnabled = false;
                    imgCenter.IsEnabled = true;
                    imgPrevious.IsEnabled = true;
                    imgPreviousFirst.IsEnabled = true;
                }));
                if (StaticClass.GeneralClass.flag_database_type_general)
                    condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' order by [OrderID] desc OFFSET " + (count_rows - current_page * page_size) + " ROWS FETCH NEXT " + page_size + " ROWS ONLY;";
                else
                    condition = "where [OrderID] like '%" + str_con + "%' or [CustomerName] like '%" + str_con + "%' limit " + (count_rows - (current_page - 1) * page_size);
                GetListOrder(condition);
            }).Start();
        }

        //ModernWindow_SizeChanged
        private void ModernWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.stpOrder.Height = this.RenderSize.Height - 180;
        }

        private void imgSearchOrder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txbSearch.Visibility == System.Windows.Visibility.Collapsed)
            {
                tblTTSearch.Text = FindResource("search").ToString(); ;
                txbSearch.Visibility = System.Windows.Visibility.Visible;
                txbSearch.Focus();
                imgColspan.Visibility = System.Windows.Visibility.Visible;
                tblTitleListOrder.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
                LoadData(txbSearch.Text.Trim().ToString());
        }

        //imgColspan_MouseDown
        private void imgColspan_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txbSearch.Visibility = System.Windows.Visibility.Collapsed;
            imgColspan.Visibility = System.Windows.Visibility.Collapsed;
            tblTitleListOrder.Visibility = System.Windows.Visibility.Visible;
            txbSearch.Text = "";
            tblTTSearch.Text = FindResource("show_textbox_search").ToString(); ;
            LoadData(txbSearch.Text.Trim().ToString());
        }

        //txbSearch_KeyDown
        private void txbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
                LoadData(txbSearch.Text.Trim().ToString());
        }

        //hplChangeInfo_Click
        private void hplChangeInfo_Click(object sender, RoutedEventArgs e)
        {
            ChangeInfoOrder page = new ChangeInfoOrder();
            page.hplchangeinfo_delegate += hplChangeInfo_Click_Delegate;
            page.ShowDialog();
        }

        //hplChangeInfo_Click_Delegate
        private void hplChangeInfo_Click_Delegate(string str_storename, string str_storeaddress, string str_storephone)
        {
            tblStoreName.Text = str_storename;
            tblStoreAddress.Text = str_storeaddress;
            tblStorePhone.Text = FindResource("phone").ToString() + ": " + str_storephone;
        }

        //muiBtnSetting_Click
        private void muiBtnSetting_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.WebBrowser webBrowserPreview = new System.Windows.Forms.WebBrowser();
            webBrowserPreview.DocumentText = CreateHTML();
            webBrowserPreview.DocumentCompleted += webBrowser_ShowPrintPreviewDialog_DocumentCompleted;
        }

        //webBrowser_ShowPrintPreviewDialog_DocumentCompleted
        private void webBrowser_ShowPrintPreviewDialog_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            if (sender != null)
                ((System.Windows.Forms.WebBrowser)sender).ShowPrintPreviewDialog();
        }

        //muiBtnPrint_Click
        private void muiBtnPrint_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.WebBrowser webBrowserPrint = new System.Windows.Forms.WebBrowser();
            webBrowserPrint.DocumentText = CreateHTML();
            webBrowserPrint.DocumentCompleted += webBrowser_ShowPrintDialog_DocumentCompleted;


            /*Helpers.PCPrint printer = new Helpers.PCPrint();
            System.Drawing.Printing.PrinterSettings ps = new System.Drawing.Printing.PrinterSettings();
            ps.Copies = 2;
            printer.PrinterSettings = ps;
            printer.PrinterFont = new System.Drawing.Font("Verdana", 10);
            printer.TextToPrint = CreateHTML();
            printer.Print();
            Byte[] bytes;

            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new Document(PageSize.A4))
                {

                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {

                        //Open the document for writing
                        doc.Open();

                        //Our sample HTML and CSS
                        var example_html = CreateHTML();
                        var example_css = @".headline{font-size:200%}";
                        /**************************************************
                         * Example #1                                     *
                         *                                                *
                         * Use the built-in HTMLWorker to parse the HTML. *
                         * Only inline CSS is supported.                  *
                         * ************************************************/

                        //Create a new HTMLWorker bound to our document
                        /*using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                        {

                            //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                            using (var sr = new StringReader(example_html))
                            {

                                //Parse the HTML
                                htmlWorker.Parse(sr);
                            }
                        }*/

                        /**************************************************
                         * Example #2                                     *
                         *                                                *
                         * Use the XMLWorker to parse the HTML.           *
                         * Only inline CSS and absolutely linked          *
                         * CSS is supported                               *
                         * ************************************************
                        BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIALUNI.TTF", BaseFont.IDENTITY_H, true);
                        Font NormalFont = new iTextSharp.text.Font(bf, 12, Font.NORMAL, BaseColor.BLACK);

                        FontFactory.Register("C:/Windows/Fonts/ARIALUNI.TTF", "arial unicode ms");

                        byte[] byteArray = Encoding.UTF8.GetBytes(example_html);



                        //XMLWorker also reads from a TextReader and not directly from a string;  using (var srHtml = new StringReader(example_html))
                        using (MemoryStream srHtml = new MemoryStream(byteArray))
                        {

                            //Parse the HTML
                            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml, Encoding.UTF8);
                        }

                        /**************************************************
                         * Example #3                                     *
                         *                                                *
                         * Use the XMLWorker to parse HTML and CSS        *
                         * ************************************************/

                        //In order to read CSS as a string we need to switch to a different constructor
                        //that takes Streams instead of TextReaders.
                        //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
                        /*using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                            {

                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss, Encoding.UTF8);
                            }
                        }*


                        doc.Close();
                    }
                }

                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }

            //Now we just need to do something with those bytes.
            //Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
            //You could also write the bytes to a database in a varbinary() column (but please don't) or you
            //could pass them to another function for further PDF processing.
            var testFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
            System.IO.File.WriteAllBytes(testFile, bytes);*/
        }

        //webBrowser_ShowPrintDialog_DocumentCompleted
        private void webBrowser_ShowPrintDialog_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            if (sender != null)
                ((System.Windows.Forms.WebBrowser)sender).ShowPrintDialog();
        }

        //CreateHTML
        private string CreateHTML()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            string _orderDate_ = StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString() + _orderTime;
            //create top portion of html document
            htmlBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
            htmlBuilder.Append("<head>");
            htmlBuilder.Append("<meta http-equiv='Content-Type' content='text/html; charset = utf-8' />");
            htmlBuilder.Append("<title>Order details</title>");
            htmlBuilder.Append("</head>");
            htmlBuilder.Append("<body>");

            htmlBuilder.Append("<div style='width:99%; padding:0.5%; margin: 0 auto;'>");
            htmlBuilder.Append("<div style='text-align:center; font-size:25px; font-weight:bold; margin-bottom: 10px; '>" + StaticClass.GeneralClass.app_settings["storeName"].ToString() + " </div>");
            htmlBuilder.Append("<div style='text-align:center; color:#595757;padding:2px;'>" + StaticClass.GeneralClass.app_settings["storeAddress"].ToString() + " </div>");
            htmlBuilder.Append("<div style='text-align:center; color:#595757;padding:2px;'>" + FindResource("phone").ToString() + ": " + StaticClass.GeneralClass.app_settings["storePhone"].ToString() + " </div>");
            htmlBuilder.Append("<hr style='border-top:2px solid #000;margin:5px auto;'/>");
            htmlBuilder.Append("<div style='text-align:center; font-size:18px; font-weight:bold; margin: 10px auto; '>" + FindResource("reciept").ToString() + "</div>");
            htmlBuilder.Append("<hr style='border-top:1px solid #000;margin:5px auto;'/>");
            htmlBuilder.Append("<div style='padding:2px;'><b>" + FindResource("customer").ToString() + ": </b>" + (dtgorder_selected == -1 ? "None" : list_ec_tb_order[dtgorder_selected].CustomerName) + ", <b>" + FindResource("phone").ToString() + ": </b>" + (dtgorder_selected == -1 ? "None" : list_ec_tb_order[dtgorder_selected].Phone) + "</div>");
            htmlBuilder.Append("<div style='padding:2px;'><b>" + FindResource("payment").ToString() + ": </b>" + (dtgorder_selected == -1 ? "None" : getPaymentMethodForOrder(orderid)) + "</div>");
            htmlBuilder.Append("<div style='padding:2px;'><b>" + FindResource("date").ToString() + ": </b>" + System.DateTime.Now.ToString(_orderDate_) + "</div>");
            htmlBuilder.Append("<table style='border-collapse:collapse; margin-top:10px; width:100%;'>");
            htmlBuilder.Append("<tr height='35'><th>" + FindResource("no_title").ToString() + "</th><th>" + FindResource("name").ToString() + "</th><th>" + FindResource("price").ToString() + "</th><th>" + FindResource("qty").ToString() + "</th><th>" + FindResource("subtotal").ToString() + "</th></tr>");
            foreach (var orderDetail in list_ec_tb_orderdetail)
            {
                htmlBuilder.Append("<tr>");
                htmlBuilder.Append("<td style='text-align:center;'>" + orderDetail.No + "</td>");
                htmlBuilder.Append("<td>" + orderDetail.ProductName + "</td>");
                htmlBuilder.Append("<td style='text-align:center;'>" + StaticClass.GeneralClass.GetNumFormatDisplay(orderDetail.Price) + "</td>");
                htmlBuilder.Append("<td style='text-align:center;'>" + orderDetail.Qty + "</td>");
                htmlBuilder.Append("<td style='text-align:right;'>" + orderDetail.Subtotal + "</td>");
                htmlBuilder.Append("</tr>");

                htmlBuilder.Append("<tr>");
                htmlBuilder.Append("<td colspan='5'><hr style='border: 1px dotted #ababab; margin:2px auto; border-style: none none dotted; background-color: #fff;'/></td>");
                htmlBuilder.Append("</tr>");
            }

            htmlBuilder.Append("<tr style='height:30px;'>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:20px; padding-right:20px; font-weight:bold;' colspan='3'>" + FindResource("subtotal").ToString() + "</td>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:20px;' colspan='2'>" + StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(subtotal) + "</td>");
            htmlBuilder.Append("</tr>");

            htmlBuilder.Append("<tr style='height:30px;'>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px; padding-right:20px; font-weight:bold;' colspan='3'>" + FindResource("tax").ToString() + "(" + StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.taxrate_setting_general) + "%)</td>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px;' colspan='2'>" + StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(totaltax) + "</td>");
            htmlBuilder.Append("</tr>");

            htmlBuilder.Append("<tr style='height:30px;'>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px; padding-right:20px; font-weight:bold;' colspan='3'>" + FindResource("discount").ToString() + "</td>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px;' colspan='2'>" + StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(totaldiscount) + "</td>");
            htmlBuilder.Append("</tr>");

            htmlBuilder.Append("<tr style='height:30px;'>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px; padding-right:20px; font-weight:bold; font-size: 20px;' colspan='3'>" + FindResource("total").ToString() + "</td>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px;' colspan='2'>" + StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(total_amount) + "</td>");
            htmlBuilder.Append("</tr>");

            htmlBuilder.Append("</table><br/><br/>");
            htmlBuilder.Append("<div style='margin-top:20px; text-align:center; color:#595757;'>THANK YOU - SEE YOU AGAIN</div>");
            htmlBuilder.Append("</div>");
            htmlBuilder.Append("</body>");
            htmlBuilder.Append("</html>");
            return htmlBuilder.ToString();
        }
    }

    /// <summary>
    /// Paginated printing of wpf visual
    /// http://undude.blogspot.com/2011/02/paginated-printing-of-wpf-visu
    /// </summary>
    /// ProgramPaginator
}
