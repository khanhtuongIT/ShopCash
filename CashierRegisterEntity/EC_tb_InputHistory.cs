using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_InputHistory
    {
        private int _No;

        public int No
        {
            get { return _No; }
            set { _No = value; }
        }

        private int _InputID;

        public int InputID
        {
            get { return _InputID; }
            set { _InputID = value; }
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

        private string _InputDate;

        public string InputDate
        {
            get { return _InputDate; }
            set { _InputDate = value; }
        }

        private int _UserID;

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private decimal _Cost;

        public decimal Cost
        {
            get { return _Cost; }
            set { _Cost = value; }
        }

        public string StrCost { get; set; }

        private decimal _Price;

        public decimal Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        public string StrPrice { get; set; }

        private int _InventoryCount;

        public int InventoryCount
        {
            get { return _InventoryCount; }
            set { _InventoryCount = value; }
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

        private int _Tax;

        public int Tax
        {
            get { return _Tax; }
            set { _Tax = value; }
        }

        private int _Active;

        public int Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        private string _Country;

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _SizeWeight;

        public string SizeWeight
        {
            get { return _SizeWeight; }
            set { _SizeWeight = value; }
        }

        private string _Currency;

        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }
    }
}
