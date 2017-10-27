using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CashierRegisterEntity;
using CashierRegisterBUS;
using System.Threading;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for DeleteRestoreLocal_Confirm.xaml
    /// </summary>
    public partial class DeleteRestoreLocal_Confirm : ModernDialog
    {
        //delegate
        public delegate void btnDelete_Click_Delegate();
        public event btnDelete_Click_Delegate btndelete_delegate;

        //folder delete
        public string folder_delete = "";
        private string _folder_delete = "";
        private string __folder_delete = "";

        //current directory
        private string current_directory = System.IO.Directory.GetCurrentDirectory();

        //DeleteRestoreLocal_Confirm
        public DeleteRestoreLocal_Confirm()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { };
            muiBtnOK.Focus();
        }

        //muiBtnCancel_Click
        private void muiBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //muiBtnOK_Click
        Thread delete_thread = null;
        private void muiBtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (delete_thread != null && delete_thread.ThreadState == ThreadState.Running) { }
                else
                {
                    delete_thread = new Thread(() =>
                    {
                        try
                        {
                            if (folder_delete == "")
                            {
                                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("cannot_find_database").ToString(); })); ;
                                return;
                            }
                            else
                            {
                                this.muiBtnOK.Dispatcher.Invoke((Action)(() => { muiBtnOK.Visibility = System.Windows.Visibility.Hidden; }));
                                this.mpr.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.mpr.Visibility = System.Windows.Visibility.Visible;
                                    this.mpr.IsActive = true;
                                }));
                                this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("please_wait").ToString(); }));
                                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ""; }));

                                _folder_delete = folder_delete.Replace("/", ".");
                                __folder_delete = _folder_delete.Replace(":", "..");

                                //if database type is sqlite
                                if (StaticClass.GeneralClass.flag_database_type_general == false)
                                {
                                    if (System.IO.Directory.Exists(current_directory + @"\DBRES_Local\" + __folder_delete) == true)
                                        System.IO.Directory.Delete(current_directory + @"\DBRES_Local\" + __folder_delete, true);
                                }
                                //if database type is sqlserver
                                else
                                {
                                    if (System.IO.Directory.Exists(current_directory + @"\DBRESSer_Local\" + __folder_delete) == true)
                                        System.IO.Directory.Delete(current_directory + @"\DBRESSer_Local\" + __folder_delete, true);
                                }

                                Thread.Sleep(500);
                                this.mpr.Dispatcher.Invoke((Action)(() =>
                                {
                                    this.mpr.Visibility = System.Windows.Visibility.Hidden;
                                    this.mpr.IsActive = false;
                                }));
                                this.muiBtnOK.Dispatcher.Invoke((Action)(() => { muiBtnOK.Visibility = System.Windows.Visibility.Visible; }));
                                this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_delete").ToString(); }));
                                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("delete_success").ToString(); }));

                                if (btndelete_delegate != null)
                                {
                                    btndelete_delegate();
                                    this.Dispatcher.Invoke((Action)(() => { this.Close(); }));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_delete").ToString(); }));
                            this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("have_not_access").ToString(); }));
                        }
                    });
                    delete_thread.Start();
                }
            }
            catch (Exception ex)
            {
                this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_delete").ToString(); }));
                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ex.Message; }));
            }
        }
    
    }
}
