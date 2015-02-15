using HadoopUserManagement.Commands;

namespace HadoopUserManagement
{
    /**
     * http://linux.die.net/man/8/newusers
     */
    class RemoveSingleUser : Task
    {
        public RemoveSingleUser(Connection conn)
            : base(conn)
        {

        }
        /*
         * pw_name:pw_passwd:pw_uid:pw_gid:pw_gecos:pw_dir:pw_shell
         */
        public int remove_single_user(string username)
        {
            var conn = this.connection;
            var delUser = new PlinkCommand(
                string.Format("hdfs dfs -rm -r /user/{0};userdel {0}", username)
                );
            return delUser.execute(connection);
        }

    }
}
