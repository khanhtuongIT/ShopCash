using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegister.Helpers
{
    public class Pagination : ModelBase
    {
        private int _curPage = 1;
        private int _rowPerPage;
        private int _totalRows;
        private int _maxPage=0;
        private int _sqlStart;
        private string _limitSql;
        private int CalculateMaxPages()
        {
            _maxPage = (_totalRows / _rowPerPage);
            if (_totalRows % _rowPerPage != 0)
            {
                _maxPage += 1;
            }
            return _maxPage;
        }
        private string GeneralLimit()
        {
            this._sqlStart = (this._maxPage == 0) ? this._maxPage : ((this._curPage * this._rowPerPage) - this._rowPerPage);
            this._limitSql = " limit " + this._sqlStart + ", " + this._rowPerPage;
            return this._limitSql;
        }
        public int MaxPage
        {
            get { return CalculateMaxPages(); }
        }
        public int CurrentPage
        {
            get { return _curPage; }
            set
            {
                _curPage = value;
                RaisePropertyChanged("CurrentPage");
            }
        }
        public string StringLimit
        {
            get { return GeneralLimit(); }
        }
        public Pagination(int currentPage, int rowsPerPage)
        {
            CurrentPage = currentPage;
            this._rowPerPage = rowsPerPage;

        }
        public int TotalRows
        {
            get { return _totalRows; }
            set
            {
                _totalRows = value;
                RaisePropertyChanged("TotalRows");
                CalculateMaxPages();
            }
        }
    }
}
