using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_LVTS.Model
{
    public class User
    {
        public string Login_Name { get; set; }
        public string Name { get; set; }
        public string Domain_Username { get; set; }
        public int Current_User { get; set; }
        public string Db_Name { get; set; }

        public User(string login_Name, string name, string domain_Username, int current_User, string db_Name)
        {
            Login_Name = login_Name;
            Name = name;
            Domain_Username = domain_Username;
            Current_User = current_User;
            Db_Name = db_Name;
        }

        public User()
        {
        }

        public User(string login_Name, string name, string domain_Username, int current_User)
        {
            Login_Name = login_Name;
            Name = name;
            Domain_Username = domain_Username;
            Current_User = current_User;
        }







    }
}
