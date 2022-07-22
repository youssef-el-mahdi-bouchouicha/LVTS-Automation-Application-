
using Automation_LVTS.Model;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automation_LVTS.Service
{
    public class ConfigFeaturesService
    {
        ConfigFeatures cf = new ConfigFeatures();

        public string LogFolderCreation()
        {
            string folderName = @"C:\Automation LVTS 2022\Automation LogFolder";
            // If directory does not exist, create it
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
                Console.WriteLine(Directory.GetDirectoryRoot(folderName));
                return folderName;
            }
            else
            {
                Console.WriteLine(Directory.GetDirectoryRoot(folderName)); ;
                return folderName;
            }

        }
        //fct for a seccess loading script  story   
        public void LoggingSuccess_ScriptLoading(String s, String ss)
        {
            System.IO.File.WriteAllText(s, ss);
        }
        //fct for a failed loading script story
        public void LoggingError_ScriptLoading(String s, String ss)
        {
            System.IO.File.WriteAllText(s, ss);
        }
        public Boolean CreateDatabase(String dbName, String serverName, string filePath)
        {
            cf.Logpath = LogFolderCreation();
            cf.DbName = dbName;
            cf.ServerName = serverName;
            cf.FilePath = filePath;

            bool val;

            //var for execution time 
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //var for logging in console and in specific file 
            var log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            log.Information("Start Script");
            // start execution 
            watch.Start();


            SqlConnection myConn = new SqlConnection(@"Data Source=" + serverName + ";Initial Catalog=master;Integrated Security=True");

            string script = "create database " + dbName;

            SqlCommand myCommand = new SqlCommand(script, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                //MessageBox.Show("DataBase is Created Successfully", "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.LoggingSuccess_ScriptLoading(cf.Logpath + "/logSuccess_CreateDB" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                            + ".txt", "Date : " + System.DateTime.Now
                                            + "\nSuccess Message : DB named " + cf.DbName + " added successfully in "+cf.ServerName+"server :"
                                            + "\nScript Path : " + filePath
                                            + $"\nExecution Time: {watch.ElapsedMilliseconds} ms");

                log.Information("Success Message : Database created  ");
                val = true;

            }
            catch (System.Exception ex)
            {
                this.LoggingSuccess_ScriptLoading(cf.Logpath + "/logFailed_CreateDB" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                            + ".txt", "Date : " + System.DateTime.Now
                                            + "\nError Message : " + ex.Message
                                            + "\nScript Path : " + filePath
                                            + $"\nExecution Time: {watch.ElapsedMilliseconds} ms");

                log.Error(ex, "You should verify the SQL Script ");
                val = false;
                //MessageBox.Show(ex.ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

            //end of execution time calcul 
            watch.Stop();
            log.Information("end script");
            return val;

            //Console.WriteLine(script);
        }
        public bool GoScriptInstall(String dbName, String serverName, string filePath)
        {
            cf.Logpath = LogFolderCreation();
            cf.DbName = dbName;
            cf.ServerName = serverName;
            cf.FilePath = filePath;
            bool val = true;
            //var to save list of exceptions in sql script
            List<Exception> exceptions = new List<Exception>();

            //var for execution time
            var watch = System.Diagnostics.Stopwatch.StartNew();

            //var for logging in console and in specific file 
            var log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            log.Information("Start Script");

            // start execution 
            watch.Start();

            // string sqlConnectionString = @"Data Source=(localdb)\MSSQLLOCALDB;Initial Catalog=test;Integrated Security=True";
            string sqlConnectionString = @"Data Source=" + serverName + ";Initial Catalog=" + dbName + ";Integrated Security=True";

            string path = filePath;
            // string path = @"C:\Users\YEMBouchouicha\Downloads\longview patch D\db files\createDB.sql";

            //create file and script 
            FileInfo file = new FileInfo(path);
            string script = file.OpenText().ReadToEnd();

            //sql Connection
            SqlConnection conn = new SqlConnection(sqlConnectionString);

            //server connection 
            Server server = new Server(new ServerConnection(conn));

            // split script on GO command
            IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            // execution for mini_scripts 
            foreach (string commandString in commandStrings)
            {
                if (commandString.Trim() != "")
                {
                    try
                    {
                        //execution sql mini_script 
                        server.ConnectionContext.ExecuteNonQuery(commandString);
                    }
                    catch (Exception ex)
                    {
                        //add to list of exceptions 
                        exceptions.Add(ex);
                    }
                }
            }
            // Connection sing test 
            if (conn.State == ConnectionState.Open)
            {
                //force close connection !
                conn.Close();
            }

            // test for exception List + log function 
            if (exceptions.Count > 0)
            {
                //local var
                string listExErrors = "";
                foreach (Exception ex in exceptions)
                {
                    //string to use in logging function 
                    listExErrors += ex.StackTrace + "\n";
                }
                //fct declared in Services 
                this.LoggingError_ScriptLoading(cf.Logpath + "/logError_" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                                    + ".txt", "Date : " + System.DateTime.Now
                                                    + "\nError Message : " + listExErrors
                                                    + "\nException Stacktrace :  "
                                                    + "\nScript Path : " + filePath
                                                    + $"\nExecution Time: {watch.ElapsedMilliseconds} ms \n ");
                //console log 
                log.Warning("there is a prob in sql file loaded !");
                // fct var 
                val = false;
            }
            else
            {
                //fct declared in Services 
                this.LoggingSuccess_ScriptLoading(cf.Logpath + "/logSuccess_" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                                    + ".txt", "Date : " + System.DateTime.Now
                                                    + "\nSuccess Message : Script added successfully"
                                                    + "\nScript Path : " + filePath
                                                    + $"\nExecution Time: {watch.ElapsedMilliseconds} ms");
                //console log 
                log.Information("Success Message : Script added successfully \n");
                // fct var 
                val = true;

            }

            //end of execution time calcul 
            watch.Stop();
            //console log 
            log.Information("end script");

            return val;
        }

        public List<String> GetAllDB(string server_name)
        {
            List<String> list = new List<String>();
            // SqlConnection myConn = new SqlConnection(@"Data Source=" + ServerName + ";Initial Catalog=master;Integrated Security=True");
            // Open connection to the database
            string conString = @"Data Source=" + server_name + ";Integrated Security=True";

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
                        }
                    }
                }
            }
            return list;
        }



    }
}
