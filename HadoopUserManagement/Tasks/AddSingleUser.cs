using HadoopUserManagement.Commands;

namespace HadoopUserManagement
{
    /**
     * http://linux.die.net/man/8/newusers
     */
    class AddSingleUser : Task
    {

        public AddSingleUser(Connection conn)
            : base(conn)
        {

        }

        /*
         * pw_name:pw_passwd:pw_uid:pw_gid:pw_gecos:pw_dir:pw_shell
         */
        public int addSingleUser(string username, string password, string group)
        {
            var conn = this.connection;
            string newusers_filename = "__users__.txt";
            string[] lines = { string.Format("{0}:{1}::{2}:::", username, password, group) };
            System.IO.File.WriteAllLines(newusers_filename, lines);

            var copyUserList = new PscpCommand(newusers_filename);
            int ex = copyUserList.execute(connection);
            if (ex != 0) { return ex; }

            var runNewUsers = new PlinkCommand(
                string.Format("newusers {0}/{1}", conn.Directory, newusers_filename));
            ex = runNewUsers.execute(connection);
            if (ex != 0) { return ex; }

            var add_hadoop = new PlinkCommand(
                string.Format("hdfs dfs -mkdir -p /user/{0};hdfs dfs -chown -R {0}:{1} /user/{0}",
                username, group));
            ex = add_hadoop.execute(connection);
            return ex;
        }

    }
}
