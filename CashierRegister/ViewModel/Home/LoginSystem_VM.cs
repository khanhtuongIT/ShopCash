using CashierRegister.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegister.ViewModel.Home
{
    class LoginSystem_VM : ModelBase
    {
        private DataTable _dt_user = null;
        public RelayCommand formLoadCmd { get; private set; }
        public RelayCommand btOKSystemLoginCmd { get; private set; }
        public RelayCommand btCancelSystemLoginCmd { get; private set; }
        public RelayCommand otherLoginCmd { get; private set; }
        private string _sellerName = string.Empty;
        public string SellerName
        {
            get { return _sellerName; }
            set
            {
                _sellerName = value;
                RaisePropertyChanged("SellerName");
            }
        }
        private string _sellerPassword = string.Empty;
        public string SellerPassword
        {
            get { return _sellerPassword; }
            set
            {
                _sellerPassword = value;
                RaisePropertyChanged("SellerPassword");
            }
        }
        private string _txtError = string.Empty;
        public string ErrorLogin
        {
            get { return _txtError; }
            set
            {
                _txtError = value;
                RaisePropertyChanged("ErrorLogin");
            }
        }
        private string _visbProgress = System.Windows.Visibility.Hidden.ToString();
        public string VisbProgress
        {
            get { return _visbProgress; }
            set
            {
                _visbProgress = value;
                RaisePropertyChanged("VisbProgress");
            }
        }
        private string _visbButton = System.Windows.Visibility.Visible.ToString();
        public string VisbButton
        {
            get { return _visbButton; }
            set
            {
                _visbButton = value;
                RaisePropertyChanged("VisbButton");
            }
        }
        public LoginSystem_VM()
        {
            formLoadCmd = new RelayCommand(onFormLoadCmdExec);
            btOKSystemLoginCmd = new RelayCommand(onButtonOKSaleLoginExec);
            btCancelSystemLoginCmd = new RelayCommand(onCancelSystemLoginExec);
            otherLoginCmd = new RelayCommand(onOtherLoginExec);
        }
        private void onFormLoadCmdExec(object param)
        {
            OnFocusRequested(nameof(SellerName));
        }
        private void onCancelSystemLoginExec(object param)
        {
            OnRequestClose();
            Views.Home.LoginSystem.LoginCancelHandler(null, null);
        }
        private async void onButtonOKSaleLoginExec(object param)
        {
            string _user = SellerName;
            string _pass = SellerPassword;
            if (string.IsNullOrEmpty(_user))
            {
                ErrorLogin = "Please enter username !";
                OnFocusRequested(nameof(SellerName));
            }
            else
            {
                this.VisbButton = System.Windows.Visibility.Hidden.ToString();
                this.VisbProgress = System.Windows.Visibility.Visible.ToString();
                ErrorLogin = string.Empty;
                var slowTask = Task<string>.Factory.StartNew(() => this.UserLogin(_user, _pass));
                await slowTask;
                if (slowTask.Result.ToString() == "Success")
                {
                    Views.Home.LoginSystem.LoginSystemSuccessHandler(_user, null);
                    OnRequestClose();
                }
                else
                {
                    this.VisbButton = System.Windows.Visibility.Visible.ToString();
                    this.VisbProgress = System.Windows.Visibility.Hidden.ToString();
                    ErrorLogin = slowTask.Result.ToString();
                }
            }
        }
        private void onOtherLoginExec(object param)
        {
            OnRequestClose();
            Views.Home.LoginSystem.AdminForgotHandler(null, null);
        }
        private string UserLogin(string _user, string _pass)
        {
            string _rs = "Success";
            bool _isLogin = false;
            try
            {
                string _newPass = StaticClass.GeneralClass.MD5Hash(_pass.Trim());
                _dt_user = Model.Home.UserModel.getUserLogin(_user.Trim());
                if (_dt_user.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(_pass.Trim()) && string.IsNullOrEmpty(_dt_user.Rows[0]["Password"].ToString()))
                    {
                        _isLogin = true;
                    }
                    else if (_dt_user.Rows[0]["Password"].ToString() == _newPass || _pass.Trim() == _dt_user.Rows[0]["Password"].ToString())
                    {
                        _isLogin = true;
                    }
                    if (_isLogin)
                    {
                        StaticClass.GeneralClass.user_permission = Convert.ToInt32(_dt_user.Rows[0]["ID"].ToString());
                        StaticClass.GeneralClass.id_user_general = Convert.ToInt32(_dt_user.Rows[0]["ID"].ToString());
                        StaticClass.GeneralClass.name_user_general = _user;
                        StaticClass.GeneralClass.password_user_general = _dt_user.Rows[0]["Password"].ToString();
                    }
                    else
                    {
                        _rs = App.Current.FindResource("password_incorrect").ToString();
                    }
                }
                else
                {
                    _dt_user = Model.Home.UserModel.getSalePersonLogin(_user.Trim(), _newPass);
                    if(_dt_user.Rows.Count > 0)
                    {
                        StaticClass.GeneralClass.user_permission = 5;
                        StaticClass.GeneralClass.flag_salespersonlogin_general = true;
                        StaticClass.GeneralClass.salespersonname_login_general = _user.Trim();
                    }
                    else
                        _rs = App.Current.FindResource("user_not_exists").ToString();
                }
            }
            catch (Exception ex)
            {
                _rs = ex.Message;
            }
            return _rs;
        }
    }
}
