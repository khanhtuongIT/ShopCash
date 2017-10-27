using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CashierRegisterDAL;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Data;
using CashierRegister.Helpers;

namespace CashierRegister.Model
{
    public class ShellOutModel : ModelBase
    {
        private int _paymentid;
        private string _paymentName;
        private string _paymentBalance;
        private string _showButtonDel = "Visible";
        public int PaymentId
        {
            get
            {
                return _paymentid;
            }
            set
            {
                if (_paymentid != value)
                {
                    _paymentid = value;
                    RaisePropertyChanged("PaymentId");
                }
            }
        }
        public string PaymentName
        {
            get
            {
                return _paymentName;
            }

            set
            {
                if (_paymentName != value)
                {
                    _paymentName = value;
                    RaisePropertyChanged("PaymentName");
                }
            }
        }

        public string PaymentBalance
        {
            get { return _paymentBalance; }

            set
            {
                if (_paymentBalance != value)
                {
                    _paymentBalance = value;
                    RaisePropertyChanged("PaymentBalance");
                }
            }
        }
        public string ShowButtonDel
        {
            get
            {
                return _showButtonDel;
            }
            set
            {
                _showButtonDel = value;
                RaisePropertyChanged("ShowButtonDel");
            }
        }
        public static DataTable getDefaultPayment()
        {
            string sql = "select PaymentID, Card from tb_Payment where [Name] = 'Cash'";
            return ConnectionDB.GetData(sql);
        }
        public static DataTable chkOrder(Int32 _orderId)
        {
            string sql = "select OrderPaymentID from tb_OrderPayment where OrderID =" + _orderId + " LIMIT 1";
            return ConnectionDB.GetData(sql);
        }
    }
    public class OrderPayment
    {
        public static int insertMultiRows(ObservableCollection<ShellOutModel> _shellout, int _orderId)
        {
            
            string sql = "INSERT INTO [tb_OrderPayment] ([OrderID], [PaymentID], [CardID], [Amount]) VALUES ";
            string val = "(";
            foreach(var shell in _shellout)
            {
                string _strId = shell.PaymentId.ToString();
                if (_strId.Length >= 3)
                {
                    _strId = _strId.Remove(2);
                }
                decimal _payBalance = StaticClass.GeneralClass.ConverStringToDecimal(shell.PaymentBalance);
                if (Convert.ToDouble(_payBalance) >0)
                    val = val + _orderId + ", " + Convert.ToInt16(_strId) + ", 0, "+ Convert.ToDouble(_payBalance) +"),(";
            }
            sql = sql + val.Remove(val.Length - 2, 2)+ ";";
            return ConnectionDB.ExecuteNonQuery(sql);
        }
        public static DataTable getOrderPaymentName(int _orderId)
        {
            string sql = "select OrderPaymentID, Card, Amount from tb_OrderPayment as op join tb_Payment as p on p.PaymentID = op.PaymentID where OrderID = " + Convert.ToInt16(_orderId);
            return ConnectionDB.GetData(sql);
        }
    }
}
