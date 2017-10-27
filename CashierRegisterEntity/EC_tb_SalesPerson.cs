using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_SalesPerson
    {
        private int _No;

        public int No
        {
            get { return _No; }
            set { _No = value; }
        }

        private int _SalespersonID;

        public int SalespersonID
        {
            get { return _SalespersonID; }
            set { _SalespersonID = value; }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Password;

        private string _Birthday;

        public string Birthday
        {
            get { return _Birthday; }
            set { _Birthday = value; }
        }


        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private int _Active;

        public int Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        //add info
        private string _Background;

        public string Background
        {
            get { return _Background; }
            set { _Background = value; }
        }

        private double _Width;

        public double Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private bool _CheckDel;

        public bool CheckDel
        {
            get { return _CheckDel; }
            set { _CheckDel = value; }
        }

        private string _ImageUrl;

        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }

        private int _Default;

        public int Defaul
        {
            get { return _Default; }
            set { _Default = value; }
        }

        private string _ImageSelect;

        public string ImageSelect
        {
            get { return _ImageSelect; }
            set { _ImageSelect = value; }
        }
    }
}
