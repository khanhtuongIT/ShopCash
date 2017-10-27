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
using System.Data;

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for SelectSalesperson.xaml
    /// </summary>
    public partial class SelectSalesperson : ModernDialog
    {
        //using for salesperson 
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();
        private List<EC_tb_SalesPerson> list_ec_tb_salesperson = new List<EC_tb_SalesPerson>();

        //using for current directory
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        //delegate
        public delegate void btnSalesperson_Click_Delegate();
        public event btnSalesperson_Click_Delegate btnsalesperson_delegate;

        //SelectSalesperson
        public SelectSalesperson()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {this.CloseButton};
            this.CloseButton.Content = FindResource("close").ToString();
            dtgSalesperson.ItemsSource = list_ec_tb_salesperson;
        }

        //get salesperson
        private DataTable dt_salesperson = new DataTable();
        private List<EC_tb_SalesPerson> GetSalesperson()
        {
            list_ec_tb_salesperson.Clear();
            using (DataTable dt_salesperson = bus_tb_salesperson.GetSalesPerson("WHERE [Active] = 1"))
            {
                foreach (DataRow datarow in dt_salesperson.Rows)
                {
                    EC_tb_SalesPerson ec_tb_salasperson = new EC_tb_SalesPerson();
                    ec_tb_salasperson.SalespersonID = Convert.ToInt32(datarow["SalespersonID"].ToString());
                    ec_tb_salasperson.Name = datarow["Name"].ToString();
                    ec_tb_salasperson.Address = datarow["Address"].ToString();
                    ec_tb_salasperson.Password = datarow["Password"].ToString();
                    ec_tb_salasperson.Active = Convert.ToInt32(datarow["Active"].ToString());
                    ec_tb_salasperson.Defaul = Convert.ToInt32(datarow["Default"].ToString());

                    if (ec_tb_salasperson.Defaul == 1)
                        ec_tb_salasperson.ImageUrl = @"pack://application:,,,/Resources/select_customer.png";
                    else
                        ec_tb_salasperson.ImageUrl = @"pack://application:,,,/Resources/login.png";

                    list_ec_tb_salesperson.Add(ec_tb_salasperson);
                }
            }

            dtgSalesperson.Items.Refresh();

            return list_ec_tb_salesperson;
        }

        //ModernDialog_Loaded
        private void ModernDialog_Loaded(object sender, RoutedEventArgs e)
        {
            GetSalesperson();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (dtgSalesperson.SelectedIndex > -1)
            {
                StaticClass.GeneralClass.salespersonid_login_general = list_ec_tb_salesperson[dtgSalesperson.SelectedIndex].SalespersonID;

                Pages.Home.SalespersonLogin page = new Pages.Home.SalespersonLogin();
                page.btnlogin_delegate += btnLogin_Click_Delegate;
                page.ShowDialog();
            }
        }

        //btnLogin_Click_Delegate
        private void btnLogin_Click_Delegate()
        {
            if (StaticClass.GeneralClass.flag_salespersonlogin_general == true)
            {
                if (btnsalesperson_delegate != null)
                {
                    btnsalesperson_delegate();
                    this.Close();
                }
            }
        }

        //dtgSalesperson_MouseDoubleClick
        private void dtgSalesperson_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgSalesperson.SelectedIndex > -1)
            {
                StaticClass.GeneralClass.salespersonid_login_general = list_ec_tb_salesperson[dtgSalesperson.SelectedIndex].SalespersonID;

                Pages.Home.SalespersonLogin page = new Pages.Home.SalespersonLogin();
                page.btnlogin_delegate += btnLogin_Click_Delegate;
                page.ShowDialog();
            }
        }
    }
}
