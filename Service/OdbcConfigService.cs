using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_LVTS.Service
{
    public class OdbcConfigService: SharedConfig
    {
        public bool registryOdbc(String name, string dataBase, string server, string driver, string encrypt
           , int skipDMLInBatches, string trusted_Connection, string trustServerCertificate
           , string type)
        {
            //var for execution time
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // start execution 
            watch.Start();
            string logPath = this.LogFolderCreation();
            bool test = false;
            RegistryKey lvts_auto = Registry.LocalMachine;
            lvts_auto = lvts_auto.OpenSubKey(@"SOFTWARE\ODBC\ODBC.INI");
            string[] list = lvts_auto.GetSubKeyNames();
            foreach (string s in list)
            {
                if (s == name)
                {
                    test = true;
                }
            }
            if (test == false)
            {
                Console.WriteLine(" __________ after add __________");
                lvts_auto = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\ODBC\ODBC.INI\" + name);
                string[] list3 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBC.INI").GetSubKeyNames();
                foreach (string s in list3)
                {
                    Console.WriteLine(s);
                }
                //value to sub key
                lvts_auto.SetValue("Database", dataBase);
                lvts_auto.SetValue("Driver", driver);
                lvts_auto.SetValue("Encrypt", encrypt);
                lvts_auto.SetValue("Server", server);
                lvts_auto.SetValue("SkipDMLInBatches", skipDMLInBatches);
                lvts_auto.SetValue("Trusted_Connection", trusted_Connection);
                lvts_auto.SetValue("TrustServerCertificate", trustServerCertificate);
                //opt
                lvts_auto.SetValue("ClientCertificate", "");
                lvts_auto.SetValue("KeystoreAuthentication", "");
                lvts_auto.SetValue("KeystoreLocation", "");
                lvts_auto.SetValue("KeystorePrincipalId", "");
                lvts_auto.SetValue("KeystoreSecret", "");

                //key to  odbc datasources
                RegistryKey lvts = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources");
                lvts.SetValue(name, type);


                this.LoggingSuccess_ScriptLoading(logPath + "/ODBC_logSuccess_" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                     + ".txt", "Date : " + System.DateTime.Now
                                     + "\nMessage : Datasource " + name + " is matched with database named  " + dataBase + " and server " + server + " ."

                                     + $"\nExecution Time: {watch.ElapsedMilliseconds} ms");

                watch.Stop();
                return true;

            }
            else
            {
                
                this.LoggingError_ScriptLoading(logPath + "/ODBC_logError_" + System.DateTime.Now.ToString("yyyy'-'MM'-'dd'__T__'HH'h__'mm'min_'ss")
                                      + ".txt", "Date : " + System.DateTime.Now
                                      + "\nMessage : Datasource " + name + " is not matched with database named  " + dataBase + " and server " + server + " ."
                                     
                                      + $"\nExecution Time: {watch.ElapsedMilliseconds} ms");

                return false;
            }

            //end of execution time calcul 
           



        }
    }
}
