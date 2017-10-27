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
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Chart
{
    /// <summary>
    /// Interaction logic for RevenueProfitChart.xaml
    /// </summary>
    public partial class RevenueProfitChart : UserControl
    {
        //thread
        private Thread thread_revenue_profit = null;
        private List<EC_tb_Charting> list_revenue_profit = new List<EC_tb_Charting>();
        private List<EC_tb_Charting> _list_revenue_profit = new List<EC_tb_Charting>();
        private List<List<EC_tb_Charting>> list_list_revenue_profit = new List<List<EC_tb_Charting>>();
        private List<EC_tb_Charting> list_ec_tb_year = new List<EC_tb_Charting>();
        private List<EC_tb_Charting> list_ec_tb_order_year = new List<EC_tb_Charting>();
        
        private bool flag_check_loaded = false;
        private int children_previous = 0;

        //using for tb_order
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //using for tb_orderdetail
        private BUS_tb_OrderDetail bus_tb_orderdetail = new BUS_tb_OrderDetail();

        //RevenueProfitChart
        public RevenueProfitChart()
        {
            InitializeComponent();
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                LoadData();
            }
        }

        //LoadData
        private void LoadData()
        {
            if (thread_revenue_profit != null && (thread_revenue_profit.ThreadState == ThreadState.Running || thread_revenue_profit.ThreadState == ThreadState.WaitSleepJoin)) { }
            else
            {
                thread_revenue_profit = new Thread(() =>
                {
                    try
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            //using for revenue and profit month
                            this.imgSaveRevenueProfitMonthPNG.Visibility = Visibility.Hidden;
                            this.grdRevenueProfitMonthPar.Visibility = System.Windows.Visibility.Hidden;
                            this.mprRevenueProfitMonth.IsActive = false;
                            this.mprRevenueProfitMonth.Visibility = System.Windows.Visibility.Hidden;

                            //using for revenue and profit year
                            this.imgSaveRevenueProfitYearPNG.Visibility = Visibility.Hidden;
                            this.grdRevenueProfitYear.Visibility = System.Windows.Visibility.Hidden;
                            this.ctRevenueProfitYear.Title = FindResource("revenue_and_profit").ToString() + " (" + FindResource("unit").ToString() + ": " + StaticClass.GeneralClass.currency_setting_general + ")";
                            this.mprRevenueProfitYear.Visibility = System.Windows.Visibility.Visible;
                            this.mprRevenueProfitYear.IsActive = true;
                        }));

                        GetRevenueProfitAll();
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            //using for revenue and profit year
                            this.mprRevenueProfitYear.Visibility = System.Windows.Visibility.Hidden;
                            this.mprRevenueProfitYear.IsActive = false;
                            this.grdRevenueProfitYear.Visibility = System.Windows.Visibility.Visible;
                            this.imgSaveRevenueProfitYearPNG.Visibility = Visibility.Visible;

                            //using for revenue and profit month
                            this.mprRevenueProfitMonth.Visibility = Visibility.Visible;
                            this.mprRevenueProfitMonth.IsActive = true;
                        }));

                        children_previous = 0;
                        GetYear();

                        Thread.Sleep(500);
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mprRevenueProfitMonth.Visibility = System.Windows.Visibility.Hidden;
                            this.mprRevenueProfitMonth.IsActive = false;
                            grdRevenueProfitMonthPar.Visibility = System.Windows.Visibility.Visible;
                            ctRevenueProfitMonth.Visibility = System.Windows.Visibility.Visible;
                            this.imgSaveRevenueProfitMonthPNG.Visibility = Visibility.Visible;
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Content = ex.Message;
                            md.Title = FindResource("notification").ToString();
                            md.ShowDialog();
                        }));
                    }
                });
                thread_revenue_profit.Start();
            }
        }

        //GetRevenueProfitAll
        private DataTable tb_revenue_all = new DataTable();
        private DataTable tb_capital_all = new DataTable();
        private List<EC_tb_Charting> list_ec_tb_revenue_profit_year = new List<EC_tb_Charting>();

        private void GetRevenueProfitAll()
        {
            //using for revenue
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.ctcRevenueYear.ItemsSource = null;
                this.ctlProfitYear.ItemsSource = null;
            }));

            tb_revenue_all.Clear();
            list_ec_tb_revenue_profit_year.Clear();
            tb_revenue_all = bus_tb_orderdetail.GetTotalRevenueAll("", StaticClass.GeneralClass.flag_database_type_general);
            foreach (DataRow dr in tb_revenue_all.Rows)
            {
                EC_tb_Charting ec_tb_charting = new EC_tb_Charting();
                ec_tb_charting.Year = dr["RevenueAll"].ToString();
                ec_tb_charting.Revenue = Convert.ToDouble(dr["TotalRevenueAll"].ToString());
                list_ec_tb_revenue_profit_year.Add(ec_tb_charting);
            }

            //using for profit
            tb_capital_all.Clear();
            tb_capital_all = bus_tb_orderdetail.GetTotalCapitalAll("", StaticClass.GeneralClass.flag_database_type_general);

            foreach (DataRow dr in tb_capital_all.Rows)
            {
                for (int i = 0; i < list_ec_tb_revenue_profit_year.Count; i++)
                {
                    if (list_ec_tb_revenue_profit_year[i].Year == dr["CapitalAll"].ToString())
                    {
                        double profit = list_ec_tb_revenue_profit_year[i].Revenue - Convert.ToDouble(dr["TotalCapitalAll"].ToString());
                        list_ec_tb_revenue_profit_year[i].Profit = profit;
                        break;
                    }
                }
            }

            this.Dispatcher.Invoke((Action)(() =>
            {
                this.ctcRevenueYear.ItemsSource = list_ec_tb_revenue_profit_year;
                this.ctlProfitYear.ItemsSource = list_ec_tb_revenue_profit_year;
            }));

            //add data to the table of revenue and profit in years
            this.Dispatcher.Invoke((Action)(() =>
            {
                tblRevenueYear.Text = FindResource("revenue").ToString() + "(" + StaticClass.GeneralClass.currency_setting_general.Trim() + ")";
                tblProfitYear.Text = FindResource("profit").ToString() + "(" + StaticClass.GeneralClass.currency_setting_general.Trim() + ")";
                grdTableRevenueProfitYear.ColumnDefinitions.Clear();
                grdTableRevenueProfitYear.Children.Clear();
            }));

            for (int i = 0; i < list_ec_tb_revenue_profit_year.Count; i++)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ColumnDefinition cd = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
                    grdTableRevenueProfitYear.ColumnDefinitions.Add(cd);
                    Border bd = new Border() { BorderBrush = new BrushConverter().ConvertFromString("#FF212121") as Brush, BorderThickness = new Thickness(1, 0, 0, 0) }; Grid.SetRowSpan(bd, 3); Grid.SetColumn(bd, i);
                    grdTableRevenueProfitYear.Children.Add(bd);
                    TextBlock tblYear = new TextBlock() { Text = list_ec_tb_revenue_profit_year[i].Year, Style = FindResource("textBlockValue") as Style, HorizontalAlignment = HorizontalAlignment.Center };
                    Grid.SetColumn(tblYear, i);
                    grdTableRevenueProfitYear.Children.Add(tblYear);

                    TextBlock tblRevenue = new TextBlock() { Text = GeneralClass.GetNumFormatDisplay((decimal)list_ec_tb_revenue_profit_year[i].Revenue), Style = FindResource("textBlockValue") as Style };
                    Grid.SetColumn(tblRevenue, i); Grid.SetRow(tblRevenue, 1);
                    grdTableRevenueProfitYear.Children.Add(tblRevenue);

                    TextBlock tblProfit = new TextBlock() { Text = GeneralClass.GetNumFormatDisplay((decimal)list_ec_tb_revenue_profit_year[i].Profit), Style = FindResource("textBlockValue") as Style };
                    Grid.SetColumn(tblProfit, i); Grid.SetRow(tblProfit, 2);
                    grdTableRevenueProfitYear.Children.Add(tblProfit);
                }));
            }

            //fixed
            this.Dispatcher.Invoke((Action)(() =>
            {
                Separator sp1 = new Separator() { VerticalAlignment = VerticalAlignment.Bottom, Background = new BrushConverter().ConvertFromString("#FF616161") as Brush, }; Grid.SetColumnSpan(sp1, list_ec_tb_revenue_profit_year.Count > 0 ? list_ec_tb_revenue_profit_year.Count : 1);
                grdTableRevenueProfitYear.Children.Add(sp1);

                Separator sp2 = new Separator() { VerticalAlignment = VerticalAlignment.Bottom, Background = new BrushConverter().ConvertFromString("#FF616161") as Brush, }; Grid.SetRow(sp2, 1); Grid.SetColumnSpan(sp2, list_ec_tb_revenue_profit_year.Count > 0 ? list_ec_tb_revenue_profit_year.Count : 1);
                grdTableRevenueProfitYear.Children.Add(sp2);
            }));
        }

        //GetYear
        private DataTable tb_year = new DataTable();
        private int total_year = -1;
        private int uid_img = -1;
        private System.Windows.Media.Imaging.BitmapImage bitmapimage_circle_year = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_year.png", UriKind.Absolute));
        private System.Windows.Media.Imaging.BitmapImage bitmapimage_circle_focus_year = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_focus_year.png", UriKind.Absolute));
        private void GetYear()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() => { this.stpYearRevenueProfittMonth.Children.Clear(); }));
                list_ec_tb_year.Clear();
                tb_year.Clear();
                total_year = -1;
                uid_img = -1;

                tb_year = bus_tb_order.GetYearFromOrder("", StaticClass.GeneralClass.flag_database_type_general);

                foreach (DataRow datarow in tb_year.Rows)
                {
                    EC_tb_Charting ec_tb_charting = new EC_tb_Charting();
                    ec_tb_charting.Year = datarow["Year"].ToString();
                    list_ec_tb_year.Add(ec_tb_charting);

                    //create new image year
                    total_year++;
                    uid_img++;

                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        Image img = new Image()
                        {
                            Width = 40,
                            Height = 40,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(5, 0, 0, 5),
                            Name = "img" + ec_tb_charting.Year,
                            Uid = "uid" + uid_img.ToString()
                        };

                        if (total_year == 0)
                        {
                            img.Source = bitmapimage_circle_focus_year;
                            GetDataFollowYear(ec_tb_charting.Year, "");
                        }
                        else
                            img.Source = bitmapimage_circle_year;

                        Grid grd = new Grid();
                        grd.MouseDown += new MouseButtonEventHandler(grd_MouseDown);
                        TextBlock tbl = new TextBlock()
                        {
                            Text = ec_tb_charting.Year,
                            Foreground = System.Windows.Media.Brushes.White,
                            FontWeight = FontWeights.Medium,
                            Margin = new Thickness(5, 0, 0, 5),
                            VerticalAlignment = System.Windows.VerticalAlignment.Center,
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        };
                        grd.Children.Add(img);
                        grd.Children.Add(tbl);
                        stpYearRevenueProfittMonth.Children.Add(grd);
                    }));
                }

                if (total_year == -1)
                {
                    list_ec_tb_revenue_profit_month.Clear();
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        ctcRevenueMonth.ItemsSource = null;
                        ctlProfitMonth.ItemsSource = null;
                    }));
                }
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ModernDialog md = new ModernDialog();
                    md.Title = FindResource("notification").ToString();
                    md.Content = ex.Message;
                    md.ShowDialog();
                }));
            }
        }

        //img_MouseDown
        private Thread thread_getdata_followyear = null;
        private void grd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (thread_getdata_followyear != null && thread_getdata_followyear.ThreadState == ThreadState.Running) { }
            else
            {
                thread_getdata_followyear = new Thread((() =>
                {
                    try
                    {
                        string str_name = "";
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Grid gr_previous = (Grid)stpYearRevenueProfittMonth.Children[children_previous];
                            Image img_previous = (Image)gr_previous.Children[0];
                            img_previous.Source = bitmapimage_circle_year;

                            Grid gr = (Grid)sender;
                            Image img = (Image)gr.Children[0];
                            str_name = img.Name.Substring(3);
                            img.Source = bitmapimage_circle_focus_year;

                            string _str_children_previous = img.Uid.Substring(3);
                            children_previous = Convert.ToInt32(_str_children_previous);
                            GetDataFollowYear(str_name, "");
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                }));
                thread_getdata_followyear.Start();
            }
        }

        //GetDataFollowMonth
        private DataTable tb_revenue_year = new DataTable();
        private DataTable tb_capital_year = new DataTable();
        private List<EC_tb_Charting> list_ec_tb_revenue_profit_month = new List<EC_tb_Charting>();
        private void GetDataFollowYear(string _str_year, string condition)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.ctRevenueProfitMonth.Title = FindResource("revenue_and_profit_in").ToString() + " " + _str_year + " (" + FindResource("unit").ToString() + ": " + StaticClass.GeneralClass.currency_setting_general + ")";

                    //using for revenue year
                    this.ctcRevenueMonth.ItemsSource = null;
                    this.ctlProfitMonth.ItemsSource = null;
                }));

                tb_revenue_year.Clear();
                list_ec_tb_revenue_profit_month.Clear();
                tb_revenue_year = bus_tb_orderdetail.GetTotalRevenueYear(_str_year, condition, StaticClass.GeneralClass.flag_database_type_general);

                for (int m = 1; m <= 12; m++)
                {
                    EC_tb_Charting ec_tb_charting = new EC_tb_Charting();
                    if (m >= 10)
                        ec_tb_charting.Month = m.ToString();
                    else
                    {
                        if (StaticClass.GeneralClass.flag_database_type_general == false)
                            ec_tb_charting.Month = "0" + m.ToString();
                        else
                            ec_tb_charting.Month = m.ToString();
                    }

                    list_ec_tb_revenue_profit_month.Add(ec_tb_charting);
                }

                foreach (DataRow dr in tb_revenue_year.Rows)
                {
                    for (int i = 0; i < list_ec_tb_revenue_profit_month.Count; i++)
                    {
                        if (list_ec_tb_revenue_profit_month[i].Month == dr["RevenueMonth"].ToString())
                        {
                            list_ec_tb_revenue_profit_month[i].Revenue = Convert.ToDouble(dr["TotalRevenueMonth"].ToString());
                            break;
                        }
                    }
                }

                //using for profit year
                tb_capital_year.Clear();
                tb_capital_year = bus_tb_orderdetail.GetTotalCapitalYear(_str_year, condition, StaticClass.GeneralClass.flag_database_type_general);

                foreach (DataRow dr in tb_capital_year.Rows)
                {
                    for (int i = 0; i < list_ec_tb_revenue_profit_month.Count; i++)
                    {
                        if (list_ec_tb_revenue_profit_month[i].Month == dr["CapitalMonth"].ToString())
                        {
                            double profit = list_ec_tb_revenue_profit_month[i].Revenue - Convert.ToDouble(dr["TotalCapitalMoth"].ToString());
                            list_ec_tb_revenue_profit_month[i].Profit = profit;
                            break;
                        }
                    }
                }

                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.ctcRevenueMonth.ItemsSource = list_ec_tb_revenue_profit_month;
                    this.ctlProfitMonth.ItemsSource = list_ec_tb_revenue_profit_month;
                }));

                //add data to the table of revenue and profit in month
                this.Dispatcher.Invoke((Action)(() =>
                {
                    tblRevenueMonth.Text = FindResource("revenue").ToString() + "(" + StaticClass.GeneralClass.currency_setting_general.Trim() + ")";
                    tblProfitMonth.Text = FindResource("profit").ToString() + "(" + StaticClass.GeneralClass.currency_setting_general.Trim() + ")";
                    grdTableRevenueProfitMonth.ColumnDefinitions.Clear();
                    grdTableRevenueProfitMonth.Children.Clear();
                }));

                for (int i = 0; i < 12; i++)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        ColumnDefinition cd = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
                        grdTableRevenueProfitMonth.ColumnDefinitions.Add(cd);
                        Border bd = new Border() { BorderBrush = new BrushConverter().ConvertFromString("#FF212121") as Brush, BorderThickness = new Thickness(1, 0, 0, 0) }; Grid.SetRowSpan(bd, 3); Grid.SetColumn(bd, i);
                        grdTableRevenueProfitMonth.Children.Add(bd);

                        TextBlock tblMonth = new TextBlock() { Text = list_ec_tb_revenue_profit_month[i].Month, Style = FindResource("textBlockValue") as Style, HorizontalAlignment = HorizontalAlignment.Center ,};
                        Grid.SetColumn(tblMonth, i);
                        grdTableRevenueProfitMonth.Children.Add(tblMonth);

                        TextBlock tblRevenue = new TextBlock() { Text = list_ec_tb_revenue_profit_month[i].Revenue == 0 ? FindResource("null").ToString() : GeneralClass.GetNumFormatDisplay((decimal)list_ec_tb_revenue_profit_month[i].Revenue), Style = FindResource("textBlockValue") as Style };
                        if (list_ec_tb_revenue_profit_month[i].Revenue == 0)
                            tblRevenue.Foreground = new BrushConverter().ConvertFromString("#FF212121") as Brush;
                        Grid.SetColumn(tblRevenue, i); Grid.SetRow(tblRevenue, 1);
                        grdTableRevenueProfitMonth.Children.Add(tblRevenue);

                        TextBlock tblProfit = new TextBlock() { Text = list_ec_tb_revenue_profit_month[i].Profit == 0 ? FindResource("null").ToString() : GeneralClass.GetNumFormatDisplay((decimal)list_ec_tb_revenue_profit_month[i].Profit), Style = FindResource("textBlockValue") as Style };
                        if (list_ec_tb_revenue_profit_month[i].Profit == 0)
                            tblProfit.Foreground = new BrushConverter().ConvertFromString("#FF212121") as Brush;
                        Grid.SetColumn(tblProfit, i); Grid.SetRow(tblProfit, 2);
                        grdTableRevenueProfitMonth.Children.Add(tblProfit);
                    }));
                }
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Separator sp1 = new Separator() { VerticalAlignment = VerticalAlignment.Bottom, Background = new BrushConverter().ConvertFromString("#FF616161") as Brush, }; Grid.SetColumnSpan(sp1, list_ec_tb_revenue_profit_month.Count);
                    grdTableRevenueProfitMonth.Children.Add(sp1);

                    Separator sp2 = new Separator() { VerticalAlignment = VerticalAlignment.Bottom, Background = new BrushConverter().ConvertFromString("#FF616161") as Brush, }; Grid.SetColumnSpan(sp2, list_ec_tb_revenue_profit_month.Count); Grid.SetRow(sp2, 1);
                    grdTableRevenueProfitMonth.Children.Add(sp2);
                }));
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ModernDialog md = new ModernDialog();
                    md.MaxWidth = 9999;
                    md.Title = FindResource("notification").ToString();
                    md.Content = ex.Message;
                    md.ShowDialog();
                }));
            }
        }

        //ModernButton_Click
        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            //RenderTargetBitmap bitmap = new RenderTargetBitmap(900, 900, 0, 0, PixelFormats.Pbgra32);
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)grdRevenueProfitYear.RenderSize.Width, (int)grdRevenueProfitYear.RenderSize.Height, 0, 0, PixelFormats.Pbgra32);
            bitmap.Render(grdRevenueProfitYear);

            BitmapFrame frame = BitmapFrame.Create(bitmap);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(frame);

            string pathToExe = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string exeDirectory = System.IO.Path.GetDirectoryName(pathToExe);
            using (var stream = System.IO.File.Create(@"u:\uc.png"))
            {
                encoder.Save(stream);
            }
        }

        //imgSaveRevenueProfitYearPNG_MouseDown
        private void imgSaveRevenueProfitYearPNG_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveToImageFile(grdRevenueProfitYear);
        }

        //imgSaveRevenueProfitMonthPNG_MouseDown
        private void imgSaveRevenueProfitMonthPNG_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveToImageFile(grdRevenueProfitMonthChil);
        }

        //SaveToImageFile
        private void SaveToImageFile(Grid _obj)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Image file (*.png)|*.png";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)_obj.RenderSize.Width, (int)_obj.RenderSize.Height, 0, 0, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(_obj);
                BitmapFrame bitmapFrame = BitmapFrame.Create(renderTargetBitmap);
                PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                pngBitmapEncoder.Frames.Add(bitmapFrame);
                string pathToExe = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                string exeDirectory = System.IO.Path.GetDirectoryName(pathToExe);
                using (var stream = System.IO.File.Create(saveFileDialog.FileName))
                {
                    pngBitmapEncoder.Save(stream);
                    stream.Close();
                }
            }
        }
    }
}
