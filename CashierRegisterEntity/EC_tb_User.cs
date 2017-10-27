using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_User
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

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
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

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string _Question;

        public string Question
        {
            get { return _Question; }
            set { _Question = value; }
        }

        private string _Answer;

        public string Answer
        {
            get { return _Answer; }
            set { _Answer = value; }
        }

        private string _EditImage;

        public string EditImage
        {
            get { return _EditImage; }
            set { _EditImage = value; }
        }
    }
}
