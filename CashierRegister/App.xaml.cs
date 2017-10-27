using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Presentation;

namespace CashierRegister
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //using for main window
        public ModernWindow mainWindow = null;
        public LinkGroup linkGroupHome = null;
        public LinkGroup linkGroupReport = null;
        public LinkGroup linkGroupSetting = null;
        public LinkGroup linkGroupChart = null;
        public LinkGroup linkGroupStatistic = null;
        public LinkGroup linkGroupOption = null;
        public LinkGroup linkGroupLogInOut = null;
        public Link linkLogin = null;
        public Link linkLogout = null;
        public Link linkAccount = null;

        //using for setting page
        public Link linkCategory = null;
        public Link linkCurrency = null;
        public Link linkCustomer = null;
        public Link linkSalesperson = null;
        public Link linkPayment = null;
        public Link linkUser = null;
        public Link linkBackup = null;
        public Link linkAppSetting = null;
        public Link lnkGiftCard = null;
    }
}
