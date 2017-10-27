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
using System.Threading;

namespace CashierRegister.Pages.Report
{
    /// <summary>
    /// Interaction logic for DeleteReport.xaml
    /// </summary>
    public partial class DeleteOrder : ModernDialog
    {
        //using for order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //using for orderdetail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //delegate
        public delegate void muiBtnDelete_Click_Delegate(bool flag_deleteed);
        public muiBtnDelete_Click_Delegate btndelete_delegate;

        //thread
        private Thread thread_delete = null;

        //DeleteOrder
        public DeleteOrder()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            StaticClass.GeneralClass.list_ec_tb_order_general.Clear();
            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (StaticClass.GeneralClass.list_ec_tb_order_general.Count == 0)
                tblNotification.Text = FindResource("select_least_order").ToString();
            else
            {
                if (thread_delete != null && thread_delete.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_delete = new Thread(() =>
                    {
                        try
                        {
                            int result_order = 0;
                            int result_orderdetail = 0;

                            for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_order_general.Count; i++)
                            {
                                EC_tb_Order ec_tb_order = new EC_tb_Order();
                                ec_tb_order.OrderID = StaticClass.GeneralClass.list_ec_tb_order_general[i].OrderID;
                                if (bus_tb_order.DeleteOrder(ec_tb_order) == 1)
                                {
                                    result_order++;
                                    if (bus_tb_orderdetail.DeleteAllOrderDetail("WHERE [OrderID]=" + StaticClass.GeneralClass.list_ec_tb_order_general[i].OrderID) > 0)
                                        result_orderdetail++;
                                }
                            }

                            if (result_order == result_orderdetail)
                            {
                                StaticClass.GeneralClass.flag_check_delete_order_orderdetail = true;
                                StaticClass.GeneralClass.list_ec_tb_order_general.Clear();
                                if (btndelete_delegate != null)
                                {
                                    btndelete_delegate(true);
                                    this.Dispatcher.Invoke((Action)(() => { this.Close(); }));
                                }
                            }
                            else
                                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("error_delete").ToString(); }));
                        }
                        catch (Exception ex)
                        {
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ex.Message; }));
                        }
                    });
                    thread_delete.Start();
                }
            }
        }

    }
}
