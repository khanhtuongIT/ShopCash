using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_Payment
    {
        private int _No;

        public int No
        {
            get { return _No; }
            set { _No = value; }
        }

        private int _PaymentID;

        public int PaymentID
        {
            get { return _PaymentID; }
            set { _PaymentID = value; }
        }

        private string _Card;

        public string Card
        {
            get { return _Card; }
            set { _Card = value; }
        }
        private System.Windows.Controls.RadioButton _RdoCard;

        public System.Windows.Controls.RadioButton RdoCard
        {
            get { return _RdoCard; }
            set
            {
                _RdoCard = value;
                _RdoCard.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _RdoCard.FontWeight = System.Windows.FontWeights.Medium;
                _RdoCard.Content = _Card;
                _RdoCard.Margin = new System.Windows.Thickness(0, 10, 5, 5);
            }
        }

        private System.Windows.Controls.Separator _SepCard;

        public System.Windows.Controls.Separator SepCard
        {
            get { return _SepCard; }
            set
            {
                _SepCard = value;
                _SepCard.Background = new System.Windows.Media.BrushConverter().ConvertFromString("#FFEEEEEE") as System.Windows.Media.Brush;
            }
        }

        private string _EditImage;

        public string EditImage
        {
            get { return _EditImage; }
            set { _EditImage = value; }
        }

        private string _DeleteImage;
        public string DeleteImage
        {
            get { return _DeleteImage; }
            set { _DeleteImage = value; }
        }
    }
}
