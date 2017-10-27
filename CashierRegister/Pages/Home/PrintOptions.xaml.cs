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

namespace CashierRegister.Pages.Home
{
    /// <summary>
    /// Interaction logic for PrintOptions.xaml
    /// </summary>
    public partial class PrintOptions : ModernWindow
    {
        private const double minimumInch = 1.5;
        private const double maximumInch = 15;

        private const double minimumCentimet = 3.81;
        private const double maximumCentimet = 38.1;

        private const double minimumFontSize = 9;
        private const double maximumFontSize = 25;

        public List<EC_tb_OrderDetail> list_ec_tb_orderdetail = new List<EC_tb_OrderDetail>();

        //PrintOptions
        public PrintOptions()
        {
            InitializeComponent();
            AddOrderDetail();
            tblGrbProperties.Foreground = new BrushConverter().ConvertFromString(StaticClass.GeneralClass.app_settings["accentColor"].ToString()) as Brush;
            tblGrbPreview.Foreground = new BrushConverter().ConvertFromString(StaticClass.GeneralClass.app_settings["accentColor"].ToString()) as Brush;
        }

        //ModernWindow_Loaded
        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //set font size for text
                sdFontSize.Minimum = minimumFontSize;
                sdFontSize.Maximum = maximumFontSize;
                sdFontSize.Value = Convert.ToDouble(StaticClass.GeneralClass.app_settings["fontSize"].ToString());
                Application.Current.Resources["font_size_val"] = Convert.ToDouble(StaticClass.GeneralClass.app_settings["fontSize"].ToString());

                //if the unit is inches
                if (StaticClass.GeneralClass.app_settings["unit"].ToString() == "0")
                {
                    sdPageWidth.Minimum = minimumInch;
                    sdPageWidth.Maximum = maximumInch;
                    sdPageWidth.Value = Convert.ToDouble(StaticClass.GeneralClass.app_settings["unitVal"].ToString()) / 96;
                }
                else //if units are centimeters
                {
                    sdPageWidth.Minimum = minimumCentimet;
                    sdPageWidth.Maximum = maximumCentimet;
                    sdPageWidth.Value = Convert.ToDouble(StaticClass.GeneralClass.app_settings["unitVal"].ToString()) * 2.54 / 96;
                }

