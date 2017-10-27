using System;
using FirstFloor.ModernUI.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierRegister.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Data;
using System.Threading;
using CashierRegister.StaticClass;
using System.Globalization;
using System.Diagnostics;
using CashierRegister.Helpers;

namespace CashierRegister.ViewModel
{
    public class ShellOutViewModel : ModelBase
    {
        static public EventHandler Event1ExecutionHandler;
        static public EventHandler eventRemoveShellOut;
        static public EventHandler eventSavePaymentOther;
        static public EventHandler eventFillValueGiftCard;
        static public EventHandler eventResetListboxSelected;
        private decimal _total = StaticClass.GeneralClass.ConverStringToDecimal(StaticClass.GeneralClass.GetNumFormatEdit(StaticClass.GeneralClass.subtotal_general + StaticClass.GeneralClass.totaltaxrate_general - StaticClass.GeneralClass.totaldiscount_general));
        private bool _isDeleting = false;
        public string ButtonInvoice;
        public string ButtonEmailinvoice;
        private Int32 _intId = 0;
        private List<string> _lstIndex = new List<string>();
        //private List<int> iList = new List<int>();

        public RelayCommand AddShellOutCommand { get; set; }
        public RelayCommand removeShellOutCommand { get; set; }
        public RelayCommand nvtamEvent { get; set; }
        public RelayCommand LoadedCmd { get; set; }
        public RelayCommand MouseDownCmd { get; set; }
        public RelayCommand btDelPaymentCmd { get; private set; }
        public RelayCommand MouseDoubleClickCmd { get; private set; }
        public RelayCommand unloadedCmd { get; private set; }
        public RelayCommand<string> getTotalCmd { get; private set; }
        public ShellOutViewModel()
        {
            ShellOut = new ObservableCollection<ShellOutModel>();
            loadDefaultShellOut();
            AddShellOutCommand = new RelayCommand(AddShellOut);
            Event1ExecutionHandler += OnEvent1Execute;
            removeShellOutCommand = new RelayCommand(onEventRemoveShellOut);
            eventSavePaymentOther += OnEventSavePaymentOther;
            LoadedCmd = new RelayCommand(onLoadedCmd);
            MouseDownCmd = new RelayCommand(onMouseDownCmdEvent);
            eventFillValueGiftCard += OnEventFillValueGiftCard;
            eventResetListboxSelected += OnEventResetListboxSelected;
            btDelPaymentCmd = new RelayCommand(onButtonDelPaymentCmdExec);
            MouseDoubleClickCmd = new RelayCommand(onMouseDoubleClickExec);
            unloadedCmd = new RelayCommand(OnUnloadedCmd);
            getTotalCmd = new RelayCommand<string>(OnGetTotalExec);
        }
        private void OnGetTotalExec(string _param)
        {
            string _strId = ShellOut[SelectedIndex].PaymentId.ToString();
            if (_strId.Length >= 3)
            {
                _strId = _strId.Remove(2);
            }
            if (Convert.ToInt32(_strId) == 11)
                return;
            if (StaticClass.GeneralClass.ConverStringToDecimal(_param) != 0 && StaticClass.GeneralClass.ConverStringToDecimal(_param) != StaticClass.GeneralClass.ConverStringToDecimal(ShellOut[SelectedIndex].PaymentBalance))
            {
                ShellOut[SelectedIndex].PaymentBalance = _param;
                CanculatorTotalBalance();
            }
        }
        private void OnUnloadedCmd(object _param)
        {
            eventFillValueGiftCard -= OnEventFillValueGiftCard;
            eventSavePaymentOther -= OnEventSavePaymentOther;
            Event1ExecutionHandler -= OnEvent1Execute;
            eventResetListboxSelected -= OnEventResetListboxSelected;
            _lstIndex.Clear();
            _intId = 0;
            StaticClass.GeneralClass.lstCash.Clear();
            StaticClass.GeneralClass.customerGiftCard.Clear();
        }
        // Referency ************************************
        private string _txtTotal = StaticClass.GeneralClass.GetNumFormatDisplay(0);
        public string txtTotal
        {
            get
            {
                return _txtTotal;
            }
            set
            {
                if (_txtTotal != value)
                {
                    _txtTotal = value;
                    RaisePropertyChanged("txtTotal");
                }
            }
        }
        private string GetNumFormatDisplay(decimal _number)
        {
            #region
            //string value = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", num);
            //if (Properties.Settings.Default.decimalSeparator == 1)
            //    value = value.Replace(".", "*").Replace(",", ".").Replace("*", ",");
            #endregion

            NumberFormatInfo nfi = null;
            if (StaticClass.GeneralClass.app_settings["decimalSeparator"].ToString() == "0")
                nfi = new CultureInfo("en-US", false).NumberFormat;
            else
            {
                nfi = new CultureInfo("fr-FR", false).NumberFormat;
                nfi.NumberGroupSeparator = ".";
            }
            nfi.NumberDecimalDigits = 2;
            return _number.ToString("N", nfi);
        }
        private void onLoadedCmd(object param)
        {

            //decimal total = StaticClass.GeneralClass.subtotal_general + StaticClass.GeneralClass.totaltaxrate_general - StaticClass.GeneralClass.totaldiscount_general;
            //txtTotal = GetNumFormatDisplay(total);
            //txtBalance = GetNumFormatDisplay(0);
        }
        private void onMouseDownCmdEvent(object param)
        {
            try
            {
                string _id = ShellOut[SelectedIndex].PaymentId.ToString();
                Int32 _tempCustomerGiftIndex = 0;
                string _strRef = string.Empty;
                if (_id.Length >= 3)
                {
                    _tempCustomerGiftIndex = Convert.ToInt32(_id.Remove(0, 2));
                    _id = _id.Remove(2);
                    _strRef = _lstIndex[_tempCustomerGiftIndex];
                }
                else if (Convert.ToInt32(_id) == 11)
                {
                    _strRef = _lstIndex[0];
                }
                if (Convert.ToInt32(_id) == 11 && !_isDeleting)
                {
                    string _strComp = ShellOut[0].PaymentBalance + ":" + txtTotal;
                    Views.PaymentScan scanPage = new Views.PaymentScan(_tempCustomerGiftIndex, _strRef, _strComp);
                    scanPage.ShowDialog();
                }
                if(Convert.ToInt32(_id) != 11)
                {
                    decimal _decTmp = 0;
                    foreach (var _shell in ShellOut)
                    {
                        if(_shell.PaymentId != ShellOut[SelectedIndex].PaymentId)
                            _decTmp += StaticClass.GeneralClass.ConverStringToDecimal(_shell.PaymentBalance.ToString());
                    }
                    _decTmp = _decTmp - _total;
                    if (_decTmp < 0)
                        _decTmp = _decTmp * -1;
                    else if (_decTmp > 0 && _decTmp < StaticClass.GeneralClass.ConverStringToDecimal(ShellOut[0].PaymentBalance))
                        _decTmp = StaticClass.GeneralClass.ConverStringToDecimal(ShellOut[0].PaymentBalance.ToString()) - _decTmp;
                    else _decTmp = 0;
                    GetTotalTxt = StaticClass.GeneralClass.GetNumFormatDisplay(_decTmp);
                }
            }
            catch { }
        }
        private void OnEvent1Execute(object sender, EventArgs e)
        {
            PaymentModel payment = (PaymentModel)sender;
            if (payment.PaymentId == 11)
            {
                if (_intId == 0)
                {
                    ShellOut.Add(new ShellOutModel { PaymentId = payment.PaymentId, PaymentName = payment.PaymentName, PaymentBalance = "0" });
                    StaticClass.GeneralClass.lstCash.Add(Convert.ToInt32(payment.PaymentId));
                }
                else
                {
                    string _strId = payment.PaymentId.ToString() + _intId.ToString();
                    ShellOut.Add(new ShellOutModel { PaymentId = Convert.ToInt32(_strId), PaymentName = payment.PaymentName, PaymentBalance = "0" });
                    StaticClass.GeneralClass.lstCash.Add(Convert.ToInt32(_strId));
                }
                _lstIndex.Insert(_intId, "0:0");
                _intId++;
            }
            else
            {
                ShellOut.Add(new ShellOutModel { PaymentId = payment.PaymentId, PaymentName = payment.PaymentName, PaymentBalance = "0" });
                StaticClass.GeneralClass.lstCash.Add(Convert.ToInt32(payment.PaymentId));
            }
        }
        private void onEventRemoveShellOut(object parameter)
        {
            int _shellSelected = SelectedIndex;
            if (ShellOut.Count() == 1)
            {
                _shellSelected = 0;
            }
            else if(_shellSelected == ShellOut.Count()-1)
            {
                _shellSelected = ShellOut.Count() - 2;
            }
            if (parameter == null)
            {
                ModernDialog md = new ModernDialog();
                md.CloseButton.FindResource("close").ToString();
                md.Content = "Please select payment to delete !";
                md.Title = App.Current.FindResource("notification").ToString();
                md.ShowDialog();
            }
            else
            {
                
                ShellOutModel shellout = (ShellOutModel)parameter;
                StaticClass.GeneralClass.lstCash.Remove(shellout.PaymentId);
                int _index = SelectedIndex;
                string _customgift_ = ShellOut[_index].PaymentId.ToString();
                if(Convert.ToInt32(_customgift_) == 11)
                {
                    string _giftValue = _lstIndex[0];
                    if (!string.IsNullOrEmpty(_giftValue))
                    {
                        _lstIndex[0] = string.Empty;
                        int _found = _giftValue.IndexOf(":");
                        string _strVal_ = _giftValue.Remove(_found);
                        StaticClass.GeneralClass.customerGiftCard.Remove(Convert.ToInt32(_strVal_));
                    }
                }
                else if(_customgift_.Length >= 3)
                {
                    int _tempIndex_ = Convert.ToInt16(_customgift_.Remove(0, 2));
                    string _giftValue = _lstIndex[_tempIndex_];
                    if (!string.IsNullOrEmpty(_giftValue))
                    {
                        _lstIndex[_tempIndex_] = string.Empty;
                        int _found = _giftValue.IndexOf(":");
                        string _strVal_ = _giftValue.Remove(_found);
                        StaticClass.GeneralClass.customerGiftCard.Remove(Convert.ToInt32(_strVal_));
                    }
                }
                ShellOut.RemoveAt(_index);
                CanculatorTotalBalance();
                SelectedIndex = _shellSelected;
                _isDeleting = false;
                string _strId = ShellOut[SelectedIndex].PaymentId.ToString();
                if (_strId.Length >= 3)
                {
                    _strId = _strId.Remove(2);
                }
                if (Convert.ToInt32(_strId) == 11 && !_isDeleting)
                {
                    onMouseDownCmdEvent(null);
                }
            }
        }
        private void OnEventSavePaymentOther(object sender, EventArgs e)
        {
            DataTable _chkRow = ShellOutModel.chkOrder(Convert.ToInt32((int)sender));
            if (_chkRow.Rows.Count > 0) return;
            OrderPayment.insertMultiRows(ShellOut, Convert.ToInt32((int)sender));
            StaticClass.GeneralClass.lstCash.Clear();
            if (_lstIndex.Count > 0)
            {
                StaticClass.GeneralClass.customerGiftCard.Clear();
                List<int> _lstid = new List<int>();
                foreach(var _shell_ in ShellOut)
                {
                    string _strId = _shell_.PaymentId.ToString();
                    if (_strId == "11")
                    {
                        _lstid.Add(0);
                    }
                    else if (_strId.Length >= 3)
                    {
                        _strId = _strId.Substring(2);
                        _lstid.Add(Convert.ToInt32(_strId));
                    }
                }
                if (_lstid.Count > 0)
                {
                    DataTable _dt;
                    decimal _decVal;
                    foreach (int j in _lstid)
                    {
                        if(!string.IsNullOrEmpty(_lstIndex[j]) && _lstIndex[j] != "0:0")
                        {
                            string _strVal = _lstIndex[j];
                            int found = _strVal.IndexOf(":");
                            string _giftId = _strVal.Remove(found);
                            string _giftVal = _strVal.Substring(found + 1);
                            _dt = GiftCardModel.getGiftCard(Convert.ToInt64(_giftId));
                            _decVal = Convert.ToDecimal(_dt.Rows[0]["Balance"].ToString()) - StaticClass.GeneralClass.ConverStringToDecimal(_giftVal);
                            if (StaticClass.GeneralClass.ConverStringToDecimal(_giftVal) > 0)
                            {
                                StaticClass.GeneralClass.customerGiftCard.Add(Convert.ToInt32(_giftId));
                                StaticClass.GeneralClass.customerGiftValue.Add(_decVal);
                            }
                        }
                    }
                    if (StaticClass.GeneralClass.customerGiftCard.Count > 0)
                    {
                        if (GiftCardModel.updateMultiGiftCardUsed(StaticClass.GeneralClass.customerid_general, StaticClass.GeneralClass.customerGiftCard, StaticClass.GeneralClass.customerGiftValue) > 0)
                        {
                            StaticClass.GeneralClass.customerGiftCard.Clear();
                            StaticClass.GeneralClass.customerGiftValue.Clear();
                            _lstIndex.Clear();
                        }
                    }
                }
            }
        }
        private void AddShellOut(object parameter)
        {
            Views.Payment page = new Views.Payment();
            page.ShowDialog();
        }
        public ObservableCollection<ShellOutModel> ShellOut
        {
            get;
            set;
        }
        private void loadDefaultShellOut()
        {
            DataTable _shellOut = ShellOutModel.getDefaultPayment();
            if (_shellOut.Rows.Count > 0)
            {
                foreach (DataRow dr in _shellOut.Rows)
                {
                    ShellOut.Add(new ShellOutModel { PaymentId = Convert.ToInt32(dr["PaymentID"].ToString()), PaymentName = dr["Card"].ToString(), PaymentBalance = "0", ShowButtonDel = "Hidden" });
                    StaticClass.GeneralClass.lstCash.Add(Convert.ToInt32(dr["PaymentID"].ToString()));
                }
            }
            else
            {
                ShellOut.Add(new ShellOutModel { PaymentId = 1, PaymentName = "Cash", PaymentBalance = "0", ShowButtonDel = "Hidden" });
                StaticClass.GeneralClass.lstCash.Add(Convert.ToInt32(1));
            }
        }

