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
using System.Windows.Shapes;

namespace Automation_LVTS.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
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
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //System.Security.Principal.WindowsIdentity.GetCurrent().Name
            if ( login_username.Text == System.Security.Principal.WindowsIdentity.GetCurrent().Name && login_pwd.Password == "admin")
            {
                this.Hide();
                Home_window home = new Home_window();
                home.Show();
            }
            else
            {
                errorLabel_login.Text = "Check your login and password \nYou're not an administrator !";
                errorLabel_login.FontSize = 15;
                Console.WriteLine(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            }
           
            
        }
    }
}