                cboUnit.SelectedIndex = Convert.ToInt16(StaticClass.GeneralClass.app_settings["unit"].ToString());
                Application.Current.Resources["order_width_val"] = StaticClass.GeneralClass.app_settings["unitVal"].ToString();
                AddOrderDetail();
                sdPageWidth.ValueChanged += sdPageWidth_ValueChanged;
                sdFontSize.ValueChanged += sdFontSize_ValueChanged;
            }
            catch (Exception ex)
            {
                ShowNotification(ex.Message);
            }
        }

        //AddOrderDetail
        int _i;
        public void AddOrderDetail()
        {
            for (int i = 0; i < list_ec_tb_orderdetail.Count; i++)
            {
                //row data
                RowDefinition rd_data = new RowDefinition();
                grdOrderDetail.RowDefinitions.Add(rd_data);

                //No column
                TextBlock tblNo = new TextBlock() { Text = list_ec_tb_orderdetail[i].No.ToString() };
                tblNo.Style = Application.Current.Resources["textBlockNormalOrderStyle"] as Style;
                Grid.SetColumn(tblNo, 0);
                Grid.SetRow(tblNo, ++_i);
                grdOrderDetail.Children.Add(tblNo);

                //Name column
                TextBlock tblName = new TextBlock() { Text = list_ec_tb_orderdetail[i].ProductName, };
                tblName.Style = Application.Current.FindResource("textBlockNormalOrderStyle") as Style;
                Grid.SetColumn(tblName, 1);
                Grid.SetRow(tblName, _i);
                grdOrderDetail.Children.Add(tblName);

                //Price column
                TextBlock tblPrice = new TextBlock() { Text = list_ec_tb_orderdetail[i].StrPrice, HorizontalAlignment = HorizontalAlignment.Right};
                tblPrice.Style = Application.Current.FindResource("textBlockNormalOrderStyle") as Style;
                Grid.SetColumn(tblPrice, 2);
                Grid.SetRow(tblPrice, _i);
                grdOrderDetail.Children.Add(tblPrice);

                //Qty column
                TextBlock tblQty = new TextBlock() { Text = list_ec_tb_orderdetail[i].Qty.ToString(), HorizontalAlignment = HorizontalAlignment.Center};
                tblQty.Style = Application.Current.FindResource("textBlockNormalOrderStyle") as Style;
                Grid.SetColumn(tblQty, 3);
                Grid.SetRow(tblQty, _i);
                grdOrderDetail.Children.Add(tblQty);

                //Subtotal column
                TextBlock tblSubTotal = new TextBlock() { Text = list_ec_tb_orderdetail[i].Subtotal, HorizontalAlignment=HorizontalAlignment.Right};
                tblSubTotal.Style = Application.Current.FindResource("textBlockNormalOrderStyle") as Style;
                Grid.SetColumn(tblSubTotal, 4);
                Grid.SetRow(tblSubTotal, _i);
                grdOrderDetail.Children.Add(tblSubTotal);

                //row separator
                RowDefinition rd_se = new RowDefinition();
                grdOrderDetail.RowDefinitions.Add(rd_se);
                Separator sep = new Separator() { Margin = new Thickness(5, 3, 0, 0), Background = new BrushConverter().ConvertFromString("#FFBDBDBD") as Brush, VerticalAlignment = VerticalAlignment.Bottom, };
                Grid.SetColumn(sep, 0);
                Grid.SetColumnSpan(sep, 5);
                Grid.SetRow(sep, ++_i);
                grdOrderDetail.Children.Add(sep);
            }
        }

        //sdPageWidth_ValueChanged
        private void sdPageWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            //if the unit is inches
            if (cboUnit.SelectedIndex == 0)
                StaticClass.GeneralClass.app_settings["unitVal"] = Math.Round(slider.Value * 96, 1);
            else //if units are centimeters
                StaticClass.GeneralClass.app_settings["unitVal"] = Math.Round(slider.Value * 96 / 2.54, 1);

            Application.Current.Resources["order_width_val"] = StaticClass.GeneralClass.app_settings["unitVal"].ToString();
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
        }

        //sdFontSize_ValueChanged
        private void sdFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            StaticClass.GeneralClass.app_settings["fontSize"] = Math.Round(slider.Value, 1);
            Application.Current.Resources["font_size_val"] = StaticClass.GeneralClass.app_settings["fontSize"].ToString();
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
        }

        //cboUnit_SelectionChanged
        private void cboUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox unitsCombobox = sender as ComboBox;
            //if the unit is inches
            if (unitsCombobox.SelectedIndex == 0)
            {
                sdPageWidth.Minimum = minimumInch;
                sdPageWidth.Maximum = maximumInch;
                StaticClass.GeneralClass.app_settings["unitVal"] = Math.Round(sdPageWidth.Value * 96, 1);
            }
            else //if units are centimeters
            {
                sdPageWidth.Minimum = minimumCentimet;
                sdPageWidth.Maximum = maximumCentimet;
                StaticClass.GeneralClass.app_settings["unitVal"] = Math.Round(sdPageWidth.Value * 96 / 2.54, 1);
            }

            Application.Current.Resources["order_width_val"] = StaticClass.GeneralClass.app_settings["unitVal"].ToString();
            StaticClass.GeneralClass.app_settings["unit"] = unitsCombobox.SelectedIndex;
            Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
        }

        //ShowNotification
        private void ShowNotification(string _content)
        {
            ModernDialog md = new ModernDialog();
            md.Buttons = new Button[] { md.CloseButton };
            md.Title = FindResource("notification").ToString();
            md.Content = _content;
            md.CloseButton.Content = FindResource("close").ToString();
            md.CloseButton.Width = 80;
            md.ShowDialog();
        }

    }
}

