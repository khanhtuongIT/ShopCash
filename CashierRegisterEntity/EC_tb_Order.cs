using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_Order
    {
        private int _No;

        public int No
        {
            get { return _No; }
            set { _No = value; }
        }

        private int _OrderID;

        public int OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }

        private int _CustomerID;

        public int CustomerID
        {
          get { return _CustomerID; }
          set { _CustomerID = value; }
        }

        private string _CustomerName;

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }

        private int _Quatity;

        public int Quatity
        {
            get { return _Quatity; }
            set { _Quatity = value; }
        }

        private string _OrderDate;

        public string OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; }
        }

        private int _SalesPersonID;

        public int SalesPersonID
        {
            get { return _SalesPersonID; }
            set { _SalesPersonID = value; }
        }

        private string _SalesPersonName;

        public string SalesPersonName
        {
            get { return _SalesPersonName; }
            set { _SalesPersonName = value; }
        }

        private int _PaymentID;

        public int PaymentID
        {
            get { return _PaymentID; }
            set { _PaymentID = value; }
        }

        private string _PaymentName;

        public string PaymentName
        {
            get { return _PaymentName; }
            set { _PaymentName = value; }
        }

        private int _DiscountType;

        public int DiscountType
        {
            get { return _DiscountType; }
            set { _DiscountType = value; }
        }

        private decimal _Discount;

        public decimal Discount
        {
            get { return _Discount; }
            set { _Discount = value; }
        }

        private string _StrDiscount;

        public string StrDiscount
        {
            get { return _StrDiscount; }
            set { _StrDiscount = value; }
        }

        private decimal _TotalDiscount;

        public decimal TotalDiscount
        {
            get { return _TotalDiscount; }
            set { _TotalDiscount = value; }
        }

        private string _StrTotalDiscount;

        public string StrTotalDiscount
        {
            get { return _StrTotalDiscount; }
            set { _StrTotalDiscount = value; }
        }

        private decimal _TotalTax;

        public decimal TotalTax
        {
            get { return _TotalTax; }
            set { _TotalTax = value; }
        }

        private string _StrTotalTax;

        public string StrTotalTax
        {
            get { return _StrTotalTax; }
            set { _StrTotalTax = value; }
        }

        private decimal _TotalAmount;

        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set { _TotalAmount = value; }
        }

        private string _StrTotalAmount;

        public string StrTotalAmount
        {
            get { return _StrTotalAmount; }
            set { _StrTotalAmount = value; }
        }

        //add info
        private bool _CheckDel;

        public bool CheckDel
        {
            get { return _CheckDel; }
            set { _CheckDel = value; }
        }

        private string _ImageSource;


        public string ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; }
        }

        public static int CompareOrderID(EC_tb_Order o1, EC_tb_Order o2)
        {
            return o2.OrderID.CompareTo(o1.OrderID);
        }

        private string _Phone;

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        private string _Currency;

        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        private string _DisPercent;

        public string DisPercent
        {
            get { return _DisPercent; }
            set { _DisPercent = value; }
        }

        private string _DisAmount;

        public string DisAmount
        {
            get { return _DisAmount; }
            set { _DisAmount = value; }
        }
    }
}
