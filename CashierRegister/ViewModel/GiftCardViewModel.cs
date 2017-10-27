using CashierRegister.Helpers;
using CashierRegister.Model;
using FirstFloor.ModernUI.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;

namespace CashierRegister.ViewModel
{
    public class GiftCardViewModel : ModelBase
    {
        public ObservableCollection<GiftCardModel> GiftCard
        {
            get;
            set;
        }
        public RelayCommand formLoadCmd { get; private set; }
        public RelayCommand addGiftCardCmd { get; private set; }
        public RelayCommand deleteGiftCardCmd { get; private set; }
        public RelayCommand editGiftCardCmd { get; private set; }
        public RelayCommand delGiftCardByOneCmd { get; private set; }
        public RelayCommand dbCLickCmd { get; private set; }
        public RelayCommand enterSearchGiftCardCmd { get; private set; }
        public RelayCommand<string> viewGiftCardCmd { get; private set; }
        public RelayCommand printMultiGiftCardCmd { get; private set; }
        public RelayCommand sendToMultiGiftCardCmd { get; private set; }
        public RelayCommand formUnloadedCmd { get; private set; }
        public RelayCommand<string> preViewGiftCardCmd { get; private set; }
        public RelayCommand toolsGiftCardCmd { get; private set; }
        public RelayCommand advanceSearchCmd { get; private set; }

