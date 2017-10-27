using System;
using FirstFloor.ModernUI.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegister.Model;
using System.Collections.ObjectModel;
using System.Windows;
using CashierRegisterBUS;
using CashierRegisterEntity;
using System.Data;
using CashierRegister.Helpers;
using System.Diagnostics;
using CashierRegister.StaticClass;
using System.Globalization;

namespace CashierRegister.ViewModel
{
    public class PaymentViewModel
    {
        public ObservableCollection<PaymentModel> Payment
        {
            get;
            set;
        }
        static public EventHandler eventSenderParameter;
        private BUS_tb_Payment bus_tb_payment = new BUS_tb_Payment();
        private List<PaymentModel> lstSelect = new List<PaymentModel>();
        public RelayCommand addPaymentCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        private List<int> _param = new List<int>();

        public PaymentViewModel()
        {
            eventSenderParameter += OnEventSenderParameter;
            GetPayment();
            addPaymentCommand = new RelayCommand(AddPayment);
            this.CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow);
        }
        private void OnEventSenderParameter(object sender, EventArgs e)
        {
            _param = (List<int>)sender;
        }
        private bool allReadyExits(List<int> lst, int val)
        {
            foreach(var intVal in lst)
            {
                if (Convert.ToInt16(intVal) == Convert.ToInt16(val))
                    return true;
            }
            return false;
        }
        private void GetPayment()
        {
            ObservableCollection<PaymentModel> _payment = new ObservableCollection<PaymentModel>();
            foreach (DataRow dr in bus_tb_payment.GetPayment("").Rows)
            {
                if (Convert.ToInt32(dr["PaymentID"].ToString())==11 || !allReadyExits(StaticClass.GeneralClass.lstCash, Convert.ToInt32(dr["PaymentID"].ToString())))
                {
                    _payment.Add(new PaymentModel { PaymentId = Convert.ToInt32(dr["PaymentID"].ToString()), PaymentName = dr["Card"].ToString() });
                }
            }
            Payment = _payment;
        }
        private void AddPayment(object parameter)
        {
            if (parameter == null) return;
            foreach (var payment in Payment)
            {
                if (payment.IsSelected == true)
                {
                    ShellOutViewModel.Event1ExecutionHandler(payment, null);
                }
                    
            }
            this.CloseWindow((Window)parameter);
        }
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
