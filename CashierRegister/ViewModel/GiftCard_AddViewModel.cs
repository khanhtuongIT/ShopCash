using CashierRegister.Helpers;
using CashierRegister.Model;
using FirstFloor.ModernUI.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace CashierRegister.ViewModel
{
    public class GiftCard_AddViewModel : ModelBase
    {
        public ObservableCollection<GiftCardModel> GiftCard
        {
            get;
            set;
        }
        static public Int32 getId;
        public RelayCommand formLoadCmd { get; private set; }
        public RelayCommand okGiftCardCmd { get; private set; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        public RelayCommand okSaveGiftCardCmd { get; private set; }
        public RelayCommand formEditEnterKeyDownCmd { get; private set; }
        public RelayCommand formAddEnterKeyDownCmd { get; private set; }
        public RelayCommand sendEmailCloseWindowsCmd { get; private set; }
        public RelayCommand sendGiftCardToCmd { get; private set; }
        public RelayCommand toolGiftCardCmd { get; private set; }
        public RelayCommand toolCloseWindowsCmd { get; private set; }
        public RelayCommand previewPrintPrintCmd { get; private set; }
        public RelayCommand previewPrintOKCmd { get; private set; }

        private List<string> _strCardImg = new List<string>();
        static public List<Int32> printId = new List<Int32>();
        private string _fileAttach = string.Empty;
        private string _orderTime = string.Empty;
        private string _dateFormat_ = string.Empty;

        public GiftCard_AddViewModel()
        {
            GiftCard = new ObservableCollection<GiftCardModel>();
            okGiftCardCmd = new RelayCommand(onOkAddGiftCard);
            Giftcard_Info = new GiftCardModel();
            CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow);
            formLoadCmd = new RelayCommand(OnLoadedEvent);
            okSaveGiftCardCmd = new RelayCommand(onOkSaveGiftCardCmd);
            formEditEnterKeyDownCmd = new RelayCommand(OnFormEditEnterKeyDownEvent);
            formAddEnterKeyDownCmd = new RelayCommand(onFormAddEnterKeyDownEvent);
            sendEmailCloseWindowsCmd = new RelayCommand(onSendEmailCloseWindowsExec);
            sendGiftCardToCmd = new RelayCommand(onSendGiftCardToExec);
            toolGiftCardCmd = new RelayCommand(onToolGiftCardExec);
            toolCloseWindowsCmd = new RelayCommand(onToolCloseWindowsExec);
            previewPrintPrintCmd = new RelayCommand(onPreviewPrintPrintExec);
            previewPrintOKCmd = new RelayCommand(onPreviewPrintOKExec);
            _orderTime = (!string.IsNullOrEmpty(StaticClass.GeneralClass.app_settings["timeFormat"].ToString())) ? " " + StaticClass.GeneralClass.timeFromatSettings[StaticClass.GeneralClass.app_settings["timeFormat"]].ToString() : "";
            _dateFormat_ = StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString() + _orderTime;
        }
        private void OnLoadedEvent(object param)
        {
            DataTable _cardInfo = GiftCardModel.getGiftCard(getId);
            Giftcard_Info.Barcode = _cardInfo.Rows[0]["Barcode"].ToString();
            Giftcard_Info.Serial = _cardInfo.Rows[0]["Serial"].ToString();
            Giftcard_Info.Amount = Convert.ToDouble(_cardInfo.Rows[0]["Amount"].ToString());
            Giftcard_Info.Balance = Convert.ToDouble(_cardInfo.Rows[0]["Amount"].ToString());
            Giftcard_Info.GiftCardID = Convert.ToInt32(getId);
            Giftcard_Info.DeliveredDate = _cardInfo.Rows[0]["DeliveredDate"].ToString();
            DateTime dt = UnixTimeToDateTime(Convert.ToInt32(_cardInfo.Rows[0]["CreateDate"]));
            Giftcard_Info.CreateDate = dt.ToString(_dateFormat_);
            StringAmount = StaticClass.GeneralClass.GetNumFormatDisplay(Convert.ToDecimal(_cardInfo.Rows[0]["Amount"].ToString()));
            
            //Bitmap bitmap = (Bitmap)Bitmap.FromFile(GiftCardModel.GiftCardImgPath + Giftcard_Info.Barcode + ".png", true);
            //StrBarcodeImg = BitmapConversion.BitmapToBitmapSource(bitmap);

            UnitCurrency = StaticClass.GeneralClass.currency_setting_general+" "+ StringAmount;
            DateTime _expreTime = UnixTimeToDateTime(Convert.ToInt32(_cardInfo.Rows[0]["ExpirationDate"]));
            StringExpire = _expreTime.ToString(_dateFormat_);
            TimeSpan span = _expreTime.Subtract(dt);
            ExpireDays = span.Days;
            ExpirationDate = _expreTime.ToString(System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);
            IsHTML = MultiCreateHTML(printId);
            IsEditTick = (Convert.ToInt32(_cardInfo.Rows[0]["DeliveredDate"]) > 0)?true:false;
        }
        private void onOkSaveGiftCardCmd(object param)
        {
            bool error = false;
            this.Notify = "";
            string _amount = param as string;
            string _strGiftAmount = StaticClass.GeneralClass.ConverStringToDecimal(_amount).ToString();
            DateTime _expreTime;
            if (string.IsNullOrEmpty(_strGiftAmount) || _strGiftAmount.Trim().ToString() == "" || Convert.ToDouble(_strGiftAmount) <= 0)
            {
                error = true;
                this.Notify = App.Current.FindResource("amount_of_giftcard_null").ToString();
                OnFocusRequested(nameof(NumberFocus));
            }
            else if (this.CurrentOption == ExprieDate.ToDate && string.IsNullOrEmpty(this.ExpirationDate))
            {
                error = true;
                this.Notify = App.Current.FindResource("select_expire").ToString();
                OnFocusRequested(nameof(ExpirationDate));
            }
            else if (this.CurrentOption == ExprieDate.ToDate && !DateTime.TryParseExact(this.ExpirationDate, System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern, null, System.Globalization.DateTimeStyles.None, out _expreTime))
            {
                error = true;
                this.Notify = App.Current.FindResource("expiredate_invalid").ToString();
                OnFocusRequested(nameof(ExpirationDate));
            }
            else if (this.CurrentOption == ExprieDate.ToDate && ConvertToUnixTime(Convert.ToDateTime(this.ExpirationDate)) <= ConvertToUnixTime(Convert.ToDateTime(Giftcard_Info.CreateDate)))
            {
                error = true;
                this.Notify = App.Current.FindResource("expiredate_lessthan").ToString();
                OnFocusRequested(nameof(ExpirationDate));
            }
            else if (this.CurrentOption == ExprieDate.AfterDay && ExpireDays <= 0)
            {
                error = true;
                this.Notify = App.Current.FindResource("numof_day").ToString();
                OnFocusRequested(nameof(ExpireDays));
            }
            if (!error)
            {
                if (this.CurrentOption == ExprieDate.AfterDay)
                {
                    DateTime oDate = Convert.ToDateTime(Giftcard_Info.CreateDate).AddDays(ExpireDays);
                    _expreTime = Convert.ToDateTime(oDate.ToString(System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern) + " 23:59:59");
                }
                else
                {
                    _expreTime = Convert.ToDateTime(this.ExpirationDate + " 23:59:59");
                }
                Int32 _intTime = ConvertToUnixTime(_expreTime);
                Giftcard_Info.ExpirationDate = _intTime.ToString();
                Giftcard_Info.Amount = Convert.ToDouble(_strGiftAmount);
                if (GiftCardModel.updateGiftCard(Giftcard_Info) > 0)
                {
                    GiftCardViewModel.eventAddGiftCard(null, null);
                    if (!IsEditTick)
                    {
                        List<Int32> _setId = new List<Int32>();
                        _setId.Add(Giftcard_Info.GiftCardID);
                        GiftCardModel.setDeliveredGiftcard(_setId, 0);
                    }
                    else if (Convert.ToInt32(Giftcard_Info.DeliveredDate) == 0)
                    {
                        List<Int32> _setId = new List<Int32>();
                        _setId.Add(Giftcard_Info.GiftCardID);
                        GiftCardModel.setDeliveredGiftcard(_setId, getCurrentUnixTime());
                    }
                    GiftCardViewModel.eventAddGiftCard(null, null);
                    OnRequestClose();
                }
                else
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.FindResource("close").ToString();
                    md.Content = App.Current.FindResource("cannot_update").ToString().Replace("$", App.Current.FindResource("gift_card").ToString()); //"Can not update gift card, try again !";
                    md.Title = App.Current.FindResource("notification").ToString();
                    md.ShowDialog();
                }
            }
        }
        private int _comboSelectedIndex = 0;
        public int ComboSelectedIndex
        {
            get
            {
                return _comboSelectedIndex;
            }
            set
            {
                if (_comboSelectedIndex != value)
                {
                    _comboSelectedIndex = value;
                    RaisePropertyChanged("ComboSelectedIndex");
                }
            }
        }
        Random rand = new Random();
        private const string Number = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";
        private bool allReadyExits(List<string> lst, string val)
        {
            foreach (var intVal in lst)
            {
                if (intVal == val)
                    return true;
            }
            return false;
        }
        private string GenerateString(int size)
        {
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Number[rand.Next(Number.Length)];
            }
            return new string(chars);
        }
        private async void onOkAddGiftCard(object param)
        {
            Notify = "";
            bool error = false;
            GiftCardModel _gift = (GiftCardModel)param;
            string _strGiftAmount = StringAmount.Replace(",", ".");
            DateTime _expreTime;
            Int32 unixTimestamp = getCurrentUnixTime();
            DateTime _dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToInt32(unixTimestamp)).ToLocalTime();
            List<string> _lstIn = new List<string>();
            if (Convert.ToString(this.NumOfGiftCard).Trim().ToString() == "" || Convert.ToInt32(this.NumOfGiftCard) <= 0)
            {
                error = true;
                this.Notify = App.Current.FindResource("numof_giftcard_null").ToString();
                OnFocusRequested(nameof(NumOfGiftCard));
            }
            else if (string.IsNullOrEmpty(_strGiftAmount) || Convert.ToDouble(_strGiftAmount) <= 0)
            {
                error = true;
                this.Notify = App.Current.FindResource("amount_of_giftcard_null").ToString();
                OnFocusRequested(nameof(NumberFocus));
            }
            else if (this.CurrentOption == ExprieDate.ToDate && string.IsNullOrEmpty(this.ExpirationDate))
            {
                error = true;
                this.Notify = App.Current.FindResource("select_expire").ToString();
                OnFocusRequested(nameof(ExpirationDate));
            }
            else if (this.CurrentOption == ExprieDate.ToDate && !DateTime.TryParseExact(this.ExpirationDate, System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern, null, System.Globalization.DateTimeStyles.None, out _expreTime))
            {
                error = true;
                this.Notify = App.Current.FindResource("expiredate_invalid").ToString();
                OnFocusRequested(nameof(ExpirationDate));
            }
            else if (this.CurrentOption == ExprieDate.ToDate && ConvertToUnixTime(Convert.ToDateTime(this.ExpirationDate)) <= ConvertToUnixTime(Convert.ToDateTime(_dt.ToString(System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern))))
            {
                error = true;
                this.Notify = App.Current.FindResource("expiredate_lessthan").ToString();
                OnFocusRequested(nameof(ExpirationDate));
            }
            else if (this.CurrentOption == ExprieDate.AfterDay && ExpireDays <= 0) {
                error = true;
                this.Notify = App.Current.FindResource("numof_day").ToString();
                OnFocusRequested(nameof(ExpireDays));
            }
            if (!error)
            {
                EnableProgress = "Visible";
                IsVisible = "Collapsed";
                _gift.Amount = Convert.ToDouble(_strGiftAmount);
                var slowTask = Task<string>.Factory.StartNew(() => _insertCards(_gift, _lstIn, _dt, unixTimestamp));
                await slowTask;
                if (slowTask.Result.ToString() == "Success")
                {
                    EnableProgress = "Collapsed";
                    IsVisible = "Visible";
                    GiftCardViewModel.eventAddGiftCard("add", null);
                    OnRequestClose();
                }
                else
                {
                    EnableProgress = "Collapsed";
                    IsVisible = "Visible";
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.FindResource("close").ToString();
                    md.Content = slowTask.Result.ToString();
                    md.Title = App.Current.FindResource("notification").ToString();
                    md.ShowDialog();
                }
            }
        }
        private string _insertCards(GiftCardModel _gift, List<string> _lstIn, DateTime _dt, Int32 unixTimestamp)
        {
            DateTime _expreTime;
            string _result = string.Empty;
            string _CountryCode = "23";
            string _ManufacturerCode = GenerateString(5);
            if (this.CurrentOption == ExprieDate.AfterDay)
            {
                DateTime oDate = _dt.AddDays(ExpireDays);
                _expreTime = Convert.ToDateTime(oDate.ToString(System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern) + " 23:59:59");
            }
            else
            {
                _expreTime = Convert.ToDateTime(this.ExpirationDate + " 23:59:59");
            }
            Int32 _intTime = ConvertToUnixTime(_expreTime);
            string _ProductCode = "";
            string strBarcode = "";
            if (Convert.ToInt32(this.NumOfGiftCard) <= 500)
            {
                for (int i = 0; i < Convert.ToInt32(this.NumOfGiftCard); i++)
                {
                    do
                    {
                        _ProductCode = GenerateString(5);
                        Ean13Barcode barcode = new Ean13Barcode(_CountryCode, _ManufacturerCode, _ProductCode);
                        strBarcode = _CountryCode + _ManufacturerCode + _ProductCode + barcode.ChecksumDigit;
                    }
                    while (GiftCardModel.getGiftCardByBarcode(strBarcode).Rows.Count > 0 || allReadyExits(_lstIn, strBarcode));
                    _lstIn.Add(strBarcode);
                    GiftCard.Add(new GiftCardModel { Barcode = strBarcode, Serial = GenerateString(13), CreateDate = unixTimestamp.ToString(), ExpirationDate = _intTime.ToString(), CustomerIDUse = 0, Amount = _gift.Amount, Balance = _gift.Amount });
                }
                if (GiftCardModel.insertGiftCard(GiftCard) > 0)
                {
                    _result = "Success";
                }
                else
                {
                    _result = App.Current.FindResource("cannot_add").ToString().Replace("$", App.Current.FindResource("gift_card").ToString());
                }
            }
            else
            {
                for (int i = 0; i < 500; i++)
                {
                    do
                    {
                        _ProductCode = GenerateString(5);
                        Ean13Barcode barcode = new Ean13Barcode(_CountryCode, _ManufacturerCode, _ProductCode);
                        strBarcode = _CountryCode + _ManufacturerCode + _ProductCode + barcode.ChecksumDigit;
                    }
                    while (GiftCardModel.getGiftCardByBarcode(strBarcode).Rows.Count > 0 || allReadyExits(_lstIn, strBarcode));
                    _lstIn.Add(strBarcode);
                    GiftCard.Add(new GiftCardModel { Barcode = strBarcode, Serial = GenerateString(13), CreateDate = unixTimestamp.ToString(), ExpirationDate = _intTime.ToString(), CustomerIDUse = 0, Amount = _gift.Amount, Balance = _gift.Amount });
                }
                if (GiftCardModel.insertGiftCard(GiftCard) > 0)
                {
                    _result = "Success";
                }
                else
                {
                    _result = App.Current.FindResource("cannot_add").ToString().Replace("$", "500 " + App.Current.FindResource("gift_card").ToString());
                }
                GiftCard.Clear();
                if(_result == "Success")
                {
                    for (int i = 0; i < Convert.ToInt32(this.NumOfGiftCard) - 500; i++)
                    {
                        do
                        {
                            _ProductCode = GenerateString(5);
                            Ean13Barcode barcode = new Ean13Barcode(_CountryCode, _ManufacturerCode, _ProductCode);
                            strBarcode = _CountryCode + _ManufacturerCode + _ProductCode + barcode.ChecksumDigit;
                        }
                        while (GiftCardModel.getGiftCardByBarcode(strBarcode).Rows.Count > 0 || allReadyExits(_lstIn, strBarcode));
                        _lstIn.Add(strBarcode);
                        GiftCard.Add(new GiftCardModel { Barcode = strBarcode, Serial = GenerateString(13), CreateDate = unixTimestamp.ToString(), ExpirationDate = _intTime.ToString(), CustomerIDUse = 0, Amount = _gift.Amount, Balance = _gift.Amount });
                    }
                    if (GiftCardModel.insertGiftCard(GiftCard) > 0)
                    {
                        _result = "Success";
                    }
                    else
                    {
                        _result = App.Current.FindResource("cannot_add").ToString().Replace("$", Convert.ToInt32(this.NumOfGiftCard) - 500 + " " + App.Current.FindResource("gift_card").ToString());
                    }
                }
            }
            return _result;
        }
        //GiftCardModel Giftcard_MD;
        private GiftCardModel _Giftcard_Info;
        public GiftCardModel Giftcard_Info
        {
            get { return _Giftcard_Info; }
            set
            {
                _Giftcard_Info = value;
                RaisePropertyChanged("Giftcard_Info");
            }
        }
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        private string _selectedExpiration;
        public String SelectedExpiration
        {
            get
            {
                return _selectedExpiration;
            }
            set
            {
                if (_selectedExpiration != value)
                {
                    _selectedExpiration = value;
                    RaisePropertyChanged("SelectedExpiration");
                }
            }
        }
        private string _numofGiftCard = "0";
        public string NumOfGiftCard
        {
            get
            {
                return _numofGiftCard;
            }
            set
            {
                if (_numofGiftCard != value)
                {
                    _numofGiftCard = value;
                    RaisePropertyChanged("NumOfGiftCard");
                }
            }
        }
        private ExprieDate _options = ExprieDate.AfterDay;
        public ExprieDate CurrentOption
        {
            get
            {
                return _options;
            }
            set
            {
                if (_options != value)
                {
                    _options = value;
                    RaisePropertyChanged("CurrentOption");
                }
                if(_options== ExprieDate.AfterDay)
                {
                    IsEnableDay = true;
                    IsEnableToDate = false;
                }
                else
                {
                    IsEnableDay = false;
                    IsEnableToDate = true;
                }
            }
        }
        private string _strNotify = "";
        public string Notify
        {
            get
            {
                return _strNotify;
            }
            set
            {
                if (_strNotify != value)
                {
                    _strNotify = value;
                    RaisePropertyChanged("Notify");
                }
            }
        }
        private bool _amountFocus = false;
        public bool AmountFocus
        {
            get
            {
                return _amountFocus;
            }
            set
            {
                if (_amountFocus != value)
                {
                    _amountFocus = value;
                    RaisePropertyChanged("AmountFocus");
                }
            }
        }
        private bool _numberFocus = true;
        public bool NumberFocus
        {
            get
            {
                return _numberFocus;
            }
            set
            {
                if (_numberFocus != value)
                {
                    _numberFocus = value;
                    RaisePropertyChanged("NumberFocus");
                }
            }
        }
        private void OnFormEditEnterKeyDownEvent(object param)
        {
            onOkSaveGiftCardCmd(param);
        }
        private string _strAmount= StaticClass.GeneralClass.GetNumFormatDisplay(0);
        public string StringAmount
        {
            get
            {
                return _strAmount;
            }
            set
            {
                _strAmount = value;
                RaisePropertyChanged("StringAmount");
            }
        }
        private void onFormAddEnterKeyDownEvent(object param)
        {
            onOkAddGiftCard(param);
        }
        private BitmapSource _strBarcodeImg;
        public BitmapSource StrBarcodeImg
        {
            get
            {
                return _strBarcodeImg;
            }
            set
            {
                _strBarcodeImg = value;
                RaisePropertyChanged("StrBarcodeImg");
            }
        }
        private string _unitCurrency;
        public string UnitCurrency
        {
            get
            {
                return _unitCurrency;
            }
            set
            {
                _unitCurrency = value;
                RaisePropertyChanged("UnitCurrency");
            }
        }
        private string _strExprire;
        public string StringExpire
        {
            get
            {
                return _strExprire;
            }
            set
            {
                _strExprire = value;
                RaisePropertyChanged("StringExpire");
            }
        }
        private Int32 _expiredays;
        public Int32 ExpireDays
        {
            get
            {
                return _expiredays;
            }
            set
            {
                _expiredays = value;
                RaisePropertyChanged("ExpireDays");
            }
        }
        private string _expirationDate;
        public string ExpirationDate
        {
            get
            {
                return _expirationDate;
            }
            set
            {
                _expirationDate = value;
                RaisePropertyChanged("ExpirationDate");
            }
        }
        private bool _isEnableDay = true;
        public bool IsEnableDay
        {
            get { return _isEnableDay; }
            set
            {
                _isEnableDay = value;
                RaisePropertyChanged("IsEnableDay");
            }
        }
        private bool _isEnableToDate = false;
        public bool IsEnableToDate
        {
            get { return _isEnableToDate; }
            set
            {
                _isEnableToDate = value;
                RaisePropertyChanged("IsEnableToDate");
            }
        }
        private string _showCurency = StaticClass.GeneralClass.currency_setting_general;
        public string ShowCurency
        {
            get { return _showCurency; }
        }

        /*Code for send email*/
        private string _strFromEmail = StaticClass.GeneralClass.app_settings["emailAcc"].ToString();
        public string FromEmail
        {
            get { return _strFromEmail; }
            set
            {
                _strFromEmail = value;
                RaisePropertyChanged("FromEmail");
            }
        }
        private string _strToEmail = "";
        public string ToEmail
        {
            get { return _strToEmail; }
            set
            {
                _strToEmail = value;
                RaisePropertyChanged("ToEmail");
            }
        }
        private string _strPassword = StaticClass.GeneralClass.app_settings["emailPass"].ToString();
        public string FromPassword
        {
            get { return _strPassword; }
            set
            {
                _strPassword = value;
                RaisePropertyChanged("FromPassword");
            }
        }
        private string _strSubject = App.Current.FindResource("new_message_from").ToString() + " " + StaticClass.GeneralClass.app_settings["storeName"].ToString();
        public string SubjectEmail
        {
            get { return _strSubject; }
            set
            {
                _strSubject = value;
                RaisePropertyChanged("SubjectEmail");
            }
        }
        private string _strContent;
        public string BodyEmail
        {
            get { return _strContent; }
            set
            {
                _strContent = value;
                RaisePropertyChanged("BodyEmail");
            }
        }
        private static FlowDocument SetRTF(string xamlString)
        {
            if (string.IsNullOrEmpty(xamlString)) return null;
            StringReader stringReader = new StringReader(xamlString);
            System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stringReader);
            FlowDocument doc = (FlowDocument)XamlReader.Load(xmlReader);
            return doc;
        }
        private void onSendEmailCloseWindowsExec(object _param)
        {
            IsTick = true;
            OnRequestClose();
        }
        private async void onSendGiftCardToExec(object _param)
        {
            Notify = "";
            if (string.IsNullOrEmpty(FromEmail) || FromEmail.Trim()=="")
            {
                Notify = App.Current.FindResource("from_null").ToString();
                FromEmail = "";
                OnFocusRequested(nameof(FromEmail));
            }
            else if (string.IsNullOrEmpty(FromPassword) || FromPassword.Trim()=="")
            {
                Notify = App.Current.FindResource("password_null").ToString();
                FromPassword = "";
                OnFocusRequested(nameof(FromPassword));
            }
            else if (string.IsNullOrEmpty(ToEmail) || ToEmail.Trim()=="")
            {
                Notify = App.Current.FindResource("to_null").ToString();
                ToEmail = "";
                OnFocusRequested(nameof(ToEmail));
            }
            else if (ToEmail.Trim().ToString()== "None")
            {
                Notify = App.Current.FindResource("no_recipient").ToString();
                ToEmail = "";
                OnFocusRequested(nameof(ToEmail));
            }
            else
            {
                EnableProgress = "Visible";
                IsVisible = "Collapsed";
                string _bodyEnail = "";
                if (!string.IsNullOrEmpty(BodyEmail))
                {
                    FlowDocument _flowDoc = SetRTF(BodyEmail);
                    TextRange text_range = new TextRange(_flowDoc.ContentStart, _flowDoc.ContentEnd);
                    _bodyEnail += "<font>" + text_range.Text.Replace("\n", "<br />") + "</font><br /><br />";
                }
                var slowTask = Task<string>.Factory.StartNew(() => SendEmailWithPDF(_bodyEnail, "smtp.gmail.com", FromEmail.Trim(), FromPassword.Trim(), ToEmail.Trim()));
                await slowTask;
                if (slowTask.Result.ToString() == "Success")
                {
                    EnableProgress = "Collapsed";
                    IsVisible = "Visible";
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.FindResource("close").ToString();
                    md.Content = App.Current.FindResource("message_sent").ToString();
                    md.Title = App.Current.FindResource("notification").ToString();
                    md.ShowDialog();
                    GiftCardViewModel.eventAddGiftCard(null, null);
                    IsTick = true;
                    OnRequestClose();
                }
                else
                {
                    EnableProgress = "Collapsed";
                    IsVisible = "Visible";
                    Notify = slowTask.Result.ToString();
                }
            }
        }
        public Bitmap CombineBitmap(DataRow _param)
        {
            //read all images into memory
            List<Bitmap> images = new List<Bitmap>();
            Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                string _strCode = _param["Barcode"].ToString();
                Ean13Barcode _code = new Ean13Barcode(_strCode.Substring(0, 2), _strCode.Substring(2, 5), _strCode.Substring(7, 5), _strCode.Substring(12, 1));
                Bitmap _giftBitmap;
                _giftBitmap = _code.CreateBitmap();
                width += _giftBitmap.Width;
                height = _giftBitmap.Height > height ? _giftBitmap.Height : height;
                images.Add(_giftBitmap);

                //create a bitmap to hold the combined image
                System.Drawing.Font font = new System.Drawing.Font("Arial", 11);

                System.Drawing.Image img = new Bitmap(1, 1);
                Graphics drawing = Graphics.FromImage(img);

                string _rs = string.Empty;
                int _kk_ = 1;
                string _valName = App.Current.FindResource("single_store").ToString() + ": " + StaticClass.GeneralClass.app_settings["storeName"].ToString();
                string[] _newName = _valName.Split(' ');
                _valName = string.Empty;
                foreach (string w in _newName)
                {
                    _rs += string.IsNullOrEmpty(_rs) ? w : " " + w;
                    SizeF textSize = drawing.MeasureString(_rs, font);
                    if (textSize.Width >= width)
                    {
                        _kk_++;
                        _rs = string.Empty;
                        _valName += "\n" + w + " ";
                    }
                    else
                    {
                        _valName += w + " ";
                    }
                }

                _rs = string.Empty;
                _kk_++;
                string _valAddress = App.Current.FindResource("address").ToString() + ": " + StaticClass.GeneralClass.app_settings["storeAddress"].ToString();
                string[] _newAddress = _valAddress.Split(' ');
                _valAddress = string.Empty;
                foreach (string w in _newAddress)
                {
                    _rs += string.IsNullOrEmpty(_rs) ? w : " " + w;
                    SizeF textSize = drawing.MeasureString(_rs, font);
                    if (textSize.Width >= width)
                    {
                        _kk_++;
                        _rs = string.Empty;
                        _valAddress += "\n" + w + " ";
                    }
                    else
                    {
                        _valAddress += w + " ";
                    }
                }

                _rs = string.Empty;
                _kk_++;
                string _valPhone = App.Current.FindResource("single_phone").ToString() + ": " + StaticClass.GeneralClass.app_settings["storePhone"].ToString();
                string[] _newPhone = _valPhone.Split(' ');
                _valPhone = string.Empty;
                foreach (string w in _newPhone)
                {
                    _rs += string.IsNullOrEmpty(_rs) ? w : " " + w;
                    SizeF textSize = drawing.MeasureString(_rs, font);
                    if (textSize.Width >= width)
                    {
                        _kk_++;
                        _rs = string.Empty;
                        _valPhone += "\n" + w + " ";
                    }
                    else
                    {
                        _valPhone += w + " ";
                    }
                }

                DateTime _expreTime = UnixTimeToDateTime(Convert.ToInt32(_param["ExpirationDate"]));
                string _valInfo = App.Current.FindResource("serial_number").ToString() + ": " + _param["Serial"].ToString() + "\n" + App.Current.FindResource("expiration").ToString() + ": " + _expreTime.ToString(_dateFormat_) + "\n" + App.Current.FindResource("amount").ToString() + ": " + StaticClass.GeneralClass.currency_setting_general + " " + _param["Balance"].ToString() + "\n";

                finalImage = new Bitmap(width, height + (_kk_ * 20) + 70);

                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.White);

                    g.DrawString(App.Current.FindResource("gift_card").ToString() + "\n", font, new SolidBrush(Color.Black), new PointF(width - 70, 0));
                    g.DrawString(_valName + "\n" + _valAddress + "\n" + _valPhone, font, new SolidBrush(Color.Black), new PointF(0, 20));

                    //go through each image and draw it on the final image
                    int offset = 0;
                    int _off = 0;
                    foreach (Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new System.Drawing.Rectangle(offset, (_kk_ * 20) + 12, image.Width, image.Height));
                        offset += image.Width;
                        _off += image.Height;
                    }
                    g.DrawString(_valInfo, font, new SolidBrush(Color.Black), new PointF(0, (_kk_ * 20) + _off + 12));
                }

                return finalImage;
            }
            catch (Exception)
            {
                if (finalImage != null)
                    finalImage.Dispose();
                //throw ex;
                throw;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }
        private string SendEmailWithPDF(string mail_body, string smtp_host, string sender, string password, string recipent)
        {
            Byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document(PageSize.A4))
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();

                        PdfPTable table = new PdfPTable(2);
                        DataTable _dtGift = GiftCardModel.getGiftCardById(printId);
                        int _totalRow = _dtGift.Rows.Count;
                        if (_totalRow > 0)
                        {
                            int j = 1;
                            foreach (DataRow dr in _dtGift.Rows)
                            {
                                PdfPCell cell1 = new PdfPCell();
                                Bitmap _giftBitmap = CombineBitmap(dr);
                                byte[] imgBytes = turnImageToByteArray(_giftBitmap);
                                iTextSharp.text.Image _imgBarcode = iTextSharp.text.Image.GetInstance(imgBytes);
                                cell1.AddElement(_imgBarcode);
                                table.AddCell(cell1);
                                j++;
                            }
                            if (j % 2 == 0)
                            {
                                PdfPCell cell2 = new PdfPCell();
                                table.AddCell(cell2);
                            }
                        }
                        doc.Add(table);
                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
                ms.Close();
                MailMessage mail_message = null;
                try
                {
                    mail_message = new MailMessage(sender, recipent);
                    mail_message.IsBodyHtml = true;
                    mail_message.Subject = (!string.IsNullOrEmpty(SubjectEmail)) ? SubjectEmail.Trim() : App.Current.FindResource("new_message_from").ToString() + " " + StaticClass.GeneralClass.app_settings["storeName"].ToString();
                    mail_message.Body = mail_body;
                    mail_message.Attachments.Add(new Attachment(new MemoryStream(bytes), "Gift_cards_" + getCurrentUnixTime().ToString() + ".pdf", System.Net.Mime.MediaTypeNames.Application.Pdf));

                    SmtpClient smtpclient = new SmtpClient();
                    smtpclient.Host = smtp_host;
                    smtpclient.Port = 587;
                    smtpclient.Timeout = 30000;
                    smtpclient.EnableSsl = true;
                    smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpclient.UseDefaultCredentials = false;

                    smtpclient.Credentials = new System.Net.NetworkCredential(sender, password);
                    smtpclient.Send(mail_message);
                    if (mail_message != null)
                        mail_message.Dispose();
                    if(IsTick)
                        GiftCardModel.setDeliveredGiftcard(printId, getCurrentUnixTime());
                    printId.Clear();
                    _strCardImg.Clear();
                    ms.Dispose();
                    return "Success";
                }
                catch (FormatException)
                {
                    return App.Current.FindResource("email_malformed").ToString();
                    ms.Dispose();
                }
                catch (SmtpException)
                {
                    return App.Current.FindResource("giftcard_notsend_email").ToString();
                    ms.Dispose();
                }
            }
        }
        private string _enableProgress = "Collapsed";
        public string EnableProgress
        {
            get { return _enableProgress; }
            set
            {
                _enableProgress = value;
                RaisePropertyChanged("EnableProgress");
            }
        }
        private string _enableButton = "Visible";
        public string IsVisible
        {
            get { return _enableButton; }
            set
            {
                _enableButton = value;
                RaisePropertyChanged("IsVisible");
            }
        }
        private byte[] turnImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        private string MultiCreateHTML(List<Int32> _lstId)
        {
            if (_lstId.Count == 0) return string.Empty;
            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
            htmlBuilder.Append("<head>");
            htmlBuilder.Append("<meta http-equiv='Content - Type' content='text / html; charset = utf - 8' />");
            htmlBuilder.Append("<title>" + App.Current.FindResource("gift_card").ToString() + "</title>");
            htmlBuilder.Append("</head>");
            htmlBuilder.Append("<body>");
            htmlBuilder.Append("<div style='width:100%; margin: 0px'>");
            htmlBuilder.Append("<table style='width:100%' border=\"1\"> ");
            htmlBuilder.Append("<tr>");
            DataTable _dtGift = GiftCardModel.getGiftCardById(_lstId);
            int _totalRow = _dtGift.Rows.Count;
            if (_totalRow <= 0)
            {
                htmlBuilder.Append("<div>" + App.Current.FindResource("cannot_found_data").ToString() + " </div>");
            }
            else
            {
                int j = 1;
                foreach (DataRow dr in _dtGift.Rows)
                {
                    htmlBuilder.Append("<td width=\"50%\" style=\"padding:5px;\">");
                    Bitmap _giftBitmap = CombineBitmap(dr);
                    byte[] imgBytes = turnImageToByteArray(_giftBitmap);
                    string imgString = Convert.ToBase64String(imgBytes);
                    htmlBuilder.Append("<img src=\"" + String.Format("data:image/Png;base64,{0}", imgString) + "\" border=\"0\" style=\"margin-top:3px\" width=\"320\" Height=\"280\"/>");
                    _giftBitmap.Dispose();
                    if (j % 2 == 0 && j < _totalRow)
                    {
                        htmlBuilder.Append("</tr><tr>");
                    }
                    j++;
                }
            }
            htmlBuilder.Append("</tr>");
            htmlBuilder.Append("</table>");
            htmlBuilder.Append("</div>");
            htmlBuilder.Append("</body>");
            htmlBuilder.Append("</html>");
            return htmlBuilder.ToString();
        }
        /*********************/

        /*Code for Tool*/
        private toolCreated _toolCreated = toolCreated.None;
        public toolCreated ToolCreated
        {
            get { return _toolCreated; }
            set
            {
                _toolCreated = value;
                RaisePropertyChanged("ToolCreated");
                setEnableCreated(_toolCreated);
            }
        }
        private bool _genaralExpired = false;
        public bool GenaralExpired
        {
            get { return _genaralExpired; }
            set
            {
                _genaralExpired = value;
                RaisePropertyChanged("GenaralExpired");
            }
        }
        private bool _genaralUsed = false;
        public bool GenaralUsed
        {
            get { return _genaralUsed; }
            set
            {
                _genaralUsed = value;
                RaisePropertyChanged("GenaralUsed");
            }
        }
        private string _toolCreatedOn = "";
        public string ToolCreatedOn
        {
            get { return _toolCreatedOn; }
            set
            {
                _toolCreatedOn = value;
                RaisePropertyChanged("ToolCreatedOn");
            }
        }
        private string _toolCreatedAfter = "";
        public string ToolCreatedAfter
        {
            get { return _toolCreatedAfter; }
            set
            {
                _toolCreatedAfter = value;
                RaisePropertyChanged("ToolCreatedAfter");
            }
        }
        private string _toolCreatedBefore = "";
        public string ToolCreatedBefore
        {
            get { return _toolCreatedBefore; }
            set
            {
                _toolCreatedBefore = value;
                RaisePropertyChanged("ToolCreatedBefore");
            }
        }
        private string _toolCreatedStart = "";
        public string ToolCreatedStart
        {
            get { return _toolCreatedStart; }
            set
            {
                _toolCreatedStart = value;
                RaisePropertyChanged("ToolCreatedStart");
            }
        }
        private string _toolCreatedEnd = "";
        public string ToolCreatedEnd
        {
            get { return _toolCreatedEnd; }
            set
            {
                _toolCreatedEnd = value;
                RaisePropertyChanged("ToolCreatedEnd");
            }
        }
        private bool _enableOn = false;
        public bool EnableOn
        {
            get { return _enableOn; }
            set
            {
                _enableOn = value;
                RaisePropertyChanged("EnableOn");
            }
        }
        private bool _enableBefore = false;
        public bool EnableBefore
        {
            get { return _enableBefore; }
            set
            {
                _enableBefore = value;
                RaisePropertyChanged("EnableBefore");
            }
        }
        private bool _enableAfter = false;
        public bool EnableAfter
        {
            get { return _enableAfter; }
            set
            {
                _enableAfter = value;
                RaisePropertyChanged("EnableAfter");
            }
        }
        private bool _enableBetween = false;
        public bool EnableBetween
        {
            get { return _enableBetween; }
            set
            {
                _enableBetween = value;
                RaisePropertyChanged("EnableBetween");
            }
        }
        public void setEnableCreated(toolCreated _opts)
        {
            if (_opts == toolCreated.None)
            {
                EnableOn = false;
                EnableBefore = false;
                EnableAfter = false;
                EnableBetween = false;
            }
            if (_opts == toolCreated.On)
            {
                EnableOn = true;
                EnableBefore = false;
                EnableAfter = false;
                EnableBetween = false;
            }
            if (_opts == toolCreated.Before)
            {
                EnableOn = false;
                EnableBefore = true;
                EnableAfter = false;
                EnableBetween = false;
            }
            if (_opts == toolCreated.After)
            {
                EnableOn = false;
                EnableBefore = false;
                EnableAfter = true;
                EnableBetween = false;
            }
            if (_opts == toolCreated.Between)
            {
                EnableOn = false;
                EnableBefore = false;
                EnableAfter = false;
                EnableBetween = true;
            }
        }
        private void onToolGiftCardExec(object _param)
        {
            Notify = "";
            bool _isError = false;
            Int32 unixTimestamp = getCurrentUnixTime();
            toolCreated _opts = ToolCreated;
            if (_opts == toolCreated.On)
            {
                if (string.IsNullOrEmpty(this.ToolCreatedOn))
                {
                    Notify = App.Current.FindResource("tool_select_on").ToString();
                    _isError = true;
                    OnFocusRequested(nameof(ToolCreatedOn));
                }
                else if(ConvertToUnixTime(Convert.ToDateTime(this.ToolCreatedOn)) > unixTimestamp)
                {
                    Notify = App.Current.FindResource("tool_selecton_invalid").ToString();
                    _isError = true;
                    OnFocusRequested(nameof(ToolCreatedOn));
                }
            }
            if (_opts == toolCreated.Before)
            {
                if (string.IsNullOrEmpty(this.ToolCreatedBefore))
                {
                    Notify = App.Current.FindResource("tool_select_before").ToString();
                    _isError = true;
                    OnFocusRequested(nameof(ToolCreatedBefore));
                }
                else if (ConvertToUnixTime(Convert.ToDateTime(this.ToolCreatedBefore)) > unixTimestamp)
                {
                    Notify = App.Current.FindResource("tool_selectbefore_invalid").ToString();
                    _isError = true;
                    OnFocusRequested(nameof(ToolCreatedBefore));
                }
            }
            if (_opts == toolCreated.After)
            {
                if (string.IsNullOrEmpty(this.ToolCreatedAfter))
                {
                    Notify = App.Current.FindResource("tool_select_after").ToString();
                    _isError = true;
                    OnFocusRequested(nameof(ToolCreatedAfter));
                }
                else if (ConvertToUnixTime(Convert.ToDateTime(this.ToolCreatedAfter)) > unixTimestamp)
                {
                    Notify = App.Current.FindResource("tool_selectafter_invalid").ToString();
                    _isError = true;
                    OnFocusRequested(nameof(ToolCreatedAfter));
                }
            }
            if (_opts == toolCreated.Between)
            {
                if(string.IsNullOrEmpty(this.ToolCreatedStart))
                {
                    Notify = App.Current.FindResource("tool_select_start").ToString();
                    _isError = true;
                    OnFocusRequested(nameof(ToolCreatedStart));
                }
                else if(ConvertToUnixTime(Convert.ToDateTime(this.ToolCreatedStart)) > unixTimestamp)
                {
                    Notify = App.Current.FindResource("tool_selectstart_invalid").ToString();
                    _isError = true;
                    OnFocusRequested(nameof(ToolCreatedStart));
                }
                else if (string.IsNullOrEmpty(this.ToolCreatedEnd))
                {
                    Notify = App.Current.FindResource("tool_select_end").ToString(); 
                     _isError = true;
                    OnFocusRequested(nameof(ToolCreatedEnd));
                }
                else if (ConvertToUnixTime(Convert.ToDateTime(this.ToolCreatedEnd)) < ConvertToUnixTime(Convert.ToDateTime(this.ToolCreatedStart)))
                {
                    Notify = App.Current.FindResource("tool_selectend_invalid").ToString();
                    _isError = true;
                    OnFocusRequested(nameof(ToolCreatedEnd));
                }
            }
            if (!_isError)
            {
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
                    string _strGenaral = string.Empty;
                    string _strCreated = string.Empty;
                    string _strError = string.Empty;
                    string _strWhere = string.Empty;
                    if (this.GenaralExpired)
                    {
                        DateTime _expreTime;
                        DateTime oDate = DateTime.Now;
                        _expreTime = Convert.ToDateTime(oDate.ToString(System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern) + " 23:59:59");
                        _strWhere = "ExpirationDate <= " + ConvertToUnixTime(_expreTime);
                        if (!string.IsNullOrEmpty(_strWhere) && GiftCardModel.delGiftcardByOpstions(_strWhere) <= 0)
                        {
                            _strError = App.Current.FindResource("tool_cannot_del_expired").ToString()+ @"\n";
                        }
                    }
                    if(string.IsNullOrEmpty(_strError) && this.GenaralUsed)
                    {
                        _strWhere = "Balance <= 0";
                        if (!string.IsNullOrEmpty(_strWhere) && GiftCardModel.delGiftcardByOpstions(_strWhere) <= 0)
                        {
                            _strError = App.Current.FindResource("tool_cannot_del_used").ToString() + @"\n";
                        }
                    }
                    if (string.IsNullOrEmpty(_strError) && this.GeneralDeliver)
                    {
                        _strWhere = "DeliveredDate > 0";
                        if (!string.IsNullOrEmpty(_strWhere) && GiftCardModel.delGiftcardByOpstions(_strWhere) <= 0)
                        {
                            _strError = App.Current.FindResource("tool_cannot_deliver").ToString() + @"\n";
                        }
                    }
                    if (string.IsNullOrEmpty(_strError) && _opts != toolCreated.None)
                    {
                        DateTime _enddate;
                        DateTime _startdate;
                        switch (_opts)
                        {
                            case toolCreated.On:
                                _enddate = Convert.ToDateTime(this.ToolCreatedOn + " 23:59:59");
                                _startdate = Convert.ToDateTime(this.ToolCreatedOn + " 00:00:00");
                                _strWhere = "CreateDate between " + ConvertToUnixTime(_startdate) + " and " + ConvertToUnixTime(_enddate);
                                break;
                            case toolCreated.Before:
                                _enddate = Convert.ToDateTime(this.ToolCreatedBefore + " 00:00:00");
                                _strWhere = "CreateDate < " + ConvertToUnixTime(_enddate);
                                break;
                            case toolCreated.After:
                                _enddate = Convert.ToDateTime(this.ToolCreatedAfter + " 23:59:59");
                                _strWhere = "CreateDate > " + ConvertToUnixTime(_enddate);
                                break;
                            case toolCreated.Between:
                                _enddate = Convert.ToDateTime(this.ToolCreatedEnd + " 23:59:59");
                                _startdate = Convert.ToDateTime(this.ToolCreatedStart + " 00:00:00");
                                _strWhere = "CreateDate between " + ConvertToUnixTime(_startdate) + " and " + ConvertToUnixTime(_enddate);
                                break;
                            default:
                                break;
                        }
                        if (!string.IsNullOrEmpty(_strWhere) && GiftCardModel.delGiftcardByOpstions(_strWhere) <= 0)
                        {
                            _strError += App.Current.FindResource("tool_cannot_del_created").ToString() + @"\n";
                        }
                    }
                    if (!string.IsNullOrEmpty(_strError))
                    {
                        ModernDialog md = new ModernDialog();
                        md.CloseButton.FindResource("close").ToString();
                        md.Content = _strError;
                        md.Title = App.Current.FindResource("notification").ToString();
                        md.ShowDialog();
                    }
                    GiftCardViewModel.eventAddGiftCard(null, null);
                    OnRequestClose();
                }
            }
        }
        private void onToolCloseWindowsExec(object _param)
        {
            IsTick = true;
            OnRequestClose();
        }
        private bool _generalDeliver = false;
        public bool GeneralDeliver
        {
            get { return _generalDeliver; }
            set
            {
                _generalDeliver = value;
                RaisePropertyChanged("GeneralDeliver");
            }
        }
        /***************/
        /*CODE FOR PREVIEW PRINT*/
        private string _isHtml;
        public string IsHTML
        {
            get { return _isHtml; }
            set
            {
                _isHtml = value;
                RaisePropertyChanged("IsHTML");
            }
        }
        private void onPreviewPrintPrintExec(object _param)
        {
            System.Windows.Forms.WebBrowser webBrowserPreview = new System.Windows.Forms.WebBrowser();
            webBrowserPreview.DocumentText = MultiCreateHTML(printId);
            webBrowserPreview.DocumentCompleted += webBrowser_ShowPrintPreviewDialog_DocumentCompleted;
        }
        private void onPreviewPrintOKExec(object _param)
        {
            if (IsTick)
            {
                GiftCardModel.setDeliveredGiftcard(printId, getCurrentUnixTime());
                printId.Clear();
                GiftCardViewModel.eventAddGiftCard(null, null);
                OnRequestClose();
            }
            else
            {
                IsTick = true;
                OnRequestClose();
            }
        }
        private void webBrowser_ShowPrintPreviewDialog_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            if (sender != null)
            {
                System.Windows.Forms.WebBrowser _send = (System.Windows.Forms.WebBrowser)sender;
                _send.ShowPrintDialog();
            }
            /*System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
            settings.Copies = 2;
            settings.ToPage = 3;
            System.Drawing.Printing.PrintDocument printDoc = new System.Drawing.Printing.PrintDocument();
            printDoc.PrinterSettings = settings;*/

        }
        private bool _isTick = true;
        public bool IsTick
        {
            get
            {
                return _isTick;
            }
            set
            {
                _isTick = value;
                RaisePropertyChanged("IsTick");
            }
        }
        private bool _isEditTick = true;
        public bool IsEditTick
        {
            get
            {
                return _isEditTick;
            }
            set
            {
                _isEditTick = value;
                RaisePropertyChanged("IsEditTick");
            }
        }
        /*END*/
    }
    public enum ExprieDate
    {
        AfterDay, ToDate
    }
    public enum toolCreated
    {
        None, On, Before, After, Between
    }
}
