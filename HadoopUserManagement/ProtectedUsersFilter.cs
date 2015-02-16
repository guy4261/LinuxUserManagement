using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadoopUserManagement
{
    public class ProtectedUsersFilter
    {

        private string[] protected_users = {
"abrt",
"adm",
"bin",
"cloudera-scm",
"daemon",
"dbus",
"flume",
"ftp",
"games",
"gopher",
"haldaemon",
"halt",
"hbase",
"hdfs",
"hive",
"httpfs",
"hue",
"impala",
"kms",
"llama",
"lp",
"mail",
"mapred",
"nfsnobody",
"nobody",
"ntp",
"oozie",
"operator",
"oprofile",
"postfix",
"postgres",
"rabbitmq",
"root",
"rpc",
"rpcuser",
"saslauth",
"sentry",
"shutdown",
"solr",
"spark",
"sqoop",
"sqoop2",
"sshd",
"sync",
"tcpdump",
"uucp",
"vcsa",
"yarn",
"zookeeper"                                       
};

        /**
         * Is the user in the protected users list.
         */
        public bool IsProtectedUser(string user)
        {
            return protected_users.Contains(user);
        }

        /**
         * Remove any protected users from a given list.
         */
        public List<string> RemoveProtectedUsers(List<string> users)
        {
            return users.Where(u => (!IsProtectedUser(u))).ToList<string>();
        }


    }
}
