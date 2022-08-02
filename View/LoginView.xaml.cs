using Automation_LVTS.Service;
using System;
using System.Collections.Generic;
using System.IO;
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
        SharedConfig sc = new SharedConfig();
        public LoginView()
        {
            InitializeComponent();
             Console.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory())+ @"\RequiredFiles\Azman.xml");
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

            //System.Security.Principal.WindowsIdentity.GetCurrent().Name
            if ( login_username.Text != System.Security.Principal.WindowsIdentity.GetCurrent().Name || login_pwd.Password != "admin" || sc.GetAllDBs(serverName.Text)==null)
            {
                errorLabel_login.Text = "Login, password or server name is invalid !";
                errorLabel_login.FontSize = 12;
                Console.WriteLine(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            }
            else
            {
                
                //Console.WriteLine(sc.GetAllDBs());
                Home_window home = new Home_window();
                home.Servername = serverName.Text;
                home.Show();
                this.Hide();
            }

            Mouse.OverrideCursor = Cursors.Arrow;


        }
    }
}
