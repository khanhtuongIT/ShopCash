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
using System.Threading;
using System.Data;
using CashierRegister.StaticClass;
using FirstFloor.ModernUI.Windows.Controls;

namespace CashierRegister.Pages.Statistic
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private Bus_tb_Statistic bus_statistic = new Bus_tb_Statistic();
        private List<EC_tb_Statistic> list_ec_statistic = new List<EC_tb_Statistic>();
        private Thread thread_statistic = null;
        private Thread thread_category = null;
        private bool isLoaded = false;
        private DataTable tbStatistic = new DataTable();

        private BUS_tb_Category bus_category = new BUS_tb_Category();
        private List<EC_tb_Category> list_ec_category = new List<EC_tb_Category>();

        private List<EC_tb_Date> list_year = new List<EC_tb_Date>();
        private List<EC_tb_Date> list_month = new List<EC_tb_Date>();
        private List<EC_tb_Date> list_day = new List<EC_tb_Date>();
        private string orderDate = "";

        //Home
        public Home()
        {
            InitializeComponent();
            dtgStatistic.ItemsSource = list_ec_statistic;
            cboCategory.ItemsSource = list_ec_category;
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
                isLoaded = false;
            else
            {
                isLoaded = true;
                GetCategoryData();
            }
        }

        //GetCategoryData
        private void GetCategoryData()
        {
            if (thread_category != null && (thread_category.ThreadState == ThreadState.WaitSleepJoin || thread_category.ThreadState == ThreadState.WaitSleepJoin)) { }
            else
            {
                thread_category = new Thread(() =>
                {
                    try
                    {
                        //get data for year, month and day
                        if(list_year.Count == 0)
                        {
                            DateTime currentDate = DateTime.Now;

                            list_year.Add(new EC_tb_Date() { Year = FindResource("all").ToString() });
                            for (int i = 2010; i <= 2030; i++)
                            {
                                list_year.Add(new EC_tb_Date() { Year = i.ToString() });
                            }
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                cboYear.ItemsSource = list_year;
                                cboYear.SelectedIndex = list_year.FindIndex(x => x.Year == currentDate.Year.ToString());
                            }));

                            list_month.Add(new EC_tb_Date() { Month = FindResource("all").ToString() });
                            for (int i = 1; i <= 12; i++)
                            {
                                list_month.Add(new EC_tb_Date() { Month = i < 10 ? "0" + i.ToString() : i.ToString() });
                            }
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                cboMonth.ItemsSource = list_month;
                                cboMonth.SelectedIndex = currentDate.Month;
                            }));

                            list_day.Add(new EC_tb_Date() { Day = FindResource("all").ToString() });
                            for (int i = 1; i <= 31; i++)
                            {
                                list_day.Add(new EC_tb_Date() { Day = i < 10 ? "0" + i.ToString() : i.ToString() });
                            }
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                cboDay.ItemsSource = list_day;
                                cboDay.SelectedIndex = currentDate.Day;
                                orderDate = cboYear.SelectedValue + "-" + cboMonth.SelectedValue + "-" + cboDay.SelectedValue;
                                cboYear.SelectionChanged += new SelectionChangedEventHandler(cboDate_SelectionChanged);
                                cboMonth.SelectionChanged += new SelectionChangedEventHandler(cboDate_SelectionChanged);
                                cboDay.SelectionChanged += new SelectionChangedEventHandler(cboDate_SelectionChanged);
                            }));
                        }

                        //get data for category
                        list_ec_category.Clear();
                        this.Dispatcher.Invoke((Action)(() => { this.cboCategory.Items.Refresh(); }));
                        list_ec_category.Add(new EC_tb_Category()
                        {
                            CategoryName = FindResource("all").ToString(),
                            CategoryID = 0,
                        });
                        foreach (DataRow item in bus_category.GetCatagorySetting("").Rows)
                        {
                            list_ec_category.Add(new EC_tb_Category()
                            {
                                CategoryID = Convert.ToInt32(item["CategoryID"].ToString()),
                                CategoryName = item["CategoryName"].ToString()
                            });
                        }
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.cboCategory.Items.Refresh();
                            this.cboCategory.SelectedIndex = 0;
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message + "!";
                            md.ShowDialog();
                        }));
                    }
                });
                thread_category.Start();
            }
        }

        //GetStatisticData
        private void GetStatisticData()
        {
            if (thread_statistic != null && (thread_statistic.ThreadState == ThreadState.WaitSleepJoin || thread_statistic.ThreadState == ThreadState.WaitSleepJoin)) { }
            else
            {
                thread_statistic = new Thread(() =>
                {
                    try
                    {
                        list_ec_statistic.Clear();
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.dtgStatistic.Items.Refresh();
                            this.dtgStatistic.Visibility = Visibility.Hidden;
                            this.mpr.IsActive = true;
                            this.mpr.Visibility = Visibility.Visible;
                            this.tblTotal.Text = FindResource("total_zero").ToString();
                            tbStatistic = (int)cboCategory.SelectedValue == 0 ? bus_statistic.GetStatisticData(txbSearch.Text.Trim(), orderDate) : bus_statistic.GetStatisticData(txbSearch.Text.Trim(), orderDate, (int)cboCategory.SelectedValue);
                        }));

                        int no = 0;
                        foreach (DataRow item in tbStatistic.Rows)
                        {
                            list_ec_statistic.Add(new EC_tb_Statistic()
                            {
                                No = ++no,
                                SortName = item["ShortName"].ToString(),
                                CategoryName = item["CategoryName"].ToString(),
                                SoldQuantity = item["SoldQuantity"].ToString(),
                                QuantityAvailable = item["QuantityAvailable"].ToString(),
                            });
                        }

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.dtgStatistic.ItemsSource = list_ec_statistic;
                            this.dtgStatistic.Items.Refresh();
                            this.mpr.IsActive = false;
                            this.mpr.Visibility = Visibility.Hidden;
                            this.tblTotal.Text = FindResource("total").ToString() + "(" + list_ec_statistic.Count.ToString() + ")";
                            this.dtgStatistic.Visibility = Visibility.Visible;
                        }));
                    }
                    catch(Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message + "!";
                            md.ShowDialog();
                        }));
                    }
                });
                thread_statistic.Start();
            }
        }

        //muiBtnSearch_Click
        private void muiBtnSearch_Click(object sender, RoutedEventArgs e)
        {
            cboCategory_SelectionChanged(null, null);
        }

        //txbSearch_KeyDown
        private void txbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().ToLower() == "return")
                muiBtnSearch_Click(null, null);
        }

        //cboCategory_SelectionChanged
        private void cboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboCategory.SelectedIndex > -1)
                GetStatisticData();
        }

        //cboYear_SelectionChanged
        private void cboDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboYear.SelectedIndex > -1)
            {
                switch (cboYear.SelectedIndex)
                {
                    case 0:
                        cboMonth.IsEnabled = false;
                        cboDay.IsEnabled = false;
                        orderDate = "";
                        break;
                    default:
                        cboMonth.IsEnabled = true;
                        cboDay.IsEnabled = true;
                        switch (cboMonth.SelectedIndex)
                        {
                            case -1: break;
                            case 0:
                                cboDay.IsEnabled = false;
                                orderDate = cboYear.SelectedValue.ToString() + "-";
                                break;
                            default:
                                switch (cboDay.SelectedIndex)
                                {
                                    case -1: break;
                                    case 0:
                                        orderDate = cboYear.SelectedValue.ToString() + "-" + cboMonth.SelectedValue.ToString() + "-";
                                        break;
                                    default:
                                        orderDate = cboYear.SelectedValue.ToString() + "-" + cboMonth.SelectedValue.ToString() + "-" + cboDay.SelectedValue;
                                        break;
                                }
                                break;
                        }
                        break;
                }
                GetStatisticData();
            }
        }
    }
}
