using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CashierRegister.Helpers
{
    public abstract class ModelBase : INotifyPropertyChanging, INotifyPropertyChanged, IRequestFocus
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public virtual bool IgnorePropertyChangeEvents { get; set; }
        public virtual void RaisePropertyChangingEvent(string propertyName)
        {
            // Exit if changes ignored
            if (IgnorePropertyChangeEvents) return;

            // Exit if no subscribers
            if (PropertyChanging == null) return;

            // Raise event
            var e = new PropertyChangingEventArgs(propertyName);
            PropertyChanging(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        static public event EventHandler RequestClose;

        protected void OnRequestClose()
        {
            EventHandler handler = RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler<FocusRequestedEventArgs> FocusRequested;
        protected virtual void OnFocusRequested(string propertyName)
        {
            FocusRequested?.Invoke(this, new FocusRequestedEventArgs(propertyName));
        }

        public Int32 getCurrentUnixTime()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds;
            return unixTimestamp;
        }
        public Int32 ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (Int32)(datetime - sTime).TotalSeconds;
        }
        public DateTime UnixTimeToDateTime(Int32 unixtime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return sTime.AddSeconds(unixtime);
        }
        public System.Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }
        public void writeSchema(string _fileCSV)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(_fileCSV);
            try
            {
                System.IO.FileStream fsOutput = new System.IO.FileStream(fi.DirectoryName.ToString() + "\\schema.ini", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                System.IO.StreamWriter srOutput = new System.IO.StreamWriter(fsOutput);
                string s1, s2, s3, s4, s5;

                s1 = "[" + fi.Name.ToString() + "]";
                s2 = "ColNameHeader=true";
                s3 = "Format=Delimited(,)";
                s4 = "MaxScanRows=25";
                s5 = "CharacterSet=65001";

                srOutput.WriteLine(s1.ToString() + "\r\n" + s2.ToString() + "\r\n" + s3.ToString() + "\r\n" + s4.ToString() + "\r\n" + s5.ToString());
                srOutput.Close();
                fsOutput.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "writeSchema");
            }
            finally
            { }
        }
    }
    public static class BitmapConversion
    {
        public static System.Windows.Media.Imaging.BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }
    }
    public enum BackupRestore
    {
        backup, restore
    }
}
