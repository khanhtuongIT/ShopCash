using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_Charting
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

        private double _Revenue;

        public double Revenue
        {
            get { return _Revenue; }
            set { _Revenue = value; }
        }

        private double _Profit;

        public double Profit
        {
            get { return _Profit; }
            set { _Profit = value; }
        }

        private int _OrderID;

        public int OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }
    }
}
