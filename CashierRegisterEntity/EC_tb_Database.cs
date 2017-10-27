using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_Database
    {
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _IdDatabase;

        public string IdDatabase
        {
            get { return _IdDatabase; }
            set { _IdDatabase = value; }
        }

        private string _DownloadUrl;

        public string DownloadUrl
        {
            get { return _DownloadUrl; }
            set { _DownloadUrl = value; }
        }

        private string _BackupDate;

        public string BackupDate
        {
            get { return _BackupDate; }
            set { _BackupDate = value; }
        }

        private int _Active;

        public int Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        private System.Windows.Media.Imaging.BitmapImage _BitmapImage_Restore;
        public System.Windows.Media.Imaging.BitmapImage BitmapImage_Restore
        {
            get { return _BitmapImage_Restore; }
            set { _BitmapImage_Restore = value; }
        }

        private System.Windows.Media.Imaging.BitmapImage _BitmapImage_Delete;

        public System.Windows.Media.Imaging.BitmapImage BitmapImage_Delete
        {
            get { return _BitmapImage_Delete; }
            set { _BitmapImage_Delete = value; }
        }

        private System.Windows.Media.Imaging.BitmapImage _BitmapImage_Undo;

        public System.Windows.Media.Imaging.BitmapImage BitmapImage_Undo
        {
            get { return _BitmapImage_Undo; }
            set { _BitmapImage_Undo = value; }
        }

        private string _Visibility;

        public string Visibility
        {
            get { return _Visibility; }
            set { _Visibility = value; }
        }

        private string _FileSize;

        public string FileSize
        {
            get { return _FileSize; }
            set { _FileSize = value; }
        }
    }
}
