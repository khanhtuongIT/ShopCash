using System;
using System.Collections.Generic;
using FirstFloor.ModernUI.Windows.Controls;
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
using System.Threading;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for ExportLocal.xaml
    /// </summary>
    public partial class ExportLocal : UserControl
    {
        //using for catefory
        private BUS_tb_Category bus_tb_category = new BUS_tb_Category();

        //using for customer
        private BUS_tb_Customer bus_tb_customer = new BUS_tb_Customer();

        //using for input detail
        private BUS_tb_InputHistory bus_tb_inputhistory = new BUS_tb_InputHistory();

        //using for order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //using for orderdetail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //using for payment
        private BUS_tb_Payment bus_tb_payment = new BUS_tb_Payment();

        //using for product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();

        //using for salesperson
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();

        //using for setting
        private BUS_tb_Setting bus_tb_setting = new BUS_tb_Setting();

        //using for user
        private BUS_tb_User bus_tb_user = new BUS_tb_User();

        //using for export table
        private StaticClass.ExcelUtilityClass excel_utility = new StaticClass.ExcelUtilityClass();

        //thread
        private Thread thread_export = null;

        private int num_export_selected = 0;

        //ExportLocal
        public ExportLocal()
        {
            InitializeComponent();
        }

        //muiBtnExport_Click
        private DataTable datatable_export;

        private void muiBtnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (thread_export != null && thread_export.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_export = new Thread(() =>
                    {
                        int num_export = 0;
                        bool flag = false;
                        string path = "";

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            System.Windows.Forms.FolderBrowserDialog folder_browser_dialog = new System.Windows.Forms.FolderBrowserDialog();
                            if (folder_browser_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                flag = true;
                                num_export = 0;
                                path = folder_browser_dialog.SelectedPath.ToString();
                            }
                        }));

                        if (flag == true)
                        {
                            this.muiBtnExport.Dispatcher.Invoke((Action)(() => { this.muiBtnExport.Visibility = System.Windows.Visibility.Hidden; }));
                            this.mprExport.Dispatcher.Invoke((Action)(() => { this.mprExport.Visibility = System.Windows.Visibility.Visible; }));
                            this.mprExport.Dispatcher.Invoke((Action)(() => { this.mprExport.IsActive = true; }));

                            //tb_Category
                            if (flag_category == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_category.GetCatagorySetting("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_Category", path + @"\tb_Category.csv", "Details") == true)
                                    num_export++;
                            }

                            //tb_Customer

                            if (flag_customer == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_customer.GetCustomer("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_Customer", path + @"\tb_Customer.csv", "Details") == true)
                                    num_export++;
                            }

                            //tb_InputHistory
                            if (flag_inputhistory == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_inputhistory.GetInputHistory("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_InputHistory", path + @"\tb_InputHistory.csv", "Details") == true)
                                    num_export++;
                            }

                            //tb_Order
                            if (flag_order == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_order.GetOrder("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_Order", path + @"\tb_Order.csv", "Details") == true)
                                    num_export++;
                            }

                            //tb_OrderDetail
                            if (flag_orderdetail == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_orderdetail.GetOrderDetail("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_OrderDetail", path + @"\tb_OrderDetail.csv", "Details") == true)
                                    num_export++;
                            }

                            //tb_Payment
                            if (flag_payment == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_payment.GetPayment("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_Payment", path + @"\tb_Payment.csv", "Details") == true)
                                    num_export++;
                            }

                            //tb_Product
                            if (flag_product == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_product.GetProduct("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_Product", path + @"\tb_Product.csv", "Details") == true)
                                    num_export++;
                            }

                            //tb_Salesperson
                            if (flag_salesperson == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_salesperson.GetSalesPerson("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_Salesperson", path + @"\tb_Salesperson.csv", "Details") == true)
                                    num_export++;
                            }

                            //tb_Setting
                            if (flag_setting == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_setting.GetSetting("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_Setting", path + @"\tb_Setting.csv", "Details") == true)
                                    num_export++;
                            }

                            //tb_User
                            if (flag_user == true)
                            {
                                datatable_export = new DataTable();
                                datatable_export = bus_tb_user.GetUser("");
                                if (excel_utility.WriteDataTableToExcel(datatable_export, "tb_User", path + @"\tb_User.csv", "Details") == true)
                                    num_export++;
                            }
                        }

                        Thread.Sleep(500);
                        this.mprExport.Dispatcher.Invoke((Action)(() => { this.mprExport.IsActive = false; }));
                        this.mprExport.Dispatcher.Invoke((Action)(() => { this.mprExport.Visibility = System.Windows.Visibility.Hidden; }));
                        this.muiBtnExport.Dispatcher.Invoke((Action)(() => { this.muiBtnExport.Visibility = System.Windows.Visibility.Visible; }));

                        //export success
                        if (num_export == num_export_selected)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                ModernDialog md = new ModernDialog();
                                md.Title = FindResource("notification").ToString();
                                md.Content = FindResource("export_success").ToString();
                                md.ShowDialog();
                            }));
                        }
                    });

                    thread_export.SetApartmentState(ApartmentState.STA);
                    thread_export.Start();
                }
            }
            catch (Exception ex)
            {
                ModernDialog mderr = new ModernDialog();
                mderr.Title = FindResource("notification").ToString();
                mderr.Content = ex.Message;
                mderr.ShowDialog();
            }
        }

        //chkCheckAll_Checked
        private void chkCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            HandleCheck(true);
            //TextBlock tbl = (TextBlock)chkCheckAll.FindName("tblTooltipCheckAll");
            //tbl.Text = "Uncheck to unselect all table";
            //num_export_selected = 12;
        }

        //chkCheckAll_Unchecked
        private void chkCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleCheck(false);
            //TextBlock tbl = (TextBlock)chkCheckAll.FindName("tblTooltipCheckAll");
            //tbl.Text = "Check to select all table";
            //num_export_selected = 0;
        }

        //chkTbCategory_Checked
        private bool flag_category = false;
        private void chkTbCategory_Checked(object sender, RoutedEventArgs e)
        {
            flag_category = true;
            num_export_selected++;
        }

        //chkTbCategory_Unchecked
        private void chkTbCategory_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_category = false;
            num_export_selected--;
        }

        //chkTbCustomer_Checked
        private bool flag_customer = false;
        private void chkTbCustomer_Checked(object sender, RoutedEventArgs e)
        {
            flag_customer = true;
            num_export_selected++;
        }

        //chkTbCustomer_Unchecked
        private void chkTbCustomer_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_customer = false;
            num_export_selected--;
        }


        //chkInputHistory_Checked
        private bool flag_inputhistory = false;
        private void chkTbInputHistory_Checked(object sender, RoutedEventArgs e)
        {
            flag_inputhistory = true;
            chkTbUser.IsChecked = true;
            chkTbProduct.IsChecked = true;
            num_export_selected++;
        }

        //chkTbInputHistory_Unchecked
        private void chkTbInputHistory_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_inputhistory = false;
            chkTbUser.IsChecked = false;
            chkTbProduct.IsChecked = false;
            num_export_selected--;
        }

        //chkTbOrder_Checked
        private bool flag_order = false;
        private void chkTbOrder_Checked(object sender, RoutedEventArgs e)
        {
            flag_order = true;
            chkTbOrderDetail.IsChecked = true;
            chkTbCustomer.IsChecked = true;
            chkTbSalesperson.IsChecked = true;
            chkTbPayment.IsChecked = true;
            chkTbCategory.IsChecked = true;
            chkTbProduct.IsChecked = true;
            num_export_selected++;
        }

        //chkTbOrder_Unchecked
        private void chkTbOrder_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_order = false;
            chkTbOrderDetail.IsChecked = false;
            chkTbCustomer.IsChecked = false;
            chkTbSalesperson.IsChecked = false;
            chkTbPayment.IsChecked = false;
            chkTbCategory.IsChecked = false;
            chkTbProduct.IsChecked = false;
            num_export_selected--;
        }

        //chkTbOrderDetail_Checked
        private bool flag_orderdetail = false;
        private void chkTbOrderDetail_Checked(object sender, RoutedEventArgs e)
        {
            flag_orderdetail = true;
            chkTbOrder.IsChecked = true;
            chkTbCustomer.IsChecked = true;
            chkTbSalesperson.IsChecked = true;
            chkTbPayment.IsChecked = true;
            chkTbCategory.IsChecked = true;
            chkTbProduct.IsChecked = true;
            num_export_selected++;
        }

        //chkTbOrderDetail_Unchecked
        private void chkTbOrderDetail_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_orderdetail = false;
            chkTbOrder.IsChecked = false;
            chkTbCustomer.IsChecked = false;
            chkTbSalesperson.IsChecked = false;
            chkTbPayment.IsChecked = false;
            chkTbCategory.IsChecked = false;
            chkTbProduct.IsChecked = false;
            num_export_selected--;
        }

        //chkTbPayment_Checked
        private bool flag_payment = false;
        private void chkTbPayment_Checked(object sender, RoutedEventArgs e)
        {
            flag_payment = true;
            num_export_selected++;
        }

        //chkTbPayment_Unchecked
        private void chkTbPayment_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_payment = false;
            num_export_selected--;
        }

        //chkTbProduct_Checked
        private bool flag_product = false;
        private void chkTbProduct_Checked(object sender, RoutedEventArgs e)
        {
            flag_product = true;
            num_export_selected++;
        }

        //chkTbProduct_Unchecked
        private void chkTbProduct_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_product = false;
            num_export_selected--;
        }

        //chkTbSalesperson_Checked
        private bool flag_salesperson = false;
        private void chkTbSalesperson_Checked(object sender, RoutedEventArgs e)
        {
            flag_salesperson = true;
            num_export_selected++;
        }

        //chkTbSalesperson_Unchecked
        private void chkTbSalesperson_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_salesperson = false;
            num_export_selected--;
        }

        //chkTbSetting_Checked
        private bool flag_setting = false;
        private void chkTbSetting_Checked(object sender, RoutedEventArgs e)
        {
            flag_setting = true;
            num_export_selected++;
        }

        //chkTbSetting_Unchecked
        private void chkTbSetting_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_setting = true;
            num_export_selected--;
        }

        //chkTbUser_Checked
        private bool flag_user = false;
        private void chkTbUser_Checked(object sender, RoutedEventArgs e)
        {
            flag_user = true;
            num_export_selected++;
        }

        //chkTbUser_Unchecked
        private void chkTbUser_Unchecked(object sender, RoutedEventArgs e)
        {
            flag_user = false;
            num_export_selected--;
        }

        //HandleCheck
        private void HandleCheck(bool ischeck)
        {
            chkTbCategory.IsChecked = ischeck;
            chkTbCustomer.IsChecked = ischeck;
            chkTbInputHistory.IsChecked = ischeck;
            chkTbOrder.IsChecked = ischeck;
            chkTbOrderDetail.IsChecked = ischeck; 
            chkTbPayment.IsChecked = ischeck;
            chkTbProduct.IsChecked = ischeck;
            chkTbSalesperson.IsChecked = ischeck;
            chkTbSetting.IsChecked = ischeck;
            chkTbUser.IsChecked = ischeck;
        }

    }
}
