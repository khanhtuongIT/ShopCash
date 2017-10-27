using System;
using System.Collections.Generic;
using FirstFloor.ModernUI.Windows.Controls;
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
using System.Threading;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for BackupDB.xaml
    /// </summary>
    public partial class BackupDB : UserControl
    {
        //user control
        //private UserControl uc_importlocal = null;
        private UserControl uc_backuplocal = null;
        private UserControl uc_undorestorelocal = null;
        private UserControl uc_exportlocal = null;
        private UserControl uc_import_export_gdrive = null;
        private UserControl uc_im_export = null;

        //thread
        private Thread thread_usercontrol = null;

        //BackupDB
        public BackupDB()
        {
            InitializeComponent();

            #region UserControl_Loaded
            if (thread_usercontrol != null && thread_usercontrol.ThreadState == ThreadState.Running) { }
            else
            {
                thread_usercontrol = new Thread(() => 
                {
                    try
                    {
                        this.Dispatcher.Invoke((Action)(() => 
                        {
                            BackupLocal backuplocal = new BackupLocal();
                            uc_backuplocal = (UserControl)backuplocal;
                            uc_backuplocal.Visibility = System.Windows.Visibility.Hidden;
                            uc_backuplocal.Name = "UCBackupLocal";
                            grContent.Children.Add(uc_backuplocal);

                            UndoRestoreLocal undorestorelocal = new UndoRestoreLocal();
                            uc_undorestorelocal = (UserControl)undorestorelocal;
                            uc_undorestorelocal.Visibility = System.Windows.Visibility.Hidden;
                            uc_undorestorelocal.Name = "UCUndoDb";
                            grContent.Children.Add(uc_undorestorelocal);

                            ExportLocal exportlocal = new ExportLocal();
                            uc_exportlocal = (UserControl)exportlocal;
                            uc_exportlocal.Visibility = System.Windows.Visibility.Hidden;
                            uc_exportlocal.Name = "UCExportLocal";
                            grContent.Children.Add(uc_exportlocal);

                        }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = "Close";
                            md.Title = "Notification";
                            md.Content = "Error: " + ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_usercontrol.Start();
            }
            #endregion
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (uc_backuplocal != null || uc_exportlocal != null)
            {
                tviBackup.IsSelected = true;
                //uc_importlocal.Visibility = System.Windows.Visibility.Visible;

                uc_undorestorelocal.Visibility = System.Windows.Visibility.Hidden;
                uc_exportlocal.Visibility = System.Windows.Visibility.Hidden;
                if (uc_import_export_gdrive != null)
                    uc_import_export_gdrive.Visibility = System.Windows.Visibility.Hidden;

                uc_backuplocal.Visibility = System.Windows.Visibility.Visible;
                if(uc_im_export!=null)
                    grContent.Children.Remove(uc_im_export);
            }
        }

        private void tviExport_Selected(object sender, RoutedEventArgs e)
        {
            //uc_importlocal.Visibility = System.Windows.Visibility.Hidden;
            uc_backuplocal.Visibility = System.Windows.Visibility.Hidden;
            uc_undorestorelocal.Visibility = System.Windows.Visibility.Hidden;
            if (uc_import_export_gdrive != null)
                uc_import_export_gdrive.Visibility = System.Windows.Visibility.Hidden;

            uc_exportlocal.Visibility = System.Windows.Visibility.Visible;
            if (uc_im_export != null)
                grContent.Children.Remove(uc_im_export);
        }

        //tviGDrive_Selected

        private void tviGDrive_Selected(object sender, RoutedEventArgs e)
        {
            if (uc_import_export_gdrive != null)
                grContent.Children.Remove(uc_import_export_gdrive);

            ImportExportGDrive import_export_gdrive = new ImportExportGDrive();
            uc_import_export_gdrive = (UserControl)import_export_gdrive;
            uc_import_export_gdrive.Visibility = System.Windows.Visibility.Hidden;
            uc_import_export_gdrive.Name = "UCImportExportGD";
            grContent.Children.Add(uc_import_export_gdrive);

            uc_backuplocal.Visibility = System.Windows.Visibility.Hidden;
            uc_undorestorelocal.Visibility = System.Windows.Visibility.Hidden;
            uc_exportlocal.Visibility = System.Windows.Visibility.Hidden;
            uc_import_export_gdrive.Visibility = System.Windows.Visibility.Visible;
            if (uc_im_export != null)
                grContent.Children.Remove(uc_im_export);
        }

        //tviBackup_Selected
        private void tviBackup_Selected(object sender, RoutedEventArgs e)
        {
            grContent.Children.Remove(uc_backuplocal);
            BackupLocal backuplocal = new BackupLocal();
            uc_backuplocal = (UserControl)backuplocal;
            uc_backuplocal.Visibility = System.Windows.Visibility.Hidden;
            uc_backuplocal.Name = "UCBackupLocal";
            grContent.Children.Add(uc_backuplocal);

            uc_backuplocal.Visibility = System.Windows.Visibility.Visible;
            uc_undorestorelocal.Visibility = System.Windows.Visibility.Hidden;
            uc_exportlocal.Visibility = System.Windows.Visibility.Hidden;
            if (uc_im_export != null)
                grContent.Children.Remove(uc_im_export);
            if (uc_import_export_gdrive != null)
                uc_import_export_gdrive.Visibility = System.Windows.Visibility.Hidden;
        }

        //tviUndo_Selected
        private void tviUndo_Selected(object sender, RoutedEventArgs e)
        {
            grContent.Children.Remove(uc_undorestorelocal);
            UndoRestoreLocal undorestorelocal = new UndoRestoreLocal();
            uc_undorestorelocal = (UserControl)undorestorelocal;
            uc_undorestorelocal.Visibility = System.Windows.Visibility.Hidden;
            uc_undorestorelocal.Name = "UCUndoDb";
            grContent.Children.Add(uc_undorestorelocal);

            uc_backuplocal.Visibility = System.Windows.Visibility.Hidden;
            uc_exportlocal.Visibility = System.Windows.Visibility.Hidden;
            if (uc_import_export_gdrive != null)
                uc_import_export_gdrive.Visibility = System.Windows.Visibility.Hidden;

            uc_undorestorelocal.Visibility = System.Windows.Visibility.Visible;
            if (uc_im_export != null)
                grContent.Children.Remove(uc_im_export);
        }

        private void tviImExport_Selected(object sender, RoutedEventArgs e)
        {
            if (uc_im_export != null)
                grContent.Children.Remove(uc_im_export);

            Views.Setting.ImExportDataBase imexportDB = new Views.Setting.ImExportDataBase();
            uc_im_export = (UserControl)imexportDB;
            uc_im_export.Visibility = Visibility.Hidden;
            uc_im_export.Name = "UCImExportDB";
            grContent.Children.Add(uc_im_export);

            if (uc_import_export_gdrive != null)
                uc_import_export_gdrive.Visibility = Visibility.Hidden;
            uc_backuplocal.Visibility = Visibility.Hidden;
            uc_undorestorelocal.Visibility = Visibility.Hidden;
            uc_exportlocal.Visibility = Visibility.Hidden;
            uc_im_export.Visibility = Visibility.Visible;
        }
    }
}