        private object _selectedShellOut;
        public object SelectedShellOut
        {
            get
            {
                return _selectedShellOut;
            }
            set
            {
                if (_selectedShellOut != value)
                {
                    _selectedShellOut = value;
                    RaisePropertyChanged("SelectedShellOut");
                }
            }
        }
        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    RaisePropertyChanged("SelectedIndex");
                }
            }
        }
        private void OnEventFillValueGiftCard(object sender, EventArgs e)
        {
            ShellOutModel _shell = SelectedShellOut as ShellOutModel;
            string _objToString = sender as string;
            decimal _strVal = 0;
            int found = 0;
            if (!string.IsNullOrEmpty(_objToString))
            {
                found = _objToString.IndexOf(":");
                _strVal = StaticClass.GeneralClass.ConverStringToDecimal(_objToString.Substring(found + 1));
            }
            decimal decTotal = 0;
            decimal tmpTotal_ = 0;
            Int32 _indTemp = 0;
            foreach (var __shell in ShellOut)
            {
                decTotal += (_indTemp == SelectedIndex) ? 0 : StaticClass.GeneralClass.ConverStringToDecimal(__shell.PaymentBalance.ToString());
                _indTemp++;
            }
            tmpTotal_ = decTotal - _total + Convert.ToDecimal(_strVal);
            if (Convert.ToDecimal(_strVal) == 0)
            {
                _shell.PaymentBalance = StaticClass.GeneralClass.GetNumFormatDisplay(0);
                int _found = 0;
                string _strVal_ = string.Empty;
                string _giftValue = string.Empty;
                if (_shell.PaymentId == 11)
                {
                    _giftValue = _lstIndex[0];
                    _lstIndex[0] = string.Empty;
                    if (!string.IsNullOrEmpty(_giftValue))
                    {
                        _found = _giftValue.IndexOf(":");
                        _strVal_ = _giftValue.Remove(_found);
                        StaticClass.GeneralClass.customerGiftCard.Remove(Convert.ToInt32(_strVal_));
                    }
                }
                else
                {
                    string _tmpInd = Convert.ToString(_shell.PaymentId).Substring(2);
                    _giftValue = _lstIndex[Convert.ToInt32(_tmpInd)];
                    _lstIndex[Convert.ToInt32(_tmpInd)] = string.Empty;
                    if (!string.IsNullOrEmpty(_giftValue))
                    {
                        _found = _giftValue.IndexOf(":");
                        _strVal_ = _giftValue.Remove(_found);
                        StaticClass.GeneralClass.customerGiftCard.Remove(Convert.ToInt32(_strVal_));
                    }
                }
            }
            else
            {
                string _giftId = _objToString.Remove(found);
                Int32 _tempIndex;
                if (_shell.PaymentId == 11)
                {
                    _tempIndex = 0;
                }
                else
                {
                    string _tmpInd = Convert.ToString(_shell.PaymentId).Substring(2);
                    _tempIndex = Convert.ToInt32(_tmpInd);
                }
                if (StaticClass.GeneralClass.ConverStringToDecimal(ShellOut[0].PaymentBalance.ToString()) == 0)
                {
                    if(Convert.ToDecimal(_strVal) > ((decTotal - _total) * -1))
                    {
                        _shell.PaymentBalance = StaticClass.GeneralClass.GetNumFormatDisplay(((decTotal - _total) * -1));
                        _lstIndex[_tempIndex] = _giftId + ":" + _shell.PaymentBalance;
                        System.Diagnostics.Debug.WriteLine("1) "+ _lstIndex[_tempIndex]);
                    }
                    else
                    {
                        _shell.PaymentBalance = StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(_strVal));
                        _lstIndex[_tempIndex] = _giftId + ":" + _shell.PaymentBalance;
                        System.Diagnostics.Debug.WriteLine("2) " + _lstIndex[_tempIndex]);
                    }
                }
                else
                {
                    decimal _compare = StaticClass.GeneralClass.ConverStringToDecimal(ShellOut[0].PaymentBalance.ToString());
                    if (tmpTotal_ > _compare)
                    {
                        if (StaticClass.GeneralClass.ConverStringToDecimal(txtTotal) < 0)
                        {
                            _strVal = StaticClass.GeneralClass.ConverStringToDecimal(txtTotal) * (-1) + _compare;
                        }
                        else
                        {
                            _strVal = _compare - StaticClass.GeneralClass.ConverStringToDecimal(txtTotal);
                        }
                        _shell.PaymentBalance = StaticClass.GeneralClass.GetNumFormatDisplay(_strVal);
                        _lstIndex[_tempIndex] = _giftId + ":" + _shell.PaymentBalance;
                        System.Diagnostics.Debug.WriteLine("3) " + _lstIndex[_tempIndex]);
                    }
                    else
                    {
                        _shell.PaymentBalance = StaticClass.GeneralClass.GetNumFormatDisplay(_strVal);
                        _lstIndex[_tempIndex] = _giftId + ":" + _shell.PaymentBalance;
                        System.Diagnostics.Debug.WriteLine("4) " + _lstIndex[_tempIndex]);
                    }
                }
            }
            CanculatorTotalBalance();
        }
        private void CanculatorTotalBalance()
        {
            decimal decTotal = 0;
            foreach (var _shell in ShellOut)
            {
                decTotal += StaticClass.GeneralClass.ConverStringToDecimal(_shell.PaymentBalance.ToString());
            }
            decTotal = decTotal - _total;
            txtTotal = StaticClass.GeneralClass.GetNumFormatDisplay(decTotal);
            if (decTotal >= 0)
            {
                IsActiveButton = true;
                IsShowButtonAddPayment = "Hidden";
                OnFocusRequested(nameof(ButtonInvoice));
            }
            else
            {
                IsActiveButton = false;
                IsShowButtonAddPayment = "Visible";
                OnFocusRequested(null);
            }
        }
        private void OnEventResetListboxSelected(object sender, EventArgs e)
        {
            int _shellSelected = SelectedIndex;
            int _lengthShell = ShellOut.Count();
            if(_shellSelected != 0 && _shellSelected != -1)
            {
                if (_shellSelected == _lengthShell - 1)
                {
                    _shellSelected = _shellSelected - 1;
                    SelectedIndex = Convert.ToInt16(_shellSelected);
                }
                else
                {
                    _shellSelected = _shellSelected + 1;
                    SelectedIndex = Convert.ToInt16(_shellSelected);
                }
            }
            _isDeleting = false;
        }
        private void onButtonDelPaymentCmdExec(object param)
        {
            _isDeleting = true;
            ModernDialog mdd = new ModernDialog();
            mdd.Buttons = new Button[] { mdd.OkButton, mdd.CancelButton, };
            mdd.OkButton.TabIndex = 0;
            mdd.OkButton.Content = App.Current.FindResource("ok").ToString();
            mdd.CancelButton.TabIndex = 1;
            mdd.CancelButton.Content = App.Current.FindResource("cancel").ToString();
            mdd.TabIndex = -1;
            mdd.Height = 200;
            mdd.Title = App.Current.FindResource("notification").ToString();
            mdd.Content = App.Current.FindResource("really_want_delete").ToString();
            mdd.OkButton.Focus();
            mdd.ShowDialog();
            if (mdd.MessageBoxResult == System.Windows.MessageBoxResult.OK)
            {
                int j = 0;
                foreach (var _shell in ShellOut)
                {
                    if (_shell.PaymentId == Convert.ToInt32(param.ToString()))
                    {
                        break;
                    }
                    j++;
                }
                SelectedIndex = j;
                onEventRemoveShellOut(ShellOut[j]);
            }
            else _isDeleting = false;
        }
        private bool _isActiveButton = false;
        public bool IsActiveButton
        {
            get
            {
                return _isActiveButton;
            }
            set
            {
                _isActiveButton = value;
                RaisePropertyChanged("IsActiveButton");
            }
        }
        private string _isShowButtonAddPayment = "Visible";
        public string IsShowButtonAddPayment
        {
            get
            {
                return _isShowButtonAddPayment;
            }
            set
            {
                _isShowButtonAddPayment = value;
                RaisePropertyChanged("IsShowButtonAddPayment");
            }
        }
        private void onMouseDoubleClickExec(object param)
        {
            string _strId = ShellOut[SelectedIndex].PaymentId.ToString();
            string _strRef = string.Empty;
            Int32 _tempCustomerGiftIndex = 0;
            if (_strId.Length >= 3)
            {
                _tempCustomerGiftIndex = Convert.ToInt32(_strId.Remove(0, 2));
                _strId = _strId.Remove(2);
                _strRef = _lstIndex[_tempCustomerGiftIndex];
            }
            else if (Convert.ToInt32(_strId) == 11)
            {
                _strRef = _lstIndex[0];
            }
            if (Convert.ToInt32(_strId) == 11 && !_isDeleting)
            {
                string _strComp = ShellOut[0].PaymentBalance + ":" + txtTotal;
                Views.PaymentScan scanPage = new Views.PaymentScan(_tempCustomerGiftIndex, _strRef, _strComp);
                scanPage.ShowDialog();
            }
        }
        private string _getTotalTxt;
        public string GetTotalTxt
        {
            get { return _getTotalTxt; }
            set
            {
                _getTotalTxt = value;
                RaisePropertyChanged("GetTotalTxt");
            }
        }
    }
}
