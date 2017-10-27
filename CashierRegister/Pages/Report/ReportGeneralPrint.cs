using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CashierRegisterBUS;
using System.Threading;

namespace CashierRegister.Pages.Report
{
    public partial class ReportGeneralPrint : Form
    {
        //using for order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();
        private System.Drawing.Color accent_color;
        public int point_x;
        public int point_y;
        private string _orderTime = string.Empty;
        private string _orderDate = string.Empty;

        //parameter
        public string titleParameter = "List of orders";
        public string idParameter = "ID";
        public string customerNameParameter = "Customer name";
        public string quantityParameter = "Quantity";
        public string orderDateParameter = "Order date";
        public string salespersonNameParameter = "Salesperson name";
        public string paymentParameter = "Payment";
        public string discountTypeParameter = "Discount type";
        public string discountParameter = "Discount";
        public string totalDiscountParameter = "Total discount";
        public string totalTaxParameter = "Total tax";
        public string totalAmountParameter = "Total amount";

        public ReportGeneralPrint()
        {
            InitializeComponent();
            _orderTime = (!string.IsNullOrEmpty(StaticClass.GeneralClass.app_settings["timeFormat"].ToString())) ? " " + StaticClass.GeneralClass.timeFromatSettings[StaticClass.GeneralClass.app_settings["timeFormat"]].ToString() : "";
            //get accent color
            accent_color = System.Drawing.ColorTranslator.FromHtml(StaticClass.GeneralClass.app_settings["accentColor"].ToString());
            panel1.BackColor = accent_color;
            panel2.BackColor = accent_color;
            panel3.BackColor = accent_color;
            panel4.BackColor = accent_color;

            ToolTip tt = new ToolTip();
            tt.BackColor = Color.White;
            tt.SetToolTip(this.imgClose, "Close");
            imgClose.BackColor = accent_color;
        }

        //ReportGeneralPrint_Load
        private void ReportGeneralPrint_Load(object sender, EventArgs e)
        {
            try
            {
                //set location for form
                this.Location = new Point(point_x, point_y);

                DataTable dt_tb_order = new DataTable();
                dt_tb_order = bus_tb_order.GetOrderPrint(StaticClass.GeneralClass.condition_report_order);

                DataTable dt_tb_cloned = dt_tb_order.Clone();
                dt_tb_cloned.Columns["DiscountType"].DataType = typeof(string);
                dt_tb_cloned.Columns["Discount"].DataType = typeof(string);
                dt_tb_cloned.Columns["TotalDiscount"].DataType = typeof(string);
                dt_tb_cloned.Columns["TotalTax"].DataType = typeof(string);
                dt_tb_cloned.Columns["TotalAmount"].DataType = typeof(string);

                foreach (DataRow row in dt_tb_order.Rows)
                {
                    dt_tb_cloned.ImportRow(row);
                }

                foreach (DataRow row in dt_tb_cloned.Rows)
                {
                    _orderDate = string.Format(@"{0:" + StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString() + _orderTime + "}", Convert.ToDateTime(row["OrderDate"].ToString()));
                    if (row["DiscountType"].ToString() == "1")
                        row["DiscountType"] = "%";
                    else
                        row["DiscountType"] = StaticClass.GeneralClass.currency_setting_general;

                    row["Discount"] = StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(row["Discount"].ToString()));
                    row["TotalDiscount"] = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(string.IsNullOrEmpty(row["TotalDiscount"].ToString()) ? "0" : row["TotalDiscount"].ToString()));
                    row["TotalTax"] = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(row["TotalTax"].ToString()));
                    row["TotalAmount"] = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(row["TotalAmount"].ToString()));
                    row["OrderDate"] = _orderDate;
                }

                this.rptOrders.RefreshReport();
                this.rptOrders.LocalReport.ReportEmbeddedResource = "CashierRegister.Pages.Report.rpt_Orders.rdlc";
                this.rptOrders.LocalReport.DataSources.Clear();
                Microsoft.Reporting.WinForms.ReportDataSource report_datasource = new Microsoft.Reporting.WinForms.ReportDataSource("DS_rpt_Order", dt_tb_cloned);
                this.rptOrders.LocalReport.DataSources.Add(report_datasource);

                Microsoft.Reporting.WinForms.ReportParameter[] rpt_parameter = new Microsoft.Reporting.WinForms.ReportParameter[]
                {
                    new Microsoft.Reporting.WinForms.ReportParameter("titleParameter", titleParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("idParameter", idParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("customerNameParameter", customerNameParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("quantityParameter", quantityParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("orderDateParameter", orderDateParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("salespersonNameParameter", salespersonNameParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("paymentParameter", paymentParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("discountTypeParameter", discountTypeParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("discountParameter", discountParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("totalDiscountParameter", totalDiscountParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("totalTaxParameter", totalTaxParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("totalAmountParameter", totalAmountParameter),
                };
                this.rptOrders.LocalReport.SetParameters(rpt_parameter);
                this.rptOrders.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //imgClose_Click
        private void imgClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //imgClose_MouseHover
        private void imgClose_MouseHover(object sender, EventArgs e)
        {
            imgClose.BackColor = Color.White;
        }

        //imgClose_MouseLeave
        private void imgClose_MouseLeave(object sender, EventArgs e)
        {
            imgClose.BackColor = accent_color;
        }

        //imgClose_MouseDown
        private void imgClose_MouseDown(object sender, MouseEventArgs e)
        {
            imgClose.BackColor = Color.White;
        }

    }
}
