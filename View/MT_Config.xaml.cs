using Automation_LVTS.Model;
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
    /// Logique d'interaction pour MT_Config.xaml
    /// </summary>
    public partial class MT_Config : Window
    {

        MTConfig mtc = new MTConfig();
        MTService mts = new MTService();

        public static BrushConverter errorCol;
        public static Brush brush;
        
        public MT_Config()
        {
            InitializeComponent();
            errorCol = new BrushConverter();
            brush = (Brush) errorCol.ConvertFrom("#FFDA5353");
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

        private void btnRun_mt_Click(object sender, RoutedEventArgs e)
        {
            if (serverName_mt.Text == "" || serverGroupName_mt.Text == ""
                || comboDB_mt.SelectedItem == null || comboDS_mt.SelectedItem == null
                || managmentPort_mt.Text == "")
            {
                errorLabel.Text = "All fields are required !\nPlease check your input before running the script \nThank you";
                errorLabel.FontSize = 15;
                serverGroupName_mt.BorderBrush = brush;
                serverName_mt.BorderBrush = brush;
                managmentPort_mt.BorderBrush = brush;
                filepath_mt.BorderBrush = brush;
                error_combodb_mt.Foreground = brush;
                error_combods_mt.Foreground = brush;
            }
            else
            {
                if (mts.Run_MTconfig(serverGroupName_mt.Text, serverName_mt.Text, comboDB_mt.SelectedItem.ToString(), comboDS_mt.SelectedItem.ToString(), int.Parse(managmentPort_mt.Text)) == true)
                {
                    errorLabel.Foreground = Brushes.Blue;
                    errorLabel.Text = "INFO :  - Middel Tier configured successfuly ! ";
                    MessageBox.Show("Middle tier : ", serverGroupName_mt.Text + "Is configured with no errors", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }
                else
                {
                    errorLabel.Foreground =brush;
                    errorLabel.Text = "INFO :  - Middel Tier is not configured ! ";
                    MessageBox.Show("Middle tier Configuration : ", serverGroupName_mt.Text + "Is not configured", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }

        
    }
}
