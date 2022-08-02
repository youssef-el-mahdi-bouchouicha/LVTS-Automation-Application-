using Automation_LVTS.Model;
using Automation_LVTS.Service;
using Microsoft.Win32;
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
        
        public MT_Config(string d )
        {
            InitializeComponent();
            
                foreach (var item in mts.GetAllDBs(d))
                {
                    comboDB_mt.Items.Add(item);
                }
            
            RegistryKey lvts_auto = Registry.LocalMachine;
            string[] list3 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBC.INI").GetSubKeyNames();
            foreach (string s in list3)
            {
                Console.WriteLine(s);
                comboDS_mt.Items.Add(s);
            }
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
            Mouse.OverrideCursor = Cursors.Wait;
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

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnChooseKF_mt_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                //cf.ServerName = @"(localdb)\MSSQLLOCALDB";
                // var sr = new StreamReader(dialog.FileName);
                filepath_mt.Text = dialog.FileName;
                Console.WriteLine(System.IO.Path.GetFullPath(filepath_mt.Text));
                //Console.WriteLine(cf.GoScriptInstall(cf.DbName,cf.ServerName, cf.FilePath));

                if (filepath_mt.Text == "")
                {
                    MessageBox.Show("Please select your License Key file ", "My Automation Program", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else
                {
                    MessageBox.Show("Your File :\n " + filepath_mt.Text + "\n is selected Successfully  ", "MyProgram", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnAddKeys_mt_Click(object sender, RoutedEventArgs e)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            if (filepath_mt.Text=="" || comboDB_mt.SelectedItem==null || serverName_mt.Text=="")
            {
                errorLabel.Foreground = brush;
                filepath_mt.BorderBrush = brush;
                serverName_mt.BorderBrush = brush;
                error_combodb_mt.Foreground= brush;
                errorLabel.Text = "INFO :  - FilePath is required ! \nPlease select your License Key file ! ";
            }
            else
            {
                mts.Add_License_Keys(filepath_mt.Text,comboDB_mt.SelectedItem.ToString(),serverName_mt.Text);
                if (mts.Run_MTconfig(serverGroupName_mt.Text, serverName_mt.Text, comboDB_mt.SelectedItem.ToString(), comboDS_mt.SelectedItem.ToString(), int.Parse(managmentPort_mt.Text)) == true)
                {
                    errorLabel.Foreground = Brushes.Blue;
                    errorLabel.Text = "INFO :  - license keys added successfuly ! ";
                    MessageBox.Show("Middle tier configuration : ","license keys added", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }
                else
                {
                    errorLabel.Foreground = brush;
                    errorLabel.Text = "Error :  - Can't add license keys  ";
                    MessageBox.Show("Middle tier Configuration : ", "Error while adding license keys", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
