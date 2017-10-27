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
using FirstFloor.ModernUI.Windows.Controls;
using System.Threading;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for Currency.xaml
    /// </summary>
    public partial class Currency : UserControl
    {
        //using for setting
        private BUS_tb_Setting bus_tb_setting = new BUS_tb_Setting();
        private List<EC_tb_Setting> list_ec_tb_setting = new List<EC_tb_Setting>();
        private Thread thread_set = null;
        private bool flag_check_loaded = false;

        //Currency
        public Currency()
        {
            InitializeComponent();
            dtgCurrency.ItemsSource = list_ec_tb_setting;
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                GetListSetting();
            }
        }

        //GetListSetting
        private DataTable dt_setting = new DataTable();
        private void GetListSetting()
        {
            if (thread_set != null && thread_set.ThreadState == ThreadState.Running) { }
            else
            {
                thread_set = new Thread(() =>
                {
                    try
                    {
                        dt_setting.Clear();
                        list_ec_tb_setting.Clear();

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.chkAll.IsChecked = false;

                            this.dtgCurrency.Items.Refresh();
                            this.dtgCurrency.Visibility = System.Windows.Visibility.Hidden;

                            this.mpr.Visibility = System.Windows.Visibility.Visible;
                            this.mpr.IsActive = true;
                        }));

                        dt_setting = bus_tb_setting.GetSetting("");
                        int no = 0;

                        foreach (DataRow datarow in dt_setting.Rows)
                        {
                            no++;
                            EC_tb_Setting ec_tb_setting = new EC_tb_Setting();
                            ec_tb_setting.No = no;
                            ec_tb_setting.SettingID = Convert.ToInt32(datarow["SettingID"].ToString());
                            ec_tb_setting.Currency = datarow["Currency"].ToString();
                            ec_tb_setting.TaxRate = Convert.ToDecimal(datarow["TaxRate"].ToString());
                            ec_tb_setting.StrTaxRate = GeneralClass.GetNumFormatDisplay(ec_tb_setting.TaxRate);
                            ec_tb_setting.Active = Convert.ToInt32(datarow["Active"].ToString());
                            ec_tb_setting.Version = Convert.ToInt32(datarow["Version"].ToString());
                            ec_tb_setting.ImageUrl = @"pack://application:,,,/Resources/edit.png";

                            list_ec_tb_setting.Add(ec_tb_setting);
                        }

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.dtgCurrency.Items.Refresh();
                            this.tblTotal.Text = FindResource("total").ToString() + "(" + list_ec_tb_setting.Count.ToString() + ")";
                        }));
                        
                        Thread.Sleep(500);
                        this.mpr.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Hidden;
                            this.mpr.IsActive = false;
                        }));
                        this.dtgCurrency.Dispatcher.Invoke((Action)(() => { this.dtgCurrency.Visibility = System.Windows.Visibility.Visible; }));
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
                thread_set.Start();
            }
        }

        //chkAll_Checked
        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            if (dtgCurrency.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_setting.Count; i++)
                {
                    list_ec_tb_setting[i].CheckDel = true;
                }

                StaticClass.GeneralClass.list_ec_tb_setting_general.AddRange(list_ec_tb_setting);
                dtgCurrency.Items.Refresh();
            }
        }

        //chkAll_Unchecked
        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            dtgCurrency.SelectedIndex = -1;

            if (dtgCurrency.SelectedIndex == -1)
            {
                for (int i = 0; i < list_ec_tb_setting.Count; i++)
                {
                    list_ec_tb_setting[i].CheckDel = false;
                }

                StaticClass.GeneralClass.list_ec_tb_setting_general.Clear();
                dtgCurrency.Items.Refresh();
            }
        }

        //chkCheckDelete_Checked
        private void chkCheckDelete_Checked(object sender, RoutedEventArgs e)
        {
            if (dtgCurrency.SelectedIndex > -1)
            {
                list_ec_tb_setting[dtgCurrency.SelectedIndex].CheckDel = true;
                StaticClass.GeneralClass.list_ec_tb_setting_general.Add(list_ec_tb_setting[dtgCurrency.SelectedIndex]);
            }
        }

        //chkCheckDelete_Unchecked
        private void chkCheckDelete_Unchecked(object sender, RoutedEventArgs e)
        {
            dtgCurrency.SelectedIndex = -1;
            if (dtgCurrency.SelectedIndex > -1)
            {
                list_ec_tb_setting[dtgCurrency.SelectedIndex].CheckDel = false;
                for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_setting_general.Count; i++)
                {
                    if (StaticClass.GeneralClass.list_ec_tb_setting_general[i].SettingID == list_ec_tb_setting[dtgCurrency.SelectedIndex].SettingID)
                        StaticClass.GeneralClass.list_ec_tb_setting_general.RemoveAt(i);
                }
            }
        }

        //btnEdit_Click
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dtgCurrency.SelectedIndex > -1)
            {
                int index = dtgCurrency.SelectedIndex;

                EditCurrency page = new EditCurrency();
                page.tblSettingID.Text = list_ec_tb_setting[index].SettingID.ToString();
                page.txbCurrency.Text = list_ec_tb_setting[index].Currency.ToString();
                page.txbTaxRate.Text = list_ec_tb_setting[index].TaxRate.ToString();
                page.txbVersion.Text = list_ec_tb_setting[index].Version.ToString();
                if (list_ec_tb_setting[index].Active == 1)
                    page.chkActive.IsChecked = true;
                else
                    page.chkActive.IsChecked = false;

                page.edit_delegate += Add_Delete_Edit_Delegate;
                page.ShowDialog();
            }
        }

        //dtgCurrency_MouseDoubleClick
        private void dtgCurrency_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgCurrency.SelectedIndex > -1)
            {
                int index = dtgCurrency.SelectedIndex;

                EditCurrency page = new EditCurrency();
                page.tblSettingID.Text = list_ec_tb_setting[index].SettingID.ToString();
                page.txbCurrency.Text = list_ec_tb_setting[index].Currency.ToString();
                page.txbTaxRate.Text = list_ec_tb_setting[index].TaxRate.ToString();
                page.txbVersion.Text = list_ec_tb_setting[index].Version.ToString();

                if (list_ec_tb_setting[index].Active == 1)
                    page.chkActive.IsChecked = true;
                else
                    page.chkActive.IsChecked = false;

                page.edit_delegate += Add_Delete_Edit_Delegate;
                page.ShowDialog();
            }
        }

        //muiBtnAdd_Click
        private void muiBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCurrency page = new AddCurrency();
            page.add_delegate += Add_Delete_Edit_Delegate;
            page.ShowDialog();
        }

        //muiBtnAdd_Click_Delegate
        private void Add_Delete_Edit_Delegate()
        {
            GetListSetting();
        }

        //muiBtnDelete_Click
        private void muiBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteCurrency page = new DeleteCurrency();
            page.delete_delegate += Add_Delete_Edit_Delegate;
            page.ShowDialog();
        }

        //muibtnGetCurrent_Click
        private void muibtnGetCurrent_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < list_ec_tb_setting.Count; i++)
            {
                if (list_ec_tb_setting[i].Active == 1)
                {
                    dtgCurrency.ScrollIntoView(dtgCurrency.Items.GetItemAt(i));
                    dtgCurrency.SelectedIndex = i;
                }
            }
        }

    }
}
