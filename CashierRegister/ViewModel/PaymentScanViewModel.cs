using CashierRegister.Helpers;
using CashierRegister.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierRegister.ViewModel
{
    public class PaymentScanViewModel : ModelBase
    {
        public RelayCommand CloseWindowCommand { get; private set; }
        public RelayCommand<string> keyDownEnterCmd { get; private set; }
        public RelayCommand<string> starScanGiftCadrCmd { get; private set; }
        public RelayCommand lostFocusCmd { get; private set; }
        public RelayCommand scanGiftLoadedCmd { get; private set; }
        public static Int32 ScanIndex = 0;
        public static string ScanRef = string.Empty;
        public static string StrCompare = string.Empty;
        private Int32 ScanGiftId = 0;
        private bool _isLoaded = false;

        private string _barcode;
        public string Barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
                _barcode = value;
                RaisePropertyChanged("Barcode");
            }
        }
        private string _strNotify;
        public string StringNotify
        {
            get
            {
                return _strNotify;
            }
            set
            {
                _strNotify = value;
                RaisePropertyChanged("StringNotify");
            }
        }
        public PaymentScanViewModel()
        {
            CloseWindowCommand = new RelayCommand(OnCloseWindowEvent);
            keyDownEnterCmd = new RelayCommand<string>(onKeyDownEnterEvent);
            starScanGiftCadrCmd = new RelayCommand<string>(onStarScanGiftCadrEvent);
            lostFocusCmd = new RelayCommand(onLostFocusEvent);
            scanGiftLoadedCmd = new RelayCommand(onScanGiftLoadedExec);
        }
        private void onScanGiftLoadedExec(object _param)
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                if (!string.IsNullOrEmpty(ScanRef))
                {
                    int _found = ScanRef.IndexOf(":");
                    string _giftId_ = ScanRef.Remove(_found);
                    string _giftVal_ = ScanRef.Remove(0, _found+1);
                    System.Diagnostics.Debug.WriteLine(_giftVal_);
                    List<Int32> _lstGift_ = new List<int>();
                    _lstGift_.Add(Convert.ToInt32(_giftId_));
                    DataTable _gift_ = GiftCardModel.getGiftCardById(_lstGift_);
                    if (_gift_.Rows.Count > 0)
                    {
                        Barcode = _gift_.Rows[0]["Barcode"].ToString();
                        GiftSerial = _gift_.Rows[0]["Serial"].ToString();
                        GiftBalance = _gift_.Rows[0]["Balance"].ToString();
                        UseValue = _giftVal_;
                    }
                    ScanGiftId = Convert.ToInt32(_giftId_);
                }
            }
            else
            {
                _isLoaded = false;
            }
        }
        private void OnCloseWindowEvent(object param)
        {
            //ShellOutViewModel.eventResetListboxSelected(null, null);
            OnRequestClose();
        }
        private bool allReadyExits(List<Int32> lst, string val)
        {
            foreach (var intVal in lst)
            {
                if (Convert.ToInt32(intVal) == Convert.ToInt32(val))
                    return true;
            }
            return false;
        }
        private void onStarScanGiftCadrEvent(string param)
        {
            if (string.IsNullOrEmpty(Barcode) || Barcode.Trim().ToString()=="")
            {
                Barcode = "";
                StringNotify = App.Current.FindResource("barcode_null").ToString();
                OnFocusRequested(nameof(Barcode));
            }
            else if (Barcode.Trim()=="0")
            {
                ShellOutViewModel.eventFillValueGiftCard("0:0", null);
                //ShellOutViewModel.eventResetListboxSelected(null, null);
                OnFocusRequested(nameof(Barcode));
                OnRequestClose();
            }
            else
            {
                DataTable _gift = GiftCardModel.getGiftCardByBarcode(Barcode.Trim());
                if (_gift.Rows.Count == 0)
                {
                    StringNotify = App.Current.FindResource("giftcard_not_existing").ToString();
                }
                else
                {
                    bool _isError = false;
                    Int32 unixTimestamp = getCurrentUnixTime();
                    Int32 _expreTime = Convert.ToInt32(_gift.Rows[0]["ExpirationDate"]);
                    if (unixTimestamp > _expreTime)
                    {
                        StringNotify = App.Current.FindResource("giftcard_expired").ToString();
                        OnFocusRequested(nameof(Barcode));
                        _isError = true;
                    }
                    else if (Convert.ToDouble(_gift.Rows[0]["Balance"])<=0)
                    {
                        StringNotify = App.Current.FindResource("giftcard_in_used").ToString();
                        OnFocusRequested(nameof(Barcode));
                        _isError = true;
                    }
                    else if(Convert.ToInt32(_gift.Rows[0]["GiftCardID"].ToString()) != ScanGiftId)
                    {
                        if (allReadyExits(StaticClass.GeneralClass.customerGiftCard, _gift.Rows[0]["GiftCardID"].ToString()))
                        {
                            StringNotify = App.Current.FindResource("acard_isnot_multiple").ToString();
                            OnFocusRequested(nameof(Barcode));
                            _isError = true;
                        }
                    }
                    else if(!_isError && StaticClass.GeneralClass.ConverStringToDecimal(UseValue) <= 0)
                    {
                        StringNotify = App.Current.FindResource("enter_amount_want").ToString();
                        OnFocusRequested(nameof(UseValue));
                        _isError = true;
                    }
                    else if (!_isError && Convert.ToDecimal(_gift.Rows[0]["Balance"]) < StaticClass.GeneralClass.ConverStringToDecimal(UseValue))
                    {
                        StringNotify = App.Current.FindResource("value_invalid").ToString();
                        OnFocusRequested(nameof(UseValue));
                        _isError = true;
                    }
                    if (!_isError)
                    {
                        if (Convert.ToInt32(_gift.Rows[0]["GiftCardID"].ToString()) != ScanGiftId)
                        {
                            StaticClass.GeneralClass.customerGiftCard.Remove(ScanGiftId);
                            StaticClass.GeneralClass.customerGiftCard.Add(Convert.ToInt32(_gift.Rows[0]["GiftCardID"].ToString()));
                        }
                        ShellOutViewModel.eventFillValueGiftCard(_gift.Rows[0]["GiftCardID"].ToString() + ":" + UseValue, null);
                        //ShellOutViewModel.eventResetListboxSelected(null, null);
                        OnRequestClose();
                    }
                }
            }
        }
        private void onKeyDownEnterEvent(string param)
        {
            if (!string.IsNullOrEmpty(Barcode) && Barcode.Trim() == "0" && Barcode.Trim() != "")
            {
                onStarScanGiftCadrEvent(null);
            }
            else onLostFocusEvent(null);
        }
        private string _giftBalance;
        public string GiftBalance
        {
            get
            {
                return _giftBalance;
            }
            set
            {
                _giftBalance = value;
                RaisePropertyChanged("GiftBalance");
            }
        }
        private string _giftSerial;
        public string GiftSerial
        {
            get
            {
                return _giftSerial;
            }
            set
            {
                _giftSerial = value;
                RaisePropertyChanged("GiftSerial");
            }
        }
        private string _useValue;
        public string UseValue
        {
            get
            {
                return _useValue;
            }
            set
            {
                _useValue = value;
                RaisePropertyChanged("UseValue");
            }
        }
        private void onLostFocusEvent(object param)
        {
            StringNotify = "";
            if (!string.IsNullOrEmpty(Barcode) && Barcode.Trim().ToString() != "")
            {
                DataTable _gift = (param != null) ? param as DataTable : GiftCardModel.getGiftCardByBarcode(Barcode.Trim());
                if (_gift.Rows.Count == 0)
                {
                    StringNotify = App.Current.FindResource("giftcard_not_existing").ToString();
                    GiftBalance = "";
                    GiftSerial = "";
                    UseValue = "";
                    //OnFocusRequested(nameof(Barcode));
                }
                else
                {
                    Int32 unixTimestamp = getCurrentUnixTime();
                    Int32 _expreTime = Convert.ToInt32(_gift.Rows[0]["ExpirationDate"]);
                    if (unixTimestamp > _expreTime)
                    {
                        StringNotify = App.Current.FindResource("giftcard_expired").ToString();
                        GiftBalance = "";
                        GiftSerial = "";
                        UseValue = "";
                        //OnFocusRequested(nameof(Barcode));
                    }
                    else if (Convert.ToDouble(_gift.Rows[0]["Balance"]) <= 0)
                    {
                        StringNotify = App.Current.FindResource("giftcard_in_used").ToString();
                        GiftBalance = "";
                        GiftSerial = "";
                        UseValue = "";
                        //OnFocusRequested(nameof(Barcode));
                    }
                    else if(Convert.ToInt32(_gift.Rows[0]["GiftCardID"].ToString()) != ScanGiftId)
                    {
                        if (allReadyExits(StaticClass.GeneralClass.customerGiftCard, _gift.Rows[0]["GiftCardID"].ToString()))
                        {
                            StringNotify = App.Current.FindResource("acard_isnot_multiple").ToString();
                            GiftBalance = "";
                            GiftSerial = "";
                            UseValue = "";
                        }
                        else
                        {
                            GiftBalance = _gift.Rows[0]["Balance"].ToString();
                            GiftSerial = _gift.Rows[0]["Serial"].ToString();
                            UseValue = StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(_gift.Rows[0]["Balance"].ToString()));
                        }
                    }
                }
            }
        }
    }
}
