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

namespace CashierRegister.Pages.LoginGuide
{
    /// <summary>
    /// Interaction logic for FirstTimeLoginGuide.xaml
    /// </summary>
    public partial class FirstTimeLoginGuide : ModernWindow
    {
        private Pages.LoginGuide.AdminGuide admin_guide = null;
        private Pages.LoginGuide.InventoryGuide inventory_guide = null;
        private Pages.LoginGuide.ReportGuide report_guide = null;
        private Pages.LoginGuide.ChangeInfo change_info_guide = null;
        int index = 1;
        private BitmapImage bitmap_image = new BitmapImage(new Uri(@"pack://application:,,,/Resources/circle.png", UriKind.Absolute));
        private BitmapImage bitmap_image_focus = new BitmapImage(new Uri(@"pack://application:,,,/Resources/circle_focus.png", UriKind.Absolute));

        //FirstTimeLoginGuide
        public FirstTimeLoginGuide()
        {
            InitializeComponent();
            img1.Source = bitmap_image;
            img2.Source = bitmap_image;
            img3.Source = bitmap_image;
            img4.Source = bitmap_image;
            admin_guide = new AdminGuide();
            inventory_guide = new InventoryGuide();
            report_guide = new ReportGuide();
            change_info_guide = new ChangeInfo();
            ContentTransition.ShowContent(admin_guide);
            index++;
            img1.Source = bitmap_image_focus;
        }

        //btnReadMore_Click
        private void btnReadMore_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).mainWindow.ContentSource = new Uri(@"/Pages/SettingsPage.xaml", UriKind.Relative);
            StaticClass.GeneralClass.help_request = true;
            this.Close();
        }

        //img1_MouseLeftButtonDown
        private void tbl1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            img1_MouseLeftButtonDown(img1, null);
        }

        private void img1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeImageSelected(sender as Image);
        }

        //img2_MouseLeftButtonDown
        private void tbl2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            img2_MouseLeftButtonDown(img2, null);
        }

        private void img2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeImageSelected(sender as Image);
        }

        //img3_MouseLeftButtonDown
        private void tbl3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            img3_MouseLeftButtonDown(img3, null);
        }
        private void img3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeImageSelected(sender as Image);
        }

        //img4_MouseLeftButtonDown
        private void tbl4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            img4_MouseLeftButtonDown(img4, null);
        }

        private void img4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeImageSelected(sender as Image);
        }

        //ChangeImageSelected
        private void ChangeImageSelected(Image img)
        {
            if (index == 1)
                img4.Source = bitmap_image;
            else
                ((Image)stpIndex.FindName("img" + (index - 1))).Source = bitmap_image;

            index = Convert.ToInt32(img.Uid);
            MoveNextPage();
        }

        //ModernWindow_Closed
        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            if (chkShowGuide.IsChecked.Value)
            {
                StaticClass.GeneralClass.app_settings["showGuide"] = "False";
                Model.UpgradeDatabase.updateAppSetting(StaticClass.GeneralClass.app_settings);
            }
        }

        //MoveNextPage
        private void MoveNextPage()
        {
            switch (index)
            {
                case 1:
                    ContentTransition.ShowContent(admin_guide);
                    img4.Source = bitmap_image;
                    img1.Source = bitmap_image_focus;
                    index++;
                    break;
                case 2:
                    ContentTransition.ShowContent(inventory_guide);
                    img1.Source = bitmap_image;
                    img2.Source = bitmap_image_focus;
                    index++;
                    break;
                case 3:
                    ContentTransition.ShowContent(report_guide);
                    img2.Source = bitmap_image;
                    img3.Source = bitmap_image_focus;
                    index++;
                    break;
                case 4:
                    ContentTransition.ShowContent(change_info_guide);
                    img3.Source = bitmap_image;
                    img4.Source = bitmap_image_focus;
                    index = 1;
                    break;
            }
        }

    }
}
