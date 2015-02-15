using HadoopUserManagement.Commands;

namespace HadoopUserManagement
{
    class Deploy : Task
    {
        public Deploy(Connection conn)
            : base(conn)
        {

        }

        public void deploy()
        {
            var conn = this.connection;
            var cmd = new PscpCommand("add_user.sh");
            cmd.execute(connection);
        }
    }
}