        static public EventHandler eventAddGiftCard;
        Pagination paging;
        private string _txtKeySearch = string.Empty;
        private string _txtSortBy = string.Empty;
        #region Command for paging
        public RelayCommand<string> pagingCmd { get; private set; }
        #endregion End for paging
        private bool flag_loaded = false;
        public List<ComboBoxItemSortBy> SortByListEnum { get; set; }
        public RelayCommand sortByGiftCardCmd { get; private set; }
        private string _orderTime = string.Empty;
        private string _orderDate = string.Empty;
        public GiftCardViewModel()
        {
            GiftCard = new ObservableCollection<GiftCardModel>();
            addGiftCardCmd = new RelayCommand(onAddGiftCardEvent);
            formLoadCmd = new RelayCommand(onFormLoadedEvent);
            deleteGiftCardCmd = new RelayCommand(onDeleteGiftCardEvent);
            eventAddGiftCard += OnEventAddedGiftCard;
            editGiftCardCmd = new RelayCommand(onEditGiftCardEvent);
            delGiftCardByOneCmd = new RelayCommand(OnDelGiftCardByOneEvent);
            dbCLickCmd = new RelayCommand(onDataGridDoubleClickEvent);
            enterSearchGiftCardCmd = new RelayCommand(onEnterSearchGiftCardEvent);
            viewGiftCardCmd = new RelayCommand<string>(onPrintGiftCardEvent);
            printMultiGiftCardCmd = new RelayCommand(onPrintMultiGiftCardEvent);
            pagingCmd = new RelayCommand<string>(onPagingClick);
            paging = new Pagination(1, 25);
            formUnloadedCmd = new RelayCommand(OnFormUnloadedExec);
            preViewGiftCardCmd = new RelayCommand<string>(onViewGiftCardEvent);
            sendToMultiGiftCardCmd = new RelayCommand(onSendToMultiGiftCardEvent);
            toolsGiftCardCmd = new RelayCommand(onToolsGiftCardEvent);
            advanceSearchCmd = new RelayCommand(onAdvanceSearchEvent);
            SortByListEnum = new List<ComboBoxItemSortBy>()
            {
                new ComboBoxItemSortBy() { ValueSortByEnum = searchSortBy.AmountAsc, ValueSortByString = App.Current.FindResource("sort_giftcard_amount_asc").ToString() },
                new ComboBoxItemSortBy() { ValueSortByEnum = searchSortBy.AmountDesc, ValueSortByString = App.Current.FindResource("sort_giftcard_amount_desc").ToString() },
                new ComboBoxItemSortBy() { ValueSortByEnum = searchSortBy.BalanceAsc, ValueSortByString = App.Current.FindResource("sort_giftcard_balance_asc").ToString() },
                new ComboBoxItemSortBy() { ValueSortByEnum = searchSortBy.BalanceDesc, ValueSortByString = App.Current.FindResource("sort_giftcard_balance_desc").ToString() },
                new ComboBoxItemSortBy() { ValueSortByEnum = searchSortBy.CreatedAsc, ValueSortByString = App.Current.FindResource("sort_giftcard_created_asc").ToString() },
                new ComboBoxItemSortBy() { ValueSortByEnum = searchSortBy.CreatedDesc, ValueSortByString = App.Current.FindResource("sort_giftcard_created_desc").ToString() },
                new ComboBoxItemSortBy() { ValueSortByEnum = searchSortBy.ExpirationAcs, ValueSortByString = App.Current.FindResource("sort_giftcard_expiration_asc").ToString() },
                new ComboBoxItemSortBy() { ValueSortByEnum = searchSortBy.ExpirationDesc, ValueSortByString = App.Current.FindResource("sort_giftcard_expiration_desc").ToString() },
            };
            sortByGiftCardCmd = new RelayCommand(onSortByGiftCardEvent);
            _orderTime = (!string.IsNullOrEmpty(StaticClass.GeneralClass.app_settings["timeFormat"].ToString())) ? " " + StaticClass.GeneralClass.timeFromatSettings[StaticClass.GeneralClass.app_settings["timeFormat"]].ToString() : "";
            _orderDate = StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString() + _orderTime;
        }
        private void onSortByGiftCardEvent(object _param)
        {
            searchSortBy _val = SearchSortBy;
            this._txtSortBy = " order by ";
            switch (_val)
            {
                case searchSortBy.AmountAsc:
                    this._txtSortBy += "Amount asc";
                    break;
                case searchSortBy.AmountDesc:
                    this._txtSortBy += "Amount desc";
                    break;
                case searchSortBy.BalanceAsc:
                    this._txtSortBy += "Balance asc";
                    break;
                case searchSortBy.BalanceDesc:
                    this._txtSortBy += "Balance desc";
                    break;
                case searchSortBy.CreatedAsc:
                    this._txtSortBy += "CreateDate asc";
                    break;
                case searchSortBy.CreatedDesc:
                    this._txtSortBy += "CreateDate desc";
                    break;
                case searchSortBy.ExpirationAcs:
                    this._txtSortBy += "ExpirationDate asc";
                    break;
                case searchSortBy.ExpirationDesc:
                    this._txtSortBy += "ExpirationDate desc";
                    break;
                default:
                    this._txtSortBy += "CreateDate desc";
                    break;
            }
            onFormLoadedEvent(null);
        }
        private void onToolsGiftCardEvent(object _param)
        {
            Views.Setting.GiftCard_Tools page = new Views.Setting.GiftCard_Tools();
            page.ShowDialog();
        }
        private void OnFormUnloadedExec(object _param)
        {
            paging.CurrentPage = 1;
            ShowCurPage = 1;
            SearchDeliver = searchDeliver.None;
            SearchCreated = searchCreated.None;
            SearchGenaral = searchGenaral.None;
            this._txtKeySearch = string.Empty;
            this._txtSortBy = string.Empty;
        }
        private void onPagingClick(string _param)
        {
            if (_param == PageOptions.First.ToString())
            {
                paging.CurrentPage = 1;
                ShowNextPage = true;
                ShowLastPage = true;
                ShowFirstPage = false;
                ShowPrevPage = false;
            }
            else if (_param == PageOptions.Last.ToString())
            {
                paging.CurrentPage = paging.MaxPage;
                ShowNextPage = false;
                ShowLastPage = false;
                ShowFirstPage = true;
                ShowPrevPage = true;
            }
            else if (_param == PageOptions.Previous.ToString())
            {
                if (paging.CurrentPage == 1)
                {
                    ShowNextPage = true;
                    ShowLastPage = true;
                    ShowFirstPage = false;
                    ShowPrevPage = false;
                }
                else
                {
                    paging.CurrentPage--;
                    ShowNextPage = true;
                    ShowLastPage = true;
                    ShowFirstPage = true;
                    ShowPrevPage = true;
                }
            }
            else if (_param == PageOptions.Next.ToString())
            {
                if (paging.CurrentPage == paging.MaxPage)
                {
                    ShowNextPage = false;
                    ShowLastPage = false;
                    ShowFirstPage = true;
                    ShowPrevPage = true;
                }
                else
                {
                    paging.CurrentPage++;
                    ShowNextPage = true;
                    ShowLastPage = true;
                    ShowFirstPage = true;
                    ShowPrevPage = true;
                }
            }
            ShowCurPage = paging.CurrentPage;
            GiftCard.Clear();
            DataTable GiftCards = GiftCardModel.advanceSearchGiftCardByMultiCondition(this._txtKeySearch, paging.StringLimit, this._txtSortBy);
            if (GiftCards.Rows.Count > 0)
            {
                var i = 1;
                foreach (DataRow dr in GiftCards.Rows)
                {
                    DateTime dt = UnixTimeToDateTime(Convert.ToInt32(dr["CreateDate"]));
                    DateTime _expreTime = UnixTimeToDateTime(Convert.ToInt32(dr["ExpirationDate"].ToString()));
                    string _delivered = string.Empty;
                    if (Convert.ToInt32(dr["DeliveredDate"]) > 0)
                    {
                        DateTime _timeDeliver = UnixTimeToDateTime(Convert.ToInt32(dr["DeliveredDate"].ToString()));
                        _delivered = _timeDeliver.ToString(_orderDate);
                    }
                    string _customName = string.IsNullOrEmpty(dr["FullName"].ToString())? "None": dr["FullName"].ToString();
                    GiftCard.Add(new GiftCardModel { No = i, GiftCardID = Convert.ToInt32(dr["GiftCardID"].ToString()), Barcode = dr["Barcode"].ToString(), Serial = dr["Serial"].ToString(), CreateDate = dt.ToString(_orderDate), ExpirationDate = _expreTime.ToString(_orderDate), CustomerIDUse = Convert.ToInt32(dr["CustomerIDUse"].ToString()), Amount = Convert.ToDouble(dr["Amount"].ToString()), Balance = Convert.ToDouble(dr["Balance"].ToString()), IsChecked = false, CustomerName= _customName, DeliveredDate = _delivered });
                    i++;
                }
            }
            IsCheckAll = false;
        }
        private object _selectedGiftCard;
        public object SelectedGiftCard
        {
            get
            {
                return _selectedGiftCard;
            }
            set
            {
                if (_selectedGiftCard != value)
                {
                    _selectedGiftCard = value;
                    RaisePropertyChanged("SelectedGiftCard");
                }
            }
        }
        private string _totoGiftCard = null;
        public string TotalGiftCard
        {
            get
            {
                return _totoGiftCard;
            }
            set
            {
                if (_totoGiftCard != value)
                {
                    _totoGiftCard = value;
                    RaisePropertyChanged("TotalGiftCard");
                }
            }
        }
        private bool _isCheckAll = false;
        public bool IsCheckAll
        {
            get
            {
                return _isCheckAll;
            }
            set
            {
                if (_isCheckAll != value)
                {
                    _isCheckAll = value;
                    RaisePropertyChanged("IsCheckAll");
                    foreach (var _gift in GiftCard)
                    {
                        _gift.IsChecked = _isCheckAll;
                    }
                }
            }
        }
        private void onAddGiftCardEvent(object param)
        {
            Views.Setting.GiftCard_Add page = new Views.Setting.GiftCard_Add(15);
            page.ShowDialog();
        }
        private void InitGiftCard()
        {
            GiftCard.Clear();
            DataTable GiftCards = GiftCardModel.advanceSearchGiftCardByMultiCondition(this._txtKeySearch, paging.StringLimit, this._txtSortBy);
            if (GiftCards.Rows.Count==0 && paging.CurrentPage > 1)
            {
                paging.CurrentPage = paging.CurrentPage - 1;
                GiftCards = GiftCardModel.advanceSearchGiftCardByMultiCondition(this._txtKeySearch, paging.StringLimit, this._txtSortBy);
                ShowCurPage = paging.CurrentPage;
            }
            if (GiftCards.Rows.Count > 0)
            {
                var i = 1;
                foreach (DataRow dr in GiftCards.Rows)
                {
                    DateTime dt = UnixTimeToDateTime(Convert.ToInt32(dr["CreateDate"]));
                    DateTime _expreTime = UnixTimeToDateTime(Convert.ToInt32(dr["ExpirationDate"].ToString()));
                    string _giftCardImg = GiftCardModel.GiftCardImgPath + dr["Barcode"].ToString() + ".png";
                    string _deliverDate = string.Empty;
                    if (Convert.ToInt32(dr["DeliveredDate"].ToString()) > 0)
                    {
                        DateTime _isDelivered = UnixTimeToDateTime(Convert.ToInt32(dr["DeliveredDate"].ToString()));
                        _deliverDate = _isDelivered.ToString(StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString() + _orderTime);
                    }
                    GiftCard.Add(new GiftCardModel { No = i, GiftCardID = Convert.ToInt32(dr["GiftCardID"].ToString()), Barcode = dr["Barcode"].ToString(), Serial = dr["Serial"].ToString(), CreateDate = dt.ToString(StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString() + _orderTime), ExpirationDate = _expreTime.ToString(StaticClass.GeneralClass.dateFromatSettings[StaticClass.GeneralClass.app_settings["dateFormat"]].ToString() + _orderTime), CustomerIDUse = Convert.ToInt32(dr["CustomerIDUse"].ToString()), Amount = Convert.ToDouble(dr["Amount"].ToString()), Balance = Convert.ToDouble(dr["Balance"].ToString()), IsChecked = false, CustomerName = string.IsNullOrEmpty(dr["FullName"].ToString())?"None": dr["FullName"].ToString(), DeliveredDate = _deliverDate });
                    i++;
                }
            }
            if (GiftCards.Rows.Count==0 || (paging.CurrentPage == 1 && paging.MaxPage == 1))
            {
                ShowNextPage = false;
                ShowLastPage = false;
                ShowFirstPage = false;
                ShowPrevPage = false;
                IsPaging = "Hidden";
                /*if (GiftCards.Rows.Count == 0)
                {
                    ModernDialog md = new ModernDialog();
                    md.CloseButton.FindResource("close").ToString();
                    md.Content = App.Current.FindResource("cannot_found_data").ToString();
                    md.Title = App.Current.FindResource("notification").ToString();
                    md.ShowDialog();
                }*/
            }
            else if (paging.CurrentPage == paging.MaxPage)
            {
                ShowNextPage = false;
                ShowLastPage = false;
                ShowFirstPage = true;
                ShowPrevPage = true;
            }
            else if (paging.MaxPage > 1)
            {
                IsPaging = "Visible";
                ShowNextPage = true;
                ShowLastPage = true;
            }
            IsCheckAll = false;
        }
        private void onFormLoadedEvent(object param)
        {
            DataTable _gift = GiftCardModel.advanceSearchGiftCardByMultiCondition(this._txtKeySearch, string.Empty, this._txtSortBy);
            paging.TotalRows = _gift.Rows.Count;
            InitGiftCard();
            TotalGiftCard = App.Current.FindResource("total").ToString() + " (" + _gift.Rows.Count + ")";
        }
        private void onDeleteGiftCardEvent(object param)
        {
            bool detechChecked = false;
            var i = 0;
            while(i<GiftCard.Count && !detechChecked)
            {
                if (GiftCard[i].IsChecked == true)
                {
                    detechChecked = true;
                }
                i++;
            }
            if (!detechChecked)
            {
                ModernDialog md = new ModernDialog();
                md.CloseButton.FindResource("close").ToString();
                md.Content = App.Current.FindResource("select_least_one").ToString();
                md.Title = App.Current.FindResource("notification").ToString();
                md.ShowDialog();
            }
            else
            {
                ModernDialog mdd = new ModernDialog();
                mdd.Buttons = new System.Windows.Controls.Button[] { mdd.OkButton, mdd.CancelButton, };
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
                    List<int> _listId = new List<int>();
                    foreach (var _gift in GiftCard)
                    {
                        if (_gift.IsChecked)
                        {
                            _listId.Add(_gift.GiftCardID);
                        }
                    }
                    if (GiftCard_DB.deleteGiftCard(_listId) > 0)
                    {
                        onFormLoadedEvent(null);
                    }
                    else
                    {
                        ModernDialog md = new ModernDialog();
                        md.CloseButton.FindResource("close").ToString();
                        md.Content = App.Current.FindResource("cannot_del_giftcard").ToString();
                        md.Title = App.Current.FindResource("notification").ToString();
                        md.ShowDialog();
                    }
                }
            }
        }
        private void OnEventAddedGiftCard(object sender, EventArgs e)
        {
            if (sender != null)
            {
                SearchCreated = searchCreated.None;
                SearchGenaral = searchGenaral.None;
                SearchSortBy = searchSortBy.CreatedDesc;
                paging.CurrentPage = 1;
                ShowCurPage = paging.CurrentPage;
            }
            onFormLoadedEvent(null);
        }
        private void onEditGiftCardEvent(object param)
        {
            GiftCardModel _gift = (GiftCardModel)param;
            List<Int32> _listId = new List<Int32>();
            _listId.Add(Convert.ToInt32(_gift.GiftCardID));
            Views.Setting.GiftCard_Edit page = new Views.Setting.GiftCard_Edit(_listId);
            page.ShowDialog();
        }
        private void OnDelGiftCardByOneEvent(object param)
        {
            var i = 0;
            while (i < GiftCard.Count)
            {
                GiftCard[i].IsChecked = false;
                i++;
            }
            GiftCardModel _gift = (GiftCardModel)param;
            _gift.IsChecked = true;
            onDeleteGiftCardEvent(param);
        }
        private void onDataGridDoubleClickEvent(object param)
        {
            if (param == null) return;
            GiftCardModel _gift = param as GiftCardModel;
            List<Int32> _listId = new List<Int32>();
            _listId.Add(Convert.ToInt32(_gift.GiftCardID));
            Views.Setting.GiftCard_Edit page = new Views.Setting.GiftCard_Edit(_listId);
            page.ShowDialog();
        }
        private void onEnterSearchGiftCardEvent(object param)
        {
            string _txtKey = param as string;
            onAdvanceSearchEvent(_txtKey.Trim());
        }
        private void onViewGiftCardEvent(string param)
        {
            List<Int32> _listId = new List<Int32>();
            _listId.Add(Convert.ToInt32(param));
            Views.Setting.GiftCard_View page = new Views.Setting.GiftCard_View(_listId);
            page.ShowDialog();
        }
        private void onPrintGiftCardEvent(string param)
        {
            if (string.IsNullOrEmpty(param) || Convert.ToInt32(param)==0) return;
            List<Int32> lstId_ = new List<Int32>();
            lstId_.Add(Convert.ToInt32(param));
            /*System.Windows.Forms.WebBrowser webBrowserPreview = new System.Windows.Forms.WebBrowser();
            webBrowserPreview.DocumentText = MultiCreateHTML(lstId_);
            webBrowserPreview.DocumentCompleted += webBrowser_ShowPrintPreviewDialog_DocumentCompleted;*/
            Views.Setting.GiftCard_PreviewPrint prevPrint = new Views.Setting.GiftCard_PreviewPrint(lstId_);
            prevPrint.ShowDialog();
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
                string _valName = App.Current.FindResource("single_store").ToString() + ": "+ StaticClass.GeneralClass.app_settings["storeName"].ToString();
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
                string _valInfo = App.Current.FindResource("serial_number").ToString() + ": " + _param["Serial"].ToString() + "\n"+ App.Current.FindResource("expiration").ToString() + ": " + _expreTime.ToString(_orderDate) + "\n" + App.Current.FindResource("amount").ToString() + ": " + StaticClass.GeneralClass.currency_setting_general + " " + _param["Balance"].ToString() + "\n";

                finalImage = new Bitmap(width, height+(_kk_*20)+75);
                
                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.White);

