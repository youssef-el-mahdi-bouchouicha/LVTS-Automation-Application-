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
    /// Logique d'interaction pour DataBaseConfig.xaml
    /// </summary>
    public partial class DataBaseConfig : Window
    {
        ConfigFeatures cf = new ConfigFeatures();
        ConfigFeaturesService cfs = new ConfigFeaturesService();
        public static BrushConverter errorCol;
        public static Brush brush;
        public int isopen { get; set; }
        public DataBaseConfig()
        {

            InitializeComponent();
            errorCol = new BrushConverter();
            brush = (Brush)errorCol.ConvertFrom("#FFDA5353");
            isopen = 0;
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
            isopen = 0;
            this.Close();
        }

        private void btnCreateDB_dbc_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            bool same=false;
            if (serverName_dbc.Text == "" || dbName_dbc.Text == "")
            {
                errorLabel_dbc.Text = "ServerName field and Database are required to create a new database ";
                errorLabel_dbc.FontSize = 15;
                serverName_dbc.BorderBrush = brush;
                dbName_dbc.BorderBrush = brush;
                filepath_dbc.BorderBrush = Brushes.DarkGray;

            }
            else
            {
                foreach (string item in cfs.GetAllDB(serverName_dbc.Text))
                {
                    if (item == dbName_dbc.Text)
                    {
                        same = true;
                    }
                }
                if (same == true)
                {
                    errorLabel_dbc.Foreground = brush;
                    errorLabel_dbc.Text = "ERROR : - Used DataBase name \nPlease choose another name ";
                }
                else
                {
                    
                    cf.val = cfs.CreateDatabase(dbName_dbc.Text, serverName_dbc.Text, "");
                    
                    if (cf.val)
                    {
                        errorLabel_dbc.Foreground = Brushes.White;
                        errorLabel_dbc.FontSize=15; 
                        errorLabel_dbc.Text = "INFO :\n- DataBase is Created Successfully ";
                        MessageBox.Show("DataBase is Created Successfully", "MyProgram", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("DataBase is NOT Created ", "MyProgram", MessageBoxButton.OK, MessageBoxImage.Error);
                        errorLabel_dbc.Foreground = brush;
                        errorLabel_dbc.FontSize = 15;
                        errorLabel_dbc.Text = "ERROR :\n- Please check the Log File ";
                    }
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        private void btnRunScript_dbc_Click(object sender, RoutedEventArgs e)
        {
            if (serverName_dbc.Text == "" || dbName_dbc.Text == "" || filepath_dbc.Text == "")
            {
                errorLabel_dbc.Text = "ServerName, Database and file path are required \nPlease check this fields before reunning script. \nThank you !";
                errorLabel_dbc.FontSize = 15;
                serverName_dbc.BorderBrush = brush;
                dbName_dbc.BorderBrush = brush;
                filepath_dbc.BorderBrush = brush;
            }
            else
            {
                string filepath = cf.FilePath.ToString();
                cf.val = cfs.GoScriptInstall(cf.DbName, cf.ServerName, filepath);
                if (cf.val)
                {
                    errorLabel_dbc.Foreground = Brushes.Blue;
                    errorLabel_dbc.Text = "INFO :  - Script installed Successfully  ";
                    MessageBox.Show("Script installed Successfully", "MyProgram", MessageBoxButton.OK, MessageBoxImage.Information);

                }

                else
                {
                    errorLabel_dbc.Foreground = brush;
                    errorLabel_dbc.Text = "Warning : - Please check the Log File ";
                    MessageBox.Show("Script installed with errors ! ", "MyProgram", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
            }
        }

        private void btnChooseF_dbc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (serverName_dbc.Text == "" || dbName_dbc.Text == "")
            {
                errorLabel_dbc.Text = "ServerName field and Database are required then you will be able to choose your script !";
                errorLabel_dbc.FontSize = 15;
                serverName_dbc.BorderBrush = brush;
                dbName_dbc.BorderBrush = brush;
                filepath_dbc.BorderBrush = Brushes.DarkGray;

            }
            else
            {
                if (dialog.ShowDialog() == true)
                {
                    cf.DbName = dbName_dbc.Text;
                    cf.ServerName = serverName_dbc.Text;
                    //cf.ServerName = @"(localdb)\MSSQLLOCALDB";
                    // var sr = new StreamReader(dialog.FileName);
                    filepath_dbc.Text = dialog.FileName;
                    cf.FilePath = dialog.FileName;
                    Console.WriteLine(System.IO.Path.GetFullPath(cf.FilePath));
                    //Console.WriteLine(cf.GoScriptInstall(cf.DbName,cf.ServerName, cf.FilePath));

                    if (filepath_dbc.Text == "")
                    {
                        MessageBox.Show("Please select your SQL file ", "My Automation Program", MessageBoxButton.OK, MessageBoxImage.Warning);

                     }
                    else
                    {
                        MessageBox.Show("Your File :\n " + cf.FilePath + "\n is selected Successfully  ", "MyProgram", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
            }

        }

        
    }
}
