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

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for DeleteOrderDetail.xaml
    /// </summary>
    public partial class DeleteOrderDetail : ModernDialog
    {
        //delegate
        public delegate void btnDeleteOrderDetail_Click_Delegate();
        public event btnDeleteOrderDetail_Click_Delegate btndelete_orderdetail_delegate;

        public DeleteOrderDetail()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            btnOK.Focus();
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //btnOK_Click
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StaticClass.GeneralClass.dtgorderdetail_selectedindex_general > -1)
                {
                    StaticClass.GeneralClass.list_ec_tb_orderdetail_general.RemoveAt(StaticClass.GeneralClass.dtgorderdetail_selectedindex_general);

                    if (btndelete_orderdetail_delegate != null)
                    {
                        btndelete_orderdetail_delegate();
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
