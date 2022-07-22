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
    /// Logique d'interaction pour ODBC__Config.xaml
    /// </summary>
    public partial class ODBC__Config : Window
    {
        public static BrushConverter errorCol;
        public static Brush brush;
        public ODBC__Config()
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

        private void btnRun_odbc_Click(object sender, RoutedEventArgs e)
        {
            if (DS_Name_odbc.Text=="" || serverName_odbc.Text ==""
                || comboDB_odbc.SelectedItem==null)
            {
                errorLabel_odbc.Text = "All fields are required !\nPlease check your input before running the script \nThank you";
                errorLabel_odbc.FontSize = 15;
                DS_Name_odbc.BorderBrush = brush;
                serverName_odbc.BorderBrush = brush;
                error_combodb_odbc.Foreground = brush;

            }

        }
    }
}
