using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegister.Helpers;
using CashierRegisterDAL;
using System.Data;
using System.Collections.ObjectModel;

namespace CashierRegister.Model
{
    public class GiftCardModel : ModelBase
    {
        private int _GiftCardID = 0;
        private string _Barcode = null;
        private string _Serial = null;
        private string _CreateDate = "";
        private string _ExpirationDate = "";
        private int _CustomerIDUse = 0;
        private double _Amount = 0;
        private double _Balance = 0;
        private bool _IsChecked = false;
        private int _No;
        public int GiftCardID
        {
            get
            {
                return _GiftCardID;
            }
            set
            {
                if (_GiftCardID != value)
                {
                    _GiftCardID = value;
                    RaisePropertyChanged("GiftCardID");
                }
            }
        }
        public string Barcode
        {
            get
            {
                return _Barcode;
            }
            set
            {
                if (_Barcode != value)
                {
                    _Barcode = value;
                    RaisePropertyChanged("Barcode");
                }
            }
        }
        public string Serial
        {
            get
            {
                return _Serial;
            }
            set
            {
                if (_Serial != value)
                {
                    _Serial = value;
                    RaisePropertyChanged("Serial");
                }
            }
        }
        public string CreateDate
        {
            get
            {
                return _CreateDate;
            }
            set
            {
                if (_CreateDate != value)
                {
                    _CreateDate = value;
                    RaisePropertyChanged("CreateDate");
                }
            }
        }
        public string ExpirationDate
        {
            get
            {
                return _ExpirationDate;
            }
            set
            {
                if (_ExpirationDate != value)
                {
                    _ExpirationDate = value;
                    RaisePropertyChanged("ExpirationDate");
                }
            }
        }
        public int CustomerIDUse
        {
            get
            {
                return _CustomerIDUse;
            }
            set
            {
                if (_CustomerIDUse != value)
                {
                    _CustomerIDUse = value;
                    RaisePropertyChanged("CustomerIDUse");
                }
            }
        }
        public double Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    RaisePropertyChanged("Amount");
                }
            }
        }
        public double Balance
        {
            get
            {
                return _Balance;
            }
            set
            {
                if (_Balance != value)
                {
                    _Balance = value;
                    RaisePropertyChanged("Balance");
                }
            }
        }
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                if(_IsChecked != value)
                {
                    _IsChecked = value;
                    RaisePropertyChanged("IsChecked");
                }
            }
        }
        public int No
        {
            get
            {
                return _No;
            }
            set
            {
                if (_No != value)
                {
                    _No = value;
                    RaisePropertyChanged("No");
                }
            }
        }
        private string _customerName = string.Empty;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                RaisePropertyChanged("CustomerName");
            }
        }
        private string _deliveredDate;
        public string DeliveredDate
        {
            get { return _deliveredDate; }
            set
            {
                _deliveredDate = value;
                RaisePropertyChanged("DeliveredDate");
            }
        }
        public static string GiftCardImgPath = System.IO.Directory.GetCurrentDirectory() + @"\images" + @"\";
        private static bool _sqlType = StaticClass.GeneralClass.flag_database_type_general;
        public static int insertGiftCard(ObservableCollection<GiftCardModel> _listCard)
        {
            if (_listCard == null || _listCard.Count==0) return 0;
            string sql = "insert into tb_GiftCard (Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, DeliveredDate) values (";
            foreach(var _gift in _listCard)
            {
                if (!_sqlType)
                {
                    sql += "'" + _gift.Barcode + "', '" + _gift.Serial + "', " + _gift.CreateDate + ", " + Convert.ToInt32(_gift.ExpirationDate) + ", " + _gift.CustomerIDUse + ", " + Convert.ToDouble(_gift.Amount) + ", " + Convert.ToDouble(_gift.Balance) + ", 0), (";
                }
                else sql += "N'" + _gift.Barcode + "', N'" + _gift.Serial + "', " + _gift.CreateDate + ", " + Convert.ToInt32(_gift.ExpirationDate) + ", " + _gift.CustomerIDUse + ", " + Convert.ToDouble(_gift.Amount) + ", " + Convert.ToDouble(_gift.Balance) + ", 0), (";
            }
            sql = sql.Remove(sql.Length - 3, 3);
            return ConnectionDB.ExecuteNonQuery(sql);
        }
        public static DataTable getGiftCardByBarcode(string _barcode)
        {
            string sql = "";
            if (_barcode.Trim().ToString() == "") return null;
            if(!_sqlType)
                sql = "select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance from tb_GiftCard where Barcode = '" + _barcode + "'";
            else sql = "select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance from tb_GiftCard where Barcode = N'" + _barcode + "'";
            return ConnectionDB.GetData(sql);
        }
        public static DataTable getGiftCard(Int64 _giftId)
        {
            if (Convert.ToUInt64(_giftId) == 0) return null;
            string sql = "select Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, DeliveredDate from tb_GiftCard where GiftCardID = " + _giftId;
            return ConnectionDB.GetData(sql);
        }
        public static int updateGiftCard(GiftCardModel _gift)
        {
            string sql = "update tb_GiftCard set ExpirationDate = " + Convert.ToInt64(_gift.ExpirationDate) + ", Amount = "+ _gift.Amount + ", Balance = Balance + " + (_gift.Amount - _gift.Balance) + " where GiftCardID = " + _gift.GiftCardID;
            return ConnectionDB.ExecuteNonQuery(sql);
        }
        public static DataTable searchGiftCardByBarcode(string _barcode, string _options)
        {
            string sql = "";
            if (_barcode.Trim().ToString() == "") return null;
            if (!_sqlType)
                sql = "select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance from tb_GiftCard where Barcode like '%" + Convert.ToString(_barcode) + "%' order by CreateDate DESC" + _options;
            else sql = "select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance from tb_GiftCard where Barcode like N'%" + Convert.ToString(_barcode) + "%' order by CreateDate DESC" + _options;
            return ConnectionDB.GetData(sql);
        }
        public static int updateMultiGiftCardUsed(int _customerId, List<Int32> _lstCardId, List<decimal> _lstUseValue)
        {
            string sql = "update tb_GiftCard set CustomerIDUse="+ _customerId+ ", Balance = case";
            string strWhere = " WHERE GiftCardID IN (";
            int j = 0;
            foreach(var _cardId in _lstCardId)
            {
                sql += " when GiftCardID = " + _cardId + " then " + Convert.ToDouble(_lstUseValue[j]);
                strWhere += _cardId + ", ";
                j++;
            }
            sql += " ELSE Balance END";
            strWhere = strWhere.Remove(strWhere.Length - 2, 2);
            strWhere += ")";
            return ConnectionDB.ExecuteNonQuery(sql + strWhere);
        }
        public static DataTable getGiftCardById(List<Int32> _lstId)
        {
            if (_lstId == null || _lstId.Count == 0) return null;
            string sql = "select Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance from tb_GiftCard where GiftCardID in (" + string.Join(",", _lstId.ToArray()) + ")";
            return ConnectionDB.GetData(sql);
        }
        public static int delGiftcardByOpstions(string _opts)
        {
            if (string.IsNullOrEmpty(_opts)) return 1;
            string sql = "delete from tb_GiftCard where " + _opts;
            return ConnectionDB.ExecuteNonQuery(sql);
        }
        public static DataTable advanceSearchGiftCardByMultiCondition(string _strWhere, string _strLimit, string _strSortBy)
        {
            string sql = "";
            string _orderBy = (string.IsNullOrEmpty(_strSortBy)) ? " order by CreateDate DESC" : _strSortBy;
            string _where;
            if (!_sqlType)
            {
                _where = (string.IsNullOrEmpty(_strWhere)) ? "" : "where "+ _strWhere.Replace(@"$", @"");
                sql = "select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, DeliveredDate, FirstName || \' \' || LastName as FullName from tb_GiftCard as gc left join tb_Customer as c on c.CustomerID = gc.CustomerIDUse " + _where + _orderBy + _strLimit;
            }
            else
            {
                if (!string.IsNullOrEmpty(_strLimit))
                {
                    int _ind = _strLimit.IndexOf(",");
                    string _tmp = _strLimit.Substring(0, _ind);
                    int _rows = Convert.ToInt16(_strLimit.Substring(_ind + 1));
                    int _offset = Convert.ToInt16(_tmp.Substring("limit ".Length));
                    _strLimit = " OFFSET " + _offset + " ROWS FETCH NEXT " + _rows + " ROWS ONLY;";
                }
                
                _where = (string.IsNullOrEmpty(_strWhere)) ? "" : "where " + _strWhere.Replace(@"$", @"N");
                sql = "select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, DeliveredDate, CONCAT(FirstName, \' \', LastName) as FullName from tb_GiftCard as gc left join tb_Customer as c on c.CustomerID = gc.CustomerIDUse " + _where + _orderBy + _strLimit;
            }
            return ConnectionDB.GetData(sql);
        }
        public static int setDeliveredGiftcard(List<Int32> _lstCard, Int32 date)
        {
            string sql = "update tb_GiftCard set DeliveredDate = " + date + " where GiftCardID in ("+ string.Join(",", _lstCard.ToArray()) + ")";
            return ConnectionDB.ExecuteNonQuery(sql);
        }
    }
    public class GiftCard_DB
    {
        private static bool _sqlType = StaticClass.GeneralClass.flag_database_type_general;
        public static int insertGiftCard(GiftCardModel _listCard)
        {
            if (_listCard == null) return 0;
            string sql = "insert into tb_GiftCard (Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance, IsDelivered) values (";
            if (!_sqlType)
            {
                sql += "'"+ _listCard.Barcode + "', '"+_listCard.Serial+"', "+_listCard.CreateDate+", "+_listCard.ExpirationDate+", "+_listCard.CustomerIDUse+", "+Convert.ToDouble(_listCard.Amount)+", "+Convert.ToDouble(_listCard.Balance)+", 0)";
            }
            else sql += "N'" + _listCard.Barcode + "', N'" + _listCard.Serial + "', " + _listCard.CreateDate + ", " + _listCard.ExpirationDate + ", " + _listCard.CustomerIDUse + ", " + Convert.ToDouble(_listCard.Amount) + ", " + Convert.ToDouble(_listCard.Balance) + ", 0)";
            return ConnectionDB.ExecuteNonQuery(sql);
        }
        public static int updateGiftCard(GiftCardModel _giftCard)
        {
            if (_giftCard == null) return 0;
            string sql = "update tb_GiftCard set ";
            if (!_sqlType)
            {
                sql += "Barcode = '"+ _giftCard .Barcode+ "', Serial = '"+_giftCard.Serial+ "', CreateDate = "+_giftCard.CreateDate+ ", ExpirationDate = "+_giftCard.ExpirationDate+ ", CustomerIDUse = "+_giftCard.CustomerIDUse+ ", Amount = "+_giftCard.Amount+ ", Balance = "+_giftCard.Balance;
            }
            else sql += "Barcode = N'" + _giftCard.Barcode + "', Serial = N'" + _giftCard.Serial + "', CreateDate = " + _giftCard.CreateDate + ", ExpirationDate = " + _giftCard.ExpirationDate + ", CustomerIDUse = " + _giftCard.CustomerIDUse + ", Amount = " + _giftCard.Amount + ", Balance = " + _giftCard.Balance;
            return ConnectionDB.ExecuteNonQuery(sql);
        }
        public static int deleteGiftCard(List<int> _listId)
        {
            if (_listId == null || _listId.Count==0) return 0;
            string sql = "delete from tb_GiftCard where GiftCardID in (";
            if (_listId.Count > 1)
            {
                foreach(var _id in _listId)
                {
                    sql += _id + ",";
                }
                sql = sql.Remove(sql.Length - 1, 1) + ")";
            }
            else sql += _listId[0]+")";
            return ConnectionDB.ExecuteNonQuery(sql);
        }
        /*public static DataTable getAllGiftCards()
        {
            string sql = "select GiftCardID, Barcode, Serial, CreateDate, ExpirationDate, CustomerIDUse, Amount, Balance from tb_GiftCard";
            return ConnectionDB.GetData(sql);
        }*/
    }
    
}
