using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_Date
    {
        private string _Year;

        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        private string _Month;

        public string Month
        {
            get { return _Month; }
            set { _Month = value; }
        }

        private string _Day;
        public string Day
        {
            get { return _Day; }
            set { _Day = value; }
        }
    }
}
