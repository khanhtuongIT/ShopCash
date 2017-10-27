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
using System.IO;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : ModernWindow
    {
        private string _orderTime = string.Empty;
        private string _orderDate = string.Empty;
        public ReportViewer()
        {
            InitializeComponent();
            _orderTime = (!string.IsNullOrEmpty(StaticClass.GeneralClass.app_settings["timeFormat"].ToString())) ? " " + StaticClass.GeneralClass.timeFromatSettings[StaticClass.GeneralClass.app_settings["timeFormat"]].ToString() : "";
            _orderDate = string.Format(@"{0:" + StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString() + _orderTime);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter streamWriter = new StreamWriter("abc.html", false, Encoding.UTF8))
            {
                streamWriter.WriteLine(CreateHTML());
                streamWriter.Close();
            }
        }

        private List<CashierRegisterEntity.EC_tb_OrderDetail> list_ec_tb_orderdetail1 = new List<CashierRegisterEntity.EC_tb_OrderDetail>()
        {
            new CashierRegisterEntity.EC_tb_OrderDetail()
            {
                 No =1,
                 ProductName = "Secondly the scrollviewer instruction is about giving the contained visual element infinite space",
                 Price = 120,
                 Qty = 10,
                 Subtotal = "143.100"
            },

            new CashierRegisterEntity.EC_tb_OrderDetail()
            {
                 No =2,
                 ProductName = "Secondly the scrollviewer instruction is about giving the contained visual element infinite space",
                 Price = 120,
                 Qty = 10,
                 Subtotal = "143.100"
            },

            new CashierRegisterEntity.EC_tb_OrderDetail()
            {
                 No =3,
                 ProductName = "Secondly the scrollviewer instruction is about giving the contained visual element infinite space",
                 Price = 120,
                 Qty = 10,
                 Subtotal = "143.100"
            },

            new CashierRegisterEntity.EC_tb_OrderDetail()
            {
                 No =4,
                 ProductName = "Secondly the scrollviewer instruction is about giving the contained visual element infinite space",
                 Price = 120,
                 Qty = 10,
                 Subtotal = "143.100"
            }
        };

        private List<CashierRegisterEntity.EC_tb_Order> list_ec_tb_order1 = new List<CashierRegisterEntity.EC_tb_Order>()
        { new CashierRegisterEntity.EC_tb_Order()
            {
                CustomerName = "Tuan",
                Phone = "01239441203",
                PaymentName = "American Express",
            }
        };
        int dtgorder_selected1 = 0;
        decimal subtotal1 = 800;
        decimal totaltax1 = 16;
        decimal totaldiscount1 = 300;
        decimal total_amount1 = 780;

        private string CreateHTML()
        {
            StringBuilder htmlBuilder = new StringBuilder();

            //create top portion of html document
            htmlBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
            htmlBuilder.Append("<head>");
            htmlBuilder.Append("<meta http-equiv='Content - Type' content='text / html; charset = utf - 8' />");
            htmlBuilder.Append("<title>Order details</title>");
            htmlBuilder.Append("</head>");
            htmlBuilder.Append("<body>");

            htmlBuilder.Append("<div style='width:99%; padding:0.5%; margin: 0 auto;'>");
            htmlBuilder.Append("<div style='text-align:center; font-size:25px; font-weight:bold; margin-bottom: 10px; '>" + StaticClass.GeneralClass.app_settings["storeName"].ToString() + " </div>");
            htmlBuilder.Append("<div style='text-align:center; color:#595757;padding:2px;'>" + StaticClass.GeneralClass.app_settings["storeAddress"].ToString() + " </div>");
            htmlBuilder.Append("<div style='text-align:center; color:#595757;padding:2px;'>" + FindResource("phone").ToString() + ": " + StaticClass.GeneralClass.app_settings["storePhone"].ToString() + " </div>");
            htmlBuilder.Append("<hr style='border-top:2px solid #000;margin:5px auto;'>");
            htmlBuilder.Append("<div style='text-align:center; font-size:18px; font-weight:bold; margin: 10px auto; '>" + FindResource("reciept").ToString() + "</div>");
            htmlBuilder.Append("<hr style='border-top:1px solid #000;margin:5px auto;'>");
            htmlBuilder.Append("<div style='padding:2px;'><b>" + FindResource("customer").ToString() + ": </b>" + list_ec_tb_order1[dtgorder_selected1].CustomerName + ", <b>" + FindResource("phone").ToString() + ": </b>" + list_ec_tb_order1[dtgorder_selected1].Phone + "</div>");
            htmlBuilder.Append("<div style='padding:2px;'><b>" + FindResource("payment").ToString() + ": </b>" + list_ec_tb_order1[dtgorder_selected1].PaymentName + "</div>");
            htmlBuilder.Append("<div style='padding:2px;'><b>" + FindResource("date").ToString() + ": </b>" + System.DateTime.Now.ToString(_orderDate) + "</div>");
            htmlBuilder.Append("<table style='border-collapse: collapse;margin-top:10px; width:100%'>");
            htmlBuilder.Append("<tr height='35'><th>" + FindResource("no_title").ToString() + "</th><th>" + FindResource("name").ToString() + "</th><th>" + FindResource("price").ToString() + "</th><th>" + FindResource("qty").ToString() + "</th><th>" + FindResource("subtotal").ToString() + "</th></tr>");

            foreach(var orderDetail in list_ec_tb_orderdetail1)
            {
                htmlBuilder.Append("<tr>");
                htmlBuilder.Append("<td style='text-align:center;'>" + orderDetail.No + "</td>");
                htmlBuilder.Append("<td>" + orderDetail.ProductName + "</td>");
                htmlBuilder.Append("<td style='text-align:center;'>" + StaticClass.GeneralClass.GetNumFormatDisplay(orderDetail.Price) + "</td>");
                htmlBuilder.Append("<td style='text-align:center;'>" + orderDetail.Qty + "</td>");
                htmlBuilder.Append("<td style='text-align:right;'>" + orderDetail.Subtotal + "</td>");
                htmlBuilder.Append("</tr>");

                htmlBuilder.Append("<tr>");
                htmlBuilder.Append("<td colspan='5'><hr style='border: 1px dashed #ababab; margin:3px auto; border-style: none none dashed; color: #fff; background-color: #fff;'></td>");
                htmlBuilder.Append("</tr>");
            }

            htmlBuilder.Append("<tr style='height:30px;'>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:20px; padding-right:20px; font-weight:bold;' colspan='4'>" + FindResource("subtotal").ToString() + "</td>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:20px;'>" + StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(subtotal1) + "</td>");
            htmlBuilder.Append("</tr>");

            htmlBuilder.Append("<tr style='height:30px;'>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px; padding-right:20px; font-weight:bold;' colspan='4'>" + FindResource("tax").ToString() + "(" + StaticClass.GeneralClass.GetNumFormatDisplay(StaticClass.GeneralClass.taxrate_setting_general) + "%)</td>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px;'>" + StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(totaltax1) + "</td>");
            htmlBuilder.Append("</tr>");

            htmlBuilder.Append("<tr style='height:30px;'>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px; padding-right:20px; font-weight:bold;' colspan='4'>" + FindResource("discount").ToString() + "</td>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px;'>" + StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(totaldiscount1) + "</td>");
            htmlBuilder.Append("</tr>");

            htmlBuilder.Append("<tr style='height:30px;'>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px; padding-right:20px; font-weight:bold; font-size: 20px;' colspan='4'>" + FindResource("total").ToString() + "</td>");
            htmlBuilder.Append("<td style='text-align:right; padding-top:7.5px;'>" + StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(total_amount1) + "</td>");
            htmlBuilder.Append("</tr>");

            htmlBuilder.Append("</table></br>");
            htmlBuilder.Append("<div style='margin-top:20px; text-align:center; color:#595757;'>THANK YOU - SEE YOU AGAIN</div>");
            htmlBuilder.Append("</body>");
            htmlBuilder.Append("</html>");

            return htmlBuilder.ToString();
        }

    }
}
