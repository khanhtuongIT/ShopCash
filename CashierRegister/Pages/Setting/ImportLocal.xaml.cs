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
using CashierRegisterBUS;
using CashierRegisterEntity;
using System.Threading;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for ImportLocal.xaml
    /// </summary>
    public partial class ImportLocal : UserControl
    {
        //using for current direction
        private System.Windows.Forms.OpenFileDialog openfiledialog;

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

        //using for excel utility
        StaticClass.ExcelUtilityClass excel_utility = new StaticClass.ExcelUtilityClass();

        //ImportLocal
        public ImportLocal()
        {
            InitializeComponent();

            #region UserControl_Loaded
            BitmapImage bitmap_image = new BitmapImage(new Uri(@"pack://application:,,,/Resources/import.png", UriKind.Absolute));
            imgTbCategory.Source = bitmap_image;
            imgTbCustomer.Source = bitmap_image;
            imgTbInputDetail.Source = bitmap_image;
            imgTbOrder.Source = bitmap_image;
            imgTbOrderDetail.Source = bitmap_image;
            imgTbPayment.Source = bitmap_image;
            imgTbProduct.Source = bitmap_image;
            imgTbSalesperson.Source = bitmap_image;
            imgTbSetting.Source = bitmap_image;
            imgTbUser.Source = bitmap_image;
            #endregion
        }

        //GetTableImport
        private DataTable data_table;
        private DataTable GetTableImport(string excel_worksheet)
        {
            try
            {
                data_table = new DataTable();
                openfiledialog = new System.Windows.Forms.OpenFileDialog();
                openfiledialog.Filter = "Excel (*.xlsx)|*.xlsx";

                if (openfiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    data_table = excel_utility.GetDataTableToImport(openfiledialog.FileName, excel_worksheet);
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
            return data_table;
        }

        #region import category
        //btnTbCategory_Click
        private void btnTbCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_category = new DataTable();
                tb_category = GetTableImport("tb_Category");

                if (tb_category.Rows.Count > 0)
                {
                    if (bus_tb_category.GetCatagorySetting("").Rows.Count > 0)
                        bus_tb_category.DeleteAllCategory();
                }

                if (InSertAllCategory(tb_category) > 0)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = "Import success!";
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //InSertAllCategory
        private int InSertAllCategory(DataTable dt_category)
        {
            int result = 0;
            foreach (DataRow datarow in dt_category.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    EC_tb_Category ec_tb_category = new EC_tb_Category();
                    ec_tb_category.CategoryID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_category.CategoryName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[1].ToString());

                    if (bus_tb_category.InsertCategory(ec_tb_category, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }

        #endregion

        #region import customer
        //btnTbCustomer_Click
        private void btnTbCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_customer = new DataTable();
                tb_customer = GetTableImport("tb_Customer");

                if (tb_customer.Rows.Count > 0)
                {
                    if (bus_tb_customer.GetCustomer("").Rows.Count > 0)
                        bus_tb_customer.DeleteAllCustomer();
                }

                if (InSertAllCustomer(tb_customer) > 0)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = "Import success!";
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
            
        }

        //InSertAllCustomer
        private int InSertAllCustomer(DataTable tb_customer)
        {
            int result = 0;
            foreach (DataRow datarow in tb_customer.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    EC_tb_Customer ec_tb_customer = new EC_tb_Customer();
                    ec_tb_customer.CustomerID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_customer.FirstName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[1].ToString());
                    ec_tb_customer.LastName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[2].ToString());
                    ec_tb_customer.Address1 = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[3].ToString());
                    ec_tb_customer.Address2 = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[4].ToString());
                    ec_tb_customer.City = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[5].ToString());
                    ec_tb_customer.State = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[6].ToString());
                    ec_tb_customer.Zipcode = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[7].ToString());
                    ec_tb_customer.Phone = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[8].ToString());
                    ec_tb_customer.Email = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[9].ToString());

                    if (bus_tb_customer.InsertCustomer(ec_tb_customer, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }

        #endregion

        #region import input detail
        //btnTbInputDetail_Click
        private void btnTbInputDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_inputhistory = new DataTable();
                tb_inputhistory = GetTableImport("tb_InputHistory");

                if (tb_inputhistory.Rows.Count > 0)
                {
                    if (bus_tb_inputhistory.GetInputHistory("").Rows.Count > 0)
                        bus_tb_inputhistory.DeleteAllInputHistory();
                }

                if (InSertAllInputHistory(tb_inputhistory) > 0)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = "Import success!";
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }

        }

        //InSertAllInputDetail
        private int InSertAllInputHistory(DataTable tb_inputhistory)
        {
            int result = 0;
            foreach (DataRow datarow in tb_inputhistory.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    EC_tb_InputHistory ec_tb_inputhistory = new EC_tb_InputHistory();
                    ec_tb_inputhistory.InputID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_inputhistory.ProductID = Convert.ToInt32(datarow[1].ToString());
                    ec_tb_inputhistory.InputDate = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[2].ToString());
                    ec_tb_inputhistory.UserID = Convert.ToInt32(datarow[3].ToString());
                    ec_tb_inputhistory.UserName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[4].ToString());
                    ec_tb_inputhistory.Cost = Convert.ToDecimal(datarow[5].ToString());
                    ec_tb_inputhistory.Price = Convert.ToDecimal(datarow[6].ToString());
                    ec_tb_inputhistory.InventoryCount = Convert.ToInt32(datarow[7].ToString());
                    ec_tb_inputhistory.CategoryID = Convert.ToInt32(datarow[8].ToString());
                    ec_tb_inputhistory.Tax = Convert.ToInt32(datarow[9].ToString());
                    ec_tb_inputhistory.Active = Convert.ToInt32(datarow[10].ToString());
                    ec_tb_inputhistory.Country = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[11].ToString());
                    ec_tb_inputhistory.SizeWeight = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[12].ToString());

                    if (bus_tb_inputhistory.InsertInputHistory(ec_tb_inputhistory, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }

        #endregion

        #region import order
        //btnTbOrder_Click
        private void btnTbOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_order = new DataTable();
                tb_order = GetTableImport("tb_Order");

                if (tb_order.Rows.Count > 0)
                {
                    if (bus_tb_order.GetOrder("").Rows.Count > 0)
                        bus_tb_order.DeleteAllOrder();
                }

                if (InSertAllOrder(tb_order) > 0)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = "Import success!";
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {

                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //InSertAllOrderDetail
        private int InSertAllOrder(DataTable tb_order)
        {
            int result = 0;
            foreach (DataRow datarow in tb_order.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    EC_tb_Order ec_tb_order = new EC_tb_Order();
                    ec_tb_order.OrderID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_order.CustomerID = Convert.ToInt32(datarow[1].ToString());
                    ec_tb_order.CustomerName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[2].ToString());
                    ec_tb_order.Quatity = Convert.ToInt32(datarow[3].ToString());
                    ec_tb_order.OrderDate = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[4].ToString());
                    ec_tb_order.SalesPersonID = Convert.ToInt32(datarow[5].ToString());
                    ec_tb_order.SalesPersonName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[6].ToString());
                    ec_tb_order.PaymentID = Convert.ToInt32(datarow[7].ToString());
                    ec_tb_order.PaymentName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[8].ToString());
                    ec_tb_order.DiscountType = Convert.ToInt32(datarow[9].ToString());
                    ec_tb_order.Discount = Convert.ToDecimal(datarow[10].ToString());
                    ec_tb_order.TotalDiscount = Convert.ToDecimal(datarow[11].ToString());
                    ec_tb_order.TotalTax = Convert.ToDecimal(datarow[12].ToString());
                    ec_tb_order.TotalAmount = Convert.ToDecimal(datarow[13].ToString());

                    if (bus_tb_order.InsertOrder(ec_tb_order, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }
        #endregion

        #region import orderdetail
        //btnTbOrderDetail_Click
        private void btnTbOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_orderdetail = new DataTable();
                tb_orderdetail = GetTableImport("tb_OrderDetail");

                if (tb_orderdetail.Rows.Count > 0)
                {
                    if (bus_tb_orderdetail.GetOrderDetail("").Rows.Count > 0)
                        bus_tb_orderdetail.DeleteAllOrderDetail("");
                }

                System.Diagnostics.Debug.WriteLine(tb_orderdetail.Rows.Count);
                if (InSertAllOrderDetail(tb_orderdetail) > 0)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = "Import success!";
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message + "ex";
                page.ShowDialog();
            }
        }

        //InSertAllOrderDetail
        private int InSertAllOrderDetail(DataTable tb_orderdetail)
        {
            int result = 0;
            foreach (DataRow datarow in tb_orderdetail.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    EC_tb_OrderDetail ec_tb_orderdetail = new EC_tb_OrderDetail();
                    ec_tb_orderdetail.ID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_orderdetail.CategoryID = Convert.ToInt32(datarow[1].ToString());
                    ec_tb_orderdetail.CategoryName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[2].ToString());
                    ec_tb_orderdetail.ProductID = Convert.ToInt32(datarow[3].ToString());
                    ec_tb_orderdetail.ProductName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[4].ToString());
                    ec_tb_orderdetail.Cost = Convert.ToDecimal(datarow[5].ToString());
                    ec_tb_orderdetail.Price = Convert.ToDecimal(datarow[6].ToString());
                    ec_tb_orderdetail.Qty = Convert.ToInt32(datarow[7].ToString());
                    ec_tb_orderdetail.Tax = Convert.ToDecimal(datarow[8].ToString());
                    ec_tb_orderdetail.DiscountType = Convert.ToInt32(datarow[9].ToString());
                    ec_tb_orderdetail.Discount = Convert.ToDecimal(datarow[10].ToString());
                    ec_tb_orderdetail.TotalDiscount = Convert.ToDecimal(datarow[11].ToString());
                    ec_tb_orderdetail.Total = Convert.ToDecimal(datarow[12].ToString());
                    ec_tb_orderdetail.OrderID = Convert.ToInt32(datarow[13].ToString());

                    if (bus_tb_orderdetail.InsertOrderDetail(ec_tb_orderdetail, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }

        #endregion

        #region import payment
        //btnTbPayment_Click
        private void btnTbPayment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_payment = new DataTable();
                tb_payment = GetTableImport("tb_Payment");

                if (tb_payment.Rows.Count > 0)
                {
                    if (bus_tb_payment.GetPayment("").Rows.Count > 0)
                        bus_tb_payment.DeleteAllPayment();

                    if (InSertAllPayment(tb_payment) > 0)
                    {
                        Pages.Notification page = new Pages.Notification();
                        page.tblNotification.Text = "Import success!";
                        page.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //InSertAllPayment
        private int InSertAllPayment(DataTable tb_payment)
        {
            int result = 0;

            foreach (DataRow datarow in tb_payment.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    EC_tb_Payment ec_tb_payment = new EC_tb_Payment();
                    ec_tb_payment.PaymentID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_payment.Card = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[1].ToString());

                    if (bus_tb_payment.InsertPayment(ec_tb_payment, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }

        #endregion

        #region import product
        //btnTbProduct_Click
        private void btnTbProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_product = new DataTable();
                tb_product = GetTableImport("tb_Product");

                if (tb_product.Rows.Count > 0)
                {
                    if (bus_tb_product.GetProduct("").Rows.Count > 0)
                        bus_tb_product.DeleteAllProduct();
                }

                if (InSertAllProduct(tb_product) > 0)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = "Import success!";
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //InSertAllProduct
        private int InSertAllProduct(DataTable tb_product)
        {
            int result = 0;

            foreach (DataRow datarow in tb_product.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    CashierRegisterEntity.EC_tb_Product ec_tb_product = new CashierRegisterEntity.EC_tb_Product();
                    ec_tb_product.ProductID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_product.BarcodeID = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[1].ToString());
                    ec_tb_product.ShortName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[2].ToString());
                    ec_tb_product.LongName = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[3].ToString());
                    ec_tb_product.Price = Convert.ToDecimal(datarow[4].ToString());
                    ec_tb_product.CategoryID = Convert.ToInt32(datarow[5].ToString());
                    ec_tb_product.Tax = Convert.ToInt32(datarow[6].ToString());
                    ec_tb_product.PathImage = datarow[7].ToString();
                    ec_tb_product.Capture = Convert.ToInt32(datarow[8].ToString());
                    ec_tb_product.Active = Convert.ToInt32(datarow[9].ToString());

                    if (bus_tb_product.InsertProduct(ec_tb_product, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }

        #endregion

        #region import salesperson
        //btnTbSalesperson_Click
        private void btnTbSalesperson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_salesperson = new DataTable();
                tb_salesperson = GetTableImport("tb_Salesperson");

                if (tb_salesperson.Rows.Count > 0)
                {
                    if (bus_tb_salesperson.GetSalesPerson("").Rows.Count > 0)
                        bus_tb_salesperson.DeleteAllSalesPerson();
                }

                if (InSertAllSalesperson(tb_salesperson) > 0)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = "Import success!";
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //InSertAllSalesperson
        private int InSertAllSalesperson(DataTable tb_salesperson)
        {
            int result = 0;

            foreach (DataRow datarow in tb_salesperson.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    EC_tb_SalesPerson ec_tb_salesperson = new EC_tb_SalesPerson();
                    ec_tb_salesperson.SalespersonID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_salesperson.Name = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[1].ToString());
                    ec_tb_salesperson.Birthday = datarow[2].ToString();
                    ec_tb_salesperson.Address = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[3].ToString());
                    ec_tb_salesperson.Email = StaticClass.GeneralClass.HandlingSpecialCharacter(datarow[4].ToString());
                    ec_tb_salesperson.Password = datarow[5].ToString();
                    ec_tb_salesperson.Active = Convert.ToInt32(datarow[6].ToString());
                    ec_tb_salesperson.Defaul = Convert.ToInt32(datarow[7].ToString());

                    if (bus_tb_salesperson.InsertSalesPerson(ec_tb_salesperson, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }

        #endregion

        #region import setting
        //btnTbSetting_Click
        private void btnTbSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_setting = new DataTable();
                tb_setting = GetTableImport("tb_Currency");

                if (tb_setting.Rows.Count > 0)
                {
                    if (bus_tb_setting.GetSetting("").Rows.Count > 0)
                        bus_tb_setting.DeleteAllSetting();
                }

                if (InSertAllSetting(tb_setting) > 0)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = "Import success!";
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //InSertAllSetting
        private int InSertAllSetting(DataTable tb_setting)
        {
            int result = 0;

            foreach (DataRow datarow in tb_setting.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    EC_tb_Setting ec_tb_setting = new EC_tb_Setting();
                    ec_tb_setting.SettingID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_setting.Currency = datarow[1].ToString();
                    ec_tb_setting.TaxRate = Convert.ToDecimal(datarow[2].ToString());
                    ec_tb_setting.Active = Convert.ToInt32(datarow[3].ToString());
                    ec_tb_setting.Version = Convert.ToInt32(datarow[4].ToString());

                    if (bus_tb_setting.InsertSetting(ec_tb_setting, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }

        #endregion

        #region import user
        //btnTbUser_Click
        private void btnTbUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tb_user = new DataTable();
                tb_user = GetTableImport("tb_User");

                if (tb_user.Rows.Count > 0)
                {
                    if (bus_tb_user.GetUser("").Rows.Count > 0)
                        bus_tb_user.DeleteAllUser();
                }

                if (InSertAllUser(tb_user) > 0)
                {
                    Pages.Notification page = new Pages.Notification();
                    page.tblNotification.Text = "Import success!";
                    page.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = ex.Message;
                page.ShowDialog();
            }
        }

        //InSertAllUser
        private int InSertAllUser(DataTable tb_user)
        {
            int result = 0;

            foreach (DataRow datarow in tb_user.Rows)
            {
                if (datarow[0].ToString() != "")
                {
                    EC_tb_User ec_tb_user = new EC_tb_User();
                    ec_tb_user.ID = Convert.ToInt32(datarow[0].ToString());
                    ec_tb_user.Name = datarow[1].ToString();
                    ec_tb_user.Email = datarow[2].ToString();
                    ec_tb_user.Address = datarow[3].ToString();
                    ec_tb_user.Password = datarow[4].ToString();
                    ec_tb_user.Question = datarow[5].ToString();
                    ec_tb_user.Answer = datarow[6].ToString();

                    if (bus_tb_user.InsertUser(ec_tb_user, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        result++;
                }
            }
            return result;
        }
        #endregion
    }
}
