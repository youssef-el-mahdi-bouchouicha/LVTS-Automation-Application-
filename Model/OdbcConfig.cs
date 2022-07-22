using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_LVTS.Model
{
    public class OdbcConfig
    {


        public string DataBase { get; set; }
        public string Server { get; set; }
        public string Driver { get; set; }
        public string Encrypt { get; set; }
        public int SkipDMLInBatches { get; set; }
        public string Trusted_Connection { get; set; }
        public string TrustServerCertificate { get; set; }


        public string ClientCertificate { get { return ClientCertificate; } set { ClientCertificate = ""; } }
        public string KeystoreAuthentication { get => KeystoreAuthentication; set => KeystoreAuthentication = ""; }
        public string KeystoreLocation { get => KeystoreLocation; set => KeystoreLocation = ""; }
        public string KeystorePrincipalId { get => KeystorePrincipalId; set => KeystorePrincipalId = ""; }
        public string KeystoreSecret { get => KeystoreSecret; set => KeystoreSecret = ""; }






    }

}
