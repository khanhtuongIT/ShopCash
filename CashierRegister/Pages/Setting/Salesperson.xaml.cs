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
using FirstFloor.ModernUI.Windows.Controls;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for Salesperson.xaml
    /// </summary>
    public partial class Salesperson : UserControl
    {
        //using for salesperson
        private BUS_tb_SalesPerson bus_tb_salesperson = new BUS_tb_SalesPerson();
        private List<EC_tb_SalesPerson> list_ec_tb_salesperson = new List<EC_tb_SalesPerson>();

        //using for current directory
        private string currnt_directory = System.IO.Directory.GetCurrentDirectory().ToString();

        //thread
        private Thread thread_content = null;
        private bool flag_check_loaded = false;

        //Salesperson
        public Salesperson()
        {
            InitializeComponent();
            dtgSalesperson.ItemsSource = list_ec_tb_salesperson;
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                GetSalesperson("");
            }
        }

        //get salesperson
        private void GetSalesperson(string condition)
        {
            if (thread_content != null && thread_content.ThreadState == ThreadState.Running) { }
            else
            {
                thread_content = new Thread(() =>
                {
                    try
                    {
                        list_ec_tb_salesperson.Clear();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mpr.IsActive = true;
                            this.chkCheckAll.IsChecked = false;
                            this.dtgSalesperson.Items.Refresh();
                            this.dtgSalesperson.Visibility = System.Windows.Visibility.Hidden;
                        }));

                        using (DataTable dt_salesperson = bus_tb_salesperson.GetSalesPerson(condition))
                        {
                            int no = 0;
                            foreach (DataRow dr in dt_salesperson.Rows)
                            {
                                no++;
                                EC_tb_SalesPerson ec_tb_salasperson = new EC_tb_SalesPerson();
                                ec_tb_salasperson.No = no;
                                ec_tb_salasperson.SalespersonID = Convert.ToInt32(dr["SalespersonID"].ToString());
                                ec_tb_salasperson.Name = dr["Name"].ToString();
                                ec_tb_salasperson.Birthday = dr["Birthday"].ToString();
                                ec_tb_salasperson.Address = dr["Address"].ToString();
                                ec_tb_salasperson.Email = dr["Email"].ToString();
                                ec_tb_salasperson.Password = dr["Password"].ToString();
                                ec_tb_salasperson.Active = Convert.ToInt32(dr["Active"].ToString());
                                ec_tb_salasperson.CheckDel = false;
                                ec_tb_salasperson.ImageUrl = @"pack://application:,,,/Resources/edit.png";

                                list_ec_tb_salesperson.Add(ec_tb_salasperson);
                            }
                        }

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            tblTotal.Text = FindResource("total").ToString() + "(" + list_ec_tb_salesperson.Count + ")";
                            dtgSalesperson.Items.Refresh();
                        }));

                        Thread.Sleep(500);
                        this.mpr.Dispatcher.Invoke((Action)(() => 
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Hidden;
                            this.mpr.IsActive = false; 
                        }));
                        this.dtgSalesperson.Dispatcher.Invoke((Action)(() => { this.dtgSalesperson.Visibility = System.Windows.Visibility.Visible; }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_content.Start();
            }
        }

        //muiBtnAdd_Click
        private void muiBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddSalesperson page = new AddSalesperson();
            page.muibtnadd_delegate += muiBtnAdd_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnAdd_Click_Delegate
        private void muiBtnAdd_Click_Delegate()
        {
            GetSalesperson("");
        }

        //chkCheckAll_Checked
        private void chkCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            dtgSalesperson.SelectedIndex = -1;

            if (dtgSalesperson.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_salesperson.Count; i++)
                {
                    list_ec_tb_salesperson[i].CheckDel = true;
                }

                StaticClass.GeneralClass.list_ec_tb_salesperson_general.AddRange(list_ec_tb_salesperson);
                dtgSalesperson.Items.Refresh();
            }
        }

        //chkCheckAll_Unchecked
        private void chkCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            dtgSalesperson.SelectedIndex = -1;

            if (dtgSalesperson.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_salesperson.Count; i++)
                {
                    list_ec_tb_salesperson[i].CheckDel = false;
                }

                StaticClass.GeneralClass.list_ec_tb_salesperson_general.Clear();
                dtgSalesperson.Items.Refresh();
            }
        }

        //muiBtnDelete_Click
        private void muiBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSalesperson page = new DeleteSalesperson();
            page.muibtndelete_delegate += muiBtnDelete_Click_Delegate;
            page.ShowDialog();
        }

        //muiBtnDelete_Click_Delegate
        private void muiBtnDelete_Click_Delegate()
        {
            GetSalesperson("");
        }

        //chkCheckDel_Checked
        private void chkCheckDel_Checked(object sender, RoutedEventArgs e)
        {
            if (dtgSalesperson.SelectedIndex > -1)
            {
                list_ec_tb_salesperson[dtgSalesperson.SelectedIndex].CheckDel = true;

                StaticClass.GeneralClass.list_ec_tb_salesperson_general.Add(list_ec_tb_salesperson[dtgSalesperson.SelectedIndex]);
            }
        }

        //chkCheckDel_Unchecked
        private void chkCheckDel_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dtgSalesperson.SelectedIndex > -1)
            {
                list_ec_tb_salesperson[dtgSalesperson.SelectedIndex].CheckDel = false;

                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_salesperson_general.Count; i++)
                {
                    if (StaticClass.GeneralClass.list_ec_tb_salesperson_general[i].SalespersonID == list_ec_tb_salesperson[dtgSalesperson.SelectedIndex].SalespersonID)
                        StaticClass.GeneralClass.list_ec_tb_salesperson_general.RemoveAt(i);
                }
            }
        }

        //btnEdit_Click
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dtgSalesperson.SelectedIndex > -1)
            {
                int index = dtgSalesperson.SelectedIndex;
                EditSalesperson page = new EditSalesperson();

                page.tblSalespersonID.Text = list_ec_tb_salesperson[index].SalespersonID.ToString();
                page.txbName.Text = list_ec_tb_salesperson[index].Name;
                page.dtpBirthday.Text = list_ec_tb_salesperson[index].Birthday;
                page.txbAddress.Text = list_ec_tb_salesperson[index].Address;
                page.txbEmail.Text = list_ec_tb_salesperson[index].Email;
                StaticClass.GeneralClass.salespersonpassword_general = list_ec_tb_salesperson[index].Password;
                if (list_ec_tb_salesperson[index].Active == 1)
                    page.chkActive.IsChecked = true;
                else
                    page.chkActive.IsChecked = false;

                page.edit_delegate += btnEdit_Click_Delegate;
                page.ShowDialog();
            }
        }

        //dtgSalesperson_MouseDoubleClick
        private void dtgSalesperson_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgSalesperson.SelectedIndex > -1)
            {
                int index = dtgSalesperson.SelectedIndex;
                EditSalesperson page = new EditSalesperson();

                page.tblSalespersonID.Text = list_ec_tb_salesperson[index].SalespersonID.ToString();
                page.txbName.Text = list_ec_tb_salesperson[index].Name;
                page.dtpBirthday.Text = list_ec_tb_salesperson[index].Birthday;
                page.txbAddress.Text = list_ec_tb_salesperson[index].Address;
                page.txbEmail.Text = list_ec_tb_salesperson[index].Email;
                StaticClass.GeneralClass.salespersonpassword_general = list_ec_tb_salesperson[index].Password;
                if (list_ec_tb_salesperson[index].Active == 1)
                    page.chkActive.IsChecked = true;
                else
                    page.chkActive.IsChecked = false;

                page.edit_delegate += btnEdit_Click_Delegate;
                page.ShowDialog();
            }
        }

        //btnEdit_Click_Delegate
        private void btnEdit_Click_Delegate()
        {
            GetSalesperson("");
        }

        //muiBtnSearch_Click
        private void muiBtnSearch_Click(object sender, RoutedEventArgs e)
        {
            GetSalesperson(" WHERE [Name] like '%" + txbSearch.Text.Trim().ToString() + "%'");
        }

        //txbSearch_KeyDown
        private void txbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
                GetSalesperson(" WHERE [Name] like '%" + txbSearch.Text.Trim().ToString() + "%'");
        }

    }
}
