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

namespace CashierRegister.Pages.Setting
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : ModernDialog
    {
        //using for user
        private BUS_tb_User bus_tb_user = new BUS_tb_User();
        //delegate
        public delegate void btnEdit_Click_Delegate();
        public event btnEdit_Click_Delegate btnedit_delegate;

        //EditUser
        public EditUser()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] {};
            this.Title = "Edit user";
        }

        //btnOK_Click
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EC_tb_User ec_tb_user = new EC_tb_User();
                ec_tb_user.ID = Convert.ToInt32(tblID.Text.Trim().ToString());
                ec_tb_user.Name = StaticClass.GeneralClass.HandlingSpecialCharacter(tblName.Text.Trim().ToString());
                ec_tb_user.Email = StaticClass.GeneralClass.HandlingSpecialCharacter(txbEmail.Text.Trim().ToString());
                ec_tb_user.Address = StaticClass.GeneralClass.HandlingSpecialCharacter(txbAddress.Text.Trim().ToString());
                ec_tb_user.Question = StaticClass.GeneralClass.HandlingSpecialCharacter(txbQuestion.Text.Trim().ToString());
                ec_tb_user.Answer = StaticClass.GeneralClass.HandlingSpecialCharacter(txbAnswer.Text.Trim().ToString());

                if (pwbPassword.Password.Trim() == "" && pwbConfirmPassword.Password.Trim() != "")
                {
                    tblNotification.Text = FindResource("password_null").ToString();
                    pwbConfirmPassword.Focus();
                    return;
                }

                if (pwbPassword.Password != "" && pwbConfirmPassword.Password == "")
                {
                    tblNotification.Text = FindResource("confirm_password_null").ToString();
                    pwbPassword.Focus();
                    return;
                }

                if (pwbPassword.Password.Trim() != "" && pwbConfirmPassword.Password.Trim() != "")
                {
                    if (pwbPassword.Password != pwbConfirmPassword.Password)
                    {
                        tblNotification.Text = FindResource("new_password_confirm_password_incorrect").ToString();
                        pwbConfirmPassword.Focus();
                        return;
                    }
                    else
                    {
                        ec_tb_user.Password = StaticClass.GeneralClass.MD5Hash(pwbPassword.Password.Trim().ToString());
                        if (bus_tb_user.UpdateUser(ec_tb_user, StaticClass.GeneralClass.flag_database_type_general) == 1)
                        {
                            if (btnedit_delegate != null)
                            {
                                btnedit_delegate();
                                this.Close();
                            }
                        }
                        else
                            ec_tb_user.Password = "";
                    }
                }
                else
                {
                    if (bus_tb_user.UpdateUserNonPassword(ec_tb_user, StaticClass.GeneralClass.flag_database_type_general) == 1)
                    {
                        if (btnedit_delegate != null)
                        {
                            btnedit_delegate();
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tblNotification.Text = ex.Message;
            }
        }

        //btnCancel_Click
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
