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
    /// Logique d'interaction pour Home_window.xaml
    /// </summary>
    public partial class Home_window : Window
    {
        UserConfig userConfig = new UserConfig();
        
        DataBaseConfig dbConfig = new DataBaseConfig();
       
        ODBC__Config odbcConfig = new ODBC__Config();

        MT_Config mtConfig = new MT_Config();
        public Home_window()
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

        private void btnDBConfig_Click(object sender, RoutedEventArgs e)
        {
            
                Thread newWindowThread = new Thread(new ThreadStart(() =>
                {

                // Create and show the Window
                    dbConfig = new DataBaseConfig();
                    dbConfig.Show();
                    

                // Start the Dispatcher Processing
                System.Windows.Threading.Dispatcher.Run();
                }));
                // Set the apartment state
                newWindowThread.SetApartmentState(ApartmentState.STA);
                // Make the thread a background thread
                newWindowThread.IsBackground = true;
                // Start the thread
                newWindowThread.Start();
          
            

            
        }

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            if (!userConfig.Activate())
            {
                userConfig = new UserConfig();
                userConfig.Show();
            }

        }

        private void btnODBCConfig_Click(object sender, RoutedEventArgs e)
        {
            if (!odbcConfig.Activate())
            {
                odbcConfig = new ODBC__Config();
                odbcConfig.Show();
            }

        }

        private void btnMtConfig_Click(object sender, RoutedEventArgs e)
        {
            if (!mtConfig.Activate())
            {
                mtConfig = new MT_Config();
                mtConfig.Show();
            }

        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginView login = new LoginView();
            login.Show();
        }
    }
}
