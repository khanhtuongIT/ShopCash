using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using CashierRegisterEntity;
using System.Data;
using CashierRegisterBUS;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : UserControl
    {
        //using for Payment
        private BUS_tb_Payment bus_payment = new BUS_tb_Payment();
        private List<EC_tb_Payment> list_ec_payment = new List<EC_tb_Payment>();
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        private bool flag_check_loaded = false;

        //thread
        private Thread thread_content = null;

        //Payment
        public Payment()
        {
            InitializeComponent();
            dtgCard.ItemsSource = list_ec_payment;
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                GetPayment();
            }
        }

        //GetUser
        private DataTable tb_payment = new DataTable();
        private void GetPayment()
        {
            if (thread_content != null && thread_content.ThreadState == ThreadState.Running) { }
            else
            {
                thread_content = new Thread(() =>
                {
                    try
                    {
                        list_ec_payment.Clear();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.dtgCard.Visibility = System.Windows.Visibility.Hidden;
                            this.mpr.Visibility = System.Windows.Visibility.Visible;
                            this.mpr.IsActive = true;
                        }));

                        int no = 0;
                        foreach (DataRow datarow in bus_payment.GetPayment("").Rows)
                        {
                            //if(datarow["Card"].ToString() != "Cash" && datarow["Card"].ToString() != "Gift Card")
                            //{
                                EC_tb_Payment ec_payment = new EC_tb_Payment();
                                ec_payment.No = ++no;
                                ec_payment.PaymentID = Convert.ToInt32(datarow["PaymentID"].ToString());
                                ec_payment.Card = datarow["Card"].ToString();
                                ec_payment.EditImage = @"pack://application:,,,/Resources/edit.png";
                                ec_payment.DeleteImage = @"pack://application:,,,/Resources/delete.png";

                                list_ec_payment.Add(ec_payment);
                            //}
                        }

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            tblTotal.Text = FindResource("total").ToString() + "(" + list_ec_payment.Count.ToString() + ")";
                            dtgCard.Items.Refresh();
                        }));

                        Thread.Sleep(500);

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Hidden;
                            this.mpr.IsActive = false;
                            this.dtgCard.Visibility = System.Windows.Visibility.Visible;
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
                thread_content.Start();
            }
        }

        //muiBtnAdd_Click
        private void muiBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddPayment page = new AddPayment();
            page.ShowInTaskbar = false;
            page.add_delegate += muiBtnAdd_Edit_Delegate;
            page.ShowDialog();
        }

        //muiBtnAdd_Edit_Delegate
        private void muiBtnAdd_Edit_Delegate()
        {
            GetPayment();
        }

        //btnEdit_Click
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            int paymentid = -1;
            Button button = sender as Button;
            if(int.TryParse(button.Uid.ToString(), out paymentid))
            {
                string _paymentName = list_ec_payment.Find(x => x.PaymentID == paymentid).Card;
                if(_paymentName== "Cash" || _paymentName == "Gift Card")
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.FindResource("close").ToString();
                    md.Content = App.Current.FindResource("payment_default").ToString();
                    md.Title = App.Current.FindResource("notification").ToString();
                    md.ShowDialog();
                    return;
                }
                EditPayment page = new EditPayment();
                page.ShowInTaskbar = false;
                page.tblPaymentID.Text = button.Uid.ToString();
                page.txbCard.Text = list_ec_payment.Find(x => x.PaymentID == paymentid).Card;
                page.edit_delegate += muiBtnAdd_Edit_Delegate;
                page.ShowDialog();
            }
        }

        //dtgUser_MouseDoubleClick
        private void dtgUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgCard.SelectedIndex > -1)
                btnEdit_Click(new Button() { Uid = list_ec_payment[dtgCard.SelectedIndex].PaymentID.ToString() }, null);
        }

        //btnDelete_Click
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int paymentid = -1;
            Button button = sender as Button;
            if(int.TryParse(button.Uid.ToString(), out paymentid))
            {
                string _paymentName = list_ec_payment.Find(x => x.PaymentID == paymentid).Card;
                if (_paymentName == "Cash" || _paymentName == "Gift Card")
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.FindResource("close").ToString();
                    md.Content = App.Current.FindResource("payment_default").ToString();
                    md.Title = App.Current.FindResource("notification").ToString();
                    md.ShowDialog();
                }
                else
                {
                    DeletePayment page = new DeletePayment();
                    page.ShowInTaskbar = false;
                    page.paymentid = paymentid;
                    page.card = list_ec_payment.Find(x => x.PaymentID == paymentid).Card;
                    page.delete_delegate += muiBtnAdd_Edit_Delegate;
                    page.ShowDialog();
                }
                
            }
        }

    }
}
