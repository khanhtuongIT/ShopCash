using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterEntity
{
    public class EC_tb_Category
    {
        private int _No;

        public int No
        {
            get { return _No; }
            set { _No = value; }
        }
        
        private int _CategoryID;

        public int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        private string _CategoryName;

        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        //add info
        private double _Height;

        public double Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        private double _Width;

        public double Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private string _BeImageDelete;

        public string BeImageDelete
        {
            get { return _BeImageDelete; }
            set { _BeImageDelete = value; }
        }

        private string _AfImageDelete;

        public string AfImageDelete
        {
            get { return _AfImageDelete; }
            set { _AfImageDelete = value; }
        }

        private string _BeImageEdit;

        public string BeImageEdit
        {
            get { return _BeImageEdit; }
            set { _BeImageEdit = value; }
        }

        private string _AfImageEdit;

        public string AfImageEdit
        {
            get { return _AfImageEdit; }
            set { _AfImageEdit = value; }
        }

        private int _Index;

        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }
    }
}
