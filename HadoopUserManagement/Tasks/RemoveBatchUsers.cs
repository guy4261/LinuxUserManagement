using System;
using System.Collections.Generic;
using System.IO;
using HadoopUserManagement.Commands;

namespace HadoopUserManagement
{
    /**
     * http://linux.die.net/man/8/newusers
     */
    class RemoveBatchUsers : Task
    {

        private char[] spaces = null;

        private ProtectedUsersFilter protectedUsersFilter =
            new ProtectedUsersFilter();

        public RemoveBatchUsers(Connection conn)
            : base(conn)
        {
            this.spaces = new char[] { '\n', '\r', ' ', '\t' };
        }

        /*
         * pw_name:pw_passwd:pw_uid:pw_gid:pw_gecos:pw_dir:pw_shell
         */
        public int RemoveUsersBatch(string filename, char delimiter = ',')
        {
            String[] lines = File.ReadAllLines(filename);
            var users = new List<string>();
            foreach (string line in lines)
            {
                var _line = line.Trim(spaces);
                if (_line.Length == 0)
                {
                    continue;
                }

                string[] cells = _line.Split(delimiter);
                string username = cells[0];
                users.Add(username);
            }

            users = protectedUsersFilter.RemoveProtectedUsers(users);
            if (users.Count == 0)
            {
                return -1;
            }

            string cmd = string.Format(@"for username in {0}
do
  hdfs dfs -rm -r /user/$username
  userdel $username
done
", string.Join<string>(" ", users.ToArray()));

            System.IO.File.WriteAllText("__cmd__.txt", cmd);
            var removeBatchUsers = new PlinkCommand("-m __cmd__.txt ");
            int ex = removeBatchUsers.execute(connection);
            return ex;
        }

    }
}
