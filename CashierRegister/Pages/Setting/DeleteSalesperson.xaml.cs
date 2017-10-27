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
    /// Interaction logic for DeleteSalesperson.xaml
    /// </summary>
    public partial class DeleteSalesperson : ModernDialog
    {
        //using for salesperson
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();
        //delegate
        public delegate void muiBtnDelete_Click_Delegate();
        public event muiBtnDelete_Click_Delegate muibtndelete_delegate;

        //DeleteSalesperson
        public DeleteSalesperson()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            StaticClass.GeneralClass.list_ec_tb_salesperson_general.Clear();
            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (StaticClass.GeneralClass.list_ec_tb_salesperson_general.Count == 0)
                tblNotification.Text = FindResource("select_least_salesperson").ToString();
            else
            {
                try
                {
                    int result = 0;
                    for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_salesperson_general.Count; i++)
                    {
                        if (bus_tb_order.GetOrder("WHERE [SalespersonID]=" + StaticClass.GeneralClass.list_ec_tb_salesperson_general[i].SalespersonID).Rows.Count > 0)
                            tblNotification.Text = FindResource("salesperson_id").ToString() + ": " + StaticClass.GeneralClass.list_ec_tb_salesperson_general[i].SalespersonID.ToString() + " " + FindResource("already_uses").ToString();
                        else
                        {
                            EC_tb_SalesPerson ec_tb_salesperson = new EC_tb_SalesPerson();
                            ec_tb_salesperson.SalespersonID = StaticClass.GeneralClass.list_ec_tb_salesperson_general[i].SalespersonID;
                            if (bus_tb_salesperson.DeleteSalesPerson(ec_tb_salesperson) == 1)
                                result++;
                        }

                    }

                    if (result > 0)
                    {
                        if (muibtndelete_delegate != null)
                        {
                            StaticClass.GeneralClass.flag_add_edit_delete_salesperson_general = true;
                            StaticClass.GeneralClass.list_ec_tb_salesperson_general.Clear();
                            muibtndelete_delegate();
                            this.Close();
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
}
