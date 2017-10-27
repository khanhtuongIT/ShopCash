using System;
using System.Collections.Generic;
using FirstFloor.ModernUI.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO;

namespace CashierRegister.Pages.CopyRight
{
    class SecurityManager
    {
        //CreateLicenseKey
        private string CreateLicenseKey(string _license_key_info)
        {
            string license_key = "";
            try
            {
                if (_license_key_info != "")
                {
                    _license_key_info = VNStrFilter(_license_key_info);
                    string charnum = "Z1X3V5T7R9P2N4L6J9H7F5D3B1A2C4E6G8I10K12M14O17Q19S00U11W01Y00Z1X3V5T7R9P2N4L6J9H7F5D3B1A26J9H7F5D3B1A2C4E6G8I10K12M14O17Q19S001Y00Z1X3V5T7R9P2N4L6J9H7F5D3B1A26J9H7F5D3B1A2C4D3B1A2C4E6G8I10K12M14O17Q19S00U11W01Y00Z1X3V5T7R9P2N4L6J9H7F5D3B1A26J9H7F5D3B1A2C4E6G8I10K12M14O17Q19S09P2N4L6J9H7F5D3B1A2C4E6G8I10K12M14O17Q19S00U11W01Y00Z1X3V5T7R9P2N4L6J9H7F5D3B1A26J9H7F5D3B1A2C4E6G8I10K12M14O17Q19S001Y00Z1X3V5T7R9P2N4L6J9H7F5D12M14O17Q19S00U11W01Y00Z1X3V5T7R9P2N4L6J9H7F5D3B1A26J9H7F5D3B1A2C4E6G8I10K12M14O";
                    SHA1Managed managed = new SHA1Managed();
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] bytes = encoding.GetBytes(_license_key_info.ToUpper());
                    byte[] arr_byte = encoding.GetBytes(Convert.ToBase64String(managed.ComputeHash(bytes)));
                    Array.Reverse(arr_byte);

                    string byteswap = "";
                    for (int i = 0; i < arr_byte.Length; i++)
                    {
                        byteswap += arr_byte[i];
                    }

                    int distance = 0;
                    for (int index = 0; index < byteswap.Length; index += 2)
                    {
                        license_key += charnum.Substring(int.Parse(byteswap.Substring(index, 2)), 1);

                        distance++;
                        if (distance == 25)
                            break;

                        if ((distance % 5) == 0 && index < (byteswap.Length - 2))
                            license_key += "-";
                    }
                }
            }
            catch (Exception ex)
            {
                ModernDialog md = new ModernDialog();
                md.Title = "Notification";
                md.Content = "Error: " + ex.Message;
                md.ShowDialog();
            }
            return license_key;
        }

