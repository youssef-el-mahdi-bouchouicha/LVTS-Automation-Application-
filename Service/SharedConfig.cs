using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_LVTS.Service
{
    public class SharedConfig
    {
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

        public List<string> GetAllDBs(string s)
        {
            List<String> list = new List<String>();

            // SqlConnection myConn = new SqlConnection(@"Data Source=" + ServerName + ";Initial Catalog=master;Integrated Security=True");
            // Open connection to the database
            string conString = @"Data Source=" + s + ";Integrated Security=True";


            using (SqlConnection con = new SqlConnection(conString))
            {
                try
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
                                //Console.WriteLine(dr[0].ToString());
                            }
                        }
                    }


                    return list;
                }
                catch (Exception)
                {

                    return null;
                }



            }
        }   
        
    }
}
