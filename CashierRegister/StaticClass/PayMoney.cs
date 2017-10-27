using System.Windows;
using System.Windows.Controls;
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Home
{
    public class PayMoney
    {
        public PayMoney() { }
        public PayMoney(string _currency, double _money, int _row, int _col, Grid _grid, Thickness _thickness, RoutedEventHandler _btnMoney_Click)
        {
            Currency = _currency;
            Money = _money;
            Row = _row;
            Col = _col;
            this.Thickness = _thickness;
            btnMoney_Click = _btnMoney_Click;
            btnMoney = new Button();
            _grid.Children.Add(_btnMoney);
        }

        public string Currency { get; set; }
        public double Money { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public Thickness Thickness { get; set; }
        public RoutedEventHandler btnMoney_Click { get; set; }

        private Button _btnMoney;
        public Button btnMoney
        {
            get { return _btnMoney; }
            set
            {
                _btnMoney = value;
                Grid.SetRow(_btnMoney, Row);
                Grid.SetColumn(_btnMoney, Col);
                if (Money == 100)
                    Grid.SetColumnSpan(_btnMoney, 2);
                _btnMoney.DataContext = this;
                Style style = new Style();
                style = System.Windows.Application.Current.TryFindResource("buttonMoneyStyle") as Style;
                _btnMoney.Style = style;
                _btnMoney.Margin = Thickness;
                StackPanel stp = new StackPanel() { Orientation = Orientation.Horizontal, Background = System.Windows.Media.Brushes.Transparent };
                stp.Children.Add(new TextBlock() { Text = Currency + GeneralClass.GetNumFormatDisplay((decimal)Money) });
                _btnMoney.Content = stp;
                _btnMoney.Click += btnMoney_Click;
            }
        }
    }
}
