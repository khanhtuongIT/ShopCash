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

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for Logs.xaml
    /// </summary>
    public partial class Logs : ModernWindow
    {
        private List<Cls_Logs> list_log = new List<Cls_Logs>();

        public Logs()
        {
            InitializeComponent();
            //lbLogs.ItemsSource = list_log;
        }

        //ModernWindow_Loaded
        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(System.IO.File.Exists("Logs"))
            {
                try
                {
                    if (System.IO.File.Exists("Logs"))
                    {
                        System.IO.StreamReader stream_reader = StaticClass.GeneralClass.DecryptFileGD("Logs", StaticClass.GeneralClass.key_register_general);
                        tblLogs.Text = stream_reader.ReadToEnd();
                        if (stream_reader != null)
                            stream_reader.Close();
                    }
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("================> The logs of login/logout: " + ex.Message);

                    if (System.IO.File.Exists("Logs"))
                    {
                        System.IO.File.Delete("Logs");
                        tblLogs.Text = "";
                    }
                }
            }
        }

        //hplClear_Click
        private void hplClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists("Logs"))
                {
                    System.IO.File.Delete("Logs");
                    tblLogs.Text = "";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("================> The logs of login/logout: " + ex.Message);
            }
        }
    }

    //Cls_Logs
    public class Cls_Logs
    {
        public string Log { get; set; }
    }
}
