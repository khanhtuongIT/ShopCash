using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_Product
    {
        private int _Index;

        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        private int _No;

        public int No
        {
            get { return _No; }
            set { _No = value; }
        }

        private int _ProductID;

        public int ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }

        private string _ShortName;

        public string ShortName
        {
            get { return _ShortName; }
            set { _ShortName = value; }
        }

        private string _StrShortName;

        public string StrShortName
        {
            get { return _StrShortName; }
            set { _StrShortName = value; }
        }

        private string _LongName;

        public string LongName
        {
            get { return _LongName; }
            set { _LongName = value; }
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

        private int _Tax;

        public int Tax
        {
            get { return _Tax; }
            set { _Tax = value; }
        }

        private string _PathImage;

        public string PathImage
        {
            get { return _PathImage; }
            set { _PathImage = value; }
        }

        private System.Windows.Media.Imaging.BitmapImage _BitmapImage;
        public System.Windows.Media.Imaging.BitmapImage BitmapImage
        {
            get { return _BitmapImage; }
            set { _BitmapImage = value; }
        }

        private int _Capture;

        public int Capture
        {
            get { return _Capture; }
            set { _Capture = value; }
        }

        private int _Active;

        public int Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        private string _BarcodeID;

        public string BarcodeID
        {
            get { return _BarcodeID; }
            set { _BarcodeID = value; }
        }

        //add info
        private string _Currency;

        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        private bool _CheckDel;

        public bool CheckDel
        {
            get { return _CheckDel; }
            set { _CheckDel = value; }
        }

        //image edit
        private string _ImageSource;

        public string ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; }
        }

        //image view history
        private string _ViewHistory;

        public string ViewHistory
        {
            get { return _ViewHistory; }
            set { _ViewHistory = value; }
        }

        private string _CategoryName;

        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        private string _Foreground;

        public string Foreground
        {
            get { return _Foreground; }
            set { _Foreground = value; }
        }

        private string _StatusImage;

        public string StatusImage
        {
            get { return _StatusImage; }
            set { _StatusImage = value; }
        }

        private string _BeAdd;

        public string BeAdd
        {
            get { return _BeAdd; }
            set { _BeAdd = value; }
        }


        private string _AfAdd;

        public string AfAdd
        {
            get { return _AfAdd; }
            set { _AfAdd = value; }
        }

        private string _Country;

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _imgSelect;

        public string imgSelect
        {
            get { return _imgSelect; }
            set { _imgSelect = value; }
        }

        private string _SizeWeight;

        public string SizeWeight
        {
            get { return _SizeWeight; }
            set { _SizeWeight = value; }
        }

        private string _Image_Inventory_Status;

        public string Image_Iventory_Status
        {
            get { return _Image_Inventory_Status; }
            set { _Image_Inventory_Status = value; }
        }

        public static int CompareProductInventoryCount(EC_tb_Product p1, EC_tb_Product p2)
        {
            return p1.InventoryCount.CompareTo(p2.InventoryCount);
        }
    }
}
