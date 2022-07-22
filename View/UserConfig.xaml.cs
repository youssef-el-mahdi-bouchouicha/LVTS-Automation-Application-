using Automation_LVTS.Model;
using Automation_LVTS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Automation_LVTS.View
{
    /// <summary>
    /// Logique d'interaction pour UserConfig.xaml
    /// </summary>
    public partial class UserConfig : Window
    {
        User us = new User();
        UserService uss = new UserService();
        public static BrushConverter errorCol;
        public static Brush brush;
        public UserConfig()
        {
            InitializeComponent();
            foreach (var item in uss.GetAllDBs("."))
            {
                comboDB_uc.Items.Add(item);
            }
            errorCol = new BrushConverter();
            brush = (Brush)errorCol.ConvertFrom("#FFDA5353");
            
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRun_uc_Click(object sender, RoutedEventArgs e)
        {
            
            if (login_name_uc.Text=="" || Username_uc.Text == "" || ServerName_uc.Text == ""
                || comboDB_uc.SelectedItem== null || domainUN_uc.Text == "")
            {
                errorLabel_uc.Text = "All fields are required \nPlease check your input before running the script !\nThank you !";
                errorLabel_uc.FontSize =15;
                login_name_uc.BorderBrush = brush;
                Username_uc.BorderBrush = brush;
                ServerName_uc.BorderBrush = brush;
                domainUN_uc.BorderBrush = brush;
                error_combod_uc.Foreground = brush;
            }
            else
            {
                User user = new User(login_name_uc.Text, Username_uc.Text, domainUN_uc.Text, int.Parse(Current_User_uc.Text));
                us = user;
                if(uss.AddUser_TO_DB(comboDB_uc.SelectedItem.ToString(), ServerName_uc.Text, user) == true)
                {
                    errorLabel_uc.Foreground = Brushes.Blue;
                    errorLabel_uc.Text = "INFO :  - User created successfuly ! ";
                    MessageBox.Show("User : " + Username_uc.Text + " is created successfully  ", "Create New User", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    errorLabel_uc.Foreground = brush;
                    errorLabel_uc.Text = "Warning : - Please check the Log File ";
                    MessageBox.Show("User: " + Username_uc.Text + " is not created !", "MyProgram", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }

       
    }
}
