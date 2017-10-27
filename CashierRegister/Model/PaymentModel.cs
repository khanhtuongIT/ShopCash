using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using CashierRegister.Helpers;

namespace CashierRegister.Model
{
    public class PaymentModel : ModelBase
    {
        private int _idPayment;
        private string _namePayment;
        private bool _isSelected = false;

        public int PaymentId
        {
            get
            {
                return _idPayment;
            }

            set
            {
                if (_idPayment != value)
                {
                    _idPayment = value;
                    RaisePropertyChanged("PaymentId");
                }
            }
        }
        public string PaymentName
        {
            get
            {
                return _namePayment;
            }

            set
            {
                if (_namePayment != value)
                {
                    _namePayment = value;
                    RaisePropertyChanged("PaymentName");
                }
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }
    }
}
