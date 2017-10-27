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

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for BackupSQLServer.xaml
    /// </summary>
    public partial class BackupSQLServer : UserControl
    {
        //using for table user
        private BUS_tb_User bus_tb_user = new BUS_tb_User();

        //using for table setting
        private BUS_tb_Setting bus_tb_setting = new BUS_tb_Setting();

        //using for table category
        private BUS_tb_Category bus_tb_category = new BUS_tb_Category();

        //using for table customer
        private BUS_tb_Customer bus_tb_customer = new BUS_tb_Customer();

        //using for table inputhistory
        private BUS_tb_InputHistory bus_tb_inputdistory = new BUS_tb_InputHistory();

        //using for table order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //using for table orderdetail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //using for table payment
        private BUS_tb_Payment bus_tb_payment = new BUS_tb_Payment();

        //using for table product
        private BUS_tb_Product bus_tb_product = new BUS_tb_Product();

        //using for table salesperson
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();


        //BackupSQLServer
        public BackupSQLServer()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.IO.StreamWriter stream_writer = new System.IO.StreamWriter(@"G:\CheckOut.sql");

            //export table user
            stream_writer.WriteLine("-- table dbo.tb_User");
            //delete data from table user
            stream_writer.WriteLine("delete * from [tb_User]");
            stream_writer.WriteLine("");
            DataTable tb_user = new DataTable();
            tb_user = bus_tb_user.GetUser("");
            foreach(DataRow dr in tb_user.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_User] ([ID],[Name],[Email],[Address],[Password],[Question],[Answer]) VALUES (" + dr["ID"].ToString() + ",'" + dr["Name"].ToString() + "','" + dr["Email"].ToString() + "','" + dr["Address"].ToString() + "','" + dr["Password"].ToString() + "','" + dr["Question"].ToString() + "','" + dr["Answer"].ToString() + "');");
            }

            //export table setting
            stream_writer.WriteLine("");
            stream_writer.WriteLine("");
            stream_writer.WriteLine("-- table dbo.tb_Setting");
            //delete data from table Setting
            stream_writer.WriteLine("delete * from [tb_Setting]");
            stream_writer.WriteLine("");
            DataTable tb_setting = new DataTable();
            tb_setting = bus_tb_setting.GetSetting("");
            foreach (DataRow dr in tb_setting.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_Setting] ([SettingID],[Currency],[TaxRate],[Active],[Version]) VALUES (" + dr["SettingID"].ToString() + ",'" + dr["Currency"].ToString() + "'," + dr["TaxRate"].ToString() + "," + dr["Active"].ToString() + "," + dr["Version"].ToString() + ");");
            }

            //export table category
            stream_writer.WriteLine("");
            stream_writer.WriteLine("");
            stream_writer.WriteLine("-- table dbo.tb_Category");
            //delete data from table category
            stream_writer.WriteLine("delete * from [tb_Category]");
            stream_writer.WriteLine("");
            DataTable tb_category = new DataTable();
            tb_category = bus_tb_category.GetCatagorySetting("");
            foreach (DataRow dr in tb_category.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_Category] ([CategoryID],[CategoryName]) VALUES (" + dr["CategoryID"].ToString() + ",'" + dr["CategoryName"].ToString() + "');");
            }

            //export table customer
            stream_writer.WriteLine("");
            stream_writer.WriteLine("");
            stream_writer.WriteLine("-- table dbo.tb_Customer");
            //delete data from table customer
            stream_writer.WriteLine("delete * from [tb_Customer]");
            stream_writer.WriteLine("");
            DataTable tb_customer = new DataTable();
            tb_customer = bus_tb_customer.GetCustomer("");
            foreach (DataRow dr in tb_customer.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_Customer] ([CustomerID],[FirstName],[LastName],[Address1],[Address2],[City],[State],[Zipcode],[Phone],[Email]) VALUES (" + dr["CustomerID"].ToString() + ",'" + dr["FirstName"].ToString() + "','" + dr["LastName"].ToString() + "','" + dr["Address1"].ToString() + "','" + dr["Address2"].ToString() + "','" + dr["City"].ToString() + "','" + dr["State"].ToString() + "','" + dr["Zipcode"].ToString() + "','" + dr["Phone"].ToString() + "','" + dr["Email"].ToString() + "');");
            }

            //export table inputhistory
            stream_writer.WriteLine("");
            stream_writer.WriteLine("");
            stream_writer.WriteLine("-- table dbo.tb_InputHistory");
            //delete data from table inputhistory
            stream_writer.WriteLine("delete * from [tb_InputHistory]");
            stream_writer.WriteLine("");
            DataTable tb_inputhistory = new DataTable();
            tb_inputhistory = bus_tb_inputdistory.GetAllInputHistory("");
            foreach (DataRow dr in tb_inputhistory.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_InputHistory] ([InputID],[ProductID],[ProductName],[InputDate],[UserID],[UserName],[Cost],[Price],[InventoryCount],[CategoryID],[CategoryName],[Tax],[Active],[Country],[Size_Weight]) VALUES (" + dr["InputID"].ToString() + "," + dr["ProductID"].ToString() + ",'" + dr["ProductName"].ToString() + "','" + dr["InputDate"].ToString() +"'," + dr["UserID"].ToString() + ",'" + dr["UserName"].ToString() + "'," + dr["Cost"].ToString() + "," + dr["Price"].ToString() + "," + dr["InventoryCount"].ToString() + "," + dr["CategoryID"].ToString() + ",'" + dr["CategoryName"].ToString() + "'," + dr["Tax"].ToString() + "," + dr["Active"].ToString() + ",'" + dr["Country"].ToString() + "','" + dr["Size_Weight"].ToString() + "');");
            }

            //export table order 
            stream_writer.WriteLine("");
            stream_writer.WriteLine("");
            stream_writer.WriteLine("-- table dbo.tb_Order");
            //delete data from table order
            stream_writer.WriteLine("delete * from [tb_Order]");
            stream_writer.WriteLine("");
            DataTable tb_order = new DataTable();
            tb_order = bus_tb_order.GetOrder("");
            foreach (DataRow dr in tb_order.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_Order] ([OrderID],[CustomerID],[CustomerName],[Quantity],[OrderDate],[SalespersonID],[SalespersonName],[PaymentID],[PaymentName],[DiscountType],[Discount],[TotalDiscount],[TotalTax],[TotalAmount]) VALUES (" + dr["OrderID"].ToString() + "," + dr["CustomerID"].ToString() + ",'" + dr["CustomerName"].ToString() + "', " + dr["Quantity"].ToString() + ",'" + dr["OrderDate"].ToString() + "', " + dr["SalespersonID"].ToString() + ", '" + dr["SalespersonName"].ToString() + "', " + dr["PaymentID"].ToString() + ",'" + dr["PaymentName"].ToString() + ", " + dr["DiscountType"].ToString() + "," + dr["Discount"].ToString() + "," + dr["TotalDiscount"].ToString() + "," + dr["TotalTax"].ToString() + "," + dr["TotalAmount"].ToString() + ");");
            }

            //export table orderdetail
            stream_writer.WriteLine("");
            stream_writer.WriteLine("");
            stream_writer.WriteLine("-- table dbo.tb_OrderDetail");
            //delete data from table orderdetail
            stream_writer.WriteLine("delete * from [tb_OrderDetail]");
            stream_writer.WriteLine("");
            DataTable tb_orderdetail = new DataTable();
            tb_orderdetail = bus_tb_orderdetail.GetOrderDetail("");
            foreach (DataRow dr in tb_orderdetail.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_OrderDetail] ([ID],[CategoryID],[CategoryName],[ProductID],[ProductName],[Cost],[Price],[Qty],[Tax],[DiscountType],[Discount],[TotalDiscount],[Total],[OrderID]) VALUES (" + dr["ID"].ToString() + "," + dr["CategoryID"].ToString() + ",'" + dr["CategoryName"].ToString() + "'," + dr["ProductID"].ToString() + ", '" + dr["ProductName"].ToString() + "'," + dr["Cost"].ToString() + "," + dr["Price"].ToString() + "," + dr["Qty"].ToString() + "," + dr["Tax"].ToString() + "," + dr["DiscountType"].ToString() + "," + dr["Discount"].ToString() + "," + dr["TotalDiscount"].ToString() + "," + dr["Total"].ToString() + "," + dr["OrderID"].ToString() + ");");
            }

            //export table payment
            stream_writer.WriteLine("");
            stream_writer.WriteLine("");
            stream_writer.WriteLine("-- table dbo.tb_Payment");
            //delete data from table payment
            stream_writer.WriteLine("delete * from [tb_Payment]");
            stream_writer.WriteLine("");
            DataTable tb_payment = new DataTable();
            tb_payment = bus_tb_payment.GetPayment("");
            foreach (DataRow dr in tb_payment.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_Payment] ([PaymentID],[Card]) VALUES (" + dr["PaymentID"].ToString() + ",'" + dr["Card"].ToString() + "');");
            }

            //export table product str
            stream_writer.WriteLine("");
            stream_writer.WriteLine("");
            stream_writer.WriteLine("-- table dbo.tb_Product");
            //delete data from table product
            stream_writer.WriteLine("delete * from [tb_Product]");
            stream_writer.WriteLine("");
            DataTable tb_product = new DataTable();
            tb_product = bus_tb_product.GetProduct("");
            foreach (DataRow dr in tb_product.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_Product] ([ProductID],[BarcodeID],[ShortName],[LongName],[Cost],[Price],[InventoryCount],[CategoryID],[Tax],[PathImage],[Capture],[Active],[Country],[Size_Weight]) VALUES (" + dr["ProductID"].ToString() + ", '" + dr["BarcodeID"].ToString() + "','" + dr["ShortName"].ToString() + "','" + dr["LongName"].ToString() + "'," + dr["Cost"].ToString() + "," + dr["Price"].ToString() + "," + dr["InventoryCount"].ToString() + "," + dr["CategoryID"].ToString() + "," + dr["Tax"].ToString() + ",'" + dr["PathImage"].ToString() + "'," + dr["Capture"].ToString() + "," + dr["Active"].ToString() + ",'" + dr["Country"].ToString() + "','" + dr["Size_Weight"].ToString() + "');");
            }

            //export table salesperson
            stream_writer.WriteLine("");
            stream_writer.WriteLine("");
            stream_writer.WriteLine("-- table dbo.tb_Salesperson");
            //delete data from table salesperson
            stream_writer.WriteLine("delete * from [tb_Salesperson]");
            stream_writer.WriteLine("");
            DataTable tb_salesperson = new DataTable();
            tb_salesperson = bus_tb_salesperson.GetSalesPerson("");
            foreach (DataRow dr in tb_salesperson.Rows)
            {
                stream_writer.WriteLine("INSERT INTO [tb_Salesperson] ([SalespersonID],[Name],[Birthday],[Address],[Email],[Password],[Active],[Default]) VALUES (" + dr["SalespersonID"].ToString() + ", '" + dr["Name"].ToString() + "', '" + dr["Birthday"].ToString() + "', '" + dr["Address"].ToString() + "', '" + dr["Email"].ToString() + "', '" + dr["Password"].ToString() + "', " + dr["Active"].ToString() + "," + dr["Default"].ToString() + ");");
            }
            stream_writer.Close();
        }
    }
}
