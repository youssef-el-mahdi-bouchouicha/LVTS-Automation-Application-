using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Automation_LVTS.Service
{
    public class MTService : SharedConfig
    {
        string logpath;

        public int add_in_ServerGroup(string server_group_name, string serverName, string db)
        {
            //var for execution time
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // start execution 
            watch.Start();
            logpath = this.LogFolderCreation();
            int idSG = 0;
            string sqlConnectionString = @"Data Source=" + serverName + ";Initial Catalog=" + db + ";Integrated Security=True";
            var log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            String sql = "INSERT INTO [" + db + "].[dbo].[cfg_server_group]" +
                "([server_group_name]" +
                ",[queue_type]" +
                ",[is_streaming_data_enabled]" +
                ",[is_time_zone_updater_enabled]" +
                ",[use_crystal_templates]," +
                "[compliance_smtp_port]" +
                ",[is_local_printing_only]" +
                ",[BThreshold]" +
                ",[PThreshold]" +
                ",[use_compression]" +
                " ,[queue_reconnect_time]" +
                " ,[run_cmpl_on_order_load]" +
                " ,[stop_load_on_fail_cmpl]" +
                ",[pass_compliance_warnings]" +
                ",[engine_host_manager_port]" +
                " ,[use_port_sharing]" +
                " ,[gw_port]" +
                "  ,[use_3rd_party_load_balancer])" +
                " VALUES" +
                "   ('" + server_group_name + "', 0, 0, 0, 0, 25, 0, 3000, 3000, 1, 5, 0, 0, 0, 61791, 1, 61791, 0)";

            SqlConnection conn = new SqlConnection(sqlConnectionString);

            Server server = new Server(new ServerConnection(conn));
            try
            {
                server.ConnectionContext.ExecuteNonQuery(sql);

                log.Information("Success Message : Script installed successfully \nAdd to serverGroup");
                idSG = Get_Server_Group_ID(db, serverName, server_group_name);
                this.LoggingSuccess_ScriptLoading(logpath + "/logSuccess_ServerGroup" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                           + ".txt", "Date : " + System.DateTime.Now
                                           + "\nSuccess Message : serverGroup added  "
                                           + $"\nExecution Time: {watch.ElapsedMilliseconds} ms");
                watch.Stop();

            }
            catch (Exception e)
            {
                this.LoggingError_ScriptLoading(logpath + "/logSuccess_ServerGroup" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                           + ".txt", "Date : " + System.DateTime.Now
                                           + "\nError Message : can't add serverGroup"
                                           + "\n Error : " + e.Message+ " \n"+ e.StackTrace
                                           + $"\nExecution Time: {watch.ElapsedMilliseconds} ms");


                log.Error(e, "there is a prob in sql !");

            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            if (idSG == 0)
                return 0;
            else
                return idSG;
           
        }

        public int Get_Server_Group_ID(string db_config_name, string serverName, string serverGroup_Name)
        {
            int val = 0;
            using (SqlConnection conn = new SqlConnection(@"Data Source=" + serverName + ";Initial Catalog=master;Integrated Security=True"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("select server_group_id from  [" + db_config_name + "].[dbo].[cfg_server_group] where server_group_name = '" + serverGroup_Name + "'", conn))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine(dr[0].ToString());
                            val = int.Parse(dr[0].ToString());
                        }
                    }
                }
                conn.Close();
            }
            return val;
        }

        public bool Add_in_ServerMachine(string server_group_name, string serverName, string db)
        {
            int idSG = Get_Server_Group_ID(db, serverName, server_group_name);
            string sqlConnectionString = @"Data Source=" + serverName + ";Initial Catalog=master;Integrated Security=True";
            var log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            string sql = "INSERT INTO [" + db + "].[dbo].[cfg_server_machine]" +
            "([server_group_id]" +
            ",[server_name]" +
            ",[dns_or_ip]" +
            " ,[is_gateway]" +
            " ,[is_mt_server]" +
            " ,[is_ioi_server]" +
            " ,[is_price_queue_host]" +
            "  ,[dotnet_url]" +
            "  ,[is_load_balancer_Server])" +
            "  VALUES " +
            "  ('" + idSG + "', '" + serverName + "','" + serverName + "', 0, 1, 0, 0,'net.tcp://" + serverName + "',0)";

            SqlConnection conn = new SqlConnection(sqlConnectionString);

            Server server = new Server(new ServerConnection(conn));
            try
            {
                server.ConnectionContext.ExecuteNonQuery(sql);

                log.Information("Success Message : Script installed successfully \nAdd to  ServerMachine");

                this.LoggingSuccess_ScriptLoading(logpath + "/logSuccess_ServerMachine" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                           + ".txt", "Date : " + System.DateTime.Now
                                           + "\nSuccess Message : serverMachine  added  ");
                return true;
            }
            catch (Exception e)
            {
                log.Error(e, "there is a prob in sql !");

                this.LoggingError_ScriptLoading(logpath + "/logSuccess_ServerMachine" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                           + ".txt", "Date : " + System.DateTime.Now
                                           + "\nError Message : can't add serverMachine"
                                           + "\n Error : " + e.Message + " \n" + e.StackTrace);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        public bool Add_in_Datasource(string server_group_name, string serverName, string db, string dataSource, int managmentPort)
        {
            string folderName = CreateRatFolder();
            string azmanPath = CreateAzmanFile(folderName);
            Console.WriteLine(azmanPath);
            int idSG = Get_Server_Group_ID(db, serverName, server_group_name);
            string sqlConnectionString = @"Data Source=" + serverName + ";Initial Catalog=master;Integrated Security=True";

            string sql = "INSERT INTO [" + db + "].[dbo].[cfg_datasource]" +
                " ([dsn_name]" +
                "  ,[username]" +
                " ,[password]" +
                " ,[server_group_id]  " +
                " ,[remote_profile_path]" +
                " ,[port]" +
                " ,[management_port]" +
                " ,[authorization_manager_store]" +
                " ,[async_alloc_poll_interval]" +
                " ,[async_alloc_fetch_num]" +
                " ,[disabled])" +
                " VALUES" +
                " ('" + dataSource + "','',''," + idSG + @",'\\" + serverName + @"\C:\Automation LVTS 2022\" + folderName + "',61791," + managmentPort + @",'msxml://" + azmanPath + "',5,5,0)";
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            Server server = new Server(new ServerConnection(conn));
            try
            {
                server.ConnectionContext.ExecuteNonQuery(sql);
                Log.Information("Success Message : Script installed successfully \nAdd to dataSource");
                this.LoggingSuccess_ScriptLoading(logpath + "/logSuccess_DataSource" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                           + ".txt", "Date : " + System.DateTime.Now
                                           + "\nSuccess Message : DataSource MT  added  ");
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "there is a prob in sql !");
                this.LoggingError_ScriptLoading(logpath + "/logSuccess_mt_DataSource" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                           + ".txt", "Date : " + System.DateTime.Now
                                           + "\nError Message : can't add Datasource to MT"
                                           + "\n Error : " + e.Message + " \n" + e.StackTrace);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }
        public bool Run_MTconfig(string server_group_name, string serverName, string db, string dataSource, int managmentPort)
        {
            bool val = false;
            int f1 = 0;
            f1 = add_in_ServerGroup(server_group_name, serverName, db);
            bool f2 = Add_in_ServerMachine(server_group_name, serverName, db);
            bool f3 = Add_in_Datasource(server_group_name, serverName, db, dataSource, managmentPort);

            if (f1 != 0 && f2 == true && f3 == true)
            {
                Console.WriteLine("jawi behy");
                return true;
            }
            else if (f1 == 0)
            {
                Console.WriteLine("f1 fih mochkla");
                return false;
            }
            else if (f2 == false)
            {
                Console.WriteLine("f2 fiha mochkla");
                return false;
            }
            else if (f3 == false)
            {
                Console.WriteLine("f3");
                return false;
            }

            // lezem tetbadel !!!!!!!!
            return val;
        }

        public string CreateRatFolder()
        {
            string folderName = "RAT_LongView";
            // If directory does not exist, create it
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(@"C:\Automation LVTS 2022\" + folderName);
                Console.WriteLine(Directory.GetDirectoryRoot(@"C:\Automation LVTS 2022\" + folderName));

                return folderName;
            }
            else
            {
                Console.WriteLine(Directory.GetDirectoryRoot(@"C:\Automation LVTS 2022\" + folderName)); ;
                return folderName;
            }

        }

        public string CreateAzmanFile(string foldername)
        {
            if (foldername == null)
            {
                string path = @"C:\Automation LVTS 2022\Azman.xml";
                Console.WriteLine("The Azman File Exist in " + path);
                return path;

            }
            else
            {
                string path = @"C:\Automation LVTS 2022\" + foldername + @"\Azman.xml";
                File.Copy(@"C:\Automation LVTS 2022\Azman.xml", path);
                return path;
            }
        }

        public bool Add_License_Keys(string pathfile, string mktDB, string serverName)
        {
            string sqlCommand = "";
            List<LinedataServicesLicenseInformationProductKeyTouple> list = new List<LinedataServicesLicenseInformationProductKeyTouple>();
            LinedataServicesLicenseInformation linedataServicesLicenseInformation = new LinedataServicesLicenseInformation();
            using (TextReader reader = new StreamReader(@"C:\Users\YEMBouchouicha\Downloads\QA_licenses_012714_with_SDK.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LinedataServicesLicenseInformation));
                linedataServicesLicenseInformation = serializer.Deserialize(reader) as LinedataServicesLicenseInformation;
            }

            string sqlConnectionString = @"Data Source=" + serverName + ";Initial Catalog=" + mktDB + ";Integrated Security=True";
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            Server server = new Server(new ServerConnection(conn));

            foreach (LinedataServicesLicenseInformationProductKeyTouple item in linedataServicesLicenseInformation.ProductKeyCollection)
            {
                sqlCommand = "INSERT INTO  [ymb_81].[dbo].[registry]" +
                   "([section],[entry],[value])" +
                   "VALUES" +
                   "('License', '" + item.Product + "', '" + item.Key + "')";
                //Console.WriteLine(item.Product +" : " +item.Key);
                try
                {
                    server.ConnectionContext.ExecuteNonQuery(sqlCommand);
                    Console.WriteLine(item.Product + " : " + item.Key);
                }
                catch (Exception )
                {
                    
                    Console.WriteLine("tzedouuuuuch ");
                }
            }
            sqlCommand = "INSERT INTO  [ymb_81].[dbo].[registry]" +
                   "([section],[entry],[value])" +
                   "VALUES" +
                   "('License', 'Client String', '" + linedataServicesLicenseInformation.ClientString + "')";
            try
            {
                server.ConnectionContext.ExecuteNonQuery(sqlCommand);

                this.LoggingSuccess_ScriptLoading(logpath + "/logSuccess_LicenseKeys" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                       + ".txt", "Date : " + System.DateTime.Now
                                       + "\nSuccess Message : License Keys added  ");
                Console.WriteLine("Client String :" + linedataServicesLicenseInformation.ClientString);
            }
            catch (Exception ex)
            {
                this.LoggingError_ScriptLoading(logpath + "/logSuccess_mt_LicenseKeys" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                          + ".txt", "Date : " + System.DateTime.Now
                                          + "\nError Message : can't add License Keys to MT"
                                          + "\n Error : " + ex.Message + " \n" + ex.StackTrace);
                Console.WriteLine("tzedouuuuuchhhhh ");
            }

            return true;
        }



        //public List<String> GetAllDB(string server_name)
        //{
        //    List<String> list = new List<String>();
        //    // SqlConnection myConn = new SqlConnection(@"Data Source=" + ServerName + ";Initial Catalog=master;Integrated Security=True");
        //    // Open connection to the database
        //    string conString = @"Data Source=" + server_name + ";Integrated Security=True";

        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        con.Open();

        //        // Set up a command with the given query and associate
        //        // this with the current connection.
        //        using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
        //        {
        //            using (IDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    list.Add(dr[0].ToString());
        //                }
        //            }
        //        }
        //    }
        //    return list;
        //}



    }
}