        //VNStrFilter
        private readonly string[] VNSigns = new string[] { "aAdDeEiIoOuUyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "đ", "Đ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "íìịỉĩ", "ÍÌỊỈĨ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "ýỳỵỷỹ", "ÝỲỴỶỸ" };
        private string VNStrFilter(string _license_key_info)
        {
            //string license_key_info = _license_key_info;
            for (int i = 1; i < VNSigns.Length; i++)
            {
                for (int j = 0; j < VNSigns[i].Length; j++)
                {
                    _license_key_info = _license_key_info.Replace(VNSigns[i][j], VNSigns[0][i - 1]);
                }
            }

            return _license_key_info;
        }

        //ByteSwap
        private Byte[] ByteSwap(ref Byte[] arr_byte)
        {
            Byte temp;
            int arr_byte_lengt = arr_byte.Length;

            for (int i = 0; i < arr_byte_lengt; i++)
            {
                if (i <= (arr_byte_lengt / 2 - 1))
                {
                    temp = arr_byte[i];

                    arr_byte[i] = arr_byte[arr_byte_lengt - 1 - i];

                    arr_byte[arr_byte_lengt - 1 - i] = temp;
                }
                else
                    break;
            }

            return arr_byte;
        }

        //CheckSerialNumber
        public bool CheckSerialNumber(string softname_softwareversion_customername, string serial_number)
        {
            return CreateLicenseKey(softname_softwareversion_customername.ToUpper()) == serial_number.ToUpper() ? true : false;
        }

        //EncryptFile
        //key must be 64bits, 8 byte
        public void EncryptFile(string customername, string serialnumber, string inputfilename, string outputfilename, string key)
        {
            try
            {
                StreamWriter stream_writer = new StreamWriter(inputfilename);
                stream_writer.WriteLine("CustomerEmail:" + customername);
                stream_writer.WriteLine("SerialNumber:" + serialnumber);
                stream_writer.Close();

                FileStream filestream_input = new FileStream(inputfilename, FileMode.Open, FileAccess.Read);
                FileStream filestream_encrypted = new FileStream(outputfilename, FileMode.Create, FileAccess.Write);
                DESCryptoServiceProvider desCtoSP = new DESCryptoServiceProvider();
                desCtoSP.Key = ASCIIEncoding.ASCII.GetBytes(key);
                desCtoSP.IV = ASCIIEncoding.ASCII.GetBytes(key);
                ICryptoTransform ICtoTf = desCtoSP.CreateEncryptor();
                CryptoStream CtoS = new CryptoStream(filestream_encrypted, ICtoTf, CryptoStreamMode.Write);
                byte[] bytearrayinput = new byte[filestream_input.Length];
                filestream_input.Read(bytearrayinput, 0, bytearrayinput.Length);
                CtoS.Write(bytearrayinput, 0, bytearrayinput.Length);
                CtoS.Close();
                filestream_input.Close();
                System.IO.File.Delete(inputfilename);
                filestream_encrypted.Close();
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = "Error: " + ex.Message;
                page.ShowDialog();
            }
        }

        //DecryptFile
        public string[] DecryptFile(string inputfilename, string key)
        {
            string[] arr_register = null;
            try
            {
                DESCryptoServiceProvider DESCtoSP = new DESCryptoServiceProvider();
                DESCtoSP.Key = ASCIIEncoding.ASCII.GetBytes(key);
                DESCtoSP.IV = ASCIIEncoding.ASCII.GetBytes(key);
                FileStream filestream = new FileStream(inputfilename, FileMode.Open, FileAccess.Read);
                ICryptoTransform ICtoTF = DESCtoSP.CreateDecryptor();
                CryptoStream cryptostream = new CryptoStream(filestream, ICtoTF, CryptoStreamMode.Read);
                StreamReader streamreader = new StreamReader(cryptostream);
                string customername = streamreader.ReadLine().Split(':')[1].Trim();
                string serial_number = streamreader.ReadLine().Split(':')[1].Trim();
                streamreader.Close();
                arr_register = new string[] { customername, serial_number };
            }
            catch (Exception ex)
            {
                Pages.Notification page = new Pages.Notification();
                page.tblNotification.Text = "Error: " + ex.Message;
                page.ShowDialog();
            }
            return arr_register;
        }

        #region create serial number follow mac address
        //GetMacAddress
        public string GetMacAddress()
        {
            string mac_address = "";
            ManagementClass management_class = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection management_object_collection = management_class.GetInstances();
            foreach (ManagementObject management_object in management_object_collection)
            {
                if ((bool)management_object["IPEnabled"] == true)
                    mac_address = management_object["MacAddress"].ToString();
            }
            return mac_address;
        }
        //GetSerial
        public long MacAddressHash(string mac_address)
        {
            long sum = 0;
            int index = 1;
            foreach (char _char in mac_address)
            {
                if (char.IsDigit(_char))
                    sum += sum + (int)_char * (index * 3);
                else
                {
                    if (char.IsLetter(_char) == true)
                    {
                        switch (_char.ToString().ToUpper())
                        {
                            case "A":
                                sum += sum + (12 * (index * 12));
                                break;
                            case "B":
                                sum += sum + (18 * (index * 18));
                                break;
                            case "C":
                                sum += sum + (24 * (index * 24));
                                break;
                            case "D":
                                sum += sum + (30 * (index * 30));
                                break;
                            case "F":
                                sum += sum + (36 * (index * 36));
                                break;
                        }
                    }
                }

                index++;
            }
            return sum;
        }

        //CreateSerialNumber
        public long CreateSerialNumber(long mac_address_hash)
        {
            return mac_address_hash * mac_address_hash + 910512 / mac_address_hash + (120591 * (mac_address_hash / 2)) + (213 * mac_address_hash / 4);
        }

        //CheckKey
        public bool CheckKey(long serial_number_check, string mac_address)
        {
            long mac_address_hash = MacAddressHash(mac_address);
            long serial_number_trust = CreateSerialNumber(mac_address_hash);
            return serial_number_trust == serial_number_check ? true : false;
        }

        #endregion
    }
}

