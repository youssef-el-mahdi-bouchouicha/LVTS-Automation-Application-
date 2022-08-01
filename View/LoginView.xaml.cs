using Automation_LVTS.Service;
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
            Mouse.OverrideCursor = Cursors.Wait;

            SharedConfig sc = new SharedConfig();
            sc.Servername = serverName.Text;
            //System.Security.Principal.WindowsIdentity.GetCurrent().Name
            if ( login_username.Text == System.Security.Principal.WindowsIdentity.GetCurrent().Name && login_pwd.Password == "admin" && sc.GetAllDBs()!=null)
            {
                Home_window home = new Home_window();
                
                home.Show();
                this.Hide();
            }
            else
            {
                errorLabel_login.Text = "Login, password or server name is invalid !";
                errorLabel_login.FontSize = 12;
                Console.WriteLine(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            }

            Mouse.OverrideCursor = Cursors.Arrow;


        }
    }
}
