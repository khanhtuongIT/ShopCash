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
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : UserControl
    {
        //thread
        private App app = Application.Current as App;
        private bool flag_check_loaded = false;
        private Thread thread_content = null;

        //Setting
        public Setting()
        {
            InitializeComponent();
        }

        //UCSetting_Loaded
        private void UCSetting_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;

                if (thread_content != null && thread_content.ThreadState == ThreadState.Running) { }
                else
                {
                    thread_content = new Thread(() =>
                    {
                        this.Dispatcher.Invoke((Action)(() =>  { this.mtSetting.Links.Clear(); }));
                        switch (GeneralClass.user_permission)
                        {
                            case (int)GeneralClass.UserPermission.admin: //admin permission
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.mtSetting.Links.Add(app.linkCategory);
                                    this.mtSetting.Links.Add(app.linkCurrency);
                                    this.mtSetting.Links.Add(app.linkCustomer);
                                    this.mtSetting.Links.Add(app.linkSalesperson);
                                    this.mtSetting.Links.Add(app.linkPayment);
                                    this.mtSetting.Links.Add(app.lnkGiftCard);
                                    this.mtSetting.Links.Add(app.linkUser);
                                    this.mtSetting.Links.Add(app.linkBackup);
                                    this.mtSetting.Links.Add(app.linkAppSetting);
                                    this.mtSetting.SelectedSource = new Uri(@"/Pages/Setting/Product.xaml", UriKind.Relative);
                                }));
                                break;
                            case (int)GeneralClass.UserPermission.inventory: //inventory permission
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.mtSetting.Links.Add(app.linkCategory);
                                    this.mtSetting.SelectedSource = new Uri(@"/Pages/Setting/Product.xaml", UriKind.Relative);
                                }));
                                break;
                        }
                    });
                    thread_content.Start();
                }
            }
        }
    }
}
