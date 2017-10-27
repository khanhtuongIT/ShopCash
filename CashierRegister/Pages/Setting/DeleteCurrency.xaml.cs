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

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for DeleteCurrency.xaml
    /// </summary>
    public partial class DeleteCurrency : ModernDialog
    {
        //using for setting
        private BUS_tb_Setting bus_tb_setting = new BUS_tb_Setting();
        private BUS_tb_Order bus_tb_order = new BUS_tb_Order();

        //delegate
        public delegate void Add_Delete_Edit_Delegate();
        public event Add_Delete_Edit_Delegate delete_delegate;

        //DeleteSalesperson
        public DeleteCurrency()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            StaticClass.GeneralClass.list_ec_tb_setting_general.Clear();
            this.Close();
        }

        //muiBtnOK_Click
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (StaticClass.GeneralClass.list_ec_tb_setting_general.Count == 0)
                tblNotification.Text = FindResource("select_least_currency").ToString();
            else
            {
                try
                {
                    int result = 0;
                    for (int i = 0; i < StaticClass.GeneralClass.list_ec_tb_setting_general.Count; i++)
                    {
                        EC_tb_Setting ec_tb_setting = new EC_tb_Setting();
                        ec_tb_setting.SettingID = StaticClass.GeneralClass.list_ec_tb_setting_general[i].SettingID;

                        if (StaticClass.GeneralClass.list_ec_tb_setting_general[i].Active == 1)
                            tblNotification.Text = FindResource("currency").ToString() + (" ") + StaticClass.GeneralClass.list_ec_tb_setting_general[i].Currency + " " + FindResource("already_uses").ToString();
                        else
                        {
                            if (bus_tb_setting.DeleteSetting(ec_tb_setting) == 1)
                                result++;
                        }
                    }

                    if (result > 0)
                    {
                        if (delete_delegate != null)
                        {
                            StaticClass.GeneralClass.list_ec_tb_setting_general.Clear();
                            delete_delegate();
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    tblNotification.Text = ex.Message;
                }
            }
        }
    }
}
