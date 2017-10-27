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
using System.Threading;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for InputHistory.xaml
    /// </summary>
    public partial class InputHistory : ModernWindow
    {
        private BUS_tb_InputHistory bus_tb_input_history = new BUS_tb_InputHistory();
        private List<EC_tb_InputHistory> list_ec_tb_inputhistory = new List<EC_tb_InputHistory>();
        public int categoryid = 0;
        public int productid = 0;
        public string product_name = "";

        //InputHistory
        public InputHistory()
        {
            InitializeComponent();
        }

        //ModernWindow_Loaded
        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                this.dtgInputHistory.Dispatcher.Invoke((Action)(() =>
                {
                    this.dtgInputHistory.Visibility = System.Windows.Visibility.Collapsed;
                    this.dtgInputHistory.ItemsSource = null;
                    this.mpr.IsActive = true;
                }));
                GetListInputHistory(productid, categoryid);
                Thread.Sleep(500);
                this.dtgInputHistory.Dispatcher.Invoke((Action)(() =>
                {
                    this.mpr.IsActive = false;
                    this.dtgInputHistory.Visibility = System.Windows.Visibility.Visible;
                    this.dtgInputHistory.ItemsSource = list_ec_tb_inputhistory;
                }));
            }).Start();
        }

        //GetListInputHistory
        private void GetListInputHistory(int productid, int categoryid)
        {
            list_ec_tb_inputhistory.Clear();
            string condition = "";
            if ((productid == 0) && (categoryid != 0))
                condition = "where CategoryID = " + categoryid + " and ProductName like '%" + product_name + "%' ";

            if ((productid != 0) && categoryid == 0)
                condition = "where ProductID = " + productid;

            DataTable tb_input_history = new DataTable();
            tb_input_history = bus_tb_input_history.GetInputHistory(condition);
            int no = 0;
            foreach (DataRow dr in tb_input_history.Rows)
            {
                no++;
                EC_tb_InputHistory _inputHistory = new EC_tb_InputHistory();
                _inputHistory.No = no;
                _inputHistory.InputID = Convert.ToInt32(dr["InputID"].ToString());
                _inputHistory.ProductID = Convert.ToInt32(dr["ProductID"].ToString());
                _inputHistory.ProductName = dr["ProductName"].ToString();
                _inputHistory.InputDate = dr["InputDate"].ToString();
                _inputHistory.UserID = Convert.ToInt32(dr["UserID"].ToString());
                _inputHistory.UserName = dr["UserName"].ToString();
                _inputHistory.Cost = Convert.ToDecimal(dr["Cost"].ToString());
                _inputHistory.StrCost = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(_inputHistory.Cost);
                _inputHistory.Price = Convert.ToDecimal(dr["Price"].ToString());
                _inputHistory.StrPrice = StaticClass.GeneralClass.currency_setting_general + StaticClass.GeneralClass.GetNumFormatDisplay(_inputHistory.Price);
                _inputHistory.InventoryCount = Convert.ToInt32(dr["InventoryCount"].ToString());
                _inputHistory.CategoryID = Convert.ToInt32(dr["CategoryID"].ToString());
                _inputHistory.CategoryName = dr["CategoryName"].ToString();
                _inputHistory.Tax = Convert.ToInt32(dr["Tax"].ToString());
                _inputHistory.Country = dr["Country"].ToString();
                _inputHistory.SizeWeight = dr["Size_Weight"].ToString();
                _inputHistory.Currency = StaticClass.GeneralClass.currency_setting_general;
                list_ec_tb_inputhistory.Add(_inputHistory);
            }
        }

    }
}
