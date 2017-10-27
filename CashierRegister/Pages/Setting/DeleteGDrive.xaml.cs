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

using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Services;
using Google.Apis.Logging;
using Google.Apis.Upload;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Util.Store;
using CashierRegister.StaticClass;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for DeleteGDrive.xaml
    /// </summary>
    public partial class DeleteGDrive : ModernDialog
    {
        //delegate
        public delegate void btnDelete_Click_Delegate();
        public event btnDelete_Click_Delegate btndelete_delegate;

        //DeleteSalesperson
        public DeleteGDrive()
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
                delete_thread = new Thread(() =>
                {
                    try
                    {
                        this.muiBtnOK.Dispatcher.Invoke((Action)(() => { muiBtnOK.Visibility = System.Windows.Visibility.Hidden; }));
                        this.mpr.Dispatcher.Invoke((Action)(() => { mpr.IsActive = true; }));
                        this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("please_wait").ToString(); }));

                        //UserCredential user_credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = client_id, ClientSecret = client_secret }, new[] { DriveService.Scope.Drive }, "user", CancellationToken.None).Result;
                        UserCredential user_credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = GeneralClass.client_id, ClientSecret = GeneralClass.client_secret, }, new[] { DriveService.Scope.DriveFile }, "user", CancellationToken.None, new FileDataStore("TuanNguyen.GoogleDrive.Auth.Store")).Result;

                        //create the drive service
                        var driveservice = new DriveService(new BaseClientService.Initializer() { HttpClientInitializer = user_credential, ApplicationName = "Delete database" });

                        if (DeleteDatabaseFromGDrive(driveservice, StaticClass.GeneralClass.id_database_general) == true)
                        {
                            if (btndelete_delegate != null)
                            {
                                this.muiBtnOK.Dispatcher.Invoke((Action)(() => { muiBtnOK.Visibility = System.Windows.Visibility.Visible; }));
                                this.mpr.Dispatcher.Invoke((Action)(() => { mpr.IsActive = false; }));
                                btndelete_delegate();
                                this.Dispatcher.Invoke((Action)(() => { this.Close(); }));
                            }
                        }
                    }
                    catch (AggregateException)
                    {
                        this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_delete").ToString(); }));
                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("have_not_access").ToString(); }));
                    }

                    catch (Exception)
                    {
                        this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_delete").ToString(); }));
                        this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = FindResource("have_not_access").ToString(); }));
                    }
                });
                delete_thread.Start();
            }
            catch (Exception ex)
            {
                this.tblConfirm.Dispatcher.Invoke((Action)(() => { this.tblConfirm.Text = FindResource("really_want_delete").ToString(); }));
                this.tblNotification.Dispatcher.Invoke((Action)(() => { this.tblNotification.Text = ex.Message; }));
            }
        }

        //delete the database form google drive
        private bool DeleteDatabaseFromGDrive(DriveService driveservice, string id)
        {
            bool flag = false;
            if (driveservice.Files.Delete(id).Execute() != null)
                flag = true;
            else
                flag = false;

            return flag;
        }
    }
}
