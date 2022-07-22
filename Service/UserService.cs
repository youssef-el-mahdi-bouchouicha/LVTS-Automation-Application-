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
    public class UserService : SharedConfig
    {

       
      
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

            string logPath = this.LogFolderCreation();


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
                this.LoggingSuccess_ScriptLoading(logPath + "/logSuccess_UserAdd" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                            + ".txt", "Date : " + System.DateTime.Now
                                            + "\nSuccess Message : user "+u.Name+" is added in database "+db+" and server "+servern+" ."
                                            + $"\nExecution Time: {watch.ElapsedMilliseconds} ms");
            }
            catch (Exception e)
            {
                log.Error(e, "there is a prob in sql  !");
                val = false;
                this.LoggingError_ScriptLoading(logPath + "/logError_UserAdd" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                            + ".txt", "Date : " + System.DateTime.Now
                                            + "\nMessage : user " + u.Name + " is not added in database " + db + " and server " + servern + " ."
                                            + "\nError : "+e.Message+" \n"+e.StackTrace
                                            + $"\nExecution Time: {watch.ElapsedMilliseconds} ms");
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
            return val;
        }


    }
}
