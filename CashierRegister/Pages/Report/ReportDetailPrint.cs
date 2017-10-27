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

namespace CashierRegister.Pages.Report
{
    public partial class ReportDetailPrint : Form
    {
        //using for order detail
        public int point_x;
        public int point_y;
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();
        private System.Drawing.Color accent_color;

        //parameter
        public string titleParameter = "Order details";
        public string idParameter = "ID";
        public string categoryNameParameter = "Category name";
        public string productIDParameter = "Product ID";
        public string productNameParameter = "Product name";
        public string priceParameter = "Price";
        public string qtyParameter = "Qty";
        public string taxParameter = "Tax";
        public string discountTypeParameter = "Discount type";
        public string discountParameter = "Discount";
        public string totalParameter = "Total";

        //ReportDetailPrint
        public ReportDetailPrint()
        {
            InitializeComponent();
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

        //ReportDetailPrint_Load
        private void ReportDetailPrint_Load(object sender, EventArgs e)
        {
            try
            {
                //set location for form
                this.Location = new Point(point_x, point_y);

                DataTable dt_tb_orderdetail = new DataTable();
                dt_tb_orderdetail = bus_tb_orderdetail.GetOrderDetail("Where [OrderID]=" + StaticClass.GeneralClass.orderdetailid_general);

                DataTable dt_tb_cloned = dt_tb_orderdetail.Clone();
                dt_tb_cloned.Columns["Price"].DataType = typeof(string);
                dt_tb_cloned.Columns["Tax"].DataType = typeof(string);
                dt_tb_cloned.Columns["DiscountType"].DataType = typeof(string);
                dt_tb_cloned.Columns["Discount"].DataType = typeof(string);
                dt_tb_cloned.Columns["Total"].DataType = typeof(string);

                foreach (DataRow row in dt_tb_orderdetail.Rows)
                {
                    dt_tb_cloned.ImportRow(row);
                }

                foreach (DataRow row in dt_tb_cloned.Rows)
                {
                    row["Price"] = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(row["Price"].ToString()));
                    row["Tax"] = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(row["Tax"].ToString()));
                    if (row["DiscountType"].ToString() == "1")
                        row["DiscountType"] = "%";
                    else
                        row["DiscountType"] = StaticClass.GeneralClass.currency_setting_general;
                    row["Discount"] = StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(row["Discount"].ToString()));
                    row["Total"] = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(row["Total"].ToString()));
                }

                this.rpvOrderDetail.RefreshReport();
                rpvOrderDetail.LocalReport.ReportEmbeddedResource = "CashierRegister.Pages.Report.rpt_OrderDetails.rdlc";
                rpvOrderDetail.LocalReport.DataSources.Clear();



                Microsoft.Reporting.WinForms.ReportDataSource reportdatasource = new Microsoft.Reporting.WinForms.ReportDataSource("DS_rpt_OrderDetail", dt_tb_cloned);
                rpvOrderDetail.LocalReport.DataSources.Add(reportdatasource);

                Microsoft.Reporting.WinForms.ReportParameter[] rpt_parameter = new Microsoft.Reporting.WinForms.ReportParameter[]
                {
                    new Microsoft.Reporting.WinForms.ReportParameter("titleParameter", titleParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("idParameter", idParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("categoryNameParameter", categoryNameParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("productIDParameter", productIDParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("productNameParameter", productNameParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("priceParameter", priceParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("qtyParameter", qtyParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("taxParameter", taxParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("discountTypeParameter", discountTypeParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("discountParameter", discountParameter),
                    new Microsoft.Reporting.WinForms.ReportParameter("totalParameter", totalParameter),

                };
                this.rpvOrderDetail.LocalReport.SetParameters(rpt_parameter);
                rpvOrderDetail.RefreshReport();
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

        //imgClose_MouseDown
        private void imgClose_MouseDown(object sender, MouseEventArgs e)
        {
            imgClose.BackColor = Color.White;
        }

        //imgClose_MouseLeave
        private void imgClose_MouseLeave(object sender, EventArgs e)
        {
            imgClose.BackColor = accent_color;
        }
    }
}