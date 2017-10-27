using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_Setting
    {
        private int _No;

        public int No
        {
            get { return _No; }
            set { _No = value; }
        }

        private int _SettingID;

        public int SettingID
        {
            get { return _SettingID; }
            set { _SettingID = value; }
        }

        private string _Currency;

        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        private decimal _TaxRate;

        public decimal TaxRate
        {
            get { return _TaxRate; }
            set { _TaxRate = value; }
        }

        public string StrTaxRate { get; set; }

        private int _Active;

        public int Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        private int _Version;

        public int Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        private string _ImageUrl;

        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }

        private bool _CheckDel;

        public bool CheckDel
        {
            get { return _CheckDel; }
            set { _CheckDel = value; }
        }
    }
}
