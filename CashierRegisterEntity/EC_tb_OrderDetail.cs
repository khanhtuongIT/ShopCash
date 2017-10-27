using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_OrderDetail
    {
        private int _No;

        public int No
        {
            get { return _No; }
            set { _No = value; }
        }

        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private int _CategoryID;

        public int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        private string _CategoryName;

        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        private int _ProductID;

        public int ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }

        private string _ProductName;

        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }

        private decimal _Cost;

        public decimal Cost
        {
            get { return _Cost; }
            set { _Cost = value; }
        }

        private string _StrCost;

        public string StrCost
        {
            get { return _StrCost; }
            set { _StrCost = value; }
        }

        private decimal _Price;

        public decimal Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        private string _StrPrice;

        public string StrPrice
        {
            get { return _StrPrice; }
            set { _StrPrice = value; }
        }

        private int _Qty;

        public int Qty
        {
            get { return _Qty; }
            set { _Qty = value; }
        }

        private decimal _Tax;

        public decimal Tax
        {
            get { return _Tax; }
            set { _Tax = value; }
        }

        private string _StrTax;

        public string StrTax
        {
            get { return _StrTax; }
            set { _StrTax = value; }
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

        private decimal _Total;

        public decimal Total
        {
            get { return _Total; }
            set { _Total = value; }
        }

        private string _StrTotal;

        public string StrTotal
        {
            get { return _StrTotal; }
            set { _StrTotal = value; }
        }

        private int _OrderID;

        public int OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }

        //add info
        private string _Currency;

        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        private string _DiscountTypeUnit0;

        public string DiscountTypeUnit0
        {
            get { return _DiscountTypeUnit0; }
            set { _DiscountTypeUnit0 = value; }
        }


        private string _DiscountTypeUnit1;

        public string DiscountTypeUnit1
        {
            get { return _DiscountTypeUnit1; }
            set { _DiscountTypeUnit1 = value; }
        }

        private float _DiscountUnit;

        public float DiscountUnit
        {
            get { return _DiscountUnit; }
            set { _DiscountUnit = value; }
        }

        private bool _CheckDel;

        public bool CheckDel
        {
            get { return _CheckDel; }
            set { _CheckDel = value; }
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

        private string _ImageDeleteUrl;

        public string ImageDeleteUrl
        {
            get { return _ImageDeleteUrl; }
            set { _ImageDeleteUrl = value; }
        }

        private string _Subtotal;

        public string Subtotal
        {
            get { return _Subtotal; }
            set { _Subtotal = value; }
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
