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
        public static BrushConverter errorCol;
        public static Brush brush;
        public UserConfig()
        {
            InitializeComponent();
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
        }

       
    }
}
