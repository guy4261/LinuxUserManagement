using System;
using System.Collections.Generic;
using System.IO;
using HadoopUserManagement.Commands;

namespace HadoopUserManagement
{
    class UserToAdd
    {
        public string Username;
        public string Password;
        public string Group;

        public override string ToString()
        {
            return string.Format("{0}:{1}::{2}:::",
                this.Username,
                this.Password,
                this.Group);
        }
    }

    /**
     * http://linux.die.net/man/8/newusers
     */
    class AddBatchUsers : Task
    {

        private char[] spaces = null;

        public AddBatchUsers(Connection conn)
            : base(conn)
        {
            this.spaces = new char[] { '\n', '\r', ' ', '\t' };
        }


        /*
         * pw_name:pw_passwd:pw_uid:pw_gid:pw_gecos:pw_dir:pw_shell
         */
        public void addUsersBatch(string filename, char delimiter = ',')
        {

            String[] lines = File.ReadAllLines(filename);
            var users = new List<UserToAdd>();
            foreach (string line in lines)
            {
                var _line = line.Trim(spaces);
                if (_line.Length == 0)
                {
                    continue;
                }

                string[] cells = _line.Split(delimiter);

                string username = cells[0];
                string password = cells[1];
                string group = cells[2];
                var u2a = new UserToAdd()
                {
                    Username = username,
                    Password = password,
                    Group = group
                };
                users.Add(u2a);
            }

            string newusers_filename = "__users__.txt";

            var userLines = new List<string>();
            var hadoopLines = new List<string>();


            foreach (var u in users)
            {
                userLines.Add(u.ToString());
                hadoopLines.Add(string.Format(
@"hdfs dfs -mkdir -p /user/{0}
hdfs dfs -chown -R {0}:{1} /user/{0}",
                                     u.Username, u.Group
                    ));
            }

            System.IO.File.WriteAllLines(newusers_filename, userLines);

            var copyUserList = new PscpCommand(newusers_filename);
            int ex = copyUserList.execute(connection);

            var runNewUsers = new PlinkCommand(
                string.Format("newusers {0}", newusers_filename));
            ex = runNewUsers.execute(connection);

            System.IO.File.WriteAllLines("__cmd__.txt", hadoopLines);
            var removeBatchUsers = new PlinkCommand("-m __cmd__.txt ");
            ex = removeBatchUsers.execute(connection);

        }

    }
}
