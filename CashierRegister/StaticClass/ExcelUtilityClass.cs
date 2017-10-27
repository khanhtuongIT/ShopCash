using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace CashierRegister.StaticClass
{
    class ExcelUtilityClass
    {

        public bool WriteDataTableToExcel(DataTable data_table, string worksheet_name, string save_location, string report_type)
        {
            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(save_location);
            bool flag = false;
            string str = "";
            try
            {
                str = report_type + ", Date: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                streamwriter.WriteLine(str);

                str = "";
                for (int i = 0; i < data_table.Columns.Count; i++)
                {
                    if (i < data_table.Columns.Count - 1)
                        str += ReplaceComma(data_table.Columns[i].ColumnName.ToString()) + ",";
                    else
                        str += ReplaceComma(data_table.Columns[i].ColumnName.ToString());
                }

                streamwriter.WriteLine(str);

                foreach (DataRow dtr in data_table.Rows)
                {
                    str = "";
                    for (int i = 0; i < data_table.Columns.Count; i++)
                    {
                        if (i < data_table.Columns.Count - 1)
                            str += ReplaceComma(dtr[i].ToString()) + ",";
                        else
                            str += ReplaceComma(dtr[i].ToString());
                    }
                    streamwriter.WriteLine(str);
                }

                streamwriter.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                streamwriter.Close();
            }
            return flag;
        }

        //ReplaceComma
        private string ReplaceComma(string _str)
        {
            return _str.Replace(",", "-");
        }

        #region WriteDataTableToExcel
        //WriteDataTableToExcel
        //public bool WriteDataTableToExcel(DataTable data_table, string worksheet_name, string save_location, string report_type)
        //{
        //    bool flag = false;
        //    Microsoft.Office.Interop.Excel.Application excel;
        //    Microsoft.Office.Interop.Excel.Workbook excel_workbook;
        //    Microsoft.Office.Interop.Excel.Worksheet excel_worksheet;
        //    Microsoft.Office.Interop.Excel.Range excel_cellrange;
        //    try
        //    {
        //        //start excell and get application object
        //        excel = new Microsoft.Office.Interop.Excel.Application();

        //        //for making excel visible
        //        excel.Visible = false;
        //        excel.DisplayAlerts = false;

        //        //creation a new workbook
        //        excel_workbook = excel.Workbooks.Add(Type.Missing);

        //        //wordsheet
        //        excel_worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel_workbook.ActiveSheet;
        //        excel_worksheet.Name = worksheet_name;
        //        excel_worksheet.Cells[1, 1] = report_type;
        //        excel_worksheet.Cells[1, 2] = "Date: " + DateTime.Now.ToShortDateString();

        //        //loop through each row and value  to our sheet
        //        int rowcount = 2;
        //        foreach (DataRow datarow in data_table.Rows)
        //        {
        //            rowcount++;
        //            for (int i = 1; i <= data_table.Columns.Count; i++)
        //            {
        //                //on the first interation we add the column headers
        //                if (rowcount == 3)
        //                {
        //                    excel_worksheet.Cells[2, i] = data_table.Columns[i - 1].ColumnName;
        //                    excel_worksheet.Cells.Font.Color = System.Drawing.Color.Black;
        //                }
        //                excel_worksheet.Cells[rowcount, i] = datarow[i - 1].ToString();

        //                //for alternate rows
        //                if (rowcount > 3)
        //                {
        //                    if (i == data_table.Columns.Count)
        //                    {
        //                        if (rowcount % 2 == 0)
        //                        {
        //                            excel_cellrange = excel_worksheet.Range[excel_worksheet.Cells[rowcount, 1], excel_worksheet.Cells[rowcount, data_table.Columns.Count]];
        //                            FormattingExcelCells(excel_cellrange, "#FFF5F5F5", System.Drawing.Color.Black, false);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        //resize the columns
        //        excel_cellrange = excel_worksheet.Range[excel_worksheet.Cells[1, 1], excel_worksheet.Cells[rowcount, data_table.Columns.Count]];
        //        excel_cellrange.EntireColumn.AutoFit();
        //        Microsoft.Office.Interop.Excel.Borders border = excel_cellrange.Borders;
        //        border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //        border.Weight = 1;

        //        //format for row header
        //        excel_cellrange = excel_worksheet.Range[excel_worksheet.Cells[1, 1], excel_worksheet.Cells[2, data_table.Columns.Count]];
        //        FormattingExcelCells(excel_cellrange, "#e0dfdf", System.Drawing.Color.Black, true);

        //        //save the workbook and exit excel
        //        excel_workbook.SaveAs(save_location);
        //        excel_workbook.Close();
        //        excel.Quit();
        //        flag = true;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    finally
        //    {
        //        excel_cellrange = null;
        //        excel_worksheet = null;
        //        excel_workbook = null;
        //    }
        //    return flag;
        //}
        #endregion

        //FormattingExcelCells
        public void FormattingExcelCells(Microsoft.Office.Interop.Excel.Range range, string HTMLcolorcode, System.Drawing.Color fontcolor, bool isfontbool)
        {
            range.Interior.Color = System.Drawing.ColorTranslator.FromHtml(HTMLcolorcode);
            range.Font.Color = System.Drawing.ColorTranslator.ToOle(fontcolor);
            if (isfontbool == true)
                range.Font.Bold = isfontbool;
        }


        //GetDataTableToImport
        public DataTable GetDataTableToImport(string path_excel_file, string excel_worksheet)
        {
            string connectionstring = "";
            DataTable data_tablename = new DataTable();
            string file_type = System.IO.Path.GetExtension(path_excel_file).ToLower();
            if (file_type.Trim() == ".xlsx")
            {
                try
                {
                    connectionstring = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + path_excel_file + "; Extended Properties=\"Excel 12.0 Xml; HDR=YES\"";
                    string query = "SELECT * FROM [" + excel_worksheet + "$]";
                    System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(connectionstring);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(query, connection);
                    System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(cmd);
                    da.Fill(data_tablename);
                }
                catch (Exception) { }
            }

            return data_tablename;
        }

    }
}
