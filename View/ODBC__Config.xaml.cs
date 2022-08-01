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
    /// Logique d'interaction pour ODBC__Config.xaml
    /// </summary>
    public partial class ODBC__Config : Window
    {
        OdbcConfig oc = new OdbcConfig();
        OdbcConfigService ocs = new OdbcConfigService();
        public static BrushConverter errorCol;
        public static Brush brush;
        public ODBC__Config()
        {
            InitializeComponent();
            if (ocs.GetAllDBs() != null)
            {
                foreach (var item in ocs.GetAllDBs())
                {
                    comboDB_odbc.Items.Add(item);
                }
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

        private void btnRun_odbc_Click(object sender, RoutedEventArgs e)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            if (DS_Name_odbc.Text=="" || serverName_odbc.Text ==""
                || comboDB_odbc.SelectedItem==null)
            {
                errorLabel_odbc.Text = "All fields are required !\nPlease check your input before running the script \nThank you";
                errorLabel_odbc.FontSize = 15;
                DS_Name_odbc.BorderBrush = brush;
                serverName_odbc.BorderBrush = brush;
                error_combodb_odbc.Foreground = brush;

            }
            else
            {
                if (ocs.registryOdbc(DS_Name_odbc.Text, comboDB_odbc.SelectedItem.ToString()
                , serverName_odbc.Text, driver_odbc.Text, Encrypt_odbc.Text, int.Parse(SkipDMLInBatches_odbc.Text)
                , Trusted_Connection_odbc.Text, TrustServerCertificate_odbc.Text, tb_Type_odbc.Text))
                {
                    errorLabel_odbc.Foreground = Brushes.Blue;
                    errorLabel_odbc.Text = "INFO :  - DataSource created successfuly ! ";
                    MessageBox.Show("New ODBC Data Source  : " + DS_Name_odbc.Text + " Created successfully", "Create New ODBC Data Source", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    errorLabel_odbc.Foreground = brush;
                    errorLabel_odbc.Text = "Warning : - Please check the Log File ";
                    MessageBox.Show("The DataSource Name Exist ! Please choose another one ", "Create New ODBC Data Source", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            Mouse.OverrideCursor = Cursors.Arrow;


        }
    }
}