                    g.DrawString(App.Current.FindResource("gift_card").ToString() + "\n", font, new SolidBrush(Color.Black), new PointF(width-70, 0));
                    g.DrawString(_valName + "\n" + _valAddress + "\n" + _valPhone, font, new SolidBrush(Color.Black), new PointF(0, 25));

                    //go through each image and draw it on the final image
                    int offset = 0;
                    int _off = 0;
                    foreach (Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new System.Drawing.Rectangle(offset, (_kk_ * 20)+20, image.Width, image.Height));
                        offset += image.Width;
                        _off += image.Height;
                    }
                    g.DrawString(_valInfo, font, new SolidBrush(Color.Black), new PointF(0, (_kk_ * 20)+ _off+20));
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
        private string MultiCreateHTML(List<Int32> _lstId)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
            htmlBuilder.Append("<head>");
            htmlBuilder.Append("<meta http-equiv='Content - Type' content='text / html; charset = utf - 8' />");
            htmlBuilder.Append("<title>"+ App.Current.FindResource("gift_card").ToString() + "</title>");
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
        private byte[] turnImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        private void webBrowser_ShowPrintPreviewDialog_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            if (sender != null)
            {
                System.Windows.Forms.WebBrowser _send = (System.Windows.Forms.WebBrowser)sender;
                _send.ShowPrintDialog();
                //System.Windows.Forms.SaveFileDialog save = new System.Windows.Forms.SaveFileDialog();

                /*System.Windows.Controls.PrintDialog dlg = new System.Windows.Controls.PrintDialog();
                dlg.PageRangeSelection = System.Windows.Controls.PageRangeSelection.AllPages;
                dlg.UserPageRangeEnabled = true;
                dlg.CurrentPageEnabled = true;
                dlg.SelectedPagesEnabled = true;
                if (dlg.ShowDialog() == true)
                {
                    _send.ShowPrintDialog();
                    _send.Print();
                    System.Windows.Xps.Packaging.XpsDocument xpsDocument = new System.Windows.Xps.Packaging.XpsDocument("C:\\FixedDocumentSequence.xps", FileAccess.ReadWrite);
                    System.Windows.Documents.FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();
                    dlg.PrintDocument(fixedDocSeq.DocumentPaginator, "Test print job");
                }*/
            }
        }
        private void onPrintMultiGiftCardEvent(object param)
        {
            List<Int32> _lstId = new List<Int32>();
            bool detechChecked = false;
            var i = 0;
            while (i < GiftCard.Count)
            {
                if (GiftCard[i].IsChecked == true)
                {
                    detechChecked = true;
                    _lstId.Add(GiftCard[i].GiftCardID);
                }
                i++;
            }
            if (!detechChecked)
            {
                ModernDialog md = new ModernDialog();
                md.CloseButton.FindResource("close").ToString();
                md.Content = App.Current.FindResource("select_least_one_print").ToString();
                md.Title = App.Current.FindResource("notification").ToString();
                md.ShowDialog();
            }
            else
            {
                /*System.Windows.Forms.WebBrowser webBrowserPreview = new System.Windows.Forms.WebBrowser();
                webBrowserPreview.DocumentText = MultiCreateHTML(_lstId);
                webBrowserPreview.DocumentCompleted += webBrowser_ShowPrintPreviewDialog_DocumentCompleted;*/
                Views.Setting.GiftCard_PreviewPrint prevPrint = new Views.Setting.GiftCard_PreviewPrint(_lstId);
                prevPrint.ShowDialog();
            }
        }
        private void onSendToMultiGiftCardEvent(object _param)
        {
            List<Int32> _lstId = new List<Int32>();
            bool detechChecked = false;
            var i = 0;
            while (i < GiftCard.Count)
            {
                if (GiftCard[i].IsChecked == true)
                {
                    detechChecked = true;
                    _lstId.Add(GiftCard[i].GiftCardID);
                }
                i++;
            }
            if (!detechChecked)
            {
                ModernDialog md = new ModernDialog();
                md.CloseButton.FindResource("close").ToString();
                md.Content = App.Current.FindResource("select_least_one_send").ToString();
                md.Title = App.Current.FindResource("notification").ToString();
                md.ShowDialog();
            }
            else
            {
                Views.Setting.GiftCard_View page = new Views.Setting.GiftCard_View(_lstId);
                page.ShowDialog();
            }
        }
        private string _showCurency = StaticClass.GeneralClass.currency_setting_general+" ";
        public string ShowCurency
        {
            get { return _showCurency; }
        }
        private string _isPaging = "Hidden";
        public string IsPaging
        {
            get { return _isPaging; }
            set
            {
                _isPaging = value;
                RaisePropertyChanged("IsPaging");
            }
        }
        private bool _showFirstPage = false;
        public bool ShowFirstPage
        {
            get { return _showFirstPage; }
            set
            {
                _showFirstPage = value;
                RaisePropertyChanged("ShowFirstPage");
            }
        }
        private bool _showLastPage = false;
        public bool ShowLastPage
        {
            get { return _showLastPage; }
            set
            {
                _showLastPage = value;
                RaisePropertyChanged("ShowLastPage");
            }
        }
        private bool _showPrevPage = false;
        public bool ShowPrevPage
        {
            get { return _showPrevPage; }
            set
            {
                _showPrevPage = value;
                RaisePropertyChanged("ShowPrevPage");
            }
        }
        private bool _showNextPage = false;
        public bool ShowNextPage
        {
            get { return _showNextPage; }
            set
            {
                _showNextPage = value;
                RaisePropertyChanged("ShowNextPage");
            }
        }
        private int _showCurPage = 1;
        public int ShowCurPage
        {
            get { return _showCurPage; }
            set
            {
                _showCurPage = value;
                RaisePropertyChanged("ShowCurPage");
            }
        }
        
        /*Code for search gift cards*/
        private searchGenaral _search_genaral = searchGenaral.None;
        public searchGenaral SearchGenaral
        {
            get { return _search_genaral; }
            set
            {
                _search_genaral = value;
                RaisePropertyChanged("SearchGenaral");
            }
        }
        private searchCreated _search_created = searchCreated.None;
        public searchCreated SearchCreated
        {
            get { return _search_created; }
            set
            {
                _search_created = value;
                RaisePropertyChanged("SearchCreated");
                setEnableCreated(_search_created);
            }
        }
        private searchSortBy _search_sortby = searchSortBy.CreatedDesc;
        public searchSortBy SearchSortBy
        {
            get { return _search_sortby; }
            set
            {
                _search_sortby = value;
                RaisePropertyChanged("SearchSortBy");
            }
        }
        private string _toolCreatedOn = "";
        public string SearchCreatedOn
        {
            get { return _toolCreatedOn; }
            set
            {
                _toolCreatedOn = value;
                RaisePropertyChanged("SearchCreatedOn");
            }
        }
        private string _toolCreatedAfter = "";
        public string SearchCreatedAfter
        {
            get { return _toolCreatedAfter; }
            set
            {
                _toolCreatedAfter = value;
                RaisePropertyChanged("SearchCreatedAfter");
            }
        }
        private string _toolCreatedBefore = "";
        public string SearchCreatedBefore
        {
            get { return _toolCreatedBefore; }
            set
            {
                _toolCreatedBefore = value;
                RaisePropertyChanged("SearchCreatedBefore");
            }
        }
        private string _toolCreatedStart = "";
        public string SearchCreatedStart
        {
            get { return _toolCreatedStart; }
            set
            {
                _toolCreatedStart = value;
                RaisePropertyChanged("SearchCreatedStart");
            }
        }
        private string _toolCreatedEnd = "";
        public string SearchCreatedEnd
        {
            get { return _toolCreatedEnd; }
            set
            {
                _toolCreatedEnd = value;
                RaisePropertyChanged("SearchCreatedEnd");
            }
        }
        private bool _enableOn = false;
        public bool SearchEnableOn
        {
            get { return _enableOn; }
            set
            {
                _enableOn = value;
                RaisePropertyChanged("SearchEnableOn");
            }
        }
        private bool _enableBefore = false;
        public bool SearchEnableBefore
        {
            get { return _enableBefore; }
            set
            {
                _enableBefore = value;
                RaisePropertyChanged("SearchEnableBefore");
            }
        }
        private bool _enableAfter = false;
        public bool SearchEnableAfter
        {
            get { return _enableAfter; }
            set
            {
                _enableAfter = value;
                RaisePropertyChanged("SearchEnableAfter");
            }
        }
        private bool _enableBetween = false;
        public bool SearchEnableBetween
        {
            get { return _enableBetween; }
            set
            {
                _enableBetween = value;
                RaisePropertyChanged("SearchEnableBetween");
            }
        }
        private void setEnableCreated(searchCreated _opts)
        {
            if (_opts == searchCreated.None)
            {
                SearchEnableOn = false;
                SearchEnableBefore = false;
                SearchEnableAfter = false;
                SearchEnableBetween = false;
            }
            if (_opts == searchCreated.CreatedOn)
            {
                SearchEnableOn = true;
                SearchEnableBefore = false;
                SearchEnableAfter = false;
                SearchEnableBetween = false;
            }
            if (_opts == searchCreated.Before)
            {
                SearchEnableOn = false;
                SearchEnableBefore = true;
                SearchEnableAfter = false;
                SearchEnableBetween = false;
            }
            if (_opts == searchCreated.After)
            {
                SearchEnableOn = false;
                SearchEnableBefore = false;
                SearchEnableAfter = true;
                SearchEnableBetween = false;
            }
            if (_opts == searchCreated.Between)
            {
                SearchEnableOn = false;
                SearchEnableBefore = false;
                SearchEnableAfter = false;
                SearchEnableBetween = true;
            }
        }
        private void onAdvanceSearchEvent(object _param)
        {
            string _txtKey = _param as string;
            this._txtKeySearch = string.Empty;
            searchGenaral _optsGenaral = SearchGenaral;
            searchCreated _optsCreated = SearchCreated;
            searchDeliver _optsDeliver = SearchDeliver;
            if (string.IsNullOrEmpty(_txtKey) && _optsGenaral == searchGenaral.None && _optsCreated == searchCreated.None && _optsDeliver==searchDeliver.None)
            {
                this._txtKeySearch = string.Empty;
                paging.CurrentPage = 1;
                ShowCurPage = paging.CurrentPage;
                onFormLoadedEvent(null);
                return;
            }
            else
            {
                string _strCondition = string.Empty;
                DateTime _enddate, _startdate;
                if (!string.IsNullOrEmpty(_txtKey))
                {
                    _strCondition += "Barcode like $'%"+ _txtKey.Trim() + "%' and ";
                }
                if(_optsGenaral != searchGenaral.None)
                {
                    switch (_optsGenaral)
                    {
                        case searchGenaral.Expired:
                            DateTime _expreTime;
                            DateTime oDate = DateTime.Now;
                            _expreTime = Convert.ToDateTime(oDate.ToString(System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern) + " 23:59:59");
                            _strCondition += "ExpirationDate <= " + ConvertToUnixTime(_expreTime)+" and ";
                            break;
                        case searchGenaral.InUse:
                            _strCondition += "Balance > 0 and ";
                            break;
                        case searchGenaral.IsUsed:
                            _strCondition += "Balance <= 0 and ";
                            break;
                        default:
                            break;
                    }
                }
                if(_optsCreated != searchCreated.None)
                {
                    switch (_optsCreated)
                    {
                        case searchCreated.Before:
                            _enddate = Convert.ToDateTime(this.SearchCreatedBefore + " 00:00:00");
                            _strCondition += "CreateDate < " + ConvertToUnixTime(_enddate) + " and ";
                            break;
                        case searchCreated.After:
                            _enddate = Convert.ToDateTime(this.SearchCreatedAfter + " 23:59:59");
                            _strCondition += "CreateDate > " + ConvertToUnixTime(_enddate) + " and ";
                            break;
                        case searchCreated.Between:
                            _enddate = Convert.ToDateTime(this.SearchCreatedEnd + " 23:59:59");
                            _startdate = Convert.ToDateTime(this.SearchCreatedStart + " 00:00:00");
                            _strCondition += "CreateDate between " + ConvertToUnixTime(_startdate) + " and " + ConvertToUnixTime(_enddate) + " and ";
                            break;
                        case searchCreated.CreatedOn:
                            _enddate = Convert.ToDateTime(this.SearchCreatedOn + " 23:59:59");
                            _startdate = Convert.ToDateTime(this.SearchCreatedOn + " 00:00:00");
                            _strCondition += "CreateDate between " + ConvertToUnixTime(_startdate) + " and " + ConvertToUnixTime(_enddate) + " and ";
                            break;
                        default:
                            break;
                    }
                }
                if (_optsDeliver != searchDeliver.None)
                {
                    if (_optsDeliver == searchDeliver.Delivered)
                    {
                        _strCondition += "DeliveredDate > 0 and ";
                    }
                    else
                    {
                        _strCondition += "DeliveredDate = 0 and ";
                    }
                }
                _strCondition = _strCondition.Remove(_strCondition.Length - 5, 5);
                this._txtKeySearch = _strCondition;
                paging.CurrentPage = 1;
                ShowCurPage = paging.CurrentPage;
                onFormLoadedEvent(null);
            }
        }
        private searchDeliver _isDeliver;
        public searchDeliver SearchDeliver
        {
            get { return _isDeliver; }
            set
            {
                _isDeliver = value;
                RaisePropertyChanged("SearchDeliver");
            }
        }
        /****************************/
    }
    public enum PageOptions
    {
        First, Previous, Next, Last
    }
    public enum searchGenaral
    {
        None, Expired, InUse, IsUsed
    }
    public enum searchCreated
    {
        None, CreatedOn, After, Before, Between
    }
    public enum searchSortBy
    {
        CreatedAsc, CreatedDesc, ExpirationAcs, ExpirationDesc, AmountAsc, AmountDesc, BalanceAsc, BalanceDesc
    }
    public enum searchDeliver
    {
        None, Delivered, NotDeliver
    }
    public class ComboBoxItemSortBy
    {
        public searchSortBy ValueSortByEnum { get; set; }
        public string ValueSortByString { get; set; }
    }

}
