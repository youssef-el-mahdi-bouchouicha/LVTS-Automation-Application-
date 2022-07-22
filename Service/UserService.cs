using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = Automation_LVTS.Model.User;

namespace Automation_LVTS.Service
{
    public class UserService
    {
        public List<string> GetAllDBs(string serverName)
        {
            List<String> list = new List<String>();

            // SqlConnection myConn = new SqlConnection(@"Data Source=" + ServerName + ";Initial Catalog=master;Integrated Security=True");
            // Open connection to the database
            string conString = @"Data Source=" + serverName + ";Integrated Security=True";

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                // Set up a command with the given query and associate
                // this with the current connection.
                using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(dr[0].ToString());
                            Console.WriteLine(dr[0].ToString());
                        }
                    }
                }

            }
            return null;
        }
        public bool AddUser_TO_DB(string db, string servern,User u)
        {
            bool val;
            //var for execution time
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //var for logging in console and in specific file 
            var log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            log.Information("Start Script");
            // start execution 
            watch.Start();

            string logPath = @"C:\Users\YEMBouchouicha\Downloads\longview patch D\db files\longview log files";


            // string sqlConnectionString = @"Data Source=(localdb)\MSSQLLOCALDB;Initial Catalog=test;Integrated Security=True";
            string sqlConnectionString = @"Data Source=" + servern + ";Initial Catalog=" + db + ";Integrated Security=True";

            // string path = filePath;
            // string path = @"C:\Users\YEMBouchouicha\Downloads\longview patch D\db files\createDB.sql";

            // FileInfo file = new FileInfo(path);

            // string script = File.ReadAllText(path);
            string script = "USE[" + db + "]"
                + "\nGO"

                + "\nDECLARE @return_value int, @user_id numeric(10, 0)"

                + "\nEXEC @return_value = [dbo].[add_user] @user_id = @user_id OUTPUT,@login_name = N'" + u.Login_Name + "',	@name = N'" + u.Name + "',@domain_username = N'" + u.Domain_Username + "',@current_user =" + u.Current_User

                + "\nSELECT @user_id as N'@user_id'"

                + "\nSELECT  'Return Value' = @return_value "

                + "\nGO";

            SqlConnection conn = new SqlConnection(sqlConnectionString);

            Server server = new Server(new ServerConnection(conn));

            try
            {
                server.ConnectionContext.ExecuteNonQuery(script);

                log.Information("Success Message : Script added successfully \n");
                val = true;
            }
            catch (Exception e)
            {
                log.Error(e, "there is a prob in sql file loaded !");
                val = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            //end of execution time calcul 
            watch.Stop();
            log.Information("end script");
            return true;
        }
    }
}
