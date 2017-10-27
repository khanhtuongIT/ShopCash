using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using CashierRegisterEntity;
using System.Data;
using CashierRegisterBUS;

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : UserControl
    {
        //using for user
        private BUS_tb_User bus_tb_user = new BUS_tb_User();
        private List<EC_tb_User> list_ec_tb_user = new List<EC_tb_User>();
        private string current_directory = System.IO.Directory.GetCurrentDirectory().ToString();
        private bool flag_check_loaded = false;

        //thread
        private Thread thread_content = null;

        //User
        public User()
        {
            InitializeComponent();
        }

        //UserControl_Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (flag_check_loaded == true)
                flag_check_loaded = false;
            else
            {
                flag_check_loaded = true;
                GetUser();
            }
        }

        //btnEdit_Click
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dtgUser.SelectedIndex > -1)
            {
                int index = dtgUser.SelectedIndex;
                EditUser page = new EditUser();
                page.tblID.Text = list_ec_tb_user[index].ID.ToString();
                page.tblName.Text = list_ec_tb_user[index].Name;
                page.txbEmail.Text = list_ec_tb_user[index].Email;
                page.txbAddress.Text = list_ec_tb_user[index].Address;
                page.txbQuestion.Text = list_ec_tb_user[index].Question;
                page.txbAnswer.Text = list_ec_tb_user[index].Answer;

                page.btnedit_delegate += btnEdit_Click_Delegate;
                page.ShowDialog();
            }
        }

        //dtgUser_MouseDoubleClick
        private void dtgUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgUser.SelectedIndex > -1)
            {
                int index = dtgUser.SelectedIndex;
                EditUser page = new EditUser();
                page.tblID.Text = list_ec_tb_user[index].ID.ToString();
                page.tblName.Text = list_ec_tb_user[index].Name;
                page.txbEmail.Text = list_ec_tb_user[index].Email;
                page.txbAddress.Text = list_ec_tb_user[index].Address;
                page.txbQuestion.Text = list_ec_tb_user[index].Question;
                page.txbAnswer.Text = list_ec_tb_user[index].Answer;

                page.btnedit_delegate += btnEdit_Click_Delegate;
                page.ShowDialog();
            }
        }

        //btnEdit_Click_Delegate
        private void btnEdit_Click_Delegate()
        {
            GetUser();
        }

        //GetUser
        private DataTable tb_user = new DataTable();
        private void GetUser()
        {
            if (thread_content != null && thread_content.ThreadState == ThreadState.Running) { }
            else
            {
                thread_content = new Thread(() =>
                {
                    try
                    {
                        this.grContent.Dispatcher.Invoke((Action)(() => 
                        {
                            dtgUser.ItemsSource = null;
                            this.dtgUser.Visibility = System.Windows.Visibility.Hidden; 
                        }));

                        this.mpr.Dispatcher.Invoke((Action)(() => 
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Visible;
                            this.mpr.IsActive = true; 
                        }));

                        tb_user.Clear();
                        list_ec_tb_user.Clear();

                        tb_user = bus_tb_user.GetUser("");
                        foreach (DataRow datarow in tb_user.Rows)
                        {
                            EC_tb_User ec_tb_user = new EC_tb_User();
                            ec_tb_user.ID = Convert.ToInt32(datarow["ID"].ToString());
                            ec_tb_user.Name = datarow["Name"].ToString();
                            ec_tb_user.Email = datarow["Email"].ToString();
                            ec_tb_user.Address = datarow["Address"].ToString();
                            ec_tb_user.Password = datarow["Password"].ToString();
                            ec_tb_user.Question = datarow["Question"].ToString();
                            ec_tb_user.Answer = datarow["Answer"].ToString();
                            ec_tb_user.EditImage = @"pack://application:,,,/Resources/edit.png";

                            list_ec_tb_user.Add(ec_tb_user);
                        }

                        this.tblTotal.Dispatcher.Invoke((Action)(() => { tblTotal.Text =  FindResource("total").ToString() + "(" + list_ec_tb_user.Count.ToString() + ")"; }));
                        this.dtgUser.Dispatcher.Invoke((Action)(() => { dtgUser.ItemsSource = list_ec_tb_user; }));

                        Thread.Sleep(500);
                        this.mpr.Dispatcher.Invoke((Action)(() =>
                        {
                            this.mpr.Visibility = System.Windows.Visibility.Hidden;
                            this.mpr.IsActive = false;
                        }));
                        this.grContent.Dispatcher.Invoke((Action)(() => { this.dtgUser.Visibility = System.Windows.Visibility.Visible; }));
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ModernDialog md = new ModernDialog();
                            md.CloseButton.Content = FindResource("close").ToString();
                            md.Title = FindResource("notification").ToString();
                            md.Content = ex.Message;
                            md.ShowDialog();
                        }));
                    }
                });
                thread_content.Start();
            }
        }

    }
}
